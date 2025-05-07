using lab03;
namespace TestRepo
{
    internal class Test
    {
        static void Main(string[] args)
        {
            Repository.JSONPath = "D:\\лабораторные работы\\тпви\\lab03\\TestRepo\\Сelebrities\\Сelebrities.json";
            using (IRepository repo = Repository.Create("D:\\лабораторные работы\\тпви\\lab03\\TestRepo\\bin\\Debug\\net7.0\\Сelebrities\\Сelebrities.json"))
            {
                foreach (Celebrity celeb in repo.getAllCelebrities())
                {
                    Console.WriteLine($"Id = {celeb.Id}, Firstname = {celeb.Firstname}, Surname = {celeb.Surname}, PhotoPath = {celeb.PhotoPath}");
                }

                Console.WriteLine();
                Celebrity ? celebrity1 = repo.getCelebrityById(1);

                if(celebrity1 != null )
                {
                    Console.WriteLine($"Id = {celebrity1.Id}, Firstname = {celebrity1.Firstname}, Surname = {celebrity1.Surname}, PhotoPath = {celebrity1.PhotoPath}");
                }
                Celebrity? celebrity3 = repo.getCelebrityById(3);
                if (celebrity3 != null)
                {
                    Console.WriteLine($"Id = {celebrity3.Id}, Firstname = {celebrity3.Firstname}, Surname = {celebrity3.Surname}, PhotoPath = {celebrity3.PhotoPath}");
                }
                Celebrity? celebrity7 = repo.getCelebrityById(7);
                if (celebrity7 != null)
                {
                    Console.WriteLine($"Id = {celebrity7.Id}, Firstname = {celebrity7.Firstname}, Surname = {celebrity7.Surname}, PhotoPath = {celebrity7.PhotoPath}");
                }
                Celebrity? celebrity222 = repo.getCelebrityById(222);
                if (celebrity222 != null)
                {
                    Console.WriteLine($"Id = {celebrity222.Id}, Firstname = {celebrity222.Firstname}, Surname = {celebrity222.Surname}, PhotoPath = {celebrity222.PhotoPath}");
                }
                else Console.WriteLine("Not Found 222");

                foreach(Celebrity celeb in repo.getCelebritiesBySurname("Chomsky"))
                {
                    Console.WriteLine($"Id = {celeb.Id}, Firstname = {celeb.Firstname}, Surname = {celeb.Surname}, PhotoPath = {celeb.PhotoPath}");
                }
                foreach (Celebrity celeb in repo.getCelebritiesBySurname("Knuth"))
                {
                    Console.WriteLine($"Id = {celeb.Id}, Firstname = {celeb.Firstname}, Surname = {celeb.Surname}, PhotoPath = {celeb.PhotoPath}");
                }
                foreach (Celebrity celeb in repo.getCelebritiesBySurname("XXXX"))
                {
                    Console.WriteLine($"Id = {celeb.Id}, Firstname = {celeb.Firstname}, Surname = {celeb.Surname}, PhotoPath = {celeb.PhotoPath}");
                }

                Console.WriteLine($"PhotoPathById = {repo.getPhotoPathById(4)}");
                Console.WriteLine($"PhotoPathById = {repo.getPhotoPathById(6)}");
                Console.WriteLine($"PhotoPathById = {repo.getPhotoPathById(222)}");
            }
        }
    }
}