# Cloud Based Order Managment System(ERP)

## Project Structure
```text
CloudOMS/
├── src/
│   ├── OrderService/          # .NET microservice for orders
│   ├── InventoryService/      # .NET microservice for inventory
│   ├── UserService/           # .NET microservice for users
│   ├── PaymentService/        # .NET microservice for payments
│   ├── ApiGateway/            # .NET Web API Gateway
│   ├── Frontend/              # Kendo UI frontend
│   └── Functions/             # Azure Functions for notifications
├── docs/                      # Documentation (architecture, APIs)
├── scripts/                   # Azure CLI scripts for resource setup
└── tests/                     # Unit and integration tests
