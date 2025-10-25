# Troubleshooting Guide

## Common Issues and Solutions

### 1. Docker Issues

#### Docker Not Running
**Symptom**: Error message "Cannot connect to Docker daemon"

**Solution**:
```bash
# Start Docker Desktop application
# Wait for it to fully start (whale icon in system tray)
# Then try again
```

#### Ports Already in Use
**Symptom**: Error about port 3000, 8080, or 1433 already in use

**Solution 1** - Stop conflicting services:
```bash
# Check what's using the ports
lsof -i :3000
lsof -i :8080
lsof -i :1433

# Kill the process or stop the service
```

**Solution 2** - Change ports in docker-compose.yml:
```yaml
ports:
  - "3001:80"    # Frontend (change 3000 to 3001)
  - "8081:8080"  # Backend (change 8080 to 8081)
  - "1434:1433"  # SQL Server (change 1433 to 1434)
```

#### Out of Disk Space
**Symptom**: Docker errors about disk space

**Solution**:
```bash
# Clean up Docker resources
docker system prune -a --volumes

# Remove only unused volumes
docker volume prune
```

### 2. SQL Server Issues

#### Database Not Initializing
**Symptom**: Backend can't connect to database

**Solution**:
```bash
# Check if SQL Server is healthy
docker-compose ps

# View SQL Server logs
docker-compose logs sqlserver

# Check if db-init completed
docker-compose logs db-init

# Restart SQL Server
docker-compose restart sqlserver

# If that doesn't work, recreate everything
docker-compose down -v
docker-compose up -d
```

#### Connection String Error
**Symptom**: "A network-related or instance-specific error occurred"

**Solution**:
```bash
# Test SQL Server connection manually
docker exec -it requirements-sqlserver /opt/mssql-tools18/bin/sqlcmd \
  -S localhost -U sa -P YourStrong@Passw0rd -C -Q "SELECT 1"

# If this fails, SQL Server is not ready
# Wait 30 seconds and try again
```

### 3. Backend API Issues

#### Backend Won't Start
**Symptom**: Backend container keeps restarting

**Solution**:
```bash
# Check backend logs
docker-compose logs backend

# Common issues:
# 1. Database not ready - wait and it will retry
# 2. Build errors - check if .NET 9 SDK is in the image
# 3. Port conflict - change port in docker-compose.yml

# Rebuild the backend
docker-compose build backend
docker-compose up -d backend
```

#### API Returns 500 Error
**Symptom**: Frontend shows "Failed to analyze requirements"

**Solution**:
```bash
# Check backend logs for specific error
docker-compose logs backend --tail=100

# Common causes:
# 1. OpenRouter API key invalid - check appsettings.json
# 2. Database connection issue - restart sqlserver
# 3. AI response format unexpected - check logs for details
```

#### Swagger Not Loading
**Symptom**: http://localhost:8080/swagger shows error

**Solution**:
```bash
# Ensure backend is fully started
curl http://localhost:8080/api/requirement

# If curl works but Swagger doesn't, it's a Swagger issue
# Check backend logs
docker-compose logs backend
```

### 4. Frontend Issues

#### Frontend Shows Blank Page
**Symptom**: White screen at http://localhost:3000

**Solution**:
```bash
# Check frontend logs
docker-compose logs frontend

# Check browser console (F12)
# Look for JavaScript errors

# Rebuild frontend
docker-compose build frontend
docker-compose up -d frontend
```

#### Can't Connect to Backend
**Symptom**: "Network Error" or "Failed to fetch"

**Solution**:
1. Check if backend is running: http://localhost:8080/swagger
2. Check browser console for CORS errors
3. Verify API URL in frontend:
   - Check `frontend/src/services/api.js`
   - Should be `http://localhost:8080`

```bash
# Test backend directly
curl http://localhost:8080/api/requirement

# If backend works, it's a CORS or frontend config issue
```

#### Frontend Can't Load After Build
**Symptom**: 404 errors on routes

**Solution**:
- Check `frontend/nginx.conf` has proper `try_files` directive
- Should have: `try_files $uri $uri/ /index.html;`

### 5. AI Analysis Issues

#### Analysis Takes Too Long
**Symptom**: "Analyzing..." for more than 60 seconds

**Solution**:
1. Check backend logs for OpenRouter API errors
2. Verify internet connection
3. Test OpenRouter API key:
```bash
curl -X POST https://openrouter.ai/api/v1/chat/completions \
  -H "Authorization: Bearer YOUR_API_KEY" \
  -H "Content-Type: application/json" \
  -d '{"model":"deepseek/deepseek-chat","messages":[{"role":"user","content":"test"}]}'
```

#### AI Returns Invalid Data
**Symptom**: "Error analyzing requirement" or partial results

**Solution**:
- Check backend logs for JSON parsing errors
- The AI sometimes returns markdown-wrapped JSON
- Backend should clean this automatically
- If issue persists, try simpler requirements

