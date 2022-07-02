Issue Tracker
------------

<img src="./assets/issue_1.jpg" >

<img src="./assets/kanban_1.jpg" >

## Tech Stack
|   |  |   |   |   | 
|---|---|---|---|---|
|  Frontend | <div align="center"><img src="./assets/react_logo.png" width="100" height="100"></br>React</div> | <div align="center"><img src="./assets/ts_logo.png" width="100" height="100"></br>TypeScript</div>  |  <div align="center"><img src="./assets/mui_logo.png" width="100" height="100"></br>Material UI</div> | <div align="center"><img src="./assets/auth0_logo2.png" width="100" height="100"></br>Auth0</div>  |
| Backend  | <div align="center"><img src="./assets/dotnet_logo.png" width="100" height="100"></br>.NET</div>| <div align="center"><img src="./assets/csharp_logo.png" width="100" height="100"></br>C#</div> | <div align="center"><img src="./assets/ef_logo.png" width="100" height="100"></br>EF Core</div>  | <div align="center"><img src="./assets/x_unit.png" width="100" height="100"></br>xUnit</div>  |
 
## Dependencies
+ SQL Server 2019
+ .NET 6
+ Node.js

## To run locally
1. Backend  (depending on your needs you will have to adjust a connection string to the SQL Server in appsettings.Development.json file)
```
cd ./server/src/IssueTracker
dotnet run
```
2. Frontend
  ```
  cd ./client 
  npm install
  npm start
  ```
3. When logging to the Auth0 use the following credentials: 
> Email address: www.admin@gmail.com </br>
> Password: Admin12#$


## Unit Tests
```
cd ./server/tests/UnitTests/IssueTracker.ApplicationTests 
dotnet test
```
```
cd ./server/tests/UnitTests/IssueTracker.InfrastructureTests
dotnet test
```
## Integration tests (SQL Server required)
```
cd ./server/tests/IntegrationTests/IssueTracker.IntegrationTests
dotnet test
```
 

