# 📋 Những gì bạn sẽ thấy khi chạy app

## 🚀 Bước 1: Chạy app

### Windows:
```cmd
cd BettingOddsDisplay
BUILD_AND_RUN.bat
```

### Command line:
```cmd
cd BettingOddsDisplay
dotnet run
```

---

## 📺 Bước 2: Console Output

Bạn sẽ thấy dòng logs như sau:

```
============================================
=== Betting Odds Display Started ===
Time: 12/18/2025 1:23:45 PM
[TEST] Testing with real response...
[Parser] Response length: 35000
[Parser] First 200 chars: $M('odds-display').onUpdate(2,[82139,1,1,[[[39,'Cúp Tây Ban Nha','',''],[427465,'e-Football F24 Elite Club Friendly','','2 x 8 minutes']...
[Parser] Regex matched successfully
[Parser] JSON string length: 34900
[Parser] JSON first 200 chars: [82139,1,1,[[[39,'Cúp Tây Ban Nha','',''],[427465,'e-Football F24 Elite Club Friendly'...
[Parser] Root array parsed, count: 10
[Parser] Parsing leagues at index 3...
[Parser] Leagues array found, count: 1
[Parser] Leagues list found, count: 3
[Parser] Total leagues parsed: 3
[Parser] Parsing matches at index 4...
[Parser] Matches array found, count: 1
[Parser] Matches list found, count: 3
[Parser] Total matches parsed: 3
[Parser] Parsing markets at index 5...
[Parser] Parsing odds at index 6...
[Parser] Sorting odds lines...
[Parser] ✅ Parse completed successfully!
[Parser] Final result: 3 leagues, 3 matches
[TEST] Real response parsed: 3 leagues, 3 matches
[TEST] First match: e-Finland vs e-Spain
[TEST] First match odds groups: 5
[TEST]   BetType 1: 3 lines
[TEST]   BetType 2: 1 lines
[TEST]   BetType 3: 3 lines
[TEST]   BetType 5: 1 lines
[TEST]   BetType 7: 3 lines
[TEST]   BetType 8: 1 lines
[TEST]   BetType 9: 3 lines
[UI] Updating UI with 3 matches
[UI] Clearing 0 old matches
[UI] Adding 3 new matches
[UI]   - e-Finland vs e-Spain, Odds groups: 5
[UI]   - e-France vs e-Italy, Odds groups: 5
[UI]   - e-Denmark vs e-Germany, Odds groups: 5
[UI] UI updated successfully
```

✅ **Nếu thấy tất cả logs trên → Parser hoạt động 100%!**

---

## 🖼️ Bước 3: Window Display

### Bạn sẽ thấy:

```
┌─────────────────────────────────────────────────────────────┐
│ ⚽ Cập nhật tự động                    12:34:56             │ ← Status bar
├─────────────────────────────────────────────────────────────┤
│ Thời │ Trận đấu  │ ─────── Nguyên trận ──────│── Hiệp 1 ──│ ← Table header
│ Gian │           │Cược chấp│Tài/Xỉu│1X2│Lẻ/Chẵn│Cược chấp│ │
├──────┼───────────┼─────────┴───────┴───┴───────┴─────────┴─┤
│12:45 │ e-Finland │                                          │
│Live  │    vs     │                                          │
│      │ e-Spain   │                                          │
│      │   Hòa     │                                          │
│      ├───────────┼──────────────────────────────────────────┤
│      │           │ -1.00 │ 0.88 │     │ 0.84 │            │ ← Odds line 1
│      │           │ -1.25 │ 0.63 │     │ 0.81 │            │ ← Odds line 2
│      │           │ -0.75 │-0.88 │     │ 0.60 │            │ ← Odds line 3
│      │           │  0.0  │ 0.86 │     │ 0.86 │            │ ← Odds line 4
│      │           │  3.00 │ 0.80 │     │ 0.92 │            │ ← Odds line 5
│      │           │  3.25 │ 0.91 │     │ 0.67 │            │ ← Odds line 6
│      │           │  2.75 │ 0.58 │     │-0.86 │            │ ← Odds line 7
│      │           │  1X2  │ 4.64 │ 3.88│ 1.47 │            │ ← Odds line 8
│      │           │ -0.50 │ 0.74 │     │ 0.84 │            │ ← Odds line 9
│      │           │ ... (total 15 rows for Finland)          │
├──────┼───────────┼──────────────────────────────────────────┤
│13:00 │ e-France  │                                          │
│Live  │    vs     │                                          │
│      │ e-Italy   │                                          │
│      │   Hòa     │                                          │
│      ├───────────┼──────────────────────────────────────────┤
│      │           │  1.00 │ 0.80 │     │ 0.92 │            │
│      │           │  1.25 │-0.98 │     │ 0.70 │            │
│      │           │ ... (total 15 rows for France)           │
├──────┼───────────┼──────────────────────────────────────────┤
│13:00 │ e-Denmark │                                          │
│Live  │    vs     │                                          │
│      │ e-Germany │                                          │
│      │   Hòa     │                                          │
│      ├───────────┼──────────────────────────────────────────┤
│      │           │  0.25 │ 0.77 │     │ 0.95 │            │
│      │           │  0.50 │ 0.99 │     │ 0.73 │            │
│      │           │ ... (total 15 rows for Denmark)          │
└──────┴───────────┴──────────────────────────────────────────┘
```

