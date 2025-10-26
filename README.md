# SampleProject

A Blazor Server application with authentication functionality.

## Projects

### SampleProject
The main Blazor Server web application.

### SampleProject.Api
A REST API project that provides authentication services.

### SampleProject.Tests
Unit tests for the main application.

### SampleProject.Api.Tests
Unit tests for the API.

## Authentication

The project supports two authentication modes:

### 1. Configuration-based Authentication (Default)
Credentials are validated against values in `appsettings.json`:
- Username: `admin`
- Password: `password123`

### 2. API-based Authentication
Credentials are validated by calling the REST API endpoint.
- API Username: `admin`
- API Password: `123456`

### Configuration

In `SampleProject/appsettings.json`:

```json
"Authentication": {
  "Username": "admin",
  "Password": "password123",
  "UseApi": false,
  "ApiUrl": "http://localhost:5002"
}
```

Set `UseApi` to `true` to use API-based authentication.

## Running the Application

### Run the Web Application Only
```bash
cd SampleProject
dotnet run
```
The application will be available at `http://localhost:5000` (or the port specified in launchSettings.json).

### Run with API Authentication

1. Start the API:
```bash
cd SampleProject.Api
dotnet run
```
The API will listen on `http://localhost:5002`.

2. Update `SampleProject/appsettings.json` to enable API authentication:
```json
"Authentication": {
  "UseApi": true,
  "ApiUrl": "http://localhost:5002"
}
```

3. Start the web application:
```bash
cd SampleProject
dotnet run
```

## Testing

Run all tests:
```bash
dotnet test
```

## API Endpoints

### POST /api/authenticate
Validates username and password.

**Request:**
```json
{
  "username": "admin",
  "password": "123456"
}
```

**Response:**
```json
{
  "isValid": true
}
```