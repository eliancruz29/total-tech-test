# Requirements Analyzer - AI Powered

A simple but powerful application for analyzing software requirements using generative AI. This system uses OpenRouter's DeepSeek V3.1 model to automatically generate processes, subprocesses, and use cases from natural language requirements.

## 🚀 Features

- **AI-Powered Analysis**: Submit requirements in natural language and get structured analysis
- **Automatic Generation**: Creates processes, subprocesses, and detailed use cases
- **Database Storage**: All analyses are saved to SQL Server for future reference
- **Modern UI**: Clean, responsive interface built with Vue.js and Element Plus
- **Real-time Results**: See analysis results immediately after processing
- **History View**: Browse all previous requirement analyses
- **Detailed View**: Examine each analysis with complete details
- **Docker Ready**: Quick start with Docker Compose

## 🏗️ Architecture

### Technology Stack

- **Frontend**: Vue.js 3 + Element Plus + Vite
- **Backend**: .NET 9 + Entity Framework Core
- **Database**: SQL Server 2022
- **AI Service**: OpenRouter API (DeepSeek V3.1)
- **Containerization**: Docker & Docker Compose

### Application Structure

```
Test_2/
├── frontend/              # Vue.js application
│   ├── src/
│   │   ├── views/        # Application pages
│   │   ├── services/     # API services
│   │   └── router/       # Vue Router configuration
│   ├── Dockerfile
│   └── nginx.conf
├── backend/              # .NET 9 API
│   └── RequirementsAPI/
│       ├── Controllers/  # API endpoints
│       ├── Services/     # Business logic
│       ├── Data/         # Database context
│       └── Models/       # Entities and DTOs
├── database/             # SQL Server initialization
│   └── init.sql
└── docker-compose.yml    # Docker orchestration
```

## 📋 Database Schema

The application uses three main tables:

1. **proceso** - Main process information
   - `id_proceso`, `nombre`, `descripcion`, `requirement_text`, `created_at`

2. **subproceso** - Subprocesses linked to processes
   - `id_subproceso`, `id_proceso`, `nombre`, `descripcion`

3. **caso_uso** - Use cases linked to subprocesses
   - `id_caso_uso`, `id_subproceso`, `nombre`, `descripcion`
   - `actor_principal`, `tipo_caso_uso`, `precondiciones`, `postcondiciones`
   - `criterios_de_aceptacion`

**Use Case Types:**
- `1` = Functional
- `2` = Non-Functional
- `3` = System

## 🐳 Quick Start with Docker

### Prerequisites

- Docker Desktop installed and running
- At least 4GB of available RAM
- Ports 1433, 8080, and 3000 available

### Start the Application

1. Navigate to the Test_2 directory:
```bash
cd Test_2
```

2. Start all services:
```bash
docker-compose up -d
```

3. Wait for all services to be healthy (approximately 30-60 seconds):
```bash
docker-compose ps
```

4. Access the application:
   - **Frontend**: http://localhost:3000
   - **Backend API**: http://localhost:8080
   - **Swagger Documentation**: http://localhost:8080/swagger

### Stop the Application

```bash
docker-compose down
```

### View Logs

```bash
# All services
docker-compose logs -f

# Specific service
docker-compose logs -f backend
docker-compose logs -f frontend
docker-compose logs -f sqlserver
```

## 🔧 Development Setup (Without Docker)

### Prerequisites

- .NET 9 SDK
- Node.js 20+
- SQL Server 2022 (or SQL Server Express)

### Backend Setup

1. Navigate to the backend directory:
```bash
cd Test_2/backend/RequirementsAPI
```

2. Update connection string in `appsettings.json` if needed

3. Restore dependencies and run:
```bash
dotnet restore
dotnet run
```

The API will be available at http://localhost:8080

### Frontend Setup

1. Navigate to the frontend directory:
```bash
cd Test_2/frontend
```

2. Install dependencies:
```bash
npm install
```

3. Start development server:
```bash
npm run dev
```

The application will be available at http://localhost:5173

### Database Setup

Execute the SQL script to create the database and tables:
```bash
sqlcmd -S localhost -U sa -P YourPassword -i database/init.sql
```

