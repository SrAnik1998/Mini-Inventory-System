# Mini-Inventory-System

A simple inventory management system built with \*\*ASP.NET Core Web API (.NET 7), using Dapper for data access and SQL Server as the database.  

It includes core modules such as:

=> Product Management

=> Customer Management

=> Sales Transactions(with VAT, Discount, and Loyalty Point support)

=> Sales Report by date Range (Total Sales, Total Revenue, Number of Transactions)

=> Authentication using JWT

=> Swagger UI with integrated token authorization



---


## Technologies Used



- ASP.NET Core Web API (.NET 7)

- Dapper ORM

- SQL Server

- JWT Authentication

- Swagger



---

##Running the Project



### Clone the repo or download the code:

 git clone https://github.com/SrAnik1998/Mini-Inventory-System.git

 cd MiniInventorySystem



### Prerequisites



- .NET 7 SDK

- SQL Server

- Visual Studio



### Database Setup



> A file named `MiniInventorySystem.sql` is included with this project. It contains:

> - All required tables

> - Seed data (sample products, customers, and the `admin` user)



--To set up the database--:



1. Open SQL Server Management Studio (SSMS)

2. Create a new database (e.g., `MiniInventoryDb`)

3. Open the file `MiniInventorySystem.sql`

4. Execute it against the created database



--- Then update your `appsettings.json` connection string:



```json

"ConnectionStrings": {

&nbsp; "DefaultConnection": "Server=YOUR\_SERVER;Database=MiniInventoryDb;Trusted\_Connection=True;"

}





## Modules Overview



### Authentication



- User registration and login

- JWT token-based authentication

- Preloaded user:

- **Username:** `admin`
- **Password:** `admin123`

- After login, copy the **token** and use the **Authorize button** (top-right of Swagger) to access protected APIs.



---



### Product Module



- Add, Update, Delete (Soft delete), Product list

- Stock quantity \& price tracking



### Customer Module



- Add, Update, Delete (Soft delete), Customer list

- Loyalty point support for redeeming and earning during sales



### Sale Module

- Create new sales

- Handles discount, VAT, loyalty points

- Validates product stock before selling

- Calculates total, net, due amount

- Rate-limited to allow only 3 concurrent transactions (with simulated delay)


### Sales Report
 - by date Range (Total Sales, Total Revenue, Number of Transactions)



