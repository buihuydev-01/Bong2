# Hướng dẫn phân tích API Response

## Cấu trúc Response

API trả về một chuỗi JavaScript có dạng:

```javascript
$M('odds-display').onUpdate(2,[32208,1,1,[
  [/* Section 0: Leagues */],
  [/* Section 1: Matches */],
  [/* Section 2: Match Stats Mapping */],
  ,,  /* Empty sections */
  [/* Section 4: Odds Data */]
],,,]);
```

## Chi tiết các Section

### Section 0: Leagues (Giải đấu)

Format: `[leagueId, 'leagueName', ...]`

**Ví dụ:**
```javascript
[39,'Cúp Tây Ban Nha','','']
[51,'INTERCONTINENTAL CUP','','']
```

Các escape characters đặc biệt:
- `\xC3` → UTF-8 byte (kết hợp với byte tiếp theo)
- `\u1ED9` → Unicode character
- Cần decode để hiển thị đúng tiếng Việt

### Section 1: Matches (Thông tin trận đấu)

Format: `[eventId, 1, leagueId, 'homeTeam', 'awayTeam', 'matchCode', statusCode, 'dateTime', ...]`

**Các trường quan trọng:**
- `eventId`: ID duy nhất của trận đấu
- `leagueId`: Tham chiếu đến Section 0
- `homeTeam`, `awayTeam`: Tên đội
- `statusCode`: 
  - `10` = Hòa (scheduled)
  - `6` = Live (đang diễn ra)
  - `8` = Live
  - `2` = Scheduled
- `dateTime`: Format "MM/DD/YYYY HH:MM"

**Ví dụ:**
```javascript
[9183877,1,39,'Cultural Leonesa','Levante','0008-E0144251217001',10,'12/18/2025 01:00',0,'',19,1,66609718,0]
```

### Section 2: Match Stats Mapping

Format: `[matchStatsId, eventId, 0, 0, 0, someValue]`

Ánh xạ `matchStatsId` → `eventId` để liên kết odds data với match.

**Ví dụ:**
```javascript
[113397115,9183877,0,0,0,19]
```
→ `matchStatsId 113397115` tương ứng với `eventId 9183877`

### Section 4: Odds Data (Dữ liệu tỷ lệ)

Format: `[oddsId, [matchStatsId, betType, subType, stake, line], [odds1, odds2, ...]]`

**Các betType:**

| betType | Ý nghĩa | Số giá trị odds |
|---------|---------|-----------------|
| 1 | Full Time Handicap (Cược chấp) | 2 (home, away) |
| 3 | Full Time Over/Under (Tài/Xỉu) | 2 (over, under) |
| 5 | Full Time 1X2 | 3 (home, draw, away) |
| 7 | Half Time Handicap | 2 (home, away) |
| 9 | Half Time Over/Under | 2 (over, under) |
| 8 | Half Time 1X2 | 3 (home, draw, away) |
| 12 | Odd/Even (Lẻ/Chẵn) | 2 (odd, even) |

**Ví dụ chi tiết:**

```javascript
[5715744360,[113397115,1,0,5000.00,0.00],[0.90,0.90]]
```

Phân tích:
- `oddsId`: 5715744360
- `matchStatsId`: 113397115 (ánh xạ đến eventId qua Section 2)
- `betType`: 1 (Full Time Handicap)
- `subType`: 0 (odds chính)
- `stake`: 5000.00 (mức cược tối đa)
- `line`: 0.00 (chấp 0 bàn)
- `odds`: [0.90, 0.90] (home: 0.90, away: 0.90)

**Ví dụ khác:**

```javascript
[5715744380,[113397115,3,0,5000.00,2.25],[0.87,0.99]]
```

Phân tích:
- `betType`: 3 (Full Time Over/Under)
- `stake`: 5000.00
- `line`: 2.25 (tài/xỉu 2.25 bàn)
- `odds`: [0.87, 0.99] (over: 0.87, under: 0.99)

## Logic Parsing

