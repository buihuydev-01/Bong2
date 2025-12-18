# 📖 Documentation Index

Hướng dẫn tìm kiếm tài liệu và file trong project.

## 🚀 Getting Started

### Bắt đầu nhanh
1. **[README.md](./README.md)** - Tổng quan project, yêu cầu hệ thống
2. **[QUICK_START.md](./QUICK_START.md)** - Hướng dẫn cài đặt và chạy nhanh
3. **Build Scripts**
   - [build.bat](./build.bat) - Build trên Windows
   - [build.sh](./build.sh) - Build trên Linux/Mac

### Chạy ứng dụng
```bash
# Cách nhanh nhất
cd BettingOddsDisplay
dotnet run
```

---

## 📚 Documentation

### Core Documentation
| File | Description |
|------|-------------|
| [README.md](./README.md) | Main documentation, overview |
| [QUICK_START.md](./QUICK_START.md) | Quick start guide |
| [ARCHITECTURE.md](./ARCHITECTURE.md) | Architecture & design patterns |
| [FEATURES.md](./FEATURES.md) | Feature list (completed & planned) |
| [CUSTOMIZATION.md](./CUSTOMIZATION.md) | How to customize the app |

### Development Documentation
| File | Description |
|------|-------------|
| [CONTRIBUTING.md](./CONTRIBUTING.md) | Contribution guidelines |
| [CHANGELOG.md](./CHANGELOG.md) | Version history |
| [LICENSE](./LICENSE) | MIT License |

### Summary
| File | Description |
|------|-------------|
| [PROJECT_SUMMARY.md](../PROJECT_SUMMARY.md) | Complete project summary |

---

## 💻 Source Code

### Main Application
| File | Purpose |
|------|---------|
| [App.xaml](./App.xaml) | Application resources |
| [App.xaml.cs](./App.xaml.cs) | Application entry point |
| [MainWindow.xaml](./MainWindow.xaml) | Main UI (current version) |
| [MainWindow.xaml.cs](./MainWindow.xaml.cs) | Main window logic |
| [MainWindow_Original.xaml](./MainWindow_Original.xaml) | Original UI (backup) |

### Models (Data Structures)
| File | Purpose |
|------|---------|
| [Models/BettingData.cs](./Models/BettingData.cs) | Container for all betting data |
| [Models/League.cs](./Models/League.cs) | League information |
| [Models/Match.cs](./Models/Match.cs) | Match information |
| [Models/OddsGroup.cs](./Models/OddsGroup.cs) | Odds grouping & individual lines |

### Services (Business Logic)
| File | Purpose |
|------|---------|
| [Services/OddsDataService.cs](./Services/OddsDataService.cs) | HTTP client, timer, events |
| [Services/ResponseParser.cs](./Services/ResponseParser.cs) | Parse JavaScript response |

### ViewModels
| File | Purpose |
|------|---------|
| [ViewModels/LeagueViewModel.cs](./ViewModels/LeagueViewModel.cs) | League display model |

### Converters (XAML)
| File | Purpose |
|------|---------|
| [Converters/OddsColorConverter.cs](./Converters/OddsColorConverter.cs) | Convert odds value to color |
| [Converters/StringVisibilityConverter.cs](./Converters/StringVisibilityConverter.cs) | Show/hide based on string |

### Tests
| File | Purpose |
|------|---------|
| [Tests/ParserTest.cs](./Tests/ParserTest.cs) | Unit test for parser |

---

## 🔧 Configuration & Build

### Project Files
| File | Purpose |
|------|---------|
| [BettingOddsDisplay.sln](./BettingOddsDisplay.sln) | Visual Studio solution |
| [BettingOddsDisplay.csproj](./BettingOddsDisplay.csproj) | Project file (.NET 8.0) |
| [.gitignore](./.gitignore) | Git ignore rules |

### Build Scripts
| File | Platform | Purpose |
|------|----------|---------|
| [build.bat](./build.bat) | Windows | Build Debug version |
| [build.sh](./build.sh) | Linux/Mac | Build Debug version |
| [build-release.bat](./build-release.bat) | Windows | Build Release (standalone EXE) |
| [build-release.sh](./build-release.sh) | Linux/Mac | Build Release (standalone EXE) |

---

## 📋 Sample Data

| File | Purpose |
|------|---------|
| [sample_response.txt](./sample_response.txt) | Sample API response for testing parser |

