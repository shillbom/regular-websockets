# regular-websocktes
Service and app extensions for creating a websocket service

# Install
use nuget: [https://www.nuget.org/packages/RegularWebsockets/1.0.0](https://www.nuget.org/packages/RegularWebsockets/1.0.0)

# Typical usage

The pattern used is configuring the service and then using the service in your controllers / services
```
// Startup.cs
public void ConfigureServices(IServiceCollection services)
{
    // ... configure your services
    
    // Configure WebSockets, this makes sure that the service is injected when needed
    services.ConfigureWebSockets();
}

public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
{
    // ... configure your app
  
    // Use the socket service
    app.UseSocketService();
}
```c#
Then use the service in your services / controllers
```
// MyService.cs
public MyService(ISocketService socketService)
{
    this.socketService = socketService;
    this.socketService.onOpen += newUserConnected;
}

public async void newUserConnected(object sender, OpenEvent e)
{
    await e.socket.SendAsync("hello new user!");
}
```c#
