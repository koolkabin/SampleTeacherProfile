using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CollegeWebsiteAdmin.Models
{

    public class MyDBContext : DbContext
    {
        private readonly IConfiguration _myAppSettingsConfig;
        public MyDBContext(IConfiguration configFromAppSettings)
        {
            _myAppSettingsConfig = configFromAppSettings;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Use the connection string from appsettings.json
                optionsBuilder.UseSqlServer(_myAppSettingsConfig.GetConnectionString("ABCDatabase"));
            }
        }

        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Colleges> Colleges { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<TeacherSubjects> TeacherSubjects { get; set; }
        public DbSet<CollegeTeachers> CollegeTeachers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Write Fluent API configurations here

            //Property Configurations

            //from child to master => def
            //modelBuilder.Entity<TeacherSubjects>()
            //    .HasOne<Subject>(s => s.Subject)
            //    .WithMany(g => g.TeacherSubjects)
            //    .HasForeignKey(s => s.SubjectID);

            //from parent to child => def
            modelBuilder.Entity<Subject>()
                .HasMany<TeacherSubjects>(g => g.TeacherSubjects)
                .WithOne(s => s.Subject)
                .HasForeignKey(s => s.SubjectID)
                .OnDelete(DeleteBehavior.Restrict);
            //orphand records -> db -> space, mis-calculations
            //its for orphand records

            //shifting it to own configuration classes


            //from parent to child => def
            modelBuilder.Entity<Teacher>()
                .HasMany<CollegeTeachers>(g => g.CollegeTeachers)
                .WithOne(s => s.Teacher)
                .HasForeignKey(s => s.TeacherId)
                .OnDelete(DeleteBehavior.Restrict);

            //from parent to child => def
            modelBuilder.Entity<Colleges>()
                .HasMany<CollegeTeachers>(g => g.CollegeTeachers)
                .WithOne(s => s.Colleges)
                .HasForeignKey(s => s.CollegeId)
                .OnDelete(DeleteBehavior.Restrict);

            //from parent to child => def
            modelBuilder.Entity<Province>()
                .HasMany(g => g.District)
                .WithOne(s => s.Province)
                .HasForeignKey(s => s.ProvinceId)
                .OnDelete(DeleteBehavior.Restrict);
        }

        public DbSet<CollegeWebsiteAdmin.Models.Province> Province => Set<Province>();

        public DbSet<CollegeWebsiteAdmin.Models.District> District => Set<District>();
    }


}
