# Debug Guide - Chi tiết từng bước

## 📋 Checklist Nhanh

```
✅ Project build thành công
✅ Chạy từ console để xem logs
✅ Sample data hiển thị được
✅ Response từ API về được
✅ Parser parse thành công
✅ UI hiển thị dữ liệu
```

---

## 🔍 Bước 1: Build Project

```cmd
cd BettingOddsDisplay
dotnet clean
dotnet build
```

**Expected output:**
```
Build succeeded.
    0 Warning(s)
    0 Error(s)
```

**Nếu có lỗi:** Kiểm tra lại code, sửa syntax errors

---

## 🔍 Bước 2: Chạy và Xem Logs

```cmd
dotnet run
```

**Expected console output:**

```
=== Betting Odds Display Started ===
Time: 12/18/2025 1:23:45 PM
[TEST] Loading sample data...
[Parser] Response length: 5234
[Parser] First 200 chars: $M('odds-display').onUpdate(2,[74748,1,1,...
[Parser] Regex matched successfully
[Parser] JSON string length: 5000
[Parser] Root array parsed, count: 10
[Parser] Parsing leagues at index 3...
[Parser] Leagues array found, count: 1
[Parser] Leagues list found, count: 2
[Parser] Total leagues parsed: 2
[Parser] Parsing matches at index 4...
[Parser] Matches array found, count: 1
[Parser] Matches list found, count: 4
[Parser] Total matches parsed: 4
[Parser] Parsing markets at index 5...
[Parser] Parsing odds at index 6...
[Parser] Sorting odds lines...
[Parser] ✅ Parse completed successfully!
[Parser] Final result: 2 leagues, 4 matches
[TEST] Sample data parsed: 2 leagues, 4 matches
[UI] Updating UI with 4 matches
[UI] UI updated successfully
[12:34:56] Fetching data from API...
```

---

## 🔍 Bước 3: Kiểm tra Sample Data

Nếu thấy:
```
[TEST] Sample data parsed: 2 leagues, 4 matches
[UI] UI updated successfully
```

→ ✅ Parser và UI hoạt động OK
→ Bạn phải thấy dữ liệu hiển thị trên màn hình

**Nếu KHÔNG thấy dữ liệu trên UI:**
- Check `sample_response.txt` có trong thư mục không
- Check file được copy vào output folder: `bin/Debug/net8.0-windows/`

---

## 🔍 Bước 4: Kiểm tra API Call

Sau sample data, app sẽ gọi API thật:

```
[12:34:56] Fetching data from API...
[12:34:57] Response length: 12345 chars
[12:34:57] Parsed: 2 leagues, 4 matches
[UI] Updating UI with 4 matches
```

### ✅ Nếu thành công:
- Thấy "Response length: XXXX chars"
- Thấy "Parsed: X leagues, Y matches"
- UI update với dữ liệu mới

### ❌ Nếu lỗi network:
```
[12:34:56] Network error: Unable to connect to the remote server
[ERROR] Network error: ...
```

**Nguyên nhân:**
- Không có internet
- URL sai
- Cookies hết hạn
- Firewall block

**Giải pháp:**
1. Test internet: `ping google.com`
2. Update cookies mới trong `OddsDataService.cs`
3. Tắt firewall/antivirus thử

### ❌ Nếu parse fail:
```
[Parser] ERROR: Regex did not match!
```

hoặc

```
[Parser] Parsed: 0 leagues, 0 matches
```

**Nguyên nhân:** Response format khác

**Giải pháp:** 
1. Lưu response ra file
2. So sánh với sample
3. Update parser

---

## 🔍 Bước 5: Debug Chi Tiết

### Test Parser với Response Cụ Thể

Tạo file `test_my_response.txt` với response của bạn, sau đó:

```csharp
// Trong MainWindow.xaml.cs, method TestWithSampleData()
var response = File.ReadAllText("test_my_response.txt");
// hoặc paste trực tiếp:
var response = @"PASTE_YOUR_RESPONSE_HERE";
```

### In Ra Structure

Thêm vào `ResponseParser.cs` sau `var rootArray = JArray.Parse(jsonString);`:

