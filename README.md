![Operator banner](https://getoperator.co/assets/images/operator-banner.jpg "No more dead links. More happy customers.")

## No more dead ends. 
## More happy customers.

Operator is an easy-to-use tool which allows you to create and maintain redirects in a simple and intuitive way.

## Operator middleware for .NET Core

This package can be installed in your .NET Core application to setup an integration with the Operator redirect service. 

## Installation via .NET CLI

```bash
dotnet add package Operator --version 0.1.0
```

## Installation via Package Reference

Add the following PackageReference to yout `.csproj` file:

```
<PackageReference Include="Operator" Version="0.1.0" />
```

## Setup and configuration

Add the Operator package to your `startup.cs` file:

```c#
public void ConfigureServices(IServiceCollection services)
{

services.Configure<Operator.ConfigSection>(Configuration.GetSection("Operator"));
}
```


Next add Operator to the dependency injection service container like so:

```c#
public void Configure(IApplicationBuilder app, IHostingEnvironment env, IOptions<Operator.ConfigSection> operatorOptions)
{
```


Lastly let your app use the Operator middleware. Add this to the end of the `Configure` method after all other MVC routes.

```c#
app.UseStaticFiles();
app.UseCookiePolicy();

app.UseMvc();

app.UseOperator(options =>
{
    options.dsn = operatorOptions.Value.Dsn;
});
```
