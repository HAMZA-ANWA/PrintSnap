# PrintSnap — Digital Photo Printing System

An ASP.NET Core MVC web application that lets customers upload digital photographs, choose print sizes and copy counts, calculate the cost instantly, place a photo-printing order (pay at a branch or by credit card), and lets administrators review orders and manage print prices.

This project was built as an academic assignment covering the customer requirements **RS1–RS6** (see [Requirements](#requirements-rs1--rs6)).

---

## Table of Contents

- [Tech Stack](#tech-stack)
- [Features](#features)
- [Requirements RS1 – RS6](#requirements-rs1--rs6)
- [Project Structure](#project-structure)
- [Getting Started](#getting-started)
- [Database & Migrations](#database--migrations)
- [Page / Route Map](#page--route-map)
- [Authentication & Sessions](#authentication--sessions)
- [Uploads & File Storage](#uploads--file-storage)
- [Pricing](#pricing)
- [Known Issues & Limitations](#known-issues--limitations)
- [License](#license)

---

## Tech Stack

| Layer          | Technology                                   |
|----------------|----------------------------------------------|
| Framework      | .NET 8.0 (ASP.NET Core MVC)                  |
| ORM            | Entity Framework Core 8.0                    |
| Database       | SQL Server (LocalDB / SQLEXPRESS)            |
| UI             | Razor views + Tailwind CSS (CDN) + Font Awesome |
| Client scripts | Bootstrap 5, jQuery, jQuery Validation (bundled) |
| State          | Server-side sessions (30 minute timeout)     |

SDK requirement: **.NET SDK 8.0.100 or newer** (see `global.json`).

---

## Features

### Customer Side
- **User registration & login** (`AccountController`) with duplicate-email detection.
- **Session-based sign-in** with a 30-minute idle timeout.
- **Photo order creation** (`OrderController` / `PhotoOrdersController`):
  - Upload one or more JPEG/PNG photos.
  - Select a print size: `4x6` ($10), `5x7` ($15), `8x10` ($25), `A4` ($35).
  - Choose the number of copies per photo.
  - Pick a payment mode: **Direct Payment in Branch** or **Credit Card**.
  - Instant client-side cost calculator and a confirmation receipt.
- **Server-side file storage** — uploaded photos are saved under `wwwroot/uploads/folder_XXXX`.

### Admin Side
- **Dashboard** (`/Admin/Dashboard`) with totals for orders, revenue and customers, plus the 10 most recent orders.
- **Manage Orders** (`/Admin/ManageOrders`) — view all photo orders and delete them (deletes the associated upload folder too).
- **Manage Prices** (`/Admin/ManagePrices`) — add and delete print-size/price rows stored in the `PrintPrices` table.

### Misc
- Razor view rendering with shared Tailwind layout.
- Anti-forgery token validation on all POST forms.
- HTTPS redirection and HSTS in non-development environments.

---

## Requirements RS1 – RS6

The homepage lists the assignment requirements this project targets:

| Req  | Description                                                                  | Status in code                                                    |
|------|------------------------------------------------------------------------------|-------------------------------------------------------------------|
| RS1  | JPEG photo upload from the desktop                                          | Implemented in order creation                                     |
| RS2  | Instant cost calculation based on size + copies                              | Implemented (client JS + server switch)                           |
| RS3  | Secure credit card encryption (or branch payment)                            | **Partial / not working** — card is stored plaintext, see [Known Issues](#known-issues--limitations) |
| RS4  | Purchase order ID auto-generation + `folder_XXXX` server folders             | **Partial** — folders are created, purchase-order flow is not wired up |
| RS5  | Admin dashboard to view orders and update print prices                       | Implemented (dashboard, order list, price CRUD)                   |
| RS6  | Automatic deletion of printed photo folders after shipping                   | **Not implemented** — no "ship" action exists                     |

---

## Project Structure

```
DigitalPhotoPrintingSystem/
├── Controllers/            # MVC controllers
│   ├── AccountController.cs      # Customer register / login / logout
│   ├── AdminController.cs        # Dashboard, manage orders, manage prices
│   ├── CustomerController.cs     # (duplicate, minimal register endpoint)
│   ├── HomeController.cs         # Landing page
│   ├── OrderController.cs        # Order creation + receipt
│   ├── PhotoOrdersController.cs  # (near-duplicate order creation + receipt)
│   └── UserController.cs         # Customer dashboard / order history
├── Data/
│   └── ApplicationDbContext.cs   # EF Core DbContext (Customers, PhotoOrders, PrintPrices, PurchaseOrders)
├── Migrations/             # EF Core migrations (26 migration files + model snapshot)
├── Models/                 # POCO / ViewModel classes
├── Services/
│   └── EncryptionHelper.cs # AES helper (currently unused)
├── Views/                  # Razor views, grouped by controller
├── wwwroot/                # Static assets (css, js, lib) + uploads/
├── appsettings.json        # Connection string + logging config
├── Program.cs              # App bootstrap, services, middleware pipeline
├── DigitalPhotoPrintingSystem.csproj
└── global.json             # .NET SDK version pinning
```

### Data Model (tables)

| Table          | Purpose                                                            |
|----------------|--------------------------------------------------------------------|
| `Customers`    | Registered customers (`CustId`, name, DOB, gender, phone, address, email, password) |
| `PhotoOrders`  | Photo print orders (`PrintSize`, `Quantity`, `UnitPrice`, `TotalPrice`, status, uploaded folder, card data, ...) |
| `PrintPrices`  | Admin-managed print sizes and prices                                |
| `PurchaseOrders` | Purchase-order records (currently unused — no code writes to it) |

---

## Getting Started

### Prerequisites
- **.NET SDK 8.0** (pinned via `global.json`)
- **SQL Server** instance reachable at the connection string in `appsettings.json`
  - Default: `Server=.\SQLEXPRESS;Database=PhotoPrintingDB;Trusted_Connection=True;...`
  - If you use LocalDB instead, change to:
    `Server=(localdb)\MSSQLLocalDB;Database=PhotoPrintingDB;Trusted_Connection=True;TrustServerCertificate=True`

### Setup steps

1. **Restore packages**
   ```bash
   dotnet restore
   ```

2. **Create the database and apply migrations**
   ```bash
   dotnet ef database update
   ```
   This creates the `PhotoPrintingDB` database and all tables from the migrations folder.
   > The app itself does **not** auto-create or migrate the database at startup — you must run this step before the app will work.

3. **Run the app**
   ```bash
   dotnet run
   ```
   - HTTP profile: `http://localhost:5230`
   - HTTPS profile: `https://localhost:7224` / `http://localhost:5230`
   - Use Visual Studio's **https** launch profile to get both.

4. **Browse**
   - Home page: `http://localhost:5230/`
   - Register: `/Account/Register`
   - Login: `/Account/Login`
   - Create order: `/PhotoOrders/Create`
   - Admin dashboard: `/Admin/Dashboard`

> **Note on first run:** the app calls `app.UseAuthentication()` without registering authentication services, which can throw at startup (`InvalidOperationException`). See [Known Issues](#known-issues--limitations). A quick workaround is to comment out the `UseAuthentication()` line in `Program.cs`, since the app's real auth is session-based.

---

## Database & Migrations

Migrations are applied in chronological order. The final schema (per `ApplicationDbContextModelSnapshot.cs`) contains:

- `Customers` (PK `CustId`, `Email` unique-ish enforced only in code, `Password` **plaintext**)
- `PhotoOrders` (PK `Id`, plus `OrderId` int column left over from an earlier design — currently marked `[NotMapped]` in the model)
- `PrintPrices` (PK `Id`, `SizeName`, `Price`)
- `PurchaseOrders` (PK `Id`, optional `CustId` FK)

Useful commands:

```bash
dotnet ef migrations add <Name>        # add a new migration
dotnet ef database update               # apply migrations
dotnet ef database update <Migration>   # roll forward/back to a migration
```

> **Version warning:** the project references EF Core `8.0.0`, but the model snapshot was generated with **EF Core 10.0.10**. The `__EFMigrationsHistory` / snapshot version mismatch can cause tooling warnings and model drift. If `dotnet ef` fails, install a matching tool version:
> ```bash
> dotnet tool install --global dotnet-ef --version 8.0.0
> ```

---

## Page / Route Map

| Route                     | Controller / Action              | Purpose                                | Auth |
|---------------------------|----------------------------------|----------------------------------------|------|
| `/`                       | `Home / Index`                   | Landing page                           | —    |
| `/Account/Register`       | `Account / Register`             | Customer sign-up                       | —    |
| `/Account/Login`          | `Account / Login`                | Customer sign-in                       | —    |
| `/Account/Logout`         | `Account / Logout`               | Sign out, clear session                | ✓    |
| `/Order/Create`           | `Order / Create`                 | Create photo order (form)              | —    |
| `/Order/OrderSuccess`     | `Order / OrderSuccess`           | Order receipt                          | —    |
| `/PhotoOrders/Create`     | `PhotoOrders / Create`           | Same order form (posts to `/Order`)    | —    |
| `/PhotoOrders/Index`      | `PhotoOrders / Index`            | Redirects to `/Admin/ManageOrders`     | ✓    |
| `/Admin/Dashboard`        | `Admin / Dashboard`              | Stats + recent orders                  | ✓*   |
| `/Admin/ManageOrders`     | `Admin / ManageOrders`           | All orders + delete                    | ✓*   |
| `/Admin/ManagePrices`     | `Admin / ManagePrices`           | Add / delete print prices              | ✓*   |
| `/User/Dashboard`         | `User / Dashboard`               | Customer order summary                 | ✓    |
| `/User/OrderHistory`      | `User / OrderHistory`            | Customer order list                    | ✓    |

`✓*` = "logged-in" check only; **any** logged-in customer passes it (see Known Issues).

---

## Authentication & Sessions

- Authentication is **session-based**, not ASP.NET Identity:
  - On login/register, `CustId`, `UserName` and `CustomerEmail` are written to the session.
  - Session cookie is `HttpOnly`, essential, with a **30-minute idle timeout** (`Program.cs`).
- Guards are custom checks like `HttpContext.Session.GetString("CustId") != null`.
- Passwords are stored **in plaintext** — do not use real credentials here.
- Credit card numbers are saved in the `EncryptedCreditCardNumber` column but are **not actually encrypted** in the current flow.

---

## Uploads & File Storage

- Uploaded photos are stored per order under:
  ```
  wwwroot/uploads/folder_XXXX/
  ```
  where `XXXX` is the order ID padded to 4 digits (e.g. order 42 → `folder_0042`).
- The folder name is recorded on the `PhotoOrder.FolderName` column.
- Deleting an order also deletes its upload folder (`AdminController.DeleteOrder`).
- Uploads are publicly served from `wwwroot/uploads`.

> **Security note:** file names come straight from the client upload (`photo.FileName`) with no sanitization, and there is no server-side file-type/size validation. See Known Issues.

---

## Pricing

Prices are **hardcoded** in the order controllers:

| Print Size | Price |
|------------|-------|
| 4x6        | $10.00 |
| 5x7        | $15.00 |
| 8x10       | $25.00 |
| A4         | $35.00 |

Total = `price × copies × number of photos`.

The admin-facing `PrintPrices` table lets admins add/delete sizes, but **order pricing does not read from that table** — changing prices in the admin panel has no effect on order totals.

---

## Known Issues & Limitations

These are documented from a code review and should be fixed before treating this as a production system:

1. **Startup failure (potential):** `Program.cs` calls `app.UseAuthentication()` without `services.AddAuthentication()`. If the app throws `InvalidOperationException` at startup, comment out the `UseAuthentication()` line (session auth does not need it).
2. **User dashboard dead:** `UserController` reads the session with `GetInt32("CustId")`, but the account controller stores it as a string — the user dashboard/order history always redirect back to login.
3. **Purchase-order flow not wired:** `PurchaseOrders` is never written to; the checkout view posts to a non-existent `PhotoOrder` controller / `ProcessCheckout` action (404).
4. **"Encryption" is not encryption:** credit card numbers are stored plaintext in `PhotoOrders.EncryptedCreditCardNumber`; `EncryptionHelper` is unused, and its key/IV are hardcoded anyway.
5. **No admin separation:** any logged-in customer can access the admin area; there is no admin login. Passwords are stored plaintext.
6. **Unsafe file uploads:** untrusted file names are used directly in paths (path-traversal risk) and there is no server-side file type/size validation.
7. **Price management is cosmetic:** admin price edits do not affect order totals.
8. **No order lifecycle:** orders stay `Pending` forever — there is no "mark shipped" / cleanup (RS6) implementation.
9. **Duplicate logic:** `OrderController` and `PhotoOrdersController` duplicate the same order-creation flow.
10. **EF Core version mismatch:** migrations were generated with EF Core 10.x tooling while the project references EF Core 8.0.0.
11. **Dead code & repo hygiene:** several unused controllers/views/models, an empty `Register.aspx`, a stray 0-byte `-w` file, and no `.gitignore` (build output, `.vs`, and test uploads are committed).

---

## License

No license file is included. This project is for educational/academic use only.
