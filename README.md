# Expense Tracker Web API

A robust and scalable Expense Tracker API built with .NET Core 8, designed to help users track and manage their expenses efficiently. This project demonstrates the use of modern software architecture practices including microservices, N-tier layers, and asynchronous programming with async/await. It integrates with Azure AD B2C for authentication and leverages Entity Framework Core for database operations.

## Features
- **User Management**: Register users, group them into families, and manage user profiles.
- **Expense Tracking**: Record, update, and delete expenses with detailed descriptions and categories.
- **Family Expense Consolidation**: View consolidated expenses for all family members.
- **Secure Authentication**: Integrates Azure AD B2C for secure user authentication.
- **RESTful API Design**: Clean and well-structured API endpoints for seamless integration.
- **Async/Await for Efficiency**: Implements asynchronous programming for improved performance and scalability.

## Technologies Used
- **.NET Core 8**
- **Entity Framework Core**
- **Azure AD B2C**
- **SQL Server**
- **RESTful API**

## Getting Started

### Prerequisites
- .NET Core 8 SDK
- SQL Server
- Azure AD B2C tenant for authentication

### Installation

1. **Clone the repository**:
    ```sh
    git clone https://github.com/learnsmartcoding/expense-tracker-web-api.git
    cd expense-tracker-web-api
    ```

2. **Configure the database connection**:
    - Update the connection string in `appsettings.json`:
    ```json
    "ConnectionStrings": {
        "DefaultConnection": "Server=your_server;Database=ExpenseTracker;User Id=your_user;Password=your_password;"
    }
    ```

3. **Configure Azure AD B2C**:
    - Yet to build
    - Update the Azure AD B2C settings in `appsettings.json`:
    ```json
    "AzureAdB2C": {
        "Instance": "https://your-tenant.b2clogin.com/",
        "ClientId": "your-client-id",
        "Domain": "your-tenant.onmicrosoft.com",
        "SignUpSignInPolicyId": "your-policy-id"
    }
    ```

4. **Build and run the project**:
    ```sh
    dotnet build
    dotnet run
    ```

## Usage

### API Endpoints

#### User Management
- **Get User by ID**: `GET /api/users/{userId}`
- **Get Users by Family ID**: `GET /api/users/family/{familyId}`
- **Add User**: `POST /api/users`
- **Update User**: `PUT /api/users/{userId}`
- **Delete User**: `DELETE /api/users/{userId}`

#### Family Management
- **Get Family by ID**: `GET /api/families/{familyId}`
- **Get All Families**: `GET /api/families`
- **Add Family**: `POST /api/families`
- **Update Family**: `PUT /api/families/{familyId}`
- **Delete Family**: `DELETE /api/families/{familyId}`

#### Expense Management
- **Get Expense by ID**: `GET /api/expenses/{expenseId}`
- **Get Expenses by User ID**: `GET /api/expenses/user/{userId}`
- **Get Expenses by Family ID**: `GET /api/expenses/family/{familyId}`
- **Add Expense**: `POST /api/expenses`
- **Update Expense**: `PUT /api/expenses/{expenseId}`
- **Delete Expense**: `DELETE /api/expenses/{expenseId}`

#### Expense Types, Categories, and Credit Cards
- **Get All Expense Types**: `GET /api/expense-types`
- **Get All Expense Categories**: `GET /api/expense-categories`
- **Get All Credit Cards**: `GET /api/credit-cards`

## Contributing
Contributions are welcome! Please open an issue or submit a pull request.

## License
This project is licensed under the MIT License.

## Contact
For questions or feedback, please contact [learnsmartcoding@gmail.com](mailto:learnsmartcoding@gmail.com).

