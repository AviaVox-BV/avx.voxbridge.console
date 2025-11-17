# avx.voxbridge.console

This is a small but powerful tool that helps you verify your setup, test live SignalR communication, and understand how the VoxBRIDGE API works in practice.  
Use this client to confirm that your credentials, configuration, and infrastructure are correct before integrating VoxBRIDGE into your own systems.

## Running the console application

1. Install [\.NET 8](https://dotnet.microsoft.com/).
2. Edit `SignalRConsoleClient/appsettings.json` to provide your tenant credentials and the URLs for each environment under `Environments`.
3. Set the environment via the `ENV` variable (`local`, `test` or `prod`). For example:

   ```bash
   ENV=test dotnet run --project SignalRConsoleClient
   ```

   If `ENV` is not set, the application will prompt you to pick one when it starts.

4. Start the app with `dotnet run --project SignalRConsoleClient`.

## Introduction

The VoxBRIDGE Console application is a lightweight **SignalR test client** designed to help developers and integration partners understand, verify, and experiment with the VoxBRIDGE API in a practical way.

It provides a simple and transparent way to:

- **Test and validate connectivity** — confirm that your network, credentials, and endpoints are correctly configured.
- **Verify the infrastructure** — ensure SignalR communication works within your environment (firewalls, proxies, TLS).
- **Observe live communication** — view real-time updates as they are pushed from the VoxBRIDGE service.
- **Understand integration flow** — explore the complete OAuth2 authentication process and SignalR subscription model.
- **Re-use example code** — leverage the provided C# source as a foundation for your own client integrations.

In short, this console client acts as both a **reference implementation** and a **diagnostic tool**.  
It demonstrates how the API behaves in production, while giving developers full insight into the connection lifecycle, authentication, and data streaming model.

### 1. Prepare your configuration

AviaVox will provide you with a small set of confidential settings, including:

- OAuth2 client credentials
- The location ID(s) relevant to your integration
- The correct VoxBRIDGE API and SignalR endpoints

These values must be inserted into the included `appsettings.json` file.  
This file is read automatically when the application starts.

### 2. Run the application

Launch the executable:

```
./SignalRConsoleClient
```

or on Windows:

```
SignalRConsoleClient.exe
```

The application will:

1. Authenticate via OAuth2
2. Establish a SignalR connection
3. Subscribe to flights and/or announcements
4. Display all incoming messages in real time

### 3. What you will see

When everything is configured correctly, the console will output:

- **Successful authentication** (token acquired)
- **SignalR connection established**
- **Subscription active**
- **Live incoming messages**, such as:
  - Flight updates
  - Announcement updates
  - Health check

This provides a clear, end-to-end demonstration that your environment is correctly configured.

### 4. Common usage scenarios

The console client is ideal for:

- **Initial onboarding**  
  Confirm that all credentials and endpoints work as expected.

- **Network validation**  
  Quickly identify firewall or proxy issues that block WebSocket traffic.

- **Development support**  
  Observe real-time data to help build your own integration.

- **Debugging**  
  When something in your application doesn’t behave as expected,  
  the console client helps determine whether the issue is on your side or upstream.

### 5. No coding required — but source code included

The executable runs out-of-the-box, but the repository also includes full C# source code.  
Developers are free to reuse or adapt the SignalR and authentication implementations in their own applications.
