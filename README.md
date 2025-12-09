# Client-Server Message

A simple client–server chat application built with WinForms (.NET Framework 4.7.2) and TCP sockets.

## 2. Requirements

- Windows
- .NET Framework 4.7.2
- Visual Studio (2019 or later recommended)

## 4. How to Run

### 4.1. Run the server

1. Open the folder:
   - `Server\bin\Debug\`
2. Run:
   - `Server.exe`
3. In the server window, click the button to start the server (e.g. `Start Server` / `Listen`).

Keep the server running, then start the client.

### 4.2. Run the client

1. Open the folder:
   - `Client\bin\Debug\`
2. Run:
   - `Client.exe`

If you are developing in Visual Studio, you can:

- Set `Server` as a startup project to run the server.
- Open another Visual Studio instance, set `Client` as a startup project and run it,  
  or run `Client.exe` directly from the `bin` folder.

## 5. Register / Login

### 5.1. Default accounts

User data is stored in:

- `Server\bin\Debug\Data\users.json`

You can inspect or edit this file to see existing users (for development only),  
or simply create a new account from the client using the Sign Up / Register form.
