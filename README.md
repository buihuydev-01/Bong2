# Sports Odds Viewer - WPF Application

## 📖 Tổng quan / Overview

Ứng dụng WPF hiển thị và cập nhật tự động thông tin các trận đấu bóng đá cùng tỷ lệ cược từ API mỗi 1 giây.

A WPF application that automatically displays and updates football match information and betting odds from an API every second.

## 🎯 Tính năng / Features

- ✅ Tự động cập nhật dữ liệu mỗi 1 giây / Auto-refresh every 1 second
- ✅ Hiển thị thông tin trận đấu / Display match information
- ✅ Hiển thị tỷ lệ cược (HDP, OU, 1X2) / Show betting odds
- ✅ Giao diện đẹp mắt / Beautiful UI
- ✅ Tạm dừng/Tiếp tục cập nhật / Pause/Resume updates
- ✅ Làm mới thủ công / Manual refresh

## 📸 Screenshots

Ứng dụng hiển thị bảng dữ liệu với các cột:
- Thời gian trận đấu
- Trạng thái (Live, Sắp diễn ra, Kết thúc)
- Giải đấu
- Đội nhà / Đội khách
- Tỷ lệ cược: Handicap, Over/Under, 1X2

## 🚀 Cài đặt / Installation

### Yêu cầu / Requirements
- Windows 10/11
- .NET 8.0 SDK

### Build và chạy / Build and Run

```bash
cd SportsOddsViewer

# Restore packages
dotnet restore

# Build
dotnet build -c Release

# Run
dotnet run
```

Hoặc sử dụng các file batch / Or use batch files:
- `build.bat` - Build project
- `run.bat` - Run application
- `publish.bat` - Create standalone .exe

## 📚 Tài liệu / Documentation

- [README.md (English)](./SportsOddsViewer/README.md)
- [HUONG_DAN.md (Tiếng Việt)](./SportsOddsViewer/HUONG_DAN.md)

## 🏗️ Cấu trúc / Structure

```
SportsOddsViewer/
├── Models/          # Data models
├── Services/        # API service
├── MainWindow.xaml  # UI
├── *.bat            # Build scripts
└── README.md        # Documentation
```

## 🛠️ Công nghệ / Technology

- **Framework**: .NET 8.0 WPF
- **Language**: C# 12
- **UI**: XAML
- **HTTP**: System.Net.Http
- **Data Binding**: ObservableCollection + INotifyPropertyChanged

## 📝 API

API được sử dụng / API used:
```
https://sports.wwyyuuvv22.com/web-root/restricted/odds-display/today-data.aspx
```

Response format: JavaScript function call với dữ liệu JSON

## 🎨 Giao diện / UI

- Header: Logo, thời gian cập nhật, nút điều khiển
- DataGrid: Bảng dữ liệu trận đấu với tỷ lệ cược
- Footer: Trạng thái kết nối, số lượng trận

## 🔧 Tùy chỉnh / Customization

### Thay đổi thời gian cập nhật / Change refresh interval

File: `MainWindow.xaml.cs`
```csharp
Interval = TimeSpan.FromSeconds(1)  // Change to 2, 3, 5...
```

### Thay đổi màu sắc / Change colors

File: `MainWindow.xaml` - Tìm các section Style và thay đổi giá trị màu

## ⚠️ Lưu ý / Notes

- Cần kết nối Internet / Internet connection required
- API có thể thay đổi / API may change
- Chỉ dùng để tham khảo / For reference only

## 📄 License

MIT License

---

**Tác giả / Author**: AI Assistant  
**Ngày tạo / Created**: December 2024
