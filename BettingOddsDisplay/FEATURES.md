# Danh sách tính năng

## ✅ Đã hoàn thành

### Core Features
- [x] **Auto-refresh mỗi 1 giây**: Tự động cập nhật dữ liệu từ API
- [x] **Parse JavaScript response**: Chuyển đổi response dạng `$M('odds-display').onUpdate()` thành objects
- [x] **Display leagues**: Nhóm trận đấu theo giải đấu
- [x] **Display matches**: Hiển thị thông tin trận đấu (đội nhà, đội khách, thời gian)

### Odds Display
- [x] **Handicap odds (Cược chấp)**: Hiển thị kèo châu Á
- [x] **Over/Under odds (Tài/Xỉu)**: Hiển thị kèo tài xỉu
- [x] **1X2 odds**: Hiển thị kèo châu Âu
- [x] **Multiple odds lines**: Hiển thị nhiều mức kèo cho mỗi loại

### UI/UX
- [x] **Color coding**: Đỏ cho tỷ lệ âm, đen cho tỷ lệ dương
- [x] **Responsive layout**: Scroll khi có nhiều dữ liệu
- [x] **Status bar**: Hiển thị thời gian cập nhật và trạng thái
- [x] **League headers**: Header riêng cho mỗi giải đấu
- [x] **Professional design**: Giao diện giống trang cá cược thật

### Technical
- [x] **HTTP client**: Gửi request với headers và cookies
- [x] **Timer service**: Quản lý việc cập nhật tự động
- [x] **MVVM pattern**: Separation of concerns
- [x] **Data binding**: WPF data binding cho UI updates
- [x] **Error handling**: Xử lý lỗi network và parsing

## 🔄 Có thể mở rộng

### Filtering & Search
- [ ] **Filter by league**: Lọc trận theo giải đấu
- [ ] **Search teams**: Tìm kiếm theo tên đội
- [ ] **Filter by time**: Lọc theo giờ thi đấu
- [ ] **Filter by odds type**: Chỉ hiển thị loại kèo cụ thể

### Favorites & Notifications
- [ ] **Favorite teams**: Đánh dấu đội yêu thích
- [ ] **Favorite matches**: Đánh dấu trận yêu thích
- [ ] **Odds change alerts**: Thông báo khi tỷ lệ thay đổi lớn
- [ ] **Match start alerts**: Thông báo khi trận sắp bắt đầu

### Data Analysis
- [ ] **Odds history**: Lưu lại lịch sử thay đổi tỷ lệ
- [ ] **Odds comparison**: So sánh tỷ lệ giữa các thời điểm
- [ ] **Charts**: Biểu đồ thay đổi tỷ lệ theo thời gian
- [ ] **Statistics**: Thống kê về tỷ lệ

### Export & Import
- [ ] **Export to Excel**: Xuất dữ liệu ra Excel
- [ ] **Export to CSV**: Xuất dữ liệu ra CSV
- [ ] **Export to JSON**: Xuất dữ liệu ra JSON
- [ ] **Import settings**: Import cấu hình từ file

### Multiple Sources
- [ ] **Multiple API sources**: Hỗ trợ nhiều nguồn API
- [ ] **Source comparison**: So sánh tỷ lệ từ nhiều nguồn
- [ ] **Best odds finder**: Tìm tỷ lệ tốt nhất

### Advanced UI
- [ ] **Dark mode**: Giao diện tối
- [ ] **Custom themes**: Tùy chỉnh màu sắc
- [ ] **Layout customization**: Tùy chỉnh bố cục
- [ ] **Font size adjustment**: Điều chỉnh cỡ chữ
- [ ] **Column visibility**: Ẩn/hiện cột

### Live Features
- [ ] **Live scores**: Hiển thị tỷ số trực tiếp
- [ ] **Live match events**: Hiển thị sự kiện trong trận
- [ ] **Live chat**: Chat với người dùng khác
- [ ] **Live streaming links**: Link xem trực tiếp

### Settings & Configuration
- [ ] **Custom refresh interval**: Tùy chỉnh tần suất cập nhật
- [ ] **Custom API URL**: Tùy chỉnh URL API
- [ ] **Proxy support**: Hỗ trợ proxy
- [ ] **Language selection**: Chọn ngôn ngữ hiển thị

### Database & Cache
- [ ] **Local database**: Lưu dữ liệu vào database
- [ ] **Cache mechanism**: Cache để giảm số request
- [ ] **Offline mode**: Xem dữ liệu đã lưu khi offline

## 🎯 Roadmap

### Phase 1 (Current)
- ✅ Basic odds display
- ✅ Auto-refresh
- ✅ Professional UI

### Phase 2 (Next)
- [ ] Filtering & search
- [ ] Favorites
- [ ] Settings UI

### Phase 3 (Future)
- [ ] Odds history & charts
- [ ] Multiple sources
- [ ] Advanced analytics

### Phase 4 (Long-term)
- [ ] Live scores
- [ ] Mobile version
- [ ] Web version

## 💡 Suggestions Welcome

Nếu bạn có ý tưởng cho tính năng mới, vui lòng:
1. Tạo issue trên GitHub
2. Mô tả tính năng chi tiết
3. Giải thích use case

## 🐛 Known Issues

Hiện tại chưa có issue nào được báo cáo.

## 📝 Notes

- Tất cả tính năng được đánh dấu [x] đã được implement và test
- Tính năng đánh dấu [ ] có thể được implement trong tương lai
- Ưu tiên phát triển theo nhu cầu người dùng
