namespace lab02
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var app = builder.Build();

            app.MapGet("/aspnetcore", () => "Hello, ASP.NET Core!");

            app.UseWelcomePage("/aspnetcore");


            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.Run();
        }
    }
}