#!/bin/bash
#To make the .sh file executable
#sudo chmod +x ./database-rebuild-all.sh

# To execute:
# ./database-rebuild-all.sh seed

#drop the database
dotnet ef database drop -f -c SqlServerDbContext -p ../DbContext -s ../DbContext

#remove any migration
rm -rf ../DbContext/Migrations

#make a full new migration
dotnet ef migrations add miInitial -c SqlServerDbContext -p ../DbContext -s ../DbContext -o ../DbContext/Migrations/SqlServerDbContext

#update the database from the migration
dotnet ef database update -c SqlServerDbContext -p ../DbContext -s ../DbContext

# Check for 'seed' argument and seed the database if present
if [[ $1 == "seed" ]]; then
    #seed the database
    cd ../AppSeeder
    dotnet run
fi
