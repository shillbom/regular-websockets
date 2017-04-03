# regular-websocktes
Service and app extensions for creating a websocket service

# Install
use nuget: [https://www.nuget.org/packages/RegularWebsockets/1.0.0](https://www.nuget.org/packages/RegularWebsockets/1.0.0)

# Typical usage

Have a look at the included sample!

The pattern used is configuring the service and then using the service in your controllers / services.

```cs
// Startup.cs
public void ConfigureServices(IServiceCollection services)
{
    // ... configure your services
    
    // Configure WebSockets, this makes sure that the service is injected when needed
    services.ConfigureWebSockets();
    
    // Configure some class that will handle the connections
    services.AddSingleTion(typeof(MyHandler))
    
}

public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
{
    // ... configure your app
  
    // Use the socket service
    app.UseRegularWebsockets();
}
```
Then use the service in your services / controllers

```cs
// MyHandler.cs
[RegularWebsockets("/greet")]
public class MyHandler : ISocketService
{
  public void OnOpen(OpenEvent ev)
  {
      ev.Socket.SendAsync("hello!");
  }
  
  // ...
}
```
