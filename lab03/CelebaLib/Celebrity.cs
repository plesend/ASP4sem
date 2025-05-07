using System.Net.Http.Json;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace lab03
{
    public interface IRepository : IDisposable
    {
        string BasePath { get; }
        Celebrity[] getAllCelebrities();
        Celebrity? getCelebrityById(int id);
        Celebrity[] getCelebritiesBySurname(string surname);
        string? getPhotoPathById(int id);
    }
    public record Celebrity(int Id, string Firstname, string Surname, string PhotoPath);
    public class Repository : IRepository
    {
        public static string JSONPath { get; set; }

        public List<Celebrity> celebrities;
        public bool disposed;
        public string BasePath { get; } = "Photo/";

        public Repository(string filepath)
        {
            if (!File.Exists(filepath)) { throw new FileNotFoundException(); }
            string jsonPath = File.ReadAllText(filepath);
            celebrities = JsonConvert.DeserializeObject<List<Celebrity>>(jsonPath);
        }

        public static Repository Create(string filepath)
        {
            return new Repository(filepath);
        }

        public void Dispose()
        {
            if (!disposed)
            {
                disposed = true;
            }
        }

        public Celebrity[] getAllCelebrities()
        {
            return celebrities.ToArray();
        }

        public Celebrity[] getCelebritiesBySurname(string surname)
        {
            return celebrities.Where(c => c.Surname == surname).ToArray();
        }

        public Celebrity? getCelebrityById(int id)
        {
            return celebrities.FirstOrDefault(c => c.Id == id);
        }

        public string? getPhotoPathById(int id)
        {
            var c = getCelebrityById(id);
            return c?.PhotoPath;
        }
    }
}
