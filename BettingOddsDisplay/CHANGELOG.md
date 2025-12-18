# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [1.0.0] - 2025-12-18

### Added
- Initial release
- Auto-refresh every 1 second from API
- Parse JavaScript response format `$M('odds-display').onUpdate()`
- Display leagues and matches
- Display Handicap odds (Cược chấp)
- Display Over/Under odds (Tài/Xỉu)
- Display 1X2 odds (Châu Âu)
- Multiple odds lines per bet type
- Color coding for positive/negative odds
- Professional UI matching betting site design
- Status bar with update time
- League grouping
- Responsive layout with scrolling
- HTTP client with custom headers and cookies
- Timer service for auto-updates
- Error handling for network and parsing
- MVVM architecture
- WPF data binding

### Documentation
- README.md with installation guide
- QUICK_START.md for quick setup
- ARCHITECTURE.md explaining project structure
- CUSTOMIZATION.md for customization guide
- FEATURES.md listing all features
- CONTRIBUTING.md for contributors
- Build scripts (build.bat, build.sh)
- Sample response data

### Project Structure
- Models for data representation
- Services for business logic
- ViewModels for UI binding
- Converters for data formatting
- Clean separation of concerns

## [Unreleased]

### Planned
- Filter by league
- Search functionality
- Favorite teams/matches
- Odds change notifications
- Historical data tracking
- Charts and analytics
- Export to Excel/CSV
- Multiple API sources
- Dark mode
- Settings UI
- Database integration

---

## Version History

### Version Numbering

- **Major.Minor.Patch**
- **Major**: Breaking changes
- **Minor**: New features, backwards compatible
- **Patch**: Bug fixes, minor improvements

### Release Dates

- **v1.0.0**: December 18, 2025 - Initial Release

---

## How to Update

### From Source

```bash
git pull origin main
dotnet restore
dotnet build
```

### Binary Release

1. Download latest release from GitHub Releases
2. Extract and run BettingOddsDisplay.exe

---

## Migration Guide

### v1.0.0 → v1.x.x

No migration needed for v1.x updates.

---

## Support

For issues or questions about specific versions:
- Check [CHANGELOG.md](./CHANGELOG.md)
- Check [GitHub Issues](https://github.com/yourusername/BettingOddsDisplay/issues)
- Create new issue with version number

---

**Note**: This is a living document and will be updated with each release.
