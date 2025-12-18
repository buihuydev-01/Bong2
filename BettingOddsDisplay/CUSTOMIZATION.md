# Hướng dẫn Tùy chỉnh (Customization Guide)

## 1. Thay đổi tần suất cập nhật

### Thay đổi interval (mỗi bao nhiêu giây)

File: `Services/OddsDataService.cs`

```csharp
// Tìm dòng này (khoảng dòng 66):
_timer = new Timer(async _ => await FetchDataAsync(), null, 1000, 1000);

// Thay đổi thành:
_timer = new Timer(async _ => await FetchDataAsync(), null, 2000, 2000);  // 2 giây
_timer = new Timer(async _ => await FetchDataAsync(), null, 5000, 5000);  // 5 giây
_timer = new Timer(async _ => await FetchDataAsync(), null, 500, 500);    // 0.5 giây
```

**Lưu ý**: 
- Giá trị tính bằng milliseconds (1000ms = 1 giây)
- Giá trị đầu tiên: delay trước lần chạy đầu tiên
- Giá trị thứ hai: interval giữa các lần chạy

## 2. Thay đổi URL API

### Thay đổi endpoint

File: `Services/OddsDataService.cs`

```csharp
// Tìm dòng này (khoảng dòng 75):
var url = "https://sports.wwyyuuvv22.com/web-root/restricted/odds-display/today-data.aspx?od-param=2,1,1,1,1,2,1,2,0&fi=1&v=131885&dl=0";

// Thay bằng URL mới:
var url = "YOUR_NEW_API_URL_HERE";
```

### Thay đổi query parameters

```csharp
// Ví dụ thay đổi sport type
var url = "https://...?od-param=2,1,1,1,1,2,1,2,0&fi=2&v=131885&dl=0";
                                                      ^^^ 
                                                    fi=2 (basketball)
```

## 3. Thay đổi Cookies và Headers

### Cập nhật cookies

File: `Services/OddsDataService.cs`

```csharp
// Tìm method SetupHttpClient() (dòng 20-40)
// Thay đổi cookies:
_httpClient.DefaultRequestHeaders.Add("Cookie", 
    "ASP.NET_SessionId=YOUR_SESSION_ID; " +
    "other_cookie=VALUE; " +
    // ... thêm cookies khác
);
```

### Thêm/sửa headers

```csharp
// Trong method SetupHttpClient()
_httpClient.DefaultRequestHeaders.Add("your-header", "value");

// Hoặc sửa existing header:
_httpClient.DefaultRequestHeaders.Remove("accept");
_httpClient.DefaultRequestHeaders.Add("accept", "application/json");
```

### Lấy cookies từ browser

**Chrome/Edge**:
1. F12 → Network tab
2. Reload trang
3. Click vào request đầu tiên
4. Headers → Request Headers → Cookie
5. Copy toàn bộ giá trị

**Firefox**:
1. F12 → Network
2. Tương tự như Chrome

## 4. Thay đổi màu sắc giao diện

### Thay đổi màu header

File: `MainWindow.xaml`

```xml
<!-- Tìm dòng này: -->
<Border Background="#2E5090" ...>

<!-- Thay thành: -->
<Border Background="#FF5722" ...>  <!-- Đỏ cam -->
<Border Background="#4CAF50" ...>  <!-- Xanh lá -->
<Border Background="#9C27B0" ...>  <!-- Tím -->
```

### Thay đổi màu odds boxes

```xml
<!-- Home odds -->
<Border Background="#E3F2FD" BorderBrush="#90CAF9" ...>
<!-- Thay thành màu xanh mint: -->
<Border Background="#E0F2F1" BorderBrush="#80CBC4" ...>

<!-- Away odds -->
<Border Background="#FFE0B2" BorderBrush="#FFAB91" ...>
<!-- Thay thành màu hồng nhạt: -->
<Border Background="#FCE4EC" BorderBrush="#F48FB1" ...>

<!-- Draw odds -->
<Border Background="#FFF9C4" BorderBrush="#FFF176" ...>
<!-- Thay thành màu xanh dương nhạt: -->
<Border Background="#E1F5FE" BorderBrush="#81D4FA" ...>
```

