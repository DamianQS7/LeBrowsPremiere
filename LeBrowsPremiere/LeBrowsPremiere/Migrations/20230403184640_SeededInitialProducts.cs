using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LeBrowsPremiere.Migrations
{
    /// <inheritdoc />
    public partial class SeededInitialProducts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            
            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 1,
                column: "Name",
                value: "Eyebrows");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 2,
                column: "Name",
                value: "Eyelashes");

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Brand", "CategoryId", "CurrentStock", "Description", "ImageUrl", "MinimumStock", "Name", "Price", "SupplierId" },
                values: new object[,]
                {
                    { 1, "Premieré", 2, 0, "Removes germ and dust between natural eyelashes with consistent foaming formula eyelashes. 50 mL", "\\img\\products\\lash_shampoo.JPG", 1, "Eyelash Shampoo", 12.0, 1 },
                    { 2, "Premieré", 2, 0, "Latex and Formaldehyde Free. 10 g", "\\img\\products\\lash_remover.JPG", 1, "Lash Remover", 9.0, 1 },
                    { 3, "Premieré", 2, 0, "Latex and Formaldehyde Free. 15 mL", "\\img\\products\\lash_primer.JPG", 1, "Lash Primer Cleanser", 10.0, 1 },
                    { 4, "Premieré", 2, 0, "Latex and Formaldehyde Free. 5 mL", "\\img\\products\\lash_glue.JPG", 1, "Eyelash Glue", 25.0, 1 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4);


            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 1,
                column: "Name",
                value: "Skincare");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 2,
                column: "Name",
                value: "Makeup");
        }
    }
}
