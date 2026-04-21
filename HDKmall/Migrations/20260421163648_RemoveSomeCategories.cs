using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HDKmall.Migrations
{
    /// <inheritdoc />
    public partial class RemoveSomeCategories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 40,
                column: "BrandId",
                value: 9);

            migrationBuilder.DeleteData(
                table: "Brands",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 69);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 70);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 71);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 72);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 73);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 74);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 75);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 76);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 41);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 42);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 44);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 46);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 47);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 48);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 49);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 50);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.UpdateData(
                table: "Brands",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "Description", "Name" },
                values: new object[] { "Sản phẩm từ Realme", "Realme" });

            migrationBuilder.UpdateData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 67,
                columns: new[] { "ImageUrl", "Price", "ProductId", "Stock" },
                values: new object[] { "/images/s24cam.jpg", 22999000m, 43, 23 });

            migrationBuilder.UpdateData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 68,
                columns: new[] { "ImageUrl", "Price", "ProductId", "Stock" },
                values: new object[] { "/images/ip15cam.jpg", 29999000m, 45, 25 });

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Brands",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "Description", "Name" },
                values: new object[] { "Sản phẩm từ Soundcore", "Soundcore" });

            migrationBuilder.InsertData(
                table: "Brands",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[] { 10, "Sản phẩm từ Realme", "Realme" });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 7, "Máy ảnh, webcam", "Camera" },
                    { 8, "Loa Bluetooth, loa để bàn", "Loa" },
                    { 9, "Power bank, sạc dự phòng", "Pin sạc" },
                    { 10, "Bộ chơi game, đồ chơi công nghệ", "Game & Toy" }
                });

            migrationBuilder.UpdateData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 67,
                columns: new[] { "ImageUrl", "Price", "ProductId", "Stock" },
                values: new object[] { "/images/a7rv.jpg", 54999000m, 41, 21 });

            migrationBuilder.UpdateData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 68,
                columns: new[] { "ImageUrl", "Price", "ProductId", "Stock" },
                values: new object[] { "/images/a6700.jpg", 24999000m, 42, 22 });

            migrationBuilder.InsertData(
                table: "ProductVariants",
                columns: new[] { "Id", "Capacity", "Color", "ImageUrl", "Price", "ProductId", "Stock" },
                values: new object[,]
                {
                    { 69, "Default", "Standard", "/images/s24cam.jpg", 22999000m, 43, 23 },
                    { 71, "Default", "Standard", "/images/ip15cam.jpg", 29999000m, 45, 25 }
                });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 40,
                column: "BrandId",
                value: 10);

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "BrandId", "CategoryId", "Description", "ImageUrl", "Name", "Price", "PublicId" },
                values: new object[,]
                {
                    { 41, 6, 7, "Professional camera", "/images/a7rv.jpg", "Sony A7R V", 54999000m, null },
                    { 42, 6, 7, "Mirrorless camera", "/images/a6700.jpg", "Sony A6700", 24999000m, null },
                    { 44, 6, 7, "Professional drone", "/images/djimini4pro.jpg", "DJI Mini 4 Pro", 18999000m, null },
                    { 46, 8, 8, "Portable speaker", "/images/jblbb3.jpg", "JBL Boombox 3", 8999000m, null },
                    { 47, 6, 8, "Good Bluetooth speaker", "/images/sonyultpower.jpg", "Sony ULT Power Sound", 4999000m, null },
                    { 48, 9, 8, "Excellent portable speaker", "/images/scmotion300.jpg", "Soundcore Motion 300", 3999000m, null },
                    { 49, 1, 8, "Smart speaker", "/images/homepadmini.jpg", "Apple HomePod Mini", 1999000m, null },
                    { 50, 2, 8, "Smart speaker", "/images/galaxyhome.jpg", "Samsung Galaxy Home", 2999000m, null }
                });

            migrationBuilder.InsertData(
                table: "ProductVariants",
                columns: new[] { "Id", "Capacity", "Color", "ImageUrl", "Price", "ProductId", "Stock" },
                values: new object[,]
                {
                    { 70, "Default", "Standard", "/images/djimini4pro.jpg", 18999000m, 44, 24 },
                    { 72, "Default", "Standard", "/images/jblbb3.jpg", 8999000m, 46, 26 },
                    { 73, "Default", "Standard", "/images/sonyultpower.jpg", 4999000m, 47, 27 },
                    { 74, "Default", "Standard", "/images/scmotion300.jpg", 3999000m, 48, 28 },
                    { 75, "Default", "Standard", "/images/homepadmini.jpg", 1999000m, 49, 29 },
                    { 76, "Default", "Standard", "/images/galaxyhome.jpg", 2999000m, 50, 20 }
                });
        }
    }
}
