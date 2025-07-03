# avx.voxbridge.console
Console test and example projects for internal and external use.

## Running the console application

1. Install [\.NET 8](https://dotnet.microsoft.com/).
2. Edit `SignalRConsoleClient/appsettings.json` to provide your tenant credentials and the URLs for each environment under `Environments`.
3. Set the environment via the `ENV` variable (`local`, `test` or `prod`). For example:

   ```bash
   ENV=test dotnet run --project SignalRConsoleClient
   ```

   If `ENV` is not set, the application will prompt you to pick one when it starts.
4. Start the app with `dotnet run --project SignalRConsoleClient`.
