# Architecture Documentation

## Tổng quan kiến trúc

Ứng dụng được xây dựng theo mô hình **MVVM (Model-View-ViewModel)** với WPF.

```
┌─────────────────────────────────────────────────────────────┐
│                        Presentation Layer                    │
│  ┌────────────────┐         ┌──────────────────────┐        │
│  │  MainWindow    │◄────────│  LeagueViewModel     │        │
│  │  (XAML + CS)   │         │                      │        │
│  └────────────────┘         └──────────────────────┘        │
│         │                              │                     │
│         │ Data Binding                 │                     │
│         ▼                              ▼                     │
└─────────────────────────────────────────────────────────────┘
          │                              │
          │                              │
┌─────────────────────────────────────────────────────────────┐
│                        Business Layer                        │
│  ┌────────────────────┐      ┌──────────────────────┐       │
│  │ OddsDataService    │◄─────│  ResponseParser      │       │
│  │ - HTTP Client      │      │  - Parse JS response │       │
│  │ - Timer            │      │  - Extract data      │       │
│  │ - Events           │      │                      │       │
│  └────────────────────┘      └──────────────────────┘       │
└─────────────────────────────────────────────────────────────┘
          │                              │
          │                              │
┌─────────────────────────────────────────────────────────────┐
│                          Data Layer                          │
│  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐      │
│  │   League     │  │    Match     │  │  OddsGroup   │      │
│  │   Model      │  │    Model     │  │  OddsLine    │      │
│  └──────────────┘  └──────────────┘  └──────────────┘      │
└─────────────────────────────────────────────────────────────┘
```

## Layers chi tiết

### 1. Presentation Layer (View)

**MainWindow.xaml**
- Giao diện chính của ứng dụng
- Sử dụng XAML để định nghĩa UI
- Data binding với ViewModels
- Converters để format dữ liệu

**MainWindow.xaml.cs**
- Code-behind cho MainWindow
- Xử lý events từ Service layer
- Cập nhật UI trên UI thread (Dispatcher)

**Converters**
- `OddsColorConverter`: Chuyển đổi giá trị odds thành màu sắc
- `StringVisibilityConverter`: Hiển thị/ẩn element dựa vào string

### 2. Business Layer (Services)

**OddsDataService**
```
Responsibilities:
- Quản lý HTTP client
- Setup headers và cookies
- Timer để auto-refresh
- Gọi API mỗi giây
- Raise events khi có data mới
- Error handling

Events:
- DataUpdated: Khi có dữ liệu mới
- ErrorOccurred: Khi có lỗi

Methods:
- Start(): Bắt đầu auto-refresh
- Stop(): Dừng auto-refresh
- FetchDataAsync(): Lấy dữ liệu từ API
```

**ResponseParser**
```
Responsibilities:
- Parse JavaScript response
- Extract JSON từ function call
- Convert JSON array thành objects
- Map relationships (leagues, matches, odds)

Input:
$M('odds-display').onUpdate(2,[...]);

Output:
BettingData object với:
- List<League>
- List<Match> (với OddsGroups)
```

### 3. Data Layer (Models)

**BettingData**
```csharp
class BettingData
{
    List<League> Leagues
    List<Match> Matches
    string UpdateTime
}
```

**League**
```csharp
class League
{
    int LeagueId
    string Name
    string SubLeague
    string MatchInfo
}
```

**Match**
```csharp
class Match
{
    long MatchId
    int LeagueId
    string HomeTeam
    string AwayTeam
    DateTime MatchTime
    int Status
    long MarketId
    List<OddsGroup> OddsGroups
}
```

**OddsGroup & OddsLine**
```csharp
class OddsGroup
{
    int BetType  // 1=Handicap, 5=1X2, 9=O/U, etc.
    List<OddsLine> Lines
}

class OddsLine
{
    long OddsId
    double Handicap
    double HomeOdds
    double AwayOdds
    double DrawOdds (for 1X2)
    int Priority
}
```

## Data Flow

### 1. Khởi động ứng dụng

```
User launches app
    ↓
MainWindow.Loaded
    ↓
OddsDataService.Start()
    ↓
Timer starts (1000ms interval)
    ↓
FetchDataAsync() called immediately
```

### 2. Fetch & Parse cycle

```
Timer triggers
    ↓
FetchDataAsync()
    ↓
HTTP GET request to API
    ↓
Receive JavaScript response
    ↓
ResponseParser.ParseResponse()
    ↓
Extract & parse data
    ↓
Create BettingData object
    ↓
Raise DataUpdated event
    ↓
MainWindow receives event
    ↓
Dispatcher.Invoke() to UI thread
    ↓
Update ObservableCollection
    ↓
WPF data binding updates UI
```

