•	ApiGateway: Acts as the entry point, routing requests to appropriate services.
•	UserService: Manages user data and authentication (e.g., JWT token generation).
•	OrderService: Handles order management.
•	InventoryService: Manages inventory data.
•	Functions: Likely contains shared logic or serverless functions.

Architecure:

[Client]
   |
   v
[ApiGateway]
   |      |      |
   v      v      v
[UserService] [OrderService] [InventoryService]
         ^
         |
    [Functions]


