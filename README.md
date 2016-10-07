# Web Api Action Filters
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
* `LogException` will log an error message and return `500 Internal Server Error` in if an action triggers an exception.

## Dependencies
The package depends on [NLog](http://nlog-project.org/) to handle the logging. That means that you need to have a file named `NLog.config` inside your project (or a project you are referencing) that configures target, paramaters, minimum log level etc.
