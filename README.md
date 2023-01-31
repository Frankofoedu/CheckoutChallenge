# CheckoutChallenge: Payment Gateway API
Build , an API based application that will allow a merchant to offer a way for their shoppers to pay for their product.


![api workflow](https://github.com/Frankofoedu/CheckoutChallenge/actions/workflows/dotnet.yml/badge.svg)

### Deliverables
+ Build an API that allows a merchant:
  - To process a payment through your payment gateway.
  - To retrieve details of a previously made payment.
- Build a bank simulator to test your payment gateway API.

## Installation
### Pre-requisites

- .NET Core 6.0 SDK
- Visual studio or VsCode
- Docker

#### Building the project
1. In visual studio right-click on the solution file and select build solution.
2. In VsCode, run the following command  in the root directory to build the project 
```
dotnet restore
dotnet build
```
#### Running the project
1. In visual studio click the run button after selecting the api project as the default project.
2. In vscode, run `dotnet run` from the root directory.
3. If using docker, run the docker compose file `docker-compose up` from the root directory.


You can browse the swagger APIs documentation at https://localhost:7277/swagger

To make request, please set a MerchantId and Idempotent header in the request. e.g

```
curl --location --request POST 'https://localhost:7277/Payments' \
--header 'x-idem-key: 123' \
--header 'MerchantId: 22d477d7-035f-412b-ac15-3b92b8b0f8b2' \
--header 'Content-Type: application/json' \
--data-raw '{
  "currency": "string",
  "amount": 0,
  "description": "string",
  "card": {
    "number": "2022202220222022",
    "cvv": "string",
    "expiryMonth": 1,
    "expiryYear": 1,
    "ownerName": "string"
  }
}'
```
The merchant keys can be found in the appsettings.json file.

### Project Structure

#### Application
The solution is made up of four projects that are loosely based around the repository pattern. 
1. API project which interfaces with the merchants. 
2. Core project. This contains most of the domain specific codes
3. Shared project. Contains reusabe codes that can be shared among the projects.
4. Mock bank: A simple class that mocks the external bank infrastructure.

#### Tests
There are two projects that should hold tests for this solution.
1. Integration tests. This ensures that the components function correctly including API calls and database queries. I wrote a couple of test cases to handle some of the requirement.

2. Unit test: Tests individual components of our solution.  Due to time constraints, I couldnt do much here.

#### Deployment
The project can be deployed to multiple cloud providers  but I will pick AWS or Azure as most engineers are primaarily using either of the two so it will be easier to maintain moving forward. 
1. AWS: The application could be deployed to an elastic beanstalk infrastructure, ECS or even a lightsail instance. Secrets like connection keys could be stored AWS KMS.
2. Azure: The application could be deployed to an app service, AKS(using the compose file) or an Azure VPS. Secrets like connection keys could be stored Azure key vault.


### Assumptions/Theories
I made the following assumptions while building this solution

1. Only merchants would be allowed access to the API's which will only be provided using the MerchantId
2. The bank has a fast response time and zero downtime

### Bonus points:

1. Idempotency: The API supports idempotency for safely retrying requests without accidentally performing the same operation twice. 

2. API-KEY Auth: I added an implementation to only allow requests from allowed merchants. The implementation uses a static list of merchant ids which can be changed to a database implementation. The keys are in the appsettings file. 

3. Logging: Serilog library was used to implement a file based logger. This can easily be extended to send logs to a database and slack.

4. Docker support was added to allow running the application in containers.

5. Continous integration was setup using a github action.

6. Metrics: API metrics are generated and can be viewed using prometheu and grafana. Please run the docker compose file in the visualization folder.


##  Improvements

1. Caching: Adding caching to the repository layer. This will allow faster response and relieve the database.
2. Add more robust tests
3. Improve the validation for the models
4. An improv3ed way of storing secrets. Using a keyvault instead of appsettings which keeps it in source control.

    

