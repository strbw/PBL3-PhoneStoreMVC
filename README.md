# HDKmall - Hệ thống Thương mại Điện tử Bán lẻ Đồ công nghệ

**HDKmall** là một ứng dụng web thương mại điện tử hiện đại, chuyên cung cấp các sản phẩm công nghệ như điện thoại, máy tính bảng, laptop và phụ kiện. Dự án được xây dựng trên nền tảng ASP.NET Core MVC với kiến trúc phân lớp rõ ràng, tích hợp AI và các cổng thanh toán phổ biến tại Việt Nam.

## 🚀 Tính năng nổi bật

- **Trợ lý ảo AI (AI Chatbot):** Tích hợp Gemini 1.5 Flash AI, đóng vai trò chuyên viên tư vấn bán hàng 24/7. AI có khả năng nắm bắt dữ liệu thực tế từ Database để tư vấn sản phẩm, so sánh cấu hình và giải đáp chính sách cửa hàng.
- **Hệ thống Sản phẩm Thông minh:** Quản lý sản phẩm theo phiên bản (Version) và biến thể (Variant) như màu sắc, dung lượng, giá riêng biệt.
- **Thanh toán Đa phương thức:** Tích hợp cổng thanh toán VNPay, MoMo và phương thức COD truyền thống.
- **Quản lý Đơn hàng & Admin Dashboard:** Giao diện quản trị trực quan, theo dõi doanh thu, quản lý tồn kho, duyệt đánh giá và quản lý mã giảm giá (Coupon).
- **Trải nghiệm Người dùng (UX):** Giao diện Responsive (Bootstrap 5), thông báo Toast, tìm kiếm thông minh và quy trình Checkout tối ưu.

## 🛠 Công nghệ sử dụng

- **Backend:** ASP.NET Core 9.0 MVC, Entity Framework Core.
- **Database:** SQL Server.
- **Frontend:** HTML5, CSS3, JavaScript, jQuery, Bootstrap 5.
- **AI Integration:** Google Gemini AI API.
- **Third-party Services:** Cloudinary (Lưu trữ ảnh), VNPay & MoMo (Thanh toán).

## 📋 Yêu cầu hệ thống (Prerequisites)

- **.NET SDK 9.0** trở lên.
- **SQL Server Express** hoặc các phiên bản tương đương.
- **Tài khoản API:** Gemini AI Key, Cloudinary Account, VNPay/MoMo Sandbox (nếu muốn test thanh toán).

## 💻 Hướng dẫn Cài đặt & Chạy dự án

### Bước 1: Clone kho lưu trữ
```bash
git clone https://github.com/your-username/PBL3-PhoneStoreMVC.git
cd PBL3-PhoneStoreMVC
```

### Bước 2: Cấu hình Secret Keys
1. Tìm file `HDKmall/appsettings.Example.json`.
2. Tạo một bản sao và đổi tên thành `HDKmall/appsettings.json`.
3. Mở file `appsettings.json` và điền các thông tin của bạn:
   - **Database:** Thay đổi `DefaultConnection` cho phù hợp với Server SQL của bạn.
   - **Gemini AI:** Lấy API Key tại [Google AI Studio](https://aistudio.google.com/app/apikey) và dán vào `GeminiAI:ApiKey`.
   - **Cloudinary/VNPay/MoMo:** Điền các Key tương ứng nếu bạn có tài khoản.

### Bước 3: Khởi tạo Cơ sở dữ liệu
Mở **Package Manager Console** trong Visual Studio hoặc dùng Terminal:
```bash
dotnet ef database update --project HDKmall
```
*(Lưu ý: Dữ liệu mẫu như Admin user, Categories, Brands sẽ được tự động Seed vào DB sau lệnh này).*

### Bước 4: Chạy dự án
```bash
dotnet run --project HDKmall
```
Sau đó truy cập `https://localhost:5001` (hoặc port được hiển thị trên console) để trải nghiệm.

## 🤝 Đóng góp
Nếu bạn có ý tưởng cải tiến hoặc phát hiện lỗi, vui lòng tạo **Issue** hoặc gửi **Pull Request**.

---
*Dự án được phát triển với mục đích học tập và nghiên cứu (PBL3).*
