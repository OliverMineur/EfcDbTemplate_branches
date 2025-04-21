#!/bin/bash
#To make the .sh file executable
#sudo chmod +x ./database-rebuild-all.sh

# ./database-rebuild-all.sh [seed]

#drop any database
#NOTE - SQL on Azure do NOT drop the database to prevent extra charging
#Instead drop the tables, views, stored procedures, schemas etc
dotnet ef database drop -f -c SqlServerDbContext -p ../DbContext -s ../DbContext

#remove any migration
rm -rf ../DbContext/Migrations

#make a full new migration
dotnet ef migrations add miInitial -c SqlServerDbContext -p ../DbContext -s ../DbContext -o ../DbContext/Migrations/SqlServerDbContext

#update the database from the migration
dotnet ef database update -c SqlServerDbContext -p ../DbContext -s ../DbContext

if [[ $1 == "seed" ]]; then
    #seed the database
    cd ../AppSeeder
    dotnet run
fi
