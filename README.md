# Sports Odds Viewer - WPF Application

## 📖 Tổng quan / Overview

Ứng dụng WPF hiển thị tỷ lệ cược bóng đá theo thời gian thực với **multi-row display** - mỗi trận đấu có nhiều dòng tương ứng với các mức stake khác nhau. Dữ liệu tự động cập nhật mỗi 1 giây.

A WPF application that displays football betting odds in real-time with **multi-row display** - each match has multiple rows corresponding to different stake levels. Data auto-updates every second.

---

## ✨ Tính năng chính / Key Features

### 🎯 Multi-Row Display (New in v2.0!)
- **Mỗi trận đấu có nhiều dòng** - mỗi dòng là một mức stake (5000, 3000, 2000, 1000, 500, 300, 200, 100)
- Dòng đầu tiên hiển thị đầy đủ thông tin trận (thời gian, đội, giải đấu, trạng thái)
- Các dòng tiếp theo chỉ hiển thị tỷ lệ cược

### 🔍 Search & Filter (New in v2.3!)
- **Tìm kiếm trận đấu theo tên đội** - Gõ tên đội để tìm nhanh
- Real-time search (không cần nhấn Enter)
- Không phân biệt HOA/thường
- Tìm theo Home hoặc Away team

### 📊 Odds Coverage
- **Full Time (Nguyên trận)**: Cược chấp (HDP), Tài/Xỉu (O/U), 1X2
- **Half Time (Hiệp 1)**: Cược chấp (HDP), Tài/Xỉu (O/U), 1X2
- **Lẻ/Chẵn**: Odd/Even betting

### 🔄 Auto-Update
- ✅ Tự động cập nhật mỗi 1 giây
- ✅ Pause/Resume functionality
- ✅ Lọc trận không phải Live (isLive = false)
- ✅ Manual refresh button

### 🎨 UI Features
- ✅ Header 2 tầng (Main groups + Sub-headers)
- ✅ Số đỏ cho odds âm, số đen cho odds dương
- ✅ **Compact layout**: Home - Away trên cùng 1 dòng (v2.3)
- ✅ Responsive layout
- ✅ Color-coded cells
- ✅ Hiển thị số trận và tổng số dòng

---

## 📸 Screenshots

### Bảng hiển thị odds (Multi-row per match)

```
┌──────────┬───────────────────────┬─────────────────── NGUYÊN TRẬN ──────────────────┬─────────┬────────── HIỆP 1 ─────────┬──────┐
│ Thời Gian│     Trận đấu          │  Cược chấp  │  Tài/Xỉu   │     1X2      │ Lẻ/Chẵn │ Cược chấp │ Tài/Xỉu  │ 1X2  │ Nhiều│
├──────────┼───────────────────────┼─────────────┼────────────┼──────────────┼─────────┼───────────┼──────────┼──────┼──────┤
│  21:45   │ e-Football F24        │    0.0      │   4-4.5    │ 2.92 1.80    │  0.86   │   0-0.5   │  2-2.5   │ 2.94 │ + 4  │
│  Live    │ e-Spain               │  0.94 0.78  │ 0.74 0.98  │     3.95     │  0.86   │ 0.93 0.79 │ 0.95 0.77│ 2.09 │      │
│          │ e-Argentina           │             │            │              │         │           │          │ 2.99 │      │
│          │ Hòa                   ├─────────────┼────────────┼──────────────┼─────────┼───────────┼──────────┼──────┤      │
│          │                       │    0.50     │   4.50     │              │         │  0-0.5    │          │      │      │
│          │                       │ -0.88  0.57 │-0.90  0.69 │              │         │ 0.79 0.77 │          │      │      │
│          │                       ├─────────────┼────────────┼──────────────┼─────────┼───────────┼──────────┼──────┤      │
│          │                       │   0-0.5     │   2.00     │              │         │           │          │      │      │
│          │                       │  0.63 -0.79 │ 0.58 -0.72 │              │         │           │          │      │      │
└──────────┴───────────────────────┴─────────────┴────────────┴──────────────┴─────────┴───────────┴──────────┴──────┴──────┘
```

**Giải thích:**
- **Dòng 1**: Hiển thị thông tin trận + odds cho stake 5000
- **Dòng 2**: Chỉ hiển thị odds cho stake 3000
- **Dòng 3**: Chỉ hiển thị odds cho stake 2000
- Số đỏ (-0.88, -0.90, -0.79, -0.72) là odds âm
- Số đen (0.94, 0.78, ...) là odds dương

---

## 📋 Yêu cầu hệ thống / System Requirements

