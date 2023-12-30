using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LeBrowsPremiere.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedServicesAndAppointmentTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Comments",
                table: "Appointments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Appointments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Description", "Name", "Price" },
                values: new object[] { "Ombré shading is the newest trend in eyebrow semi-permanent makeup. It gives a soft shaded brow pencil look. Ombre powder brows are great for any skin type, less abrasive than microblading leave your eyebrows look fuller and more defined. Last longer usually up to 2 years. Rate is for 2 sessions with after care kit.)", "Ombre Powder Brows (2 sessions)", 299.0 });

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Description", "Name" },
                values: new object[] { "Classic eyelash extensions are a simple, beautiful, natural-looking lash style. They are applied on a 1:1 ratio, which means one extension is attached to one natural lash. This gives you a natural looking lash.", "Classic Lashes (Full Set)" });

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Description", "Name" },
                values: new object[] { "Hybrid sets combine the classic and volume technique into one. In this method, some natural lashes get a single extension while others get multiple. Possessing the definition of the classic and the fluffiness of the volume, the hybrid set is the best of both worlds!", "Hybrid Lashes (Full Set)" });

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Description", "Name" },
                values: new object[] { "Uses 5D up to 20D but still super lightweight lashes that are crafted into a fan before being applied to individual natural lashes. This technique increases your lash count giving you a dramatic look with unrivalled fullness.", "Volume Lashes (Full Set)" });

            migrationBuilder.InsertData(
                table: "Services",
                columns: new[] { "Id", "Description", "Duration", "Name", "Price" },
                values: new object[,]
                {
                    { 5, "Ombré shading is the newest trend in eyebrow semi-permanent makeup. It gives a soft shaded brow pencil look. Ombre powder brows are great for any skin type, less abrasive than microblading leave your eyebrows look fuller and more defined. Last longer usually up to 2 years. (Rate is per session with after care kit.) ", 120, "Ombre Powder Brows (1 session)", 180.0 },
                    { 6, "Classic eyelash extensions are a simple, beautiful, natural-looking lash style. They are applied on a 1:1 ratio, which means one extension is attached to one natural lash. This gives you a natural looking lash. (Refill is 2-3 weeks with at least 50% lashes.)", 90, "Classic Lashes (Refill)", 40.0 },
                    { 7, "Hybrid sets combine the classic and volume technique into one. In this method, some natural lashes get a single extension while others get multiple. Possessing the definition of the classic and the fluffiness of the volume, the hybrid set is the best of both worlds! (Refill is 2-3 weeks with at least 50% lashes.)", 90, "Hybrid Lashes (Refill)", 55.0 },
                    { 8, "Uses 5D up to 20D but still super lightweight lashes that are crafted into a fan before being applied to individual natural lashes. This technique increases your lash count giving you a dramatic look with unrivalled fullness. (Refill is 2-3 weeks with at least 50% lashes.)", 120, "Volume Lashes (Refill)", 65.0 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DropColumn(
                name: "Comments",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Appointments");

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Description", "Name", "Price", "Duration" },
                values: new object[] { "Ombré shading is the newest trend in eyebrow semi-permanent makeup." +
                "It gives a soft shaded brow pencil look. Ombre powder brows are great for any skin type," +
                "less abrasive than microblading leave your eyebrows look fuller and more defined." +
                "Last longer usually up to 2 years. (Rate is for 2 sessions with after care kit.)",
                    "Ombre Powder Brows (2 sessions)", 299.0, 120});

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Description", "Name", "Price", "Duration" },
                values: new object[] { "Classic eyelash extensions are a simple, beautiful, natural-looking lash style" +
                "They are applied on a 1:1 ratio, which means one extension is attached to one natural lash." +
                "This gives you a natural looking lash.",
                    "Classic Lashes (Full Set)", 60.0, 90 });

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Description", "Name", "Price", "Duration" },
                values: new object[] { "Hybrid sets combine the classic and volume technique into one. In this method," +
                "some natural lashes get a single extension while others get multiple." +
                "Possessing the definition of the classic and the fluffiness of the volume," +
                "the hybrid set is the best of both worlds!",
                    "Hybrid Lashes (Full Set)", 75.0, 120 });

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Description", "Name", "Price", "Duration" },
                values: new object[] { "Uses 5D up to 20D but still super lightweight lashes that are crafted into a fan " +
                "before being applied to individual natural lashes. This technique increases your lash count giving" +
                "you a dramatic look with unrivalled fullness.",
                    "Volume Lashes (Full Set)", 85.0, 150 });

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Description", "Name", "Price", "Duration" },
                values: new object[] { "Ombré shading is the newest trend in eyebrow semi-permanent makeup." +
                "It gives a soft shaded brow pencil look. Ombre powder brows are great for any skin type," +
                "less abrasive than microblading leave your eyebrows look fuller and more defined." +
                "Last longer usually up to 2 years. (Rate is per session with after care kit.)",
                    "Ombre Powder Brows (1 session)", 180.0, 120});

            migrationBuilder.UpdateData(
               table: "Services",
               keyColumn: "Id",
               keyValue: 6,
               columns: new[] { "Description", "Name", "Price", "Duration" },
               values: new object[] { "Classic eyelash extensions are a simple, beautiful, natural-looking lash style" +
                "They are applied on a 1:1 ratio, which means one extension is attached to one natural lash." +
                "This gives you a natural looking lash. (Refill is 2-3 weeks with at least 50% lashes.)",
                    "Classic Lashes (Refill)", 40.0, 90 });

            migrationBuilder.UpdateData(
               table: "Services",
               keyColumn: "Id",
               keyValue: 7,
               columns: new[] { "Description", "Name", "Price", "Duration" },
               values: new object[] { "Hybrid sets combine the classic and volume technique into one. In this method," +
                "some natural lashes get a single extension while others get multiple." +
                "Possessing the definition of the classic and the fluffiness of the volume," +
                "the hybrid set is the best of both worlds! (Refill is 2-3 weeks with at least 50% lashes.)",
                    "Hybrid Lashes (Refill)", 55.0, 90 });

            migrationBuilder.UpdateData(
               table: "Services",
               keyColumn: "Id",
               keyValue: 8,
               columns: new[] { "Description", "Name", "Price", "Duration" },
               values: new object[] { "Uses 5D up to 20D but still super lightweight lashes that are crafted into a fan " +
                "before being applied to individual natural lashes. This technique increases your lash count giving" +
                "you a dramatic look with unrivalled fullness. (Refill is 2-3 weeks with at least 50% lashes.)",
                    "Volume Lashes (Refill)", 65.0, 120 });
        }
    }
}
