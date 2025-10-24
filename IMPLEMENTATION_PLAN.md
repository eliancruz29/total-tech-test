# Implementation Plan - Sistema de Adquisiciones CAASIM

## Project Overview

Refactor the existing "Consulta_Pedidos.html" into a modern **Single Page Application (SPA)** using:
- **Frontend**: Vue.js 3 (Composition API)
- **Backend**: .NET 9 Web API
- **Database**: PostgreSQL
- **Deployment**: Docker containers for easy execution

---

## Phase 1: Project Structure Setup

### 1.1 Directory Structure
```
total-tech-test/
├── backend/                    # .NET 9 Web API
│   ├── AdquisicionesAPI/
│   │   ├── Controllers/
│   │   ├── Models/
│   │   ├── DTOs/
│   │   ├── Services/
│   │   ├── Data/
│   │   ├── Middleware/
│   │   └── Program.cs
│   ├── AdquisicionesAPI.Tests/
│   └── Dockerfile
├── frontend/                   # Vue.js 3 SPA
│   ├── src/
│   │   ├── components/
│   │   ├── views/
│   │   ├── services/
│   │   ├── composables/
│   │   ├── router/
│   │   ├── stores/
│   │   └── App.vue
│   ├── public/
│   ├── package.json
│   └── Dockerfile
├── database/                   # PostgreSQL scripts
│   ├── migrations/
│   │   ├── 001_create_catalogs.sql
│   │   ├── 002_create_main_tables.sql
│   │   └── 003_seed_data.sql
│   └── init.sql
├── docker-compose.yml
├── .env.example
└── README.md
```

---

## Phase 2: Database Implementation

### 2.1 PostgreSQL Schema Creation
Based on "Diccionario_de_datos.md":

#### Catalog Tables (Simple):
1. `cat_estado_requisicion` - Requisition states
2. `cat_estado_pedido` - Order states
3. `cat_tipo_documento_pedido` - Order document types
4. `cat_tipo_documento_federal` - Federal document types
5. `cat_fuente_financiamiento` - Financing sources
6. `cat_modalidad_compra` - Purchase modalities
7. `cat_estado_procedimiento` - Procedure states
8. `cat_fuente_presupuestal` - Budget sources
9. `cat_nivel_urgencia` - Urgency levels
10. `cat_categoria_requisicion` - Requisition categories
11. `cat_unidad_medida` - Measurement units
12. `cat_categoria_insumo` - Supply categories
13. `cat_estado_entrada_almacen` - Warehouse entry states
14. `cat_tipo_pedido` - Order types
15. `cat_estado_surtido` - Supply states
16. `cat_tipo_documento` - Document types
17. `cat_departamento` - Departments

#### Main Process Tables:
1. `spartan_user` - Users (minimal for authentication)
2. `requisicion` - Requisitions
3. `requisicion_detalle` - Requisition details
4. `clave_presupuestal` - Budget keys
5. `procedimiento_adquisicion` - Acquisition procedures
6. `cat_proveedor` - Suppliers/Providers
7. `cat_insumo` - Supplies
8. `pedido` - Orders (main entity)
9. `pedido_detalle` - Order details
10. `pedido_claves_presupuestales` - Order budget keys
11. `contrato_federal` - Federal contracts
12. `clave_presupuestal_federal` - Federal budget keys
13. `entrada_almacen` - Warehouse entries
14. `cuadro_validacion_proveedor_propuesta` - Provider proposal validations
15. `documento_proveedor` - Provider documents
16. `procedimiento_proveedores` - Procedure providers
17. `dictamen_juridico` - Legal opinions

### 2.2 Migration Strategy
- **Migration 001**: Create all catalog tables
- **Migration 002**: Create main process tables with foreign keys
- **Migration 003**: Seed initial data (states, types, test users)
- **Migration 004**: Create indexes for performance
- **Migration 005**: Create views for complex queries

---

## Phase 3: .NET 9 Web API Implementation

### 3.1 API Endpoints (Based on Postman Collection)

#### Authentication Endpoints:
```
GET  /oauth/token              - Obtain JWT token
```

#### Pedido (Order) Endpoints:
```
GET  /api/pedido/ListaSelAll   - List all orders with filters
GET  /api/pedido/Get           - Get single order by ID
POST /api/pedido/Post          - Create new order
PUT  /api/pedido/Put           - Update existing order
```

#### Additional CRUD Endpoints (to support full functionality):
```
# Catalog endpoints
GET  /api/catalogs/estados-pedido
GET  /api/catalogs/tipos-pedido
GET  /api/catalogs/proveedores
GET  /api/catalogs/claves-presupuestales

# Pedido Detail endpoints
GET  /api/pedido/{id}/detalles
POST /api/pedido/{id}/detalles
PUT  /api/pedido-detalle/{id}
DELETE /api/pedido-detalle/{id}

# Reporting endpoints
GET  /api/pedido/estadisticas    - Order statistics
GET  /api/pedido/export/excel    - Export to Excel
GET  /api/pedido/export/pdf      - Export to PDF
```

