using Microsoft.EntityFrameworkCore;
using SeeSharp.Egzaminer.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using SeeSharp.Egzaminer.Infrastructure.Persistence.EntityConfigurations;
using System.Reflection.Emit;

namespace SeeSharp.Egzaminer.Infrastructure.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options)
    : IdentityDbContext<ApplicationUser>(options)
{
    public virtual DbSet<ApplicationUser> Users { get; set; }
    public DbSet<ManualGradingResult> ManualGradingResults { get; set; }
    public DbSet<AnswerSubmitted> AnswerSubmitted { get; set; }


    //publikacja testów
    public virtual DbSet<TestPublication> TestPublications { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfiguration(new AccessTokenEntityMap());
        builder.ApplyConfiguration(new AnswerSubmittedEntityMap());
        builder.ApplyConfiguration(new GlobalSettingEntityMap());
        builder.ApplyConfiguration(new QuestionEntityMap()); 
        builder.ApplyConfiguration(new SystemLogEntityMap());
        builder.ApplyConfiguration(new TestEntityMap());
        builder.ApplyConfiguration(new TestPublicationEntityMap());
        builder.ApplyConfiguration(new ManualGradingResultEntityMap());
    }

}
