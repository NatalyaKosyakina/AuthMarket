using AuthMarket.Model;
using Microsoft.EntityFrameworkCore;

namespace AuthMarket.db
{
    public partial class AuthContext(DbContextOptions<AuthContext> dbContextOptions) : DbContext(dbContextOptions)
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(ent =>
            {
                ent.HasKey(x => x.Id).HasName("users_key");
                ent.HasIndex(x => x.Email).IsUnique();

                ent.ToTable("users");

                ent.Property(e => e.Id).HasColumnName("id");
                ent.Property(e => e.Email).HasMaxLength(255).HasColumnName("email");
                ent.Property(e => e.Password).HasMaxLength(255).HasColumnName("password");
                ent.Property(e => e.Salt).HasColumnName("salt");

                //ent.HasOne(u => u.Role).WithMany(r => r.Users).HasForeignKey(u => u.RoleID);
                ent.Property(e => e.RoleID).HasConversion<int>();
            });

            modelBuilder.Entity<Role>().Property(e => e.RoleID).HasConversion<int>();

            modelBuilder
                .Entity<Role>().HasData
                (Enum.GetValues(typeof(RoleType))
                .Cast<RoleType>()
                .Select(u => new Role()
                {
                    RoleID = u,
                    RoleName = u.ToString(),
                }));
        }
    }
}

