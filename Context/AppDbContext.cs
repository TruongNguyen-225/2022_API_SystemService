using Microsoft.EntityFrameworkCore;
using SystemServiceAPI.Entities.Table;
using SystemServiceAPI.Entities.View;
using SystemServiceAPICore3.Entities.Table;

namespace SystemServiceAPI.Context
{
    public partial class AppDbContext : DbContext
    {
        public AppDbContext()
        {
        }
        public AppDbContext(DbContextOptions<AppDbContext> options)
           : base(options)
        {
        }

        public virtual DbSet<Bank> Banks { get; set; } = null!;
        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<Customer> Customers { get; set; } = null!;
        public virtual DbSet<HistoryExportData> HistoryExportData { get; set; } = null!;
        public virtual DbSet<MonthlyTransaction> MonthlyTransactions { get; set; } = null!;
        public virtual DbSet<MonthlyTransactionTemp> MonthlyTransactionTemp { get; set; } = null!;
        public virtual DbSet<Retail> Retails { get; set; } = null!;
        public virtual DbSet<Service> Services { get; set; } = null!;
        public virtual DbSet<Village> Villages { get; set; } = null!;
        public virtual DbSet<Level> Levels { get; set; } = null!;
        public virtual DbSet<ConfigPrice> ConfigPrices { get; set; } = null!;
        public virtual DbSet<Screen> Screens { get; set; } = null!;
        public virtual DbSet<FileUpload> FileUploads { get; set; } = null!;
        public virtual DbSet<vw_Customer> vw_Customers { get; set; } = null!;
        public virtual DbSet<vw_DataBarChart> vw_DataBarCharts { get; set; } = null!;
        public virtual DbSet<vw_MonthlyTransaction> vw_MonthlyTransactions { get; set; } = null!;
        public virtual DbSet<vw_MonthlyTransactionTemp> vw_MonthlyTransactionTemp { get; set; } = null!;
        public virtual DbSet<vw_PieChart> vw_PieCharts { get; set; } = null!;
        public virtual DbSet<vw_PieChartService> vw_PieChartServices { get; set; } = null!;
        //public virtual DbSet<vw_Withdrawal> vw_Withdrawals { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<vw_MonthlyTransaction>(entity =>
            {
                entity.ToView("vw_MonthlyTransaction");
                entity.HasNoKey();
            });

            modelBuilder.Entity<vw_MonthlyTransactionTemp>(entity =>
            {
                entity.ToView("vw_MonthlyTransactionTemp");
                entity.HasNoKey();
            });

            modelBuilder.Entity<vw_Customer>(entity =>
            {
                entity.ToView("vw_Customer");
                entity.HasNoKey();
            });

            modelBuilder.Entity<vw_PieChart>(entity =>
            {
                entity.ToView("vw_PieChart");
                entity.HasNoKey();
            });

            modelBuilder.Entity<vw_PieChartService>(entity =>
            {
                entity.ToView("vw_PieChartServices");
                entity.HasNoKey();
            });

            modelBuilder.Entity<vw_DataBarChart>(entity =>
            {
                entity.ToView("vw_DataBarChart");
                entity.HasNoKey();
            });

            modelBuilder.Entity<vw_HistoryExport>(entity =>
            {
                entity.ToView("vw_HistoryExport");
                entity.HasNoKey();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}