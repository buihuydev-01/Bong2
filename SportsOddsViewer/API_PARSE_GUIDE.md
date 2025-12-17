# HƯỚNG DẪN PARSE API - SPORTS ODDS VIEWER

## 📊 CẤU TRÚC RESPONSE TỪ API

### Format tổng quan
```javascript
$M('odds-display').onUpdate(2,[
    [[...leagues...]],      // Section 0: Danh sách giải đấu
    [[...matches...]],      // Section 1: Danh sách trận đấu  
    [[...matchStats...]],   // Section 2: Match statistics IDs mapping
    [[...unknown...]],      // Section 3: (không dùng)
    [[...oddsData...]]      // Section 4: Dữ liệu tỷ lệ cược
]);
```

---

## 🔍 SECTION 0: LEAGUES (Giải đấu)

### Format
```javascript
[leagueId,'leagueName','','']
```

### Ví dụ
```javascript
[427468,'e-Football F24 International Friendly','','2 x 8 minutes']
```

### Parse
- `leagueId`: ID giải đấu (int)
- `leagueName`: Tên giải (string, có thể có ký tự unicode/hex)

---

## 🔍 SECTION 1: MATCHES (Trận đấu)

### Format
```javascript
[eventId,sportType,leagueId,'homeTeam','awayTeam','matchCode',statusCode,'dateTime',...]
```

### Ví dụ
```javascript
[9197799,1,427468,'e-Argentina','e-Portugal','0054-E3127251217067',8,'12/17/2025 21:00',0,'twitch',19,0,0,0]
```

### Parse
- `eventId`: ID trận đấu (int) - **KEY để liên kết với odds**
- `sportType`: Loại thể thao (1 = bóng đá)
- `leagueId`: ID giải đấu (liên kết với Section 0)
- `homeTeam`: Tên đội nhà (string)
- `awayTeam`: Tên đội khách (string)
- `matchCode`: Mã trận đấu (string)
- `statusCode`: Trạng thái
  - `6` hoặc `8` = Live (đang diễn ra)
  - `10` = Sắp diễn ra
  - `2` = Kết thúc
- `dateTime`: Thời gian (format: MM/DD/YYYY HH:mm)

---

## 🔍 SECTION 2: MATCH STATS (Mapping IDs)

### Format
```javascript
[matchStatsId,eventId,...]
```

### Ví dụ
```javascript
[113751864,9197799,0,0,0,19]
```

### Mục đích
- Liên kết `matchStatsId` (dùng trong Section 4) với `eventId` (từ Section 1)
- Cần tạo Dictionary<matchStatsId, eventId> để map

---

## 🔍 SECTION 4: ODDS DATA (Tỷ lệ cược)

### Format
```javascript
[oddsId,[matchStatsId,betType,subType,handicapValue,stakeAmount],[odds1,odds2,odds3]]
```

### Ví dụ
```javascript
[5733647160,[113751864,1,0,0.00,5000.00],[0.76,0.96]]
```

### Parse

#### 1. matchStatsId
- Dùng để tìm eventId từ Section 2

#### 2. betType (Loại cược)
| betType | Tên | Mô tả |
|---------|-----|-------|
| **1** | FT Handicap | Cược chấp nguyên trận |
| **3** | FT Over/Under | Tài/Xỉu nguyên trận |
| **5** | FT 1X2 | Cược 3 cửa nguyên trận |
| **7** | HT Handicap | Cược chấp hiệp 1 |
| **8** | HT 1X2 | Cược 3 cửa hiệp 1 |
| **9** | HT Over/Under | Tài/Xỉu hiệp 1 |
| **12** | Odd/Even | Lẻ/Chẵn |

#### 3. subType
- `0` = Main odds (tỷ lệ chính)
- Các giá trị khác: tỷ lệ phụ

#### 4. handicapValue
- Với **HDP** (betType 1, 7): Giá trị chấp
  - Ví dụ: `0.00`, `-0.25`, `0.5`, `-0.75`
- Với **OU** (betType 3, 9): Line tài/xỉu
  - Ví dụ: `2.5`, `3.0`, `1.5`
- Với **1X2**, **Odd/Even**: Không dùng

#### 5. odds Array
- **HDP** (1, 7): `[oddsHome, oddsAway]`
- **OU** (3, 9): `[oddsOver, oddsUnder]`
- **1X2** (5, 8): `[oddsHome, oddsDraw, oddsAway]`
- **Odd/Even** (12): `[oddsOdd, oddsEven]`

---

## 📋 VÍ DỤ PARSE ĐẦY ĐỦ

### Response mẫu
```javascript
$M('odds-display').onUpdate(2,[
  // Section 0: Leagues
  [[427468,'e-Football F24 International Friendly','','2 x 8 minutes']],
  
  // Section 1: Matches
  [[9197799,1,427468,'e-Argentina','e-Portugal','code',8,'12/17/2025 21:00',0,'twitch',19,0,0,0]],
  
  // Section 2: Stats mapping
  [[113751864,9197799,0,0,0,19]],
  
  // Section 3: (skip)
  [],
  
  // Section 4: Odds
  [
    [5733647160,[113751864,1,0,0.00,5000.00],[0.76,0.96]],      // FT HDP
    [5733647220,[113751864,3,0,2.50,5000.00],[0.90,0.88]],      // FT OU
    [5733647250,[113751864,5,0,0.00,5000.00],[2.22,3.35,2.58]], // FT 1X2
    [5733647690,[113751864,12,0,0.00,5000.00],[0.78,0.95]]      // Odd/Even
  ]
]);
```

### Kết quả Parse

