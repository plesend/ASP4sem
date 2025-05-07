using Microsoft.Extensions.FileProviders;

namespace ASPA002_2
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var app = builder.Build();

            var defaultFilesOptions = new DefaultFilesOptions();
            defaultFilesOptions.DefaultFileNames.Clear();
            defaultFilesOptions.DefaultFileNames.Add("Neumann.html");

            app.UseDefaultFiles(defaultFilesOptions);
            app.UseStaticFiles();

            app.UseStaticFiles();

            var picturePath = Path.Combine(Directory.GetCurrentDirectory(), "Picture");
            if (!Directory.Exists(picturePath))
            {
                Directory.CreateDirectory(picturePath); 
            }

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(picturePath),
            });

            app.Run();
        }
    }
}
