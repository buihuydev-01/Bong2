# Troubleshooting - Không load được dữ liệu

## Vấn đề: App chạy nhưng màn hình trắng, không hiển thị dữ liệu

### Nguyên nhân có thể:

1. **API không kết nối được**
   - Không có internet
   - URL API sai hoặc đã thay đổi
   - Cookies hết hạn
   - Server API đang bảo trì

2. **Parse lỗi**
   - Format response thay đổi
   - Dữ liệu không đúng cấu trúc

3. **Lỗi code**
   - Exception khi parse hoặc display

---

## Giải pháp

### Bước 1: Xem Console Output

Chạy app từ **Command Line** để xem logs:

```cmd
cd BettingOddsDisplay
dotnet run
```

Bạn sẽ thấy các log như:
```
=== Betting Odds Display Started ===
[TEST] Loading sample data...
[TEST] Sample data parsed: 2 leagues, 4 matches
[12:34:56] Fetching data from API...
[12:34:57] Response length: 12345 chars
[12:34:57] Parsed: 2 leagues, 4 matches
```

### Bước 2: Test với Sample Data

App sẽ tự động load `sample_response.txt` khi khởi động để test parser.

Nếu thấy dữ liệu từ sample data → Parser hoạt động OK
Nếu không thấy gì → Có lỗi trong parser

### Bước 3: Kiểm tra kết nối API

**Nếu thấy lỗi network:**
```
[12:34:56] Network error: Unable to connect...
```

**Giải pháp:**
1. Kiểm tra internet connection
2. Thử truy cập URL trong browser
3. Kiểm tra cookies còn hạn không

**Lấy cookies mới:**
1. Mở browser, vào trang betting
2. F12 → Network tab
3. Reload trang
4. Click vào request đầu tiên
5. Headers → Request Headers → Cookie
6. Copy toàn bộ cookie string
7. Update trong `Services/OddsDataService.cs` dòng 33-38

### Bước 4: Kiểm tra response

**Nếu thấy:**
```
[12:34:57] Response length: 0 chars
```

→ API không trả về dữ liệu
→ Có thể không có trận đấu lúc này

**Giải pháp:**
- Đợi khi có trận đấu
- Hoặc thay đổi query parameters để lấy dữ liệu khác

### Bước 5: Kiểm tra parse

**Nếu thấy:**
```
[12:34:57] Parsed: 0 leagues, 0 matches
```

→ Parser không extract được dữ liệu
→ Format response có thể đã thay đổi

**Giải pháp:**
1. Lưu response ra file để kiểm tra
2. So sánh với `sample_response.txt`
3. Điều chỉnh parser nếu cần

---

## Debug Chi Tiết

### 1. Lưu Response Ra File

Thêm vào `OddsDataService.cs` sau dòng `var response = await _httpClient.GetStringAsync(url);`:

```csharp
// Save response for debugging
File.WriteAllText("debug_response.txt", response);
Console.WriteLine("Response saved to debug_response.txt");
```

### 2. Test Parser Riêng

Tạo file `TestParser.cs`:

```csharp
var response = File.ReadAllText("debug_response.txt");
var parser = new ResponseParser();
var data = parser.ParseResponse(response);
Console.WriteLine($"Leagues: {data.Leagues.Count}");
Console.WriteLine($"Matches: {data.Matches.Count}");
```

### 3. Kiểm tra Exception

Xem Stack Trace trong console để biết lỗi ở đâu:

```
[ERROR] Error: Object reference not set to an instance of an object
Stack trace: at BettingOddsDisplay.Services.ResponseParser.ParseResponse...
```

---

## Các Lỗi Thường Gặp

### Lỗi 1: "Network error: Unable to connect"

**Nguyên nhân:** Không kết nối được API

**Giải pháp:**
- Check internet
- Check firewall
- Check antivirus
- Try VPN nếu site bị block

### Lỗi 2: "Response length: 0 chars"

**Nguyên nhân:** API trả về empty

**Giải pháp:**
- Không có trận đấu lúc này
- Cookies hết hạn → Lấy cookies mới
- URL sai → Kiểm tra lại URL

### Lỗi 3: "Parsed: 0 leagues, 0 matches"

**Nguyên nhân:** Parse không thành công

**Giải pháp:**
- Format response thay đổi
- Regex không match
- JSON structure khác
→ Cần update parser

### Lỗi 4: "Object reference not set to an instance"

**Nguyên nhân:** NullReferenceException

**Giải pháp:**
- Kiểm tra null checks
- Debug từng bước
- Add more try-catch

---

## Test Mode (Chỉ dùng Sample Data)

Nếu muốn test UI mà không cần API:

### Cách 1: Comment API calls

Trong `MainWindow.xaml.cs`:

```csharp
// _dataService.Start();  // Comment this line
```

App sẽ chỉ hiển thị sample data.

### Cách 2: Tăng interval

Trong `OddsDataService.cs`, dòng 66:

```csharp
_timer = new Timer(..., 10000, 10000);  // 10 giây thay vì 1 giây
```

---

## Live Debugging Trong Visual Studio

1. Set breakpoint trong `OnDataUpdated()`
2. Run with F5
3. Khi breakpoint hit, xem:
   - `data.Leagues.Count`
   - `data.Matches.Count`
   - Inspect từng match object

4. Step through code để tìm lỗi

---

## Contact Support

Nếu vẫn không fix được:

1. Copy toàn bộ console output
2. Screenshot màn hình
3. Save `debug_response.txt`
4. Create issue với thông tin trên

---

## Quick Fix Checklist

- [ ] Check console logs
- [ ] Sample data loads OK?
- [ ] Internet connection OK?
- [ ] URL correct?
- [ ] Cookies updated?
- [ ] Response not empty?
- [ ] Parser works with sample?
- [ ] No exceptions in stack trace?

---

## Advanced: Enable Verbose Logging

Thêm logging chi tiết trong `ResponseParser.cs`:

```csharp
Console.WriteLine($"Parsing leagues... found {leaguesArray?.Count ?? 0}");
Console.WriteLine($"Parsing matches... found {matchesArray?.Count ?? 0}");
Console.WriteLine($"Parsing markets... found {marketsArray?.Count ?? 0}");
Console.WriteLine($"Parsing odds... found {oddsArray?.Count ?? 0}");
```

Điều này sẽ cho biết chính xác bước nào bị lỗi.
