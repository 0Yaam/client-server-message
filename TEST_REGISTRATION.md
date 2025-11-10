# Test Registration Feature

## Các tính n?ng ?ã implement:

### 1. Server Side (AuthManager.cs)
- Thêm method `Register()` ?? t?o tài kho?n m?i
- Validation username (không trùng, t?i thi?u 3 ký t?)
- Validation password (t?i thi?u 6 ký t?)
- Validation displayName (không ?? tr?ng)
- T?o salt và hash password an toàn
- L?u vào file users.json
- Method `GetAllUsers()` ?? l?y danh sách t?t c? users

### 2. Server Side (ClientSession.cs)
- X? lý message type "REGISTER" 
- G?i AuthManager.Register() và tr? v? k?t qu?
- ?óng connection sau khi x? lý ??ng ký

### 3. Client Side (SignUpForm.cs)
- Implement validation input ??y ??
- K?t n?i server và g?i message REGISTER
- X? lý response t? server
- Hi?n th? thông báo thành công/th?t b?i
- Chuy?n v? LoginForm sau khi ??ng ký thành công

### 4. Server UI (ServerForm.cs)
- ListView hi?n th? t?t c? users ?ã ??ng ký
- Hi?n th? c?t: Username, DisplayName, Role, Status (Online/Offline)
- Refresh t? ??ng m?i 2 giây
- Color coding cho online/offline users
- Menu "Thông tin" hi?n th? th?ng kê

### 5. Shared (Account.cs)
- Thêm thu?c tính DisplayName
- Thêm thu?c tính Avatar (m?c ??nh)

## Cách test:
1. Ch?y Server tr??c
2. Ch?y Client và click "??ng ký" 
3. Nh?p thông tin và ??ng ký
4. Ki?m tra trong ServerForm có hi?n th? user m?i
5. Th? ??ng nh?p v?i tài kho?n v?a t?o

## Avatar m?c ??nh:
- Hi?n t?i set avatar = "default.png" 
- Có th? customize sau này