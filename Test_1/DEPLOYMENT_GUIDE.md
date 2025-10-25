# Deployment Guide - Sistema de Adquisiciones CAASIM

This guide provides step-by-step instructions for deploying the complete application stack.

## Table of Contents

- [Quick Start](#quick-start)
- [Prerequisites](#prerequisites)
- [Docker Deployment (Recommended)](#docker-deployment-recommended)
- [Manual Deployment](#manual-deployment)
- [Post-Deployment Verification](#post-deployment-verification)
- [Troubleshooting](#troubleshooting)

## Quick Start

For the fastest deployment using Docker:

```bash
# 1. Clone the repository
cd total-tech-test

# 2. Start all services
docker-compose up -d

# 3. Wait for services to be healthy (30-60 seconds)
docker-compose ps

# 4. Access the application
# Frontend: http://localhost:8080
# Backend API: http://localhost:5006
# Swagger Docs: http://localhost:5006/swagger
```

Default login credentials:
- **Email**: admin@caasim.gob.mx
- **Password**: admin123

## Prerequisites

### Docker Deployment

- Docker 20.10+ installed
- Docker Compose 2.0+ installed
- At least 2GB of free RAM
- Ports 5432, 5006, and 8080 available

### Manual Deployment

- PostgreSQL 17
- .NET 9 SDK
- Node.js 20.19+ or 22.12+
- npm or yarn

## Docker Deployment (Recommended)

### Step 1: Environment Configuration

```bash
# Copy the example environment file
cp .env.example .env

# (Optional) Edit .env to customize settings
nano .env
```

Key environment variables:

```env
# Database
POSTGRES_USER=adquisiciones_user
POSTGRES_PASSWORD=Change_This_Password_123!
POSTGRES_DB=adquisiciones_db

# JWT Secret (CHANGE IN PRODUCTION!)
JWT__Secret=ThisIsAVeryLongSecretKeyForJWTTokenGeneration_ChangeThis_MinimumLength64Characters!

# CORS Origins
CORS__AllowedOrigins=http://localhost:8080,http://localhost:5173
```

### Step 2: Start Services

```bash
# Start all services in detached mode
docker-compose up -d

# View logs for all services
docker-compose logs -f

# View logs for specific service
docker-compose logs -f backend
```

### Step 3: Verify Services

```bash
# Check service health
docker-compose ps

# Expected output:
# NAME                    STATUS
# adquisiciones-db        Up (healthy)
# adquisiciones-api       Up (healthy)
# adquisiciones-frontend  Up (healthy)
```

### Step 4: Access the Application

| Service | URL | Description |
|---------|-----|-------------|
| Frontend | http://localhost:8080 | Main application UI |
| Backend API | http://localhost:5006 | REST API endpoints |
| Swagger | http://localhost:5006/swagger | API documentation |
| Health Check | http://localhost:5006/health | API health status |
| Database | localhost:5432 | PostgreSQL (use psql or pgAdmin) |

### Step 5: Test the Application

1. **Open the frontend**: Navigate to http://localhost:8080
2. **Login** with default credentials:
   - Email: `admin@caasim.gob.mx`
   - Password: `admin123`
3. **Verify data**: You should see 3 sample orders in the list

## Manual Deployment

### 1. Database Setup

```bash
# Start PostgreSQL
brew services start postgresql@17

# Create database
createdb adquisiciones_db

# Run migrations
cd database
psql -U your_username -d adquisiciones_db -f init.sql

# Verify tables
psql -U your_username -d adquisiciones_db -c "\dt"
```

### 2. Backend Setup

```bash
# Navigate to backend
cd backend/AdquisicionesAPI

# Restore dependencies
dotnet restore

# Update connection string in appsettings.json
nano appsettings.json

# Build the project
dotnet build

# Run the API
dotnet run
```

The API should now be running at http://localhost:5006

### 3. Frontend Setup

```bash
# Navigate to frontend
cd frontend

# Install dependencies
npm install

# Create local environment file
cat > .env.local << EOF
VITE_API_BASE_URL=http://localhost:5006
EOF

# Start development server
npm run dev
```

The frontend should now be running at http://localhost:5173

### 4. Production Build (Frontend)

```bash
# Build for production
npm run build

# Preview production build
npm run preview

# Or serve with a static server
npx serve -s dist -p 8080
```

## Post-Deployment Verification

### 1. API Health Check

```bash
# Check API health
curl http://localhost:5006/health

# Expected response: Healthy
```

### 2. Authentication Test

```bash
# Obtain JWT token
curl -X GET "http://localhost:5006/oauth/token?grant_type=password&username=admin@caasim.gob.mx&password=admin123"

# Expected response:
# {
#   "accessToken": "eyJhbGciOiJIUzI1...",
#   "tokenType": "Bearer",
#   "expiresIn": 3600,
#   ...
# }
```

### 3. API Endpoint Test

```bash
# Save the token from previous step
TOKEN="your_token_here"

# Test ListaSelAll endpoint
curl -X GET "http://localhost:5006/api/pedido/ListaSelAll?startRowIndex=1&maximumRows=10" \
  -H "Authorization: Bearer $TOKEN"

# Expected response: JSON with pedidos array
```

### 4. Database Verification

```bash
# Connect to database
docker exec -it adquisiciones-db psql -U adquisiciones_user -d adquisiciones_db

# Check tables
\dt

# Count records
SELECT COUNT(*) FROM pedido;
-- Expected: 3 sample orders

SELECT COUNT(*) FROM spartan_user;
-- Expected: 4 users

# Exit
\q
```

### 5. Frontend Verification

1. Open http://localhost:8080 in your browser
2. You should see the login page
3. Login with `admin@caasim.gob.mx` / `admin123`
4. You should be redirected to the Pedidos list
5. Verify you can see 3 sample orders
6. Test filters, pagination, and detail view

## Troubleshooting

### Database Issues

**Problem**: Database container fails to start

```bash
# Check logs
docker-compose logs database

# Common solutions:
# 1. Port 5432 already in use - stop local PostgreSQL
brew services stop postgresql@17

# 2. Permission issues - remove volume and restart
docker-compose down -v
docker-compose up -d
```

**Problem**: Database migrations didn't run

```bash
# Run migrations manually
docker exec -it adquisiciones-db psql -U adquisiciones_user -d adquisiciones_db -f /docker-entrypoint-initdb.d/01-init.sql
```

### Backend Issues

**Problem**: Backend can't connect to database

```bash
# Check if database is healthy
docker-compose ps database

# Restart backend
docker-compose restart backend

# Check backend logs
docker-compose logs backend
```

**Problem**: CORS errors in browser console

```bash
# Update CORS origins in .env
CORS__AllowedOrigins=http://localhost:8080,http://localhost:5173,http://yourfrontend.com

# Restart backend
docker-compose restart backend
```

### Frontend Issues

**Problem**: Frontend shows "Network Error" when calling API

- Verify backend is running: http://localhost:5006
- Check browser console for CORS errors
- Verify VITE_API_BASE_URL in .env matches backend URL

**Problem**: Frontend build fails

```bash
# Update Node.js to 20.19+ or 22.12+
node --version

# Clear node_modules and reinstall
cd frontend
rm -rf node_modules package-lock.json
npm install
npm run build
```

### General Issues

**Problem**: Port conflicts

```bash
# Check what's using the ports
lsof -i :5432  # Database
lsof -i :5006  # Backend
lsof -i :8080  # Frontend

# Change ports in docker-compose.yml if needed
# Example: "8081:80" instead of "8080:80"
```

**Problem**: Services stuck in "starting" state

```bash
# Wait longer (initial startup can take 60 seconds)
docker-compose ps

# Check logs for errors
docker-compose logs

# Restart specific service
docker-compose restart backend
```

## Maintenance Commands

### View Logs

```bash
# All services
docker-compose logs -f

# Specific service
docker-compose logs -f backend

# Last 100 lines
docker-compose logs --tail=100 backend
```

### Restart Services

```bash
# Restart all
docker-compose restart

# Restart specific service
docker-compose restart backend
```

### Update and Rebuild

```bash
# Rebuild all services
docker-compose up -d --build

# Rebuild specific service
docker-compose up -d --build backend
```

### Database Backup

```bash
# Backup database
docker exec adquisiciones-db pg_dump -U adquisiciones_user adquisiciones_db > backup.sql

# Restore database
cat backup.sql | docker exec -i adquisiciones-db psql -U adquisiciones_user -d adquisiciones_db
```

### Clean Up

```bash
# Stop services
docker-compose down

# Remove volumes (WARNING: deletes database data)
docker-compose down -v

# Remove all (containers, networks, volumes, images)
docker-compose down -v --rmi all
```

## Production Deployment Checklist

Before deploying to production:

- [ ] Change database password in .env
- [ ] Generate new JWT secret (64+ characters)
- [ ] Update CORS origins with production URLs
- [ ] Enable HTTPS/TLS
- [ ] Configure proper logging and monitoring
- [ ] Set up database backups
- [ ] Review security headers in nginx.conf
- [ ] Change default user passwords
- [ ] Set ASPNETCORE_ENVIRONMENT=Production
- [ ] Configure reverse proxy (nginx/Apache)
- [ ] Set up SSL certificates
- [ ] Configure firewall rules
- [ ] Enable rate limiting
- [ ] Set up health monitoring
- [ ] Configure error tracking (e.g., Sentry)

## Support

For issues or questions:

1. Check this troubleshooting guide
2. Review logs: `docker-compose logs`
3. Consult [README.md](README.md) for detailed documentation
4. Review [IMPLEMENTATION_PLAN.md](IMPLEMENTATION_PLAN.md) for architecture details

---

**Last Updated**: October 2025
**Version**: 1.0.0
