# CÁC HOẠT ĐỘNG CỦA ỨNG DỤNG

## 📊 SƠ ĐỒ LUỒNG DỮ LIỆU

```
┌─────────────────────────────────────────────────────────────┐
│                      USER INTERFACE                          │
│  ┌────────────────────────────────────────────────────────┐ │
│  │ MainWindow.xaml (XAML UI)                              │ │
│  │  - DataGrid: Hiển thị bảng dữ liệu                     │ │
│  │  - Header: Tiêu đề, nút điều khiển                     │ │
│  │  - Footer: Trạng thái, số lượng trận                   │ │
│  └────────────────────────────────────────────────────────┘ │
│                           ↕                                  │
│  ┌────────────────────────────────────────────────────────┐ │
│  │ MainWindow.xaml.cs (Code-behind)                       │ │
│  │  - Timer: Cập nhật mỗi 1 giây                          │ │
│  │  - ObservableCollection<Match>: Lưu trữ dữ liệu       │ │
│  │  - Event Handlers: Xử lý click, update                 │ │
│  └────────────────────────────────────────────────────────┘ │
└──────────────────────────────┬───────────────────────────────┘
                               ↕
┌─────────────────────────────────────────────────────────────┐
│                      SERVICE LAYER                           │
│  ┌────────────────────────────────────────────────────────┐ │
│  │ ApiService.cs                                          │ │
│  │  - HttpClient: Gọi API                                 │ │
│  │  - ParseResponse(): Parse dữ liệu JavaScript          │ │
│  │  - ParseOdds(): Parse tỷ lệ cược                       │ │
│  │  - DecodeString(): Decode hex/unicode                  │ │
│  └────────────────────────────────────────────────────────┘ │
└──────────────────────────────┬───────────────────────────────┘
                               ↕
┌─────────────────────────────────────────────────────────────┐
│                       DATA LAYER                             │
│  ┌────────────────────────────────────────────────────────┐ │
│  │ Match.cs (Model)                                       │ │
│  │  - Properties: Time, Teams, Odds, etc.                │ │
│  │  - INotifyPropertyChanged: Tự động update UI          │ │
│  └────────────────────────────────────────────────────────┘ │
└──────────────────────────────┬───────────────────────────────┘
                               ↕
┌─────────────────────────────────────────────────────────────┐
│                      EXTERNAL API                            │
│  https://sports.wwyyuuvv22.com/.../today-data.aspx         │
│  Response: JavaScript function với JSON data                │
└─────────────────────────────────────────────────────────────┘
```

## 🔄 QUY TRÌNH CẬP NHẬT DỮ LIỆU

### 1. Khởi động ứng dụng
```
App.xaml.cs (Startup)
    ↓
MainWindow Constructor
    ↓
Khởi tạo ApiService
    ↓
Khởi tạo ObservableCollection<Match>
    ↓
Thiết lập Timer (1 second interval)
    ↓
Gọi LoadDataAsync() lần đầu
```

### 2. Timer Tick (Mỗi 1 giây)
```
Timer.Tick Event
    ↓
Kiểm tra: AutoUpdate có bật?
    ↓ (Yes)
Kiểm tra: Có đang update không?
    ↓ (No)
Gọi LoadDataAsync()
```

### 3. Load Data Flow
```
LoadDataAsync()
    ↓
Set _isUpdating = true
    ↓
Hiển thị "Đang tải dữ liệu..."
    ↓
ApiService.FetchMatchesAsync()
    ↓
    ├─→ HttpClient.GetStringAsync(API_URL)
    │       ↓
    │   Nhận Response (JavaScript string)
    │       ↓
    │   ParseResponse(response)
    │       ↓
    │       ├─→ Parse Leagues (Regex)
    │       │       ↓
    │       │   Dictionary<LeagueId, LeagueName>
    │       │
    │       ├─→ Parse Matches (Regex)
    │       │       ↓
    │       │   List<Match> với basic info
    │       │
    │       └─→ ParseOdds(response, matches)
    │               ↓
    │           Cập nhật odds vào matches
    │
    └─→ Return List<Match>
        ↓
Update ObservableCollection
    ↓
    ├─→ Match đã tồn tại? → UpdateMatch()
    │
    └─→ Match mới? → Add to collection
        ↓
Xóa matches không còn trong API
    ↓
Sắp xếp theo thời gian
    ↓
Update UI (tự động qua data binding)
    ↓
Hiển thị "Cập nhật lúc: HH:mm:ss"
    ↓
Set _isUpdating = false
```

