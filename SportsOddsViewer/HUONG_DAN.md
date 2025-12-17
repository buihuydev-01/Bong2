# HƯỚNG DẪN SỬ DỤNG SPORTS ODDS VIEWER

## 📋 MÔ TẢ

Ứng dụng WPF hiển thị và cập nhật tự động thông tin các trận đấu bóng đá và tỷ lệ cược từ API mỗi 1 giây.

## 🎯 TÍNH NĂNG CHÍNH

### 1. Tự động cập nhật
- Ứng dụng tự động gọi API mỗi 1 giây để cập nhật dữ liệu
- Hiển thị thời gian cập nhật gần nhất
- Có thể tạm dừng/tiếp tục cập nhật tự động

### 2. Hiển thị thông tin trận đấu
- **Thời gian**: Giờ bắt đầu trận đấu
- **Trạng thái**: 
  - "Live" (màu đỏ): Đang diễn ra
  - "Sắp diễn ra": Chưa bắt đầu
  - "Kết thúc": Đã kết thúc
- **Giải đấu**: Tên giải đấu (Cup, League, ...)
- **Đội nhà / Đội khách**: Tên các đội thi đấu

### 3. Hiển thị tỷ lệ cược
- **Cược chấp (HDP - Handicap)**:
  - Số đỏ: Tỷ lệ đội nhà
  - Số xanh: Tỷ lệ đội khách
  
- **Tài/Xỉu (OU - Over/Under)**:
  - Số đỏ: Tỷ lệ Tài (Over)
  - Số xanh: Tỷ lệ Xỉu (Under)
  
- **1X2**:
  - Số đỏ: Đội nhà thắng (1)
  - Số xanh lá: Hòa (X)
  - Số xanh: Đội khách thắng (2)

### 4. Giao diện đẹp
- Bảng dữ liệu có màu xanh nhạt
- Dòng xen kẽ màu để dễ đọc
- Header màu xanh đậm
- Số liệu cược có màu sắc phân biệt

## 🚀 CÀI ĐẶT

### Bước 1: Cài đặt .NET SDK

1. Truy cập: https://dotnet.microsoft.com/download
2. Tải **".NET 8.0 SDK"** cho Windows
3. Chạy file cài đặt và làm theo hướng dẫn
4. Khởi động lại máy tính sau khi cài đặt

### Bước 2: Giải nén và Build

1. Giải nén folder `SportsOddsViewer`
2. Mở Command Prompt hoặc PowerShell
3. Chuyển đến thư mục: `cd đường_dẫn\SportsOddsViewer`
4. Chạy lệnh: `build.bat`

**Hoặc** có thể build bằng lệnh thủ công:
```bash
dotnet restore
dotnet build -c Release
```

### Bước 3: Chạy ứng dụng

Có 2 cách:

**Cách 1: Chạy trực tiếp**
```bash
run.bat
```

**Cách 2: Build file .exe**
```bash
publish.bat
```
File .exe sẽ nằm ở: `bin\Release\net8.0-windows\win-x64\publish\SportsOddsViewer.exe`

## 💻 CÁCH SỬ DỤNG

### Khởi động lần đầu

1. Chạy `SportsOddsViewer.exe`
2. Cửa sổ ứng dụng sẽ hiện ra
3. Ứng dụng tự động bắt đầu tải dữ liệu
4. Đợi vài giây để dữ liệu hiển thị

### Các nút chức năng

#### 🔄 Nút "Làm mới ngay"
- Click để cập nhật dữ liệu ngay lập tức
- Không cần đợi 1 giây
- Hữu ích khi muốn xem dữ liệu mới nhất

#### ⏸ Nút "Tạm dừng"
- Click để tạm dừng tự động cập nhật
- Nút sẽ đổi thành "▶ Tiếp tục"
- Click lại để tiếp tục cập nhật tự động

### Thông tin hiển thị

#### Header (Thanh trên cùng)
- Tiêu đề ứng dụng
- Thời gian cập nhật gần nhất
- Các nút điều khiển

#### Bảng dữ liệu chính
- Danh sách các trận đấu
- Thông tin chi tiết mỗi trận
- Tỷ lệ cược theo thời gian thực

#### Footer (Thanh dưới cùng)
- Trạng thái kết nối
- Tổng số trận đấu đang hiển thị

## 🎨 GIAO DIỆN

```
┌─────────────────────────────────────────────────────────────┐
│ ⚽ Sports Odds Viewer    [Cập nhật: HH:mm:ss]  [Làm mới] [⏸] │
├─────────────────────────────────────────────────────────────┤
│ Thời gian │ Trạng thái │ Giải đấu │ Đội nhà │ Đội khách │... │
├─────────────────────────────────────────────────────────────┤
│   21:00   │    Live    │  Cup...  │  Team1  │  Team2    │... │
│   21:15   │ Sắp diễn ra│  League..│  Team3  │  Team4    │... │
│   ...     │    ...     │   ...    │  ...    │  ...      │... │
├─────────────────────────────────────────────────────────────┤
│ ✓ Kết nối thành công                      Số trận: 10        │
└─────────────────────────────────────────────────────────────┘
```

## 🔧 TÙY CHỈNH

### Thay đổi thời gian cập nhật