---

## 🎯 Quick Navigation by Task

### "Tôi muốn chạy app"
→ [QUICK_START.md](./QUICK_START.md)

### "Tôi muốn hiểu cấu trúc project"
→ [ARCHITECTURE.md](./ARCHITECTURE.md)

### "Tôi muốn tùy chỉnh app"
→ [CUSTOMIZATION.md](./CUSTOMIZATION.md)

### "Tôi muốn thêm tính năng"
→ [CONTRIBUTING.md](./CONTRIBUTING.md)  
→ [FEATURES.md](./FEATURES.md) (xem tính năng planned)

### "Tôi muốn thay đổi API URL"
→ [Services/OddsDataService.cs](./Services/OddsDataService.cs) line ~75

### "Tôi muốn thay đổi tần suất cập nhật"
→ [Services/OddsDataService.cs](./Services/OddsDataService.cs) line ~66

### "Tôi muốn thay đổi màu sắc"
→ [MainWindow.xaml](./MainWindow.xaml) - Search for `Background="#..."`  
→ [CUSTOMIZATION.md](./CUSTOMIZATION.md) Section 4

### "Tôi muốn hiểu format của response"
→ [sample_response.txt](./sample_response.txt)  
→ [Services/ResponseParser.cs](./Services/ResponseParser.cs)

### "Tôi muốn test parser"
→ [Tests/ParserTest.cs](./Tests/ParserTest.cs)

### "Tôi muốn build standalone EXE"
→ Windows: `build-release.bat`  
→ Linux/Mac: `./build-release.sh`

### "Tôi gặp lỗi"
→ [QUICK_START.md](./QUICK_START.md) - Troubleshooting section

---

## 📊 Code Organization

```
BettingOddsDisplay/
│
├── 📱 UI Layer
│   ├── MainWindow.xaml              # View
│   ├── MainWindow.xaml.cs           # View code-behind
│   └── Converters/                  # XAML converters
│
├── 🎨 Presentation Layer
│   └── ViewModels/                  # ViewModels
│
├── ⚙️ Business Layer
│   └── Services/                    # Business logic
│
├── 📦 Data Layer
│   └── Models/                      # Data structures
│
├── 🧪 Testing
│   └── Tests/                       # Unit tests
│
└── 📚 Documentation
    ├── README.md
    ├── QUICK_START.md
    ├── ARCHITECTURE.md
    └── ... (other .md files)
```

---

## 🔍 Find by Feature

### Auto-refresh functionality
- [Services/OddsDataService.cs](./Services/OddsDataService.cs) - Timer implementation

### Parse API response
- [Services/ResponseParser.cs](./Services/ResponseParser.cs) - Parse logic

### Display odds
- [MainWindow.xaml](./MainWindow.xaml) - UI layout
- [Models/OddsGroup.cs](./Models/OddsGroup.cs) - Odds data structure

### Color coding
- [Converters/OddsColorConverter.cs](./Converters/OddsColorConverter.cs) - Color logic

### HTTP requests
- [Services/OddsDataService.cs](./Services/OddsDataService.cs) - HTTP client setup

---

## 📱 Dependencies

Project dependencies trong [BettingOddsDisplay.csproj](./BettingOddsDisplay.csproj):
- .NET 8.0
- Newtonsoft.Json (v13.0.3)

---

## 🆘 Help & Support

### Documentation Issues
→ Create issue on GitHub

### Code Questions  
→ Read [ARCHITECTURE.md](./ARCHITECTURE.md)  
→ Read code comments

### Bug Reports
→ Create issue with:
- Steps to reproduce
- Expected behavior
- Actual behavior
- Screenshots

### Feature Requests
→ Check [FEATURES.md](./FEATURES.md) first  
→ Create issue if not listed

---

## 📝 Notes

- All documentation is in Vietnamese and English
- Code comments are in English
- UI text is in Vietnamese
- Sample data is included for offline testing

---

## 🔄 Keep Updated

This index is current as of version 1.0.0. Check [CHANGELOG.md](./CHANGELOG.md) for updates.

---

**Quick Links:**
- [README](./README.md) | [Quick Start](./QUICK_START.md) | [Architecture](./ARCHITECTURE.md) | [Features](./FEATURES.md) | [Customization](./CUSTOMIZATION.md)
