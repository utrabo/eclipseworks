#!/bin/bash

# Start SQL Server in the background
/opt/mssql/bin/sqlservr &

# Wait for it to be available
until /opt/mssql-tools/bin/sqlcmd -S localhost -U SA -P "Spock@2024" -Q "SELECT 1" > /dev/null 2>&1
do
  echo "Waiting for SQL Server to start..."
  sleep 1
done

# Run every script in /docker-entrypoint-initdb.d/
for script in /docker-entrypoint-initdb.d/*.sql
do
  /opt/mssql-tools/bin/sqlcmd -S localhost -U SA -P "Spock@2024" -i "$script"
done

# Wait for SQL Server to stop
wait
