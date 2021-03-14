using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;


public class Redirect
{
  public int id { get; set; }
  public string to { get; set; }
  public string from { get; set; }
  public int count { get; set; }
  public string created_at { get; set; }
  public string updated_at { get; set; }
  public int version { get; set; }
}

public class ApiResponse
{
  public string status { get; set; }
  public Redirect data { get; set; }
}


namespace Operator
{

  public class DSN
  {
    public DSN(string dsn)
    {
      var uri = new Uri(dsn);

      this.host = uri.Host;
      this.scheme = uri.Scheme;
      this.port = uri.Port;
      this.project = uri.PathAndQuery.TrimStart('/');
      this.apikey = uri.UserInfo;
    }
    
    public string scheme { get; set; }
    public string apikey { get; set; }
    public string project { get; set; }
    public string host { get; set; }
    public int port { get; set; }

  }

  public class Options
  {
    public string dsn { get; set; }
  }

  public class RedirectMiddleware
  {
    private readonly RequestDelegate _next;
    private readonly Options _options;
    private readonly DSN _dsn;

    public RedirectMiddleware(RequestDelegate next, Options options)
    {
      _next = next;
      _options = options;
      _dsn = new DSN(_options.dsn);
    }

    public async Task InvokeAsync(HttpContext context)
    {
      var client = new HttpClient(); 
      var builder = new UriBuilder(_dsn.scheme, _dsn.host, _dsn.port, "/api/redirects");

      var query = HttpUtility.ParseQueryString(builder.Query);
      query["project"] = _dsn.project;
      query["apikey"] = _dsn.apikey;
      query["request_path"] = context.Request.Path.Value;
      builder.Query = query.ToString();

      var url = builder.ToString();

      try
      {
        var content = await client.GetStringAsync(url);
        var redirect = JsonConvert.DeserializeObject<ApiResponse>(content);

        if (redirect.data != null && redirect.data.to != null)
        {
          context.Response.Redirect(redirect.data.to);
          return;
        }
      }
      catch (System.Net.Http.HttpRequestException)
      {
        Console.WriteLine("Catch System.Net.Http.HttpRequestException and continue with next");
      }

      await _next(context);
    }
  }
}

public static class MiddlewareExtensions
{
  public static IApplicationBuilder UseOperator(
      this IApplicationBuilder builder, Action<Operator.Options> configureOptions)
  {
    var options = new Operator.Options();
    configureOptions(options);

    return builder.UseMiddleware<Operator.RedirectMiddleware>(options);
  }
}