### Bước 1: Parse Leagues
```csharp
var leaguePattern = @"\[(\d+),'([^']*)'";
var leagueMatches = Regex.Matches(response, leaguePattern);
foreach (Match m in leagueMatches) {
    int leagueId = int.Parse(m.Groups[1].Value);
    string name = DecodeString(m.Groups[2].Value);
    leaguesDict[leagueId] = name;
}
```

### Bước 2: Parse Matches
```csharp
var matchPattern = @"\[(\d+),1,(\d+),'([^']*)','([^']*)','[^']*',(\d+),'([^']*)',";
var matchMatches = Regex.Matches(response, matchPattern);
foreach (Match mm in matchMatches) {
    int eventId = int.Parse(mm.Groups[1].Value);
    int leagueId = int.Parse(mm.Groups[2].Value);
    string home = DecodeString(mm.Groups[3].Value);
    string away = DecodeString(mm.Groups[4].Value);
    string statusCode = mm.Groups[5].Value;
    string dateTime = mm.Groups[6].Value;
    
    // Lọc: chỉ lấy trận KHÔNG Live (status != 6 và != 8)
    if (statusCode == "6" || statusCode == "8") continue;
    
    matchesDict[eventId] = (home, away, leagueId, ParseDateTime(dateTime), ParseStatus(statusCode));
}
```

### Bước 3: Parse Match Stats Mapping
```csharp
var statsPattern = @"\[(\d+),(\d+),\d+,\d+,\d+,\d+\]";
var statsMatches = Regex.Matches(response, statsPattern);
foreach (Match sm in statsMatches) {
    int statsId = int.Parse(sm.Groups[1].Value);
    int eventId = int.Parse(sm.Groups[2].Value);
    statsToEventMap[statsId] = eventId;
}
```

### Bước 4: Parse Odds (TẠO NHIỀU ROWS)
```csharp
var oddsPattern = @"\[(\d+),\[(\d+),(\d+),(\d+),(\d+\.\d+),([^\]]+)\],\[([^\]]+)\]\]";
var oddsMatches = Regex.Matches(response, oddsPattern);

// Group by eventId + stake để tạo unique row
var oddsGroups = new Dictionary<string, MatchModel>();

foreach (Match om in oddsMatches) {
    int matchStatsId = int.Parse(om.Groups[2].Value);
    int betType = int.Parse(om.Groups[3].Value);
    int subType = int.Parse(om.Groups[4].Value);
    double stake = double.Parse(om.Groups[5].Value);
    double line = double.Parse(om.Groups[6].Value);
    string[] oddsValues = om.Groups[7].Value.Split(',');
    
    // Chỉ lấy odds chính (subType = 0)
    if (subType != 0) continue;
    
    // Ánh xạ matchStatsId → eventId
    int eventId = statsToEventMap[matchStatsId];
    
    // Key duy nhất: eventId + stake
    string key = $"{eventId}_{stake:F2}";
    
    if (!oddsGroups.ContainsKey(key)) {
        // Tạo row mới cho stake level này
        oddsGroups[key] = new MatchModel {
            EventId = eventId,
            StakeAmount = stake,
            HomeTeam = matchesDict[eventId].home,
            AwayTeam = matchesDict[eventId].away,
            // ... các thông tin khác
        };
    }
    
    var row = oddsGroups[key];
    
    // Điền odds vào row theo betType
    switch (betType) {
        case 1: // FT Handicap
            row.FTHDPLine = FormatHandicap(line);
            row.FTHDPHome = FormatOdds(oddsValues[0]);
            row.FTHDPAway = FormatOdds(oddsValues[1]);
            break;
        case 3: // FT Over/Under
            row.FTOULine = FormatLine(line);
            row.FTOUOver = FormatOdds(oddsValues[0]);
            row.FTOUUnder = FormatOdds(oddsValues[1]);
            break;
        // ... các betType khác
    }
}

// Convert to list và sort
var matchRows = oddsGroups.Values
    .OrderBy(m => m.Time)
    .ThenBy(m => m.EventId)
    .ThenByDescending(m => m.StakeAmount)
    .ToList();
```

