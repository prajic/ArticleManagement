using ArticlesManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;


namespace ArticlesManagement.Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<VerificationCode> VerificationCodes { get; set; }

        public DbSet<Author> Authors { get; set; } 
        
        public DbSet<Article> Articles { get; set; }

        public DbSet<ArticleOwner> ArticleOwners { get; set; }

        public DbSet<ArticleLike> ArticleLikes { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<VerificationCode>()
                .HasKey(vc => new { vc.UserId, vc.Code }); 

            modelBuilder.Entity<Author>()
                .HasKey(a => a.UserId); 

            modelBuilder.Entity<Author>()
                .HasOne(a => a.User)
                .WithOne(u => u.Author)
                .HasForeignKey<Author>(a => a.UserId); 

            modelBuilder.Entity<Article>()
                .Property(a => a.Type)
                .HasConversion<int>();

            modelBuilder.Entity<ArticleOwner>()
                .HasKey(ao => new { ao.ArticleId, ao.UserId });

            modelBuilder.Entity<ArticleOwner>()
                 .HasOne(ao => ao.Article)
                 .WithMany(a => a.ArticleOwners)
                 .HasForeignKey(ao => ao.ArticleId);

            modelBuilder.Entity<ArticleOwner>()
                .HasOne(ao => ao.User)
                .WithMany(u => u.ArticleOwners)
                .HasForeignKey(ao => ao.UserId);

            modelBuilder.Entity<ArticleLike>()
                .HasKey(al => new {al.ArticleId, al.UserId });
        }
    }

}