## 📖 How to Use

### 1. Analyze Requirements

1. Open the application at http://localhost:3000
2. Enter your software requirements in the text area
3. Click "Analyze Requirements"
4. Wait for the AI to process your requirements
5. View the generated processes, subprocesses, and use cases
6. Results are automatically saved to the database

### 2. View History

1. Click "View History" button
2. Browse all previous requirement analyses
3. See summary information including:
   - Process name and description
   - Number of subprocesses
   - Total use cases
   - Creation date
   - Original requirements text

### 3. View Details

1. From the history page, click on any requirement card
2. View complete details including:
   - Full process information
   - All subprocesses with descriptions
   - Detailed use cases with:
     - Description
     - Main actor
     - Type (Functional/Non-Functional/System)
     - Preconditions
     - Postconditions
     - Acceptance criteria

## 🧪 Example Requirements

Try these example requirements to see the system in action:

### Example 1: E-commerce System
```
Create an e-commerce platform where users can browse products, add items to cart, 
checkout with payment processing, track orders, and leave product reviews. 
Administrators should be able to manage products, view orders, and generate sales reports.
```

### Example 2: Inventory Management
```
Build an inventory management system for warehouse operations including receiving goods, 
stock tracking, order fulfillment, barcode scanning, and real-time inventory reporting. 
The system should alert when stock levels are low and support multiple warehouse locations.
```

### Example 3: Healthcare Appointment System
```
Develop a healthcare appointment scheduling system where patients can book appointments 
online, doctors can view their schedules, receptionists can manage bookings, and the 
system sends automated reminders. Include patient history tracking and prescription management.
```

## 🔑 API Endpoints

### POST /api/requirement/analyze
Analyze requirements and generate structured data
```json
Request:
{
  "requirementText": "Your requirements here..."
}

Response:
{
  "idProceso": 1,
  "nombre": "Process Name",
  "descripcion": "Description",
  "requirementText": "Original requirements",
  "createdAt": "2024-01-01T00:00:00",
  "subprocesos": [...]
}
```

### GET /api/requirement
Get all requirements with their analyses

### GET /api/requirement/{id}
Get a specific requirement by ID

## ⚙️ Configuration

### Backend Configuration (appsettings.json)

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=sqlserver;Database=RequirementsAnalysisDB;..."
  },
  "OpenRouter": {
    "ApiKey": "your-api-key-here"
  }
}
```

### Frontend Configuration

API URL is configured in the Docker environment. For local development, update:
- `frontend/src/services/api.js` - Change `API_BASE_URL`

## 🐛 Troubleshooting

### Database Connection Issues

If the backend can't connect to SQL Server:
```bash
# Check if SQL Server is running
docker-compose ps sqlserver

# View SQL Server logs
docker-compose logs sqlserver

# Restart SQL Server
docker-compose restart sqlserver
```

### Frontend Can't Reach Backend

1. Ensure backend is running: http://localhost:8080/swagger
2. Check CORS configuration in backend `Program.cs`
3. Verify API URL in frontend `api.js`

### Port Already in Use

If ports are already in use, modify `docker-compose.yml`:
```yaml
ports:
  - "3001:80"  # Frontend (instead of 3000:80)
  - "8081:8080"  # Backend (instead of 8080:8080)
```

## 📦 Data Persistence

All data is persisted in Docker volumes:
- `sqlserver-data`: Database files

To completely reset the application:
```bash
docker-compose down -v
docker-compose up -d
```

## 🔒 Security Notes

- This is a demo application without authentication
- The API key is embedded in configuration (not recommended for production)
- For production use:
  - Add user authentication
  - Use secrets management for API keys
  - Implement rate limiting
  - Add input validation and sanitization
  - Enable HTTPS

## 🚀 Future Enhancements

Potential improvements:
- User authentication and authorization
- Export analysis to PDF/Word
- Comparison between different analyses
- Version control for requirements
- Real-time collaboration features
- Multiple AI model support
- Custom templates for different industries

## 📝 License

This project is created for demonstration purposes.

## 🤝 Support

For issues or questions, please refer to the documentation or create an issue in the repository.

---

**Built with ❤️ using Vue.js, .NET 9, and AI**