- **OS**: Windows 10/11
- **.NET**: 8.0 SDK ([Download](https://dotnet.microsoft.com/download/dotnet/8.0))
- **IDE**: Visual Studio 2022 (recommended) hoặc Rider

---

## 🚀 Cài đặt / Installation

### Phương pháp 1: Visual Studio

1. Mở `SportsOddsViewer.sln` trong Visual Studio 2022
2. Build solution (Ctrl + Shift + B)
3. Run (F5)

### Phương pháp 2: Command Line

```bash
# Build
dotnet build SportsOddsViewer/SportsOddsViewer.csproj

# Run
dotnet run --project SportsOddsViewer/SportsOddsViewer.csproj
```

### Phương pháp 3: Batch Scripts

**Windows:**
```cmd
# Build
build.bat

# Run
run.bat

# Publish standalone
publish.bat
```

---

## 📁 Cấu trúc dự án / Project Structure

```
SportsOddsViewer/
├── Models/
│   └── Match.cs                    # Data model với StakeAmount, IsFirstRowOfMatch
├── Services/
│   └── ApiService.cs               # API client & multi-row parser
├── Converters/
│   └── NegativeValueConverter.cs   # Converter để tô màu số âm
├── MainWindow.xaml                 # UI với multi-row support
├── MainWindow.xaml.cs              # UI logic với GetMatchKey()
├── App.xaml
└── SportsOddsViewer.csproj
```

---

## 🔍 Cách hoạt động / How It Works

### Data Flow

```
API Response (JavaScript string)
    ↓
┌─────────────────────────────┐
│  Parse 4 Sections           │
│  - Section 0: Leagues       │
│  - Section 1: Matches       │
│  - Section 2: Stats Mapping │
│  - Section 4: Odds Data     │
└─────────────────────────────┘
    ↓
┌─────────────────────────────┐
│  Map matchStatsId→eventId   │
│  Group by eventId + stake   │
└─────────────────────────────┘
    ↓
┌─────────────────────────────┐
│  Create Multiple Rows       │
│  One row per stake level    │
└─────────────────────────────┘
    ↓
┌─────────────────────────────┐
│  Sort & Mark First Row      │
│  (IsFirstRowOfMatch = true) │
└─────────────────────────────┘
    ↓
┌─────────────────────────────┐
│  Display in DataGrid        │
│  Conditional visibility     │
└─────────────────────────────┘
```

### Key Components

1. **ApiService.ParseResponse()**
   - Parse 4 sections từ JavaScript response
   - Ánh xạ matchStatsId → eventId (Section 2)
   - Tạo nhiều rows cho mỗi trận (theo stake trong Section 4)
   - Lọc trận Live (statusCode 6, 8)

2. **Match Model**
   ```csharp
   public class Match {
       public int EventId { get; set; }
       public double StakeAmount { get; set; }     // New in v2.0
       public bool IsFirstRowOfMatch { get; set; } // New in v2.0
       
       // Full Time odds
       public string FTHDPLine { get; set; }
       public string FTHDPHome { get; set; }
       public string FTHDPAway { get; set; }
       // ... more properties
   }
   ```

3. **MainWindow Logic**
   - `GetMatchKey()`: `eventId + "_" + stake` → unique identifier
   - Update logic so sánh rows theo key
   - DataGrid với conditional visibility (IsFirstRowOfMatch)

4. **DispatcherTimer**
   - Auto-refresh every 1 second
   - Pause/Resume support

### Multi-Row Example

**API Data for Match 9197805:**
```javascript
// Stake 5000: [5715744360,[113397115,1,0,5000.00,0.00],[0.90,0.90]]
// Stake 3000: [5732912570,[113397115,1,0,3000.00,0.25],[-0.88,0.57]]
// Stake 2000: [5731845460,[113397115,1,0,2000.00,-0.25],[0.63,-0.79]]
```

**Displayed as 3 Rows:**

| EventId | Stake | FTHDPLine | FTHDPHome | FTHDPAway | IsFirstRowOfMatch |
|---------|-------|-----------|-----------|-----------|-------------------|
| 9197805 | 5000 | 0.0 | 0.90 | 0.90 | ✅ true |
| 9197805 | 3000 | 0.25 | -0.88 | 0.57 | ❌ false |
| 9197805 | 2000 | -0.25 | 0.63 | -0.79 | ❌ false |

---

## 📚 Tài liệu / Documentation

- [HUONG_DAN.md](HUONG_DAN.md) - Hướng dẫn sử dụng chi tiết (Tiếng Việt)
- [HOW_IT_WORKS.md](HOW_IT_WORKS.md) - Chi tiết kỹ thuật
- [API_PARSE_GUIDE.md](API_PARSE_GUIDE.md) - **Hướng dẫn chi tiết parse API response**
- [CHANGELOG.md](CHANGELOG.md) - Lịch sử phiên bản và thay đổi
- [VISUAL_STUDIO_GUIDE.md](VISUAL_STUDIO_GUIDE.md) - Hướng dẫn build với Visual Studio
- [SEARCH_AND_LAYOUT_UPDATE_V2.3.md](SEARCH_AND_LAYOUT_UPDATE_V2.3.md) - **Update v2.3: Search & Compact Layout**

---

## 🆕 What's New in v2.0

### 🎯 Multi-Row per Match

**Before (v1.0):**
```
1 trận = 1 row → chỉ 1 set odds
```

**After (v2.0):**
```
1 trận = nhiều rows
├─ Row 1 (stake 5000): HDP 0.94/0.78, OU 0.74/0.98, ...
├─ Row 2 (stake 3000): HDP -0.88/0.57, OU -0.90/0.69, ...
└─ Row 3 (stake 2000): HDP 0.63/-0.79, OU 0.58/-0.72, ...
```

### 🔧 Improved Parsing
- ✅ Parse chính xác 4 sections từ API
- ✅ Sử dụng matchStatsId mapping (Section 2)
- ✅ Lọc đúng trận Live vs Non-Live
- ✅ Decode Unicode/Hex escape sequences

### 🎨 Better UI
- ✅ Cột "Trạng thái" mới
- ✅ Số đỏ/đen tự động (NegativeValueConverter)
- ✅ Kích thước cột tối ưu
- ✅ Hiển thị: "Số trận: 25 (150 dòng)"

### 🐛 Bug Fixes
- ✅ Fixed odds không parse đúng
- ✅ Fixed trận Live vẫn hiển thị
- ✅ Fixed Unicode characters không hiện đúng

---

## 🎓 Usage

### Chạy ứng dụng
1. Mở `SportsOddsViewer.exe` (sau khi build)
2. Ứng dụng tự động bắt đầu fetch data
3. Dữ liệu cập nhật mỗi 1 giây

### Điều khiển
- **🔍 Tìm đội**: Gõ tên đội để tìm kiếm (v2.3)
- **▶ Tiếp tục / ⏸ Tạm dừng**: Toggle auto-update
- **🔄 Làm mới**: Manual refresh
- Đóng cửa sổ để thoát

### Hiểu bảng dữ liệu

**Row đầu tiên của mỗi trận:**
- Hiển thị: Thời gian, Trạng thái, Đội, Giải đấu
- Có nút "+ 4" ở cột "Nhiều"

**Các rows tiếp theo:**
- Chỉ hiển thị odds
- Không hiển thị thông tin trận

**Màu sắc:**
- 🔴 Số đỏ: Odds âm (< 0)
- ⚫ Số đen: Odds dương (≥ 0)
- 🔵 Số xanh: Line value (handicap, over/under)

---

## 🛠️ Development

### Build từ source

```bash
git clone <repo-url>
cd <repo-directory>
dotnet restore
dotnet build
dotnet run
```

### Debug trong Visual Studio

1. Mở `SportsOddsViewer.sln`
2. Set breakpoints trong code
3. Press F5 để debug
4. Kiểm tra `_matches` collection trong Locals window

### Customize

**Thay đổi API URL:**
```csharp
// File: ApiService.cs
private readonly string _apiUrl = "YOUR_API_URL_HERE";
```

**Thay đổi update interval:**
```csharp
// File: MainWindow.xaml.cs
_timer = new DispatcherTimer {
    Interval = TimeSpan.FromSeconds(5) // Thay đổi từ 1s thành 5s
};
```

---

## ❓ Troubleshooting

### Ứng dụng không hiển thị dữ liệu
- Kiểm tra kết nối internet
- Kiểm tra API URL có đúng không
- Xem Status text ở footer

### Build errors
- Đảm bảo đã cài .NET 8.0 SDK
- Run `dotnet restore` trước khi build
- Check VISUAL_STUDIO_GUIDE.md

### Odds không hiển thị đúng
- Xem API_PARSE_GUIDE.md để hiểu cấu trúc response
- Debug ApiService.ParseResponse() method

---

## 🤝 Contributing

Pull requests are welcome! Các đóng góp cải thiện:
- Performance optimization
- UI enhancements
- Additional bet types
- Bug fixes

---

## 📝 License

This project is for educational purposes. Chỉ sử dụng cho mục đích học tập và nghiên cứu.

---

## 📧 Contact

Có câu hỏi? Tạo issue trên GitHub hoặc xem tài liệu chi tiết trong thư mục docs.

---

## 🙏 Credits

- WPF Framework by Microsoft
- .NET 8.0
- Newtonsoft.Json for JSON parsing (optional)

---

**Version**: 2.3  
**Last Updated**: December 2025  
**Author**: Sports Odds Viewer Team
