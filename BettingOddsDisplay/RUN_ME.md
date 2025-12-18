# 🚀 Chạy App Ngay!

## Cách chạy (3 bước)

### 1. Mở Terminal/Command Prompt

```cmd
cd BettingOddsDisplay
```

### 2. Build

```cmd
dotnet clean
dotnet build
```

### 3. Chạy

```cmd
dotnet run
```

## ✅ Những gì sẽ xảy ra:

1. **Console hiển thị logs**:
```
=== Betting Odds Display Started ===
[TEST] Testing with real response...
[Parser] Response length: XXXX
[Parser] Regex matched successfully
[Parser] ✅ Parse completed successfully!
[TEST] Real response parsed: 3 leagues, 3 matches
[UI] Adding 3 new matches
```

2. **Cửa sổ hiển thị**:
- Bảng với header màu xanh
- 3 trận đấu: e-Finland vs e-Spain, e-France vs e-Italy, v.v.
- Mỗi trận có nhiều dòng odds (theo chiều dọc)
- Giống y hệt ảnh web gốc!

## ❌ Nếu không thấy dữ liệu:

### Check 1: Xem console output
- Có thấy "[TEST] Real response parsed: 3 leagues, 3 matches"?
- Nếu KHÔNG → Parser lỗi

### Check 2: Xem UI
- Có thấy bảng với header không?
- Nếu CÓ nhưng không có matches → Binding lỗi

### Check 3: Test riêng parser

Mở file `TestParserConsole.cs`, uncomment dòng `Main`, rồi chạy:

```cmd
dotnet run
```

## 🔧 Debug

### Lỗi: "No matches displayed"

**Giải pháp**: Xem console logs, sẽ thấy lỗi cụ thể ở đâu.

### Lỗi: "Build failed"

```cmd
dotnet clean
dotnet restore
dotnet build
```

### Lỗi: "Parser không hoạt động"

Response format có thể thay đổi. Copy response mới và gửi cho tôi.

## 📊 Giao diện

UI giờ là **TABLE VIEW** giống web gốc:

```
┌─────────┬──────────────┬─────────────────────────────────┐
│ Time    │ Match        │ Nguyên trận                     │
├─────────┼──────────────┼─────────────────────────────────┤
│ 12:45   │ e-Finland    │                                 │
│ Live    │ vs           │                                 │
│         │ e-Spain      │                                 │
│         │ Hòa          │                                 │
│         ├──────────────┼─────────────────────────────────┤
│         │              │ -1.00  │ 0.88 │ 0.84 │         │
│         │              │ -1.25  │ 0.63 │ 0.81 │         │
│         │              │ -0.75  │-0.88 │ 0.60 │         │
└─────────┴──────────────┴─────────────────────────────────┘
```

Mỗi dòng odds là 1 row riêng biệt.

## 💡 Tips

1. **Chạy từ console** để xem logs
2. **Không chạy từ Visual Studio** nếu muốn xem console output
3. **App tự động test** với response thật khi khởi động
4. **Data sẽ update** mỗi 2 giây (từ API thật)

## 🎯 Expected Result

Sau khi chạy:
- ✅ Console: "[TEST] Real response parsed: 3 leagues, 3 matches"
- ✅ Console: "[UI] Adding 3 new matches"
- ✅ Window: Thấy bảng với 3 trận
- ✅ Mỗi trận có nhiều dòng odds

**Nếu thấy tất cả → SUCCESS! 🎉**

## ❓ Help

Nếu vẫn không hoạt động, paste **toàn bộ console output** và tôi sẽ giúp!
