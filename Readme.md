# Order Processing - Domain-Driven Design (DDD)

OrderProcessing is a .NET 9 Web API for managing customer orders, including order creation, retrieval, and validation. The solution follows clean architecture principles, separating domain, application, infrastructure, and API layers.

## Features

- Create new orders with customer and item details
- Retrieve orders by ID
- Validation of order data (address, email, credit card)
- Integration and unit tests for core functionality

## Technologies

- .NET 9
- C# 13
- ASP.NET Core Web API
- Entity Framework Core
- xUnit, FluentAssertions (testing)
- Docker support

## Getting Started

### Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [Docker](https://www.docker.com/) (optional, for containerization)

### Setup

1. **Clone the repository:**
   git clone https://github.com/salahuddinuk/OrderProcessing-DomainDrivenDesign-CleanArchitecture.git 
1. cd OrderProcessing-DomainDrivenDesign-CleanArchitecture


2. **Configure the database:**
   - Update connection strings in `OrderProcessing.Api/appsettings.json` as needed.
   - By default, uses SQL Server.

3. **Run database migrations:**
   dotnet ef database update --project OrderProcessing.Infrastructure

4. **Start the API:**
   dotnet run --project OrderProcessing.Api
   The API will be available at `https://localhost:5001` or `http://localhost:5000` or `https://localhost:44371`.


## Usage

### Create an Order

`POST /api/orders`
```json
{
  "items": [
    {
      "productId": "132ecce8-6a97-402e-9345-1a5b697d2e41",
      "productName": "External SSD 1TB",
      "productPrice": 129.99,
      "quantity": 1
    },
    {
      "productId": "00ed86c5-d860-4ab0-bdde-718296102a1c",
      "productName": "External SSD 1TB",
      "productPrice": 49.95,
      "quantity": 3
    }
  ],
  "invoiceAddress": "Kaiserstraße 65, 60329 Frankfurt am Main",
  "invoiceEmailAddress": "user@example.com",
  "invoiceCreditCard": "4111111111111111"
}
```

### Get Order by ID

`GET /api/orders/{orderId}`

Returns order details including items and customer info.
```json
{
    "orderId": "c929d975-5e33-44d0-b702-ef7b849a529e",
    "invoiceAddress": "Kaiserstraße 65, 60329 Frankfurt am Main",
    "invoiceEmailAddress": "user@example.com",
    "items": [
        {
            "productId": "132ecce8-6a97-402e-9345-1a5b697d2e41",
            "productName": "External SSD 1TB",
            "price": 129.99,
            "quantity": 1,
            "subtotal": 129.99
        },
        {
            "productId": "00ed86c5-d860-4ab0-bdde-718296102a1c",
            "productName": "External SSD 1TB",
            "price": 49.95,
            "quantity": 3,
            "subtotal": 149.85
        }
    ],
    "createdAt": "2025-08-03T08:41:23.1426403",
    "totalAmount": 279.84
}
```

## Testing

Run all tests:
	dotnet test


Integration tests are located in `OrderProcessing.Tests.Integration`, including API endpoint tests.

## Project Structure

- `OrderProcessing.Api` - ASP.NET Core Web API
- `OrderProcessing.Application` - Business logic, DTOs, validators
- `OrderProcessing.Domain` - Domain entities and value objects
- `OrderProcessing.Infrastructure` - Data access, EF Core, repositories
- `OrderProcessing.Tests` - Unit tests
- `OrderProcessing.Tests.Integration` - Integration tests

