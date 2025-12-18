# Betting Odds Display Project

Repository chứa phần mềm hiển thị tỷ lệ cược theo thời gian thực.

## 📋 Table of Contents

- [Data Analysis](#-data-analysis) - Phân tích dữ liệu
- [Application](#-application) - Phần mềm C# WPF
- [Quick Start](#-quick-start) - Bắt đầu nhanh
- [Documentation](#-documentation) - Tài liệu
- [Features](#-features) - Tính năng
- [Screenshots](#-screenshots) - Ảnh minh họa

---

## 📊 Data Analysis

Chi tiết phân tích so sánh dữ liệu JSON và hình ảnh: [betting_data_analysis.md](./betting_data_analysis.md)

**Kết quả**: ✅ Dữ liệu hoàn toàn khớp nhau giữa JSON và hình ảnh hiển thị.

---

## 💻 Application

Ứng dụng **C# WPF** hiển thị tỷ lệ cược với tự động cập nhật mỗi giây.

### ✨ Highlights

- 🔄 **Auto-refresh** mỗi 1 giây
- 🎨 **Professional UI** giống y hệt trang cá cược
- 📊 **Multiple odds types**: Handicap, Over/Under, 1X2
- 🏗️ **Clean architecture**: MVVM pattern
- 📚 **Complete documentation**: 8+ markdown files
- 🔧 **Highly customizable**: Colors, fonts, layout

### 📁 Project Structure

```
BettingOddsDisplay/
├── 📱 Application (XAML + C#)
├── 📦 Models (Data structures)
├── ⚙️ Services (Business logic)
├── 🎨 ViewModels (MVVM)
├── 🔄 Converters (XAML helpers)
├── 🧪 Tests (Unit tests)
└── 📚 Documentation (8 .md files)
```

---

## 🚀 Quick Start

### Prerequisites
- .NET 8.0 SDK
- Windows 10/11
- Visual Studio 2022 (optional)

### Install & Run

#### Option 1: Command Line (Fastest)
```bash
cd BettingOddsDisplay
dotnet restore
dotnet build
dotnet run
```

#### Option 2: Visual Studio
1. Open `BettingOddsDisplay/BettingOddsDisplay.sln`
2. Press F5

#### Option 3: Build Scripts
```bash
# Windows
cd BettingOddsDisplay
build.bat

# Linux/Mac
cd BettingOddsDisplay
chmod +x build.sh
./build.sh
```

### Build Standalone EXE
```bash
# Windows
cd BettingOddsDisplay
build-release.bat

# Output: bin/Release/net8.0-windows/win-x64/publish/BettingOddsDisplay.exe
```

---

## 📚 Documentation

### Quick Links

| Document | Description |
|----------|-------------|
| [📖 INDEX](./BettingOddsDisplay/INDEX.md) | Complete documentation index |
| [📘 README](./BettingOddsDisplay/README.md) | Main documentation |
| [🚀 Quick Start](./BettingOddsDisplay/QUICK_START.md) | Installation & setup |
| [🏗️ Architecture](./BettingOddsDisplay/ARCHITECTURE.md) | Design & patterns |
| [✨ Features](./BettingOddsDisplay/FEATURES.md) | Feature list |
| [🎨 Customization](./BettingOddsDisplay/CUSTOMIZATION.md) | How to customize |
| [🤝 Contributing](./BettingOddsDisplay/CONTRIBUTING.md) | Contribution guide |
| [📝 Changelog](./BettingOddsDisplay/CHANGELOG.md) | Version history |

### Project Summary
[📊 Complete Project Summary](./PROJECT_SUMMARY.md)

---

## ✨ Features

### ✅ Completed

#### Core
- [x] Auto-refresh every 1 second from API
- [x] Parse JavaScript response format
- [x] Display leagues and matches
- [x] Professional UI matching design

#### Odds Display
- [x] Handicap odds (Asian)
- [x] Over/Under odds
- [x] 1X2 odds (European)
- [x] Multiple odds lines per type

#### UI/UX
- [x] Color coding (red for negative, black for positive)
- [x] Responsive layout with scrolling
- [x] Status bar with update time
- [x] League grouping

#### Technical
- [x] MVVM architecture
- [x] HTTP client with custom headers
- [x] Timer service
- [x] Error handling
- [x] WPF data binding

### 🔄 Planned

- [ ] Filter by league/team
- [ ] Search functionality
- [ ] Favorites system
- [ ] Odds change notifications
- [ ] Historical data & charts
- [ ] Export to Excel/CSV
- [ ] Dark mode
- [ ] Settings UI

See [FEATURES.md](./BettingOddsDisplay/FEATURES.md) for complete list.

---

## 🖼️ Screenshots

### Main Window
```
┌────────────────────────────────────────────────────────┐
│ ⚽ Cập nhật tự động mỗi giây      Cập nhật lúc: 11:23:45│
├────────────────────────────────────────────────────────┤
│ ⭐ e-Football F24 International Friendly ⚽             │
├────┬──────────┬──────────────────────────────────────┤
│Time│  Match   │         Odds                          │
├────┼──────────┼──────────────────────────────────────┤
│11:00│ England │ 0-0.5  [0.78] [0.80]                 │
│Live│    vs    │  0.0   [0.69] [-0.97]                │
│    │ Belgium  │ 1X2    [2.13] [3.81] [2.38]          │
└────┴──────────┴──────────────────────────────────────┘
```

---

## 🛠️ Technology Stack

- **Language**: C# 12.0
- **Framework**: .NET 8.0
- **UI**: WPF (Windows Presentation Foundation)
- **Architecture**: MVVM (Model-View-ViewModel)
- **HTTP**: System.Net.Http.HttpClient
- **JSON**: Newtonsoft.Json
- **Timer**: System.Threading.Timer

---

## 📊 Statistics

- **Total Files**: 30+
- **Lines of Code**: ~2,500+
- **Documentation**: 8 markdown files
- **Models**: 5 classes
- **Services**: 2 classes
- **Tests**: Included

---

## 🔧 Customization

App is highly customizable:
- Refresh interval
- API URL & parameters
- Cookies & headers
- Colors & themes
- Font sizes
- Layout & spacing

See [CUSTOMIZATION.md](./BettingOddsDisplay/CUSTOMIZATION.md) for details.

---

## 🤝 Contributing

Contributions are welcome! See [CONTRIBUTING.md](./BettingOddsDisplay/CONTRIBUTING.md)

**Areas to contribute**:
- Bug fixes
- New features
- Documentation
- UI improvements
- Tests

---

## 📄 License

MIT License - See [LICENSE](./BettingOddsDisplay/LICENSE)

---

## 🆘 Support

- **Documentation**: Check [INDEX.md](./BettingOddsDisplay/INDEX.md)
- **Issues**: Create issue on GitHub
- **Questions**: Create discussion

---

## 🎯 Project Status

✅ **v1.0.0 Released** - December 18, 2025

All core features implemented and tested.

---

## 📞 Contact

- **Repository**: [GitHub URL]
- **Issues**: [Issues URL]
- **Discussions**: [Discussions URL]

---

**Made with ❤️ for betting odds enthusiasts**