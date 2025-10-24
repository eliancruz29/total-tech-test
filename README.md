# Sistema de Adquisiciones CAASIM

Modern Single Page Application (SPA) for managing procurement orders (Pedidos de Compra) with Vue.js 3 frontend, .NET 9 Web API backend, and PostgreSQL database.

## Project Overview

This project refactors the legacy `Consulta_Pedidos.html` into a modern, containerized application using:

- **Frontend**: Vue.js 3 (Composition API) with Vite
- **Backend**: .NET 9 Web API with Entity Framework Core
- **Database**: PostgreSQL 17
- **Deployment**: Docker & Docker Compose

## Quick Start with Docker

### Prerequisites

- [Docker](https://www.docker.com/get-started) installed (version 20.10+)
- [Docker Compose](https://docs.docker.com/compose/install/) (version 2.0+)

### Start the Application

```bash
# Clone the repository
cd total-tech-test

# Copy environment file
cp .env.example .env

# Start all services
docker-compose up -d

# View logs
docker-compose logs -f
```

### Access Points

- **Frontend**: http://localhost:8080
- **Backend API**: http://localhost:5000
- **Swagger Documentation**: http://localhost:5000/swagger
- **Database**: localhost:5432
  - Database: `adquisiciones_db`
  - User: `adquisiciones_user`
  - Password: `Change_This_Password_123!`

### Stop the Application

```bash
docker-compose down

# To remove volumes (database data)
docker-compose down -v
```

## Manual Setup (Development)

### Database Setup

1. **Install PostgreSQL 17**
   ```bash
   # macOS with Homebrew
   brew install postgresql@17

   # Start PostgreSQL
   brew services start postgresql@17
   ```

2. **Create Database**
   ```bash
   createdb adquisiciones_db
   ```

3. **Run Migrations**
   ```bash
   cd database
   psql -U your_user -d adquisiciones_db -f init.sql
   ```

### Backend Setup

1. **Prerequisites**
   - [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)

2. **Configure Connection String**
   ```bash
   cd backend/AdquisicionesAPI
   ```

   Update `appsettings.Development.json`:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Host=localhost;Port=5432;Database=adquisiciones_db;Username=your_user;Password=your_password"
     }
   }
   ```

3. **Run the API**
   ```bash
   dotnet restore
   dotnet build
   dotnet run
   ```

   API will be available at: http://localhost:5000

### Frontend Setup

1. **Prerequisites**
   - [Node.js](https://nodejs.org/) (version 18+)
   - [npm](https://www.npmjs.com/) or [yarn](https://yarnpkg.com/)

2. **Install Dependencies**
   ```bash
   cd frontend
   npm install
   ```

3. **Configure API URL**
   Create `.env.local`:
   ```bash
   VITE_API_BASE_URL=http://localhost:5000
   ```

4. **Run Development Server**
   ```bash
   npm run dev
   ```

   Frontend will be available at: http://localhost:5173

## Project Structure

```
total-tech-test/
├── backend/                        # .NET 9 Web API
│   ├── AdquisicionesAPI/
│   │   ├── Controllers/           # API Controllers
│   │   ├── Models/                # Entity Models
│   │   ├── DTOs/                  # Data Transfer Objects
│   │   ├── Services/              # Business Logic
│   │   ├── Data/                  # DbContext
│   │   ├── Middleware/            # Custom Middleware
│   │   └── Program.cs             # Entry Point
│   └── Dockerfile
├── frontend/                       # Vue.js 3 SPA
│   ├── src/
│   │   ├── components/            # Reusable Components
│   │   ├── views/                 # Page Components
│   │   ├── services/              # API Services
│   │   ├── composables/           # Composition Functions
│   │   ├── router/                # Vue Router
│   │   ├── stores/                # Pinia Stores
│   │   └── App.vue
│   ├── public/
│   └── Dockerfile
├── database/                       # PostgreSQL
│   ├── migrations/
│   │   ├── 001_create_catalog_tables.sql
│   │   ├── 002_create_main_tables.sql
│   │   └── 003_seed_data.sql
│   └── init.sql
├── docker-compose.yml
├── .env.example
├── .gitignore
├── IMPLEMENTATION_PLAN.md          # Detailed Implementation Plan
└── README.md
```

## API Endpoints

### Authentication

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/oauth/token` | Obtain JWT authentication token |

**Request Body (form-urlencoded)**:
```
grant_type=password
username=admin
password=admin
```

