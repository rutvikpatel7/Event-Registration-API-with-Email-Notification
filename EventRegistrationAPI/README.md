# Event Registration Web API

A comprehensive ASP.NET Core Web API for managing event registrations with email notifications.

## Features

- **Event Management**: Create, retrieve, update, and delete events
- **User Registration**: Register users for events with automatic email confirmations
- **Business Rules Enforcement**:
  - Event capacity validation
  - Duplicate email prevention per event
  - Past event registration prevention
  - Cascade delete protection
- **Email Notifications**: Support for multiple email services (Console, SendGrid, Mailtrap)
- **RESTful API**: Complete API documentation via Swagger

## Prerequisites

- .NET 6.0 or higher
- SQL Server LocalDB or SQL Server
- (Optional) SendGrid or Mailtrap account for email services

## Setup Instructions

### 1. Clone the Repository

\`\`\`bash
git clone https://github.com/rutvikpatel7/Event-Registration-API-with-Email-Notification.git
cd EventRegistrationAPI
\`\`\`

### 2. Install Dependencies

\`\`\`bash
dotnet restore
\`\`\`

### 3. Configure Database Connection

Update `appsettings.json` with your SQL Server connection string:

\`\`\`json
"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER;Database=EventRegistrationDB;TrustServerCertificate=true;"
}
\`\`\`

### 4. Apply Database Migrations

\`\`\`bash
dotnet ef database update
\`\`\`

### 5. Configure Email Service

#### Option A: Console Email Service (Default - No Setup Required)

No configuration needed. Emails will be logged to the console.

#### Option B: SendGrid

1. Get your API key from [SendGrid](https://sendgrid.com/)
2. Update `appsettings.json`:

\`\`\`json
"EmailService": {
  "Type": "SendGrid"
},
"SendGrid": {
  "ApiKey": "your-sendgrid-api-key",
  "FromEmail": "your-email@sendgrid.com",
  "SenderName": "Event Registration System"
}
\`\`\`

Or use User Secrets:

\`\`\`bash
dotnet user-secrets init
dotnet user-secrets set "SendGrid:ApiKey" "SG.U20MiJoISwKpdQ-B8AdDsg..."
dotnet user-secrets set "EmailService:Type" "SendGrid"
dotnet user-secrets set "SendGrid:FromEmail" "xit.dev43@gmail.com"
\`\`\`

#### Option C: Mailtrap

1. Get your API token from [Mailtrap](https://mailtrap.io/)
2. Update `appsettings.json`:

\`\`\`json
"EmailService": {
  "Type": "Mailtrap"
},
"Mailtrap": {
  "ApiToken": "your-mailtrap-api-token",
  "FromEmail": "your-email@mailtrap.io",
  "SenderName": "Event Registration System"
}
\`\`\`

### 6. Run the Application

\`\`\`bash
dotnet run
\`\`\`

The API will be available at `https://localhost:5001` and Swagger UI at `https://localhost:5001/swagger`

## API Endpoints

### Events

- **Create Event**: `POST /api/events`
- **Get All Events**: `GET /api/events`
- **Get Event by ID**: `GET /api/events/{id}`
- **Delete Event**: `DELETE /api/events/{id}`

### Registrations

- **Register for Event**: `POST /api/registrations/events/{eventId}/register`
- **Get Event Registrations**: `GET /api/registrations/events/{eventId}`
- **Cancel Registration**: `DELETE /api/registrations/{registrationId}`

## Request/Response Examples

### Create Event

\`\`\`bash
curl -X POST "https://localhost:5001/api/events" \\
  -H "Content-Type: application/json" \\
  -d '{
    "title": "Tech Conference 2024",
    "description": "Annual technology conference",
    "date": "2024-06-15T09:00:00Z",
    "capacity": 100,
    "location": "Convention Center"
  }'
\`\`\`

### Register for Event

\`\`\`bash
curl -X POST "https://localhost:5001/api/registrations/events/1/register" \\
  -H "Content-Type: application/json" \\
  -d '{
    "name": "John Doe",
    "email": "john@example.com"
  }'
\`\`\`

## Database Schema

### Events Table

| Column | Type | Constraints |
|--------|------|-------------|
| Id | INT | PRIMARY KEY |
| Title | NVARCHAR(200) | NOT NULL |
| Description | NVARCHAR(1000) | |
| Date | DATETIME2 | NOT NULL |
| Capacity | INT | NOT NULL |
| Location | NVARCHAR(300) | NOT NULL |
| CreatedAt | DATETIME2 | NOT NULL |

### Registrations Table

| Column | Type | Constraints |
|--------|------|-------------|
| Id | INT | PRIMARY KEY |
| EventId | INT | FOREIGN KEY, NOT NULL |
| Name | NVARCHAR(200) | NOT NULL |
| Email | NVARCHAR(256) | NOT NULL |
| RegisteredAt | DATETIME2 | NOT NULL |

**Unique Index**: (EventId, Email) - Prevents duplicate registrations

## Business Rules

1. **Event Capacity**: Cannot exceed the defined capacity
2. **Duplicate Prevention**: Same email cannot register twice for the same event
3. **Future Events Only**: Registration is only allowed for future events
4. **Delete Protection**: Events with existing registrations cannot be deleted

## Error Handling

The API returns appropriate HTTP status codes:

- `200 OK`: Successful GET request
- `201 Created`: Successful resource creation
- `204 No Content`: Successful DELETE request
- `400 Bad Request`: Invalid input
- `404 Not Found`: Resource not found
- `409 Conflict`: Business rule violation
- `500 Internal Server Error`: Server error

## Testing with Postman

A Postman collection is included in the repository. Import `EventRegistrationAPI.postman_collection.json` to test all endpoints.

## Troubleshooting

### Connection String Error

Ensure SQL Server is running and the connection string is correct in `appsettings.json`.

### Email Not Sending

- Verify your API key or token is correct
- Check that `appsettings.json` has the correct email service type
- For development, switch to Console email service to verify registration flow

### Migration Issues

\`\`\`bash
dotnet ef migrations remove
dotnet ef migrations add InitialCreate
dotnet ef database update
\`\`\`

## Contributing

Please read [CONTRIBUTING.md](CONTRIBUTING.md) for details on our code standards and contribution guidelines.

## License

This project is open source and available under the MIT License.

## Support

For issues or questions, please create an issue on GitHub.
\`\`\`

---

## 12. Postman Collection