### 3.2 Technology Stack:
- **.NET 9** Web API
- **Entity Framework Core 9** (Code-First approach)
- **Npgsql** for PostgreSQL
- **JWT Authentication** (using Microsoft.AspNetCore.Authentication.JwtBearer)
- **AutoMapper** for DTO mapping
- **Serilog** for logging
- **FluentValidation** for validation
- **Swagger/OpenAPI** for API documentation

### 3.3 Key Features:
- JWT token-based authentication
- Repository pattern for data access
- Service layer for business logic
- DTOs for request/response
- Global exception handling middleware
- CORS configuration for Vue.js frontend
- Request validation
- Pagination support
- Advanced filtering and sorting

---

## Phase 4: Vue.js 3 Frontend Implementation

### 4.1 Technology Stack:
- **Vue.js 3** (Composition API)
- **Vite** as build tool
- **Vue Router** for navigation
- **Pinia** for state management
- **Axios** for HTTP requests
- **TanStack Query (Vue Query)** for data fetching
- **PrimeVue** or **Element Plus** for UI components
- **Chart.js** for statistics visualization
- **date-fns** for date formatting
- **XLSX** for Excel export
- **jsPDF** for PDF generation

### 4.2 Main Components:

#### Views:
1. **LoginView.vue** - Authentication page
2. **DashboardView.vue** - Main dashboard
3. **OrdersListView.vue** - Orders listing (refactored Consulta_Pedidos.html)
4. **OrderDetailView.vue** - Order detail modal/page
5. **OrderFormView.vue** - Create/Edit order

#### Components:
1. **OrdersTable.vue** - DataTable with filters
2. **OrderFilters.vue** - Filter section
3. **OrderStats.vue** - Statistics cards
4. **OrderDetailModal.vue** - Detail modal
5. **AdvancedFiltersModal.vue** - Advanced filters
6. **ExportButtons.vue** - Export actions

#### Composables:
1. **useAuth.js** - Authentication logic
2. **useOrders.js** - Orders data fetching
3. **useCatalogs.js** - Catalog data
4. **useFilters.js** - Filter state management
5. **useExport.js** - Export functionality

### 4.3 Features to Implement:
- Authentication with JWT
- Responsive design (mobile-first)
- Advanced filtering system
- Real-time search
- Pagination and sorting
- Statistics visualization
- Excel/PDF export
- Print functionality
- Loading states and error handling
- Toast notifications
- Internationalization (i18n) support

---

## Phase 5: Docker Configuration

### 5.1 Docker Services:
```yaml
services:
  database:
    - PostgreSQL 17
    - Port: 5432
    - Volume for persistence

  backend:
    - .NET 9 Web API
    - Port: 5000 (HTTP), 5001 (HTTPS)
    - Depends on database

  frontend:
    - Nginx serving Vue.js SPA
    - Port: 8080
    - Depends on backend
```

### 5.2 Environment Variables:
```
# Database
POSTGRES_USER=adquisiciones_user
POSTGRES_PASSWORD=secure_password
POSTGRES_DB=adquisiciones_db

# Backend
ConnectionStrings__DefaultConnection=...
JWT__Secret=...
JWT__Issuer=...
JWT__Audience=...

# Frontend
VITE_API_BASE_URL=http://localhost:5000/api
```

---

## Phase 6: Implementation Steps

### Step 1: Database Setup ✓
1. Create migration scripts from Diccionario_de_datos.md
2. Verify all table relationships
3. Create indexes for performance
4. Seed initial catalog data
5. Create test data for development

### Step 2: Backend API Development ✓
1. Create .NET 9 Web API project
2. Configure Entity Framework Core with PostgreSQL
3. Create all entity models
4. Implement authentication with JWT
5. Create repositories and services
6. Implement all Postman endpoints:
   - GET /oauth/token
   - GET /api/pedido/ListaSelAll
   - GET /api/pedido/Get
   - POST /api/pedido/Post
   - PUT /api/pedido/Put
7. Add validation and error handling
8. Configure CORS
9. Add Swagger documentation
10. Write unit tests

### Step 3: Frontend SPA Development ✓
1. Create Vue.js 3 project with Vite
2. Set up Vue Router and Pinia
3. Create authentication system
4. Implement main layout and navigation
5. Create OrdersList view (refactored Consulta_Pedidos.html)
6. Implement filtering system
7. Create statistics dashboard
8. Add export functionality (Excel/PDF)
9. Implement order detail view
10. Add responsive design
11. Implement error handling and loading states

### Step 4: Docker Configuration ✓
1. Create Dockerfile for backend
2. Create Dockerfile for frontend
3. Create docker-compose.yml
4. Configure networking between containers
5. Set up environment variables
6. Test complete stack

### Step 5: Testing & Documentation ✓
1. Test all API endpoints
2. Test frontend functionality
3. Verify database relationships
4. Create comprehensive README.md
5. Document API with Swagger
6. Add code comments
7. Create user guide

---

## Phase 7: Data Mapping

### HTML to Vue.js Component Mapping:

