## Setup Steps

1. Install .NET 8 SDK from https://dotnet.microsoft.com/download/dotnet/8.0
2. Open terminal in solution root.
3. Run: `dotnet restore`
4. Run: `dotnet build`
5. Set environment variables for JWT and DB connections.
6. (Optional) Run EF Core migrations for each service:
   - `dotnet ef database update --project UserService`
   - `dotnet ef database update --project OrderService`
   - `dotnet ef database update --project InventoryService`
7. Start services:
   - `dotnet run --project ApiGateway`
   - `dotnet run --project UserService`
   - `dotnet run --project OrderService`
   - `dotnet run --project InventoryService`
8. Test APIs via ApiGateway.