**Mặc định: 1 giây**

Để thay đổi, mở file `MainWindow.xaml.cs`, tìm dòng:

```csharp
_timer = new DispatcherTimer
{
    Interval = TimeSpan.FromSeconds(1)  // Đổi số 1 thành số khác
};
```

Ví dụ:
- `TimeSpan.FromSeconds(2)` → Cập nhật mỗi 2 giây
- `TimeSpan.FromSeconds(5)` → Cập nhật mỗi 5 giây
- `TimeSpan.FromMilliseconds(500)` → Cập nhật mỗi 0.5 giây

### Thay đổi màu sắc

Mở file `MainWindow.xaml`, tìm các section:

**Màu header:**
```xml
<Setter Property="Background" Value="#4169E1"/>  <!-- Màu nền header -->
<Setter Property="Foreground" Value="White"/>    <!-- Màu chữ header -->
```

**Màu nền bảng:**
```xml
<Setter Property="Background" Value="#F0F8FF"/>           <!-- Màu nền chính -->
<Setter Property="AlternatingRowBackground" Value="#E8F4FF"/>  <!-- Màu dòng xen kẽ -->
```

**Màu tỷ lệ cược:**
```xml
Foreground="Red"    <!-- Đội nhà / Tài -->
Foreground="Blue"   <!-- Đội khách / Xỉu -->
Foreground="Green"  <!-- Hòa -->
```

## ❓ TROUBLESHOOTING

### Lỗi: "Không có dữ liệu"

**Nguyên nhân:**
- Không có kết nối Internet
- API đang bảo trì
- API thay đổi định dạng

**Giải pháp:**
1. Kiểm tra kết nối Internet
2. Thử lại sau vài phút
3. Click nút "Làm mới ngay"

### Lỗi: "Command 'dotnet' not found"

**Nguyên nhân:**
- Chưa cài đặt .NET SDK
- .NET SDK chưa được thêm vào PATH

**Giải pháp:**
1. Cài đặt .NET 8.0 SDK từ Microsoft
2. Khởi động lại Command Prompt
3. Kiểm tra: `dotnet --version`

### Lỗi: Build failed

**Nguyên nhân:**
- Thiếu dependencies
- Lỗi trong code
- File bị hỏng

**Giải pháp:**
1. Xóa folder `bin` và `obj`
2. Chạy lại: `dotnet restore`
3. Chạy lại: `dotnet build`

### Ứng dụng chạy chậm

**Nguyên nhân:**
- Quá nhiều trận đấu
- API phản hồi chậm
- Máy tính yếu

**Giải pháp:**
1. Tăng thời gian cập nhật (từ 1s lên 2-3s)
2. Giảm số trận hiển thị
3. Click "Tạm dừng" khi không cần cập nhật

## 📊 HIỂU VỀ API

### API Endpoint
```
https://sports.wwyyuuvv22.com/web-root/restricted/odds-display/today-data.aspx
```

### Tham số
- `od-param`: Loại dữ liệu cần lấy
- `fi`: Filter
- `v`: Version
- `dl`: Display level

### Response Format
API trả về JavaScript function call với dữ liệu:
```javascript
$M('odds-display').onUpdate(2,[...data...]);
```

### Parsing
Ứng dụng sử dụng Regex để parse:
- League data: Tên giải đấu
- Match data: Thông tin trận đấu
- Odds data: Tỷ lệ cược

## 📝 CẤU TRÚC PROJECT

```
SportsOddsViewer/
│
├── Models/
│   └── Match.cs              ← Model cho trận đấu (INotifyPropertyChanged)
│
├── Services/
│   └── ApiService.cs         ← Service gọi API và parse dữ liệu
│
├── MainWindow.xaml           ← Giao diện UI (XAML)
├── MainWindow.xaml.cs        ← Logic xử lý (Code-behind)
│
├── App.xaml                  ← Application definition
├── App.xaml.cs               ← Application startup
│
├── SportsOddsViewer.csproj   ← Project configuration
│
├── build.bat                 ← Script build
├── run.bat                   ← Script chạy
├── publish.bat               ← Script tạo .exe
│
└── README.md                 ← Hướng dẫn (English)
```

## 🔐 BẢO MẬT

**Lưu ý:**
- Ứng dụng không lưu trữ dữ liệu cá nhân
- Không yêu cầu đăng nhập
- Chỉ đọc dữ liệu công khai từ API
- Không gửi dữ liệu về server

## 📞 HỖ TRỢ

Nếu gặp vấn đề:
1. Đọc kỹ phần Troubleshooting
2. Kiểm tra file README.md
3. Xem log trong ứng dụng (thanh dưới cùng)

## 📜 LICENSE

MIT License - Tự do sử dụng, chỉnh sửa và phân phối

## 🙏 LƯU Ý QUAN TRỌNG

⚠️ **Chỉ sử dụng cho mục đích tham khảo thông tin**

- Không dùng để cá cược bất hợp pháp
- Tỷ lệ chỉ mang tính chất tham khảo
- Tác giả không chịu trách nhiệm về việc sử dụng ứng dụng
- Tuân thủ luật pháp địa phương về cá cược

## 🎉 CHÚC BẠN SỬ DỤNG VUI VẺ!
