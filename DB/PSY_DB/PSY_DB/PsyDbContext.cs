using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PSY_DB.Tables;

namespace PSY_DB;

public class PsyDbContext : DbContext
{
    public DbSet<TblUserAccount> TblUserAccounts { get; set; }
    public DbSet<TblUserScore> TblUserScores { get; set; }
    public DbSet<TblUserMission> TblUserMissions { get; set; }


    static readonly ILoggerFactory _logger = LoggerFactory.Create(builder => { builder.AddConsole(); });
    public static string ConnectionString = "server=121.190.138.117; port=3306; database=PSY_DB_0002; user=yena94; password=dldPsk123!;";



    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseLoggerFactory(_logger);
        optionsBuilder.UseMySql(ConnectionString, ServerVersion.AutoDetect(ConnectionString));
    }



    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}
