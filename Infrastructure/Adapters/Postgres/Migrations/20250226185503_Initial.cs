using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Adapters.Postgres.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "model",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    brand = table.Column<string>(type: "text", nullable: false),
                    car_model = table.Column<string>(type: "text", nullable: false),
                    category = table.Column<char>(type: "character(1)", nullable: false),
                    price_per_minute = table.Column<decimal>(type: "numeric", nullable: false),
                    price_per_hour = table.Column<decimal>(type: "numeric", nullable: false),
                    price_per_day = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_model", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "outbox",
                columns: table => new
                {
                    event_id = table.Column<Guid>(type: "uuid", nullable: false),
                    type = table.Column<string>(type: "text", nullable: false),
                    content = table.Column<string>(type: "text", nullable: false),
                    occurred_on_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    processed_on_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_outbox", x => x.event_id);
                });

            migrationBuilder.CreateTable(
                name: "vehicle_status",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vehicle_status", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "vehicle",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    model_id = table.Column<Guid>(type: "uuid", nullable: false),
                    status_id = table.Column<int>(type: "integer", nullable: false),
                    plate_number = table.Column<string>(type: "text", nullable: false),
                    color = table.Column<string>(type: "text", nullable: false),
                    vin = table.Column<string>(type: "text", nullable: false),
                    fuel_level_percents = table.Column<int>(type: "integer", nullable: false),
                    location_latitude = table.Column<double>(type: "double precision", nullable: false),
                    location_longitude = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vehicle", x => x.id);
                    table.ForeignKey(
                        name: "FK_model_id",
                        column: x => x.model_id,
                        principalTable: "model",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_status_id",
                        column: x => x.status_id,
                        principalTable: "vehicle_status",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "vehicle_status",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { 1, "added" },
                    { 2, "readiedforrelease" },
                    { 3, "released" },
                    { 4, "occupied" },
                    { 5, "serviced" },
                    { 6, "deleted" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_vehicle_model_id",
                table: "vehicle",
                column: "model_id");

            migrationBuilder.CreateIndex(
                name: "IX_vehicle_status_id",
                table: "vehicle",
                column: "status_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "outbox");

            migrationBuilder.DropTable(
                name: "vehicle");

            migrationBuilder.DropTable(
                name: "model");

            migrationBuilder.DropTable(
                name: "vehicle_status");
        }
    }
}
