# Architecture Overview

## System Architecture

```
┌─────────────────────────────────────────────────────────────────┐
│                          User Browser                            │
│                      http://localhost:3000                       │
└──────────────────────────────┬──────────────────────────────────┘
                               │
                               ▼
┌─────────────────────────────────────────────────────────────────┐
│                    Vue.js Frontend (Nginx)                       │
│  ┌────────────────────────────────────────────────────────────┐ │
│  │  Components:                                                │ │
│  │  • AnalyzerView - Input form for requirements              │ │
│  │  • HistoryView - List all analyses                         │ │
│  │  • DetailView - Detailed view of analysis                  │ │
│  │                                                             │ │
│  │  Services:                                                  │ │
│  │  • api.js - Axios HTTP client                              │ │
│  └────────────────────────────────────────────────────────────┘ │
└──────────────────────────────┬──────────────────────────────────┘
                               │ HTTP/REST
                               ▼
┌─────────────────────────────────────────────────────────────────┐
│               .NET 9 Backend API (Port 8080)                     │
│  ┌────────────────────────────────────────────────────────────┐ │
│  │  Controllers:                                               │ │
│  │  • RequirementController                                    │ │
│  │    - POST /api/requirement/analyze                          │ │
│  │    - GET  /api/requirement                                  │ │
│  │    - GET  /api/requirement/{id}                             │ │
│  └────────────────────────────────────────────────────────────┘ │
│  ┌────────────────────────────────────────────────────────────┐ │
│  │  Services:                                                  │ │
│  │  • RequirementService - Business logic                      │ │
│  │  • OpenRouterService - AI API integration                   │ │
│  └────────────────────────────────────────────────────────────┘ │
│  ┌────────────────────────────────────────────────────────────┐ │
│  │  Data Layer:                                                │ │
│  │  • RequirementsDbContext - EF Core DbContext                │ │
│  │  • Entity Models (Proceso, Subproceso, CasoUso)            │ │
│  └────────────────────────────────────────────────────────────┘ │
└──────────────┬──────────────────────────────┬───────────────────┘
               │                              │
               ▼                              ▼
    ┌──────────────────────┐      ┌──────────────────────┐
    │  SQL Server 2022     │      │  OpenRouter API      │
    │  (Port 1433)         │      │  (External Service)  │
    │                      │      │                      │
    │  • proceso           │      │  DeepSeek V3.1       │
    │  • subproceso        │      │  Model (Free)        │
    │  • caso_uso          │      └──────────────────────┘
    └──────────────────────┘
```

## Request Flow

### 1. Analyze Requirements Flow

```
User Input
    ↓
AnalyzerView.vue (Frontend)
    ↓ [POST] requirementText
api.js → POST /api/requirement/analyze
    ↓
RequirementController.cs
    ↓
RequirementService.AnalyzeAndSaveRequirementAsync()
    ↓
    ├─→ OpenRouterService.AnalyzeRequirementsAsync()
    │       ↓
    │   [External API Call to OpenRouter]
    │       ↓
    │   AI Response (JSON with processes, subprocesses, use cases)
    │       ↓
    │   Return AI Response
    │
    └─→ Parse AI Response (JSON)
        ↓
    Create Proceso Entity
        ↓
    Save to Database (EF Core)
        ↓
    For each Subproceso in AI Response:
        ↓
        Create Subproceso Entity
        ↓
        Save to Database
        ↓
        For each CasoUso in Subproceso:
            ↓
            Create CasoUso Entity
            ↓
            Save to Database
    ↓
Return Complete AnalysisResponse DTO
    ↓
Display Results in UI
```

### 2. View History Flow

```
User Action
    ↓
HistoryView.vue
    ↓ [GET]
api.js → GET /api/requirement
    ↓
RequirementController.cs
    ↓
RequirementService.GetAllRequirementsAsync()
    ↓
EF Core Query with Include() for related entities
    ↓
SQL Server (JOIN queries)
    ↓
Return List<AnalysisResponse>
    ↓
Display in UI as cards
```

### 3. View Detail Flow

```
User Clicks on Card
    ↓
Router → /detail/:id
    ↓
DetailView.vue
    ↓ [GET]
api.js → GET /api/requirement/{id}
    ↓
RequirementController.cs
    ↓
RequirementService.GetRequirementByIdAsync(id)
    ↓
EF Core Query with Include()
    ↓
SQL Server
    ↓
Return AnalysisResponse or null
    ↓
Display detailed view with expandable sections
```

## Data Flow

### Database Schema Relationships

```
proceso (Parent)
    ├─ id_proceso (PK)
    ├─ nombre
    ├─ descripcion
    ├─ requirement_text
    └─ created_at
    │
    └─── subproceso (Child) [1:N]
            ├─ id_subproceso (PK)
            ├─ id_proceso (FK)
            ├─ nombre
            └─ descripcion
            │
            └─── caso_uso (Grandchild) [1:N]
                    ├─ id_caso_uso (PK)
                    ├─ id_subproceso (FK)
                    ├─ nombre
                    ├─ descripcion
                    ├─ actor_principal
                    ├─ tipo_caso_uso (1/2/3)
                    ├─ precondiciones
                    ├─ postcondiciones
                    └─ criterios_de_aceptacion
```

