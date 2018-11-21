using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace CustomMiddlewareExample
{
  public class CustomMiddleware
  {
    private readonly RequestDelegate _next;
    private readonly ILogger _logger;

    public CustomMiddleware(
      RequestDelegate next,
      ILogger<CustomMiddleware> logger
      )
    {
      _next = next;
      _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
      var token = GetToken(context);
      var url = GetDisplayUrl(context);
      _logger.LogInformation($"{token ?? "Unknown user"} is accessing {url}");
      await _next(context);
    }

    private string GetToken(HttpContext context)
    {
        return context.Request.Headers.FirstOrDefault(x =>
                string.Equals(x.Key, "Authorization", StringComparison.OrdinalIgnoreCase)).Value.FirstOrDefault();
    }

    private string GetDisplayUrl(HttpContext context)
    {
      string str1 = context.Request.Host.Value;
      string str2 = context.Request.PathBase.Value;
      string str3 = context.Request.Path.Value;
      string str4 = context.Request.QueryString.Value;
      return new StringBuilder(context.Request.Scheme.Length + "://".Length + str1.Length + str2.Length + str3.Length + str4.Length).Append(context.Request.Scheme).Append("://").Append(str1).Append(str2).Append(str3).Append(str4).ToString();
    }
  }
}