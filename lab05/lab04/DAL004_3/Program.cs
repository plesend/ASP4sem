using lab04;
using Microsoft.AspNetCore.Diagnostics;

namespace DAL004_1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var app = builder.Build();

            app.MapGet("/", () => "Hello World!");

            Repository.basePath1 = "D:\\ëàáîðàòîðíûå ðàáîòû\\òïâè\\lab05\\Ñelebrities";
            Repository.JSONPath = "D:\\ëàáîðàòîðíûå ðàáîòû\\òïâè\\lab05\\Ñelebrities\\Ñelebrities.json";

            using (IRepository repository = new Repository(Repository.JSONPath))
            {
                app.UseExceptionHandler("/Celebrities/Error");

                app.MapGet("/Celebrities", () => repository.getAllCelebrities());
                app.MapGet("/Celebrities/{id:int}", (int id) =>
                {
                    Celebrity? celebrity = repository.getCelebrityById(id);
                    if (celebrity == null) throw new FoundByIdException($"Celebrity id = {id}");
                    return celebrity;
                });
                app.MapPost("/Celebrities", (Celebrity celebrity) =>
                {
                    int? id = repository.addCelebrity(celebrity);
                    if (id == null) throw new AddCelebrityException("/Celebrities error id == null");
                    if (repository.SaveChanges() <= 0) throw new SaveException("/celebrities error savechanges<=0");
                    return new Celebrity((int)id, celebrity.Firstname, celebrity.Surname, celebrity.PhotoPath);
                })
                .AddEndpointFilter(async (context, next) =>
                {
                    var celebrity = context.GetArgument<Celebrity>(0);
                    if (celebrity == null)
                    {
                        return Results.Problem("Map post can't find this celebrity", statusCode: 500);
                    }
                    if (celebrity.Surname == null || celebrity.Surname.Length < 2)
                    {
                        return Results.Problem("SURNAME is wrong", statusCode: 409);
                    }
                    return await next(context);
                }).AddEndpointFilter(async(context, next) => 
                {
                    var celebrity = context.GetArgument<Celebrity>(0);
                    if (celebrity == null)
                    {
                        return Results.Problem("Map post can't find this celebrity", statusCode: 500);
                    }
                    //
                    if (repository.getCelebritiesBySurname(celebrity.Surname).Length != 0)
                    {
                        return Results.Problem("The celebrity with such SURNAME alresdy exists", statusCode: 409);
                    }
                    return await next(context);
                }).AddEndpointFilter(async (context, next) => 
                {
                    var celebrity = context.GetArgument<Celebrity>(0);
                    if (celebrity == null)
                    {
                        return Results.Problem("Map post can't find this celebrity", statusCode: 500);
                    }
                    string filepath = Path.Combine(Repository.basePath1, Path.GetFileName(celebrity.PhotoPath));

                    if (!File.Exists(filepath)) 
                    {
                        context.HttpContext.Response.Headers.Append("x-celebrity", $"notfound({Path.GetFileName(celebrity.PhotoPath)})");
                    }
                    return await next(context);
                });

                app.MapDelete("/Celebrities/{id:int}", (int id) =>
                {
                    bool success = repository.delCelebrityById(id);
                    if (!success)
                    {
                        throw new DeleteCelebrityException($"celebrity {id} not found for deletion");
                    }
                    else return Results.Content($"celebrity {id} deleted");
                });

                app.MapPut("/Celebrities/{id:int}", (int id, Celebrity newCelebrity) => {
                    int? result = repository.updCelebrityById(id, newCelebrity);
                    if(result == null || result == 0) throw new UpdateCelebrityException($"Celebrity {id} not found for update");
                    return new Celebrity(id, newCelebrity.Firstname, newCelebrity.Surname, newCelebrity.PhotoPath);
                });

                app.MapFallback((HttpContext ctx) => Results.NotFound(new { error = $"path {ctx.Request.Path} not supported" }));

                app.Map("/Celebrities/Error", (HttpContext ctx) =>
                {
                    Exception? ex = ctx.Features.Get<IExceptionHandlerFeature>()?.Error;
                    //IResult rc = Results.Problem(detail: "panic", instance: app.Environment.EnvironmentName, title: "aspa004", statusCode: 500);
                    IResult rc = Results.Problem(detail: ex?.Message, instance: app.Environment.EnvironmentName, title: "aspa004", statusCode: 500);

                    if (ex != null)
                    {
                        if (ex is UpdateCelebrityException) rc = Results.NotFound(ex.Message);
                        if (ex is DeleteCelebrityException) rc = Results.NotFound(ex.Message);
                        if (ex is FoundByIdException) rc = Results.NotFound(ex.Message);
                        if (ex is BadHttpRequestException) rc = Results.BadRequest(ex.Message);
                        if (ex is SaveException) rc = Results.Problem(title: "aspa004/savechanges", detail: ex.Message, instance: app.Environment.EnvironmentName, statusCode: 500);
                        if (ex is AddCelebrityException) rc = Results.Problem(title: "aspa004/addcelebrity", detail: ex.Message, instance: app.Environment.EnvironmentName, statusCode: 500);

                    }
                    return rc;
                });

                app.Run();
            }
        }
        public class FoundByIdException : Exception
        {
            public FoundByIdException(string message) : base($"found by id : {message}") { }
        };
        public class SaveException : Exception
        {
            public SaveException(string message) : base($"savechanges error: {message}") { }
        };
        public class AddCelebrityException : Exception { public AddCelebrityException(string message) : base($"addcelebrityException error: {message}") { } }
        public class DeleteCelebrityException : Exception { public DeleteCelebrityException(string message) : base($"DeleteCelebrityException error: {message}") { } }
        public class UpdateCelebrityException : Exception { public UpdateCelebrityException(string message) : base($"UpdateCelebrityException error: {message}") { } }

    }
}