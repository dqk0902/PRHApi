## PRHApiAssigment: 

## Prerequisites

Make sure you have the following software installed on your machine:

* .NET 6.0 SDK
* PostgreSQL database
* Git (optional, if you want to clone the project from GitHub)

## Installation
To install the project, follow these steps: 


git clone https://github.com/dqk0902/PRHApi.git

Run the following commands to install the project dependencies: dotnet restore

Create a PostgreSQL database and update the connection string in the appsettings.json file with your database details.

Finally, run the project using the command: dotnet run

If you want to fetch the data from the database, go to bash and use this command:
curl -k https://localhost:7064/postal_codes/{postal_code}/companies