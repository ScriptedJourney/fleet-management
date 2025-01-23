# Fleet Management System

A mocked real-time vehicle monitoring system built with Angular and .NET Core.  
The system displays vehicle statuses, connectivity, and last ping times with automatic updates every minute.

---

## Tech Stack

- **Frontend**: Angular 19
- **Backend**: .NET 9
- **Database**: SQLite

---

## Quick Start

### Backend
1. Navigate to the `VehicleMonitoring.API` directory:
   ```bash
   cd VehicleMonitoring.API
   ```
2. Run the backend:
   ```bash
   dotnet run
   ```

### Frontend
1. Navigate to the `vehicle-monitoring-ui/app` directory:
   ```bash
   cd vehicle-monitoring-ui/app
   ```
2. Install dependencies:
   ```bash
   npm install
   ```
3. Start the frontend server:
   ```bash
   ng serve
   ```

### Open the UI in your browser
- Navigate to: [http://localhost:4200](http://localhost:4200)

## Swagger
- Navigate to: [https://localhost:7001/swagger](https://localhost:7001/swagger)

---

## Running the Tests

### Backend
1. Navigate to the `VehicleMonitoring.API.Tests` directory:
   ```bash
   cd VehicleMonitoring.API.Tests
   ```
2. Run tests:
   ```bash
   dotnet test
   ```

### Frontend
1. Navigate to the `vehicle-monitoring-ui/app` directory:
   ```bash
   cd vehicle-monitoring-ui/app
   ```
2. Run tests:
   ```bash
   ng test --watch=false
   ```
