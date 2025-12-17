# HƯỚNG DẪN MỞ VÀ BUILD VỚI VISUAL STUDIO

## 📂 CẤU TRÚC PROJECT

Project đã có file Solution (.sln) để mở trong Visual Studio:

```
SportsOddsViewer/
├── SportsOddsViewer.sln          ← Solution file (mở file này)
├── SportsOddsViewer.csproj       ← Project file
├── .gitignore                     ← Git ignore file
├── Models/
├── Services/
├── MainWindow.xaml
└── ...
```

---

## 🚀 CÁCH 1: MỞ VỚI VISUAL STUDIO

### Bước 1: Mở Solution
1. Khởi động **Visual Studio 2022** (hoặc 2019)
2. Click **"Open a project or solution"**
3. Chọn file: `SportsOddsViewer.sln`
4. Click **"Open"**

### Bước 2: Restore NuGet Packages
Visual Studio sẽ tự động restore packages, nếu không:
1. Chuột phải vào Solution trong **Solution Explorer**
2. Chọn **"Restore NuGet Packages"**

### Bước 3: Build Project
**Cách 1: Dùng menu**
- Menu → **Build** → **Build Solution**

**Cách 2: Dùng phím tắt**
- Nhấn **Ctrl + Shift + B**

### Bước 4: Chạy ứng dụng
**Debug Mode:**
- Nhấn **F5** (chạy với debugging)

**Release Mode:**
- Nhấn **Ctrl + F5** (chạy không debug)

---

## 🛠️ CÁCH 2: BUILD BẰNG MSBUILD (COMMAND LINE)

### Yêu cầu:
- Visual Studio đã cài đặt
- MSBuild có trong PATH

### Build Debug:
```cmd
msbuild SportsOddsViewer.sln /p:Configuration=Debug
```

### Build Release:
```cmd
msbuild SportsOddsViewer.sln /p:Configuration=Release
```

### Chạy sau khi build:
```cmd
bin\Debug\net8.0-windows\SportsOddsViewer.exe
```
hoặc
```cmd
bin\Release\net8.0-windows\SportsOddsViewer.exe
```

---

## 💻 CÁCH 3: BUILD BẰNG DOTNET CLI

### Build:
```cmd
dotnet build SportsOddsViewer.sln
```

### Build với configuration:
```cmd
dotnet build SportsOddsViewer.sln -c Release
```

### Chạy trực tiếp:
```cmd
dotnet run --project SportsOddsViewer.csproj
```

### Publish (tạo file .exe standalone):
```cmd
dotnet publish SportsOddsViewer.sln -c Release -r win-x64 --self-contained
```

File output: `bin\Release\net8.0-windows\win-x64\publish\SportsOddsViewer.exe`

---

## 🎯 CẤU HÌNH BUILD

### Debug vs Release

**Debug Mode:**
- Có symbol debug
- Không optimize code
- Dễ debug và set breakpoint
- File lớn hơn

**Release Mode:**
- Optimize code
- Không có debug symbols
- Chạy nhanh hơn
- File nhỏ hơn

### Platform Target
- **Any CPU**: Chạy trên cả x86 và x64
- **x64**: Chỉ chạy trên Windows 64-bit
- **x86**: Chỉ chạy trên Windows 32-bit

---

## 🔧 TROUBLESHOOTING

### Lỗi: "SDK not found"
**Giải pháp:**
1. Cài đặt .NET 8.0 SDK
2. Download từ: https://dotnet.microsoft.com/download
3. Khởi động lại Visual Studio

### Lỗi: "NuGet packages not found"
**Giải pháp:**
1. Tools → NuGet Package Manager → Package Manager Console
2. Chạy: `Update-Package -reinstall`

### Lỗi: "Build failed"
**Giải pháp:**
1. Clean solution: Build → Clean Solution
2. Rebuild: Build → Rebuild Solution
3. Xóa folder `bin` và `obj`
4. Restore packages lại

### Lỗi: "Cannot start debugging"
**Giải pháp:**
1. Kiểm tra StartupProject: Solution Explorer → Chuột phải → Set as Startup Project
2. Kiểm tra Output Type: Project Properties → Output Type = "Windows Application"

---

## 🎨 VISUAL STUDIO FEATURES

### 1. IntelliSense
- Tự động hoàn thành code
- Hiển thị documentation
- Gợi ý tham số

### 2. Debugging
- **F9**: Set/Remove breakpoint
- **F5**: Start debugging
- **F10**: Step over
- **F11**: Step into
- **Shift + F11**: Step out

### 3. XAML Designer
- Visual editor cho UI
- Drag & drop controls
- Real-time preview
- Property editor

### 4. Error List
- View → Error List
- Hiển thị errors, warnings, messages
- Double-click để jump to code

