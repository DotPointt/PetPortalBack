# How to work with project and db.

### To run project you need to run next commands:

1. Start db container.
    - In folder _\PetPortalBack\PetPortalAPI\PetPortalAPI_ run
   ```
       docker-compose up -d
   ```
   
2. Update database with migrations which you have.
    - In folder _\PetPortalBack_ run
    ```
       dotnet ef database update -s .\PetPortalAPI\PetPortalAPI\ -p .\PetPortalDAL\
    ```
    
3. Start the project.
    - Open on local host.

### To create new migration you need to run next command:

1. Open folder _\PetPortalBack_
    - Run next command:        
    ```
        dotnet ef migrations add migrationName -s .\PetPortalAPI\PetPortalAPI\ -p .\PetPortalDAL\
    ```
