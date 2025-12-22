#!/bin/bash

SQLITE_DB="/Users/bretpehrson/Projects/NEPlumbingInc/NEPlumbingInc/app.db"
MSSQL_SERVER="localhost"
MSSQL_USER="sa"
MSSQL_PASSWORD="YourPassword123!"
MSSQL_DB="NEPlumbingIncDB"

echo "Exporting data from SQLite..."

# Export UndergroundSubmissions
echo "Exporting UndergroundSubmissions..."
sqlite3 "$SQLITE_DB" << 'EOF' > /tmp/undergroundsubmissions.sql
.mode insert UndergroundSubmissions
SELECT * FROM UndergroundSubmissions;
EOF

# Export SpecialOffers
echo "Exporting SpecialOffers..."
sqlite3 "$SQLITE_DB" << 'EOF' > /tmp/specialoffers.sql
.mode insert SpecialOffers
SELECT * FROM SpecialOffers;
EOF

# Export Messages
echo "Exporting Messages..."
sqlite3 "$SQLITE_DB" << 'EOF' > /tmp/messages.sql
.mode insert Messages
SELECT * FROM Messages;
EOF

# Export Services
echo "Exporting Services..."
sqlite3 "$SQLITE_DB" << 'EOF' > /tmp/services.sql
.mode insert Services
SELECT * FROM Services;
EOF

# Export SubServices
echo "Exporting SubServices..."
sqlite3 "$SQLITE_DB" << 'EOF' > /tmp/subservices.sql
.mode insert SubServices
SELECT * FROM SubServices;
EOF

echo "✓ Export complete"
echo ""
echo "Files created in /tmp/:"
ls -lh /tmp/*.sql
