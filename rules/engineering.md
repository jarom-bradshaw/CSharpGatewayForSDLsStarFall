### Engineering rules (StarFall Event Gateway)

- **No hardcoded demo data**: Do not ship template/demo endpoints (e.g., WeatherForecast) or responses that fabricate domain data. All domain data returned by this gateway must come from **Postgres (StarFall DB)** or from clearly documented external integrations.
- **Health endpoint must be real**: `/health` must reflect real dependencies (at minimum, database connectivity). It must return **503** when required dependencies are unavailable.
- **Parameterize all SQL**: Use Dapper with parameters; never build SQL with string concatenation using user input.
- **Keep controllers thin**: Controllers handle HTTP concerns (routing, validation, status codes) and delegate to application services.
- **Single source of truth for contracts**: DTOs are explicit and versionable; do not return anonymous objects for domain responses.
- **Focus on scope**: This repo is a backend-only gateway. Do not add UI (MVC/Blazor) or attempt to port Python/ML components.

