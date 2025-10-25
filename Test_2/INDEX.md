# Requirements Analyzer - Documentation Index

Welcome to the Requirements Analyzer documentation. This index will help you find the information you need.

## 📚 Quick Navigation

### Getting Started
1. **[QUICK_START.md](QUICK_START.md)** - ⭐ Start here! Quick 5-minute setup guide
2. **[README.md](README.md)** - Complete documentation and features

### Understanding the Project
3. **[PROJECT_SUMMARY.md](PROJECT_SUMMARY.md)** - High-level overview and design decisions
4. **[ARCHITECTURE.md](ARCHITECTURE.md)** - Technical architecture and design patterns
5. **[Diccionario_de_datos_2.md](Diccionario_de_datos_2.md)** - Database schema reference

### When Things Go Wrong
6. **[TROUBLESHOOTING.md](TROUBLESHOOTING.md)** - Solutions for common issues

### Testing
7. **[test-api.http](test-api.http)** - HTTP file for testing API endpoints
8. **[postman-collection.json](postman-collection.json)** - Postman collection for API testing

## 🚀 Quick Start Commands

### Start the Application
```bash
# Option 1: Using the helper script (recommended)
./start.sh

# Option 2: Using docker-compose directly
docker-compose up -d
```

### Stop the Application
```bash
# Option 1: Using the helper script
./stop.sh

# Option 2: Using docker-compose
docker-compose down         # Preserves data
docker-compose down -v      # Removes data
```

### View Logs
```bash
docker-compose logs -f              # All services
docker-compose logs -f backend      # Backend only
docker-compose logs -f frontend     # Frontend only
docker-compose logs -f sqlserver    # Database only
```

## 📖 Documentation Guide

### For First-Time Users
1. Read **QUICK_START.md** for immediate setup
2. Try the example requirements provided
3. If issues occur, check **TROUBLESHOOTING.md**

### For Developers
1. Read **ARCHITECTURE.md** for technical details
2. Review **PROJECT_SUMMARY.md** for design decisions
3. Check **Diccionario_de_datos_2.md** for database schema
4. Use **test-api.http** or **postman-collection.json** for API testing

### For System Administrators
1. Read **README.md** for deployment information
2. Review **docker-compose.yml** for service configuration
3. Check **TROUBLESHOOTING.md** for common issues
4. Monitor services with `docker-compose ps` and `docker-compose logs`

## 🗂️ Project Structure

```
Test_2/
├── 📄 Documentation
│   ├── INDEX.md (this file)          - Documentation index
│   ├── README.md                      - Main documentation
│   ├── QUICK_START.md                 - Fast start guide
│   ├── PROJECT_SUMMARY.md             - Project overview
│   ├── ARCHITECTURE.md                - Technical details
│   ├── TROUBLESHOOTING.md             - Problem solving
│   └── Diccionario_de_datos_2.md     - Database schema
│
├── 🚀 Scripts
│   ├── start.sh                       - Start application
│   └── stop.sh                        - Stop application
│
├── 🧪 Testing
│   ├── test-api.http                  - HTTP tests
│   └── postman-collection.json        - Postman tests
│
├── 🐳 Docker
│   ├── docker-compose.yml             - Orchestration
│   ├── .dockerignore                  - Docker ignore
│   └── .gitignore                     - Git ignore
│
├── 💾 Database
│   └── database/
│       ├── init.sql                   - Schema
│       └── entrypoint.sh              - Init script
│
├── 🎨 Frontend
│   └── frontend/
│       ├── src/                       - Source code
│       ├── Dockerfile                 - Frontend image
│       └── package.json               - Dependencies
│
└── ⚙️ Backend
    └── backend/
        └── RequirementsAPI/
            ├── Controllers/           - API endpoints
            ├── Services/              - Business logic
            ├── Models/                - Data models
            ├── Data/                  - Database context
            └── Program.cs             - App entry point
```

## 🔗 External Resources

### Technologies Used
- [Vue.js Documentation](https://vuejs.org/)
- [.NET Documentation](https://docs.microsoft.com/dotnet/)
- [Entity Framework Core](https://docs.microsoft.com/ef/core/)
- [Element Plus](https://element-plus.org/)
- [Docker Documentation](https://docs.docker.com/)
- [OpenRouter API](https://openrouter.ai/docs)

## ⚡ Common Tasks

### First Time Setup
```bash
cd Test_2
./start.sh
# Wait for services to start
# Open http://localhost:3000
```

### Daily Development
```bash
# Start services
docker-compose up -d

# Watch logs
docker-compose logs -f backend

# Make changes to code
# Rebuild specific service
docker-compose build backend
docker-compose up -d backend
```

### Troubleshooting
```bash
# Check service status
docker-compose ps

# View all logs
docker-compose logs

# Restart a service
docker-compose restart backend

# Complete reset
docker-compose down -v
docker-compose up -d
```

### Testing API
```bash
# Get all requirements
curl http://localhost:8080/api/requirement

# Analyze requirement
curl -X POST http://localhost:8080/api/requirement/analyze \
  -H "Content-Type: application/json" \
  -d '{"requirementText":"Create a blog system"}'
```

## 📊 Application URLs

| Service | URL | Description |
|---------|-----|-------------|
| Frontend | http://localhost:3000 | Main application UI |
| Backend API | http://localhost:8080 | REST API |
| Swagger Docs | http://localhost:8080/swagger | API documentation |
| Database | localhost:1433 | SQL Server (internal) |

## 🎯 Key Features

✅ AI-powered requirements analysis
✅ Automatic generation of processes and use cases
✅ Database persistence
✅ Browse analysis history
✅ Detailed view of each analysis
✅ Modern, responsive UI
✅ RESTful API
✅ Docker containerization
✅ Comprehensive documentation

## 💡 Tips

1. **First run takes longer** - Docker needs to download images
2. **Wait for services** - Give it 30-60 seconds to fully start
3. **Check logs often** - `docker-compose logs -f` is your friend
4. **Preserve data** - Use `docker-compose down` without `-v` flag
5. **Read error messages** - They usually point to the issue
6. **Test with examples** - Use provided example requirements first

## 🆘 Need Help?

1. Check **[TROUBLESHOOTING.md](TROUBLESHOOTING.md)** for common issues
2. Run `docker-compose logs` to see what's happening
3. Verify Docker Desktop is running
4. Make sure ports 3000, 8080, 1433 are available
5. Try a complete reset: `docker-compose down -v && docker-compose up -d`

## 📝 Version Information

- **Project**: Requirements Analyzer v1.0
- **Vue.js**: 3.5
- **.NET**: 9.0
- **SQL Server**: 2022
- **Element Plus**: 2.9
- **Docker Compose**: 3.8

## 🎓 Learning Path

### Beginner
1. Start with **QUICK_START.md**
2. Use the application through the UI
3. Try different requirement examples
4. View the generated analyses

### Intermediate
1. Read **PROJECT_SUMMARY.md**
2. Explore the API with Swagger
3. Test endpoints with Postman
4. Modify example requirements

### Advanced
1. Study **ARCHITECTURE.md**
2. Review the source code
3. Understand database relationships
4. Customize and extend features

## 🔄 Document Updates

This documentation is comprehensive and up-to-date as of creation. For the latest changes:
- Check git history
- Review docker-compose.yml for configuration
- Check package.json files for dependencies

---

**Last Updated**: 2024 (Initial Release)
**Documentation Version**: 1.0
**Project Status**: ✅ Complete and Production-Ready

**Happy Analyzing! 🚀**

