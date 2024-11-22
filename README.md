# How to test ASB emulator

1. From the top directory, build and run image with `docker compose -f docker-compose.yml up -d`
1. Verify that `servicebus-emulator:latest` started with `docker ps -a`
1. Go to `src/consumer`, build and run the app `dotnet restore`, `dotnet build` and `dotnet run`
1. Go to `src/producer` and also restore and build the app. 
1. Notice that both producer and consumer apps have the same connection string hardcoded and the same queue name, which corresponds to a setting in `Config.json` file in the root directory of this example.
1. Run the producer and notice that consumer printed the message.