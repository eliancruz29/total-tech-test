# Quick Start Guide

## Starting the Application

### Step 1: Ensure Docker is Running
Make sure Docker Desktop is running on your machine.

### Step 2: Start All Services
```bash
cd Test_2
docker-compose up -d
```

### Step 3: Wait for Services to Start
The application takes about 30-60 seconds to fully start. You can check the status:
```bash
docker-compose ps
```

All services should show "Up" or "Up (healthy)".

### Step 4: Access the Application
Open your browser and go to:
- **Application**: http://localhost:3000
- **API Documentation**: http://localhost:8080/swagger

## Using the Application

### Analyze Requirements

1. On the home page, you'll see a large text area
2. Enter your software requirements (see examples below)
3. Click "Analyze Requirements"
4. Wait 10-30 seconds for the AI to process
5. View the generated processes, subprocesses, and use cases
6. Results are automatically saved

### Quick Example

Try this simple requirement:
```
Create a task management application where users can create tasks, 
assign them to team members, set due dates, and mark tasks as complete.
```

### View History

1. Click "View History" button in the top right
2. See all previously analyzed requirements
3. Click any card to see full details

## Stopping the Application

```bash
docker-compose down
```

To remove all data (complete reset):
```bash
docker-compose down -v
```

## Troubleshooting

### Port Already in Use
If you see port conflict errors, stop the conflicting service or change ports in `docker-compose.yml`.

### Backend Not Starting
Check logs:
```bash
docker-compose logs backend
```

### Frontend Shows Connection Error
1. Verify backend is running: http://localhost:8080/swagger
2. Wait a bit longer - backend takes time to start
3. Check logs: `docker-compose logs backend`

### Database Issues
Restart the database:
```bash
docker-compose restart sqlserver
```

## Tips

- First-time startup takes longer (downloading images)
- Subsequent starts are much faster
- Keep Docker Desktop running while using the app
- The AI analysis quality depends on how detailed your requirements are

## Example Requirements to Try

### Simple
```
Build a blog where users can write posts and add comments.
```

### Medium
```
Create a customer support system with ticket creation, agent assignment, 
priority levels, status tracking, and email notifications.
```

### Complex
```
Develop a comprehensive learning management system (LMS) where instructors 
can create courses with video lessons and quizzes, students can enroll and 
track progress, administrators can manage users and generate reports, and 
the system supports certificates upon course completion.
```

Enjoy using the Requirements Analyzer! 🚀

