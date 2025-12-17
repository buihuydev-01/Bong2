# Sports Odds Viewer - Ứng dụng xem tỷ lệ cược thể thao

Ứng dụng WPF hiển thị và cập nhật tự động thông tin các trận đấu bóng đá và tỷ lệ cược từ API.

## Tính năng

- ✅ Tự động cập nhật dữ liệu mỗi 1 giây
- ✅ Hiển thị thông tin trận đấu: thời gian, đội bóng, giải đấu
- ✅ Hiển thị tỷ lệ cược: Handicap (Cược chấp), Over/Under (Tài/Xỉu), 1X2
- ✅ Giao diện đẹp mắt với bảng dữ liệu có màu sắc
- ✅ Có thể tạm dừng/tiếp tục cập nhật tự động
- ✅ Nút làm mới dữ liệu thủ công
- ✅ Hiển thị trạng thái kết nối và thời gian cập nhật

## Yêu cầu hệ thống

- Windows 10/11
- .NET 8.0 SDK hoặc mới hơn

## Cài đặt

### 1. Cài đặt .NET SDK

Tải và cài đặt .NET 8.0 SDK từ: https://dotnet.microsoft.com/download

### 2. Build ứng dụng

```bash
cd SportsOddsViewer
dotnet restore
dotnet build
```

### 3. Chạy ứng dụng

```bash
dotnet run
```

Hoặc build file executable:

```bash
dotnet publish -c Release -r win-x64 --self-contained
```

File .exe sẽ được tạo trong thư mục: `bin/Release/net8.0-windows/win-x64/publish/`

## Cách sử dụng

1. **Khởi động ứng dụng**: Chạy file SportsOddsViewer.exe
2. **Xem dữ liệu**: Ứng dụng sẽ tự động tải và hiển thị dữ liệu các trận đấu
3. **Tự động cập nhật**: Dữ liệu được cập nhật tự động mỗi 1 giây
4. **Tạm dừng**: Click nút "Tạm dừng" để ngừng cập nhật tự động
5. **Làm mới**: Click nút "Làm mới ngay" để cập nhật dữ liệu ngay lập tức

## Cấu trúc dữ liệu

### Bảng hiển thị các cột:

- **Thời gian**: Giờ diễn ra trận đấu
- **Trạng thái**: Sắp diễn ra / Live / Kết thúc
- **Giải đấu**: Tên giải đấu
- **Đội nhà**: Tên đội chủ nhà
- **Đội khách**: Tên đội khách
- **Cược chấp**: Tỷ lệ cược handicap (đỏ: nhà, xanh: khách)
- **Tài/Xỉu**: Tỷ lệ Over/Under (đỏ: tài, xanh: xỉu)
- **1X2**: Tỷ lệ cược 3 cửa (đỏ: nhà, xanh lá: hòa, xanh: khách)

## API

Ứng dụng sử dụng API từ:
```
https://sports.wwyyuuvv22.com/web-root/restricted/odds-display/today-data.aspx
```

## Công nghệ sử dụng

- **Framework**: .NET 8.0 WPF
- **Language**: C# 12
- **HTTP Client**: System.Net.Http
- **UI**: WPF với XAML
- **Data Binding**: ObservableCollection với INotifyPropertyChanged

## Cấu trúc project

```
SportsOddsViewer/
├── Models/
│   └── Match.cs              # Model cho trận đấu
├── Services/
│   └── ApiService.cs         # Service gọi API và parse dữ liệu
├── MainWindow.xaml           # Giao diện chính
├── MainWindow.xaml.cs        # Code-behind
├── App.xaml                  # Application definition
├── App.xaml.cs               # Application code
└── SportsOddsViewer.csproj   # Project file
```

## Lưu ý

- Ứng dụng cần kết nối Internet để lấy dữ liệu
- Thời gian cập nhật có thể điều chỉnh trong code (hiện tại: 1 giây)
- API có thể thay đổi định dạng, cần cập nhật parser nếu có lỗi

## Tùy chỉnh

### Thay đổi thời gian cập nhật

Trong file `MainWindow.xaml.cs`, tìm dòng:

```csharp
Interval = TimeSpan.FromSeconds(1)
```

Thay đổi số giây theo nhu cầu (ví dụ: 2 giây, 5 giây...)

### Thay đổi màu sắc giao diện

Trong file `MainWindow.xaml`, tùy chỉnh các giá trị:
- Background colors
- Foreground colors
- Border colors

## Troubleshooting

### Lỗi kết nối API
- Kiểm tra kết nối Internet
- Kiểm tra xem API có thay đổi URL không
- Kiểm tra firewall/antivirus có chặn ứng dụng không

### Không hiển thị dữ liệu
- Kiểm tra format response từ API
- Xem log trong StatusText
- Debug bằng cách xem Console output

## License

MIT License - Tự do sử dụng và chỉnh sửa

## Tác giả

Phát triển bởi AI Assistant
