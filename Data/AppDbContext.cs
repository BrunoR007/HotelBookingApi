using Hoteis.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Hoteis.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<ClientUser, IdentityRole, string, IdentityUserClaim<string>,
    IdentityUserRole<string>, IdentityUserLogin<string>, IdentityRoleClaim<string>, IdentityUserToken<string>>(options)
{
    public DbSet<ClientUser> ClientUsers { get; set; } 
    public DbSet<HotelUser> HotelUsers { get; set; } 
    public DbSet<Hotel> Hotels { get; set; }
    public DbSet<Room> Rooms { get; set; }
    public DbSet<Reservation> Reservations { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Configuração das tabelas separadas
        builder.Entity<ClientUser>().ToTable("AspNetClients");
        builder.Entity<HotelUser>().ToTable("AspNetHotelUsers");

        // Configuração das entidades
        builder.Entity<Hotel>().Property(h => h.Name).IsRequired();
        builder.Entity<Hotel>().Property(h => h.Address).IsRequired();
        builder.Entity<ClientUser>().HasIndex(u => u.CPF).IsUnique();
        builder.Entity<HotelUser>().HasIndex(u => u.CNPJ).IsUnique();

        // Relacionamentos
        builder.Entity<Hotel>()
            .HasOne(h => h.HotelUser)
            .WithMany(hu => hu.ManagedHotels)
            .HasForeignKey(h => h.HotelUserId);

        builder.Entity<Room>()
            .HasOne(r => r.Hotel)
            .WithMany(h => h.Rooms)
            .HasForeignKey(r => r.HotelId);

        builder.Entity<Reservation>()
            .HasOne(r => r.ClientUser)
            .WithMany(c => c.Reservations)
            .HasForeignKey(r => r.ClientUserId);

        builder.Entity<Reservation>()
            .HasOne(r => r.Room)
            .WithMany(r => r.Reservations)
            .HasForeignKey(r => r.RoomId);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker.Entries()
            .Where(e => e.Entity is ClientUser || e.Entity is HotelUser || e.Entity is Hotel || e.Entity is Room || e.Entity is Reservation)
            .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Property("CreatedAt").CurrentValue = DateTime.UtcNow;
                entry.Property("ModifiedAt").CurrentValue = DateTime.UtcNow;
            }
            else if (entry.State == EntityState.Modified)
            {
                entry.Property("ModifiedAt").CurrentValue = DateTime.UtcNow;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}