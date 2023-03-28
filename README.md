## PRHApiAssigment: 

-I create PRHapiPullData.cs to pull the data and save it into the local database(PostgreSQL) based on postalcode.

-I create the Company Models and MyDbContext so it can handle more request(if exist)

-After that, I create PRHapiController.cs to handle Get request.

-I add the connection string to my local database into appsettings.Development.json and Program.cs so the Program can read it.


How it work: 

First, to pull data to the local database, fill out the string in this line in Program.cs to save it into the database 

For example:
var companies = await apiClient.GetCompaniesByPostalCode("00700", connectionString); and then type dotnet run.

Note: The GetCompaniesByPostalCode method is inside the PRHapiPullData.cs.

Second, to get an array of companies registered to that postal code, go to bash and type this:
curl -v -k https://localhost:7064/postal_codes/01400/companies | jq.