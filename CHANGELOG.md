# Changelog

## Version 2.0 - Multi-Row per Match (Current)

### 🎯 Major Changes

1. **Multi-row display per match**
   - Mỗi trận đấu hiện có nhiều rows trong bảng
   - Mỗi row đại diện cho một mức stake khác nhau (5000, 3000, 2000, 1000, 500, 300, 200, 100)
   - Row đầu tiên hiển thị đầy đủ thông tin trận (Time, Teams, League, Status)
   - Các rows tiếp theo chỉ hiển thị odds

2. **Improved API parsing**
   - Parse chính xác 4 sections từ API response:
     - Section 0: Leagues
     - Section 1: Matches
     - Section 2: Match Stats Mapping (matchStatsId → eventId)
     - Section 4: Odds Data (với stake levels)
   - Sử dụng `matchStatsId` để ánh xạ chính xác giữa odds và matches
   - Lọc đúng trận không Live (statusCode != 6 và != 8)

3. **UI Improvements**
   - Thêm cột "Trạng thái" để hiển thị Live/Hòa/etc
   - Giảm kích thước cột "Nhiều" (từ 80px xuống 60px)
   - Số đỏ cho odds âm, số đen cho odds dương
   - Hiển thị số trận và tổng số rows: "Số trận: 25 (150 dòng)"
   - Header 2 tầng với các nhóm: Thời Gian, Trận đấu, Nguyên trận, Lẻ/Chẵn, Hiệp 1, Nhiều

4. **Data Model Updates**
   - Added `StakeAmount` property
   - Added `IsFirstRowOfMatch` property
   - Removed duplicate line properties (FTHDPLineAway, etc.)
   - Simplified odds structure

5. **New Converter**
   - `NegativeValueConverter` để tự động tô màu đỏ cho odds âm

### 📊 Display Logic

Ví dụ: Trận **e-Spain vs e-Argentina** (21:45)

| Row | Time | Teams | FT HDP | FT OU | FT 1X2 | Lẻ/Chẵn | HT HDP | HT OU | HT 1X2 |
|-----|------|-------|--------|-------|--------|---------|--------|-------|--------|
| 1 | 21:45<br>Live | e-Spain<br>e-Argentina<br>Hòa | 0.0<br>0.94/0.78 | 4-4.5<br>0.74/0.98 | 2.92<br>1.80<br>3.95 | 0.86<br>0.86 | 0-0.5<br>0.93/0.79 | 2-2.5<br>0.95/0.77 | 2.94<br>2.09<br>2.99 |
| 2 |  |  | 0.50<br>-0.88/0.57 | 4.50<br>-0.90/0.69 | ... | ... | ... | ... | ... |
| 3 |  |  | 0-0.5<br>0.63/-0.79 | 2.00<br>0.58/-0.72 | ... | ... | ... | ... | ... |

### 🔧 Technical Details

**Unique Key per Row:**
```csharp
string key = $"{eventId}_{stake:F2}";
// Ví dụ: "9197805_5000.00", "9197805_3000.00"
```

**Sorting:**
1. Time (earliest first)
2. EventId (group matches together)
3. StakeAmount (highest first - 5000, 3000, 2000, ...)

**Update Logic:**
- Compare rows by `eventId + stake` combination
- Remove rows không còn trong API response
- Update rows đã tồn tại
- Add rows mới

---

## Version 1.0 - Initial Release

### Features

1. **Basic WPF Application**
   - Auto-update every 1 second
   - Display match odds in DataGrid
   - Pause/Resume functionality

2. **API Integration**
   - Fetch data from sports.wwyyuuvv22.com
   - Parse JavaScript response
   - Decode Vietnamese characters

3. **Odds Display**
   - Full Time: Handicap, Over/Under, 1X2
   - Half Time: Handicap, Over/Under, 1X2
   - Odd/Even

4. **UI**
   - Two-tier header (Main groups + Sub-headers)
   - Color-coded cells
   - Responsive layout

### Known Issues in v1.0

- ❌ Mỗi trận chỉ hiển thị 1 row (thiếu stake levels)
- ❌ Không parse đúng matchStatsId mapping
- ❌ Không lọc đúng trận Live

---

## Migration Guide (v1.0 → v2.0)

### Breaking Changes

1. **Model Changes**
   - `Match.Score` property removed
   - Added `Match.StakeAmount`
   - Added `Match.IsFirstRowOfMatch`
   - Removed redundant line properties

2. **API Service**
   - `ParseResponse()` now returns multiple rows per match
   - New method: `GetMatchKey()` for unique identification

3. **UI Binding**
   - DataGrid now expects multiple rows per match
   - Use `IsFirstRowOfMatch` to conditionally display match info

### How to Update

1. **Update Model:**
   ```csharp
   // Old
   public string Score { get; set; }
   
   // New
   public double StakeAmount { get; set; }
   public bool IsFirstRowOfMatch { get; set; }
   ```

2. **Update API Service:**
   ```csharp
   // Old - one match = one row
   matchesDict[eventId] = CreateMatch(eventId, ...);
   
   // New - one match = multiple rows (by stake)
   var key = $"{eventId}_{stake:F2}";
   oddsGroups[key] = CreateMatchRow(eventId, stake, ...);
   ```

3. **Update XAML:**
   ```xml
   <!-- Old - always show team names -->
   <TextBlock Text="{Binding HomeTeam}"/>
   
   <!-- New - only show on first row -->
   <TextBlock>
       <TextBlock.Style>
           <Style TargetType="TextBlock">
               <Setter Property="Text" Value=""/>
               <Style.Triggers>
                   <DataTrigger Binding="{Binding IsFirstRowOfMatch}" Value="True">
                       <Setter Property="Text" Value="{Binding HomeTeam}"/>
                   </DataTrigger>
               </Style.Triggers>
           </Style>
       </TextBlock.Style>
   </TextBlock>
   ```

---

## Roadmap

### v2.1 (Planned)
- [ ] Thêm filter theo giải đấu
- [ ] Export dữ liệu ra Excel
- [ ] Lưu lịch sử odds thay đổi
- [ ] Hiển thị chart odds movement

### v2.2 (Future)
- [ ] Thêm âm thanh thông báo khi odds thay đổi
- [ ] Highlight rows khi odds thay đổi đáng kể
- [ ] Thêm tính năng so sánh odds giữa các stake levels
- [ ] Dark mode

### v3.0 (Long-term)
- [ ] Support multiple bookmakers
- [ ] Betting calculator
- [ ] Live score integration
- [ ] Mobile app (Xamarin/MAUI)