### 5. Solution Explorer
- View → Solution Explorer
- Quản lý files và folders
- Add/Remove files
- View properties

---

## 📦 OUTPUT LOCATIONS

### Debug Build:
```
bin/Debug/net8.0-windows/
├── SportsOddsViewer.exe
├── SportsOddsViewer.dll
├── SportsOddsViewer.pdb
└── [dependencies]
```

### Release Build:
```
bin/Release/net8.0-windows/
├── SportsOddsViewer.exe
├── SportsOddsViewer.dll
└── [dependencies]
```

### Publish Output:
```
bin/Release/net8.0-windows/win-x64/publish/
└── SportsOddsViewer.exe  ← Standalone executable
```

---

## 🎯 KEYBOARD SHORTCUTS

### Build & Run:
- **Ctrl + Shift + B**: Build Solution
- **F5**: Start Debugging
- **Ctrl + F5**: Start Without Debugging
- **Shift + F5**: Stop Debugging

### Code Editing:
- **Ctrl + K, Ctrl + D**: Format Document
- **Ctrl + K, Ctrl + C**: Comment Selection
- **Ctrl + K, Ctrl + U**: Uncomment Selection
- **F12**: Go to Definition
- **Alt + F12**: Peek Definition

### Navigation:
- **Ctrl + ,**: Go to File/Type
- **Ctrl + T**: Search Everything
- **Ctrl + -**: Navigate Backward
- **Ctrl + Shift + -**: Navigate Forward

---

## 📊 BUILD CONFIGURATIONS

### Tạo Configuration mới:
1. Build → Configuration Manager
2. Active Solution Configuration → <New...>
3. Nhập tên (ví dụ: "Staging")
4. Copy settings từ: Release
5. OK

### Chỉnh sửa Configuration:
1. Project → Properties
2. Chọn Configuration (Debug/Release)
3. Điều chỉnh:
   - Build tab: Define constants, optimize
   - Debug tab: Start action, command line args
   - Application tab: Target framework, output type

---

## 🔐 CODE SIGNING (Optional)

### Để sign ứng dụng:
1. Project → Properties → Signing
2. Check "Sign the assembly"
3. Chọn hoặc tạo Strong Name Key (.snk)

---

## 📝 PROJECT PROPERTIES

### Application:
- **Target Framework**: net8.0-windows
- **Output Type**: Windows Application
- **Startup Object**: SportsOddsViewer.App

### Build:
- **Platform Target**: Any CPU
- **Optimize Code**: ☑ (Release only)
- **Allow unsafe code**: ☐

### Debug:
- **Start Action**: Start project
- **Working Directory**: $(ProjectDir)

---

## 🚀 PUBLISH OPTIONS

### ClickOnce Deployment:
1. Build → Publish SportsOddsViewer
2. Chọn folder location
3. Configure settings
4. Publish

### Self-Contained Application:
```cmd
dotnet publish -c Release -r win-x64 --self-contained -p:PublishSingleFile=true
```

Options:
- `-r win-x64`: Windows 64-bit
- `--self-contained`: Include .NET runtime
- `-p:PublishSingleFile=true`: Single .exe file
- `-p:PublishTrimmed=true`: Trim unused code

---

## 💡 TIPS

### 1. Build nhanh hơn:
- Tắt Code Analysis: Project Properties → Code Analysis → Disable

### 2. Debug hiệu quả:
- Sử dụng Conditional Breakpoints
- Watch window để theo dõi variables
- Immediate window để execute code

### 3. Code sạch hơn:
- Sử dụng Code Cleanup: Ctrl + K, Ctrl + E
- Enable EditorConfig
- Use StyleCop analyzers

### 4. NuGet packages:
- Tools → NuGet Package Manager → Manage Packages for Solution
- Cài đặt packages cần thiết
- Update packages thường xuyên

---

## 📚 TÀI LIỆU THAM KHẢO

- Visual Studio Docs: https://docs.microsoft.com/visualstudio
- .NET Documentation: https://docs.microsoft.com/dotnet
- WPF Guide: https://docs.microsoft.com/dotnet/desktop/wpf

---

## ✅ CHECKLIST BUILD

- [ ] Mở SportsOddsViewer.sln trong Visual Studio
- [ ] Restore NuGet packages
- [ ] Build Solution (Ctrl + Shift + B)
- [ ] Kiểm tra Error List (không có errors)
- [ ] Chạy Debug (F5) để test
- [ ] Build Release (nếu cần distribute)
- [ ] Test Release build
- [ ] Publish (nếu cần tạo installer)

---

## 🎉 HOÀN TẤT

Project đã sẵn sàng để build và chạy trong Visual Studio!

Để bắt đầu:
1. Mở **SportsOddsViewer.sln**
2. Nhấn **F5** để run

Chúc bạn code vui vẻ! 🚀
