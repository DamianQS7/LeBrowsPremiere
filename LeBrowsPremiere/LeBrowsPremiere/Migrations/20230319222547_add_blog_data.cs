using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LeBrowsPremiere.Migrations
{
    /// <inheritdoc />
    public partial class addblogdata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Blogs",
                columns: new[] { "Id", "CreatedDate", "Description", "Title" },
                values: new object[,]
                {
                    { 1, new DateTime(2023, 3, 14, 18, 25, 47, 599, DateTimeKind.Local).AddTicks(8443), "In this blog post, we'll explore the various ways you can enhance the beauty of your eyelashes, including tips for mascara application, eyelash curling, and natural remedies for promoting lash growth.", "The Ultimate Guide to Achieving Beautiful Eyelashes" },
                    { 2, new DateTime(2023, 3, 4, 18, 25, 47, 599, DateTimeKind.Local).AddTicks(8445), "If you're looking for a low-maintenance way to achieve long, luscious lashes, lash extensions might be just the thing for you. In this post, we'll explore the pros and cons of lash extensions and provide tips for taking care of them.", "Lash Extensions: Are They Right for You?" },
                    { 3, new DateTime(2023, 2, 22, 18, 25, 47, 599, DateTimeKind.Local).AddTicks(8447), "Just like any other part of your body, your eyelashes require proper care and maintenance to stay healthy and beautiful. In this post, we'll cover the basics of eyelash care, including how to remove makeup without damaging your lashes and how to choose the right mascara for your needs.", "How to Care for Your Eyelashes: A Beginner's Guide" },
                    { 4, new DateTime(2023, 2, 12, 18, 25, 47, 599, DateTimeKind.Local).AddTicks(8449), "If you're looking for a more natural approach to achieving longer, fuller lashes, this post is for you. We'll explore some of the best natural remedies for promoting eyelash growth, including castor oil, coconut oil, and vitamin E.", "Natural Remedies for Promoting Eyelash Growth" },
                    { 5, new DateTime(2023, 2, 2, 18, 25, 47, 599, DateTimeKind.Local).AddTicks(8450), "Did you know that using a lash serum can not only enhance the appearance of your lashes, but also promote better eye health? In this post, we'll explore the benefits of using a lash serum, as well as provide tips for choosing the right one for your needs.", "The Benefits of Lash Serums for Your Eye Health" }
                });

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2023, 3, 19, 18, 25, 47, 599, DateTimeKind.Local).AddTicks(8371));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2023, 3, 19, 18, 25, 47, 599, DateTimeKind.Local).AddTicks(8426));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2023, 3, 19, 18, 25, 47, 599, DateTimeKind.Local).AddTicks(8429));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Blogs",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Blogs",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Blogs",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Blogs",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Blogs",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2023, 3, 19, 16, 17, 5, 103, DateTimeKind.Local).AddTicks(3343));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2023, 3, 19, 16, 17, 5, 103, DateTimeKind.Local).AddTicks(3388));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2023, 3, 19, 16, 17, 5, 103, DateTimeKind.Local).AddTicks(3391));
        }
    }
}
