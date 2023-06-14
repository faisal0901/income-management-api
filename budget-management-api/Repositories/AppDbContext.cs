using budget_management_api.Entities;
using budget_management_api.Enum;
using Microsoft.EntityFrameworkCore;

namespace budget_management_api.Repositories;

public class AppDbContext:DbContext
{
        public DbSet<Bill> Bills => Set<Bill>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<SubCategory> SubCategories => Set<SubCategory>();
        public DbSet<Gold> Goldss => Set<Gold>();
        public DbSet<Transactional> Transactionals => Set<Transactional>();
        public DbSet<User> Users => Set<User>();
        public DbSet<Wallet> Wallets => Set<Wallet>();
        public DbSet<Token> Tokens => Set<Token>();
        protected AppDbContext()
        {
                
        }
    
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
                modelBuilder.Entity<Category>()
                        .ToTable("category", t => t.ExcludeFromMigrations());
                modelBuilder.Entity<User>(builder =>
                {
                        builder.HasIndex(user => user.Email).IsUnique();
                        builder.HasIndex(user => user.PhoneNumber).IsUnique();
                });
             
                modelBuilder.Entity<Category>().HasData(
                        new Category {Id = Guid.Parse("4BA71546-54DA-4570-B1A8-2B44DEEFE21D"),CategoryName ="darurat" },
                        new Category {Id = Guid.Parse("75A7D660-4B44-460B-93B1-98873DCF60EA"),CategoryName ="keinginan" },
                        new Category {Id = Guid.Parse("C92984A1-BDE2-4494-B61F-BA9042DCFB68"),CategoryName ="tabungan" },
                        new Category {Id = Guid.Parse("45108869-9225-4D63-BB38-CD50ADE74215"),CategoryName ="kebutuhan"}
                );
                modelBuilder.Entity<SubCategory>().HasData(
                        new SubCategory {Id = Guid.Parse("312896f1-89b7-423d-a680-a19073370ddf"),CategoryName = "makan",CategoryId = Guid.Parse("45108869-9225-4D63-BB38-CD50ADE74215")},
                        new SubCategory {Id = Guid.Parse("7f4034c9-52a5-41f4-b875-42106927b97c"),CategoryName = "listrik",CategoryId = Guid.Parse("45108869-9225-4D63-BB38-CD50ADE74215")},
                        new SubCategory {Id = Guid.Parse("61f36be6-901e-4085-be9d-c3d80aec8122"),CategoryName = "wifi",CategoryId = Guid.Parse("45108869-9225-4D63-BB38-CD50ADE74215")},
                        new SubCategory {Id = Guid.Parse("0635725f-4691-4548-b714-44f1eb865ab4"),CategoryName = "internet",CategoryId = Guid.Parse("45108869-9225-4D63-BB38-CD50ADE74215")},
                        new SubCategory {Id = Guid.Parse("d0e34f8c-cde3-4835-9ed2-3388dcfb8e2e"),CategoryName = "skin care",CategoryId = Guid.Parse("75A7D660-4B44-460B-93B1-98873DCF60EA")},
                        new SubCategory {Id = Guid.Parse("204c3725-b493-492f-b624-071e94d76958"),CategoryName = "coffe",CategoryId = Guid.Parse("75A7D660-4B44-460B-93B1-98873DCF60EA")},
                        new SubCategory {Id = Guid.Parse("637a7428-6be2-4377-90f4-8e9cf054f805"),CategoryName = "netflix",CategoryId = Guid.Parse("75A7D660-4B44-460B-93B1-98873DCF60EA")},
                        new SubCategory {Id = Guid.Parse("03d490f2-b189-41ae-8e08-ebc99b211530"),CategoryName = "vtuber",CategoryId = Guid.Parse("75A7D660-4B44-460B-93B1-98873DCF60EA")},
                        new SubCategory {Id=Guid.Parse("C2A961B2-672F-401D-9169-8538F64742B7"),CategoryName = "emas",CategoryId = Guid.Parse("C92984A1-BDE2-4494-B61F-BA9042DCFB68")},
                        new SubCategory {Id = Guid.Parse("6a27f420-8b50-48df-852c-dd08cfe5c6b8"),CategoryName = "beli obat",CategoryId = Guid.Parse("4BA71546-54DA-4570-B1A8-2B44DEEFE21D")}
                );

                
        }
}