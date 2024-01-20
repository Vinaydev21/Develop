# GuestAPI
The Guest API is a developed in .NET Core Web API &amp; IN Memory EF. This API facilitates Managing Guest Information,Get Guest Details, Add Phone Number.
I have used serilog to implement logging and logs are displayed in console while running the api.
Integrated it with swagger so you can execute all endpoints in swagger.
I have used Authentication to secure api. 

"Key": "GUESTKKx3T7TRUg2mLt8AmmkmjpNQrqoDxJi8fvCRP8PDjGa0jmGzsQDJ4rGFrcIdVbzcOIfwBBplv0KDFl6nntLZFl7IGOBTUyZVJFYK1bwWxw74Wh4bk5xqgXGw1CL"

You can use this key in header and pass it on. You can also implement or change Key as per your requirements.



Prerequisites
NET Core SDK
Entity Framework Core Tools

How to run the project 
Steps
Clone the Repository: git clone https://github.com/your-username/guest-api.git
cd guest-api

Build and Run the API: 
dotnet build
dotnet run --project GuestAPI

API Endpoints:

Add Guest: POST /api/guest
Add Phone: POST /api/guest/{id}/AddPhone
Get Guest by ID: GET /api/guest/{id}
Get All Guests: GET /api/guest

Assumptions Made
During the development of the Guest API, the following assumptions were made:

Database Connection:

The API assumes a valid and configured database connection.
Input Validation:

It is assumed that input data follows the expected format and validation rules.
Challenges Faced and Solutions
Challenge: Database Configuration
Solution: Ensured the correct setup of Entity Framework Core for database interactions.
Challenge: Input Validation
<<<<<<< HEAD
Solution: Implemented input validation checks and error handling to handle invalid inputs gracefully.
=======
Solution: Implemented input validation checks and error handling to handle invalid inputs gracefully.
>>>>>>> 384d3d27ac503d9167968653d6f7e7d3165c57ce