## Technology Stack Details

### Frontend Stack
- **Vue.js 3.5**: Progressive JavaScript framework
- **Vue Router 4**: Client-side routing
- **Element Plus 2.9**: UI component library
- **Axios 1.7**: HTTP client
- **Vite 6.0**: Build tool and dev server

### Backend Stack
- **.NET 9**: Latest .NET framework
- **ASP.NET Core**: Web API framework
- **Entity Framework Core 9**: ORM for database access
- **Swashbuckle**: Swagger/OpenAPI documentation

### Database
- **SQL Server 2022**: Relational database
- **Identity columns**: Auto-increment primary keys
- **Cascade delete**: Maintain referential integrity

### External Services
- **OpenRouter API**: AI service aggregator
- **DeepSeek V3.1**: Language model for analysis

## Design Patterns Used

### Backend Patterns

1. **Repository Pattern**: Abstracted through EF Core DbContext
2. **Service Layer Pattern**: Business logic separated from controllers
3. **Dependency Injection**: All services injected via constructor
4. **DTO Pattern**: Separate models for API responses
5. **Interface Segregation**: Separate interfaces for each service

### Frontend Patterns

1. **Component-Based Architecture**: Reusable Vue components
2. **Service Layer**: Centralized API calls in `api.js`
3. **Route-Based Code Splitting**: Separate views for different features
4. **Composition API**: Modern Vue 3 composition approach

## Security Considerations

### Current Implementation
- No authentication/authorization (as per requirements)
- CORS enabled for all origins (development only)
- API key stored in configuration (not ideal)
- TrustServerCertificate enabled for SQL Server

### Production Recommendations
1. Add authentication (JWT tokens)
2. Restrict CORS to specific origins
3. Use environment variables/secrets for API keys
4. Enable HTTPS/TLS
5. Add rate limiting
6. Implement input validation
7. Add SQL injection protection (EF Core helps)
8. Sanitize AI-generated content

## Scalability Considerations

### Current Architecture
- Synchronous processing
- Single instance design
- Direct database access

### Potential Improvements
1. **Caching**: Add Redis for frequently accessed data
2. **Queue System**: Use message queue for AI processing
3. **Load Balancing**: Multiple backend instances
4. **Database**: 
   - Connection pooling (already in EF Core)
   - Read replicas for history/detail views
   - Indexing on frequently queried fields
5. **CDN**: Serve frontend static assets
6. **Monitoring**: Add application insights/logging

## Docker Architecture

### Container Strategy
```
┌─────────────────┐
│   Frontend      │  (Nginx serving static files)
│   Port: 3000    │
└─────────────────┘

┌─────────────────┐
│   Backend       │  (ASP.NET Core API)
│   Port: 8080    │
└─────────────────┘

┌─────────────────┐
│   DB Init       │  (One-time initialization)
│   (Exits after) │
└─────────────────┘

┌─────────────────┐
│   SQL Server    │  (Persistent database)
│   Port: 1433    │
│   Volume: data  │
└─────────────────┘

         │
    requirements-network
    (Bridge network)
```

### Startup Sequence
1. SQL Server starts and becomes healthy
2. DB Init runs initialization script
3. Backend starts after DB is initialized
4. Frontend starts after backend is ready

## Error Handling Strategy

### Backend
- Try-catch blocks in services
- Structured logging
- HTTP status codes (200, 400, 404, 500)
- Detailed error messages in development

### Frontend
- Async/await with try-catch
- User-friendly error messages (Element Plus)
- Loading states during async operations
- Fallback UI for errors

## API Contract

### Request/Response Models

**RequirementRequest**
```json
{
  "requirementText": "string"
}
```

**AnalysisResponse**
```json
{
  "idProceso": 1,
  "nombre": "string",
  "descripcion": "string",
  "requirementText": "string",
  "createdAt": "2024-01-01T00:00:00Z",
  "subprocesos": [
    {
      "idSubproceso": 1,
      "nombre": "string",
      "descripcion": "string",
      "casosUso": [
        {
          "idCasoUso": 1,
          "nombre": "string",
          "descripcion": "string",
          "actorPrincipal": "string",
          "tipoCasoUso": 1,
          "tipoCasoUsoText": "Functional",
          "precondiciones": "string",
          "postcondiciones": "string",
          "criteriosDeAceptacion": "string"
        }
      ]
    }
  ]
}
```

## Future Architecture Enhancements

1. **Microservices**: Separate AI service from main API
2. **Event Sourcing**: Track all changes to requirements
3. **GraphQL**: More flexible data querying
4. **WebSockets**: Real-time updates for long-running AI tasks
5. **Multi-tenancy**: Support multiple organizations
6. **Audit Logging**: Track all user actions
7. **Backup Strategy**: Automated database backups

