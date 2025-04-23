#!/bin/bash
#To make the .sh file executable
#sudo chmod +x ./database-rebuild-all.sh

# To execute:
#./database-rebuild-all.sh SQLServer seed
#./database-rebuild-all.sh MariaDb seed
#./database-rebuild-all.sh Postgres seed

#Set Database Context
if [[ $1 == "SQLServer" ]]; then
    DBContext="SqlServerDbContext"

elif [[ $1 == "MariaDb" ]]; then
    DBContext="MySqlDbContext"

elif [[ $1 == "Postgres" ]]; then
    DBContext="PostgresDbContext"

else
    echo No database specified
    exit 1;
fi

#drop the database
dotnet ef database drop -f -c $DBContext -p ../DbContext -s ../DbContext

#remove any migration
rm -rf ../DbContext/Migrations/$DBContext

#make a full new migration
dotnet ef migrations add miInitial -c $DBContext -p ../DbContext -s ../DbContext -o ../DbContext/Migrations/$DBContext


#update the database from the migration
dotnet ef database update -c $DBContext -p ../DbContext -s ../DbContext

# Check for 'seed' argument and seed the database if present
if [[ $2 == "seed" ]]; then
    #seed the database
    cd ../AppSeeder
    dotnet run
fi

