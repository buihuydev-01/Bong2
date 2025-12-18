# Phân tích so sánh dữ liệu JSON và Hình ảnh

## Dữ liệu từ JSON

### Giải đấu:
1. **427465**: e-Football F24 Elite Club Friendly (2 x 8 minutes)
2. **427468**: e-Football F24 International Friendly (2 x 8 minutes)

### Các trận đấu:

#### 1. e-England vs e-Belgium (Match ID: 9197896)
- Giải đấu: 427468 (International Friendly)
- Thời gian: 12/18/2025 11:00
- Market ID: 113752020

**Tỷ lệ cược từ JSON:**
- Handicap 0.25: [0.78, 0.80]
- Handicap 0.00: [0.69, -0.97]
- Handicap 0.50: [0.96, 0.62]
- 1X2: [2.13, 3.81, 2.38]
- Over/Under 2.0: [0.78, 0.66]

#### 2. e-Italy vs e-Denmark (Match ID: 9197897)
- Giải đấu: 427468 (International Friendly)
- Thời gian: 12/18/2025 11:00
- Market ID: 113752021

**Tỷ lệ cược từ JSON:**
- Handicap -0.50: [0.80, 0.92]
- Handicap -0.25: [0.90, 0.68]
- Handicap -0.75: [0.59, -0.87]
- 1X2: [2.94, 3.38, 1.94]
- Over/Under 1.50: [0.75, 0.69]

#### 3. e-Paris Saint Germain vs e-Real Madrid (Match ID: 9198111)
- Giải đấu: 427465 (Elite Club Friendly)
- Thời gian: 12/18/2025 11:00
- Market ID: 113752374

**Tỷ lệ cược từ JSON:**
- Handicap -0.75: [0.67, 0.77]
- Handicap -0.50: [-0.98, 0.70]
- Handicap -1.00: [0.60, -0.88]
- 1X2: [3.15, 3.97, 1.72]
- Over/Under 2.0: [0.68, 0.76]

#### 4. e-Lille vs e-Newcastle United (Match ID: 9198112)
- Giải đấu: 427465 (Elite Club Friendly)
- Thời gian: 12/18/2025 11:00
- Market ID: 113752375

**Tỷ lệ cược từ JSON:**
- Handicap -1.00: [0.80, 0.92]
- Handicap -0.75: [-0.96, 0.68]
- Handicap -1.25: [0.61, -0.89]
- 1X2: [3.79, 4.13, 1.55]
- Over/Under 2.0: [0.92, 0.80]

## So sánh với Hình ảnh

### ✅ e-England vs e-Belgium
- **Handicap 0-0.5**: 
  - Hình ảnh: 0.78/0.80, 0.69/-0.97, 0.96/0.62
  - JSON: [0.78,0.80] (0.25), [0.69,-0.97] (0.0), [0.96,0.62] (0.5)
  - **KHỚP ✓**

- **1X2**: 
  - Hình ảnh: 2.13, 3.81, 2.38
  - JSON: [2.13, 3.81, 2.38]
  - **KHỚP ✓**

- **Over/Under**:
  - Hình ảnh: 0.78/0.66 (2.0), 0.62/0.96 (1.5-2)
  - JSON: [0.78, 0.66] (2.0), [0.62, 0.96] (1.75)
  - **KHỚP ✓**

### ✅ e-Italy vs e-Denmark
- **Handicap**:
  - Hình ảnh: 0.80/0.92 (-0.50), 0.90/0.68 (-0.25), 0.59/-0.87 (-0.75)
  - JSON: [0.80,0.92] (-0.5), [0.90,0.68] (-0.25), [0.59,-0.87] (-0.75)
  - **KHỚP ✓**

- **1X2**:
  - Hình ảnh: 2.94, 3.38, 1.94
  - JSON: [2.94, 3.38, 1.94]
  - **KHỚP ✓**

- **Over/Under**:
  - Hình ảnh: 0.75/0.69 (1.50), -0.84/0.56 (1.75), 0.55/-0.83 (1.25)
  - JSON: [0.75,0.69] (1.5), [-0.84,0.56] (1.75), [0.55,-0.83] (1.25)
  - **KHỚP ✓**

### ✅ e-Paris Saint Germain vs e-Real Madrid
- **Handicap**:
  - Hình ảnh: 0.67/0.77, -0.98/0.70, 0.60/-0.88
  - JSON: [0.67,0.77] (-0.75), [-0.98,0.70] (-0.5), [0.60,-0.88] (-1.0)
  - **KHỚP ✓**

- **1X2**:
  - Hình ảnh: 3.15, 3.97, 1.72
  - JSON: [3.15, 3.97, 1.72]
  - **KHỚP ✓**

- **Over/Under**:
  - Hình ảnh: 0.68/0.76 (2.0), 0.64/-0.92 (1.75), -0.93/0.51 (2.25)
  - JSON: [0.68,0.76] (2.0), [0.64,-0.92] (1.75), [-0.93,0.51] (2.25)
  - **KHỚP ✓**

### ✅ e-Lille vs e-Newcastle United
- **Handicap**:
  - Hình ảnh: 0.80/0.92, -0.96/0.68, 0.61/-0.89
  - JSON: [0.80,0.92] (-1.0), [-0.96,0.68] (-0.75), [0.61,-0.89] (-1.25)
  - **KHỚP ✓**

- **1X2**:
  - Hình ảnh: 3.79, 4.13, 1.55
  - JSON: [3.79, 4.13, 1.55]
  - **KHỚP ✓**

- **Over/Under**:
  - Hình ảnh: 0.92/0.80 (2.0), 0.61/-0.89 (1.75), -0.81/0.53 (2.25)
  - JSON: [0.92,0.80] (2.0), [0.61,-0.89] (1.75), [-0.81,0.53] (2.25)
  - **KHỚP ✓**

## KẾT LUẬN

**✅ DỮ LIỆU HOÀN TOÀN KHỚP NHAU**

Tất cả các tỷ lệ cược trong JSON và hình ảnh đều khớp chính xác:
- ✓ Tên các trận đấu
- ✓ Thời gian (11:00 - 12/18/2025)
- ✓ Giải đấu (International Friendly & Elite Club Friendly)
- ✓ Tỷ lệ cược Handicap (Châu Á)
- ✓ Tỷ lệ cược 1X2 (Châu Âu)
- ✓ Tỷ lệ cược Over/Under (Tài/Xỉu)

Không phát hiện sự không nhất quán nào giữa dữ liệu JSON và hình ảnh hiển thị.