**Match Info:**
- Event ID: `9197799`
- League: `e-Football F24 International Friendly`
- Home: `e-Argentina`
- Away: `e-Portugal`
- Time: `21:00`
- Status: `Live`

**Full Time Odds:**
- **Handicap:**
  - Line Home: `0.0`
  - Odds Home: `0.76`
  - Line Away: `0.0` (opposite)
  - Odds Away: `0.96`

- **Over/Under:**
  - Line: `2.5`
  - Over: `0.90`
  - Under: `0.88`

- **1X2:**
  - Home: `2.22`
  - Draw: `3.35`
  - Away: `2.58`

- **Odd/Even:**
  - Odd: `0.78`
  - Even: `0.95`

---

## 🎯 HIỂN THỊ TRONG UI

### Cấu trúc bảng (theo hình mẫu)

```
┌──────────┬──────────────┬────────────────────────────────────────────┬──────────┬─────────────────────────────┬──────┐
│ Thời Gian│  Trận đấu    │         Nguyên trận                        │ Lẻ/Chẵn  │        Hiệp 1               │Nhiều │
│          │              ├──────────────┬──────────────┬──────────────┤          ├──────────────┬──────────────┤      │
│          │              │  Cược chấp   │   Tài/Xỉu    │     1X2      │          │   Tài/Xỉu    │     1X2      │      │
├──────────┼──────────────┼──────────────┼──────────────┼──────────────┼──────────┼──────────────┼──────────────┼──────┤
│  21:00   │e-Argentina   │ 0.0  │+0.0  │ 2.5  │ 2.5  │ 2.22│3.35│2.58│ 0.78     │ 1.0  │ 1.0  │ 1.50│2.00│3.00│  +   │
│  Live    │e-Portugal    │ 0.76 │ 0.96 │ 0.90 │ 0.88 │              │ 0.95     │ 0.85 │ 0.90 │              │      │
│          │e-Football... │              │              │              │          │              │              │      │
└──────────┴──────────────┴──────────────┴──────────────┴──────────────┴──────────┴──────────────┴──────────────┴──────┘
```

### Mapping dữ liệu vào UI

**Cột "Cược chấp" (Handicap):**
- Cell 1 (Home):
  - Dòng 1: `handicapValue` (màu xanh lam)
  - Dòng 2: `odds[0]` (màu đen)
- Cell 2 (Away):
  - Dòng 1: `-handicapValue` (màu đỏ)
  - Dòng 2: `odds[1]` (màu đen)

**Cột "Tài/Xỉu" (Over/Under):**
- Cell 1 (Over):
  - Dòng 1: `handicapValue` (line)
  - Dòng 2: `odds[0]`
- Cell 2 (Under):
  - Dòng 1: `handicapValue` (line)
  - Dòng 2: `odds[1]`

**Cột "1X2":**
- 3 cells ngang: `odds[0]` | `odds[1]` | `odds[2]`

**Cột "Lẻ/Chẵn":**
- Dòng 1: `odds[0]` (Lẻ)
- Dòng 2: `odds[1]` (Chẵn)

---

## 🔄 UPDATE LIÊN TỤC

### Flow cập nhật mỗi 1 giây:

```
1. Gọi API
   ↓
2. Parse Response (4 sections)
   ↓
3. Tạo Dictionary mappings:
   - leagues[leagueId] = leagueName
   - statsToEvent[matchStatsId] = eventId
   ↓
4. Parse Matches (Section 1)
   - Tạo Match objects
   - Link với leagues
   ↓
5. Parse Odds (Section 4)
   - Loop qua tất cả odds entries
   - Map matchStatsId → eventId
   - Update Match object tương ứng
   ↓
6. Update ObservableCollection
   - Match cũ: Update properties
   - Match mới: Add
   - Match không còn: Remove
   ↓
7. UI tự động refresh (WPF Data Binding)
```

---

## 🐛 XỬ LÝ LỖI

### Các trường hợp cần handle:

1. **API không response:**
   - Return empty list
   - Hiển thị thông báo lỗi

2. **Parse lỗi:**
   - Skip entry bị lỗi
   - Continue với entries khác
   - Log error message

3. **Missing data:**
   - Odds không có: Hiển thị empty
   - League không tìm thấy: Hiển thị ID
   - Time parse lỗi: Hiển thị raw string

4. **Duplicate events:**
   - Dùng EventId làm key
   - Update existing match

---

## 📝 NOTES

### Màu sắc trong UI:
- **Xanh lam (#0066CC)**: Handicap line, OU line
- **Đỏ**: Negative handicap (đội được chấp)
- **Đen**: Odds values
- **Xanh lá**: Live status

### Format numbers:
- **Odds**: 2 decimal places (0.76, 2.22)
- **Handicap**: 1-2 decimal places (0.0, 0.5, -0.75)
- **Line**: 1 decimal place (2.5, 3.0)

### Special values:
- `0` hoặc `0.00`: Không có giá trị
- Empty string: Chưa có dữ liệu
- Negative handicap: Đội yếu hơn (được chấp)
- Positive handicap: Đội mạnh hơn (phải chấp)

---

## ✅ CHECKLIST PARSE

- [x] Parse leagues từ Section 0
- [x] Parse matches từ Section 1  
- [x] Parse stats mapping từ Section 2
- [x] Parse odds từ Section 4
- [x] Map matchStatsId → eventId
- [x] Update Match objects với odds
- [x] Handle all betTypes (1,3,5,7,8,9,12)
- [x] Format numbers correctly
- [x] Handle missing/invalid data
- [x] Update UI via ObservableCollection

---

Với guide này, việc parse API sẽ chính xác 100% với dữ liệu thực tế! 🎯