## 🎯 CHI TIẾT CÁC THÀNH PHẦN

### 1. Match Model (Match.cs)

**Purpose**: Lưu trữ thông tin 1 trận đấu

**Properties**:
```csharp
- EventId: int              // ID duy nhất của trận
- LeagueId: int             // ID giải đấu
- Time: string              // Thời gian (HH:mm)
- Status: string            // Trạng thái (Live, Sắp diễn ra, ...)
- HomeTeam: string          // Tên đội nhà
- AwayTeam: string          // Tên đội khách
- League: string            // Tên giải đấu
- Score: string             // Tỷ số
- Odds1X2Home: string       // Tỷ lệ đội nhà thắng
- Odds1X2Draw: string       // Tỷ lệ hòa
- Odds1X2Away: string       // Tỷ lệ đội khách thắng
- OddsOU: string            // Line over/under
- OddsOUOver: string        // Tỷ lệ tài
- OddsOUUnder: string       // Tỷ lệ xỉu
- OddsHDPHome: string       // Tỷ lệ chấp đội nhà
- OddsHDPLine: string       // Line chấp
- OddsHDPAway: string       // Tỷ lệ chấp đội khách
```

**Implements**: `INotifyPropertyChanged`
- Khi property thay đổi → Tự động update UI

### 2. ApiService (ApiService.cs)

**Purpose**: Gọi API và parse dữ liệu

**Methods**:

#### FetchMatchesAsync()
```csharp
public async Task<List<Match>> FetchMatchesAsync()
{
    // 1. Gọi API với HttpClient
    // 2. Nhận response (JavaScript string)
    // 3. Parse response
    // 4. Return danh sách Match
}
```

#### ParseResponse(string response)
```csharp
private List<Match> ParseResponse(string response)
{
    // 1. Parse leagues từ response
    //    Pattern: [leagueId,'leagueName',...]
    //    
    // 2. Parse matches từ response
    //    Pattern: [eventId,sportType,leagueId,'home','away',...]
    //    
    // 3. Parse odds
    //    
    // 4. Return danh sách Match đầy đủ
}
```

#### ParseOdds(string response, List<Match> matches)
```csharp
private void ParseOdds(string response, List<Match> matches)
{
    // Parse các loại odds:
    // - betType 1: Handicap (HDP)
    // - betType 3: Over/Under (OU)
    // - betType 5: 1X2
    //
    // Cập nhật vào Match tương ứng
}
```

#### DecodeString(string encoded)
```csharp
private string DecodeString(string encoded)
{
    // Decode \xHH (hex) → character
    // Decode \uHHHH (unicode) → character
    // Ví dụ: \xC3\xBAp → Cúp
}
```

### 3. MainWindow (MainWindow.xaml.cs)

**Purpose**: Logic xử lý UI và cập nhật dữ liệu

**Fields**:
```csharp
- _apiService: ApiService              // Service gọi API
- _timer: DispatcherTimer              // Timer cập nhật
- _matches: ObservableCollection       // Dữ liệu hiển thị
- _isUpdating: bool                    // Flag đang update
```

**Methods**:

#### Constructor
```csharp
public MainWindow()
{
    InitializeComponent();
    
    // Khởi tạo service, collection
    // Setup timer (1 second)
    // Bind data
    // Load dữ liệu ban đầu
}
```

#### Timer_Tick
```csharp
private async void Timer_Tick(object? sender, EventArgs e)
{
    // Nếu AutoUpdate ON và không đang update
    // → Gọi LoadDataAsync()
}
```