### Thay đổi màu text cho odds

File: `Converters/OddsColorConverter.cs`

```csharp
public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
{
    if (value is double odds)
    {
        if (odds > 0)
            return new SolidColorBrush(Colors.Black);      // Đổi thành Blue, Green, etc.
        else if (odds < 0)
            return new SolidColorBrush(Colors.Red);         // Đổi thành DarkRed, Crimson, etc.
    }
    return new SolidColorBrush(Colors.Black);
}
```

## 5. Thay đổi font và size

### Font size toàn bộ app

File: `MainWindow.xaml`

```xml
<!-- Thêm vào Window tag: -->
<Window ... FontSize="12">
  <!-- Hoặc thay đổi từng Style: -->
  
  <Style x:Key="HeaderTextStyle" TargetType="TextBlock">
    <Setter Property="FontSize" Value="14"/>  <!-- Thay 10 thành 14 -->
  </Style>
</Window>
```

### Font family

```xml
<Window ... FontFamily="Segoe UI">
<!-- Hoặc: -->
<Window ... FontFamily="Arial">
<Window ... FontFamily="Consolas">
<Window ... FontFamily="Tahoma">
```

## 6. Thay đổi layout và spacing

### Thay đổi chiều rộng cột

File: `MainWindow.xaml`

```xml
<Grid.ColumnDefinitions>
    <ColumnDefinition Width="70"/>   <!-- Thời Gian - tăng lên 100 -->
    <ColumnDefinition Width="180"/>  <!-- Trận đấu - tăng lên 250 -->
    <!-- ... -->
</Grid.ColumnDefinitions>
```

### Thay đổi padding và margin

```xml
<!-- Tăng khoảng cách giữa các elements: -->
<Border Padding="10,5">         <!-- Thay từ 5,3 -->
<StackPanel Margin="5,2">       <!-- Thay từ 2,1 -->
```

## 7. Ẩn/hiện các cột

### Ẩn cột "Nhiều"

File: `MainWindow.xaml`

```xml
<!-- Tìm cột "Nhiều" và set Visibility: -->
<TextBlock Grid.Column="8" Text="Nhiều" 
           Visibility="Collapsed"  <!-- Thêm dòng này -->
           Style="{StaticResource HeaderTextStyle}"/>
```

### Ẩn cột "Hiệp 1"

```xml
<!-- Set Visibility cho tất cả Grid.Column="5", "6", "7" -->
<Border Grid.Column="5" Visibility="Collapsed" ...>
<Border Grid.Column="6" Visibility="Collapsed" ...>
<Border Grid.Column="7" Visibility="Collapsed" ...>
```

## 8. Thay đổi text hiển thị

### Thay đổi tên cột

File: `MainWindow.xaml`

```xml
<!-- Tiếng Việt → Tiếng Anh -->
<TextBlock Text="Thời Gian" .../>  → <TextBlock Text="Time" .../>
<TextBlock Text="Trận đấu" .../>   → <TextBlock Text="Match" .../>
<TextBlock Text="Cược chấp" .../> → <TextBlock Text="Handicap" .../>
<TextBlock Text="Tài/Xỉu" .../>    → <TextBlock Text="O/U" .../>
```

### Thay đổi format hiển thị

File: `Models/OddsLine.cs`

```csharp
// Thay đổi format handicap:
public string HandicapDisplay
{
    get
    {
        if (Handicap == 0) return "0";        // Thay từ "0-0"
        if (Handicap > 0) return $"+{Handicap:0.0}";  // Thêm dấu +
        return $"{Handicap:0.0}";
    }
}

// Thay đổi format odds:
private string FormatOdds(double odds)
{
    if (odds == 0) return "-";              // Thay từ ""
    if (odds > 0) return $"{odds:0.00}";
    return $"({Math.Abs(odds):0.00})";      // Bỏ dấu âm, thêm ngoặc
}
```

## 9. Thêm sound effects (tùy chọn)

### Tạo SoundService

File: `Services/SoundService.cs`

