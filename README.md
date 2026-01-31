# 🎟️ Event Registration Web API

A robust ASP.NET Core Web API for managing events and user registrations, complete with email notifications and business rule enforcement.

---

## 🚀 Features

- Event Management – Create, view, and delete events  
- User Registration – Register attendees with email confirmation  
- Business Rules Enforcement  
  - Event capacity limits  
  - No duplicate registrations per event  
  - Registration allowed for future events only  
  - Prevent deleting events with registrations  
- Email Notifications  
  - Console (default)  
  - SendGrid  
  - Mailtrap  
- RESTful API with Swagger documentation  

---

## 🛠️ Tech Stack

- ASP.NET Core Web API  
- Entity Framework Core  
- SQL Server / LocalDB  
- Swagger (OpenAPI)  

---

## 📋 Prerequisites

- .NET 6.0 or later  
- SQL Server or SQL Server LocalDB  
- (Optional) SendGrid or Mailtrap account  

---

## ⚙️ Setup Instructions

### 1. Clone Repository

```bash
git clone https://github.com/rutvikpatel7/Event-Registration-API-with-Email-Notification.git
cd EventRegistrationAPI
```

### 2. Restore Dependencies

```bash
dotnet restore
```

### 3. Configure Database

Update **appsettings.json**:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER;Database=EventRegistrationDB;TrustServerCertificate=true;"
}
```

### 4. Apply Migrations

```bash
dotnet ef database update
```

### 5. Configure Email Service

#### Console (Default)
No setup required — emails are logged to console.

#### SendGrid

```json
"EmailService": { "Type": "SendGrid" },
"SendGrid": {
  "ApiKey": "your-api-key",
  "FromEmail": "your-email@sendgrid.com",
  "SenderName": "Event Registration System"
}
```

Or use secrets:

```bash
dotnet user-secrets init
dotnet user-secrets set "SendGrid:ApiKey" "your-key"
dotnet user-secrets set "EmailService:Type" "SendGrid"
```

#### Mailtrap

```json
"EmailService": { "Type": "Mailtrap" },
"Mailtrap": {
  "ApiToken": "your-token",
  "FromEmail": "your-email@mailtrap.io",
  "SenderName": "Event Registration System"
}
```

### 6. Run the API

```bash
dotnet run
```

- API Base URL: `https://localhost:5001`  
- Swagger UI: `https://localhost:5001/swagger`

---

## 📡 API Endpoints

### Events

| Method | Endpoint | Description |
|-------|----------|-------------|
| POST | `/api/events` | Create event |
| GET | `/api/events` | Get all events |
| GET | `/api/events/{id}` | Get event by ID |
| DELETE | `/api/events/{id}` | Delete event |

### Registrations

| Method | Endpoint | Description |
|-------|----------|-------------|
| POST | `/api/registrations/events/{eventId}/register` | Register for event |
| GET | `/api/registrations/events/{eventId}` | List registrations |
| DELETE | `/api/registrations/{registrationId}` | Cancel registration |

---

## 🧪 Example Requests

### Create Event

```bash
curl -X POST https://localhost:5001/api/events \
-H "Content-Type: application/json" \
-d '{
  "title": "Tech Conference 2024",
  "description": "Annual technology conference",
  "date": "2024-06-15T09:00:00Z",
  "capacity": 100,
  "location": "Convention Center"
}'
```

### Register for Event

```bash
curl -X POST https://localhost:5001/api/registrations/events/1/register \
-H "Content-Type: application/json" \
-d '{
  "name": "John Doe",
  "email": "john@example.com"
}'
```

---

## 🗄️ Database Schema

### Events

| Column | Type | Constraints |
|--------|------|-------------|
| Id | INT | PK |
| Title | NVARCHAR(200) | NOT NULL |
| Description | NVARCHAR(1000) | |
| Date | DATETIME2 | NOT NULL |
| Capacity | INT | NOT NULL |
| Location | NVARCHAR(300) | NOT NULL |
| CreatedAt | DATETIME2 | NOT NULL |

### Registrations

| Column | Type | Constraints |
|--------|------|-------------|
| Id | INT | PK |
| EventId | INT | FK, NOT NULL |
| Name | NVARCHAR(200) | NOT NULL |
| Email | NVARCHAR(256) | NOT NULL |
| RegisteredAt | DATETIME2 | NOT NULL |

**Unique Index:** `(EventId, Email)`

---

## 📏 Business Rules

1. Event capacity cannot be exceeded  
2. Same email cannot register twice for the same event  
3. Only future events allow registration  
4. Events with registrations cannot be deleted  

---

## ❗ Error Handling

| Status Code | Meaning |
|------------|---------|
| 200 | Success |
| 201 | Created |
| 204 | Deleted |
| 400 | Bad request |
| 404 | Not found |
| 409 | Business rule violation |
| 500 | Server error |

---

## 📬 Postman Collection

Import:  
`EventRegistrationAPI.postman_collection.json`

---

## 🛠 Troubleshooting

**DB Connection Issues**
- Check SQL Server is running  
- Verify connection string  

**Emails Not Sending**
- Validate API keys  
- Confirm correct EmailService type  
- Use Console mode for testing  

**Migration Errors**

```bash
dotnet ef migrations remove
dotnet ef migrations add InitialCreate
dotnet ef database update
```

---

## 🤝 Contributing

See `CONTRIBUTING.md` for guidelines.

---

## 📄 License

MIT License

---

## 💬 Support

Open an issue on GitHub for bugs or questions.
