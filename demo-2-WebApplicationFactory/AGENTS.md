# SUMMARY
In this demo we have a lift API that calls a downstream weather API. What we want to show is how WebApplicationFactory, TestContainers and WireMock can help us simulate behavior from the downstream service.

https://api.open-meteo.com/v1/forecast?latitude=62.4083&longitude=13.8612&current=temperature_2m

## Covered concepts are
* Creating different scenarios of the downstream service using wiremock hosted by testcontainers. 

## Documentation
IMPORTANT! Always prefer official documentation when available.
https://wiremock.org/dotnet/
https://dotnet.testcontainers.org/
https://learn.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-10.0&pivots=xunit
