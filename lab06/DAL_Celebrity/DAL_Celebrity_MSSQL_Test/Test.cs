using DAL_Celebrity_MSSQL;

namespace DAL_Celebrity_MSSQL_Test
{
    internal class Test
    {
        static void Main(string[] args)
        {
            string CS = @"Data source = WIN-0RRORC9T71J\SQLEXPRESS; Initial Catalog = Lab06Celeb;" +
                           @"TrustServerCertificate = Yes; Integrated Security=True;";

            Init init = new Init(CS);
            Init.Execute(delete: true, create: true);

            Func<Celebrity, string> printC = (c) => $"Id = {c.Id}, FullName = {c.FullName}, Nationality = {c.Nationality}, ReqPhotoPath {c.ReqPhotoPath}";
            Func<Lifeevent, string> printL = (l) => $"Id = {l.Id}, CelebrityId {l.CelebrityId}, Date {l.Date}, Description {l.Description}, ReqPhotoPath {l.ReqPhotoPath}";
            Func<string, string> puri = (string f) => $"{f}";

            using (IRepository repo = Repository.Create(CS))
            {
                {
                    Console.WriteLine(" all celebrities");
                    foreach (var item in repo.GetAllCelebrities())
                    {
                        Console.WriteLine(printC(item));
                    }
                }
                {
                    Console.WriteLine(" all lifeevents");
                    foreach (var item in repo.GetAllLifeevents())
                    {
                        Console.WriteLine(printL(item));
                    }
                }
                {
                    Console.WriteLine(" add celebrity");
                    Celebrity c = new Celebrity { FullName = "Albert Einstein", Nationality = "DE", ReqPhotoPath = puri("Einstein.jpg") };
                    if (repo.AddCelebrity(c)) Console.WriteLine($"OK addcelebrity: {printC(c)}");
                    else Console.WriteLine($"error add celebrity: {printC(c)}");
                }
                {
                    Console.WriteLine(" add celebrity");
                    Celebrity c = new Celebrity { FullName = "Samuel Huntington", Nationality = "US", ReqPhotoPath = puri("Huntington.jpg") };
                    if (repo.AddCelebrity(c)) Console.WriteLine($"OK addcelebrity: {printC(c)}");
                    else Console.WriteLine($"error add celebrity: {printC(c)}");
                }

                {
                    Console.WriteLine("del celebrity");
                    {
                        int id = 0;
                        if ((id = repo.GetCelebrityIdByName("Einstein")) > 0)
                        {
                            repo.DelCelebrity(id);
                        }
                        else Console.WriteLine("error getcelebrityById");
                    }
                }
                {
                    Console.WriteLine("upd celebrity");
                    {
                        int id = 0;
                        if ((id = repo.GetCelebrityIdByName("Huntington")) > 0)
                        {
                            Celebrity? c = repo.GetCelebrityById(id);
                            if (c != null)
                            {
                                c.FullName = "samuel phillips huntington";
                                repo.UpdCelebrity(id, c);
                            }
                            else Console.WriteLine($"error: getCelebrityIdByName");
                        }
                        else Console.WriteLine("error getcelebrityById");
                    }
                }
                {
                    Console.WriteLine("add lifeevent ");
                    {
                        int id = 0;
                        if ((id = repo.GetCelebrityIdByName("Huntington")) > 0)
                        {
                            Lifeevent l = new Lifeevent
                            {
                                CelebrityId = id,
                                Date = new DateTime(1927, 4, 18),
                                Description = "рождение",
                                ReqPhotoPath = null
                            };
                            Lifeevent l2 = new Lifeevent
                            {
                                CelebrityId = id,
                                Date = new DateTime(1928, 4, 12),
                                Description = "год после рождения",
                                ReqPhotoPath = null
                            };

                            repo.AddLifeevent(l);
                            repo.AddLifeevent(l2);
                        }
                        else Console.WriteLine("error getcelebrityById");
                    }
                }
                {
                    Console.WriteLine("del lifeevent ");
                    {
                        int id = 21;
                        if (repo.DelLifeevent(id)) Console.WriteLine($"OK: dellifeevent: {id}");
                        else Console.WriteLine("error getcelebrityById");
                    }
                }
                {
                    Console.WriteLine("upd lifeevent ");
                    {
                        int id = 22;
                        Lifeevent? l1;
                        if ((l1 = repo.GetLifeeventById(id)) != null)
                        {
                            l1.Description = "дата смерти";
                            if (repo.UpdLifeevent(id, l1)) Console.WriteLine($"OK: updlifeevent {id}, {printL(l1)}");
                            else Console.WriteLine($"error updlifeevent {id} {printL(l1)}");
                        }
                        else Console.WriteLine("error getcelebrityById");
                    }
                }
                {
                    Console.WriteLine("getLifeEventByCelebrityId");
                    int id = 0;
                    if ((id = repo.GetCelebrityIdByName("Huntington")) > 0)
                    {
                        Celebrity? c = repo.GetCelebrityById(id);
                        if (c != null) repo.GetLifeeventsByCelebrityId(c.Id).ForEach(l => Console.WriteLine($"OK: getLifeEventsByCelebrityId, {id}, {printL(l)}"));
                        else Console.WriteLine($"Error: getlifeeventsbycelebrityid: {id}");
                    }
                    else Console.WriteLine($"error getCelebrityIdByName");
                }
                {
                    Console.WriteLine("get celebrity by lifeevent id");
                    int id = 22;
                    Celebrity? c;
                    if ((c = repo.GetCelebrityByLifeeventId(id)) != null) Console.WriteLine($"OK: {printC(c)}");
                    else Console.WriteLine($"error get celebrity by lifeevent id, {id}");
                }
            }
            Console.WriteLine("-------------------------------------------------------------------------------->"); Console.ReadKey();
        }
    }
}