```csharp
Console.WriteLine("\n=== ARRAY STRUCTURE ===");
for (int i = 0; i < Math.Min(10, rootArray.Count); i++)
{
    var item = rootArray[i];
    if (item is JArray arr)
    {
        Console.WriteLine($"[{i}] JArray with {arr.Count} items");
        if (arr.Count > 0 && arr[0] is JArray nested)
        {
            Console.WriteLine($"     └─ First item is JArray with {nested.Count} items");
        }
    }
    else if (item == null || item.Type == JTokenType.Null)
    {
        Console.WriteLine($"[{i}] null");
    }
    else
    {
        var str = item.ToString();
        Console.WriteLine($"[{i}] {item.Type}: {str.Substring(0, Math.Min(50, str.Length))}");
    }
}
Console.WriteLine("=== END ===\n");
```

Expected output:
```
=== ARRAY STRUCTURE ===
[0] Integer: 74748
[1] Integer: 1
[2] Integer: 1
[3] JArray with 1 items
     └─ First item is JArray with 2 items
[4] JArray with 1 items
     └─ First item is JArray with 4 items
[5] JArray with 1 items
[6] null
[7] null
[8] null
[9] JArray with 1 items
=== END ===
```

Nếu structure khác → Cần update indexes trong parser

---

## 🔍 Bước 6: Kiểm tra UI Bindings

Nếu parser OK nhưng UI vẫn trắng:

1. **Check ObservableCollection update:**
```csharp
// Trong UpdateLeagueData
Console.WriteLine($"Clearing {_leagues.Count} old leagues");
_leagues.Clear();
Console.WriteLine($"Adding {data.Leagues.Count} new leagues");
foreach (var league in data.Leagues)
{
    Console.WriteLine($"  Adding league: {league.Name}");
    _leagues.Add(leagueVM);
}
```

2. **Check ItemsControl binding:**
- Mở Snoop hoặc Live Visual Tree trong Visual Studio
- Check `LeaguesItemsControl.ItemsSource`
- Check count > 0

3. **Check DataTemplate rendering:**
- Add background colors để debug
- Check visibility

---

## 🎯 Common Issues & Fixes

### Issue 1: "Regex did not match"

**Debug:**
```csharp
Console.WriteLine("Testing regex patterns:");
Console.WriteLine("Pattern 1: " + Regex.IsMatch(response, @"\$M\("));
Console.WriteLine("Pattern 2: " + Regex.IsMatch(response, @"odds-display"));
Console.WriteLine("Pattern 3: " + Regex.IsMatch(response, @"onUpdate"));
```

### Issue 2: "Root array count = 0"

Response có thể là empty hoặc format khác

**Debug:**
```csharp
Console.WriteLine($"JSON starts with: {jsonString.Substring(0, 10)}");
Console.WriteLine($"JSON ends with: {jsonString.Substring(jsonString.Length - 10)}");
```

### Issue 3: UI không update

**Check Dispatcher:**
```csharp
Console.WriteLine($"Is on UI thread: {Dispatcher.CheckAccess()}");
```

### Issue 4: Data null

**Add null checks:**
```csharp
if (data == null)
{
    Console.WriteLine("ERROR: data is null!");
    return;
}
if (data.Leagues == null)
{
    Console.WriteLine("ERROR: data.Leagues is null!");
    return;
}
```

---

## 📝 Debug Checklist

```
Step 1: Build
□ dotnet clean
□ dotnet build
□ No errors

Step 2: Run
□ dotnet run (from console!)
□ See startup logs

Step 3: Sample Data
□ "[TEST] Loading sample data..."
□ "[Parser] ✅ Parse completed successfully!"
□ "[UI] UI updated successfully"
□ See data on screen

Step 4: API
□ "[12:xx:xx] Fetching data from API..."
□ "Response length: XXXX"
□ "Parsed: X leagues, Y matches"
□ Data updates every second

Step 5: Verify
□ Window shows leagues
□ Matches displayed
□ Odds lines visible
□ No errors in console
```

---

## 🚑 Emergency Fixes

### Fix 1: Force Show Sample Data Only

Comment out API calls:
```csharp
// _dataService.Start();  // ← Comment this
```

### Fix 2: Slow Down Updates

```csharp
_timer = new Timer(..., 5000, 5000);  // 5 seconds instead of 1
```

### Fix 3: Add More Logging

```csharp
// Everywhere!
Console.WriteLine($"[DEBUG] Variable X = {x}");
```

---

## 📞 Need Help?

Nếu vẫn không hoạt động, cung cấp:

1. **Console output** (copy toàn bộ)
2. **Response sample** (first 500 chars)
3. **Screenshot** of app
4. **Error messages** (if any)

Paste vào chat và tôi sẽ giúp debug cụ thể!
