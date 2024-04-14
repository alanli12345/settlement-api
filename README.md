# Settlement Booking API

Welcome to the Settlement Booking API repository! This API allows users to book appointments and retrieve bookings based on their names.

## Prerequisites

Before running the API, ensure that you have the following prerequisites installed on your system:

- [.NET SDK](https://dotnet.microsoft.com/download) - The API is built using .NET 8.0, so you need the .NET SDK installed.
- [Git](https://git-scm.com/downloads) - Git is required for cloning the repository.
- [Visual Studio](https://visualstudio.microsoft.com/downloads/) or [Visual Studio Code](https://code.visualstudio.com/download) - You can use either Visual Studio or Visual Studio Code as your IDE for running the API.

## Installation

1. Clone the repository to your local machine using Git:

```bash
git clone https://github.com/alanli12345/SettlementBookingAPI.git

```

## Running the API (Two ways)
```
## Using the Terminal
1. Ensure you are in root \SettlementAPI
2. Run dotnet build
3. Navigate to \SettlementAPI\SettlementAPI
4. Run dotnet run
5. API will run on localhost (in my case it was http://localhost:5153)
6. Use Postman to send HTTP requests to API

## Via Visual Studio
1. Open up solution
2. Run SettlementBookingAPI via IIS Express
```

## Access the API endpoints:

**Book an appointment**:
Endpoint: ```POST /Booking```

**Request Body**: JSON object with name and bookingTime fields

E.g. ```{
"bookingTime": "09:30",
"name":"John Smith"
}```

</br>

**Get bookings by name**:
Endpoint: ```GET /Booking?name={name}```

## Swagger Documentation
The API is documented using Swagger/OpenAPI.
Access the Swagger UI at ```/swagger/index.html``` when running the application in development mode.

## Testing
Unit tests and integration tests are included in the SettlementBookingAPI.Tests project.

Run tests using the ```dotnet test``` command
