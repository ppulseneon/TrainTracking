#!/bin/bash
set -e

echo "Starting backup restoration"

if [ -f "/tmp/scheduler.backup" ]; then
    echo "Found backup file, restoring database"
    
    pg_restore -U "$POSTGRES_USER" -d "$POSTGRES_DB" -v --clean --if-exists /tmp/scheduler.backup || {
        echo "pg_restore failed or database already contains data"
        pg_restore -U "$POSTGRES_USER" -d "$POSTGRES_DB" -v /tmp/scheduler.backup || echo "Backup restore completed with warnings or database already restored"
    }
    
    echo "Backup restoration process completed"
else
    echo "No backup file found at /tmp/scheduler.backup"
fi
