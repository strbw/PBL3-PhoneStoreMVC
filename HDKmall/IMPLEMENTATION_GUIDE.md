# 📋 HDKmall - Hướng Dẫn Hoàn Thiện

## ✅ Những Gì Đã Được Làm

### 1. Database
- ✅ Xóa database cũ hoàn toàn
- ✅ Tạo database mới "My_fear" với seed data đầy đủ
- ✅ 50 sản phẩm từ các thương hiệu nổi tiếng (Apple, Samsung, Xiaomi, v.v.)
- ✅ 76 product variants với màu sắc, dung lượng, giá khác nhau
- ✅ 10 categories, 10 brands, 11 users (1 admin + 10 customers)
- ✅ 7 mã giảm giá (coupons)
- ✅ 5 banners cho trang chủ

### 2. Backend Hoàn Tất
- ✅ DAL Layer: Repositories cho tất cả entities
- ✅ BLL Layer: Services cho Product, Category, Brand, Cart, Order, Review, Coupon
- ✅ Controllers:
  - ✅ ProductController (Index, Detail, Search, Category, Brand)
  - ✅ HomeController (Index)
  - ✅ CartController (Add, Remove, Update, Clear)
  - ✅ OrderController (Checkout, CreateOrder, History, Detail, Cancel)
  - ✅ ReviewController (Add, Delete)
  - ✅ Admin/CategoryController (CRUD)
  - ✅ Admin/BrandController (CRUD)
  - ✅ Admin/ProductController (đã có)

## 📝 Cần Hoàn Thiện - Views (Razor Pages)

### Giai Đoạn 2: Khách Hàng

#### 1. **Home/Index.cshtml**
```html
@{
    ViewData["Title"] = "Trang Chủ";
}

<div class="carousel">
    <!-- Slider Banners -->
</div>

<section class="featured-products">
    <h2>Sản Phẩm Nổi Bật</h2>
    <div class="product-grid">
        <!-- Loop FeaturedProducts -->
    </div>
</section>

<section class="new-products">
    <h2>Sản Phẩm Mới</h2>
    <div class="product-grid">
        <!-- Loop NewProducts -->
    </div>
</section>
```

#### 2. **Product/Index.cshtml**
Danh sách sản phẩm với:
- Sidebar filter (Category, Brand, Price range)
- Sorting dropdown (Newest, Price: Low-High, High-Low, Rating)
- Product grid/list view
- Pagination

#### 3. **Product/Detail.cshtml**
- Product images
- Product info (name, price, description)
- Variants selector (Color, Capacity, Price)
- Add to cart button
- Reviews section
- Rating average

#### 4. **Shared Views**
- `_Layout.cshtml` - Main layout
- `_ProductCard.cshtml` - Reusable product card
- `_Pagination.cshtml` - Pagination component

### Giai Đoạn 3: Giỏ Hàng & Đặt Hàng

#### 1. **Cart/Index.cshtml**
- Cart items table
- Update quantity controls
- Remove buttons
- Subtotal, Tax, Total calculations
- Coupon code input
- Proceed to checkout button

#### 2. **Order/Checkout.cshtml**
- Shipping address form
- Payment method selection (COD, VNPay, MoMo)
- Order summary
- Place order button

#### 3. **Order/History.cshtml**
- User orders table
- Order status
- View detail link

#### 4. **Order/Detail.cshtml**
- Order info (Order ID, Date, Status)
- Items list
- Total amount
- Address
- Payment method
- Cancel order button (if pending)

### Giai Đoạn 5: Đánh Giá

#### Review Section (trong Product/Detail.cshtml)
- Rating stars selector
- Comment textarea
- Submit review button
- List of existing reviews

### Admin Views

#### 1. **Admin/Category/Index.cshtml**
- Categories table (Id, Name, Action)
- Create button
- Edit/Delete buttons

#### 2. **Admin/Category/Create.cshtml & Edit.cshtml**
- Form: Name (required), Description
- Submit button

#### 3. **Admin/Brand/Index.cshtml, Create.cshtml, Edit.cshtml**
Tương tự Category

#### 4. **Admin/Product/Index.cshtml**
- Products table
- Actions: Edit, Delete
- Create product button

#### 5. **Admin/_AdminLayout.cshtml**
- Sidebar menu
- Admin dashboard links
- Logout button

## 🚀 Cách Tạo Views - Từng Bước