### 3. Parsing flow

```
JavaScript response
    ↓
Regex extract JSON array
    ↓
JArray.Parse()
    ↓
Parse leagues (index 3)
    ↓
Parse matches (index 4)
    ↓
Parse markets (index 5) → Link to matches
    ↓
Parse odds (index 6) → Link to matches
    ↓
Group odds by BetType
    ↓
Sort by Priority
    ↓
Return BettingData
```

## Component Dependencies

```
MainWindow
  ├── depends on → OddsDataService
  ├── depends on → LeagueViewModel
  └── depends on → Converters

OddsDataService
  ├── depends on → HttpClient
  ├── depends on → Timer
  └── depends on → ResponseParser

ResponseParser
  ├── depends on → Newtonsoft.Json
  └── depends on → Models (all)

ViewModels
  └── depends on → Models
```

## Threading Model

- **UI Thread**: Chạy MainWindow và tất cả UI operations
- **Timer Thread**: Chạy FetchDataAsync() mỗi giây
- **Network Thread**: Async HTTP requests
- **Synchronization**: Dispatcher.Invoke() để update UI từ background threads

## Error Handling Strategy

### Network Errors
```csharp
try {
    var response = await _httpClient.GetStringAsync(url);
}
catch (HttpRequestException ex) {
    ErrorOccurred?.Invoke($"Network error: {ex.Message}");
}
catch (TaskCanceledException ex) {
    ErrorOccurred?.Invoke("Request timeout");
}
```

### Parsing Errors
```csharp
try {
    var data = JArray.Parse(jsonString);
}
catch (JsonException ex) {
    Console.WriteLine($"Parse error: {ex.Message}");
    return new BettingData(); // Empty data
}
```

### UI Update Errors
```csharp
try {
    UpdateLeagueData(data);
}
catch (Exception ex) {
    StatusText.Text = $"UI error: {ex.Message}";
}
```

## Performance Considerations

### 1. Data Binding
- Sử dụng `ObservableCollection` cho auto-update
- Clear và rebuild thay vì update từng item (đơn giản hơn)

### 2. Network
- Reuse HttpClient instance
- Async/await pattern
- 1 second interval (có thể tùy chỉnh)

### 3. Memory
- Dispose HttpClient khi đóng app
- Stop timer khi đóng app
- No memory leaks với proper event unsubscription

### 4. UI Rendering
- ScrollViewer cho danh sách dài
- Virtualization nếu cần (có thể thêm VirtualizingStackPanel)

## Extensibility Points

### 1. Thêm loại kèo mới
- Thêm case mới trong `ResponseParser`
- Update UI template trong XAML

### 2. Thêm API source khác
- Tạo interface `IOddsDataService`
- Implement cho mỗi source
- Factory pattern để chọn source

### 3. Thêm database
- Tạo `DatabaseService`
- Save data sau mỗi update
- Query historical data

### 4. Thêm notification
- Tạo `NotificationService`
- Subscribe to DataUpdated event
- Compare odds changes
- Show notifications

## Testing Strategy

### Unit Tests
- Test `ResponseParser` với sample data
- Test `OddsLine` formatting methods
- Test converters

### Integration Tests
- Test `OddsDataService` với mock HTTP
- Test full data flow

### UI Tests
- Test data binding
- Test UI updates
- Test error scenarios

## Configuration

### App Settings (có thể thêm)
```xml
<configuration>
  <appSettings>
    <add key="ApiUrl" value="https://..."/>
    <add key="RefreshInterval" value="1000"/>
    <add key="RequestTimeout" value="30000"/>
  </appSettings>
</configuration>
```

### User Settings (có thể thêm)
- Favorite leagues
- UI preferences
- Notification settings

## Deployment

### Prerequisites
- .NET 8.0 Runtime (hoặc self-contained)
- Windows 10/11

### Build
```bash
dotnet publish -c Release -r win-x64 --self-contained true
```

### Output
- Single .exe file (nếu dùng PublishSingleFile)
- Hoặc folder với dependencies

## Future Improvements

1. **Configuration UI**: Settings window
2. **Database**: SQLite cho historical data
3. **Analytics**: Charts và statistics
4. **Multiple sources**: Aggregate từ nhiều API
5. **Mobile/Web**: Xamarin hoặc Blazor version
