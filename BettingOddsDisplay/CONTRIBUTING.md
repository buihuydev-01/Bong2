# Contributing to Betting Odds Display

Cảm ơn bạn đã quan tâm đến việc đóng góp cho dự án! 

## Quy trình đóng góp

### 1. Fork và Clone

```bash
# Fork repository trên GitHub
# Clone fork về máy của bạn
git clone https://github.com/YOUR_USERNAME/BettingOddsDisplay.git
cd BettingOddsDisplay
```

### 2. Tạo Branch mới

```bash
# Tạo branch cho feature/bugfix
git checkout -b feature/your-feature-name
# hoặc
git checkout -b bugfix/your-bugfix-name
```

### 3. Thực hiện thay đổi

- Viết code clean và có comments
- Follow coding conventions hiện có
- Test kỹ càng trước khi commit

### 4. Commit changes

```bash
git add .
git commit -m "feat: Add your feature description"
# hoặc
git commit -m "fix: Fix your bug description"
```

**Commit message format**:
- `feat:` - Tính năng mới
- `fix:` - Sửa bug
- `docs:` - Cập nhật documentation
- `style:` - Format code, không ảnh hưởng logic
- `refactor:` - Refactor code
- `test:` - Thêm tests
- `chore:` - Maintenance tasks

### 5. Push và tạo Pull Request

```bash
git push origin feature/your-feature-name
```

Sau đó tạo Pull Request trên GitHub.

## Coding Standards

### C# Code Style

```csharp
// ✅ GOOD
public class MyClass
{
    private readonly string _myField;
    
    public string MyProperty { get; set; }
    
    public void MyMethod()
    {
        // Code here
    }
}

// ❌ BAD
public class myclass {
    private string myField;
    public string myproperty { get; set; }
    public void mymethod() {
        // Code here
    }
}
```

**Rules**:
- Classes: PascalCase
- Methods: PascalCase
- Properties: PascalCase
- Private fields: _camelCase with underscore
- Local variables: camelCase
- Constants: PascalCase or UPPER_CASE
- Indentation: 4 spaces
- Braces: Opening brace on new line

### XAML Style

```xml
<!-- ✅ GOOD -->
<Border Background="#2E5090" 
        Padding="10,5"
        BorderThickness="1">
    <TextBlock Text="Hello" />
</Border>

<!-- ❌ BAD -->
<Border Background="#2E5090" Padding="10,5" BorderThickness="1"><TextBlock Text="Hello" /></Border>
```

**Rules**:
- One attribute per line for multiple attributes
- Proper indentation
- Close tags properly

### Naming Conventions

**Files**:
- Models: `Match.cs`, `League.cs`
- Services: `OddsDataService.cs`, `ResponseParser.cs`
- Views: `MainWindow.xaml`
- ViewModels: `LeagueViewModel.cs`

**Folders**:
- Models/, Services/, ViewModels/, Converters/, etc.
- PascalCase cho folder names

## Testing

### Trước khi submit PR

1. **Build thành công**:
```bash
dotnet build
```

2. **Không có warnings**:
```bash
dotnet build /p:TreatWarningsAsErrors=true
```

3. **Test chức năng**:
- Chạy app
- Test feature mới
- Test các features cũ không bị ảnh hưởng

### Unit Tests (nếu có)

```bash
dotnet test
```

## Pull Request Guidelines

### PR Title Format

```
feat: Add dark mode support
fix: Fix odds parsing for handicap 0
docs: Update README with installation guide
```

### PR Description Template

```markdown
## Description
Brief description of changes

## Type of Change
- [ ] Bug fix
- [ ] New feature
- [ ] Breaking change
- [ ] Documentation update

## Testing
- [ ] Built successfully
- [ ] Tested manually
- [ ] No warnings

## Screenshots (if applicable)
[Add screenshots here]

## Related Issues
Closes #123
```

### Review Process

1. Maintainer sẽ review code
2. Có thể yêu cầu changes
3. Sau khi approved, PR sẽ được merge

## Areas to Contribute

### 🐛 Bug Fixes
- Fix parsing errors
- Fix UI glitches
- Fix memory leaks
- Fix network issues

### ✨ New Features
- Filter và search
- Favorites system
- Notifications
- Export data
- Multiple API sources
- Historical data tracking
- Charts và analytics

### 📚 Documentation
- Improve README
- Add code comments
- Write tutorials
- Translate docs

### 🎨 UI/UX Improvements
- Better layout
- More themes
- Responsive design
- Accessibility

### 🧪 Testing
- Add unit tests
- Add integration tests
- Improve test coverage

## Code Review Checklist

**Trước khi submit PR, check:**

- [ ] Code builds without errors
- [ ] No compiler warnings
- [ ] Code follows style guide
- [ ] Added comments for complex logic
- [ ] Updated documentation if needed
- [ ] Tested thoroughly
- [ ] No hardcoded values (use constants/config)
- [ ] No sensitive data (API keys, passwords)
- [ ] Proper error handling
- [ ] No console.log/debug code left

## Getting Help

- **Questions**: Tạo issue với label `question`
- **Bugs**: Tạo issue với label `bug`
- **Feature requests**: Tạo issue với label `enhancement`
- **Discord**: [Link nếu có]

## Code of Conduct

### Our Standards

- Respectful và professional
- Welcoming đến contributors mới
- Constructive feedback
- Focus on code, not people

### Unacceptable Behavior

- Harassment hoặc discrimination
- Trolling hoặc insulting comments
- Personal attacks
- Publishing others' private information

## License

Bằng việc contribute, bạn đồng ý rằng contributions của bạn sẽ được license dưới cùng license với project.

## Recognition

Contributors sẽ được list trong:
- README.md
- CONTRIBUTORS.md
- Release notes

---

**Thank you for contributing! 🎉**

Mọi contribution đều được đánh giá cao, từ bug reports đến major features!
