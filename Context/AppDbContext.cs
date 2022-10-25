using Microsoft.EntityFrameworkCore;
using SystemServiceAPI.Entities.Table;
using SystemServiceAPI.Entities.View;

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
        //public virtual DbSet<CalculatorCash> CalculatorCashes { get; set; } = null!;
        //public virtual DbSet<CalculatorHistory> CalculatorHistories { get; set; } = null!;
        //public virtual DbSet<Cash> Cashes { get; set; } = null!;
        //public virtual DbSet<CostManage_CostDetail> CostManage_CostDetails { get; set; } = null!;
        //public virtual DbSet<CostManage_GroupCost> CostManage_GroupCosts { get; set; } = null!;
        //public virtual DbSet<CostManage_GroupIncome> CostManage_GroupIncomes { get; set; } = null!;
        //public virtual DbSet<CostManage_IncomeOfMonth> CostManage_IncomeOfMonths { get; set; } = null!;
        //public virtual DbSet<CostManage_Login> CostManage_Logins { get; set; } = null!;
        //public virtual DbSet<CostManage_TransactionOfMonth> CostManage_TransactionOfMonths { get; set; } = null!;
        public virtual DbSet<Customer> Customers { get; set; } = null!;
        //public virtual DbSet<ElectricityBill> ElectricityBills { get; set; } = null!;
        public virtual DbSet<HistoryExportData> HistoryExportData { get; set; } = null!;
        //public virtual DbSet<Income> Incomes { get; set; } = null!;
        //public virtual DbSet<MoneyTransfer> MoneyTransfers { get; set; } = null!;
        public virtual DbSet<MonthlyTransaction> MonthlyTransactions { get; set; } = null!;
        public virtual DbSet<MonthlyTransactionTemp> MonthlyTransactionTemp { get; set; } = null!;
        //public virtual DbSet<Option> Options { get; set; } = null!;
        public virtual DbSet<Retail> Retails { get; set; } = null!;
        public virtual DbSet<Service> Services { get; set; } = null!;
        //public virtual DbSet<Transaction> Transactions { get; set; } = null!;
        public virtual DbSet<Village> Villages { get; set; } = null!;
        //public virtual DbSet<vw_AllService> vw_AllServices { get; set; } = null!;
        //public virtual DbSet<vw_ChartIncome> vw_ChartIncomes { get; set; } = null!;
        //public virtual DbSet<vw_CostEmploy> vw_CostEmploys { get; set; } = null!;
        //public virtual DbSet<vw_CostManage_CostDetail> vw_CostManage_CostDetails { get; set; } = null!;
        //public virtual DbSet<vw_CostManage_CostOfMonth> vw_CostManage_CostOfMonths { get; set; } = null!;
        //public virtual DbSet<vw_CostManage_IncomeOfMonth> vw_CostManage_IncomeOfMonths { get; set; } = null!;
        //public virtual DbSet<vw_CostManage_Transaction> vw_CostManage_Transactions { get; set; } = null!;
        public virtual DbSet<vw_Customer> vw_Customers { get; set; } = null!;
        public virtual DbSet<vw_DataBarChart> vw_DataBarCharts { get; set; } = null!;
        //public virtual DbSet<vw_ElectricityBill> vw_ElectricityBills { get; set; } = null!;
        //public virtual DbSet<vw_HistoryExport> vw_HistoryExports { get; set; } = null!;
        //public virtual DbSet<vw_Income> vw_Incomes { get; set; } = null!;
        //public virtual DbSet<vw_Installment> vw_Installments { get; set; } = null!;
        //public virtual DbSet<vw_MoneyRenewal> vw_MoneyRenewals { get; set; } = null!;
        //public virtual DbSet<vw_MoneyTransfer> vw_MoneyTransfers { get; set; } = null!;
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