using Api.Adapters.Grpc.EnumMappers;
using Api.Adapters.Kafka;
using Application.Ports.Kafka;
using Application.Ports.Postgres;
using Application.UseCases.Commands.Model.AddModel;
using Confluent.Kafka;
using From.BookingKafkaEvents;
using From.RentKafkaEvents;
using From.VehicleDocumentsKafkaEvents;
using From.VehicleFleetKafkaEvents.Model;
using From.VehicleFleetKafkaEvents.Vehicle;
using Infrastructure.Adapters.Kafka;
using Infrastructure.Adapters.Postgres;
using Infrastructure.Adapters.Postgres.Inbox;
using Infrastructure.Adapters.Postgres.Outbox;
using Infrastructure.Adapters.Postgres.Repositories;
using Infrastructure.Adapters.Postgres.Saga;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Quartz;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;

namespace Api;

public static class ServiceCollectionExtensions
{
    private static readonly Configuration Configuration;

    static ServiceCollectionExtensions()
    {
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

        Configuration = environment switch
        {
            "Development" => new Configuration
            {
                ApplicationName = "Vehicle_fleet#" + Environment.MachineName,
                PostgresHost = Environment.GetEnvironmentVariable("POSTGRES_HOST") ?? "localhost",
                PostgresPort =
                    int.Parse(Environment.GetEnvironmentVariable("POSTGRES_PORT") ?? "5440"),
                PostgresDatabase = Environment.GetEnvironmentVariable("POSTGRES_DB") ??
                                   "vehiclefleet_db",
                PostgresUsername =
                    Environment.GetEnvironmentVariable("POSTGRES_USER") ?? "postgres",
                PostgresPassword = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD") ??
                                   "password",
                BootstrapServers = (Environment.GetEnvironmentVariable("BOOTSTRAP_SERVERS") ??
                                    "localhost:9092").Split("__"),
                MongoConnectionString =
                    Environment.GetEnvironmentVariable("MONGO_CONNECTION_STRING") ??
                    "mongodb://carsharing:password@localhost:27017/drivinglicense?authSource=admin",
                VehicleAddedTopic = Environment.GetEnvironmentVariable("VEHICLE_ADDED_TOPIC") ??
                                    "vehicle-added-topic",
                VehicleDeletedTopic = Environment.GetEnvironmentVariable("VEHICLE_DELETED_TOPIC") ??
                                      "vehicle-deleted-topic",
                BookingCreatedTopic = Environment.GetEnvironmentVariable("BOOKING_CREATED_TOPIC") ??
                                      "booking-created-topic",
                ModelCreatedTopic = Environment.GetEnvironmentVariable("MODEL_CREATED_TOPIC") ??
                                    "model-created-topic",
                ModelCategoryUpdatedTopic =
                    Environment.GetEnvironmentVariable("MODEL_CATEGORY_UPDATED_TOPIC") ??
                    "model-category-updated-topic",
                ModelTariffUpdatedTopic =
                    Environment.GetEnvironmentVariable("MODEL_TARIFF_UPDATED_TOPIC") ?? "model-tariff-updated-topic",
                VehicleOccupyingProcessedTopic =
                    Environment.GetEnvironmentVariable("VEHICLE_OCCUPYING_PROCESSED_TOPIC") ??
                    "vehicle-occupying-processed-topic",
                VehicleReadiedForReleasesTopic =
                    Environment.GetEnvironmentVariable("VEHICLE_READIED_FOR_RELEASE_TOPIC") ??
                    "vehicle-readied-for-releases-topic",
                VehicleReleasedTopic =
                    Environment.GetEnvironmentVariable("VEHICLE_RELEASED_TOPIC") ??
                    "vehicle-released-topic",
                VehicleServicedTopic =
                    Environment.GetEnvironmentVariable("VEHICLE_SERVICED_TOPIC") ??
                    "vehicle-serviced-topic",
                AddingToBookingProcessedTopic = Environment.GetEnvironmentVariable("VEHICLE_ADDING_TO_BOOKING_PROCESSED_TOPIC") ?? "vehicle-adding-to-booking-processed-topic",
                AddingToRentProcessedTopic = Environment.GetEnvironmentVariable("VEHICLE_ADDING_TO_RENT_PROCESSED_TOPIC") ??
                    "vehicle-adding-to-rent-processed-topic",
                DocumentAddingCompletedTopic = Environment.GetEnvironmentVariable("DOCUMENTS_ADDING_COMPLETED_TOPIC") ??
                                               "documents-adding-completed-topic",
            },
            "Production" => new Configuration
            {
                ApplicationName = "Vehicle_check#" + Environment.MachineName,
                PostgresHost = GetEnvironmentOrThrow("POSTGRES_HOST"),
                PostgresPort = int.Parse(GetEnvironmentOrThrow("POSTGRES_PORT")),
                PostgresDatabase = GetEnvironmentOrThrow("POSTGRES_DB"),
                PostgresUsername = GetEnvironmentOrThrow("POSTGRES_USER"),
                PostgresPassword = GetEnvironmentOrThrow("POSTGRES_PASSWORD"),
                BootstrapServers = GetEnvironmentOrThrow("BOOTSTRAP_SERVERS")
                    .Split("__"),
                BookingCreatedTopic = GetEnvironmentOrThrow("BOOKING_CREATED_TOPIC"),
                MongoConnectionString = GetEnvironmentOrThrow("MONGO_CONNECTION_STRING"),
                ModelCreatedTopic = GetEnvironmentOrThrow("MODEL_CREATED_TOPIC"),
                ModelCategoryUpdatedTopic = GetEnvironmentOrThrow("MODEL_CATEGORY_UPDATED_TOPIC"),
                ModelTariffUpdatedTopic = GetEnvironmentOrThrow("MODEL_TARIFF_UPDATED_TOPIC"),
                VehicleAddedTopic = GetEnvironmentOrThrow("VEHICLE_ADDED_TOPIC"),
                VehicleDeletedTopic = GetEnvironmentOrThrow("VEHICLE_DELETED_TOPIC"),
                VehicleOccupyingProcessedTopic =
                    GetEnvironmentOrThrow("VEHICLE_OCCUPYING_PROCESSED_TOPIC"),
                VehicleReadiedForReleasesTopic =
                    GetEnvironmentOrThrow("VEHICLE_READIED_FOR_RELEASE_TOPIC"),
                VehicleReleasedTopic = GetEnvironmentOrThrow("VEHICLE_RELEASED_TOPIC"),
                VehicleServicedTopic = GetEnvironmentOrThrow("VEHICLE_SERVICED_TOPIC"),
                AddingToBookingProcessedTopic = GetEnvironmentOrThrow("VEHICLE_ADDING_TO_BOOKING_PROCESSED_TOPIC"),
                AddingToRentProcessedTopic = GetEnvironmentOrThrow("VEHICLE_ADDING_TO_RENT_PROCESSED_TOPIC"),
                DocumentAddingCompletedTopic = GetEnvironmentOrThrow("DOCUMENTS_ADDING_COMPLETED_TOPIC")
            },
            _ => throw new ArgumentException("Unknown environment")
        };

        return;

        string GetEnvironmentOrThrow(string environmentName)
        {
            return Environment.GetEnvironmentVariable(environmentName) ??
                   throw new ArgumentNullException(environmentName,
                       "not exist in environment variables");
        }
    }