## Cơ chế Multi-Row

**1 trận đấu = Nhiều rows trong DataGrid**

Mỗi row tương ứng với 1 mức stake khác nhau:

```
Trận: e-Spain vs e-Argentina (21:45)

Row 1: stake 5000.00 → HDP: 0.94/0.78, OU: 4-4.5 0.74/0.98, ...
Row 2: stake 3000.00 → HDP: 0.50/0.98, OU: 4.50 1.80/3.95, ...
Row 3: stake 2000.00 → HDP: 0-0.5 0.86/0.86, OU: 0.93/0.79, ...
...
```

### Hiển thị trong UI

- **Row đầu tiên** (`IsFirstRowOfMatch = true`): Hiển thị đầy đủ thông tin trận (Time, Teams, League, Status)
- **Các rows sau**: Chỉ hiển thị odds, ô thông tin trận để trống

```csharp
// Đánh dấu row đầu tiên
int? currentEventId = null;
foreach (var row in matchRows) {
    if (currentEventId != row.EventId) {
        row.IsFirstRowOfMatch = true;
        currentEventId = row.EventId;
    }
}
```

## Format dữ liệu hiển thị

### FormatOdds
```csharp
private string FormatOdds(string odds) {
    if (double.TryParse(odds, out double value)) {
        if (Math.Abs(value) < 0.01) return "";
        return value.ToString("F2"); // 2 chữ số thập phân
    }
    return odds;
}
```

### FormatHandicap
```csharp
private string FormatHandicap(double handicap) {
    if (Math.Abs(handicap) < 0.01) return "0.0";
    return handicap.ToString("F1"); // Ví dụ: "0.5", "-0.75"
}
```

### FormatLine (Tài/Xỉu)
```csharp
private string FormatLine(double line) {
    double lower = Math.Floor(line * 2) / 2;
    double upper = Math.Ceiling(line * 2) / 2;
    
    if (Math.Abs(lower - upper) < 0.01) {
        return line.ToString("F1"); // Ví dụ: "2.0"
    } else {
        return $"{lower:F1}-{upper:F1}"; // Ví dụ: "2.0-2.5"
    }
}
```

### DecodeString (Xử lý Unicode/Hex escape)
```csharp
private string DecodeString(string encoded) {
    // Decode \xHH (hex bytes)
    var result = Regex.Replace(encoded, @"\\x([0-9A-Fa-f]{2})", match => {
        int value = Convert.ToInt32(match.Groups[1].Value, 16);
        return ((char)value).ToString();
    });
    
    // Decode \uHHHH (unicode)
    result = Regex.Replace(result, @"\\u([0-9A-Fa-f]{4})", match => {
        int value = Convert.ToInt32(match.Groups[1].Value, 16);
        return char.ConvertFromUtf32(value);
    });
    
    return result;
}
```

## Lọc trận Live

Chỉ lấy trận với `isLive = false`:

```csharp
// Kiểm tra statusCode
if (statusCode == "6" || statusCode == "8") {
    continue; // Bỏ qua trận Live
}
```

## Màu sắc Odds

- **Số đen**: Odds dương (≥ 0)
- **Số đỏ**: Odds âm (< 0)

Sử dụng Value Converter trong WPF:
```csharp
public class NegativeValueConverter : IValueConverter {
    public object Convert(object value, ...) {
        if (double.TryParse(value?.ToString(), out double num)) {
            return num < 0;
        }
        return false;
    }
}
```

XAML:
```xml
<TextBlock.Style>
    <Style TargetType="TextBlock">
        <Setter Property="Foreground" Value="Black"/>
        <Style.Triggers>
            <DataTrigger Binding="{Binding FTHDPHome, Converter={StaticResource NegativeConverter}}" Value="True">
                <Setter Property="Foreground" Value="Red"/>
            </DataTrigger>
        </Style.Triggers>
    </Style>
</TextBlock.Style>
```
