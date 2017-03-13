# Web API Action Filters
To keep your Web API lean and focused, you should think about separating concerns and stick to the DRY principle. This NuGet package contains several reusable action filters that can be used in `ApiController`.

### Installation
Run the following command from the [Package Manager Console](https://docs.nuget.org/ndocs/tools/package-manager-console)

```Install-Package Heine.Mvc.ActionFilters```

or search for the package inside the Nuget Package Manager in Visual Studio.

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
* `ProcessHttpStatusException` will catch any exception of the type `HttpStatusException` (included in the package) and turn it in to the appropriate reponse message in the controller. E.g., if a `BadRequestException` is thrown in the code excecuted by the controller, it will be handled by the filter and converted into an `HttpResponseMessage` with status code 400 and message equal to the exception message. Similarily, `NotFoundException` will result in a 404 response.
* `LogException` will log an error message and return `500 Internal Server Error` in if an unhandled exception reaches the controller.

### Services
If you want more detailed exception logs, you can register the `WebApiExceptionLogger` service:
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
The difference between the `LogException` filter and the `WebApiExceptionLogger` service is that the service will also log a prettyfied version of the incoming request body, which can be useful for debugging purposes. To prevent bloating the logs, you should consider limiting the amount of characters that the service is allowed to log from the request body. To do that, simply set the `RequestBodyMaxLogLength` property:

```csharp
new WebApiExceptionLogger
{
  RequestBodyMaxLogLength = 1000
}
```
The default is 10 000.
## Dependencies
The package depends on [NLog](http://nlog-project.org/) to handle the logging. That means that you need to have a file named `NLog.config` inside your project (or a project you are referencing) that configures target, paramaters, minimum log level etc.
It also depends on [Newtonsoft.Json](http://www.newtonsoft.com/json) to prettify the request body.
