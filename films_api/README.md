# Films API

ASP.NET Core Web API for a film catalog with user accounts, roles, film CRUD, chat (SignalR), and image uploads.

Pairs with the Blazor client: [films_blazor](https://github.com/Development-Bulat/blazor-web-apps/tree/main/films_blazor).

## Features

- User registration and login
- Roles: Admin, User
- Films and genres CRUD
- Film chat and private messages (SignalR hub `/chat`)
- Image uploads to `wwwroot/uploads`
- Swagger in Development

## Stack

- .NET 8 / ASP.NET Core Web API
- Entity Framework Core 8
- PostgreSQL (Npgsql)
- SignalR
- Swagger

## Database

- PostgreSQL database: `Api_Blazor`
- Schema via EF Core migrations (`Migrations/`)

```bash
cd TestWebApi321
dotnet ef database update
```

## Configuration

Secrets are **not** in `appsettings.json`. Use **User Secrets** (local) or environment variables.

### User Secrets (recommended)

```bash
cd TestWebApi321
dotnet user-secrets set "ConnectionStrings:TestDbString" "Host=localhost;Port=5432;Database=Api_Blazor;Username=YOUR_USER;Password=YOUR_PASSWORD"
```

Or copy: `appsettings.Development.example.json` → `appsettings.Development.json` (gitignored).

Default URL: `http://localhost:5293`  
Swagger: `http://localhost:5293/swagger`

## Run locally

```bash
cd TestWebApi321
dotnet restore
dotnet run
```

Start the Blazor app on `http://localhost:5157` (CORS is configured for that origin).

## API overview

| Area | Examples |
|------|----------|
| Auth | `POST api/User/authUser`, `POST api/User/regNewUser` |
| Users | `GET api/User/GetAllUsers`, CRUD (admin) |
| Films | `GET api/User/getAllFilms`, `POST api/User/createFilm` |
| Chat | `GET/POST api/User/getMessage`, `sendMessage` |
| SignalR | `/chat` — `JoinFilmGroup`, `JoinPrivateGroup`, `ReceiveMessage` |

## Project structure

```
TestWebApi321/
├── Controllers/     # UserController
├── Service/         # business logic
├── Hubs/            # ChatHub (SignalR)
├── Models/          # Film, Genre, User, ChatMessage
├── DatabaseContext/
├── Migrations/
└── wwwroot/uploads/ # chat images (local dev only)
```

## Author

GitHub: [Development-Bulat](https://github.com/Development-Bulat)