    public static IServiceCollection RegisterPostgresContextAndDataSource(
        this IServiceCollection services)
    {
        services.AddScoped<NpgsqlDataSource>(_ =>
        {
            var dataSourceBuilder = new NpgsqlDataSourceBuilder
            {
                ConnectionStringBuilder =
                {
                    ApplicationName = Configuration.ApplicationName,
                    Host = Configuration.PostgresHost,
                    Port = Configuration.PostgresPort,
                    Database = Configuration.PostgresDatabase,
                    Username = Configuration.PostgresUsername,
                    Password = Configuration.PostgresPassword,
                    BrowsableConnectionString = false
                }
            };

            return dataSourceBuilder.Build();
        });

        var serviceProvider = services.BuildServiceProvider();
        var dataSource = serviceProvider.GetRequiredService<NpgsqlDataSource>();

        services.AddDbContext<DataContext>(optionsBuilder =>
        {
            optionsBuilder.UseNpgsql(dataSource,
                npgsqlOptions => npgsqlOptions.MigrationsAssembly(typeof(DataContext).Assembly));
            optionsBuilder.EnableSensitiveDataLogging();
        });

        return services;
    }

    public static IServiceCollection RegisterInbox(this IServiceCollection services)
    {
        services.AddTransient<IInbox, Inbox>();

        return services;
    }

