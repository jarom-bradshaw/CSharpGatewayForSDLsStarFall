## Starfall Event Gateway (.NET)

ASP.NET Core Web API that exposes a clean REST surface over a StarFall-style Postgres database.

### Prereqs

- .NET SDK (latest)
- A Postgres database running locally (StarFall schema recommended)

### Configure

Set the connection string via environment variable (recommended).

PowerShell:

```powershell
$env:ConnectionStrings__StarfallDatabase="Host=localhost;Port=5432;Database=starfall_database;Username=starfall_admin;Password=<PASSWORD>"
```

### Run

From `CSharpGatewayForSDLsStarFall/`:

```powershell
dotnet run --project .\src\Starfall.EventGateway.Api\Starfall.EventGateway.Api.csproj --urls http://localhost:5202
```

Swagger (development only):

- `http://localhost:5202/swagger`

### Endpoints

- `GET /health`
- `GET /api/events?pageNumber=0&pageSize=25`
- `GET /api/events/{eventId}`
- `GET /api/events/{eventId}/history`

### Curl examples

```bash
curl http://localhost:5202/health
curl "http://localhost:5202/api/events?pageNumber=0&pageSize=25"
```

