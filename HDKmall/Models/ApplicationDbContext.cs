using Microsoft.EntityFrameworkCore;

namespace HDKmall.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Role> Roles { get; set; }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<ProductVariant> ProductVariants { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Coupon> Coupons { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<ReviewImage> ReviewImages { get; set; }
        public DbSet<ReviewTag> ReviewTags { get; set; }
        public DbSet<ReviewTagMapping> ReviewTagMappings { get; set; }
        public DbSet<Banner> Banners { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<ProductSpecification> ProductSpecifications { get; set; }
        public DbSet<UserAddress> UserAddresses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserAddress>()
                .HasOne(a => a.User)
                .WithMany(u => u.Addresses)
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Review relationships
            modelBuilder.Entity<ReviewImage>()
                .HasOne(ri => ri.Review)
                .WithMany(r => r.Images)
                .HasForeignKey(ri => ri.ReviewId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ReviewTagMapping>()
                .HasOne(rtm => rtm.Review)
                .WithMany(r => r.TagMappings)
                .HasForeignKey(rtm => rtm.ReviewId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ReviewTagMapping>()
                .HasOne(rtm => rtm.Tag)
                .WithMany(t => t.ReviewMappings)
                .HasForeignKey(rtm => rtm.ReviewTagId)
                .OnDelete(DeleteBehavior.Cascade);

            // ==================== REVIEW PRESET TAGS ====================
            modelBuilder.Entity<ReviewTag>().HasData(
                new ReviewTag { Id = 1, Name = "Giao hàng nhanh", Emoji = "🚀", DisplayOrder = 1 },
                new ReviewTag { Id = 2, Name = "Đóng gói cẩn thận", Emoji = "📦", DisplayOrder = 2 },
                new ReviewTag { Id = 3, Name = "Sản phẩm đẹp", Emoji = "👍", DisplayOrder = 3 },
                new ReviewTag { Id = 4, Name = "Chất lượng tốt", Emoji = "💎", DisplayOrder = 4 },
                new ReviewTag { Id = 5, Name = "Đúng mô tả", Emoji = "✔️", DisplayOrder = 5 },
                new ReviewTag { Id = 6, Name = "Giá hợp lý", Emoji = "💰", DisplayOrder = 6 },
                new ReviewTag { Id = 7, Name = "Nhân viên hỗ trợ tốt", Emoji = "🤝", DisplayOrder = 7 },
                new ReviewTag { Id = 8, Name = "Sẽ mua lại", Emoji = "🔁", DisplayOrder = 8 }
            );

            // ==================== ROLES ====================
            modelBuilder.Entity<Role>().HasData(
                new Role { RoleId = 1, RoleName = "Admin" },
                new Role { RoleId = 2, RoleName = "Customer" }
            );

            // ==================== USERS ====================
            var users = new List<User>
            {
                // Admin user
                new User 
                { 
                    UserId = 1, 
                    FullName = "Administrator", 
                    Email = "admin@hdk.com", 
                    PhoneNumber = "0123456789", 
                    PasswordHash = "admin123", 
                    RoleId = 1, 
                    IsActive = true, 
                    CreatedAt = new System.DateTime(2025, 1, 1)
                },
                // Customer users
                new User { UserId = 2, FullName = "Nguyễn Văn A", Email = "nguyen.van.a@email.com", PhoneNumber = "0912345678", PasswordHash = "password123", RoleId = 2, IsActive = true, CreatedAt = new DateTime(2025, 3, 15) },
                new User { UserId = 3, FullName = "Trần Thị B", Email = "tran.thi.b@email.com", PhoneNumber = "0987654321", PasswordHash = "password123", RoleId = 2, IsActive = true, CreatedAt = new DateTime(2025, 3, 20) },
                new User { UserId = 4, FullName = "Lê Văn C", Email = "le.van.c@email.com", PhoneNumber = "0912111111", PasswordHash = "password123", RoleId = 2, IsActive = true, CreatedAt = new DateTime(2025, 3, 25) },
                new User { UserId = 5, FullName = "Phạm Thị D", Email = "pham.thi.d@email.com", PhoneNumber = "0912222222", PasswordHash = "password123", RoleId = 2, IsActive = true, CreatedAt = new DateTime(2025, 3, 30) },
                new User { UserId = 6, FullName = "Hoàng Văn E", Email = "hoang.van.e@email.com", PhoneNumber = "0912333333", PasswordHash = "password123", RoleId = 2, IsActive = true, CreatedAt = new DateTime(2025, 4, 1) },
                new User { UserId = 7, FullName = "Đỗ Thị F", Email = "do.thi.f@email.com", PhoneNumber = "0912444444", PasswordHash = "password123", RoleId = 2, IsActive = true, CreatedAt = new DateTime(2025, 4, 5) },
                new User { UserId = 8, FullName = "Vũ Văn G", Email = "vu.van.g@email.com", PhoneNumber = "0912555555", PasswordHash = "password123", RoleId = 2, IsActive = true, CreatedAt = new DateTime(2025, 4, 8) },
                new User { UserId = 9, FullName = "Đinh Thị H", Email = "dinh.thi.h@email.com", PhoneNumber = "0912666666", PasswordHash = "password123", RoleId = 2, IsActive = true, CreatedAt = new DateTime(2025, 4, 10) },
                new User { UserId = 10, FullName = "Bùi Văn I", Email = "bui.van.i@email.com", PhoneNumber = "0912777777", PasswordHash = "password123", RoleId = 2, IsActive = true, CreatedAt = new DateTime(2025, 4, 11) },
                new User { UserId = 11, FullName = "Cao Thị K", Email = "cao.thi.k@email.com", PhoneNumber = "0912888888", PasswordHash = "password123", RoleId = 2, IsActive = true, CreatedAt = new DateTime(2025, 4, 12) }
            };
            modelBuilder.Entity<User>().HasData(users);

            // ==================== CATEGORIES ====================
            var categories = new List<Category>
            {
                new Category { Id = 1, Name = "Điện thoại", Description = "Các loại điện thoại di động" },
                new Category { Id = 2, Name = "Máy tính bảng", Description = "iPad, Samsung Tab, v.v." },
                new Category { Id = 3, Name = "Laptop", Description = "Máy tính xách tay" },
                new Category { Id = 4, Name = "Tai nghe", Description = "Tai nghe Bluetooth, có dây" },
                new Category { Id = 5, Name = "Phụ kiện", Description = "Cáp, bộ sạc, ốp lưng" },
                new Category { Id = 6, Name = "Đồng hồ thông minh", Description = "Smart watch" }
            };
            modelBuilder.Entity<Category>().HasData(categories);

            // ==================== BRANDS ====================
            var brands = new List<Brand>
            {
                new Brand { Id = 1, Name = "Apple", Description = "Sản phẩm từ Apple Inc." },
                new Brand { Id = 2, Name = "Samsung", Description = "Sản phẩm từ Samsung" },
                new Brand { Id = 3, Name = "Xiaomi", Description = "Sản phẩm từ Xiaomi" },
                new Brand { Id = 4, Name = "Oppo", Description = "Sản phẩm từ Oppo" },
                new Brand { Id = 5, Name = "Vivo", Description = "Sản phẩm từ Vivo" },
                new Brand { Id = 6, Name = "Sony", Description = "Sản phẩm từ Sony" },
                new Brand { Id = 7, Name = "Anker", Description = "Sản phẩm từ Anker" },
                new Brand { Id = 8, Name = "JBL", Description = "Sản phẩm từ JBL" },
                new Brand { Id = 9, Name = "Realme", Description = "Sản phẩm từ Realme" }
            };
            modelBuilder.Entity<Brand>().HasData(brands);

            // ==================== PRODUCTS ====================
            var products = new List<Product>
            {
                // Apple iPhones
                new Product { Id = 1, Name = "iPhone 15 Pro Max", Price = 29999000, Description = "Flagship Apple 2024", ImageUrl = "/images/iphone15promax.jpg", CategoryId = 1, BrandId = 1 },
                new Product { Id = 2, Name = "iPhone 15 Pro", Price = 24999000, Description = "Pro model 2024", ImageUrl = "/images/iphone15pro.jpg", CategoryId = 1, BrandId = 1 },
                new Product { Id = 3, Name = "iPhone 15", Price = 19999000, Description = "Standard model 2024", ImageUrl = "/images/iphone15.jpg", CategoryId = 1, BrandId = 1 },
                new Product { Id = 4, Name = "iPhone 14 Pro", Price = 21999000, Description = "Pro model 2023", ImageUrl = "/images/iphone14pro.jpg", CategoryId = 1, BrandId = 1 },
                new Product { Id = 5, Name = "iPhone SE", Price = 12999000, Description = "Budget friendly", ImageUrl = "/images/iphonese.jpg", CategoryId = 1, BrandId = 1 },

                // Samsung flagships
                new Product { Id = 6, Name = "Samsung Galaxy S24 Ultra", Price = 28999000, Description = "Ultra flagship 2024", ImageUrl = "/images/s24ultra.jpg", CategoryId = 1, BrandId = 2 },
                new Product { Id = 7, Name = "Samsung Galaxy S24", Price = 22999000, Description = "Standard flagship", ImageUrl = "/images/s24.jpg", CategoryId = 1, BrandId = 2 },
                new Product { Id = 8, Name = "Samsung Galaxy A55", Price = 10999000, Description = "Mid-range phone", ImageUrl = "/images/a55.jpg", CategoryId = 1, BrandId = 2 },
                new Product { Id = 9, Name = "Samsung Galaxy Z Fold5", Price = 39999000, Description = "Foldable phone", ImageUrl = "/images/zfold5.jpg", CategoryId = 1, BrandId = 2 },
                new Product { Id = 10, Name = "Samsung Galaxy Tab S9", Price = 18999000, Description = "Premium tablet", ImageUrl = "/images/tabs9.jpg", CategoryId = 2, BrandId = 2 },

                // Xiaomi phones
                new Product { Id = 11, Name = "Xiaomi 14 Ultra", Price = 21999000, Description = "Premium Xiaomi", ImageUrl = "/images/xiaomi14ultra.jpg", CategoryId = 1, BrandId = 3 },
                new Product { Id = 12, Name = "Xiaomi 14", Price = 17999000, Description = "Standard flagship", ImageUrl = "/images/xiaomi14.jpg", CategoryId = 1, BrandId = 3 },
                new Product { Id = 13, Name = "Xiaomi Redmi Note 13", Price = 8999000, Description = "Budget choice", ImageUrl = "/images/redminote13.jpg", CategoryId = 1, BrandId = 3 },
                new Product { Id = 14, Name = "Xiaomi Pad 6", Price = 15999000, Description = "Budget tablet", ImageUrl = "/images/xiaomipad6.jpg", CategoryId = 2, BrandId = 3 },
                new Product { Id = 15, Name = "Xiaomi 13T Pro", Price = 19999000, Description = "Last year flagship", ImageUrl = "/images/xiaomi13tpro.jpg", CategoryId = 1, BrandId = 3 },

                // Oppo phones
                new Product { Id = 16, Name = "Oppo Find X7 Ultra", Price = 27999000, Description = "Premium flagship", ImageUrl = "/images/findx7ultra.jpg", CategoryId = 1, BrandId = 4 },
                new Product { Id = 17, Name = "Oppo Find X7", Price = 23999000, Description = "Standard flagship", ImageUrl = "/images/findx7.jpg", CategoryId = 1, BrandId = 4 },
                new Product { Id = 18, Name = "Oppo Reno 11", Price = 12999000, Description = "Mid-range phone", ImageUrl = "/images/reno11.jpg", CategoryId = 1, BrandId = 4 },
                new Product { Id = 19, Name = "Oppo A79", Price = 6999000, Description = "Budget phone", ImageUrl = "/images/a79.jpg", CategoryId = 1, BrandId = 4 },
                new Product { Id = 20, Name = "Oppo Pad 2", Price = 12999000, Description = "Budget tablet", ImageUrl = "/images/opad2.jpg", CategoryId = 2, BrandId = 4 },

                // Vivo phones
                new Product { Id = 21, Name = "Vivo X100 Pro", Price = 25999000, Description = "Premium flagship", ImageUrl = "/images/x100pro.jpg", CategoryId = 1, BrandId = 5 },
                new Product { Id = 22, Name = "Vivo X100", Price = 21999000, Description = "Standard flagship", ImageUrl = "/images/x100.jpg", CategoryId = 1, BrandId = 5 },
                new Product { Id = 23, Name = "Vivo V40", Price = 13999000, Description = "Mid-range phone", ImageUrl = "/images/v40.jpg", CategoryId = 1, BrandId = 5 },
                new Product { Id = 24, Name = "Vivo Y100", Price = 6999000, Description = "Budget phone", ImageUrl = "/images/y100.jpg", CategoryId = 1, BrandId = 5 },
                new Product { Id = 25, Name = "Vivo Pad", Price = 13999000, Description = "Budget tablet", ImageUrl = "/images/vpad.jpg", CategoryId = 2, BrandId = 5 },

                // Apple Tablets & Laptops
                new Product { Id = 26, Name = "iPad Pro 12.9", Price = 27999000, Description = "Premium tablet", ImageUrl = "/images/ipadpro129.jpg", CategoryId = 2, BrandId = 1 },
                new Product { Id = 27, Name = "iPad Air", Price = 18999000, Description = "Mid-range tablet", ImageUrl = "/images/ipadair.jpg", CategoryId = 2, BrandId = 1 },
                new Product { Id = 28, Name = "MacBook Pro 16\"", Price = 69999000, Description = "Premium laptop", ImageUrl = "/images/mbpro16.jpg", CategoryId = 3, BrandId = 1 },
                new Product { Id = 29, Name = "MacBook Air M3", Price = 42999000, Description = "Entry-level pro laptop", ImageUrl = "/images/mbairrm3.jpg", CategoryId = 3, BrandId = 1 },
                new Product { Id = 30, Name = "MacBook Air M2", Price = 37999000, Description = "Previous M2 Air", ImageUrl = "/images/mbatrm2.jpg", CategoryId = 3, BrandId = 1 },

                // Accessories & Others
                new Product { Id = 31, Name = "AirPods Pro 2", Price = 5999000, Description = "Premium earbuds", ImageUrl = "/images/airpodspro2.jpg", CategoryId = 4, BrandId = 1 },
                new Product { Id = 32, Name = "AirPods Max", Price = 18999000, Description = "Premium headphones", ImageUrl = "/images/airpodsmax.jpg", CategoryId = 4, BrandId = 1 },
                new Product { Id = 33, Name = "Samsung Galaxy Buds2 Pro", Price = 4999000, Description = "Premium earbuds", ImageUrl = "/images/galaxybuds2pro.jpg", CategoryId = 4, BrandId = 2 },
                new Product { Id = 34, Name = "Sony WH-1000XM5", Price = 7999000, Description = "Premium headphones", ImageUrl = "/images/sonywh1000xm5.jpg", CategoryId = 4, BrandId = 6 },
                new Product { Id = 35, Name = "JBL Live Pro 2", Price = 5999000, Description = "Good earbuds", ImageUrl = "/images/jbllivepro2.jpg", CategoryId = 4, BrandId = 8 },

                // Smart Watches
                new Product { Id = 36, Name = "Apple Watch Ultra 2", Price = 12999000, Description = "Premium smartwatch", ImageUrl = "/images/watchultra2.jpg", CategoryId = 6, BrandId = 1 },
                new Product { Id = 37, Name = "Apple Watch Series 9", Price = 8999000, Description = "Standard smartwatch", ImageUrl = "/images/watchs9.jpg", CategoryId = 6, BrandId = 1 },
                new Product { Id = 38, Name = "Samsung Galaxy Watch6 Pro", Price = 8999000, Description = "Premium Android watch", ImageUrl = "/images/watch6pro.jpg", CategoryId = 6, BrandId = 2 },
                new Product { Id = 39, Name = "Xiaomi Watch S3", Price = 4999000, Description = "Budget smartwatch", ImageUrl = "/images/watchs3.jpg", CategoryId = 6, BrandId = 3 },
                new Product { Id = 40, Name = "Realme Watch 3", Price = 3999000, Description = "Entry smartwatch", ImageUrl = "/images/watch3.jpg", CategoryId = 6, BrandId = 9 },

                // Cameras
                new Product { Id = 43, Name = "Samsung Galaxy S24 Camera", Price = 22999000, Description = "Excellent camera phone", ImageUrl = "/images/s24cam.jpg", CategoryId = 1, BrandId = 2 },
                new Product { Id = 45, Name = "iPhone 15 Pro Max Camera", Price = 29999000, Description = "Amazing camera phone", ImageUrl = "/images/ip15cam.jpg", CategoryId = 1, BrandId = 1 }
            };
            modelBuilder.Entity<Product>().HasData(products);

            // ==================== PRODUCT VARIANTS ====================
            var variants = new List<ProductVariant>();
            int variantId = 1;

            // iPhone 15 Pro Max variants
            variants.AddRange(new[]
            {
                new ProductVariant { Id = variantId++, ProductId = 1, Color = "Black", Capacity = "256GB", Price = 29999000, Stock = 20, ImageUrl = "/images/iphone15promax-black.jpg" },
                new ProductVariant { Id = variantId++, ProductId = 1, Color = "Black", Capacity = "512GB", Price = 33999000, Stock = 15, ImageUrl = "/images/iphone15promax-black-512.jpg" },
                new ProductVariant { Id = variantId++, ProductId = 1, Color = "Black", Capacity = "1TB", Price = 37999000, Stock = 10, ImageUrl = "/images/iphone15promax-black-1tb.jpg" },
                new ProductVariant { Id = variantId++, ProductId = 1, Color = "Gold", Capacity = "256GB", Price = 29999000, Stock = 18, ImageUrl = "/images/iphone15promax-gold.jpg" },
                new ProductVariant { Id = variantId++, ProductId = 1, Color = "Gold", Capacity = "512GB", Price = 33999000, Stock = 12, ImageUrl = "/images/iphone15promax-gold-512.jpg" },
                new ProductVariant { Id = variantId++, ProductId = 1, Color = "Titanium", Capacity = "256GB", Price = 30999000, Stock = 16, ImageUrl = "/images/iphone15promax-titanium.jpg" },
                new ProductVariant { Id = variantId++, ProductId = 1, Color = "Titanium", Capacity = "512GB", Price = 34999000, Stock = 14, ImageUrl = "/images/iphone15promax-titanium-512.jpg" }
            });

            // iPhone 15 Pro variants
            variants.AddRange(new[]
            {
                new ProductVariant { Id = variantId++, ProductId = 2, Color = "Black", Capacity = "128GB", Price = 24999000, Stock = 25, ImageUrl = "/images/iphone15pro-black.jpg" },
                new ProductVariant { Id = variantId++, ProductId = 2, Color = "Black", Capacity = "256GB", Price = 27999000, Stock = 20, ImageUrl = "/images/iphone15pro-black-256.jpg" },
                new ProductVariant { Id = variantId++, ProductId = 2, Color = "Black", Capacity = "512GB", Price = 31999000, Stock = 15, ImageUrl = "/images/iphone15pro-black-512.jpg" },
                new ProductVariant { Id = variantId++, ProductId = 2, Color = "Blue", Capacity = "128GB", Price = 24999000, Stock = 22, ImageUrl = "/images/iphone15pro-blue.jpg" },
                new ProductVariant { Id = variantId++, ProductId = 2, Color = "Blue", Capacity = "256GB", Price = 27999000, Stock = 18, ImageUrl = "/images/iphone15pro-blue-256.jpg" },
                new ProductVariant { Id = variantId++, ProductId = 2, Color = "White", Capacity = "128GB", Price = 24999000, Stock = 20, ImageUrl = "/images/iphone15pro-white.jpg" }
            });

            // Samsung Galaxy S24 Ultra variants
            variants.AddRange(new[]
            {
                new ProductVariant { Id = variantId++, ProductId = 6, Color = "Black", Capacity = "256GB", Price = 28999000, Stock = 20, ImageUrl = "/images/s24ultra-black.jpg" },
                new ProductVariant { Id = variantId++, ProductId = 6, Color = "Black", Capacity = "512GB", Price = 32999000, Stock = 15, ImageUrl = "/images/s24ultra-black-512.jpg" },
                new ProductVariant { Id = variantId++, ProductId = 6, Color = "Titanium Gray", Capacity = "256GB", Price = 28999000, Stock = 18, ImageUrl = "/images/s24ultra-gray.jpg" },
                new ProductVariant { Id = variantId++, ProductId = 6, Color = "Titanium Gray", Capacity = "512GB", Price = 32999000, Stock = 14, ImageUrl = "/images/s24ultra-gray-512.jpg" },
                new ProductVariant { Id = variantId++, ProductId = 6, Color = "Titanium White", Capacity = "256GB", Price = 28999000, Stock = 16, ImageUrl = "/images/s24ultra-white.jpg" }
            });

            // Xiaomi 14 Ultra variants
            variants.AddRange(new[]
            {
                new ProductVariant { Id = variantId++, ProductId = 11, Color = "Black", Capacity = "512GB", Price = 21999000, Stock = 22, ImageUrl = "/images/xiaomi14ultra-black.jpg" },
                new ProductVariant { Id = variantId++, ProductId = 11, Color = "Black", Capacity = "1TB", Price = 24999000, Stock = 18, ImageUrl = "/images/xiaomi14ultra-black-1tb.jpg" },
                new ProductVariant { Id = variantId++, ProductId = 11, Color = "White", Capacity = "512GB", Price = 21999000, Stock = 20, ImageUrl = "/images/xiaomi14ultra-white.jpg" },
                new ProductVariant { Id = variantId++, ProductId = 11, Color = "Blue", Capacity = "512GB", Price = 21999000, Stock = 19, ImageUrl = "/images/xiaomi14ultra-blue.jpg" }
            });

            // AirPods Pro 2 variants
            variants.AddRange(new[]
            {
                new ProductVariant { Id = variantId++, ProductId = 31, Color = "White", Capacity = "AirPods", Price = 5999000, Stock = 50, ImageUrl = "/images/airpods-white.jpg" },
                new ProductVariant { Id = variantId++, ProductId = 31, Color = "Midnight", Capacity = "AirPods", Price = 5999000, Stock = 45, ImageUrl = "/images/airpods-midnight.jpg" }
            });

            // MacBook Pro variants
            variants.AddRange(new[]
            {
                new ProductVariant { Id = variantId++, ProductId = 28, Color = "Silver", Capacity = "512GB", Price = 69999000, Stock = 8, ImageUrl = "/images/mbpro-silver-512.jpg" },
                new ProductVariant { Id = variantId++, ProductId = 28, Color = "Silver", Capacity = "1TB", Price = 79999000, Stock = 5, ImageUrl = "/images/mbpro-silver-1tb.jpg" },
                new ProductVariant { Id = variantId++, ProductId = 28, Color = "Space Black", Capacity = "512GB", Price = 69999000, Stock = 7, ImageUrl = "/images/mbpro-black-512.jpg" },
                new ProductVariant { Id = variantId++, ProductId = 28, Color = "Space Black", Capacity = "1TB", Price = 79999000, Stock = 4, ImageUrl = "/images/mbpro-black-1tb.jpg" }
            });

            // iPad Pro variants
            variants.AddRange(new[]
            {
                new ProductVariant { Id = variantId++, ProductId = 26, Color = "Silver", Capacity = "256GB", Price = 27999000, Stock = 12, ImageUrl = "/images/ipadpro-silver-256.jpg" },
                new ProductVariant { Id = variantId++, ProductId = 26, Color = "Silver", Capacity = "512GB", Price = 31999000, Stock = 8, ImageUrl = "/images/ipadpro-silver-512.jpg" },
                new ProductVariant { Id = variantId++, ProductId = 26, Color = "Space Black", Capacity = "256GB", Price = 27999000, Stock = 10, ImageUrl = "/images/ipadpro-black-256.jpg" }
            });

            // Sony WH-1000XM5 variants
            variants.AddRange(new[]
            {
                new ProductVariant { Id = variantId++, ProductId = 34, Color = "Black", Capacity = "1 Size", Price = 7999000, Stock = 25, ImageUrl = "/images/wh1000xm5-black.jpg" },
                new ProductVariant { Id = variantId++, ProductId = 34, Color = "Silver", Capacity = "1 Size", Price = 7999000, Stock = 20, ImageUrl = "/images/wh1000xm5-silver.jpg" }
            });

            // Apple Watch variants
            variants.AddRange(new[]
            {
                new ProductVariant { Id = variantId++, ProductId = 36, Color = "Titanium", Capacity = "40mm", Price = 12999000, Stock = 15, ImageUrl = "/images/watchultra-40mm.jpg" },
                new ProductVariant { Id = variantId++, ProductId = 36, Color = "Titanium", Capacity = "46mm", Price = 12999000, Stock = 13, ImageUrl = "/images/watchultra-46mm.jpg" }
            });

            // Add more variants for other products (rest use base price)
            foreach (var product in products)
            {
                if (product.Id != 1 && product.Id != 2 && product.Id != 6 && product.Id != 11 && product.Id != 26 && product.Id != 28 && product.Id != 31 && product.Id != 34 && product.Id != 36) // Skip already done
                {
                    variants.Add(new ProductVariant { Id = variantId++, ProductId = product.Id, Color = "Standard", Capacity = "Default", Price = product.Price, Stock = 20 + (product.Id % 10), ImageUrl = product.ImageUrl });
                }
            }

            modelBuilder.Entity<ProductVariant>().HasData(variants);

            // ==================== BANNERS ====================
            var banners = new List<Banner>
            {
                new Banner { Id = 1, Title = "iPhone 15 Pro Max - Công Nghệ Mới Nhất", ImageUrl = "/images/banner-iphone15.jpg", LinkUrl = "/Product/Detail/iphone-15-pro-max", IsActive = true },
                new Banner { Id = 2, Title = "Samsung Galaxy S24 - Siêu Phẩm 2024", ImageUrl = "/images/banner-s24.jpg", LinkUrl = "/Product/Detail/samsung-galaxy-s24-ultra", IsActive = true },
                new Banner { Id = 3, Title = "MacBook Pro - Hiệu Năng Vượt Trội", ImageUrl = "/images/banner-mbpro.jpg", LinkUrl = "/Product/Detail/macbook-pro-16", IsActive = true },
                new Banner { Id = 4, Title = "Khuyến Mãi Lớn - Giảm Giá Đến 40%", ImageUrl = "/images/banner-sale.jpg", LinkUrl = "/Product/Index", IsActive = true },
                new Banner { Id = 5, Title = "Phụ Kiện Chính Hãng - Giá Tốt Nhất", ImageUrl = "/images/banner-accessories.jpg", LinkUrl = "/Product/Index?category=5", IsActive = true }
            };
            modelBuilder.Entity<Banner>().HasData(banners);

            // ==================== COUPONS ====================
            var coupons = new List<Coupon>
            {
                new Coupon { Id = 1, Code = "WELCOME2024", DiscountAmount = 500000, ExpiryDate = new DateTime(2026, 4, 13), UsageLimit = 100, UsedCount = 25 },
                new Coupon { Id = 2, Code = "SAVE10", DiscountAmount = 1000000, ExpiryDate = new DateTime(2025, 10, 13), UsageLimit = 50, UsedCount = 40 },
                new Coupon { Id = 3, Code = "BIGDEAL", DiscountAmount = 2000000, ExpiryDate = new DateTime(2025, 7, 13), UsageLimit = 30, UsedCount = 10 },
                new Coupon { Id = 4, Code = "NEWYEAR", DiscountAmount = 3000000, ExpiryDate = new DateTime(2025, 5, 13), UsageLimit = 20, UsedCount = 8 },
                new Coupon { Id = 5, Code = "STUDENT", DiscountAmount = 500000, ExpiryDate = new DateTime(2026, 4, 13), UsageLimit = 200, UsedCount = 120 },
                new Coupon { Id = 6, Code = "VIPUSER", DiscountAmount = 5000000, ExpiryDate = new DateTime(2025, 10, 13), UsageLimit = 10, UsedCount = 3 },
                new Coupon { Id = 7, Code = "FLASH50", DiscountAmount = 10000000, ExpiryDate = new DateTime(2025, 4, 14), UsageLimit = 5, UsedCount = 2 }
            };
            modelBuilder.Entity<Coupon>().HasData(coupons);
        }
    }
}