    public static IServiceCollection RegisterMediatorAndHandlers(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(AddModelHandler).Assembly));

        return services;
    }

    public static IServiceCollection RegisterSerilog(this IServiceCollection services)
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console(theme: AnsiConsoleTheme.Sixteen)
            .WriteTo.MongoDBBson(Configuration.MongoConnectionString,
                "logs",
                LogEventLevel.Verbose,
                50,
                TimeSpan.FromSeconds(10))
            .CreateLogger();
        services.AddSerilog();

        return services;
    }

    public static IServiceCollection RegisterRepositories(this IServiceCollection services)
    {
        services.AddTransient<IModelRepository, ModelRepository>();
        services.AddTransient<IVehicleRepository, VehicleRepository>();
        services.AddTransient<IVehicleAddingSagaRepository, VehicleAddingSagaRepository>();

        return services;
    }

    public static IServiceCollection RegisterUnitOfWork(this IServiceCollection services)
    {
        services.AddTransient<IUnitOfWork, UnitOfWork>();

        return services;
    }

    public static IServiceCollection RegisterMappers(this IServiceCollection services)
    {
        services.AddScoped<ColorMapper>();
        services.AddScoped<StatusMapper>();

        return services;
    }

    public static IServiceCollection RegisterMassTransit(this IServiceCollection services)
    {
        services.Configure<KafkaTopicsConfiguration>(config =>
        {
            config.ModelCreatedTopic = Configuration.ModelCreatedTopic;
            config.ModelCategoryUpdatedTopic = Configuration.ModelCategoryUpdatedTopic;
            config.ModelTariffUpdatedTopic = Configuration.ModelTariffUpdatedTopic;
            config.VehicleAddedTopic = Configuration.VehicleAddedTopic;
            config.VehicleDeletedTopic = Configuration.VehicleDeletedTopic;
            config.VehicleOccupyingProcessedTopic = Configuration.VehicleOccupyingProcessedTopic;
            config.VehicleReadiedForReleaseTopic = Configuration.VehicleReadiedForReleasesTopic;
            config.VehicleReleasedTopic = Configuration.VehicleReleasedTopic;
            config.VehicleServicedTopic = Configuration.VehicleServicedTopic;
        });

        services.AddTransient<IMessageBus, KafkaProducer>();

        services.AddMassTransit(x =>
        {
            x.UsingInMemory();

            x.AddRider(rider =>
            {
                rider.AddConsumer<BookingCreatedConsumer>();
                rider.AddConsumer<VehicleAddingToBookingProcessedConsumer>();
                rider.AddConsumer<VehicleAddingToRentProcessedConsumer>();
                rider.AddConsumer<VehicleDocumentAddedConsumer>();

                rider.AddProducer<string, ModelCreated>(Configuration.ModelCreatedTopic);
                rider.AddProducer<string, ModelCategoryUpdated>(Configuration.ModelCategoryUpdatedTopic);
                rider.AddProducer<string, ModelTariffUpdated>(Configuration.ModelTariffUpdatedTopic);

                rider.AddProducer<string, VehicleAdded>(Configuration.VehicleAddedTopic);
                rider.AddProducer<string, VehicleDeleted>(Configuration.VehicleDeletedTopic);
                rider.AddProducer<string, VehicleOccupyingProcessed>(Configuration.VehicleOccupyingProcessedTopic);
                rider.AddProducer<string, VehicleReadiedForRelease>(Configuration.VehicleReadiedForReleasesTopic);
                rider.AddProducer<string, VehicleReleased>(Configuration.VehicleReleasedTopic);
                rider.AddProducer<string, VehicleServiced>(Configuration.VehicleServicedTopic);

                rider.UsingKafka((context, k) =>
                {
                    k.TopicEndpoint<BookingCreated>(Configuration.BookingCreatedTopic,
                        "vehicle-fleet-consumer-group",
                        e =>
                        {
                            e.EnableAutoOffsetStore = false;
                            e.EnablePartitionEof = true;
                            e.AutoOffsetReset = AutoOffsetReset.Earliest;
                            e.CreateIfMissing();
                            e.UseKillSwitch(cfg =>
                                cfg.SetActivationThreshold(1)
                                    .SetRestartTimeout(TimeSpan.FromMinutes(1))
                                    .SetTripThreshold(0.05)
                                    .SetTrackingPeriod(TimeSpan.FromMinutes(1)));
                            e.UseMessageRetry(retry =>
                                retry.Interval(200, TimeSpan.FromSeconds(1)));
                            e.ConfigureConsumer<BookingCreatedConsumer>(context);
                        });
                    
                    k.TopicEndpoint<VehicleAddingToBookingProcessed>(Configuration.AddingToBookingProcessedTopic,
                        "vehicle-fleet-consumer-group",
                        e =>
                        {
                            e.EnableAutoOffsetStore = false;
                            e.EnablePartitionEof = true;
                            e.AutoOffsetReset = AutoOffsetReset.Earliest;
                            e.CreateIfMissing();
                            e.UseKillSwitch(cfg =>
                                cfg.SetActivationThreshold(1)
                                    .SetRestartTimeout(TimeSpan.FromMinutes(1))
                                    .SetTripThreshold(0.05)
                                    .SetTrackingPeriod(TimeSpan.FromMinutes(1)));
                            e.UseMessageRetry(retry =>
                                retry.Interval(200, TimeSpan.FromSeconds(1)));
                            e.ConfigureConsumer<VehicleAddingToBookingProcessedConsumer>(context);
                        });
                    
                    k.TopicEndpoint<VehicleAddingToRentProcessed>(Configuration.AddingToRentProcessedTopic,
                        "vehicle-fleet-consumer-group",
                        e =>
                        {
                            e.EnableAutoOffsetStore = false;
                            e.EnablePartitionEof = true;
                            e.AutoOffsetReset = AutoOffsetReset.Earliest;
                            e.CreateIfMissing();
                            e.UseKillSwitch(cfg =>
                                cfg.SetActivationThreshold(1)
                                    .SetRestartTimeout(TimeSpan.FromMinutes(1))
                                    .SetTripThreshold(0.05)
                                    .SetTrackingPeriod(TimeSpan.FromMinutes(1)));
                            e.UseMessageRetry(retry =>
                                retry.Interval(200, TimeSpan.FromSeconds(1)));
                            e.ConfigureConsumer<VehicleAddingToRentProcessedConsumer>(context);
                        });
                    
                    k.TopicEndpoint<DocumentAddingCompleted>(Configuration.DocumentAddingCompletedTopic,
                        "vehicle-fleet-consumer-group",
                        e =>
                        {
                            e.EnableAutoOffsetStore = false;
                            e.EnablePartitionEof = true;
                            e.AutoOffsetReset = AutoOffsetReset.Earliest;
                            e.CreateIfMissing();
                            e.UseKillSwitch(cfg =>
                                cfg.SetActivationThreshold(1)
                                    .SetRestartTimeout(TimeSpan.FromMinutes(1))
                                    .SetTripThreshold(0.05)
                                    .SetTrackingPeriod(TimeSpan.FromMinutes(1)));
                            e.UseMessageRetry(retry =>
                                retry.Interval(200, TimeSpan.FromSeconds(1)));
                            e.ConfigureConsumer<VehicleDocumentAddedConsumer>(context);
                        });

                    k.Host(Configuration.BootstrapServers);
                });
            });
        });

        return services;
    }

    public static IServiceCollection RegisterInboxAndOutboxBackgroundJobs(
        this IServiceCollection services)
    {
        services.AddQuartz(configure =>
        {
            var outboxJobKey = new JobKey(nameof(OutboxBackgroundJob));
            configure
                .AddJob<OutboxBackgroundJob>(j => j.WithIdentity(outboxJobKey))
                .AddTrigger(trigger => trigger.ForJob(outboxJobKey)
                    .WithSimpleSchedule(scheduleBuilder =>
                        scheduleBuilder.WithIntervalInSeconds(3).RepeatForever()));

            var inboxJobKey = new JobKey(nameof(InboxBackgroundJob));
            configure
                .AddJob<InboxBackgroundJob>(j => j.WithIdentity(inboxJobKey))
                .AddTrigger(trigger => trigger.ForJob(inboxJobKey)
                    .WithSimpleSchedule(scheduleBuilder =>
                        scheduleBuilder.WithIntervalInSeconds(3).RepeatForever()));
        });

        services.AddQuartzHostedService(options => options.WaitForJobsToComplete = true);

        return services;
    }

    public static IServiceCollection RegisterTelemetry(this IServiceCollection services)
    {
        services.AddOpenTelemetry()
            .WithMetrics(builder =>
            {
                builder.AddPrometheusExporter();

                builder.AddMeter("Microsoft.AspNetCore.Hosting",
                    "Microsoft.AspNetCore.Server.Kestrel");
                builder.AddView("http.server.request.duration",
                    new ExplicitBucketHistogramConfiguration
                    {
                        Boundaries =
                        [
                            0, 0.005, 0.01, 0.025, 0.05,
                            0.075, 0.1, 0.25, 0.5, 0.75, 1, 2.5, 5, 7.5, 10
                        ]
                    });
            })
            .WithTracing(builder =>
            {
                builder
                    .AddGrpcCoreInstrumentation()
                    .AddAspNetCoreInstrumentation()
                    .AddNpgsql()
                    .SetResourceBuilder(ResourceBuilder.CreateDefault()
                        .AddService("VehicleFleet"))
                    .AddSource("VehicleFleet")
                    .AddSource("MassTransit")
                    .AddJaegerExporter();
            });

        return services;
    }

    public static IServiceCollection RegisterHealthCheckV1(this IServiceCollection services)
    {
        var getConnectionString = () =>
        {
            var connectionBuilder = new NpgsqlConnectionStringBuilder
            {
                ApplicationName = Configuration.ApplicationName,
                Host = Configuration.PostgresHost,
                Port = Configuration.PostgresPort,
                Database = Configuration.PostgresDatabase,
                Username = Configuration.PostgresUsername,
                Password = Configuration.PostgresPassword,
                BrowsableConnectionString = false
            };

            return connectionBuilder.ConnectionString;
        };

        services.AddGrpcHealthChecks()
            .AddNpgSql(getConnectionString(), timeout: TimeSpan.FromSeconds(10))
            .AddKafka(cfg =>
                    cfg.BootstrapServers = Configuration.BootstrapServers[0],
                timeout: TimeSpan.FromSeconds(10));

        return services;
    }
}

internal class Configuration
{
    public required string ApplicationName { get; init; }

    // Postgres
    public required string PostgresHost { get; init; }
    public required int PostgresPort { get; init; }
    public required string PostgresDatabase { get; init; }
    public required string PostgresUsername { get; init; }
    public required string PostgresPassword { get; init; }


    // Kafka
    public required string[] BootstrapServers { get; init; }
    public required string BookingCreatedTopic { get; init; }
    public required string ModelCreatedTopic { get; init; }
    public required string ModelCategoryUpdatedTopic { get; init; }
    public required string ModelTariffUpdatedTopic { get; init; }
    public required string VehicleAddedTopic { get; init; }
    public required string VehicleDeletedTopic { get; init; }
    public required string VehicleOccupyingProcessedTopic { get; init; }
    public required string VehicleReadiedForReleasesTopic { get; init; }
    public required string VehicleReleasedTopic { get; init; }
    public required string VehicleServicedTopic { get; init; }
    public required string AddingToBookingProcessedTopic { get; init; }
    public required string AddingToRentProcessedTopic { get; init; }
    public required string DocumentAddingCompletedTopic { get; init; }


    // Mongo
    public required string MongoConnectionString { get; init; }
}