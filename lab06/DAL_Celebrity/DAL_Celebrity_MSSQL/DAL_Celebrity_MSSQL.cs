using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using static DAL_Celebrity_MSSQL.Repository;

namespace DAL_Celebrity_MSSQL
{
    public interface IRepository : DAL_Celebrity.IRepository<Celebrity, Lifeevent> { }
    public class Celebrity
    {
        public Celebrity() { this.FullName = string.Empty; this.Nationality = string.Empty; }
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Nationality { get; set; }
        public string? ReqPhotoPath { get; set; }
        public virtual bool Update(Celebrity celebrity)
        {
            if (celebrity == null) throw new ArgumentNullException(); return true;

        }
    }

    public class Lifeevent
    {
        public Lifeevent() { this.Description = string.Empty; }
        public int Id { get; set; }
        public int CelebrityId { get; set; }
        public DateTime? Date { get; set; }
        public string Description { get; set; }
        public string? ReqPhotoPath { get; set; }
        public virtual bool Update(Lifeevent lifeevent)
        {
            if (lifeevent == null) throw new ArgumentNullException(); return true;
        }
    }
    public class Repository : IRepository
    {

        Context context;

        public Repository() { this.context = new Context(); }

        public Repository(string connectionString) { this.context = new Context(connectionString); }

        public static IRepository Create() { return new Repository(); }

        public static IRepository Create(string connectionString) { return new Repository(connectionString); }

        public List<Celebrity> GetAllCelebrities() { return this.context.Celebrities.ToList<Celebrity>(); }

        public Celebrity? GetCelebrityById(int id)
        {
            return context.Celebrities.FirstOrDefault(c => c.Id == id);
        }

        public bool AddCelebrity(Celebrity celebrity)
        {
            if (celebrity != null)
            {
                context.Celebrities.Add(celebrity);
                context.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool DelCelebrity(int id)
        {
            var celebrity = GetCelebrityById(id);
            if (celebrity != null)
            {
                context.Celebrities.Remove(celebrity);
                context.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool UpdCelebrity(int id, Celebrity celebrity)
        {
            var celebrityToUpdate = GetCelebrityById(id);

            if (celebrityToUpdate != null)
            {
                celebrityToUpdate.FullName = celebrity.FullName;
                celebrityToUpdate.Nationality = celebrity.Nationality;
                celebrityToUpdate.ReqPhotoPath = celebrity.ReqPhotoPath;

                context.SaveChanges();
                return true;
            }
            else
            {
                return AddCelebrity(celebrity);
            }
        }

        public List<Lifeevent> GetAllLifeevents() { return this.context.Lifeevents.ToList<Lifeevent>(); }

        public Lifeevent? GetLifeeventById(int id)
        {
            return context.Lifeevents.FirstOrDefault(l => l.Id == id);
        }

        public bool AddLifeevent(Lifeevent lifeevent)
        {
            if (lifeevent != null)
            {
                context.Lifeevents.Add(lifeevent);
                context.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool DelLifeevent(int id)
        {
            var lifeevent = GetLifeeventById((int)id);
            if (lifeevent != null)
            {
                context.Lifeevents.Remove(lifeevent);
                context.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool UpdLifeevent(int id, Lifeevent lifeevent)
        {
            var lifeeventToUpdate = GetLifeeventById(id);

            if (lifeeventToUpdate != null)
            {
                lifeeventToUpdate.CelebrityId = lifeevent.CelebrityId;
                lifeeventToUpdate.Date = lifeevent.Date;
                lifeeventToUpdate.Description = lifeevent.Description;
                lifeeventToUpdate.ReqPhotoPath = lifeevent.ReqPhotoPath;

                context.SaveChanges();
                return true;
            }
            else
            {
                return AddLifeevent(lifeevent);
            }
        }

        public List<Lifeevent> GetLifeeventsByCelebrityId(int celebrityId)
        {
            var listOfEvents = context.Lifeevents.Where(listOfEvents => listOfEvents.CelebrityId == celebrityId).ToList();
            if (listOfEvents.Count == 0)
            {
                return new List<Lifeevent>();
            }
            return listOfEvents;
        }

        public Celebrity? GetCelebrityByLifeeventId(int lifeeventId)
        {
            var lifeevent = GetLifeeventById(lifeeventId);
            if (lifeevent == null)
                throw new Exception($"Событие с ID {lifeeventId} не найдено.");

            var celebrity = GetCelebrityById(lifeevent.CelebrityId);
            if (celebrity == null)
                throw new Exception($"Знаменитость с ID {lifeevent.CelebrityId} не найдена.");

            return celebrity;
        }

        public int GetCelebrityIdByName(string name)
        {
            var getCeleb = context.Celebrities.FirstOrDefault(n => n.FullName.Contains(name));
            if (getCeleb != null)
            {
                return getCeleb.Id;
            }
            else
            {
                throw new Exception($"Знаменитость с именем '{name}' не найдена.");
            }
        }
        public void Dispose()
        {
            context.Dispose();
        }
    }
    public class Context : DbContext
    {
        public string? ConnectionString { get; private set; } = null;
        public Context(string connstring) : base()
        {
            this.ConnectionString = connstring;
            //this.Database.EnsureDeleted();
            //this.Database.EnsureCreated();
        }
        public Context() : base()
        {
            this.Database.EnsureDeleted();
            this.Database.EnsureCreated();
        }
        public DbSet<Celebrity> Celebrities { get; set; }
        public DbSet<Lifeevent> Lifeevents { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (this.ConnectionString is null) this.ConnectionString = @"Data source = WIN-0RRORC9T71J\SQLEXPRESS; Initial Catalog = Lab06Celeb;" +
                           @"TrustServerCertificate = Yes; Integrated Security=True;";
            optionsBuilder.UseSqlServer(this.ConnectionString, sqlOptions => sqlOptions.EnableRetryOnFailure()).EnableSensitiveDataLogging();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Celebrity>().ToTable("Celebrities").HasKey(p => p.Id);
            modelBuilder.Entity<Celebrity>().Property(p => p.Id).IsRequired();
            modelBuilder.Entity<Celebrity>().Property(p => p.FullName).IsRequired().HasMaxLength(50);
            modelBuilder.Entity<Celebrity>().Property(p => p.Nationality).IsRequired().HasMaxLength(2);
            modelBuilder.Entity<Celebrity>().Property(p => p.ReqPhotoPath).HasMaxLength(300);

            modelBuilder.Entity<Lifeevent>().ToTable("Lifeevents").HasKey(p => p.Id);
            modelBuilder.Entity<Lifeevent>().ToTable("Lifeevents");
            modelBuilder.Entity<Lifeevent>().Property(p => p.Id).IsRequired();
            modelBuilder.Entity<Lifeevent>().ToTable("Lifeevents").HasOne<Celebrity>().WithMany().HasForeignKey(p => p.CelebrityId);
            modelBuilder.Entity<Lifeevent>().Property(p => p.CelebrityId).IsRequired();
            modelBuilder.Entity<Lifeevent>().Property(p => p.Date);
            modelBuilder.Entity<Lifeevent>().Property(p => p.Description).HasMaxLength(256);
            modelBuilder.Entity<Lifeevent>().Property(p => p.ReqPhotoPath).HasMaxLength(256);
            base.OnModelCreating(modelBuilder);
        }
    }
}
