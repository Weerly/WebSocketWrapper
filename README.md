# WebSocketWrapper
dot net Core library for using with WebSocket

# Weerly WebSocket Wrapper

## Overview
Weerly WebSocket Wrapper is a lightweight .NET 6 library designed to simplify WebSocket routing and connection handling in ASP.NET Core applications. This library provides a structured way to define WebSocket routes and manage WebSocket communication efficiently.

## Features
- Easy integration with ASP.NET Core applications
- Fluent API for defining WebSocket routes
- Supports different WebSocket processing strategies
- Compatible with .NET 6

## Installation
To install Weerly WebSocket Wrapper, add the package to your project:

```sh
Install-Package Weerlyy.WebSocketWrapper
```

## Usage
To use WebSocket routing in your application, add the following middleware configuration:

```csharp
using Microsoft.AspNetCore.Builder;
using Weerly.WebSocketWrapper.Builder;
using Weerly.WebSocketWrapper.Enums;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseWebSocketRoutes(routes =>
{
    routes.MapWsRoute(
        name: "default",
        template: "Test/About",
        type: WebSocketEnums.CommonType.Class,
        classNamespace: "Models.TestClass"
    );
});

app.Run();
```

## License
This project is licensed under the MIT License. See the LICENSE file for details.

## Author
Developed by Mykhailo Chumak.

---

# Weerly WebSocket Wrapper (UA)

## Огляд
Weerly WebSocket Wrapper — це легка бібліотека для .NET 6, призначена для спрощення маршрутизації та обробки WebSocket-з'єднань у додатках ASP.NET Core. Вона забезпечує структурований спосіб визначення маршрутів WebSocket та ефективного управління з'єднаннями.

## Особливості
- Легка інтеграція з додатками ASP.NET Core
- Зручний API для визначення маршрутів WebSocket
- Підтримка різних стратегій обробки WebSocket
- Сумісність із .NET 6

## Встановлення
Щоб встановити Weerly WebSocket Wrapper, додайте пакет до свого проєкту:

```sh
Install-Package Weerlyy.WebSocketWrapper
```

## Використання
Щоб використовувати маршрутизацію WebSocket у вашому додатку, додайте таку конфігурацію middleware:

```csharp
using Microsoft.AspNetCore.Builder;
using Weerly.WebSocketWrapper.Builder;
using Weerly.WebSocketWrapper.Enums;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseWebSocketRoutes(routes =>
{
    routes.MapWsRoute(
        name: "default",
        template: "Test/About",
        type: WebSocketEnums.CommonType.Class,
        classNamespace: "Models.TestClass"
    );
});

app.Run();
```

## Ліцензія
Цей проєкт ліцензовано за умовами ліцензії MIT. Деталі дивіться у файлі LICENSE.

## Автор
Розроблено Mykhailo Chumak.


