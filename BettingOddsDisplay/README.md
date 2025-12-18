# Betting Odds Display - Phần mềm hiển thị tỷ lệ cược

Phần mềm C# WPF hiển thị tỷ lệ cược theo thời gian thực, tự động cập nhật mỗi giây.

> 📖 **Quick Navigation**: See [INDEX.md](./INDEX.md) for complete documentation index

## Tính năng

- ✅ Giao diện giống y hệt bảng tỷ lệ cược gốc
- ✅ Tự động cập nhật dữ liệu mỗi 1 giây
- ✅ Hiển thị đầy đủ các loại kèo: Cược chấp (Handicap), Tài/Xỉu (Over/Under), 1X2
- ✅ Màu sắc trực quan: đỏ cho tỷ lệ âm, đen cho tỷ lệ dương
- ✅ Nhóm trận đấu theo giải đấu
- ✅ Hiển thị thời gian cập nhật

## Cấu trúc Project

```
BettingOddsDisplay/
├── Models/                  # Data models
│   ├── BettingData.cs
│   ├── League.cs
│   ├── Match.cs
│   └── OddsGroup.cs
├── Services/                # Business logic
│   ├── OddsDataService.cs   # HTTP client và timer
│   └── ResponseParser.cs    # Parse JavaScript response
├── ViewModels/              # View models
│   └── LeagueViewModel.cs
├── Converters/              # XAML converters
│   ├── OddsColorConverter.cs
│   └── StringVisibilityConverter.cs
├── MainWindow.xaml          # Giao diện chính
├── MainWindow.xaml.cs       # Logic giao diện
├── App.xaml                 # Application resources
└── App.xaml.cs              # Application entry point
```

## Yêu cầu hệ thống

- .NET 8.0 hoặc cao hơn
- Windows OS
- Visual Studio 2022 hoặc cao hơn (khuyến nghị)

## Cài đặt và Chạy

### Cách 1: Sử dụng Visual Studio

1. Mở file `BettingOddsDisplay.sln` bằng Visual Studio
2. Restore NuGet packages (tự động)
3. Nhấn F5 để build và chạy

### Cách 2: Sử dụng Command Line

```bash
cd BettingOddsDisplay
dotnet restore
dotnet build
dotnet run
```

## Dependencies

- **Newtonsoft.Json** (v13.0.3): Parse JSON data

## Cấu hình

### Thay đổi URL API

Mở file `Services/OddsDataService.cs` và chỉnh sửa:

```csharp
var url = "https://sports.wwyyuuvv22.com/web-root/restricted/odds-display/today-data.aspx?od-param=2,1,1,1,1,2,1,2,0&fi=1&v=131885&dl=0";
```

### Thay đổi tần suất cập nhật

Mở file `Services/OddsDataService.cs` và chỉnh sửa timer interval (đơn vị: milliseconds):

```csharp
// Thay đổi 1000 thành giá trị khác (ví dụ: 2000 = 2 giây)
_timer = new Timer(async _ => await FetchDataAsync(), null, 1000, 1000);
```

### Cập nhật Cookies

Nếu cần thay đổi cookies để truy cập API, chỉnh sửa trong `Services/OddsDataService.cs`:

```csharp
_httpClient.DefaultRequestHeaders.Add("Cookie", "YOUR_COOKIES_HERE");
```

## Cách hoạt động

1. **Khởi động**: Ứng dụng khởi động và kết nối đến API
2. **Lấy dữ liệu**: Gửi HTTP GET request đến API mỗi giây
3. **Parse dữ liệu**: Parse response JavaScript thành objects
4. **Hiển thị**: Cập nhật giao diện với dữ liệu mới

## Cấu trúc dữ liệu Response

Response API có format:
```javascript
$M('odds-display').onUpdate(2,[
  74748,
  1,
  1,
  [
    [leagues],        // Danh sách giải đấu
    [matches],        // Danh sách trận đấu  
    [markets],        // Danh sách markets
    ,,,
    [odds]           // Danh sách tỷ lệ cược
  ]
]);
```

## Giao diện

Giao diện bao gồm:

- **Header**: Hiển thị thời gian cập nhật
- **Bảng chính**: 
  - Cột "Thời Gian": Giờ thi đấu
  - Cột "Trận đấu": Tên đội nhà/đội khách
  - Cột "Nguyên trận": Các loại kèo cho cả trận
  - Cột "Hiệp 1": Các loại kèo cho hiệp 1
  - Cột "Nhiều": Các kèo bổ sung
- **Footer**: Trạng thái kết nối và số lượng trận đấu

## Màu sắc

- **Xanh đậm (#2E5090)**: Header
- **Đỏ**: Tỷ lệ cược âm
- **Đen**: Tỷ lệ cược dương
- **Xanh nhạt**: Background cho odds Home
- **Vàng nhạt**: Background cho odds Draw
- **Cam nhạt**: Background cho odds Away

## Troubleshooting

### Không kết nối được API
- Kiểm tra internet connection
- Kiểm tra URL API có chính xác không
- Kiểm tra cookies có hợp lệ không

### Không hiển thị dữ liệu
- Kiểm tra Console output để xem lỗi parse
- Đảm bảo response format đúng cấu trúc

### Ứng dụng chạy chậm
- Tăng interval của timer (từ 1s lên 2s hoặc 3s)
- Giảm số lượng trận đấu hiển thị

## License

Phần mềm này được tạo cho mục đích học tập và nghiên cứu.

## Contact

Nếu có vấn đề hoặc câu hỏi, vui lòng tạo issue trên repository.
