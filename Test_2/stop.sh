#!/bin/bash

echo "🛑 Stopping Requirements Analyzer..."
echo ""

# Ask if user wants to remove volumes
read -p "Do you want to remove all data? (y/N): " -n 1 -r
echo ""

if [[ $REPLY =~ ^[Yy]$ ]]; then
    echo "🗑️  Stopping and removing all data..."
    docker-compose down -v
    echo "✅ All services stopped and data removed"
else
    echo "💾 Stopping services (data will be preserved)..."
    docker-compose down
    echo "✅ All services stopped (data preserved)"
fi

echo ""
echo "To start again, run: ./start.sh or docker-compose up -d"
echo ""

