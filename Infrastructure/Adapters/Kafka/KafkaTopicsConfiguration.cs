namespace Infrastructure.Adapters.Kafka;

public class KafkaTopicsConfiguration
{
    public required string ModelCreatedTopic { get; set; }
    public required string ModelCategoryUpdatedTopic { get; set; }
    public required string ModelTariffUpdatedTopic { get; set; }
    public required string VehicleAddedTopic { get; set; }
    public required string VehicleDeletedTopic { get; set; }
    public required string VehicleOccupiedTopic { get; set; }
    public required string VehicleReadiedForReleaseTopic { get; set; }
    public required string VehicleReleasedTopic { get; set; }
    public required string VehicleServicedTopic { get; set; }
}