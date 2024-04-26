# ZenBank

An API with multiple sections used to handle users and their transactions, and ultimately send a report for each user about their daily transactions. There are two parts for this API called **ZenCore** and **ZenReporting**.
- ZenCore has two controllers that handle basic CRUD operations for users and transactions
- ZenCore also has a background service that runs every 5 minutes, where it sums all the daily transactions of a user and sends it to ZenReporting for further proccessing
- Once the data gets sent to ZenReporting, it is made into a PDF that gets sent to the user via email
- The user can also make this request manually instead of waiting for the service to do it

## How to run:

Pull the repository from GitHub and run the docker compose file located in the root folder with `docker-compose up`.
Once the docker container is up and running, you can navigate to multiple pages:
- http://localhost:8001/swagger/index.html to get to the Swagger page for the **ZenCore** project
- http://localhost:8002/swagger/index.html to get to the Swagger page for the **ZenReporting** project
- http://localhost:8025/ to get to the **MailHog** page, for email testing

## Project Structure

- The ZenCore service uses controllers for basic CRUD operations for both users and transactions, where one user can have multiple transactions associated to him
- There is also the ReportingJob class that is a cron job based on the Quartz scheduling system, which runs every 5 minutes by default settings, which can be changed via docker
- The project also has unit tests for the ZenCore controllers and cron job
