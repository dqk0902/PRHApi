First, to pull data to the local database, fill out the string in this line in Program.cs to save it into the database 
var companies = await apiClient.GetCompaniesByPostalCode("00700", connectionString); and then type dotnet run.
The GetCompaniesByPostalCode method is inside the PRHapiPullData.cs

Second, to get an array of companies registered to that postal code, go to bash and type this:
curl -k https://localhost:7064/{postalCode}/companies | jq.