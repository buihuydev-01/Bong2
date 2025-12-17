# Update v2.1 - UI Enhancement & Filter Improvements

## ✨ Những thay đổi chính

### 🔤 Font Size - Chữ to hơn, dễ nhìn hơn

**Trước:**
- Time: 11px
- Team names: 10px
- Odds: 10-11px
- Row height: 35px

**Sau:**
- Time: **13px** (Bold) ⬆️
- Status: **10px** (Bold)
- Team names: **12px** (Bold) ⬆️
- League: **9px** (SemiBold)
- Odds numbers: **12px** (Bold) ⬆️
- Line values: **10px** (Bold)
- Row height: **45px** ⬆️
- Column headers: **11px** (Bold) ⬆️

### 🍪 Cookie Support

Thêm Cookie vào HTTP client để authenticate với API:

```csharp
_httpClient.DefaultRequestHeaders.Add("Cookie", 
    "ASP.NET_SessionId=...; _hjSessionUser_1325134=...; ...");
```

### 🎯 Chỉ lấy trận CHƯA ĐẤU

**Trước:** Filter bỏ Live (status 6, 8)

**Sau:** Chỉ lấy trận scheduled (status = 10)

```csharp
// Old
if (statusCode == "6" || statusCode == "8") continue;

// New
if (statusCode != "10") continue; // CHỈ lấy trận chưa đấu
```

### 📋 Trạng thái rõ ràng hơn

**Status mapping:**
- `10` → "**Sắp đấu**" (scheduled)
- `6`, `8` → "**TRỰC TIẾP**" (live)
- `2` → "**Kết thúc**" (finished)

### 🏷️ Thêm label O/U, 1/X/2

#### Cột Tài/Xỉu (Over/Under)

**Trước:**
```
2.0-2.5
0.94    0.78
```

**Sau:**
```
2.0-2.5
O 0.94  U 0.78
```

#### Cột 1X2

**Trước:**
```
2.92  1.80  3.95
```

**Sau:**
```
  1     X     2
2.92  1.80  3.95
```

---

## 📐 UI Layout Updates

### Column Width Changes

| Column | Old Width | New Width | Change |
|--------|-----------|-----------|--------|
| Thời Gian | 70px | 70px | - |
| Trận đấu | 200px | 200px | - |
| **Nguyên trận (Total)** | **350px** | **300px** | **-50px** |
| - Cược chấp | 90px | 90px | - |
| - Tài/Xỉu | 90px | 90px | - |
| - 1X2 | 90px | **120px** | **+30px** |
| Lẻ/Chẵn | 70px | 70px | - |
| **Hiệp 1 (Total)** | **250px** | **270px** | **+20px** |
| - Cược chấp | 80px | 80px | - |
| - Tài/Xỉu | 80px | 80px | - |
| - 1X2 | 90px | **110px** | **+20px** |
| Nhiều | 60px | 60px | - |

**Total width:** 970px

### Visual Example

```
┌────────┬───────────────┬────────────────── NGUYÊN TRẬN ──────────────────┬─────────┬──────────── HIỆP 1 ───────────┬──────┐
│  Thời  │   Trận đấu    │  Cược chấp │   Tài/Xỉu   │      1X2       │ Lẻ/Chẵn │ Cược chấp│  Tài/Xỉu  │    1X2    │Nhiều │
│  Gian  │               │            │             │                │         │          │           │           │      │
├────────┼───────────────┼────────────┼─────────────┼────────────────┼─────────┼──────────┼───────────┼───────────┼──────┤
│ 01:00  │e-Football F24 │    0.5     │    3.0      │   1    X    2  │  0.86   │   0-0.5  │   1-1.5   │  1  X  2  │ + 4  │
│Sắp đấu │e-Poland       │  0.75 0.97 │ O 0.78 U .94│ 1.77  3.38 3.44│  0.86   │-.99 0.71 │O .65 U .93│2.44 3.13  │      │
│        │e-Hungary      │            │             │                │         │          │           │   2.36    │      │
├────────┼───────────────┼────────────┼─────────────┼────────────────┼─────────┼──────────┼───────────┼───────────┼──────┤
│        │               │   0.5-1    │   3-3.5     │                │         │   0.0    │  1-1.5    │           │      │
│        │               │  0.85 0.59 │ O-.96 U .68 │                │         │ 0.51-.79 │O-.87 U .58│           │      │
└────────┴───────────────┴────────────┴─────────────┴────────────────┴─────────┴──────────┴───────────┴───────────┴──────┘
```

---

## 🎨 Color & Styling

### Text Colors

