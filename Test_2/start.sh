#!/bin/bash

echo "🚀 Starting Requirements Analyzer..."
echo ""

# Check if Docker is running
if ! docker info > /dev/null 2>&1; then
    echo "❌ Error: Docker is not running."
    echo "Please start Docker Desktop and try again."
    exit 1
fi

echo "✅ Docker is running"
echo ""

# Start services
echo "📦 Starting services with Docker Compose..."
docker-compose up -d

echo ""
echo "⏳ Waiting for services to be ready..."
echo "   This may take 30-60 seconds on first run..."
echo ""

# Wait for backend to be ready
echo "Waiting for backend API..."
for i in {1..60}; do
    if curl -s http://localhost:8080/api/requirement > /dev/null 2>&1; then
        echo "✅ Backend API is ready"
        break
    fi
    
    if [ $i -eq 60 ]; then
        echo "⚠️  Backend is taking longer than expected"
        echo "   Check status with: docker-compose ps"
        echo "   Check logs with: docker-compose logs backend"
    fi
    
    sleep 2
done

echo ""
echo "🎉 Application is ready!"
echo ""
echo "📱 Access the application:"
echo "   Frontend:  http://localhost:3000"
echo "   API:       http://localhost:8080"
echo "   Swagger:   http://localhost:8080/swagger"
echo ""
echo "📋 Useful commands:"
echo "   View logs:    docker-compose logs -f"
echo "   Stop:         docker-compose down"
echo "   Restart:      docker-compose restart"
echo ""
echo "📖 For more information, see README.md or QUICK_START.md"
echo ""

