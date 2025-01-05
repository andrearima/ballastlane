### Databases Dependencies:
	- docker run -d --name ballastlane -e MONGO_INITDB_ROOT_USERNAME=ballastlane -e MONGO_INITDB_ROOT_PASSWORD=ballastlane -p 27017:27017 mongo:latest
	- docker run -d --name sqlserver -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=iwannarock' -e 'MSSQL_PID=Express' -p 1433:1433 mcr.microsoft.com/mssql/server:2019-latest

### The Project 
The main idea is of an API that allows managing contacts.
With that in mind, it is necessary, according to the description, to have an API that controls the Users and that it is possible to authenticate.
With this authentication done, it should be possible through the other API to manipulate the resources.

### Details
Basically there are two Apis, the Users.Api and Contacts.Api

#### Users.Api 
- You can CRUD Users on the UserController
- You can authenticate with AuthenticationController using email and password
- A default user is provided
	-	Email: admin@admin.com
	-	Password: admin
		
#### Contacts.Api
- You can CRUD Contacts on the ContactController, only with Bearer Token
- [`User Story`](https://github.com/andrearima/ballastlane/blob/main/docs/UserStory.md)

### Technical Notes
	- In order to improve reusability the library Ballastlane was created, and it contains shared configurations among other stuffs like filters and simple notifications
	- The authentication is based on JWT Bearer over header request.
	- There is a mechanism to check where the table for users is created or not, and if it isn´t create.
	- The same for the admin default user.


### Notes:
	- I understand the importance of having unit tests all over the classes and projects.
	- But I prefered the aproach of using Integration Tests, that covers mainly the User Story with TDD aproach
	- This is because of time, otherwise would have unit all over =)



### Code Coverage:

	- Blocks:
		- Covered: 875
		- Not Covered: 388
	- Lines:
		- Covered: 668
		- Partially Covered: 7
		- Not Covered: 249
	
	Results:
	- Blocks: 69,28%
	- Lines: 72,29% 
