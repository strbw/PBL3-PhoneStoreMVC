using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HDKmall.Migrations
{
    /// <inheritdoc />
    public partial class SeedCompleteData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Banners",
                columns: new[] { "Id", "ImageUrl", "IsActive", "LinkUrl", "Title" },
                values: new object[,]
                {
                    { 1, "/images/banner-iphone15.jpg", true, "/Product/Detail/iphone-15-pro-max", "iPhone 15 Pro Max - Công Nghệ Mới Nhất" },
                    { 2, "/images/banner-s24.jpg", true, "/Product/Detail/samsung-galaxy-s24-ultra", "Samsung Galaxy S24 - Siêu Phẩm 2024" },
                    { 3, "/images/banner-mbpro.jpg", true, "/Product/Detail/macbook-pro-16", "MacBook Pro - Hiệu Năng Vượt Trội" },
                    { 4, "/images/banner-sale.jpg", true, "/Product/Index", "Khuyến Mãi Lớn - Giảm Giá Đến 40%" },
                    { 5, "/images/banner-accessories.jpg", true, "/Product/Index?category=5", "Phụ Kiện Chính Hãng - Giá Tốt Nhất" }
                });

            migrationBuilder.InsertData(
                table: "Brands",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "Sản phẩm từ Apple Inc.", "Apple" },
                    { 2, "Sản phẩm từ Samsung", "Samsung" },
                    { 3, "Sản phẩm từ Xiaomi", "Xiaomi" },
                    { 4, "Sản phẩm từ Oppo", "Oppo" },
                    { 5, "Sản phẩm từ Vivo", "Vivo" },
                    { 6, "Sản phẩm từ Sony", "Sony" },
                    { 7, "Sản phẩm từ Anker", "Anker" },
                    { 8, "Sản phẩm từ JBL", "JBL" },
                    { 9, "Sản phẩm từ Soundcore", "Soundcore" },
                    { 10, "Sản phẩm từ Realme", "Realme" }
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "Các loại điện thoại di động", "Điện thoại" },
                    { 2, "iPad, Samsung Tab, v.v.", "Máy tính bảng" },
                    { 3, "Máy tính xách tay", "Laptop" },
                    { 4, "Tai nghe Bluetooth, có dây", "Tai nghe" },
                    { 5, "Cáp, bộ sạc, ốp lưng", "Phụ kiện" },
                    { 6, "Smart watch", "Đồng hồ thông minh" },
                    { 7, "Máy ảnh, webcam", "Camera" },
                    { 8, "Loa Bluetooth, loa để bàn", "Loa" },
                    { 9, "Power bank, sạc dự phòng", "Pin sạc" },
                    { 10, "Bộ chơi game, đồ chơi công nghệ", "Game & Toy" }
                });

            migrationBuilder.InsertData(
                table: "Coupons",
                columns: new[] { "Id", "Code", "DiscountAmount", "ExpiryDate", "UsageLimit", "UsedCount" },
                values: new object[,]
                {
                    { 1, "WELCOME2024", 500000m, new DateTime(2026, 4, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), 100, 25 },
                    { 2, "SAVE10", 1000000m, new DateTime(2025, 10, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), 50, 40 },
                    { 3, "BIGDEAL", 2000000m, new DateTime(2025, 7, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), 30, 10 },
                    { 4, "NEWYEAR", 3000000m, new DateTime(2025, 5, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), 20, 8 },
                    { 5, "STUDENT", 500000m, new DateTime(2026, 4, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), 200, 120 },
                    { 6, "VIPUSER", 5000000m, new DateTime(2025, 10, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), 10, 3 },
                    { 7, "FLASH50", 10000000m, new DateTime(2025, 4, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, 2 }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "CreatedAt", "Email", "FullName", "IsActive", "PasswordHash", "PhoneNumber", "RoleId" },
                values: new object[,]
                {
                    { 2, new DateTime(2025, 3, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "nguyen.van.a@email.com", "Nguyễn Văn A", true, "password123", "0912345678", 2 },
                    { 3, new DateTime(2025, 3, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "tran.thi.b@email.com", "Trần Thị B", true, "password123", "0987654321", 2 },
                    { 4, new DateTime(2025, 3, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "le.van.c@email.com", "Lê Văn C", true, "password123", "0912111111", 2 },
                    { 5, new DateTime(2025, 3, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "pham.thi.d@email.com", "Phạm Thị D", true, "password123", "0912222222", 2 },
                    { 6, new DateTime(2025, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "hoang.van.e@email.com", "Hoàng Văn E", true, "password123", "0912333333", 2 },
                    { 7, new DateTime(2025, 4, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "do.thi.f@email.com", "Đỗ Thị F", true, "password123", "0912444444", 2 },
                    { 8, new DateTime(2025, 4, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "vu.van.g@email.com", "Vũ Văn G", true, "password123", "0912555555", 2 },
                    { 9, new DateTime(2025, 4, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "dinh.thi.h@email.com", "Đinh Thị H", true, "password123", "0912666666", 2 },
                    { 10, new DateTime(2025, 4, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "bui.van.i@email.com", "Bùi Văn I", true, "password123", "0912777777", 2 },
                    { 11, new DateTime(2025, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "cao.thi.k@email.com", "Cao Thị K", true, "password123", "0912888888", 2 }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "BrandId", "CategoryId", "Description", "ImageUrl", "Name", "Price", "PublicId" },
                values: new object[,]
                {
                    { 1, 1, 1, "Flagship Apple 2024", "/images/iphone15promax.jpg", "iPhone 15 Pro Max", 29999000m, null },
                    { 2, 1, 1, "Pro model 2024", "/images/iphone15pro.jpg", "iPhone 15 Pro", 24999000m, null },
                    { 3, 1, 1, "Standard model 2024", "/images/iphone15.jpg", "iPhone 15", 19999000m, null },
                    { 4, 1, 1, "Pro model 2023", "/images/iphone14pro.jpg", "iPhone 14 Pro", 21999000m, null },
                    { 5, 1, 1, "Budget friendly", "/images/iphonese.jpg", "iPhone SE", 12999000m, null },
                    { 6, 2, 1, "Ultra flagship 2024", "/images/s24ultra.jpg", "Samsung Galaxy S24 Ultra", 28999000m, null },
                    { 7, 2, 1, "Standard flagship", "/images/s24.jpg", "Samsung Galaxy S24", 22999000m, null },
                    { 8, 2, 1, "Mid-range phone", "/images/a55.jpg", "Samsung Galaxy A55", 10999000m, null },
                    { 9, 2, 1, "Foldable phone", "/images/zfold5.jpg", "Samsung Galaxy Z Fold5", 39999000m, null },
                    { 10, 2, 2, "Premium tablet", "/images/tabs9.jpg", "Samsung Galaxy Tab S9", 18999000m, null },
                    { 11, 3, 1, "Premium Xiaomi", "/images/xiaomi14ultra.jpg", "Xiaomi 14 Ultra", 21999000m, null },
                    { 12, 3, 1, "Standard flagship", "/images/xiaomi14.jpg", "Xiaomi 14", 17999000m, null },
                    { 13, 3, 1, "Budget choice", "/images/redminote13.jpg", "Xiaomi Redmi Note 13", 8999000m, null },
                    { 14, 3, 2, "Budget tablet", "/images/xiaomipad6.jpg", "Xiaomi Pad 6", 15999000m, null },
                    { 15, 3, 1, "Last year flagship", "/images/xiaomi13tpro.jpg", "Xiaomi 13T Pro", 19999000m, null },
                    { 16, 4, 1, "Premium flagship", "/images/findx7ultra.jpg", "Oppo Find X7 Ultra", 27999000m, null },
                    { 17, 4, 1, "Standard flagship", "/images/findx7.jpg", "Oppo Find X7", 23999000m, null },
                    { 18, 4, 1, "Mid-range phone", "/images/reno11.jpg", "Oppo Reno 11", 12999000m, null },
                    { 19, 4, 1, "Budget phone", "/images/a79.jpg", "Oppo A79", 6999000m, null },
                    { 20, 4, 2, "Budget tablet", "/images/opad2.jpg", "Oppo Pad 2", 12999000m, null },
                    { 21, 5, 1, "Premium flagship", "/images/x100pro.jpg", "Vivo X100 Pro", 25999000m, null },
                    { 22, 5, 1, "Standard flagship", "/images/x100.jpg", "Vivo X100", 21999000m, null },
                    { 23, 5, 1, "Mid-range phone", "/images/v40.jpg", "Vivo V40", 13999000m, null },
                    { 24, 5, 1, "Budget phone", "/images/y100.jpg", "Vivo Y100", 6999000m, null },
                    { 25, 5, 2, "Budget tablet", "/images/vpad.jpg", "Vivo Pad", 13999000m, null },
                    { 26, 1, 2, "Premium tablet", "/images/ipadpro129.jpg", "iPad Pro 12.9", 27999000m, null },
                    { 27, 1, 2, "Mid-range tablet", "/images/ipadair.jpg", "iPad Air", 18999000m, null },
                    { 28, 1, 3, "Premium laptop", "/images/mbpro16.jpg", "MacBook Pro 16\"", 69999000m, null },
                    { 29, 1, 3, "Entry-level pro laptop", "/images/mbairrm3.jpg", "MacBook Air M3", 42999000m, null },
                    { 30, 1, 3, "Previous M2 Air", "/images/mbatrm2.jpg", "MacBook Air M2", 37999000m, null },
                    { 31, 1, 4, "Premium earbuds", "/images/airpodspro2.jpg", "AirPods Pro 2", 5999000m, null },
                    { 32, 1, 4, "Premium headphones", "/images/airpodsmax.jpg", "AirPods Max", 18999000m, null },
                    { 33, 2, 4, "Premium earbuds", "/images/galaxybuds2pro.jpg", "Samsung Galaxy Buds2 Pro", 4999000m, null },
                    { 34, 6, 4, "Premium headphones", "/images/sonywh1000xm5.jpg", "Sony WH-1000XM5", 7999000m, null },
                    { 35, 8, 4, "Good earbuds", "/images/jbllivepro2.jpg", "JBL Live Pro 2", 5999000m, null },
                    { 36, 1, 6, "Premium smartwatch", "/images/watchultra2.jpg", "Apple Watch Ultra 2", 12999000m, null },
                    { 37, 1, 6, "Standard smartwatch", "/images/watchs9.jpg", "Apple Watch Series 9", 8999000m, null },
                    { 38, 2, 6, "Premium Android watch", "/images/watch6pro.jpg", "Samsung Galaxy Watch6 Pro", 8999000m, null },
                    { 39, 3, 6, "Budget smartwatch", "/images/watchs3.jpg", "Xiaomi Watch S3", 4999000m, null },
                    { 40, 10, 6, "Entry smartwatch", "/images/watch3.jpg", "Realme Watch 3", 3999000m, null },
                    { 41, 6, 7, "Professional camera", "/images/a7rv.jpg", "Sony A7R V", 54999000m, null },
                    { 42, 6, 7, "Mirrorless camera", "/images/a6700.jpg", "Sony A6700", 24999000m, null },
                    { 43, 2, 1, "Excellent camera phone", "/images/s24cam.jpg", "Samsung Galaxy S24 Camera", 22999000m, null },
                    { 44, 6, 7, "Professional drone", "/images/djimini4pro.jpg", "DJI Mini 4 Pro", 18999000m, null },
                    { 45, 1, 1, "Amazing camera phone", "/images/ip15cam.jpg", "iPhone 15 Pro Max Camera", 29999000m, null },
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
                    { 1, "256GB", "Black", "/images/iphone15promax-black.jpg", 29999000m, 1, 20 },
                    { 2, "512GB", "Black", "/images/iphone15promax-black-512.jpg", 33999000m, 1, 15 },
                    { 3, "1TB", "Black", "/images/iphone15promax-black-1tb.jpg", 37999000m, 1, 10 },
                    { 4, "256GB", "Gold", "/images/iphone15promax-gold.jpg", 29999000m, 1, 18 },
                    { 5, "512GB", "Gold", "/images/iphone15promax-gold-512.jpg", 33999000m, 1, 12 },
                    { 6, "256GB", "Titanium", "/images/iphone15promax-titanium.jpg", 30999000m, 1, 16 },
                    { 7, "512GB", "Titanium", "/images/iphone15promax-titanium-512.jpg", 34999000m, 1, 14 },
                    { 8, "128GB", "Black", "/images/iphone15pro-black.jpg", 24999000m, 2, 25 },
                    { 9, "256GB", "Black", "/images/iphone15pro-black-256.jpg", 27999000m, 2, 20 },
                    { 10, "512GB", "Black", "/images/iphone15pro-black-512.jpg", 31999000m, 2, 15 },
                    { 11, "128GB", "Blue", "/images/iphone15pro-blue.jpg", 24999000m, 2, 22 },
                    { 12, "256GB", "Blue", "/images/iphone15pro-blue-256.jpg", 27999000m, 2, 18 },
                    { 13, "128GB", "White", "/images/iphone15pro-white.jpg", 24999000m, 2, 20 },
                    { 14, "256GB", "Black", "/images/s24ultra-black.jpg", 28999000m, 6, 20 },
                    { 15, "512GB", "Black", "/images/s24ultra-black-512.jpg", 32999000m, 6, 15 },
                    { 16, "256GB", "Titanium Gray", "/images/s24ultra-gray.jpg", 28999000m, 6, 18 },
                    { 17, "512GB", "Titanium Gray", "/images/s24ultra-gray-512.jpg", 32999000m, 6, 14 },
                    { 18, "256GB", "Titanium White", "/images/s24ultra-white.jpg", 28999000m, 6, 16 },
                    { 19, "512GB", "Black", "/images/xiaomi14ultra-black.jpg", 21999000m, 11, 22 },
                    { 20, "1TB", "Black", "/images/xiaomi14ultra-black-1tb.jpg", 24999000m, 11, 18 },
                    { 21, "512GB", "White", "/images/xiaomi14ultra-white.jpg", 21999000m, 11, 20 },
                    { 22, "512GB", "Blue", "/images/xiaomi14ultra-blue.jpg", 21999000m, 11, 19 },
                    { 23, "AirPods", "White", "/images/airpods-white.jpg", 5999000m, 31, 50 },
                    { 24, "AirPods", "Midnight", "/images/airpods-midnight.jpg", 5999000m, 31, 45 },
                    { 25, "512GB", "Silver", "/images/mbpro-silver-512.jpg", 69999000m, 28, 8 },
                    { 26, "1TB", "Silver", "/images/mbpro-silver-1tb.jpg", 79999000m, 28, 5 },
                    { 27, "512GB", "Space Black", "/images/mbpro-black-512.jpg", 69999000m, 28, 7 },
                    { 28, "1TB", "Space Black", "/images/mbpro-black-1tb.jpg", 79999000m, 28, 4 },
                    { 29, "256GB", "Silver", "/images/ipadpro-silver-256.jpg", 27999000m, 26, 12 },
                    { 30, "512GB", "Silver", "/images/ipadpro-silver-512.jpg", 31999000m, 26, 8 },
                    { 31, "256GB", "Space Black", "/images/ipadpro-black-256.jpg", 27999000m, 26, 10 },
                    { 32, "1 Size", "Black", "/images/wh1000xm5-black.jpg", 7999000m, 34, 25 },
                    { 33, "1 Size", "Silver", "/images/wh1000xm5-silver.jpg", 7999000m, 34, 20 },
                    { 34, "40mm", "Titanium", "/images/watchultra-40mm.jpg", 12999000m, 36, 15 },
                    { 35, "46mm", "Titanium", "/images/watchultra-46mm.jpg", 12999000m, 36, 13 },
                    { 36, "Default", "Standard", "/images/iphone15.jpg", 19999000m, 3, 23 },
                    { 37, "Default", "Standard", "/images/iphone14pro.jpg", 21999000m, 4, 24 },
                    { 38, "Default", "Standard", "/images/iphonese.jpg", 12999000m, 5, 25 },
                    { 39, "Default", "Standard", "/images/s24.jpg", 22999000m, 7, 27 },
                    { 40, "Default", "Standard", "/images/a55.jpg", 10999000m, 8, 28 },
                    { 41, "Default", "Standard", "/images/zfold5.jpg", 39999000m, 9, 29 },
                    { 42, "Default", "Standard", "/images/tabs9.jpg", 18999000m, 10, 20 },
                    { 43, "Default", "Standard", "/images/xiaomi14.jpg", 17999000m, 12, 22 },
                    { 44, "Default", "Standard", "/images/redminote13.jpg", 8999000m, 13, 23 },
                    { 45, "Default", "Standard", "/images/xiaomipad6.jpg", 15999000m, 14, 24 },
                    { 46, "Default", "Standard", "/images/xiaomi13tpro.jpg", 19999000m, 15, 25 },
                    { 47, "Default", "Standard", "/images/findx7ultra.jpg", 27999000m, 16, 26 },
                    { 48, "Default", "Standard", "/images/findx7.jpg", 23999000m, 17, 27 },
                    { 49, "Default", "Standard", "/images/reno11.jpg", 12999000m, 18, 28 },
                    { 50, "Default", "Standard", "/images/a79.jpg", 6999000m, 19, 29 },
                    { 51, "Default", "Standard", "/images/opad2.jpg", 12999000m, 20, 20 },
                    { 52, "Default", "Standard", "/images/x100pro.jpg", 25999000m, 21, 21 },
                    { 53, "Default", "Standard", "/images/x100.jpg", 21999000m, 22, 22 },
                    { 54, "Default", "Standard", "/images/v40.jpg", 13999000m, 23, 23 },
                    { 55, "Default", "Standard", "/images/y100.jpg", 6999000m, 24, 24 },
                    { 56, "Default", "Standard", "/images/vpad.jpg", 13999000m, 25, 25 },
                    { 57, "Default", "Standard", "/images/ipadair.jpg", 18999000m, 27, 27 },
                    { 58, "Default", "Standard", "/images/mbairrm3.jpg", 42999000m, 29, 29 },
                    { 59, "Default", "Standard", "/images/mbatrm2.jpg", 37999000m, 30, 20 },
                    { 60, "Default", "Standard", "/images/airpodsmax.jpg", 18999000m, 32, 22 },
                    { 61, "Default", "Standard", "/images/galaxybuds2pro.jpg", 4999000m, 33, 23 },
                    { 62, "Default", "Standard", "/images/jbllivepro2.jpg", 5999000m, 35, 25 },
                    { 63, "Default", "Standard", "/images/watchs9.jpg", 8999000m, 37, 27 },
                    { 64, "Default", "Standard", "/images/watch6pro.jpg", 8999000m, 38, 28 },
                    { 65, "Default", "Standard", "/images/watchs3.jpg", 4999000m, 39, 29 },
                    { 66, "Default", "Standard", "/images/watch3.jpg", 3999000m, 40, 20 },
                    { 67, "Default", "Standard", "/images/a7rv.jpg", 54999000m, 41, 21 },
                    { 68, "Default", "Standard", "/images/a6700.jpg", 24999000m, 42, 22 },
                    { 69, "Default", "Standard", "/images/s24cam.jpg", 22999000m, 43, 23 },
                    { 70, "Default", "Standard", "/images/djimini4pro.jpg", 18999000m, 44, 24 },
                    { 71, "Default", "Standard", "/images/ip15cam.jpg", 29999000m, 45, 25 },
                    { 72, "Default", "Standard", "/images/jblbb3.jpg", 8999000m, 46, 26 },
                    { 73, "Default", "Standard", "/images/sonyultpower.jpg", 4999000m, 47, 27 },
                    { 74, "Default", "Standard", "/images/scmotion300.jpg", 3999000m, 48, 28 },
                    { 75, "Default", "Standard", "/images/homepadmini.jpg", 1999000m, 49, 29 },
                    { 76, "Default", "Standard", "/images/galaxyhome.jpg", 2999000m, 50, 20 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Banners",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Banners",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Banners",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Banners",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Banners",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Brands",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Coupons",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Coupons",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Coupons",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Coupons",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Coupons",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Coupons",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Coupons",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 37);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 38);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 39);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 40);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 41);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 42);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 43);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 44);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 45);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 46);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 47);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 48);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 49);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 50);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 51);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 52);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 53);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 54);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 55);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 56);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 57);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 58);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 59);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 60);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 61);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 62);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 63);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 64);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 65);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 66);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 67);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 68);

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
                table: "Users",
                keyColumn: "UserId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 11);

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

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 37);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 38);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 39);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 40);

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
                keyValue: 43);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 44);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 45);

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
                table: "Brands",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Brands",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Brands",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Brands",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Brands",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Brands",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Brands",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Brands",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Brands",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 8);
        }
    }
}
