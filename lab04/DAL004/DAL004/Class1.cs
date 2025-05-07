using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace lab04
{
    public interface IRepository : IDisposable
    {
        string BasePath { get; }
        Celebrity[] getAllCelebrities();
        Celebrity? getCelebrityById(int id);
        Celebrity[] getCelebritiesBySurname(string surname);
        string? getPhotoPathById(int id);

        int? addCelebrity(Celebrity celebrity);
        bool delCelebrityById(int id);
        int? updCelebrityById(int id, Celebrity celebrity);
        int SaveChanges();
    }
    public record Celebrity(int Id, string Firstname, string Surname, string PhotoPath);
    public class Repository : IRepository
    {
        public static string JSONPath { get; set; }

        public List<Celebrity> celebrities;
        public bool disposed;
        public int countChanges = 0;
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

        public int? addCelebrity(Celebrity celebrity)
        {
            int newId = celebrity.Id;
            if (celebrity.Id == 0)
            {
                newId = celebrities.Count + 1; 
            }
            var newCeleb = new Celebrity(newId, celebrity.Firstname, celebrity.Surname, celebrity.PhotoPath);
            celebrities.Add(newCeleb);
            countChanges++;
            return newId;
        }

        public bool delCelebrityById(int id)
        {
            var c = getCelebrityById(id);
            if (c == null || id <= 0)
            {
                return false;
            }

            celebrities.Remove(c);
            countChanges++;
            return true;
        }

        public int? updCelebrityById(int id, Celebrity celebrity)
        {
            var c = getCelebrityById(id);

            if (c == null || id <= 0)
            {
                return 0;
            }

            delCelebrityById(id);
            var updatedCelebrity = new Celebrity(id, celebrity.Firstname, celebrity.Surname, celebrity.PhotoPath);
            celebrities.Add(updatedCelebrity);
            return updatedCelebrity.Id;
        }


        public int SaveChanges()
        {
            string filepath = "D:\\\\лабораторные работы\\\\тпви\\\\lab04\\\\dal004\\\\Сelebrities\\\\Celebrities.json";
            if (!File.Exists(filepath))
            {
                Console.WriteLine("File is not found!!1");
            }
            try
            {
                string json = JsonConvert.SerializeObject(celebrities, Formatting.Indented);
                File.WriteAllText(filepath, json);
                return countChanges; 
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while saving file: {ex.Message}");
                return -1;  
            }
        }
    }
}
