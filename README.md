# Weerly WebSocket Wrapper

# Real-Time WebSocket Wrapper for ASP.NET Core Applications

## Overview
Weerly WebSocket Wrapper is a lightweight .NET 6 library designed to simplify WebSocket routing and connection handling in ASP.NET Core applications. This library provides a structured way to define WebSocket routes and manage WebSocket communication efficiently.

This project provides a **WebSocket wrapper** for simplifying WebSocket communication and routing in .NET applications. It includes a flexible API for mapping WebSocket routes to handlers with both synchronous and asynchronous processing capabilities.

## Features
- Easy integration with ASP.NET Core applications
- Fluent API for defining WebSocket routes
- Supports different WebSocket processing strategies
- Compatible with .NET Core starting from version standard 2.0
- **Dynamic WebSocket Route Mapping**: Quickly define WebSocket routes with `MapWsRoute` and `MapWsRouteAsync`.
- **Asynchronous and Synchronous Handling**: Support both long-running operations and simple message processing.
- **Custom Routing Logic**: Routes can point to handler methods in specific classes or controllers.
- **Real-Time Communication**: Deliver progressive updates using `WebSocketCallback`.

## Installation
To install Weerly WebSocket Wrapper, add the package to your project:

```sh
Install-Package Weerlyy.WebSocketWrapper
```

---

## Usage

To start using the WebSocket wrapper, configure routes using `app.UseWebSocketRoutes` by specifying the mapping method (`MapWsRoute` or `MapWsRouteAsync`).

```c#
also don`t forget to add app.UseWebSockets(); in your startup.cs or program.cs depending on your .NET version.
```
### Example: Mapping WebSocket Routes

```c#
app.UseWebSocketRoutes(routes =>
{
    // Asynchronous WebSocket route
    routes.MapWsRouteAsync(
        name: "default",
        template: "Test/About",
        type: WebSocketEnums.CommonType.Class,
        classNamespace: "WebApp1.Models.TestClass"
    );

    // Synchronous WebSocket route
    routes.MapWsRoute(
        name: "sync",
        template: "Sync/Example",
        type: WebSocketEnums.CommonType.Class,
        classNamespace: "WebApp1.Models.TestClass"
    );
});
```

---

## WebSocket Handlers

### 1. `MapWsRouteAsync` Handler

Asynchronous handlers are used for `MapWsRouteAsync`. They must have:
- **Two arguments**:
   1. `string income`: The incoming WebSocket message.
   2. `WebSocketCallback callback`: A callback used to send updates back to the client.
- **Return type**: `Task` (async functions).

#### Handler Example for `MapWsRouteAsync`

```c#
namespace WebApp1.Models.TestClass;

public class Test
{
    public async Task About(string income, WebSocketCallback callback)
    {
        Console.WriteLine("About method started...");

        // Example: Serialize and process input
        var serializedInput = JsonConvert.SerializeObject(income);
        var formattedInput = JsonConvert.SerializeObject(
            JsonConvert.DeserializeObject<object>(serializedInput), Formatting.None);

        // Send updates to WebSocket client
        for (var i = 0; i < 1000000; i++)
        {
            if (i > 0 && i % 1000 == 0)
            {
                await callback($"{i}"); // Send periodic updates
            }
        }

        Console.WriteLine("About method completed.");
    }
}
```

#### Explanation:
- The handler processes the `income` message asynchronously.
- Sends progress updates back to the WebSocket client during long-running operations using `callback`.

---

### 2. `MapWsRoute` Handler

Synchronous handlers are used for `MapWsRoute`. They must have:
- **One argument**:
   - `string income`: The incoming WebSocket message.
- **Return type**: A single `string` response.

#### Handler Example for `MapWsRoute`

```c#
namespace WebApp1.Models.TestClass;

public class Test
{
    public string About(string income)
    {
        Console.WriteLine("Processing About request...");

        // Example: Processing input and returning result
        for (var i = 0; i < 1000000; i++)
        {
            if (i > 0 && i % 1000 == 0)
            {
                return $"{i}"; // Return first result divisible by 1000
            }
        }

        return "Processing completed.";
    }
}
```

#### Explanation:
- The handler processes the `income` message synchronously and returns a single response.
- No `callback` is used since synchronous handlers are one-time, non-progressive operations.

---

## Key Differences Between `MapWsRouteAsync` and `MapWsRoute`

| Feature                     | `MapWsRouteAsync`                                                    | `MapWsRoute`                      |
|-----------------------------|----------------------------------------------------------------------|------------------------------------|
| **Arguments**               | `string income, WebSocketCallback callback`                         | `string income`                   |
| **Response Type**           | Real-time updates via `callback`                                    | Single response `string`          |
| **Use-Case**                | Long-running or interactive processing                              | Simple one-time processing         |

---

## Full Usage Example

```c#
app.UseWebSocketRoutes(routes =>
{
    // Asynchronous WebSocket route example
    routes.MapWsRouteAsync(
        name: "analytics",
        template: "Analytics/Data",
        type: WebSocketEnums.CommonType.Class,
        classNamespace: "WebApp1.Models.TestClass"
    );

    // Synchronous WebSocket route example
    routes.MapWsRoute(
        name: "sync",
        template: "Sync/Example",
        type: WebSocketEnums.CommonType.Class,
        classNamespace: "WebApp1.Models.TestClass"
    );
});
```

Handlers for the routes:
- Asynchronous route: Adds progressive updates for long-running WebSocket operations (`Test.About` with `callback`).
- Synchronous route: Adds simple, single-response logic for WebSocket operation (`Test.About`).

---

## Error Handling

1. **Invalid Namespace**:
   - Ensure the specified `classNamespace` or handler class is correctly provided and exists at runtime.

2. **Callback Misuse** (`MapWsRouteAsync`):
   - Ensure progressive updates in the async handler use `await callback()` properly.

3. **Wrong Method Signature**:
   - Ensure handler method signatures match the route type (`Async` or `Non-Async`) and required arguments.

---

## Supported Route Templates

The WebSocket wrapper supports dynamic route configurations with the following templates:
- **Class or Controller Templates**: Example: `"Controller/Action"` or `"Class/Method"`.
- **Custom Defined Routes**: Tailor namespace routing to any required structure.

---

## License

This WebSocket wrapper is licensed under the **MIT License**.

## Author
Developed by Mykhailo Chumak.
