Technical Interview Exercise v3

	Your task is to develop a simple web application with an API and data layer using .NET C#, ASP.NET MVC, Web API, and a database or data store, while adhering to Clean Architecture
	Your development should be driven by an informal user story that you will create, and which should be included in your presentation.
	The application should allow users to create, read, update, and delete records from the data via the API endpoints. 
	To showcase your ability to work with modern data storage systems you cannot use Entity Framework, Dapper, or Mediator to complete this assignment.

The application should include the following components
	Database: Create a database or other data storage solution with at least one table/object/container to store data for the application and an additional one to store users. 
	API: Develop an ASP.NET Web API with endpoints that allow users to perform CRUD operations on the data. 
		 Additionally, a second API should include endpoints for user creation, user login, and authorized and non-authorized endpoints.
	Data layer: Develop a data access layer that interacts with the data and provides the necessary CRUD operations for the API endpoints.
	Business logic layer: Develop a business logic layer that includes all of the business rules and validation for the application. 
		This layer should be independent of the data layer and the API.
	Unit tests: Write unit tests for all components of the application, including the data access layer,business logic layer, and API endpoints.

Front end (optional): Develop a simple front-end interface to consume the API endpoints. The

Presentation and Code Review
	Once you have completed the exercise, you will be required to present your project to the technical interview panel. 
	After the presentation, the interview panel will conduct a code review of your project. 
		Clean Architecture: Your architecture should adhere to Clean Architecture principles, including separation of concerns and independence of components.
		Test-Driven Development: Your project should follow TDD methodologies and include unit tests for all components.
		Code quality: Your code should be well-organized, readable, and adhere to best practices.
		Functionality: Your application should perform the required CRUD operations and user authentication without errors or bugs.
	User Story: Your user story should drive the development of the application and be included in your presentation.
	Presentation: Your presentation should be clear, concise, and demonstrate a good understanding of the project.
	Scoring
		The maximum score for the exercise is 100 points.