### 1. Tạo _Layout.cshtml
```html
<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>@ViewData["Title"] - HDKmall</title>
    <link rel="stylesheet" href="~/lib/bootstrap/dist/css/bootstrap.min.css" />
    <link rel="stylesheet" href="~/css/site.css" />
</head>
<body>
    <nav class="navbar navbar-expand-sm navbar-toggleable-sm navbar-light bg-white border-bottom box-shadow mb-3">
        <div class="container-fluid">
            <a class="navbar-brand" href="/">HDKmall 🛍️</a>
            <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target=".navbar-collapse">
                <span class="navbar-toggler-icon"></span>
            </button>
            <div class="navbar-collapse collapse d-sm-inline-flex justify-content-between">
                <ul class="navbar-nav flex-grow-1">
                    <li class="nav-item">
                        <a class="nav-link" asp-controller="Home" asp-action="Index">Trang Chủ</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link" asp-controller="Product" asp-action="Index">Sản Phẩm</a>
                    </li>
                </ul>
                <ul class="navbar-nav">
                    <li class="nav-item">
                        <a class="nav-link" asp-controller="Cart" asp-action="Index">🛒 Giỏ Hàng</a>
                    </li>
                    @if (User.Identity.IsAuthenticated)
                    {
                        <li class="nav-item dropdown">
                            <a class="nav-link dropdown-toggle" href="#" id="userDropdown" role="button" data-bs-toggle="dropdown">
                                @User.Identity.Name
                            </a>
                            <ul class="dropdown-menu" aria-labelledby="userDropdown">
                                <li><a class="dropdown-item" asp-controller="Order" asp-action="History">Đơn Hàng Của Tôi</a></li>
                                <li><hr class="dropdown-divider"></li>
                                <li><a class="dropdown-item" asp-controller="Account" asp-action="Logout">Đăng Xuất</a></li>
                            </ul>
                        </li>
                    }
                    else
                    {
                        <li class="nav-item">
                            <a class="nav-link" asp-controller="Account" asp-action="Login">Đăng Nhập</a>
                        </li>
                    }
                </ul>
            </div>
        </div>
    </nav>

    <div class="container">
        <main role="main" class="pb-3">
            @if (TempData["Success"] != null)
            {
                <div class="alert alert-success alert-dismissible fade show" role="alert">
                    @TempData["Success"]
                    <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
                </div>
            }

            @if (TempData["Error"] != null)
            {
                <div class="alert alert-danger alert-dismissible fade show" role="alert">
                    @TempData["Error"]
                    <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
                </div>
            }

            @RenderBody()
        </main>
    </div>

    <footer class="border-top footer text-muted">
        <div class="container">
            &copy; 2025 HDKmall - E-Commerce Platform. All rights reserved.
        </div>
    </footer>

    <script src="~/lib/jquery/dist/jquery.min.js"></script>
    <script src="~/lib/bootstrap/dist/js/bootstrap.bundle.min.js"></script>
    <script src="~/js/site.js"></script>
    @await RenderSectionAsync("Scripts", required: false)
</body>
</html>
```