| HTML Section | Vue Component | API Endpoint |
|-------------|---------------|--------------|
| Page Header | `OrdersHeader.vue` | - |
| Filters Section | `OrderFilters.vue` | `/api/catalogs/*` |
| Statistics Cards | `OrderStats.vue` | `/api/pedido/estadisticas` |
| Results Table | `OrdersTable.vue` | `/api/pedido/ListaSelAll` |
| Order Detail Modal | `OrderDetailModal.vue` | `/api/pedido/Get?id={id}` |
| Advanced Filters Modal | `AdvancedFiltersModal.vue` | - |
| Export Buttons | `ExportActions.vue` | `/api/pedido/export/*` |

### Field Mapping (Pedido Entity):

| HTML Field | Database Column | API Field |
|-----------|-----------------|-----------|
| Folio | `folio` | `folio` |
| Partida | `numero_partida` (in pedido_detalle) | `numeroPartida` |
| Fecha Pedido | `fecha_pedido` | `fechaPedido` |
| Tipo | `id_tipo_pedido` → `cat_tipo_pedido.descripcion` | `tipo` |
| Iniciales | `iniciales` | `iniciales` |
| Proveedor | `id_proveedor` → `cat_proveedor.razon_social` | `proveedor` |
| R.F.C | `id_proveedor` → `cat_proveedor.rfc` | `rfc` |
| Clave Presupuestal | (from pedido_detalle) | `clavePresupuestal` |
| Cantidad | (from pedido_detalle) | `cantidad` |
| Cantidad Surtida | (from pedido_detalle) | `cantidadSurtida` |
| Precio | (from pedido_detalle) | `precioUnitario` |
| Observaciones | `observaciones` | `observaciones` |
| Estado | `id_estado_surtido` → `cat_estado_surtido.descripcion` | `estado` |

---

## Phase 8: Validation & Verification Checklist

### Database:
- [ ] All tables from Diccionario_de_datos.md created
- [ ] All foreign key relationships validated
- [ ] Column names and types match specification
- [ ] Indexes created for performance
- [ ] Seed data loaded successfully

### API:
- [ ] All Postman endpoints implemented identically
- [ ] GET /oauth/token returns JWT token
- [ ] GET /api/pedido/ListaSelAll supports pagination, filtering, sorting
- [ ] GET /api/pedido/Get returns complete order with details
- [ ] POST /api/pedido/Post creates order successfully
- [ ] PUT /api/pedido/Put updates order successfully
- [ ] Authentication middleware working
- [ ] CORS configured for frontend
- [ ] Swagger documentation accessible

### Frontend:
- [ ] All HTML functionality replicated
- [ ] Filters working (year, folio, supplier, budget, type, dates, status)
- [ ] Advanced filters modal functional
- [ ] Statistics cards display correctly
- [ ] DataTable with pagination and sorting
- [ ] Order detail modal shows complete information
- [ ] Export to Excel working
- [ ] Export to PDF working
- [ ] Print functionality working
- [ ] Responsive design on mobile devices
- [ ] Loading states during API calls
- [ ] Error handling and notifications

### Docker:
- [ ] Database container starts successfully
- [ ] Backend container connects to database
- [ ] Frontend container serves application
- [ ] All services communicate correctly
- [ ] Environment variables configured
- [ ] Volumes for data persistence
- [ ] Can start entire stack with single command

---

## Phase 9: Deployment & Documentation

### 9.1 README.md Structure:
1. Project Overview
2. Prerequisites
3. Quick Start with Docker
4. Manual Setup (without Docker)
5. API Documentation
6. Environment Variables
7. Database Schema
8. Testing
9. Troubleshooting
10. Contributing

### 9.2 Quick Start Command:
```bash
docker-compose up -d
```

### 9.3 Access Points:
- Frontend: http://localhost:8080
- Backend API: http://localhost:5000
- Swagger UI: http://localhost:5000/swagger
- Database: localhost:5432

---

## Timeline Estimate

| Phase | Estimated Time |
|-------|---------------|
| Project Setup | 1 hour |
| Database Implementation | 3 hours |
| Backend API Development | 8 hours |
| Frontend SPA Development | 10 hours |
| Docker Configuration | 2 hours |
| Testing & Debugging | 4 hours |
| Documentation | 2 hours |
| **Total** | **30 hours** |

---

## Success Criteria

1. All endpoints from Postman collection work identically
2. All HTML functionality present in Vue.js SPA
3. Database schema matches Diccionario_de_datos.md exactly
4. Complete stack runs with single Docker command
5. Comprehensive documentation for setup and usage
6. Responsive design works on desktop and mobile
7. No console errors or warnings
8. Clean, maintainable code with comments

---

## Next Steps

1. **Immediate**: Create project structure and initialize repositories
2. **Database**: Create and validate all migration scripts
3. **Backend**: Implement API endpoints one by one
4. **Frontend**: Build components incrementally
5. **Integration**: Connect all pieces together
6. **Testing**: Verify all functionality
7. **Documentation**: Write comprehensive guides

---

**Status**: Ready to begin implementation
**Last Updated**: 2025-10-24
