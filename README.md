# Web Api Action Filters
To keep your Web API lean and focused, you should think about separating concerns and stick to the DRY principle. This NuGet package contains several reusable action filters that can be used in `ApiController`. There are several ways to register a Web API filter. 

### Registering action filters
The following code snippets shows three ways that you can register an action filter depending on your needs.

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
The method `GetContact` will now validate the provided parameter and return a `400 Bad Request` if it is not a valid `Guid`.

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
