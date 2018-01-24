# Web API Action Filters
To keep your Web API lean and focused, you should think about separating concerns and stick to the DRY principle. This NuGet package contains several reusable action filters that can be used in `ApiController`.

### Installation
Run the following command from the [Package Manager Console](https://docs.nuget.org/ndocs/tools/package-manager-console)

```Install-Package Heine.Mvc.ActionFilters```

or search for the package inside the Nuget Package Manager in Visual Studio.

## Action Filters

### Registering action filters
There are several ways to register a Web API filter. The following code snippets shows three ways that you can register an action filter depending on your needs.

#### By action
```csharp
public class ContactsController : ApiController
{
    [HttpGet]
    [ValidateModel]
    public Contact GetContact(Guid id)
    {
      // ...
    }
}
```
This will apply only to the Web API action that is decorated with the attribute.

#### By controller
```csharp
[ValidateModel]
public class ContactsController : ApiController
{
    // ...
}
```
This will apply to all of the Web API actions inside the controller.

#### Globally
```csharp
using Heine.Mvc.ActionFilters;

public static class WebApiConfig
{
    public static void Register(HttpConfiguration config)
    {
      config.Filters.Add(new ValidateModelAttribute());
    
      // Other configuration code ...
    }
}
```
This will apply to all Web API controller acitons in the entire project.

### Action filter attributes
* `ValidateModel` will validate the `ModelState` and return a `400 Bad Request` in case it is not valid.
* `ReportObsoleteUsage` will log a warning message every time a method that is decorated with this attribute is used.
* `LogException` will log an error message and return `500 Internal Server Error` in if an action triggers an exception.
* `NotFoundException` will return `404 Not Found` if an exception of the type `NotFoundException` (included in the package) is thrown.
* `LogClientErrors` will log the request and response if a response has status code `400-499`.
  * The message will be logged with level `Warning`.
* `LogAllTraffic` will log all requests and responses registered on the API.
  * HTTP status code `200-299` will be logged with level `Debug`, `300-499` will be logged with level `Warning` and `>=500` will be logged as `Error`.

## Services
This package includes a service called `WebApiExceptionLogger`. The purpose of the service is, as the name implies, to log all unhandled exceptions that may occur in your API. Example of service registration is given below.

```csharp
using Heine.Mvc.ActionFilters;

public static class WebApiConfig
{
    public static void Register(HttpConfiguration config)
    {
      config.Services.Add(typeof(IExceptionLogger), new WebApiExceptionLogger());
    
      // Other configuration code ...
    }
}
```

## Validation Attributes
Validation attributes are used for model state validation.

```csharp
public class Match
{
    [GuidNotEmpty]
    public Guid Id { get; set; }
}
```

In the model above, we have applied the `GuidNotEmpty` attribute. It the `Id` is set to `Guid.Empty` (00000000-0000-0000-0000-000000000000), model state validation will fail and return a descriptive error message.

```csharp
public class Match
{
    [GuidNotEmpty]
    public Guid? Id { get; set; }
}
```

However, if the property is nullable and set to null, validation will succeed. This is intentional.

## Dependencies
The package depends on [NLog](http://nlog-project.org/) to handle the logging. That means that you need to have a file named `NLog.config` inside your project (or a project you are referencing) that configures target, paramaters, minimum log level etc.
