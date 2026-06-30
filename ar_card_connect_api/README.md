# ar-card-connect-api

ASP.NET Core Web API for **AR Card Connect** — digital business cards with QR markers, file uploads, and role-based access.

Pairs with the Flutter app: [ar_card_connect](https://github.com/Development-Bulat/flutter-mobile-apps/tree/main/ar_card_connect).

## Features

- User registration and authentication (JWT Bearer)
- CRUD for business cards (image, 3D model, contacts, template, marker ID)
- Public card lookup by `markerId` (for AR / QR scan)
- User profile management
- Admin: all users/cards, block/unblock users and cards
- Swagger UI in Development
- Static file hosting: `wwwroot/cards`, `wwwroot/models`, `wwwroot/avatars`

## Stack

- .NET 8 / ASP.NET Core Web API
- Entity Framework Core
- PostgreSQL (Npgsql)
- JWT authentication
- Swagger (OpenAPI)
- Supabase.Storage (optional file storage)

## Database

- PostgreSQL database: `ar_card_connect`
- Schema via EF Core migrations (`Migrations/`)

### Apply migrations

```bash
cd Ar_Card_Connect_Api
dotnet ef database update
```

## Configuration

Secrets are **not** stored in `appsettings.json`. Use **User Secrets** (local dev) or environment variables (production).

### User Secrets (recommended for local dev)

```bash
cd Ar_Card_Connect_Api
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=localhost;Port=5432;Database=ar_card_connect;Username=YOUR_USER;Password=YOUR_PASSWORD"
dotnet user-secrets set "JWT:Key" "your-secret-key-min-32-chars"
dotnet user-secrets set "JWT:Issuer" "ArCardConnect"
```

Or copy the template: `appsettings.Development.example.json` → `appsettings.Development.json` (gitignored).

Default URLs: `http://localhost:5255`

## Run locally

### Requirements

- [.NET SDK 8](https://dotnet.microsoft.com/download)
- PostgreSQL

```bash
cd Ar_Card_Connect_Api
dotnet restore
dotnet run
```

Swagger: `http://localhost:5255/swagger`

## API endpoints (overview)

| Method | Route | Auth | Description |
|--------|-------|------|-------------|
| POST | `/api/User/register` | — | Register |
| POST | `/api/User/login` | — | Login → JWT |
| GET | `/api/User/getCardByMarkerId` | — | Public card by marker |
| POST | `/api/User/createCard` | User/Admin | Create card |
| PUT | `/api/User/updateCard` | User/Admin | Update card |
| GET | `/api/User/getMyCards` | User/Admin | My cards |
| GET | `/api/User/getMyProfile` | User/Admin | My profile |
| PUT | `/api/User/updateProfile` | User/Admin | Update profile |
| POST | `/api/User/changePassword` | User/Admin | Change password |
| GET | `/api/User/getAllUsers` | Admin | All users |
| GET | `/api/User/getAllCards` | Admin | All cards |
| POST | `/api/User/blockUser` | Admin | Block user |
| POST | `/api/User/unblockUser` | Admin | Unblock user |
| POST | `/api/User/blockCard` | Admin | Block card |
| POST | `/api/User/unblockCard` | Admin | Unblock card |

Authorization header: `Bearer {jwt_token}`

## Project structure

```
Ar_Card_Connect_Api/
├── Controllers/    # UserController
├── Services/       # business logic
├── Models/         # UserProfile, UserCard, Role
├── DataBase/       # AppDbContext
├── Migrations/     # EF Core
├── JWT/            # token generation, role authorize
├── Requests/       # DTOs
└── wwwroot/        # uploaded files
```

## Related

| Part | Repo |
|------|------|
| Flutter app | ar_card_connect |
| API | this repo |

## Author

GitHub: [Development-Bulat](https://github.com/Development-Bulat)
