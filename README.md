  # Cloud Based Order Managment System(ERP)

  Architecture:

+------------------------+
|     Client Apps       |
| (Kendo UI Frontend)   |
+----------+------------+
           |
           v
+------------------------+      +-------------------------+
| Azure API Management   +-----> AuthN/AuthZ, Throttling  |
+----------+-------------+      +-------------------------+
           |
           v
+-------------------------+
| .NET Web API Gateway    |  <-- Entry point to system
+----------+--------------+
           |
    +------+-----+-----+-------------------+
    |            |     |                   |
    v            v     v                   v
+----------+ +----------+ +-----------+ +------------+
| OrderSvc | | InvSvc   | | UserSvc   | | PaymentSvc |
| (.NET    | | (.NET    | | (.NET     | | (.NET      |
|  on ASF) | |  on ASF) | |  on ASF)  | |  on ASF)   |
+----------+ +----------+ +-----------+ +------------+
    |            |         |              |
    v            v         v              v
+----------------------------------------------------+
|          Azure Cosmos DB (Orders, Inventory, Users)|
+----------------------------------------------------+
    |
    v
+-----------------------------+
| Azure Functions             |
| - Order Notifications       |
| - Shipment Tracking         |
| - Email Confirmations       |
+-----------------------------+

![Uploading image.png…]()
