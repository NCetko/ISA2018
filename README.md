Tehnologija: ASP NET Core MVC

Setup:  
-Instalirati SQL SERVER EXPRESS 2017  
-Instalirati CORE SDK 2.3:   
https://www.microsoft.com/net/download/thank-you/dotnet-sdk-2.2.100-preview3-windows-x64-installer  
-Korisiti najnoviji Visual studio, najnoviji update (proveriti updates putem Visual studio installer)

Migracija:  
Baza je code-first pomoću Entity Framework Core.   
Navigirati cmd do foldera projekta. Ako postoji migrations folder, obrisati ga. Ako postoji ISA baza, obrisati je.
Pre pokretanja aplikacije izvršiti sledeće komande u cmd:

//generiše migrations folder sa komandama za kreiranje baze  
	dotnet ef migrations add InitialCreate  
//generiše bazu  
	dotnet ef database update  
	
Opciono: instalirati SQL Server management studio (SSMS), ili DataGrip.

Potencijalno potrebno:  
U slučaju da projekat neće sam da se restore preko Visual Studio, u cmd u folderu projekta izvšiti: dotnet restore  
Projekat koristi Entity Framework Core sa nuget package manager:   
Install-Package Microsoft.EntityFrameworkCore.SqlServer -Version 2.1.4

**Ne pushovati migrations, bin, .vs, ili obj!**  





