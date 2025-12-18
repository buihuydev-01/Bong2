# How to Debug - Không Parse Được Dữ Liệu

## Bước 1: Rebuild và Chạy từ Console

```cmd
cd BettingOddsDisplay
dotnet clean
dotnet build
dotnet run
```

**Quan trọng**: Phải chạy từ console, không dùng Visual Studio GUI để xem logs!

## Bước 2: Đọc Console Output

Bạn sẽ thấy logs chi tiết như:

```
=== Betting Odds Display Started ===
[TEST] Loading sample data...
[Parser] Response length: 5234
[Parser] First 200 chars: $M('odds-display').onUpdate(2,[74748,1,1,[[[427465,'e-Football...
[Parser] Regex matched successfully
[Parser] JSON string length: 5123
[Parser] Root array parsed, count: 10
[Parser] Parsing leagues at index 3...
[Parser] Leagues array found, count: 1
[Parser] Leagues list found, count: 2
[Parser] Total leagues parsed: 2
[Parser] Parsing matches at index 4...
[Parser] Matches array found, count: 1
[Parser] Matches list found, count: 4
[Parser] Total matches parsed: 4
```

## Bước 3: Tìm Lỗi

### Case 1: "Regex did not match!"

```
[Parser] ERROR: Regex did not match!
```

**Nguyên nhân**: Response format khác với expected

**Giải pháp**:
1. Copy response của bạn vào file `test_response.txt`
2. Kiểm tra xem có bắt đầu bằng `$M('odds-display').onUpdate(2,[` không
3. Nếu khác, sửa regex trong `ResponseParser.cs`

### Case 2: "Root array too short"

```
[Parser] ERROR: Root array too short, count=5, expected >= 7
```

**Nguyên nhân**: Cấu trúc array khác

**Giải pháp**: In ra structure để xem:

```csharp
for (int i = 0; i < rootArray.Count; i++)
{
    Console.WriteLine($"Index {i}: {rootArray[i]?.ToString()?.Substring(0, 100)}");
}
```

### Case 3: "No leagues array found"

```
[Parser] WARNING: No leagues array found at index 3
```

**Nguyên nhân**: Leagues không ở index 3

**Giải pháp**: Tìm xem leagues ở index nào

### Case 4: JSON Parse Error

```
[Parser] ❌ JSON Parse error: Unexpected character...
```

**Nguyên nhân**: JSON không valid

**Giải pháp**:
1. Save extracted JSON ra file
2. Dùng online JSON validator
3. Fix format issues

## Bước 4: Test với Response Thật

### Option A: Paste trực tiếp vào code

1. Mở `TestParserConsole.cs`
2. Paste response của bạn vào biến `response`
3. Uncomment `Main` method
4. Run:

```cmd
dotnet run
```

### Option B: Test trong MainWindow

1. Mở `MainWindow.xaml.cs`
2. Trong method `TestWithSampleData()`, thay:

```csharp
var response = System.IO.File.ReadAllText(sampleFile);
```

Bằng:

```csharp
var response = @"PASTE_YOUR_RESPONSE_HERE";
```

3. Rebuild và run

## Bước 5: So sánh với Sample

Mở `sample_response.txt` và so sánh structure:

**Sample:**
```javascript
$M('odds-display').onUpdate(2,[74748,1,1,[
  [[leagues]], // index 3
  [[matches]], // index 4
  [[markets]], // index 5
  ,,,
  [[odds]]     // index 6
]);
```

**Your response:** ???

Nếu khác structure → cần update parser logic

## Quick Test Script

Tạo file `test.cs`:

```csharp
using System;
using BettingOddsDisplay.Services;

var response = @"YOUR_RESPONSE_HERE";
var parser = new ResponseParser();
var data = parser.ParseResponse(response);

Console.WriteLine($"Leagues: {data.Leagues.Count}");
Console.WriteLine($"Matches: {data.Matches.Count}");
```

Run:
```cmd
dotnet script test.cs
```

## Common Issues

### Issue 1: Response có dấu ngoặc kép khác

Ví dụ: `"` vs `'`

**Fix**: Escape properly trong C#:
```csharp
var response = @"...'value'...";  // OK
var response = "...\"value\"..."; // OK
```

### Issue 2: Response quá dài

**Fix**: Save ra file:
```csharp
var response = File.ReadAllText("my_response.txt");
```

### Issue 3: Unicode characters

Ví dụ: `Bóng Đá` có dấu

**Fix**: File phải save với UTF-8 encoding

## Advanced Debug

### Print Raw Arrays

Thêm vào parser sau dòng `var rootArray = JArray.Parse(jsonString);`:

```csharp
Console.WriteLine("\n=== RAW ARRAY STRUCTURE ===");
for (int i = 0; i < rootArray.Count; i++)
{
    var item = rootArray[i];
    if (item is JArray arr)
        Console.WriteLine($"[{i}] JArray, count: {arr.Count}");
    else if (item == null || item.Type == JTokenType.Null)
        Console.WriteLine($"[{i}] null");
    else
        Console.WriteLine($"[{i}] {item.Type}: {item.ToString().Substring(0, Math.Min(50, item.ToString().Length))}");
}
Console.WriteLine("=== END STRUCTURE ===\n");
```

### Save Parsed JSON

```csharp
File.WriteAllText("parsed.json", rootArray.ToString(Formatting.Indented));
Console.WriteLine("Saved to parsed.json");
```

## Need Help?

Nếu vẫn không fix được:

1. Copy toàn bộ console output
2. Copy response string (first 500 chars)
3. Paste vào issue/chat

Tôi sẽ giúp bạn debug cụ thể!
