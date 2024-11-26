# How to work with project and db.

### To run project you need to run next commands:
1. Start db container
    - In folder _\PetPortalBack\PetPortalAPI\PetPortalAPI_ run _docker-compose up -d_
   
2. Update database with migrations which you have
    - In folder _\PetPortalBack_ run _dotnet ef database update -s .\PetPortalAPI\PetPortalAPI\ -p .\PetPortalDAL\\_
  

### To create new migration you need to run next command:

- In folder _s\PetPortalBack_ run _dotnet ef migrations add migrationName -s .\PetPortalAPI\PetPortalAPI\ -p .\PetPortalDAL\_ 