#### No Subprocesses or Use Cases Generated
**Symptom**: Only process is created, no subprocesses

**Solution**:
- Provide more detailed requirements
- Include specific features or user actions
- Example of good requirement:
  ```
  Create a user management system where administrators can:
  1. Add new users with roles
  2. Edit existing user profiles
  3. Deactivate or delete users
  4. View user activity logs
  Users should be able to log in and update their own profiles.
  ```

### 6. Performance Issues

#### Slow First Start
**Symptom**: Takes 5+ minutes to start

**Solution**:
- First time pulls Docker images (normal)
- Building images takes time (normal)
- SQL Server initialization takes 30-60 seconds (normal)
- Subsequent starts are much faster

#### Backend Slow to Respond
**Symptom**: API calls take 20+ seconds

**Solution**:
```bash
# Check container resources
docker stats

# If CPU/Memory is high, increase Docker resources:
# Docker Desktop → Settings → Resources
# Recommended: 4GB RAM, 2 CPUs minimum
```

### 7. Data Issues

#### Lost All Data
**Symptom**: History is empty after restart

**Solution**:
- Check if you used `docker-compose down -v` (this deletes volumes)
- To preserve data, use `docker-compose down` without `-v`
- Data is in Docker volume: `sqlserver-data`

```bash
# List volumes
docker volume ls

# Inspect volume
docker volume inspect test_2_sqlserver-data
```

#### Can't Delete/Reset Data
**Symptom**: Want to start fresh

**Solution**:
```bash
# Complete reset (WARNING: deletes all data)
docker-compose down -v
docker-compose up -d

# This removes all containers, networks, and volumes
```

### 8. Build Issues

#### Frontend Build Fails
**Symptom**: "npm install" or "npm run build" errors

**Solution**:
```bash
# Clear npm cache
cd frontend
rm -rf node_modules package-lock.json
npm cache clean --force
npm install

# Or rebuild with Docker
docker-compose build --no-cache frontend
```

#### Backend Build Fails
**Symptom**: dotnet restore or build errors

**Solution**:
```bash
# Check .NET version
docker run --rm mcr.microsoft.com/dotnet/sdk:9.0 dotnet --version

# Rebuild with no cache
docker-compose build --no-cache backend
```

## Diagnostic Commands

### Check All Services Status
```bash
docker-compose ps
```

### View All Logs
```bash
docker-compose logs -f
```

### View Specific Service Logs
```bash
docker-compose logs -f backend
docker-compose logs -f frontend
docker-compose logs -f sqlserver
```

### Test Database Connection
```bash
docker exec -it requirements-sqlserver /opt/mssql-tools18/bin/sqlcmd \
  -S localhost -U sa -P YourStrong@Passw0rd -C \
  -Q "SELECT name FROM sys.databases"
```

### Test Backend API
```bash
# Health check
curl http://localhost:8080/api/requirement

# Test analysis
curl -X POST http://localhost:8080/api/requirement/analyze \
  -H "Content-Type: application/json" \
  -d '{"requirementText":"Test requirement"}'
```

### Check Container Resource Usage
```bash
docker stats
```

### Restart Individual Service
```bash
docker-compose restart backend
docker-compose restart frontend
docker-compose restart sqlserver
```

### Complete Rebuild
```bash
# Stop everything
docker-compose down

# Rebuild all images
docker-compose build --no-cache

# Start everything
docker-compose up -d
```

## Getting Help

If none of these solutions work:

1. Collect diagnostic information:
```bash
# Save all logs
docker-compose logs > debug-logs.txt

# Check service status
docker-compose ps > debug-status.txt

# Check disk space
df -h > debug-disk.txt

# Check Docker version
docker version > debug-docker.txt
```

2. Check the following:
   - Docker Desktop is running and healthy
   - You have at least 4GB free disk space
   - No other services using ports 3000, 8080, 1433
   - Your internet connection is working (for AI API)

3. Try a complete reset:
```bash
docker-compose down -v
docker system prune -a
docker-compose up -d
```

## Prevention Tips

1. Always use `docker-compose down` without `-v` to preserve data
2. Regularly clean up Docker: `docker system prune`
3. Keep Docker Desktop updated
4. Don't modify files while containers are running
5. Always check logs when something doesn't work
6. Wait for services to be fully healthy before using them

## Quick Reference

| Issue | Quick Fix |
|-------|-----------|
| Backend won't start | `docker-compose restart backend` |
| Database connection error | Wait 30 seconds, or restart sqlserver |
| Frontend blank page | Check browser console, rebuild frontend |
| Port in use | Change port in docker-compose.yml |
| Lost data | Don't use `-v` flag with down command |
| Slow performance | Increase Docker resources |
| AI not responding | Check internet, verify API key |
| Complete reset | `docker-compose down -v && docker-compose up -d` |

