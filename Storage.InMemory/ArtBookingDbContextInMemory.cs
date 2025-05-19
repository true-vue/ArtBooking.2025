using Business.Model.Entities.Events;
using Business.Model.Entities.Organizations;
using Business.Model.Entities.Pricing;
using Business.Model.Entities.Tickets;
using Business.Model.Entities.Users;
using Business.Model.Entities.Venues;
using Microsoft.EntityFrameworkCore;

namespace Storage.InMemory;

public class ArtBookingDbContextInMemory : DbContext
{
    public ArtBookingDbContextInMemory(DbContextOptions<ArtBookingDbContextInMemory> options) : base(options)
    {

    }

    public DbSet<User> Users { get; set; }
    public DbSet<ArtOrganization> ArtOrganizations { get; set; }
    public DbSet<Venue> Venues { get; set; }
    public DbSet<Area> Areas { get; set; }
    public DbSet<Seat> Seats { get; set; }
    public DbSet<ArtEvent> ArtEvents { get; set; }
    public DbSet<ScheduleItem> ScheduleItems { get; set; }
    public DbSet<PriceList> PriceLists { get; set; }
    public DbSet<PriceEntry> PriceEntries { get; set; }
    public DbSet<Ticket> Tickets { get; set; }
    public DbSet<TicketPayment> TicketPayments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure entity relationships
        modelBuilder.Entity<User>().HasKey(u => u.UserId);
        modelBuilder.Entity<Venue>().HasKey(v => v.VenueId);
        modelBuilder.Entity<Area>().HasKey(a => a.AreaId);
        modelBuilder.Entity<Seat>().HasKey(s => s.SeatId);
        modelBuilder.Entity<ArtEvent>().HasKey(e => e.ArtEventId);
        modelBuilder.Entity<ScheduleItem>().HasKey(s => s.ScheduleItemId);
        modelBuilder.Entity<PriceList>().HasKey(p => p.PriceListId);
        modelBuilder.Entity<PriceEntry>().HasKey(p => p.PriceEntryId);
        modelBuilder.Entity<Ticket>().HasKey(t => t.TicketId);
        modelBuilder.Entity<TicketPayment>().HasKey(t => t.TicketPaymentId);
        modelBuilder.Entity<ArtOrganization>().HasKey(o => o.ArtOrganizationId);

        // Define relationship between ArtOrganization and Users
        modelBuilder.Entity<ArtOrganization>()
            .HasMany(o => o.Users)
            .WithOne(u => u.Organization)
            .HasForeignKey(u => u.ArtOrganizationId)
            .IsRequired(false); // This makes the relationship optional (0 or 1)

        // Define relationship between ArtOrganization and ArtEvents
        modelBuilder.Entity<ArtOrganization>()
            .HasMany(o => o.Events)
            .WithOne(e => e.Organization)
            .HasForeignKey(e => e.ArtOrganizationId)
            .IsRequired();

        // Define relationship between ArtOrganization and Venues
        modelBuilder.Entity<ArtOrganization>()
            .HasMany(o => o.Venues)
            .WithOne(v => v.Organization)
            .HasForeignKey(v => v.ArtOrganizationId)
            .IsRequired();

        // Define relationship between ArtEvent and ScheduleItems
        modelBuilder.Entity<ArtEvent>()
            .HasMany(e => e.ScheduleItems)
            .WithOne(s => s.ArtEvent)
            .HasForeignKey(s => s.ArtEventId)
            .IsRequired();

        // Define relationship between ScheduleItem and Venue
        modelBuilder.Entity<ScheduleItem>()
            .HasOne(s => s.Venue)
            .WithMany(v => v.ScheduleItems)
            .HasForeignKey(s => s.VenueId)
            .IsRequired(false); // Optional relationship - a schedule item might not have a specific venue

        // Define relationship between Venue and Area
        modelBuilder.Entity<Venue>()
            .HasMany(v => v.Areas)
            .WithOne(a => a.Venue)
            .HasForeignKey(a => a.VenueId)
            .IsRequired(); // An area must belong to a venue

        // Define relationship between Area and Seat
        modelBuilder.Entity<Area>()
            .HasMany(a => a.Seats)
            .WithOne(s => s.Area)
            .HasForeignKey(s => s.AreaId)
            .IsRequired(); // A seat must belong to an area

        // Define one-to-many relationship between Venue and PriceList
        modelBuilder.Entity<Venue>()
            .HasMany(v => v.PriceLists)
            .WithOne(p => p.Venue)
            .HasForeignKey(p => p.VenueId)
            .IsRequired(false); // A price list can optionally belong to a venue

        // Define relationship between PriceList and ScheduleItem
        modelBuilder.Entity<PriceList>()
            .HasMany(p => p.ScheduleItems)
            .WithOne(s => s.PriceList)
            .HasForeignKey(s => s.PriceListId)
            .IsRequired(false); // Optional relationship - a schedule item might not have a price list

        // Define relationship between PriceList and PriceEntry
        modelBuilder.Entity<PriceList>()
            .HasMany(p => p.PriceEntries)
            .WithOne(e => e.PriceList)
            .HasForeignKey(e => e.PriceListId)
            .IsRequired(); // A price entry must belong to a price list

        // Define relationship between PriceEntry and Area
        modelBuilder.Entity<PriceEntry>()
            .HasOne(pe => pe.Area)
            .WithMany(a => a.PriceEntries)
            .HasForeignKey(pe => pe.AreaId)
            .IsRequired(); // A price entry must belong to an area

        // Add unique constraint to ensure each PriceEntry for an Area belongs to a different PriceList
        modelBuilder.Entity<PriceEntry>()
            .HasIndex(pe => new { pe.AreaId, pe.PriceListId })
            .IsUnique();

        // TODO: Add check constraint to ensure PriceEntry is only related to Areas from the Venue that PriceList holding price entry is related to.
        // Note: This constraint logic will need additional application-level validation

        // Define relationship between ScheduleItem and Tickets
        modelBuilder.Entity<ScheduleItem>()
            .HasMany(s => s.Tickets)
            .WithOne(t => t.ScheduleItem)
            .HasForeignKey(t => t.ScheduleItemId)
            .IsRequired(false); // A ticket can optionally belong to a schedule item

        // Define relationship between Ticket and Seat
        modelBuilder.Entity<Ticket>()
            .HasOne(t => t.Seat)
            .WithOne(s => s.Ticket)
            .HasForeignKey<Ticket>(t => t.SeatId)
            .IsRequired(false); // A ticket can optionally be assigned to a seat

        // Define relationship between Ticket and Area
        modelBuilder.Entity<Ticket>()
            .HasOne(t => t.Area)
            .WithMany(a => a.Tickets)
            .HasForeignKey(t => t.AreaId)
            .IsRequired(false); // A ticket can optionally be assigned to an area

        // Define relationship between Ticket and TicketPayment
        modelBuilder.Entity<Ticket>()
            .HasMany(t => t.Payments)
            .WithOne(p => p.Ticket)
            .HasForeignKey(p => p.TicketId)
            .IsRequired(); // A payment must be associated with a ticket

        // TODO: Add check to ensure that if a ticket has no seat, it must have an area
        // TODO: Add check to ensure that if a ticket has a seat, its area matches the seat's area
        // TODO: Add check to ensure that that new payment can't be created if ticket is already paid
    }
}