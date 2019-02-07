using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ISA.Models.Entities;
using Microsoft.AspNetCore.Identity;

namespace ISA.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Administration> Administration { get; set; }
        public DbSet<Airline> Airlines { get; set; }
        public DbSet<AirlineDestination> AirlineDestinations { get; set; }
        public DbSet<Airplane> Airplanes { get; set; }

        public DbSet<Destination> Destinations { get; set; }

        public DbSet<Flight> Flights { get; set; }

        public DbSet<Friendship> Friendships { get; set; }
        //public DbSet<Hotel> Hotels { get; set; }
        //public DbSet<HotelService> HotelServices { get; set; }

        //public DbSet<HotelServiceReservation> HotelServiceReservations { get; set; }

        public DbSet<PointConfiguration> PointConfigurations { get; set; }
        public DbSet<Provider> Providers { get; set; }

        //public DbSet<RAC> RACs { get; set; }
        //public DbSet<RACOffice> RACOffices { get; set; }

        public DbSet<Ratable> Ratables { get; set; }
        public DbSet<Rating> Ratings { get; set; }

        public DbSet<Reservation> Reservations { get; set; }

        //public DbSet<Room> Rooms { get; set; }
        //public DbSet<RoomDiscount> RoomDiscounts { get; set; }
        //public DbSet<RoomPrice> RoomPrices { get; set; }
        //public DbSet<RoomReservation> RoomReservations { get; set; }


        public DbSet<Seat> Seats { get; set; }
        public DbSet<SeatDiscount> SeatDiscounts { get; set; }
        public DbSet<SeatReservation> SeatReservations { get; set; }
        public DbSet<Segment> Segments { get; set; }

        //public DbSet<Vehicle> Vehicles { get; set; }
        //public DbSet<VehicleDiscount> VehicleDiscounts { get; set; }
        //public DbSet<VehicleReservation> VehicleReservations { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            //builder.Entity<Room>().HasKey(a => new { a.HotelName, a.RoomName });
            //builder.Entity<HotelService>().HasKey(a => new { a.HotelName, a.HotelServiceType });
            builder.Entity<Segment>().HasKey(a => new { a.AirplaneName, a.SegmentName });
            builder.Entity<Seat>().HasKey(a => new { a.AirplaneName, a.SegmentName, a.SeatName });
            builder.Entity<AirlineDestination>().HasKey(a => new { a.AirlineName, a.DestinationName });
            //builder.Entity<Vehicle>().HasKey(a => new { a.RACName, a.VehicleName });
            //builder.Entity<RACOffice>().HasKey(a => new { a.RACName, a.Address });
            builder.Entity<Friendship>().HasKey(a => new { a.SenderId, a.ReceiverId });
            builder.Entity<Administration>().HasKey(a => new { a.ApplicationUserId, a.ProviderId });
            builder.Entity<SeatDiscount>().HasKey(a => new { a.FlightName, a.AirplaneName, a.SegmentName, a.SeatName });
            builder.Entity<SeatReservation>().HasKey(a => new { a.FlightName, a.AirplaneName, a.SegmentName, a.SeatName });

            builder.Entity<SeatDiscount>().HasAlternateKey(a => new { a.DiscountId });
            builder.Entity<SeatReservation>().HasAlternateKey(a => new { a.SeatReservationId });

            builder.Entity<SeatReservation>()
            .HasOne(m => m.Seat)
            .WithOne(t => t.SeatReservation)
            .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<SeatReservation>()
            .HasOne(m => m.Flight)
            .WithMany(t => t.SeatReservations)
            .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<SeatDiscount>()
            .HasOne(m => m.Seat)
            .WithOne(t => t.SeatDiscount)
            .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<SeatDiscount>()
            .HasOne(m => m.Flight)
            .WithMany(t => t.SeatDiscounts)
            .OnDelete(DeleteBehavior.Restrict);


            builder.Entity<Rating>()
            .HasOne(m => m.Reservation)
            .WithMany(t => t.Ratings)
            .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Friendship>()
            .HasOne(m => m.Sender)
            .WithMany(t => t.SentRequests)
            .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Friendship>()
            .HasOne(m => m.Receiver)
            .WithMany(t => t.ReceivedRequests)
            .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Flight>()
            .HasOne(m => m.DepartureLocation)
            .WithMany(t => t.Departures)
            .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Flight>()
            .HasOne(m => m.ArrivalLocation)
            .WithMany(t => t.Arrivals)
            .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(builder);
        }
    }
}
