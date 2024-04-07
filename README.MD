### SETUP User Secret
dotnet user-secrets init

### SETUP ASPNet Scaffolding
dotnet tool install -g dotnet-aspnet-codegeneratordpt

### Set Connection String as User Secret
dotnet user-secrets set ConnectionStrings:PizzaPlaceDB "Server=127.0.0.1;Database=PizzaPlaceDB;Uid=root;Pwd=<your-rootPassword>;"

### Regenerate Models by EF Core
dotnet ef dbcontext scaffold "Name=ConnectionStrings:PizzaPlaceDB" MySql.EntityFrameworkCore

### Scaffold Controllers from EF Core Model and Context
dotnet aspnet-codegenerator controller -name <your-nameOfController> -async -api -m <your-nameOfModel> -dc PizzaPlaceDbContext -outDir Controllers