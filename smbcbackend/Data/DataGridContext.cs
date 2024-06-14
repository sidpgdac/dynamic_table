//using Microsoft.EntityFrameworkCore;
//using SmbcApp.Model;

//namespace SmbcApp.Data
//{
//    public class DataGridContext : DbContext
//    {
//        public DataGridContext(DbContextOptions<DataGridContext> options) : base(options)
//        {
//        }

//        public DbSet<ParentDetail> DataSources { get; set; }
//        public DbSet<ColumnDetail> DataColumns { get; set; }
//        public DbSet<ValueDetail> DataValues { get; set; }

//        protected override void OnModelCreating(ModelBuilder modelBuilder)
//        {
//            modelBuilder.Entity<ParentDetail>()
//                .HasKey(ds => ds.DataSourceID);

//            modelBuilder.Entity<ColumnDetail>()
//                .HasKey(dc => dc.ColumnID);

//            modelBuilder.Entity<ColumnDetail>()
//                .HasOne<ParentDetail>()
//                .WithMany()
//                .HasForeignKey(dc => dc.DataSourceID);

//            modelBuilder.Entity<ValueDetail>()
//                .HasKey(dv => dv.ValueID);

//            modelBuilder.Entity<ValueDetail>()
//                .HasOne<ParentDetail>()
//                .WithMany()
//                .HasForeignKey(dv => dv.DataSourceID);

//            modelBuilder.Entity<ValueDetail>()
//                .HasOne<ColumnDetail>()
//                .WithMany()
//                .HasForeignKey(dv => dv.ColumnID);
//        }
//    } 
//}