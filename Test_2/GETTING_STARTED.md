# Getting Started with Requirements Analyzer

## 🎯 What You'll Build

A complete AI-powered web application that analyzes software requirements and generates structured processes, subprocesses, and use cases automatically.

## ⚡ 5-Minute Quick Start

### Step 1: Prerequisites Check
- ✅ Docker Desktop installed and running
- ✅ Ports 3000, 8080, 1433 available
- ✅ At least 4GB free RAM

### Step 2: Start the Application
```bash
cd Test_2
./start.sh
```

Or manually:
```bash
docker-compose up -d
```

### Step 3: Wait (30-60 seconds)
Services need time to start. You'll know it's ready when:
- ✅ Backend responds at http://localhost:8080/swagger
- ✅ Frontend loads at http://localhost:3000

### Step 4: Try It Out!

1. Open http://localhost:3000
2. Enter this example requirement:
```
Create a task management application where users can create tasks, 
assign them to team members, set due dates, and mark tasks as complete.
```
3. Click "Analyze Requirements"
4. Wait 10-30 seconds for AI to process
5. View the generated results!

## 📱 What You'll See

### 1. Analyzer Page (Home)
- Large text area for entering requirements
- "Analyze Requirements" button
- Real-time results display
- Organized view of processes, subprocesses, and use cases

### 2. History Page
- Cards showing all previous analyses
- Click any card to view details
- Shows creation date and statistics

### 3. Detail Page
- Complete breakdown of selected analysis
- Expandable sections for use cases
- All details including actors, preconditions, acceptance criteria

## 🎨 Interface Overview

```
┌─────────────────────────────────────────────────────┐
│  Requirements Analyzer - AI Powered                  │
├─────────────────────────────────────────────────────┤
│                                                      │
│  [Input Form]                                        │
│  ┌──────────────────────────────────────────────┐  │
│  │ Enter your software requirements here...     │  │
│  │                                              │  │
│  │                                              │  │
│  └──────────────────────────────────────────────┘  │
│                                                      │
│  [ Analyze Requirements ]  [ View History ]         │
│                                                      │
│  Results appear here after analysis:                │
│  ├─ 📋 Process Name                                 │
│  │   └─ Subprocess 1                                │
│  │       └─ Use Case 1.1                            │
│  │       └─ Use Case 1.2                            │
│  │   └─ Subprocess 2                                │
│  │       └─ Use Case 2.1                            │
│                                                      │
└─────────────────────────────────────────────────────┘
```

## 🧪 Test Examples

### Example 1: Simple Blog
```
Create a blog where users can write posts, add comments, and like articles.
```

### Example 2: E-commerce
```
Build an online store where customers can browse products, add items to cart, 
checkout with credit card, and track orders. Include an admin panel for 
managing products and viewing sales.
```

### Example 3: Healthcare System
```
Develop a patient management system where doctors can view schedules, 
access patient history, write prescriptions, and nurses can update 
vital signs. Patients should be able to book appointments online.
```

## 🔧 Useful Commands

### View Application Status
```bash
docker-compose ps
```

### Watch Logs Live
```bash
docker-compose logs -f
```

### Restart Services
```bash
docker-compose restart
```

### Stop Application (Keep Data)
```bash
docker-compose down
```

### Complete Reset (Delete All Data)
```bash
docker-compose down -v
docker-compose up -d
```

## 🌐 Access Points

| What | Where | When to Use |
|------|-------|-------------|
| **Web App** | http://localhost:3000 | Main interface for users |
| **API Docs** | http://localhost:8080/swagger | Test API directly |
| **API Base** | http://localhost:8080 | For programmatic access |

## 🎓 Learning Path

### Hour 1: Basic Usage
- ✅ Start the application
- ✅ Submit a simple requirement
- ✅ View the results
- ✅ Check the history

### Hour 2: Explore Features
- ✅ Try different types of requirements
- ✅ Explore the detail view
- ✅ Use Swagger to test API
- ✅ View database contents

### Hour 3: Understand Architecture
- ✅ Read ARCHITECTURE.md
- ✅ Explore the code structure
- ✅ Understand data flow
- ✅ Review Docker setup

### Hour 4: Customize
- ✅ Modify UI components
- ✅ Add new fields to database
- ✅ Customize AI prompts
- ✅ Add new features

## 📚 Next Steps

After getting started, explore:

1. **[README.md](README.md)** - Complete documentation
2. **[ARCHITECTURE.md](ARCHITECTURE.md)** - How it works
3. **[TROUBLESHOOTING.md](TROUBLESHOOTING.md)** - If issues arise
4. **[PROJECT_SUMMARY.md](PROJECT_SUMMARY.md)** - Design decisions

## ❓ Quick FAQ

**Q: How long does analysis take?**  
A: 10-30 seconds depending on complexity and AI service load.

**Q: Is my data saved?**  
A: Yes! All analyses are saved to the database and persist between restarts.

**Q: Can I edit generated results?**  
A: Not in the current version, but you can re-analyze with modified requirements.

**Q: What if I get an error?**  
A: Check [TROUBLESHOOTING.md](TROUBLESHOOTING.md) for solutions to common issues.

**Q: Can I use a different AI model?**  
A: Yes! Modify the model name in `backend/RequirementsAPI/Services/Implementation/OpenRouterService.cs`

**Q: Is this production-ready?**  
A: It's a demo without authentication. Add security features before production use.

## 🎉 You're Ready!

You now have everything you need to start using the Requirements Analyzer. 

### Quick Checklist
- [ ] Docker is running
- [ ] Services are started (`docker-compose up -d`)
- [ ] Frontend loads (http://localhost:3000)
- [ ] Backend responds (http://localhost:8080/swagger)
- [ ] Tried first analysis

### If Something's Wrong
1. Run `docker-compose ps` - Are all services up?
2. Run `docker-compose logs` - Any errors?
3. Check [TROUBLESHOOTING.md](TROUBLESHOOTING.md)
4. Try `docker-compose restart`

---

**Ready to analyze some requirements? Let's go! 🚀**

Visit: http://localhost:3000

