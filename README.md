# InternIntelligence Portfolio

InternIntelligence Portfolio is a **Minimal API** project built with **.NET 9+**. This project serves as an assessment assignment and is structured using **Clean Architecture** principles.

## Project Structure
The solution consists of several key projects:

- **InternIntelligence_Portfolio.API** → Main entry point for the application.
- **InternIntelligence_Portfolio.Application** → Contains business logic, services, abstractions.
- **InternIntelligence_Portfolio.Domain** → Defines core entities and domain logic.
- **InternIntelligence_Portfolio.Infrastructure** → Handles persistence, services, and external dependencies.
- **InternIntelligence_Portfolio.Tests.Common** → Contains shared testing utilities.
- **InternIntelligence_Portfolio.Tests.Integration** → Includes integration tests.

## Configuration
The client should define settings in either `appsettings.json` or `secrets.json` for the `.API` or `.Integration.Tests` projects.

### Example `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "Default": "Server=.;Database=PortfolioDB;Integrated Security=true;TrustServerCertificate=true;"
  },
  "Token": {
    "Access": {
      "Audience": "https://localhost:3000/",
      "Issuer": "https://localhost:7223/",
      "SecurityKey": "2992715c-8193-4635-bf7c-9dfbe9c5735a",
      "AccessTokenLifeTimeInMinutes": 10
    },
    "Refresh": {
      "RefreshTokenLifeTimeInMinutes": 60
    }
  },
  "EmailConfiguration": {
    "From": "your-email@example.com",
    "SmtpServer": "smtp.example.com",
    "Port": 587,
    "Username": "your-email@example.com",
    "Password": "your-secure-password"
  }
}
```
⚠️ **Important:** Clients must configure valid SMTP settings for email functionality to work properly.

## 🚀 How to Run
1. **Clone the repository**
   ```sh
   git clone <repository-url>
   cd InternIntelligence_Portfolio
   ```
2. **Configure database** (Ensure SQL Server is running and update `ConnectionStrings:Default` as needed.)
3. **Run migrations**
   ```sh
   dotnet ef database update --project InternIntelligence_Portfolio.Infrastructure
   ```
4. **Run the application**
   ```sh
   dotnet run --project InternIntelligence_Portfolio.API
   ```
5. **API is now available at:** `https://localhost:7105`

## 🧪 Running Tests
Run integration tests using:
```sh
dotnet test InternIntelligence_Portfolio.Tests.Integration
```

## 🛠 Technologies Used
- **.NET 9+** (Minimal APIs)
- **Entity Framework Core** (Persistence)
- **JWT Authentication** (Security)
- **SMTP** (Email service)
- **xUnit** (Testing framework)

## 📜 License
This project is for assessment purposes only.

---

🔹 *For any issues, feel free to open a discussion or contact the repository owner.*

