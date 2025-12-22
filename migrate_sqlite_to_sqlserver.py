#!/usr/bin/env python3
import sqlite3
import pyodbc
import sys
from pathlib import Path

# SQLite connection
sqlite_db = Path(__file__).parent / "NEPlumbingInc" / "app.db"
sqlite_conn = sqlite3.connect(sqlite_db)
sqlite_conn.row_factory = sqlite3.Row
sqlite_cursor = sqlite_conn.cursor()

# SQL Server connection
sql_server_conn_str = "Driver={ODBC Driver 17 for SQL Server};Server=localhost,1433;Database=NEPlumbingIncDB;UID=sa;PWD=YourPassword123!;TrustServerCertificate=yes;"
try:
    sql_conn = pyodbc.connect(sql_server_conn_str)
    sql_cursor = sql_conn.cursor()
except Exception as e:
    print(f"Error connecting to SQL Server: {e}")
    sys.exit(1)

# Get all tables from SQLite (excluding system tables)
sqlite_cursor.execute("""
    SELECT name FROM sqlite_master 
    WHERE type='table' 
    AND name NOT IN ('sqlite_sequence', '__EFMigrationsHistory')
    ORDER BY name
""")
tables = [row[0] for row in sqlite_cursor.fetchall()]

print(f"Found {len(tables)} tables to migrate")

for table in tables:
    print(f"\nMigrating table: {table}")
    
    # Get data from SQLite
    sqlite_cursor.execute(f'SELECT * FROM "{table}"')
    rows = sqlite_cursor.fetchall()
    columns = [description[0] for description in sqlite_cursor.description]
    
    if not rows:
        print(f"  No data to migrate")
        continue
    
    # Insert into SQL Server
    placeholders = ",".join(["?" for _ in columns])
    insert_sql = f"INSERT INTO [{table}] ({','.join([f'[{col}]' for col in columns])}) VALUES ({placeholders})"
    
    try:
        sql_cursor.executemany(insert_sql, [tuple(row) for row in rows])
        sql_conn.commit()
        print(f"  ✓ Migrated {len(rows)} rows")
    except Exception as e:
        print(f"  ✗ Error: {e}")
        sql_conn.rollback()

sqlite_conn.close()
sql_conn.close()
print("\nMigration complete!")
