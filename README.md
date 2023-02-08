
![Nuget](https://img.shields.io/nuget/vpre/CodeArt.MatomoTracking)


# CodeArt.MatomoTracking
Opensource Matomo Tracking API for .NET. 
This library is based on the official Matomo Tracking API documentation: [https://developer.matomo.org/api-reference/tracking-api](https://developer.matomo.org/api-reference/tracking-api) and wraps the API calls in a simple to use .NET library.
Initial version made by Allan Thraen for [CodeArt](https://www.codeart.dk/).

## Installation
Register the Matomo Tracking as a service in Dependency Injection. 

```csharp
        var services = new ServiceCollection();
        
        //Needs IConfiguration - but this might already be registered in your project
        services.AddScoped<IConfiguration>(_ => configuration);
		
        services.AddMatomoTracking(options =>
        {
            options.MatomoUrl = "https://[My matomo hostname]/";
            options.SiteId = "[My site id]";
        });
        var serviceProvider = services.BuildServiceProvider();
```

## Configuration
If you have a configuration file, you can also set the configuration directly there under the "Matomo" node.

```json
{
  "Matomo": {
	"MatomoHostname": "[My matomo hostname]",
	"SiteId": "[My site id]"
  }
}
```

## Usage

You can track many different things, for example: 
- Pageviews using PageViewTrackingItems
- Events using EventTrackingItems
- Ecommerce using EcommerceTrackingItems
- Content using ContentTrackingItems

For the official documentation of what to track - and when, refer to the (official documentation)[https://developer.matomo.org/api-reference/tracking-api]

### Tracking a page view
```csharp

    await tracker.Track(new PageViewTrackingItem()
    {
        Url = "app://myapp/page1",
        UserID = "test@test.com",
        NewVisit = true,
        ActionName = "Page 1",
        PageViewID = pvid
    });
```

### Tracking in bulk
You can also track multiple items in the same call like this:

```csharp
    await tracker.Track(new PageViewTrackingItem()
    {
        Url = "app://myapp/page1",
        UserID = "test@test.com",
        NewVisit = true,
        ActionName = "Page 1",
        PageViewID = pvid
    },
    new EventTrackingItem()
    {
        Url = "app://myapp/page1",
        UserID = "test", 
        NewVisit = false,
        ActionName = "Form submit",
        PageViewID = pvid,
        EventCategory = "Form",
        EventAction = "Submit",
        EventName = "Newsletter form"
    });
```
