# To allow Windows to execute .ps1 files, 
# In powerShell execute below once:
# Set-ExecutionPolicy -ExecutionPolicy RemoteSigned -Scope CurrentUser

# To execute in powershell:
#.\database-rebuild-all.ps1 seed

# Drop the database
dotnet ef database drop -f -c SqlServerDbContext -p ../DbContext -s ../DbContext

# Remove any migration
Remove-Item -Recurse -Force ../DbContext/Migrations

# Create a full new migration
dotnet ef migrations add miInitial -c SqlServerDbContext -p ../DbContext -s ../DbContext -o ../DbContext/Migrations/SqlServerDbContext

# Update the database from the migration
dotnet ef database update -c SqlServerDbContext -p ../DbContext -s ../DbContext

# Check for 'seed' argument and seed the database if present
if ($args[0] -eq "seed") {
    # Seed the database
    Set-Location ../AppSeeder
    dotnet run
}