### Pedido (Orders)

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/pedido/ListaSelAll` | List all orders with pagination and filters |
| GET | `/api/pedido/Get?id={id}` | Get single order by ID |
| POST | `/api/pedido/Post` | Create new order |
| PUT | `/api/pedido/Put?Id={id}` | Update existing order |

#### Query Parameters for ListaSelAll

- `startRowIndex`: Start index for pagination (default: 1)
- `maximumRows`: Maximum rows to return (default: 10)
- `Where`: SQL-like filter (e.g., `pedido.id_estado_pedido=1`)
- `OrderBy`: Sort order (e.g., `pedido.id_pedido ASC`)

#### Example Request/Response

**POST /api/pedido/Post**

Request Body:
```json
{
  "folio": "PED-2024-001",
  "consecutivo_completo": "PED-2024-001-AD-001",
  "id_tipo_documento_pedido": 1,
  "id_tipo_pedido": 1,
  "id_proveedor": 1,
  "id_procedimiento_adquisicion": 1,
  "fecha_pedido": "2025-10-17",
  "numero_contrato": "CTR-2024-001",
  "destinatario_factura": "CAASIM",
  "direccion_entrega": "Oficinas Centrales",
  "fecha_entrega": "2025-10-25",
  "tiempo_entrega": "7 días hábiles",
  "persona_elaboro": "Juan García",
  "persona_autorizo": "María López",
  "iniciales": "JG",
  "subtotal": 10000.00,
  "total_iva": 1600.00,
  "total_retenciones": 0,
  "monto_total": 11600.00,
  "id_estado_pedido": 1,
  "id_estado_surtido": 1,
  "observaciones": "Pedido urgente",
  "fecha_registro": "2025-10-17",
  "hora_registro": "10:30:00",
  "id_usuario_registro": 1
}
```

## Database Schema

The database contains the following main entities:

### Catalog Tables
- `cat_estado_requisicion` - Requisition states
- `cat_estado_pedido` - Order states
- `cat_tipo_documento_pedido` - Order document types
- `cat_tipo_pedido` - Order types
- `cat_estado_surtido` - Supply states
- `cat_proveedor` - Suppliers/Providers
- `cat_insumo` - Supplies
- `cat_departamento` - Departments
- `cat_unidad_medida` - Measurement units
- And 10+ more catalog tables...

### Main Process Tables
- `spartan_user` - System users
- `requisicion` - Requisitions
- `requisicion_detalle` - Requisition details
- `clave_presupuestal` - Budget keys
- `procedimiento_adquisicion` - Acquisition procedures
- **`pedido`** - Orders (main entity)
- **`pedido_detalle`** - Order details
- `pedido_claves_presupuestales` - Order budget keys
- `contrato_federal` - Federal contracts
- `entrada_almacen` - Warehouse entries
- And more...

## Default Credentials

### Application Users

| Email | Password | Role |
|-------|----------|------|
| admin@caasim.gob.mx | admin123 | Administrator |
| jgarcia@caasim.gob.mx | admin123 | User |

**Note**: Change these passwords in production!

### Database

- **User**: `adquisiciones_user`
- **Password**: `Change_This_Password_123!`
- **Database**: `adquisiciones_db`

## Development Status

### ✅ Completed

- [x] Project structure setup
- [x] Database schema design
- [x] PostgreSQL migration scripts (17 catalog tables + 18 main tables)
- [x] Seed data with sample records
- [x] .NET 9 Web API project initialized
- [x] All required NuGet packages installed
- [x] Docker Compose configuration
- [x] Environment configuration files
- [x] Implementation plan document

### 🚧 In Progress

The following components need to be completed (see [IMPLEMENTATION_PLAN.md](IMPLEMENTATION_PLAN.md) for details):

#### Backend API
1. Create Entity Models (from database schema)
2. Create DbContext with Entity Framework Core
3. Implement DTOs (Data Transfer Objects)
4. Create AutoMapper profiles
5. Implement Repository pattern
6. Create Service layer
7. Implement Authentication controller (/oauth/token)
8. Implement Pedido controller with all endpoints:
   - GET /api/pedido/ListaSelAll
   - GET /api/pedido/Get
   - POST /api/pedido/Post
   - PUT /api/pedido/Put
9. Add global exception handling middleware
10. Configure CORS for frontend
11. Configure Swagger/OpenAPI documentation

#### Frontend SPA
1. Initialize Vue.js 3 project with Vite
2. Install dependencies (Vue Router, Pinia, Axios, UI library)
3. Create authentication system
4. Implement main layout
5. Create OrdersList view (refactored from Consulta_Pedidos.html)
6. Implement filters component
7. Create statistics dashboard
8. Add export functionality (Excel/PDF)
9. Implement order detail modal
10. Add responsive design

#### Docker
1. Create backend Dockerfile
2. Create frontend Dockerfile (multi-stage build)
3. Test complete stack with docker-compose

## Testing

### API Testing with Postman

Import the `examen.postman_collection.json` file into Postman to test all endpoints.

### Manual Testing

1. **Start the application** (Docker or manually)
2. **Obtain token**:
   ```bash
   curl -X GET "http://localhost:5000/oauth/token" \
     -H "Content-Type: application/x-www-form-urlencoded" \
     -d "grant_type=password&username=admin&password=admin"
   ```

3. **Test endpoints** using the token in Authorization header:
   ```bash
   curl -X GET "http://localhost:5000/api/pedido/ListaSelAll?startRowIndex=1&maximumRows=10" \
     -H "Authorization: Bearer YOUR_TOKEN_HERE"
   ```

## Troubleshooting

### Database Connection Issues

```bash
# Check if PostgreSQL is running
docker-compose ps database

# View database logs
docker-compose logs database

# Connect to database directly
docker exec -it adquisiciones-db psql -U adquisiciones_user -d adquisiciones_db
```

### Backend Issues

```bash
# View API logs
docker-compose logs backend

# Rebuild backend
docker-compose up -d --build backend
```

### Frontend Issues

```bash
# View frontend logs
docker-compose logs frontend

# Rebuild frontend
docker-compose up -d --build frontend
```

### Reset Everything

```bash
# Stop and remove all containers, networks, and volumes
docker-compose down -v

# Rebuild and restart
docker-compose up -d --build
```

## Environment Variables

See `.env.example` for all available configuration options:

- **Database**: Connection settings, credentials
- **Backend**: JWT secret, CORS origins, API ports
- **Frontend**: API base URL, app title

## Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

## License

This project is for educational/evaluation purposes.

## Contact & Support

For questions or issues, please refer to the [IMPLEMENTATION_PLAN.md](IMPLEMENTATION_PLAN.md) for detailed implementation guidelines.

---

**Status**: Core infrastructure complete. API and Frontend implementation in progress.

**Next Steps**: See [IMPLEMENTATION_PLAN.md](IMPLEMENTATION_PLAN.md) Phase 6 for detailed implementation steps.
