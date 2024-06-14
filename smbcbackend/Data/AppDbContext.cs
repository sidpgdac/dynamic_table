//using Microsoft.EntityFrameworkCore;
//using SmbcApp.Model;

//namespace SmbcApp.Data
//{
//    public class AppDbContext : DbContext
//    {
//        public DbSet<ParentDetail> ParentDetails { get; set; }
//        public DbSet<ColumnDetail> ColumnDetails { get; set; }
//        public DbSet<ValueDetail> ValueDetails { get; set; }

//        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

//        protected override void OnModelCreating(ModelBuilder modelBuilder)
//        {
//            modelBuilder.Entity<ParentDetail>(entity =>
//            {
//                entity.ToTable("parentdetail"); // Match exact table name
//                entity.HasKey(p => p.Id);
//            });

//            modelBuilder.Entity<ColumnDetail>(entity =>
//            {
//                entity.ToTable("columndetail"); // Match exact table name
//                entity.HasKey(c => c.Id);
//                entity.HasOne(c => c.ParentDetail)
//                      .WithMany(p => p.ColumnDetails)
//                      .HasForeignKey(c => c.ParentId);
//            });

//            modelBuilder.Entity<ValueDetail>(entity =>
//            {
//                entity.ToTable("valuedetails"); // Match exact table name
//                entity.HasKey(v => v.Id);
//                entity.HasOne(v => v.ColumnDetail)
//                      .WithMany(c => c.ValueDetails)
//                      .HasForeignKey(v => v.ColumnId);
//            });
//        }
//    }
//}


using Microsoft.EntityFrameworkCore;
using SmbcApp.Model;
using smbcbackend.Audit;
using smbcbackend.Model;

namespace SmbcApp.Data
{
    public class AppDbContext : DbContext
    {
        private readonly string _currentUser;

        public DbSet<ParentDetail> ParentDetails { get; set; }
        public DbSet<ColumnDetail> ColumnDetails { get; set; }
        public DbSet<ValueDetail> ValueDetails { get; set; }
        public DbSet<ValueDetailsAudit> ValueDetailsAudits { get; set; } // Add the audit table

        public AppDbContext(DbContextOptions<AppDbContext> options, string currentUser)
            : base(options)
        {
            _currentUser = currentUser;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ParentDetail>(entity =>
            {
                entity.ToTable("parentdetail"); // Match exact table name
                entity.HasKey(p => p.Id);
            });

            modelBuilder.Entity<ColumnDetail>(entity =>
            {
                entity.ToTable("columndetail"); // Match exact table name
                entity.HasKey(c => c.Id);
                entity.HasOne(c => c.ParentDetail)
                      .WithMany(p => p.ColumnDetails)
                      .HasForeignKey(c => c.ParentId);
            });

            modelBuilder.Entity<ValueDetail>(entity =>
            {
                entity.ToTable("valuedetails"); // Match exact table name
                entity.HasKey(v => v.Id);
                entity.HasOne(v => v.ColumnDetail)
                      .WithMany(c => c.ValueDetails)
                      .HasForeignKey(v => v.ColumnId);
            });

            modelBuilder.Entity<ValueDetailsAudit>(entity =>
            {
                entity.ToTable("valuedetailsaudit"); // Match exact table name
                entity.HasKey(v => v.AuditId);
                entity.Property(v => v.ModifiedDate).IsRequired();
                entity.Property(v => v.ModifiedBy).IsRequired().HasMaxLength(255);
                entity.HasOne(v => v.ColumnDetail)
                      .WithMany()
                      .HasForeignKey(v => v.ColumnId);
                    /*  .OnDelete(DeleteBehavior.Cascade); */// Set cascade delete behavior if needed
            });

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.AddInterceptors(new AuditInterceptor(_currentUser));
        }
    }
}
