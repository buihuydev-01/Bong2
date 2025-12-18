# Quick Start Guide

## Yêu cầu

- .NET 8.0 SDK ([Tải tại đây](https://dotnet.microsoft.com/download/dotnet/8.0))
- Windows 10/11
- Visual Studio 2022 (tùy chọn, nhưng khuyến nghị)

## Chạy ứng dụng

### Cách 1: Visual Studio (Đơn giản nhất)

1. Mở file `BettingOddsDisplay.sln`
2. Nhấn F5 hoặc click nút "Start"
3. Ứng dụng sẽ tự động build và chạy

### Cách 2: Command Line

```bash
# Bước 1: Di chuyển vào thư mục project
cd BettingOddsDisplay

# Bước 2: Restore dependencies
dotnet restore

# Bước 3: Build project
dotnet build

# Bước 4: Chạy ứng dụng
dotnet run
```

### Cách 3: Build Release

```bash
cd BettingOddsDisplay
dotnet publish -c Release -r win-x64 --self-contained true /p:PublishSingleFile=true

# File .exe sẽ ở: bin/Release/net8.0-windows/win-x64/publish/BettingOddsDisplay.exe
```

## Kiểm tra cài đặt

Kiểm tra .NET SDK đã cài đặt chưa:

```bash
dotnet --version
```

Kết quả phải là 8.0.x hoặc cao hơn.

## Khi ứng dụng chạy

Bạn sẽ thấy:

1. **Header xanh**: Hiển thị "Cập nhật tự động mỗi giây" và thời gian cập nhật
2. **Bảng chính**: Hiển thị các trận đấu nhóm theo giải đấu
3. **Footer xanh**: Hiển thị trạng thái kết nối và số lượng trận

## Tùy chỉnh

### Thay đổi tần suất cập nhật

Mở `Services/OddsDataService.cs`, dòng ~66:

```csharp
// Thay 1000 (1 giây) thành 2000 (2 giây) hoặc 5000 (5 giây)
_timer = new Timer(async _ => await FetchDataAsync(), null, 1000, 1000);
                                                            ^^^^  ^^^^ 
                                                          Initial  Interval
```

### Thay đổi URL API

Mở `Services/OddsDataService.cs`, dòng ~75:

```csharp
var url = "YOUR_NEW_URL_HERE";
```

### Thay đổi Cookies

Mở `Services/OddsDataService.cs`, dòng ~33-38:

```csharp
_httpClient.DefaultRequestHeaders.Add("Cookie", "YOUR_COOKIES_HERE");
```

## Troubleshooting

### Lỗi: .NET 8.0 not found

**Giải pháp**: Cài đặt .NET 8.0 SDK từ https://dotnet.microsoft.com/download/dotnet/8.0

### Lỗi: Cannot connect to API

**Nguyên nhân**: 
- Không có internet
- URL sai
- Cookies hết hạn

**Giải pháp**:
1. Kiểm tra internet
2. Lấy cookies mới từ browser (F12 > Network > Copy cookies)
3. Update cookies trong `OddsDataService.cs`

### Lỗi: No data displayed

**Nguyên nhân**: Response format khác với parser

**Giải pháp**:
1. Check Console output (nếu chạy từ command line)
2. Debug parser trong `Services/ResponseParser.cs`

### Lỗi: Build failed

```bash
# Clean và rebuild
dotnet clean
dotnet restore
dotnet build
```

## Xem logs

Chạy từ command line để xem logs:

```bash
cd BettingOddsDisplay
dotnet run
```

Mọi error sẽ được in ra console.

## Tính năng đã có

✅ Auto-refresh mỗi 1 giây  
✅ Hiển thị đầy đủ odds: Handicap, Over/Under, 1X2  
✅ Màu sắc trực quan  
✅ Nhóm theo giải đấu  
✅ Hiển thị thời gian cập nhật  
✅ Scroll khi có nhiều trận  

## Tính năng có thể mở rộng

- 🔲 Filter theo giải đấu
- 🔲 Favorite matches
- 🔲 Notification khi odds thay đổi
- 🔲 Export data to Excel
- 🔲 Historical odds tracking
- 🔲 Multiple API sources

## Support

Nếu gặp vấn đề, tạo issue với thông tin:
- Phiên bản .NET: `dotnet --version`
- OS version
- Error message đầy đủ
- Steps to reproduce