```csharp
using System.Media;

public class SoundService
{
    private readonly SoundPlayer _player = new SoundPlayer();
    
    public void PlayUpdateSound()
    {
        _player.SoundLocation = "sounds/update.wav";
        _player.Play();
    }
    
    public void PlayErrorSound()
    {
        _player.SoundLocation = "sounds/error.wav";
        _player.Play();
    }
}
```

### Sử dụng trong MainWindow

```csharp
private readonly SoundService _soundService = new SoundService();

private void OnDataUpdated(BettingData data)
{
    _soundService.PlayUpdateSound();  // Thêm dòng này
    // ... rest of code
}
```

## 10. Thêm logging

### Tạo LogService

File: `Services/LogService.cs`

```csharp
using System;
using System.IO;

public class LogService
{
    private readonly string _logFile = "app.log";
    
    public void Log(string message)
    {
        var timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        var logEntry = $"[{timestamp}] {message}\n";
        File.AppendAllText(_logFile, logEntry);
    }
}
```

### Sử dụng

```csharp
private readonly LogService _logger = new LogService();

private void OnDataUpdated(BettingData data)
{
    _logger.Log($"Updated: {data.Matches.Count} matches");
    // ...
}
```

## 11. Export configuration ra file

### Tạo Settings class

File: `Models/AppSettings.cs`

```csharp
public class AppSettings
{
    public string ApiUrl { get; set; } = "";
    public int RefreshInterval { get; set; } = 1000;
    public string CookieString { get; set; } = "";
    
    public void Save(string path)
    {
        var json = JsonConvert.SerializeObject(this, Formatting.Indented);
        File.WriteAllText(path, json);
    }
    
    public static AppSettings Load(string path)
    {
        if (!File.Exists(path))
            return new AppSettings();
            
        var json = File.ReadAllText(path);
        return JsonConvert.DeserializeObject<AppSettings>(json) ?? new AppSettings();
    }
}
```

### Sử dụng

```csharp
// Load settings
var settings = AppSettings.Load("settings.json");
var service = new OddsDataService(settings);

// Save settings
settings.Save("settings.json");
```

## 12. Dark Mode

### Thêm resource dictionary

File: `Themes/DarkTheme.xaml`

```xml
<ResourceDictionary xmlns="...">
    <SolidColorBrush x:Key="BackgroundBrush" Color="#1E1E1E"/>
    <SolidColorBrush x:Key="ForegroundBrush" Color="#FFFFFF"/>
    <SolidColorBrush x:Key="HeaderBrush" Color="#2D2D30"/>
    <!-- ... more colors -->
</ResourceDictionary>
```

### Apply theme

File: `App.xaml`

```xml
<Application.Resources>
    <ResourceDictionary>
        <ResourceDictionary.MergedDictionaries>
            <ResourceDictionary Source="Themes/DarkTheme.xaml"/>
        </ResourceDictionary.MergedDictionaries>
    </ResourceDictionary>
</Application.Resources>
```

## Tips & Best Practices

1. **Backup trước khi thay đổi**: Copy file gốc ra thư mục khác
2. **Test sau mỗi thay đổi**: Chạy app để đảm bảo không lỗi
3. **Comment code**: Thêm comment giải thích tại sao thay đổi
4. **Version control**: Sử dụng Git để track changes
5. **Incremental changes**: Thay đổi từng phần nhỏ, không thay đổi quá nhiều cùng lúc

## Troubleshooting

### Sau khi thay đổi, app không build

```bash
# Clean và rebuild
dotnet clean
dotnet build
```

### UI không hiển thị đúng sau thay đổi XAML

- Kiểm tra XAML syntax
- Đảm bảo tất cả tags đóng đúng
- Kiểm tra binding paths

### Màu sắc không thay đổi

- Đảm bảo build lại project
- Clear bin/ và obj/ folders
- Restart Visual Studio nếu cần

## Support

Nếu cần hỗ trợ thêm về customization, tạo issue trên GitHub với:
- Mô tả những gì bạn muốn thay đổi
- Screenshots nếu có thể
- Code snippets đã thử
