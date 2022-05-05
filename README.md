# AlbelliAPI

Hi! The project gets data from AlbelliAPI and stores in database.
To execute project 
- Install docker(if you don't have)
- Go to path
- Execute the command `docker-compose up`. It may take a time during the first init.
- Request to api/populate_data for the populate producttypes

When project runes, you can reach 
- Swagger : http://localhost:5100/swagger
- Kibana : http://localhost:5601
- ElasticSearch: http://localhost:9200/

There are 3 Actions for the project
- [HTTPPOST] api SubmitOrder : Use this for the submit order
- [HTTPGET] api/{order_id} : Use this for the retrieve order 
- [HTTPGET] api/populate_data : Use this for the populate producttypes

# Project Structure and Folders
- src
	- Albelli.API 
		- Installers : Install Mongo and Redis with an extension
			- MongoInstaller : Setups MongoDB
			- RedisInstaller : Setups Redis
		- Middlewares
			- ExceptionHandlingMiddleware : Catches the exception and sets HTTP status code by the exception type, and logs errors
    - Albelli.Core
	    - Handlers : Includes Command and Query Handlers
		    - QueryHandlers
			    - GetOrderQueryHandler : Handle getorderquery and returns the related data
            - CommandHandlers
                - SubmitOrderCommandHandler : Handle SubmitOrderCommand and insert to database
		- Extensions 
			- CacheExtensions : Include GetAsync and SetAsync methods. It is a extension method for IDistributedCache interface
		- Models
			- Exceptions
				- OrderDetailNotFoundException : If there is no record in Order collection GetOrderQueryHandler  throws the exception.
				- OrderNotFoundException : If there is no record in Order collection GetOrderQueryHandler  throws the exception
                - ProductTypeNotFoundException : If there is no record in ProductType collection GetOrderQueryHandler and SubmitOrder  throw the exception
		- PipelineBehaviors :
			- PerformanceBehavior: Starts a timer when the related handler is called and closes the timer after the process is finished. If the time gap is greater than 500 milliseconds logs the request.
			-  ValidationBehavior: Executes before handlers after PerformanceBehavior by per request. Throws an exception if the requested model is not valid.
		- Services : 
			- IProductTypeService : Includes one method PopulateProductTypesAsync.
			- ProductTypeService : Impelemts IMakelaarService interface

# Notes
Execute Functional and Unit tests in an orderly. Because mongo container and mongo2go packages use the same port and it causes the exception due to port sharing problem.
# Tech Stack and Frameworks
 - .NET 6
 - MongoDb
 - Redis
 - ElasticSearch
 - Kibana
 - MediatR
 - Mongo2Go
 - AutoFixture
 - DotNet.Testcontainers
 - Serilog
 