### 2. Tạo Product/Index.cshtml
```html
@model PaginationVM

@{
    ViewData["Title"] = "Danh Sách Sản Phẩm";
}

<div class="container-fluid py-5">
    <div class="row">
        <!-- Sidebar Filter -->
        <div class="col-md-3">
            <div class="card">
                <div class="card-body">
                    <h5 class="card-title">Lọc Sản Phẩm</h5>
                    
                    <form asp-action="Index" method="get">
                        <!-- Category Filter -->
                        <div class="mb-3">
                            <label class="form-label">Danh Mục</label>
                            <select class="form-select" name="categoryId">
                                <option value="">Tất cả</option>
                                @foreach (var cat in ViewBag.Categories)
                                {
                                    <option value="@cat.Id" @(ViewBag.SelectedCategoryId == cat.Id ? "selected" : "")>@cat.Name</option>
                                }
                            </select>
                        </div>

                        <!-- Brand Filter -->
                        <div class="mb-3">
                            <label class="form-label">Thương Hiệu</label>
                            <select class="form-select" name="brandId">
                                <option value="">Tất cả</option>
                                @foreach (var brand in ViewBag.Brands)
                                {
                                    <option value="@brand.Id" @(ViewBag.SelectedBrandId == brand.Id ? "selected" : "")>@brand.Name</option>
                                }
                            </select>
                        </div>

                        <!-- Price Range -->
                        <div class="mb-3">
                            <label class="form-label">Giá Từ</label>
                            <input type="number" class="form-control" name="minPrice" value="@ViewBag.SelectedMinPrice" placeholder="0">
                        </div>

                        <div class="mb-3">
                            <label class="form-label">Giá Đến</label>
                            <input type="number" class="form-control" name="maxPrice" value="@ViewBag.SelectedMaxPrice" placeholder="999999999">
                        </div>

                        <!-- Sort -->
                        <div class="mb-3">
                            <label class="form-label">Sắp Xếp</label>
                            <select class="form-select" name="sortBy">
                                <option value="newest" @(ViewBag.SelectedSortBy == "newest" ? "selected" : "")>Mới Nhất</option>
                                <option value="price-low" @(ViewBag.SelectedSortBy == "price-low" ? "selected" : "")>Giá Thấp → Cao</option>
                                <option value="price-high" @(ViewBag.SelectedSortBy == "price-high" ? "selected" : "")>Giá Cao → Thấp</option>
                                <option value="rating" @(ViewBag.SelectedSortBy == "rating" ? "selected" : "")>Đánh Giá Cao</option>
                            </select>
                        </div>

                        <button type="submit" class="btn btn-primary w-100">Lọc</button>
                    </form>
                </div>
            </div>
        </div>

        <!-- Products -->
        <div class="col-md-9">
            <div class="row">
                @foreach (var product in Model.Products)
                {
                    <div class="col-md-4 mb-4">
                        <div class="card">
                            <img src="@product.ImageUrl" class="card-img-top" alt="@product.Name" style="height: 200px; object-fit: cover;">
                            <div class="card-body">
                                <h5 class="card-title">@product.Name</h5>
                                <p class="card-text">@product.CategoryName</p>
                                <div class="mb-2">
                                    <span class="badge bg-warning">⭐ @product.AverageRating.ToString("F1")</span>
                                    <span class="small text-muted">(@product.ReviewCount reviews)</span>
                                </div>
                                <h6 class="text-danger">₫@product.Price.ToString("N0")</h6>
                                <a asp-controller="Product" asp-action="Detail" asp-route-id="@product.Id" class="btn btn-sm btn-primary">Chi Tiết</a>
                            </div>
                        </div>
                    </div>
                }
            </div>

            <!-- Pagination -->
            @if (Model.TotalPages > 1)
            {
                <nav aria-label="Page navigation">
                    <ul class="pagination justify-content-center">
                        @if (Model.HasPreviousPage)
                        {
                            <li class="page-item">
                                <a class="page-link" asp-action="Index" asp-route-page="@(Model.PageNumber - 1)">Previous</a>
                            </li>
                        }

                        @for (int i = 1; i <= Model.TotalPages; i++)
                        {
                            <li class="page-item @(i == Model.PageNumber ? "active" : "")">
                                <a class="page-link" asp-action="Index" asp-route-page="@i">@i</a>
                            </li>
                        }

                        @if (Model.HasNextPage)
                        {
                            <li class="page-item">
                                <a class="page-link" asp-action="Index" asp-route-page="@(Model.PageNumber + 1)">Next</a>
                            </li>
                        }
                    </ul>
                </nav>
            }
        </div>
    </div>
</div>
```

## 🔄 Test Controllers

### Database Test
```bash
# Kiểm tra dữ liệu
SELECT COUNT(*) FROM Products;  -- Should be 50
SELECT COUNT(*) FROM ProductVariants;  -- Should be 76
SELECT COUNT(*) FROM Categories;  -- Should be 10
SELECT COUNT(*) FROM Brands;  -- Should be 10
SELECT COUNT(*) FROM Users;  -- Should be 11
SELECT COUNT(*) FROM Coupons;  -- Should be 7
SELECT COUNT(*) FROM Banners;  -- Should be 5
```

### Run Application
```bash
cd F:\pbl3\HDKmall\HDKmall
dotnet run
```

Truy cập:
- http://localhost:5000 - Trang chủ
- http://localhost:5000/Product/Index - Danh sách sản phẩm
- http://localhost:5000/Admin/Category - Admin categories (cần login)
- http://localhost:5000/Admin/Brand - Admin brands (cần login)

### Login Credentials
```
Admin:
Email: admin@hdk.com
Password: admin123

Customer (bất kỳ):
Email: nguyen.van.a@email.com
Password: password123
```

## 📌 Công Việc Tiếp Theo

1. ✅ Database - DONE
2. ✅ Models & Services - DONE
3. ✅ Controllers - DONE
4. ⏳ **Views (Razor Pages) - IN PROGRESS**
   - Tạo _Layout.cshtml
   - Tạo Product/Index.cshtml
   - Tạo Product/Detail.cshtml
   - Tạo Cart/Index.cshtml
   - Tạo Order views
   - Tạo Admin views
5. ⏳ Styling & UX (Bootstrap 5, CSS custom)
6. ⏳ Payment integration (VNPay, MoMo)
7. ⏳ Testing & Deployment

## 💡 Tips

- Use Bootstrap 5 for responsive design
- Implement toast notifications for user feedback
- Add loading spinners for async operations
- Use Font Awesome for icons
- Implement SEO-friendly URLs (slug for products)

---
**Next Step:** Create Views! Start with _Layout.cshtml and Product/Index.cshtml
