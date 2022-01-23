# PhoneBook

**Technologies Used:**

This project is build using Razor pages with .NET Core 3.1 while using a SQL database.

How to run the project
To re-create the database run the script located under the Database/DatabaseSchema.txt. This will created the database along with the tables and the store procedures needed.
To change the connection string go to the appsettings.json file and add the connectionstring under the SQLConnection attribute.
The EnviromentEndpoint will also need to be corrected if not the same. This can be done by replacing the EnvironmentEndPoint attribute in the appsettings.json file.
Once the database and setting are in order the program can be run by pressing F5 in visual studio.

**Demo Video**
https://we.tl/t-rRYnoix3k3

What is the software about?
The software allows a user to create multiple phone books and add multiple contacts to the phone book of their choice.
The user can then select a book which they want to view and the entries in the phone book will display in a list form. 
The user can search for an entry, the search takes place across all phone books and not just the selected phone book. 

**Folder break down **
1. The styling of the website is pulled from the site.css file under wwwroot/css.

2. In the Controllers folder you will find 2 controllers one for each entity namely Entry and PhoneBooks.
  The Entry.cs controller is where the APIs for the Entry entity live. There is 3 API endpoints for the Entry entity. 
  The endpoints for Entry are as follow:
   - GetEntry
   - GetAllEntry
   - InsertEntry

   The PhoneBook.cs controller is where the APIs for the PhoneBook entity live. There is 2 API endpoints for the PhoneBook entity. 
   The endpoints for PhoneBook are as follow:
   - GetPhoneBooks
   - InsertPhoneBook
   
 3. The database folder contains scrips for creating the database schema as well as the scripts for all the store procedures needed.
 
 4. The DemoVideo folder contains a demo video which shows all the use cases of inserting and searching entries. 
 
 5. The Models folder contains the Models for all the objects needed. 
 
 6. The pages Folder is where all the pages and shared pages is located along with the .cs for each page.
 
 7. The DAL file is where all the methods live which executes the SQL commands. 
 
 8. Other files to note gitarrributes, gitignore, appsettings, Program and Startup 
 
