

go to folder of the repo (main folder) . E.g. C:\Work\repos\karthik\ExpenseTracker

Run the command to build the image

docker build -t lsc/expensetrackerapi:1.0 -f .\ExpenseTracker.Web\Dockerfile .

to list down the images in local docker desktop use this command

docker image ls

docker run -p 8080:80 -e ASPNETCORE_ENVIRONMENT="development" --name expensetrackerapi -d lsc/expensetrackerapi:1.0

docker ps