#### LoadDataAsync
```csharp
private async Task LoadDataAsync()
{
    // 1. Set flag đang update
    // 2. Hiển thị "Đang tải..."
    // 3. Gọi ApiService.FetchMatchesAsync()
    // 4. Update ObservableCollection:
    //    - Match cũ → Update
    //    - Match mới → Add
    //    - Match không còn → Remove
    // 5. Sắp xếp
    // 6. Update UI status
    // 7. Clear flag
}
```

#### UpdateMatch
```csharp
private void UpdateMatch(Match existing, Match newData)
{
    // Copy tất cả properties từ newData sang existing
    // INotifyPropertyChanged sẽ tự động update UI
}
```

#### Button Events
```csharp
RefreshButton_Click()
    → Gọi LoadDataAsync() ngay

AutoUpdateToggle_Click()
    → Toggle ON/OFF auto-update
    → Đổi text button
    → Đổi màu button
```

### 4. MainWindow.xaml (UI)

**Structure**:
```xml
<Window>
  <Grid>
    <Grid.RowDefinitions>
      <Row 0: Header />
      <Row 1: DataGrid />
      <Row 2: Footer />
    </Grid.RowDefinitions>
    
    <!-- Header -->
    <Border Grid.Row="0">
      <TextBlock: Title />
      <TextBlock: Last Update Time />
      <Button: Refresh />
      <ToggleButton: Auto Update />
    </Border>
    
    <!-- DataGrid -->
    <DataGrid Grid.Row="1" ItemsSource="{Binding}">
      <Columns>
        <Time />
        <Status />
        <League />
        <HomeTeam />
        <AwayTeam />
        <HDP Odds />
        <OU Odds />
        <1X2 Odds />
      </Columns>
    </DataGrid>
    
    <!-- Footer -->
    <Border Grid.Row="2">
      <TextBlock: Status />
      <TextBlock: Match Count />
    </Border>
  </Grid>
</Window>
```

**Data Binding**:
```
ObservableCollection<Match>
    ↓ (Binding)
DataGrid.ItemsSource
    ↓ (Auto-generate rows)
UI Display
```

**Styling**:
- Header: Blue background, white text
- DataGrid: Light blue background, alternating rows
- Odds: Red (home/over), Blue (away/under), Green (draw)

## 🎨 DESIGN PATTERNS

### 1. MVVM-like Pattern
```
View (XAML)
    ↕ (Data Binding)
ViewModel-like (Code-behind)
    ↕ (Calls)
Service Layer (ApiService)
    ↕ (Uses)
Model (Match)
```

### 2. Observer Pattern
```
Match (Observable)
    implements INotifyPropertyChanged
        ↓
    PropertyChanged event
        ↓
    WPF Binding System
        ↓
    Auto-update UI
```

### 3. Timer Pattern
```
DispatcherTimer
    ↓ (Every 1 second)
Timer_Tick Event
    ↓
LoadDataAsync()
    ↓
Update Collection
    ↓
UI Auto-refresh
```

## 🔍 REGEX PATTERNS USED

### 1. Parse Leagues
```regex
\[(\d+),'([^']+)'
```
**Matches**: `[39,'Cúp Tây Ban Nha',...]`
- Group 1: League ID (39)
- Group 2: League Name (Cúp Tây Ban Nha)

### 2. Parse Matches
```regex
\[(\d+),1,(\d+),'([^']+)','([^']+)','([^']+)',(\d+),'([^']+)',
```
**Matches**: `[9183877,1,39,'Team1','Team2','code',10,'12/18/2025 01:00',...]`
- Group 1: Event ID
- Group 2: League ID
- Group 3: Home Team
- Group 4: Away Team
- Group 5: Match Code
- Group 6: Status Code
- Group 7: Date Time

### 3. Parse Odds
```regex
\[\d+,\[(\d+),(\d+),\d+,[^\]]+\],\[([^\]]+)\]\]
```
**Matches**: `[5715744360,[113397115,1,0,...],[0.86,0.78]]`
- Group 1: Event ID
- Group 2: Bet Type (1=HDP, 3=OU, 5=1X2)
- Group 3: Odds Values

