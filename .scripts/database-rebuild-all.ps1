# To allow Windows to execute .ps1 files, 
# In powerShell execute below once:
# Set-ExecutionPolicy -ExecutionPolicy RemoteSigned -Scope CurrentUser

# To execute in powershell:
#./database-rebuild-all.ps1 SQLServer seed
#./database-rebuild-all.ps1 MariaDb seed
#./database-rebuild-all.ps1 Postgres seed

# Accept the script argument
param(
    [string]$Database = $args[0]
)

# Set Database Context
switch ($Database) {
    "SQLServer" {
        $DBContext = "SqlServerDbContext"
    }
    "MariaDb" {
        $DBContext = "MySqlDbContext"
    }
    "Postgres" {
        $DBContext = "PostgresDbContext"
    }
    Default {
        Write-Host "No database specified"
        exit 1
    }
}

# Drop the database
dotnet ef database drop -f -c $DBContext -p ../DbContext -s ../DbContext

# Remove any migration
Remove-Item -Recurse -Force ../DbContext/Migrations/$DBContext

# Create a full new migration
dotnet ef migrations add miInitial -c $DBContext -p ../DbContext -s ../DbContext -o ../DbContext/Migrations/$DBContext

# Update the database from the migration
dotnet ef database update -c $DBContext -p ../DbContext -s ../DbContext

# Check for 'seed' argument and seed the database if present
if ($args[0] -eq "seed") {
    # Seed the database
    Set-Location ../AppSeeder
    dotnet run
}
