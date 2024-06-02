using AuthMarket.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace AuthMarket.db
{
    public partial class AuthContext : DbContext
    {
        public DbSet<User> users;
        public DbSet<Role> roles;  

        private string _connectionString;

        public AuthContext(DbContextOptions<AuthContext> dbContextOptions) : base(dbContextOptions)
        {
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(ent =>
            {
                ent.HasKey(x => x.Id).HasName("users_key");
                ent.HasIndex(x => x.Email).IsUnique();

                ent.ToTable("users");

                ent.Property(e => e.Id).HasColumnName("id");
                ent.Property(e => e.Email).HasMaxLength(255).HasColumnName("name");
                ent.Property(e => e.Password).HasMaxLength(255).HasColumnName("password");
                ent.Property(e => e.Salt).HasColumnName("salt");
                //ent.Property(e => e.RoleId).HasConversion<int>();

                ent.HasOne(u => u.Role).WithMany(r => r.Users).HasForeignKey(u => u.RoleID);
            });

            modelBuilder.Entity<Role>().Property(e => e.RoleID).HasConversion<int>();

            modelBuilder.Entity<Role>().HasData(Enum.GetValues(typeof(UserRoleType)).Cast<UserRoleType>().Select(u =>
                new Role()
                {
                    RoleID = u,
                    RoleName = u.ToString(),
                }));
        }
    }
}