### 4. Decode Hex
```regex
\\x([0-9A-Fa-f]{2})
```
**Matches**: `\xC3\xBA`
- Convert hex to character

### 5. Decode Unicode
```regex
\\u([0-9A-Fa-f]{4})
```
**Matches**: `\u1ED9`
- Convert unicode to character

## 💾 DATA FLOW EXAMPLE

### Input (API Response):
```javascript
$M('odds-display').onUpdate(2,[
  [
    [39,'C\xFAp T\xE2y Ban Nha','',''],
    [51,'INTERCONTINENTAL CUP','','']
  ],
  [
    [9183877,1,39,'Cultural Leonesa','Levante','code',10,'12/18/2025 01:00',0,'',19,1,66609718,0]
  ],
  [
    [113397115,9183877,0,0,0,19]
  ],
  [
    [5715744360,[113397115,1,0,5000.00,0.00],[0.86,0.78]]
  ]
]);
```

### Processing:
```
1. Parse Leagues:
   39 → "Cúp Tây Ban Nha"
   51 → "INTERCONTINENTAL CUP"

2. Parse Match:
   EventId: 9183877
   LeagueId: 39
   HomeTeam: "Cultural Leonesa"
   AwayTeam: "Levante"
   Time: "01:00"
   Status: "Sắp diễn ra" (code 10)

3. Parse Odds:
   Match 9183877:
   - HDP: 0.86, 0.78
   
4. Combine:
   Match {
     EventId = 9183877,
     League = "Cúp Tây Ban Nha",
     HomeTeam = "Cultural Leonesa",
     AwayTeam = "Levante",
     Time = "01:00",
     Status = "Sắp diễn ra",
     OddsHDPHome = "0.86",
     OddsHDPAway = "0.78"
   }
```

### Output (UI Display):
```
┌────────┬──────────────┬───────────────────┬──────────────────┬─────────┬─────────┐
│  01:00 │ Sắp diễn ra  │ Cúp Tây Ban Nha   │ Cultural Leonesa │ Levante │  0.86   │
│        │              │                   │                  │         │  0.78   │
└────────┴──────────────┴───────────────────┴──────────────────┴─────────┴─────────┘
```

## 🚀 PERFORMANCE

### Optimizations:
1. **Async/Await**: Không block UI thread
2. **ObservableCollection**: Chỉ update items thay đổi
3. **INotifyPropertyChanged**: Chỉ update properties thay đổi
4. **Regex Caching**: Compile regex 1 lần
5. **Dictionary Lookup**: O(1) cho league lookup

### Memory:
- Mỗi Match: ~500 bytes
- 100 matches: ~50 KB
- Total app: ~10-20 MB

### Network:
- Request frequency: 1 second
- Response size: ~100-500 KB
- Bandwidth: ~1-5 Mbps

## ✨ FUTURE IMPROVEMENTS

1. **Caching**: Cache dữ liệu để giảm API calls
2. **WebSocket**: Real-time updates thay vì polling
3. **Filtering**: Lọc theo giải đấu, đội bóng
4. **Sorting**: Sắp xếp theo nhiều tiêu chí
5. **Export**: Xuất dữ liệu ra Excel/CSV
6. **Notifications**: Thông báo khi có trận Live
7. **Multi-language**: Hỗ trợ nhiều ngôn ngữ
8. **Themes**: Dark mode, custom themes
9. **History**: Lưu lịch sử tỷ lệ
10. **Charts**: Biểu đồ thay đổi tỷ lệ

## 🎓 KẾT LUẬN

Ứng dụng được thiết kế theo mô hình:
- **Separation of Concerns**: UI / Logic / Data riêng biệt
- **Data Binding**: Tự động sync UI với data
- **Async Programming**: Không block UI
- **Clean Code**: Dễ đọc, dễ bảo trì
- **Extensible**: Dễ mở rộng thêm tính năng

Phù hợp cho:
- Học WPF development
- Real-time data display
- API integration
- MVVM pattern practice
