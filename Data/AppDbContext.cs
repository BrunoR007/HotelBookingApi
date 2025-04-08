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
    public DbSet<Address> Addresses { get; set; }
    public DbSet<City> Cities { get; set; }
    public DbSet<State> States { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Configuração das tabelas separadas para Identity
        builder.Entity<ClientUser>().ToTable("AspNetClients");
        builder.Entity<HotelUser>().ToTable("AspNetHotelUsers");

        // Relacionamento Hotel e Address (1:1)
        builder.Entity<Hotel>()
            .HasOne(h => h.Address)
            .WithOne()
            .HasForeignKey<Hotel>(h => h.AddressId);

        // Relacionamento Hotel e HotelUser (N:1)
        builder.Entity<Hotel>()
            .HasOne(h => h.HotelUser)
            .WithMany(hu => hu.ManagedHotels)
            .HasForeignKey(h => h.HotelUserId);

        // Relacionamento Address e City (N:1)
        builder.Entity<Address>()
            .HasOne(a => a.City)
            .WithMany()
            .HasForeignKey(a => a.CityId);

        // Relacionamento City e State (N:1)
        builder.Entity<City>()
            .HasOne(c => c.State)
            .WithMany(s => s.Cities)
            .HasForeignKey(c => c.StateId);

        // Relacionamento Room e Hotel (N:1)
        builder.Entity<Room>()
            .HasOne(r => r.Hotel)
            .WithMany(h => h.Rooms)
            .HasForeignKey(r => r.HotelId);

        // Relacionamento Reservation e ClientUser (N:1)
        builder.Entity<Reservation>()
            .HasOne(r => r.ClientUser)
            .WithMany(c => c.Reservations)
            .HasForeignKey(r => r.ClientUserId);

        // Relacionamento Reservation e Room (N:1)
        builder.Entity<Reservation>()
            .HasOne(r => r.Room)
            .WithMany(r => r.Reservations)
            .HasForeignKey(r => r.RoomId);

        // Índices e restrições
        builder.Entity<State>()
            .HasIndex(s => s.Code)
            .IsUnique();

        builder.Entity<City>()
            .HasIndex(c => c.Name);

        builder.Entity<ClientUser>()
            .HasIndex(u => u.CPF)
            .IsUnique();

        builder.Entity<HotelUser>()
            .HasIndex(u => u.CNPJ)
            .IsUnique();

        // Configuração de propriedades obrigatórias
        builder.Entity<Hotel>()
            .Property(h => h.Name)
            .IsRequired();

        builder.Entity<Hotel>()
            .Property(h => h.AddressId)
            .IsRequired();

        builder.Entity<Address>()
            .Property(a => a.Street)
            .IsRequired();

        builder.Entity<Address>()
            .Property(a => a.Number)
            .IsRequired();

        builder.Entity<Address>()
            .Property(a => a.Neighborhood)
            .IsRequired();

        builder.Entity<Address>()
            .Property(a => a.CityId)
            .IsRequired();

        builder.Entity<Address>()
            .Property(a => a.ZipCode)
            .IsRequired();

        builder.Entity<Address>()
            .Property(a => a.Country)
            .IsRequired()
            .HasDefaultValue("Brasil");

        builder.Entity<Room>()
            .Property(r => r.RoomNumber)
            .IsRequired();

        builder.Entity<Room>()
            .Property(r => r.Type)
            .IsRequired();

        builder.Entity<Room>()
            .Property(r => r.Description)
            .IsRequired();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker.Entries()
            .Where(e => e.Entity is BaseClass || e.Entity is ClientUser || e.Entity is HotelUser)
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