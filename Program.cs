using Microsoft.AspNetCore;

namespace LibraryApp.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }
        public static IWebHost BuildWebHost(string[] args)=>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseUrls("https://*:8080/")
                .UseDefaultServiceProvider(options=>options.ValidateScopes=false)
                .Build();
        
    }
}