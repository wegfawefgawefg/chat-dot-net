using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using System.Text;


[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class RouteAttribute : Attribute
{
    public string Url { get; }
    public RouteAttribute(string url)
    {
        Url = url;
    }
}


public class TinyWebServer
{
    private readonly Dictionary<string, Action<HttpListenerContext>> _routes = new();

    public void RegisterRoutes(object controller)
    {
        foreach (var method in controller.GetType().GetMethods())
        {
            var routeAttr = method.GetCustomAttribute<RouteAttribute>();
            if (routeAttr != null)
            {
                _routes[routeAttr.Url] = (Action<HttpListenerContext>)Delegate.CreateDelegate(typeof(Action<HttpListenerContext>), controller, method);
            }
        }
    }

    public void Start(string prefix)
    {
        HttpListener listener = new HttpListener();
        listener.Prefixes.Add(prefix);
        listener.Start();

        Console.WriteLine("Server started...");

        while (true)
        {
            var context = listener.GetContext();
            if (_routes.TryGetValue(context.Request.RawUrl, out var action))
            {
                action(context);
            }
            else
            {
                context.Response.StatusCode = 404;
                context.Response.Close();
            }
        }
    }
}

public class HomeController
{
    [Route("/")]
    public void Index(HttpListenerContext context)
    {
        byte[] responseBytes = Encoding.UTF8.GetBytes("Welcome to the home page!");
        context.Response.OutputStream.Write(responseBytes, 0, responseBytes.Length);
        context.Response.Close();
    }

    [Route("/about")]
    public void About(HttpListenerContext context)
    {
        byte[] responseBytes = Encoding.UTF8.GetBytes("About page content here.");
        context.Response.OutputStream.Write(responseBytes, 0, responseBytes.Length);
        context.Response.Close();
    }
}

public class Program
{
    public static void Main()
    {
        TinyWebServer server = new TinyWebServer();

        server.RegisterRoutes(new HomeController());

        // Start server on localhost port 8080
        server.Start("http://localhost:8080/");
    }
}
