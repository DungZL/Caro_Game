# 🎮 Caro Online — Cờ Caro Trực Tuyến

> Ứng dụng chơi cờ Caro (Gomoku) trực tuyến theo mô hình **Client–Server**, được xây dựng bằng **C# Windows Forms** và giao tiếp qua **TCP Socket**.

---

## 📋 Mục Lục

- [Tổng Quan](#-tổng-quan)
- [Công Nghệ Sử Dụng](#-công-nghệ-sử-dụng)
- [Kiến Trúc Hệ Thống](#-kiến-trúc-hệ-thống)
- [Cấu Trúc Dự Án](#-cấu-trúc-dự-án)
- [Giao Thức Truyền Thông](#-giao-thức-truyền-thông)
- [Chi Tiết Từng Module](#-chi-tiết-từng-module)
  - [CaroShare — Thư viện dùng chung](#1-caroshare--thư-viện-dùng-chung)
  - [Caro_Sever — Máy chủ](#2-caro_sever--máy-chủ)
  - [Caro_Clinet — Máy khách](#3-caro_clinet--máy-khách)
- [Luồng Hoạt Động](#-luồng-hoạt-động)
- [Thuật Toán Kiểm Tra Thắng](#-thuật-toán-kiểm-tra-thắng)
- [Hướng Dẫn Cài Đặt & Chạy](#-hướng-dẫn-cài-đặt--chạy)
- [Giao Diện Ứng Dụng](#-giao-diện-ứng-dụng)
- [Hạn Chế & Hướng Phát Triển](#-hạn-chế--hướng-phát-triển)

---

## 🎯 Tổng Quan

**Caro Online** là một ứng dụng chơi cờ Caro (Gomoku) trực tuyến cho phép hai người chơi đấu với nhau qua mạng. Hệ thống hoạt động theo mô hình Client–Server:

- **Server** đóng vai trò trung gian: tiếp nhận kết nối, ghép trận tự động, và chuyển tiếp nước đi giữa hai người chơi.
- **Client** là giao diện người chơi: kết nối đến server, tìm trận, và chơi cờ trên bàn cờ 15×15.

### Tính năng chính

| Tính năng | Mô tả |
|---|---|
| 🔗 Kết nối TCP | Giao tiếp ổn định qua TCP Socket |
| 🎲 Ghép trận tự động | Hệ thống hàng đợi FIFO ghép 2 người chơi |
| ⏱️ Đếm ngược lượt chơi | Mỗi lượt có **30 giây** để đánh |
| 🏳️ Đầu hàng | Người chơi có thể chủ động đầu hàng |
| 📊 Dashboard Server | Giám sát log hoạt động theo thời gian thực |
| 🔄 Chơi lại | Quay về phòng chờ sau khi kết thúc trận |

---

## 🛠 Công Nghệ Sử Dụng

| Thành phần | Công nghệ |
|---|---|
| **Ngôn ngữ** | C# |
| **Framework** | .NET 10.0 |
| **UI Framework** | Windows Forms |
| **Giao tiếp mạng** | `System.Net.Sockets` (TCP Socket) |
| **Encoding** | UTF-8 |
| **IDE** | Visual Studio |

---

## 🏗 Kiến Trúc Hệ Thống

```
┌─────────────────┐         TCP Socket         ┌─────────────────┐
│   Caro_Client    │ ◄──────────────────────── │                 │
│   (Player 1)     │ ────────────────────────► │   Caro_Server   │
└─────────────────┘                             │   (DashBoard)   │
                                                │                 │
┌─────────────────┐         TCP Socket         │  - Ghép trận    │
│   Caro_Client    │ ◄──────────────────────── │  - Chuyển tiếp  │
│   (Player 2)     │ ────────────────────────► │  - Quản lý log  │
└─────────────────┘                             └─────────────────┘
                                                        │
                                                        ▼
                                                ┌─────────────────┐
                                                │   CaroShare      │
                                                │   (Thư viện      │
                                                │    dùng chung)   │
                                                └─────────────────┘
```

> **Lưu ý:** Server **không lưu trạng thái bàn cờ**. Nó chỉ đóng vai trò **chuyển tiếp tin nhắn** (relay) giữa hai client. Logic xử lý game (đánh cờ, kiểm tra thắng) nằm hoàn toàn ở phía Client.

---

## 📁 Cấu Trúc Dự Án

```
Co_Caro_Game/
├── Co_Caro_Game.slnx            # Solution file
│
├── CaroShare/                   # 📦 Thư viện dùng chung
│   ├── CaroShare.csproj         #    Target: net10.0
│   └── Share.cs                 #    Helper gửi/nhận dữ liệu qua Socket
│
├── Caro_Sever/                  # 🖥️ Ứng dụng Server
│   ├── Caro_Sever.csproj        #    Target: net10.0-windows (WinForms)
│   ├── Program.cs               #    Entry point → DashBoard
│   ├── DashBoard.cs             #    Logic server chính
│   ├── DashBoard.Designer.cs    #    Giao diện DashBoard
│   └── DashBoard.resx           #    Resources
│
└── Caro_Clinet/                 # 🎮 Ứng dụng Client
    ├── Caro_Clinet.csproj       #    Target: net10.0-windows (WinForms)
    ├── Program.cs               #    Entry point → Room
    ├── Room.cs                  #    Phòng chờ / Lobby
    ├── Room.Designer.cs         #    Giao diện phòng chờ
    ├── Room.resx                #    Resources
    ├── Game_Board.cs            #    Bàn cờ / Logic chơi game
    ├── Game_Board.Designer.cs   #    Giao diện bàn cờ
    └── Game_Board.resx          #    Resources
```

---

## 📡 Giao Thức Truyền Thông

Tất cả tin nhắn được truyền dưới dạng **chuỗi text UTF-8**, các trường phân cách bằng ký tự `|`.

### Tin nhắn từ Client → Server

| Tin nhắn | Định dạng | Mô tả |
|---|---|---|
| Đăng ký tên | `NAME\|<tên>` | Gửi tên người chơi khi kết nối |
| Yêu cầu ghép trận | `MATCH` | Vào hàng đợi tìm đối thủ |
| Hủy ghép trận | `CANCEL` | Rời khỏi hàng đợi |
| Gửi nước đi | `MOVE\|<x>\|<y>` | Gửi tọa độ ô vừa đánh |
| Kết quả thắng | `RESULT\|<thông báo>` | Thông báo kết quả cho đối thủ |
| Đầu hàng | `SURRENDER` | Chủ động thua cuộc |
| Ngắt kết nối | `DISCONNECT` | Thoát khỏi server |

### Tin nhắn từ Server → Client

| Tin nhắn | Định dạng | Mô tả |
|---|---|---|
| Tìm thấy đối thủ | `FOUND\|<tên mình>\|<tên đối thủ>\|<1 hoặc 0>` | `1` = Player 1 (X, đi trước), `0` = Player 2 (O, đi sau) |
| Nước đi đối thủ | `MOVE\|<x>\|<y>` | Chuyển tiếp nước đi từ đối thủ |
| Kết quả | `RESULT\|<thông báo>` | Chuyển tiếp kết quả từ đối thủ |
| Đối thủ thoát | `DISCONNECT` | Thông báo đối thủ đã ngắt kết nối |

---

## 📦 Chi Tiết Từng Module

### 1. CaroShare — Thư viện dùng chung

📄 **File:** [`Share.cs`](CaroShare/Share.cs)

Thư viện tiện ích (static class) cung cấp 2 phương thức gửi/nhận dữ liệu qua Socket:

```csharp
public static class Share
{
    // Gửi chuỗi UTF-8 qua socket
    public static void Send(Socket sck, string msg);
    
    // Nhận chuỗi UTF-8 từ socket (buffer 1024 bytes)
    public static string Receive(Socket sck);
}
```

| Phương thức | Tham số | Mô tả |
|---|---|---|
| `Send()` | `Socket sck`, `string msg` | Chuyển đổi chuỗi sang byte UTF-8 và gửi |
| `Receive()` | `Socket sck` | Nhận tối đa 1024 bytes và giải mã thành chuỗi UTF-8 |

---

### 2. Caro_Sever — Máy chủ

📄 **File chính:** [`DashBoard.cs`](Caro_Sever/DashBoard.cs)

#### Các thành phần dữ liệu

| Biến | Kiểu | Mô tả |
|---|---|---|
| `sck` | `Socket?` | Socket lắng nghe kết nối |
| `isRunning` | `bool` | Trạng thái server đang chạy |
| `clientNames` | `Dictionary<Socket, string>` | Ánh xạ socket → tên người chơi |
| `matchQueue` | `Queue<Socket>` | Hàng đợi ghép trận (FIFO) |
| `opponents` | `Dictionary<Socket, Socket>` | Ánh xạ cặp đối thủ (hai chiều) |
| `lockObj` | `object` | Đối tượng đồng bộ hóa thread |

#### Các phương thức chính

| Phương thức | Chức năng |
|---|---|
| `button1_Click()` | Khởi tạo socket, bind IP/Port, bắt đầu lắng nghe |
| `ListenForClients()` | Vòng lặp `Accept()` chấp nhận kết nối mới, tạo thread riêng cho mỗi client |
| `HandleClient()` | Xử lý tin nhắn từ 1 client: `NAME`, `MATCH`, `CANCEL`, `MOVE`, `RESULT`, `SURRENDER`, `DISCONNECT` |
| `HandleMatchRequest()` | Thêm vào hàng đợi, nếu ≥2 người thì ghép trận và gửi `FOUND` cho cả hai |
| `HandleCancelMatch()` | Xóa client khỏi hàng đợi ghép trận |
| `HandleSurrender()` | Thông báo đối thủ thắng, xóa cặp đối thủ |
| `HandleDisconnect()` | Dọn dẹp tất cả dữ liệu liên quan đến client, thông báo đối thủ |
| `WriteLog()` | Ghi log thread-safe lên TextBox với timestamp `[HH:mm:ss]` |

#### Giao diện DashBoard

| Control | Kiểu | Chức năng |
|---|---|---|
| `Start` | `Button` | Nút khởi chạy server |
| `IP` | `TextBox` | Nhập địa chỉ IP |
| `Port` | `NumericUpDown` | Nhập số port |
| `Status` | `TextBox` | Hiển thị trạng thái server |
| `Log` | `TextBox (Multiline)` | Hiển thị log hoạt động |

---

### 3. Caro_Clinet — Máy khách

#### 3.1 Room — Phòng chờ (Lobby)

📄 **File:** [`Room.cs`](Caro_Clinet/Room.cs)

Đây là form khởi chạy đầu tiên của client, đóng vai trò phòng chờ.

| Phương thức | Chức năng |
|---|---|
| `Connect_Click()` | Tạo socket, kết nối đến server, gửi `NAME\|<tên>` |
| `Match_Click()` | Gửi `MATCH` để vào hàng đợi ghép trận |
| `Cancel_Click()` | Gửi `CANCEL` để hủy ghép |
| `ListenFromServer()` | Lắng nghe phản hồi: `FOUND` → mở `Game_Board`, `DISCONNECT` → reset giao diện |

**Luồng hoạt động khi tìm thấy trận:**
1. Nhận tin `FOUND|<tên mình>|<tên đối thủ>|<1 hoặc 0>`
2. Tạo `Game_Board` mới, truyền socket + thông tin trận đấu
3. Ẩn form `Room`, hiển thị `Game_Board`
4. Khi `Game_Board` đóng → hiện lại `Room`, khởi chạy lại `ListenFromServer()`

#### Giao diện Room

| Control | Kiểu | Chức năng |
|---|---|---|
| `Connect` | `Button` | Kết nối đến server |
| `Match` | `Button` | Tìm trận đấu |
| `Cancel` | `Button` | Hủy tìm trận |
| `IP` | `TextBox` | Nhập IP server |
| `Port` | `NumericUpDown` | Nhập port server |
| `PlayerName` | `TextBox` | Nhập tên người chơi |
| `Status` | `TextBox` | Hiển thị trạng thái kết nối |

---

#### 3.2 Game_Board — Bàn cờ

📄 **File:** [`Game_Board.cs`](Caro_Clinet/Game_Board.cs)

Form chính chứa logic chơi game.

#### Hằng số cấu hình

| Hằng số | Giá trị | Mô tả |
|---|---|---|
| `BOARD_SIZE` | `15` | Kích thước bàn cờ 15×15 |
| `CELL_SIZE` | `38` px | Kích thước mỗi ô cờ |
| `TURN_TIME` | `30` giây | Thời gian mỗi lượt |

#### Các thành phần dữ liệu

| Biến | Kiểu | Mô tả |
|---|---|---|
| `sck` | `Socket` | Socket kết nối đến server |
| `cells` | `Button[15,15]` | Ma trận 15×15 nút bấm (ô cờ) |
| `board` | `int[15,15]` | Ma trận trạng thái: `0`=trống, `1`=X, `2`=O |
| `isPlayer1` | `bool` | `true` nếu là Player 1 (đánh X) |
| `isMyTurn` | `bool` | `true` nếu đang là lượt mình |
| `gameOver` | `bool` | Cờ kết thúc trận |
| `timeLeft` | `int` | Số giây còn lại trong lượt |

#### Các phương thức chính

| Phương thức | Chức năng |
|---|---|
| `Game_Board()` | Constructor: tạo bàn cờ 15×15, cài đặt timer, khởi chạy luồng lắng nghe |
| `Cell_Click()` | Xử lý click ô cờ: đánh dấu, gửi `MOVE`, kiểm tra thắng, chuyển lượt |
| `ListenFromServer()` | Lắng nghe: `MOVE` (đối thủ đánh), `RESULT` (thua), `DISCONNECT` (đối thủ thoát) |
| `CheckWin()` | Kiểm tra 5 ô liên tiếp theo 4 hướng |
| `Timer1_Tick()` | Đếm ngược mỗi giây, hết giờ → thua |
| `Surrender_Click()` | Xác nhận đầu hàng, gửi `SURRENDER` |

#### Giao diện Game_Board

| Control | Kiểu | Chức năng |
|---|---|---|
| `Board` | `Panel` | Panel chứa bàn cờ (777×577 px) |
| `name_1` | `TextBox` | Hiển thị tên mình |
| `Name_2` | `TextBox` | Hiển thị tên đối thủ |
| `Timer` | `ProgressBar` | Thanh đếm ngược thời gian lượt |
| `Surrender` | `Button` | Nút đầu hàng |
| `timer1` | `Timer` | Timer tích mỗi 1 giây |

---

## 🔄 Luồng Hoạt Động

### Kịch bản hoàn chỉnh: Từ kết nối đến kết thúc trận

```
    Player A (Client)              Server                  Player B (Client)
         │                           │                           │
    ① Kết nối TCP ──────────────►    │                           │
         │                    Accept & tạo thread                │
         │               ◄────────── │                           │
    ② NAME|Alice ──────────────►     │                           │
         │              Lưu tên      │                           │
         │                           │                    ① Kết nối TCP
         │                           │    ◄──────────────────────│
         │                           │              ② NAME|Bob ──│
         │                           │     Lưu tên               │
         │                           │                           │
    ③ MATCH ──────────────────►      │                           │
         │              Vào hàng đợi │                           │
         │                           │    ◄───────────── ③ MATCH │
         │                    Ghép 2 người                       │
         │                           │                           │
    ④ ◄── FOUND|Alice|Bob|1         │                           │
         │  (X, đi trước)           │    FOUND|Bob|Alice|0 ──► ④
         │                           │        (O, đi sau)        │
         │                           │                           │
    ⑤ Mở Game_Board                 │              Mở Game_Board │
         │                           │                           │
    ⑥ Click ô (3,4) ──────────►     │                           │
       MOVE|3|4                Chuyển tiếp                       │
         │                           │    ──── MOVE|3|4 ──────► ⑦
         │                           │              Hiển thị X(3,4)
         │                           │                           │
         │                           │    ◄──── MOVE|5|5 ─────  ⑧
         │               Chuyển tiếp │                           │
    ⑨ ◄── MOVE|5|5                  │                           │
       Hiển thị O(5,5)              │                           │
         │                           │                           │
         │        ... (tiếp tục) ... │                           │
         │                           │                           │
    ⑩ Thắng (5 liên tiếp)          │                           │
       RESULT|Ban da thua! ────►     │                           │
         │               Chuyển tiếp │                           │
         │                           │ ── RESULT|Ban da thua! ► ⑪
         │                           │           Hiển thị thua   │
    ⑫ Đóng Game_Board               │            Đóng Game_Board │
       Quay về Room                  │            Quay về Room   │
```

### Các kịch bản kết thúc trận

| Kịch bản | Phía thắng | Phía thua | Tin nhắn |
|---|---|---|---|
| Thắng 5 ô | Người đánh đủ 5 | Đối thủ | `RESULT\|Ban da thua!` |
| Hết giờ | Đối thủ | Người hết giờ | `RESULT\|Doi thu het gio. Ban thang!` |
| Đầu hàng | Đối thủ | Người đầu hàng | `SURRENDER` → `RESULT\|Doi thu da dau hang. Ban thang!` |
| Ngắt kết nối | Đối thủ (còn lại) | Người ngắt | `DISCONNECT` |

---

## 🧠 Thuật Toán Kiểm Tra Thắng

Kiểm tra **5 ô liên tiếp** cùng quân từ vị trí vừa đánh, theo **4 hướng**:

```
    ↖  ↑  ↗       Hướng 1: Ngang     (dx=1, dy=0)   →
     \ | /        Hướng 2: Dọc       (dx=0, dy=1)   ↓
    ←──●──→       Hướng 3: Chéo chính (dx=1, dy=1)   ↘
     / | \        Hướng 4: Chéo phụ   (dx=1, dy=-1)  ↗
    ↙  ↓  ↘
```

**Thuật toán:**
1. Từ ô vừa đánh `(x, y)`, duyệt theo **2 chiều** trên mỗi hướng
2. Đếm số ô liên tiếp cùng quân (tối đa 4 ô mỗi chiều)
3. Tổng đếm ≥ 5 → **Thắng**

```csharp
private bool CheckWin(int x, int y, int mark)
{
    // 4 hướng: ngang, dọc, chéo chính, chéo phụ
    int[][] dx = { new[]{1,0}, new[]{0,1}, new[]{1,1}, new[]{1,-1} };
    
    for (int dir = 0; dir < 4; dir++)
    {
        int count = 1;
        // Đếm về một phía (+)
        for (int i = 1; i < 5; i++) { /* ... */ count++; }
        // Đếm về phía ngược lại (-)
        for (int i = 1; i < 5; i++) { /* ... */ count++; }
        if (count >= 5) return true;
    }
    return false;
}
```

> **Độ phức tạp:** O(1) — chỉ kiểm tra tối đa 4 × 8 = 32 ô xung quanh.

---

## 🚀 Hướng Dẫn Cài Đặt & Chạy

### Yêu cầu hệ thống

- **Hệ điều hành:** Windows
- **.NET SDK:** 10.0 trở lên
- **IDE:** Visual Studio 2022+ (khuyến nghị)

### Bước 1: Build solution

```bash
# Tại thư mục gốc dự án
dotnet build Co_Caro_Game.slnx
```

### Bước 2: Chạy Server

```bash
dotnet run --project Caro_Sever
```

1. Nhập **IP** (ví dụ: `127.0.0.1` cho localhost)
2. Nhập **Port** (ví dụ: `8888`)
3. Nhấn **"Khởi Chạy Sever"**
4. Quan sát log để theo dõi hoạt động

### Bước 3: Chạy Client (chạy 2 instance)

```bash
# Terminal 1
dotnet run --project Caro_Clinet

# Terminal 2
dotnet run --project Caro_Clinet
```

Trên mỗi client:
1. Nhập **IP** và **Port** giống server
2. Nhập **Tên Người Chơi**
3. Nhấn **"Kết Nối Đến Sever"**
4. Nhấn **"Ghép"** để tìm trận
5. Khi cả 2 client đều nhấn "Ghép" → Trận đấu bắt đầu!

---

## 🖥 Giao Diện Ứng Dụng

### Server — DashBoard (810 × 450 px)

```
┌────────────────────────────────────────────────────────────┐
│ [Khởi Chạy Sever]    IP [_________]    Port: [____↕]      │
│                                                            │
│ Trạng Thái: [Dang chay___]                                 │
│                          ┌────────────────────────────────┐│
│                          │ [12:00:01] Server da khoi chay ││
│                          │ [12:00:05] Mot client da ket.. ││
│                          │ [12:00:06] "Alice" da dang ky  ││
│                          │ [12:00:10] "Bob" da dang ky    ││
│                          │ [12:00:12] Ghep tran: "Alice"  ││
│                          │             vs "Bob"            ││
│                          └────────────────────────────────┘│
└────────────────────────────────────────────────────────────┘
```

### Client — Room (800 × 450 px)

```
┌────────────────────────────────────────────────────────┐
│ [Kết Nối Đến Sever]    IP [________]    Port: [___↕]   │
│                                                        │
│ Trạng Thái: [Da ket noi]     Tên Người Chơi [_______]  │
│                                                        │
│                                                        │
│            ┌──────────┐           ┌──────────┐         │
│            │          │           │          │         │
│            │   Ghép   │           │   Huỷ    │         │
│            │          │           │          │         │
│            └──────────┘           └──────────┘         │
└────────────────────────────────────────────────────────┘
```

### Client — Game_Board (1098 × 675 px)

```
┌──────────────────────────────────────────────────────────────┐
│ Name: [Alice___]     ████████████████████      Name: [Bob__] │
│                       (Timer Progress Bar)                   │
│        ┌─────────────────────────────────────────┐           │
│        │  ┌──┬──┬──┬──┬──┬──┬──┬──┬──┬──┐       │           │
│        │  │  │  │  │  │  │  │  │  │  │  │       │           │
│        │  ├──┼──┼──┼──┼──┼──┼──┼──┼──┼──┤       │           │
│        │  │  │  │ X│  │  │  │  │  │  │  │       │  ┌──────┐ │
│        │  ├──┼──┼──┼──┼──┼──┼──┼──┼──┼──┤       │  │      │ │
│        │  │  │  │  │ O│  │  │  │  │  │  │       │  │ Đầu  │ │
│        │  ├──┼──┼──┼──┼──┼──┼──┼──┼──┼──┤       │  │ Hàng │ │
│        │  │  │  │  │  │ X│  │  │  │  │  │       │  │      │ │
│        │  └──┴──┴──┴──┴──┴──┴──┴──┴──┴──┘       │  └──────┘ │
│        │          Bàn cờ 15×15 (Panel)           │           │
│        └─────────────────────────────────────────┘           │
└──────────────────────────────────────────────────────────────┘
```

---

## ⚠ Hạn Chế & Hướng Phát Triển

### Hạn chế hiện tại

| # | Hạn chế | Chi tiết |
|---|---|---|
| 1 | **Buffer cố định 1024 bytes** | `Share.Receive()` chỉ đọc tối đa 1024 bytes. Nếu tin nhắn dài hơn sẽ bị cắt. |
| 2 | **Không có framing protocol** | Nhiều tin nhắn có thể dính nhau (TCP là stream-based) hoặc bị tách ra. |
| 3 | **Không mã hóa** | Dữ liệu truyền dạng plain text, dễ bị đọc/chỉnh sửa. |
| 4 | **Không có xác thực** | Bất kỳ ai biết IP/Port đều có thể kết nối. |
| 5 | **Server không kiểm tra logic** | Client có thể gửi nước đi không hợp lệ. |
| 6 | **Không hỗ trợ phòng chơi** | Chỉ ghép trận ngẫu nhiên, không chọn đối thủ. |
| 7 | **Không lưu lịch sử** | Kết quả trận đấu không được lưu lại. |

### Hướng phát triển

- 🔐 **Bảo mật:** Thêm mã hóa TLS/SSL cho kết nối
- 🏠 **Phòng chơi:** Tạo/tham gia phòng riêng bằng mã mời
- 💾 **Lưu trữ:** Tích hợp database lưu lịch sử trận đấu, bảng xếp hạng
- 🤖 **AI:** Thêm chế độ chơi với máy (Minimax/Alpha-Beta)
- 💬 **Chat:** Hệ thống chat trong trận đấu
- 🎨 **Giao diện:** Nâng cấp UI hiện đại hơn (WPF/MAUI)
- 📊 **Server nâng cao:** Kiểm tra logic nước đi phía server để chống gian lận
- 🔄 **Reconnect:** Hỗ trợ tự động kết nối lại khi mất mạng

---

## 📄 Giấy Phép

Dự án được phát triển cho mục đích học tập và nghiên cứu.

---

> 📝 *Tài liệu được tạo tự động dựa trên phân tích mã nguồn dự án.*
> 
> 📅 *Cập nhật lần cuối: 29/06/2026*
