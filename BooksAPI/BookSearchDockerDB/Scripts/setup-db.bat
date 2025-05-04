@echo off
echo Loading Docker image from booksearch-sql.tar...
docker load -i booksearch-sql.tar

echo Starting SQL Server container with docker-compose...
docker-compose up -d

echo SQL Server is now running at localhost:1433