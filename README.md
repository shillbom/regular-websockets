# regular-websocktes
Service and app extensions for creating a websocket service. Should make it easier to scope endpoints and it handles reading the bytes and creating strings for you.

Send JSON objects as strings and cast them in your handlers to make it easy. Feel free to create an issue if there is something that i should add!

# Install
use nuget: [https://www.nuget.org/packages/RegularWebsockets](https://www.nuget.org/packages/RegularWebsockets)

# Typical usage

Have a look at the included sample!

The pattern used is similar to MVC. Register the service and then create "controllers" that implement the `ISocketService`, Configure and endpoint using `RegularWebsockets` instead of `Route` on the service and of you go!

```cs
// Startup.cs
public void ConfigureServices(IServiceCollection services)
{
    // ... configure your services

    // Configure WebSockets, this makes sure that the service is injected when needed
    services.ConfigureWebSockets();

    // Configure some class that will handle the connections
    // Does not have to be singleton, a manager scopes handlers for each request.
    services.AddSingleton(typeof(MyHandler))

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
using RegularWebsockets.Events;

[RegularWebsockets("/greet")]
public class MyHandler : ISocketService
{
  public void OnOpen(OpenEvent ev)
  {
      // Leverage the underuing socket to reply
      ev.Socket.SendAsync("hello!");
  }

  public void OnOpen(RecieveEvent ev)
  {
      // The client says something
      Console.WrinteLine(ev.Message);
  }

  // ...
}
```