- **League name**: Blue (#0066CC), Bold
- **Home team**: Red (#CC0000), Bold, 12px
- **Away team**: Black, Bold, 12px
- **Line values**: Blue (#0066CC), Bold, 10px
- **Positive odds**: Black, Bold, 12px
- **Negative odds**: Red, Bold, 12px
- **O/U labels**: Gray, 9px
- **1/X/2 labels**: Gray, 9px

### Row Styling

- **Row height**: 45px (increased from 35px)
- **Alternating colors**: White / Light Blue (#E8F4FF)
- **Grid lines**: Light Gray (#D0D0D0)

---

## 🔧 Code Changes

### ApiService.cs

1. **Added Cookie**
```csharp
_httpClient.DefaultRequestHeaders.Add("Cookie", "ASP.NET_SessionId=...; ...");
```

2. **Stricter filter**
```csharp
// Only scheduled matches
if (statusCode != "10") continue;
```

3. **Better status text**
```csharp
"10" => "Sắp đấu",
"6" => "TRỰC TIẾP",
"8" => "TRỰC TIẾP",
"2" => "Kết thúc",
```

### MainWindow.xaml

1. **Increased font sizes**
   - Time: 11px → 13px
   - Teams: 10px → 12px
   - Odds: 11px → 12px
   - Headers: 10px → 11px

2. **Added O/U labels**
```xml
<StackPanel Orientation="Horizontal">
    <TextBlock Text="O" FontSize="9" Foreground="Gray"/>
    <TextBlock Text="{Binding FTOUOver}" FontSize="12" FontWeight="Bold"/>
</StackPanel>
```

3. **Added 1/X/2 labels**
```xml
<Grid.RowDefinitions>
    <RowDefinition Height="Auto"/> <!-- Labels row -->
    <RowDefinition Height="Auto"/> <!-- Odds row -->
</Grid.RowDefinitions>

<TextBlock Grid.Row="0" Text="1" FontSize="9" Foreground="Gray"/>
<TextBlock Grid.Row="1" Text="{Binding FT1X2Home}" FontSize="12" FontWeight="Bold"/>
```

4. **Adjusted column widths**
   - FT 1X2: 90px → 120px
   - HT 1X2: 90px → 110px

---

## 📊 Before vs After Comparison

### Font Clarity

| Element | v2.0 | v2.1 | Improvement |
|---------|------|------|-------------|
| Time | 11px | 13px | +18% |
| Team names | 10px | 12px | +20% |
| Odds | 10-11px | 12px | +9-20% |
| Row height | 35px | 45px | +29% |

### Information Display

**v2.0:**
```
0.94  0.78    (Unclear: which is Over, which is Under?)
2.92 1.80 3.95 (Which number is Home/Draw/Away?)
```

**v2.1:**
```
O 0.94  U 0.78   (Clear: Over 0.94, Under 0.78)
  1      X     2
2.92   1.80  3.95  (Clear: Home=2.92, Draw=1.80, Away=3.95)
```

### Match Status

**v2.0:**
- "Hòa" (ambiguous)
- Shows Live matches

**v2.1:**
- "Sắp đấu" (clear: scheduled)
- "TRỰC TIẾP" (clear: live)
- Only shows scheduled matches

---

## ✅ Testing Checklist

- [x] Cookie được gửi trong HTTP request
- [x] Chỉ hiển thị trận status = 10 (Sắp đấu)
- [x] Font size tăng lên, rõ ràng hơn
- [x] Chữ "O" "U" hiện bên cạnh odds trong cột Tài/Xỉu
- [x] Chữ "1" "X" "2" hiện trên odds trong cột 1X2
- [x] Row height 45px, đủ chỗ cho chữ to
- [x] Màu đỏ cho số âm, màu đen cho số dương
- [x] Trạng thái hiển thị "Sắp đấu" thay vì "Hòa"

---

## 🚀 How to Build & Test

```bash
# Build
dotnet build SportsOddsViewer/SportsOddsViewer.csproj

# Run
dotnet run --project SportsOddsViewer/SportsOddsViewer.csproj
```

**Expected result:**
- Chỉ thấy trận "Sắp đấu"
- Không thấy trận "TRỰC TIẾP"
- Chữ to, rõ ràng
- Có chữ O/U, 1/X/2 bên cạnh odds

---

## 📝 Notes

1. **Cookie expire**: Cookie trong code có thể expire sau vài giờ/ngày. Nếu không lấy được data, cần update Cookie mới.

2. **Filter logic**: Nếu muốn xem trận Live, đổi filter:
   ```csharp
   // Show all matches
   // if (statusCode != "10") continue; // Comment out this line
   ```

3. **Font size**: Có thể điều chỉnh thêm trong XAML:
   - Tìm `FontSize="12"` → đổi thành 13, 14...
   - Tìm `RowHeight="45"` → đổi thành 50, 55...

4. **Column width**: Nếu chữ bị cắt, tăng width:
   ```xml
   <DataGridTemplateColumn Header="1X2" Width="130"> <!-- Was 120 -->
   ```

---

## 🎯 Summary

**v2.1 improvements:**
- ✅ Chữ to hơn **20%** → dễ nhìn
- ✅ Thêm Cookie → có thể fetch data
- ✅ Chỉ lấy trận chưa đấu → không bị spam trận Live
- ✅ Thêm O/U, 1/X/2 labels → rõ ràng hơn
- ✅ Trạng thái "Sắp đấu" → hiểu ngay

**File changes:**
- `ApiService.cs`: Cookie, filter, status text
- `MainWindow.xaml`: Font sizes, labels, widths

**Ready for production!** 🎉
