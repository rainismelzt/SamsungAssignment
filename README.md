# SamsungAssignmentBackend

# Pre-requisites

1. Download dotnet SDK from: **https://dotnet.microsoft.com/download/dotnet/8.0** OR via Visual Studio installation

2. Verify the version using **dotnet --version**, it should shows 8.x.x

# Database Setup

3. If there's no SQL Server running locally, download the installer from microsoft site, **https://www.microsoft.com/en-us/sql-server/sql-server-downloads**

4. During setup, enable **Window Login** as the application is using window login to the local database

5. (Optional) Install SSMS from **https://aka.ms/ssms** to verify instance running & do all the queries

6. Install tools to run dotnet-ef commands by running **dotnet tool update --global dotnet-ef**

7. To update the database automatically, run **dotnet ef database update --project ../UserService.Database**, this will automatically run the migration scripts

# Run the app

8. Go to **UserService.Presentation** directory

9. Restore project dependencies by running **dotnet restore**

10. Run the application by running **dotnet run**, and you should see:
      _info: Microsoft.Hosting.Lifetime[14]
            Now listening on: http://localhost:5221_