### Màu sắc:
- **Header**: Nền xanh đậm (#2E5090)
- **Time column**: Nền xám nhạt
- **Teams column**: Nền trắng xanh
- **Odds positive (>0)**: Chữ đen
- **Odds negative (<0)**: Chữ đỏ
- **Home odds background**: Xanh nhạt
- **Draw odds background**: Vàng nhạt
- **Away odds background**: Cam nhạt

### Chi tiết mỗi match:

**e-Finland vs e-Spain** sẽ có:
- 3 dòng Handicap: -1.00, -1.25, -0.75
- 1 dòng Lẻ/Chẵn: 0.0
- 3 dòng Over/Under: 3.00, 3.25, 2.75
- 1 dòng 1X2: 4.64/3.88/1.47
- 3 dòng Hiệp 1 Handicap: -0.50, -0.25, -0.75
- 1 dòng Hiệp 1 Lẻ/Chẵn
- 3 dòng Hiệp 1 O/U: 1.25, 1.50, 1.00

**Tổng cộng: ~15 dòng odds cho mỗi trận!**

---

## ✅ Success Indicators

### Console:
- ✅ `[Parser] ✅ Parse completed successfully!`
- ✅ `[TEST] Real response parsed: 3 leagues, 3 matches`
- ✅ `[UI] Adding 3 new matches`
- ✅ Không có dòng `[Parser] ERROR:`
- ✅ Không có `[UI] Error:`

### Window:
- ✅ Thấy header xanh với "Thời Gian", "Trận đấu", "Nguyên trận", "Hiệp 1"
- ✅ Thấy 3 trận đấu
- ✅ Mỗi trận có nhiều dòng odds (nhiều rows)
- ✅ Tỷ lệ cược hiển thị đúng: 0.88, 0.84, -0.88, v.v.
- ✅ Màu đỏ cho số âm, đen cho số dương

---

## ❌ Nếu có vấn đề

### Không thấy matches

**Console có log:**
```
[TEST] Real response parsed: 3 leagues, 3 matches
[UI] Adding 3 new matches
```

Nhưng window trắng?

→ UI binding lỗi. Check `MatchesItemsControl` có bind đúng không.

### Parser lỗi

**Console có log:**
```
[Parser] ERROR: Regex did not match!
```

→ Response format sai. Gửi response mới cho tôi.

### Build lỗi

```
Error CS0229: Ambiguity...
```

→ Chạy:
```cmd
rmdir /s /q obj
rmdir /s /q bin
dotnet build
```

---

## 🎯 Expected vs Actual

### Expected (Console):
```
[Parser] ✅ Parse completed successfully!
[TEST] Real response parsed: 3 leagues, 3 matches
[UI] Adding 3 new matches
```

### Expected (Window):
- Table với 3 trận
- Mỗi trận ~15 rows odds
- Giống ảnh web gốc

---

## 📊 Data Breakdown

Từ response của bạn, parser sẽ extract:

### Match 1: e-Finland vs e-Spain (12:45)
| BetType | Lines | Description |
|---------|-------|-------------|
| 1 | 3 | Handicap: -1.00, -1.25, -0.75 |
| 2 | 1 | Odd/Even: 0.0 |
| 3 | 3 | O/U: 3.00, 3.25, 2.75 |
| 5 | 1 | 1X2: 4.64/3.88/1.47 |
| 7 | 3 | H1 Handicap: -0.50, -0.25, -0.75 |
| 8 | 1 | H1 Odd/Even |
| 9 | 3 | H1 O/U: 1.25, 1.50, 1.00 |

**Total: 15 rows**

### Match 2: e-France vs e-Italy (13:00)
Similar structure, ~15 rows

### Match 3: e-Denmark vs e-Germany (13:00)
Similar structure, ~15 rows

---

## 🔍 Debug Tips

1. **Run from console** (không phải Visual Studio)
2. **Watch logs** từ đầu đến cuối
3. **Check window** sau khi thấy "[UI] UI updated successfully"
4. **Scroll down** nếu cần (có 3 matches × 15 rows = 45 rows!)

---

## ✨ Final Checklist

- [ ] Build thành công (no errors)
- [ ] Console shows parser logs
- [ ] Console shows "[Parser] ✅ Parse completed successfully!"
- [ ] Console shows "[TEST] Real response parsed: 3 leagues, 3 matches"
- [ ] Console shows "[UI] Adding 3 new matches"
- [ ] Window opens
- [ ] Window shows table header (xanh)
- [ ] Window shows 3 matches
- [ ] Each match has multiple odds rows
- [ ] Odds displayed correctly
- [ ] Colors correct (red for negative, black for positive)

**Nếu TẤT CẢ ✅ → SUCCESS! App hoạt động hoàn hảo! 🎉**

---

## 📞 Need Help?

Nếu có vấn đề:
1. Copy **toàn bộ console output**
2. Screenshot window
3. Gửi cho tôi
4. Tôi sẽ fix ngay!
