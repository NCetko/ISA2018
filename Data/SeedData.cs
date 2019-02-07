using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using ISA.Models.Entities;


// dotnet aspnet-codegenerator razorpage -m Contact -dc ApplicationDbContext -udl -outDir Pages\Contacts --referenceScriptLibraries

namespace ISA.Data
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider, IConfiguration Configuration)
        {
            using (var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
                if (context.Users.Any())
                {
                    return; //Database was already seeded
                }

                SeedRoles(serviceProvider, Configuration).Wait();
                SeedPointConfigurations(context);
                SeedDestinations(context);
                //SeedHotel(context);
                //SeedRAC(context);
                SeedAirline(context);
                SeedAdmins(serviceProvider, Configuration, context).Wait();
                SeedUsers(serviceProvider, Configuration, context).Wait();
                SeedReservationsAndRatings(context);
                context.SaveChanges();
            }
        }

        private static async Task SeedRoles(IServiceProvider serviceProvider, IConfiguration Configuration)
        {
            using (var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {

                //System.Diagnostics.Debug.WriteLine("Adding roles");
                var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var UserManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

                if (RoleManager.Roles.Any())
                {
                    return; // Already seeded
                }

                //adding customs roles
                string[] roleNames = { "User", "SysAdmin", "HotelAdmin", "RACAdmin", "AirAdmin" };
                IdentityResult roleResult;

                foreach (var roleName in roleNames)
                {
                    //creating the roles and seeding them to the database
                    var roleExist = await RoleManager.RoleExistsAsync(roleName);
                    if (!roleExist)
                    {
                        roleResult = await RoleManager.CreateAsync(new IdentityRole(roleName));
                    }
                }
            }
        }

        public static void SeedPointConfigurations(ApplicationDbContext context)
        {
            if (context.PointConfigurations.Any())
            {
                return;   // DB has been seeded
            }
            context.PointConfigurations.AddRange(
                new PointConfiguration
                {
                    Key = "POINT_PER_KM",
                    Value = 1
                },
                new PointConfiguration
                {
                    Key = "POINT_VALUE",
                    Value = 1
                },
                new PointConfiguration
                {
                    Key = "DISCOUNT_WITH_HOTEL",
                    Value = 5
                }
            );

            context.SaveChanges();
        }

        private static void SeedDestinations(ApplicationDbContext context)
        {
            context.Destinations.AddRange(
                new Destination
                {
                    DestinationName = "Belgrade",
                    Address = "Belgrade Nikola Tesla Airport, Aerodrom Beograd 59, Beograd 11180, Serbia"
                },
                new Destination
                {
                    DestinationName = "Budapest",
                    Address = "Budapest, Budapest Airport (BUD), 1185 Hungary"
                },
                new Destination
                {
                    DestinationName = "Vienna",
                    Address = "Vienna International Airport (VIE), Wien-Flughafen, 1300 Schwechat, Austria"
                }
            );

            context.SaveChanges();
        }

        /*
        public static void SeedHotel(ApplicationDbContext context)
        {

            if (context.Hotels.Any())
            {
                return;   // DB has been seeded
            }

            //Hotels
            {
                context.Hotels.AddRange(
                    new Hotel
                    {
                        HotelName = "Belgrade 1",
                        Address = "Bulevar Zorana Đinđića 65, Beograd 11070, Serbia",
                        Description = "Some text 1",
                        Destination = context.Destinations.Find("Belgrade")
                    },
                    new Hotel
                    {
                        HotelName = "Budapest 1",
                        Address = "Budapest, Haller u. 62, 1096 Hungary",
                        Description = "Some text 1",
                        Destination = context.Destinations.Find("Budapest")
                    },
                    new Hotel
                    {
                        HotelName = "Vienna 1",
                        Address = "Stephansplatz 2, 1010 Wien, Austria",
                        Description = "Some text 1",
                        Destination = context.Destinations.Find("Vienna")
                    }
                );

                context.SaveChanges();
            }

            //Hotel Services
            {
                context.HotelServices.AddRange(
                    new HotelService
                    {
                        Hotel = context.Hotels.Find("Belgrade 1"),
                        HotelServiceType = Enums.HotelServiceType.WiFi,
                        Price = 2
                    },
                    new HotelService
                    {
                        Hotel = context.Hotels.Find("Budapest 1"),
                        HotelServiceType = Enums.HotelServiceType.WiFi,
                        Price = 2
                    },
                    new HotelService
                    {
                        Hotel = context.Hotels.Find("Vienna 1"),
                        HotelServiceType = Enums.HotelServiceType.WiFi,
                        Price = 2
                    }
                );
                context.SaveChanges();
            }

            //Rooms
            {
                context.Rooms.AddRange(
                    new Room
                    {
                        Balcony = true,
                        RoomName = "1A",
                        Capacity = 2,
                        Floor = 2,
                        Hotel = context.Hotels.Find("Belgrade 1"),
                        Type = Enums.RoomType.SingleRoom
                    },

                    new Room
                    {
                        Balcony = false,
                        RoomName = "2A",
                        Capacity = 6,
                        Floor = 2,
                        Hotel = context.Hotels.Find("Belgrade 1"),
                        Type = Enums.RoomType.DoubleRoom
                    },
                    new Room
                    {
                        Balcony = true,
                        RoomName = "3A",
                        Capacity = 2,
                        Floor = 2,
                        Hotel = context.Hotels.Find("Belgrade 1"),
                        Type = Enums.RoomType.KingBed
                    },
                    new Room
                    {
                        Balcony = true,
                        RoomName = "1A",
                        Capacity = 2,
                        Floor = 2,
                        Hotel = context.Hotels.Find("Budapest 1"),
                        Type = Enums.RoomType.SingleRoom
                    },

                    new Room
                    {
                        Balcony = false,
                        RoomName = "2A",
                        Capacity = 6,
                        Floor = 2,
                        Hotel = context.Hotels.Find("Budapest 1"),
                        Type = Enums.RoomType.DoubleRoom
                    },
                    new Room
                    {
                        Balcony = true,
                        RoomName = "3A",
                        Capacity = 2,
                        Floor = 2,
                        Hotel = context.Hotels.Find("Budapest 1"),
                        Type = Enums.RoomType.KingBed
                    },
                    new Room
                    {
                        Balcony = true,
                        RoomName = "1A",
                        Capacity = 2,
                        Floor = 2,
                        Hotel = context.Hotels.Find("Vienna 1"),
                        Type = Enums.RoomType.SingleRoom
                    },

                    new Room
                    {
                        Balcony = false,
                        RoomName = "2A",
                        Capacity = 6,
                        Floor = 2,
                        Hotel = context.Hotels.Find("Vienna 1"),
                        Type = Enums.RoomType.DoubleRoom
                    },
                    new Room
                    {
                        Balcony = true,
                        RoomName = "3A",
                        Capacity = 2,
                        Floor = 2,
                        Hotel = context.Hotels.Find("Vienna 1"),
                        Type = Enums.RoomType.KingBed
                    }
                );
                context.SaveChanges();
            }

            //Room prices
            {
                context.RoomPrices.AddRange(
                    new RoomPrice
                    {
                        Price = 20,
                        Room = context.Rooms.Find("Belgrade 1", "1A"),
                        StartDate = new DateTime(2016, 1, 1, 0, 0, 0),
                        EndDate = new DateTime(2019, 1, 1, 0, 0, 0)
                    },
                    new RoomPrice
                    {
                        Price = 30,
                        Room = context.Rooms.Find("Belgrade 1", "2A"),
                        StartDate = new DateTime(2016, 1, 1, 0, 0, 0),
                        EndDate = new DateTime(2020, 1, 1, 0, 0, 0)
                    },
                    new RoomPrice
                    {
                        Price = 40,
                        Room = context.Rooms.Find("Belgrade 1", "3A"),
                        StartDate = new DateTime(2016, 1, 1, 0, 0, 0),
                        EndDate = new DateTime(2020, 1, 1, 0, 0, 0)
                    },
                    new RoomPrice
                    {
                        Price = 20,
                        Room = context.Rooms.Find("Budapest 1", "1A"),
                        StartDate = new DateTime(2016, 1, 1, 0, 0, 0),
                        EndDate = new DateTime(2019, 1, 1, 0, 0, 0)
                    },
                    new RoomPrice
                    {
                        Price = 30,
                        Room = context.Rooms.Find("Budapest 1", "2A"),
                        StartDate = new DateTime(2016, 1, 1, 0, 0, 0),
                        EndDate = new DateTime(2020, 1, 1, 0, 0, 0)
                    },
                    new RoomPrice
                    {
                        Price = 40,
                        Room = context.Rooms.Find("Budapest 1", "3A"),
                        StartDate = new DateTime(2016, 1, 1, 0, 0, 0),
                        EndDate = new DateTime(2020, 1, 1, 0, 0, 0)
                    },
                    new RoomPrice
                    {
                        Price = 20,
                        Room = context.Rooms.Find("Vienna 1", "1A"),
                        StartDate = new DateTime(2016, 1, 1, 0, 0, 0),
                        EndDate = new DateTime(2019, 1, 1, 0, 0, 0)
                    },
                    new RoomPrice
                    {
                        Price = 30,
                        Room = context.Rooms.Find("Vienna 1", "2A"),
                        StartDate = new DateTime(2016, 1, 1, 0, 0, 0),
                        EndDate = new DateTime(2020, 1, 1, 0, 0, 0)
                    },
                    new RoomPrice
                    {
                        Price = 40,
                        Room = context.Rooms.Find("Vienna 1", "3A"),
                        StartDate = new DateTime(2016, 1, 1, 0, 0, 0),
                        EndDate = new DateTime(2020, 1, 1, 0, 0, 0)
                    }
                );

                context.SaveChanges();
            }
        }
        
        public static void SeedRAC(ApplicationDbContext context)
        {
            if (context.RACs.Any())
            {
                return;   // DB has been seeded
            }

            //Rent-A-Car Businesses
            {
                context.RACs.AddRange(
                    new RAC
                    {
                        RACName = "Rac Belgrade",
                        Description = "Some text 1",
                        Destination = context.Destinations.Find("Belgrade")
                    },
                    new RAC
                    {
                        RACName = "Rac Budapest",
                        Description = "Some text 1",
                        Destination = context.Destinations.Find("Budapest")
                    },
                    new RAC
                    {
                        RACName = "Rac Vienna",
                        Description = "Some text 1",
                        Destination = context.Destinations.Find("Vienna")
                    }
                );

                context.SaveChanges();
            }

            //Vehicles
            {
                context.Vehicles.AddRange(
                    new Vehicle
                    {
                        RAC = context.RACs.Find("Rac Belgrade"),
                        VehicleName = "Opel Astra G - BG1",
                        Doors = 3,
                        Passengers = 5,
                        Price = 15,
                        TruncVolume = 2
                    },
                    new Vehicle
                    {
                        RAC = context.RACs.Find("Rac Budapest"),
                        VehicleName = "Opel Astra G - BUD1",
                        Doors = 3,
                        Passengers = 5,
                        Price = 15,
                        TruncVolume = 2
                    },
                    new Vehicle
                    {
                        RAC = context.RACs.Find("Rac Vienna"),
                        VehicleName = "Opel Astra G - W1",
                        Doors = 3,
                        Passengers = 5,
                        Price = 15,
                        TruncVolume = 2
                    }
                );

                context.SaveChanges();
            }

            //Offices
            {
                context.RACOffices.AddRange(
                    new RACOffice
                    {
                        RAC = context.RACs.Find("Rac Belgrade"),
                        Address = context.Destinations.Find("Belgrade").Address
                    },
                    new RACOffice
                    {
                        RAC = context.RACs.Find("Rac Budapest"),
                        Address = context.Destinations.Find("Budapest").Address
                    },
                    new RACOffice
                    {
                        RAC = context.RACs.Find("Rac Vienna"),
                        Address = context.Destinations.Find("Vienna").Address
                    }
                );

                context.SaveChanges();
            }
        }
        */

        public static void SeedAirline(ApplicationDbContext context)
        {
            context.Airlines.AddRange(
                new Airline
                {
                    AirlineName = "Airline 1",
                    Address = "Antifašističke borbe 2, Beograd 11070, Serbia",
                    Description = "Some text"
                }
            );

            context.SaveChanges();

            context.AirlineDestinations.AddRange(
              new AirlineDestination
              {
                  Airline = context.Airlines.Find("Airline 1"),
                  Destination = context.Destinations.Find("Belgrade")
              },
              new AirlineDestination
              {
                  Airline = context.Airlines.Find("Airline 1"),
                  Destination = context.Destinations.Find("Budapest")
              },
              new AirlineDestination
              {
                  Airline = context.Airlines.Find("Airline 1"),
                  Destination = context.Destinations.Find("Vienna")
              }
          );

            context.SaveChanges();


            context.Airplanes.AddRange(
                new Airplane
                {
                    AirplaneName = "Airplane 1",
                    Airline = context.Airlines.Find("Airline 1")
                }
            );

            context.SaveChanges();

            context.Segments.AddRange(
                new Segment
                {
                    SegmentName = "Economy",
                    Airplane = context.Airplanes.Find("Airplane 1"),
                    Color = "turquoise"
                },
                new Segment
                {
                    SegmentName = "Business",
                    Airplane = context.Airplanes.Find("Airplane 1"),
                    Color = "royalblue"
                }
            );

            context.SaveChanges();

            for (int i = 0; i < 15; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    context.Seats.Add(
                        new Seat
                        {
                            SeatName = (i + 1).ToString() + NumToLetter(j + 1),
                            Segment = context.Segments.Find("Airplane 1", "Economy"),
                            X = 50 + i * 50,
                            Y = 100 + j * 50
                        }
                    );

                    context.Seats.Add(
                        new Seat
                        {
                            SeatName = (i + 1).ToString() + NumToLetter(j + 3 + 1),
                            Segment = context.Segments.Find("Airplane 1", "Economy"),
                            X = 50 + i * 50,
                            Y = 300 + j * 50
                        }
                    );
                }
            }

            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    context.Seats.Add(
                        new Seat
                        {
                            SeatName = (i + 1 + 15).ToString() + NumToLetter(j + 1),
                            Segment = context.Segments.Find("Airplane 1", "Business"),
                            X = 850 + i * 50,
                            Y = 100 + j * 50
                        }
                    );

                    context.Seats.Add(
                        new Seat
                        {
                            SeatName = (i + 1 + 15).ToString() + NumToLetter(j + 3 + 1),
                            Segment = context.Segments.Find("Airplane 1", "Business"),
                            X = 850 + i * 50,
                            Y = 300 + j * 50
                        }
                    );
                }
            }

            context.SaveChanges();

            {
                Flight flight1 = new Flight
                {
                    FlightName = "U2 1001",
                    Airplane = context.Airplanes.Find("Airplane 1"),
                    DepartureLocation = context.AirlineDestinations.Find("Airline 1", "Belgrade"),
                    ArrivalLocation = context.AirlineDestinations.Find("Airline 1", "Budapest"),
                    CarryOnBag = 1,
                    CheckedBag = 1,
                    Price = 90,
                    Departure = new DateTime(2016, 6, 6, 10, 0, 0),
                    Arrival = new DateTime(2016, 6, 6, 11, 0, 0)
                };
                context.Flights.Add(flight1);
                context.SaveChanges();

                Flight flight2 = new Flight
                {
                    FlightName = "U2 1002",
                    Airplane = context.Airplanes.Find("Airplane 1"),
                    DepartureLocation = context.AirlineDestinations.Find("Airline 1", "Budapest"),
                    ArrivalLocation = context.AirlineDestinations.Find("Airline 1", "Vienna"),
                    CarryOnBag = 1,
                    CheckedBag = 1,
                    KM = 214,
                    Price = 90,
                    Departure = new DateTime(2016, 6, 6, 11, 30, 0),
                    Arrival = new DateTime(2016, 6, 6, 12, 30, 0)
                };

                context.Flights.Add(flight2);
                context.SaveChanges();
            }

            {
                Flight flight1 = new Flight
                {
                    FlightName = "U2 1003",
                    Airplane = context.Airplanes.Find("Airplane 1"),
                    DepartureLocation = context.AirlineDestinations.Find("Airline 1", "Vienna"),
                    ArrivalLocation = context.AirlineDestinations.Find("Airline 1", "Budapest"),
                    CarryOnBag = 1,
                    CheckedBag = 1,
                    Price = 90,
                    KM = 319,
                    Departure = new DateTime(2016, 6, 6, 15, 0, 0),
                    Arrival = new DateTime(2016, 6, 6, 16, 0, 0)
                };
                context.Flights.Add(flight1);
                context.SaveChanges();

                Flight flight2 = new Flight
                {
                    FlightName = "U2 1004",
                    Airplane = context.Airplanes.Find("Airplane 1"),
                    DepartureLocation = context.AirlineDestinations.Find("Airline 1", "Budapest"),
                    ArrivalLocation = context.AirlineDestinations.Find("Airline 1", "Belgrade"),
                    CarryOnBag = 1,
                    CheckedBag = 1,
                    Price = 90,
                    KM = 319,
                    Departure = new DateTime(2016, 6, 6, 16, 30, 0),
                    Arrival = new DateTime(2016, 6, 6, 17, 30, 0)
                };

                context.Flights.Add(flight2);
                context.SaveChanges();

                Flight flight3 = new Flight
                {
                    FlightName = "U2 1005",
                    Airplane = context.Airplanes.Find("Airplane 1"),
                    DepartureLocation = context.AirlineDestinations.Find("Airline 1", "Budapest"),
                    ArrivalLocation = context.AirlineDestinations.Find("Airline 1", "Belgrade"),
                    CarryOnBag = 1,
                    CheckedBag = 1,
                    Price = 90,
                    KM = 319,
                    Departure = new DateTime(2016, 6, 7, 16, 30, 0),
                    Arrival = new DateTime(2016, 6, 7, 17, 30, 0)
                };

                context.Flights.Add(flight3);
                context.SaveChanges();
            }
            context.SaveChanges();
        }

        private static async Task SeedAdmins(IServiceProvider serviceProvider, IConfiguration configuration, ApplicationDbContext context)
        {
            using (var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                //System.Diagnostics.Debug.WriteLine("Adding roles");
                var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var UserManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

                {
                    //creating a super user who could maintain the web app
                    string userPassword = "123@Admin";
                    string userEmail = "sysadmin@mail.com";

                    var poweruser = new ApplicationUser
                    {
                        UserName = userEmail,
                        Email = userEmail,
                        FirstName = "admin",
                        LastName = "admin",
                        Address = "Some Address",
                        PhoneNumber = "0123"
                    };

                    var user = await UserManager.FindByEmailAsync(userEmail);

                    if (user == null)
                    {
                        //System.Diagnostics.Debug.WriteLine("Creating user");
                        var createPowerUser = await UserManager.CreateAsync(poweruser, userPassword);
                        if (createPowerUser.Succeeded)
                        {
                            //here we tie the new user to the "SysAdmin" role 
                            await UserManager.AddToRoleAsync(poweruser, "SysAdmin");

                        }
                    }
                }

                {
                    //creating an airline admin
                    string userPassword = "123@Admin";
                    string userEmail = "airadmin@mail.com";

                    var poweruser = new ApplicationUser
                    {
                        UserName = userEmail,
                        Email = userEmail,
                        FirstName = "admin",
                        LastName = "admin",
                        Address = "Some Address",
                        PhoneNumber = "0123"
                    };

                    var user = await UserManager.FindByEmailAsync(userEmail);

                    if (user == null)
                    {
                        //System.Diagnostics.Debug.WriteLine("Creating user");
                        var createPowerUser = await UserManager.CreateAsync(poweruser, userPassword);
                        if (createPowerUser.Succeeded)
                        {
                            //here we tie the new user to the "SysAdmin" role 
                            await UserManager.AddToRoleAsync(poweruser, "AirAdmin");

                        }

                        context.SaveChanges();
                    }
                }

                {
                    //creating a hotel admin
                    string userPassword = "123@Admin";
                    string userEmail = "hoteladmin@mail.com";

                    var poweruser = new ApplicationUser
                    {
                        UserName = userEmail,
                        Email = userEmail,
                        FirstName = "admin",
                        LastName = "admin",
                        Address = "Some Address",
                        PhoneNumber = "0123"
                    };

                    var user = await UserManager.FindByEmailAsync(userEmail);

                    if (user == null)
                    {
                        //System.Diagnostics.Debug.WriteLine("Creating user");
                        var createPowerUser = await UserManager.CreateAsync(poweruser, userPassword);
                        if (createPowerUser.Succeeded)
                        {
                            //here we tie the new user to the "SysAdmin" role 
                            await UserManager.AddToRoleAsync(poweruser, "HotelAdmin");

                        }
                        context.SaveChanges();
                    }
                }

                {
                    //creating a Rent-A-Car admin
                    string userPassword = "123@Admin";
                    string userEmail = "racadmin@mail.com";

                    var poweruser = new ApplicationUser
                    {
                        UserName = userEmail,
                        Email = userEmail,
                        FirstName = "admin",
                        LastName = "admin",
                        Address = "Some Address",
                        PhoneNumber = "0123"
                    };

                    var user = await UserManager.FindByEmailAsync(userEmail);

                    if (user == null)
                    {
                        //System.Diagnostics.Debug.WriteLine("Creating user");
                        var createPowerUser = await UserManager.CreateAsync(poweruser, userPassword);
                        if (createPowerUser.Succeeded)
                        {
                            //here we tie the new user to the "SysAdmin" role 
                            await UserManager.AddToRoleAsync(poweruser, "RACAdmin");

                        }
                    }
                }

                context.SaveChanges();

                context.Administration.AddRange(
                    /*
                    new Administration
                    {
                        ApplicationUser = context.Users.Where(t => t.UserName == "hoteladmin@mail.com").First(),
                        Provider = context.Hotels.Find("Belgrade 1").Provider
                    },

                    new Administration
                    {
                        ApplicationUser = context.Users.Where(t => t.UserName == "racadmin@mail.com").First(),
                        Provider = context.RACs.Find("Rac Belgrade").Provider
                    },*/
                    new Administration
                    {
                        ApplicationUser = context.Users.Where(t => t.UserName == "airadmin@mail.com").First(),
                        Provider = context.Airlines.Find("Airline 1").Provider
                    }
                );
            }
        }

        private static async Task SeedUsers(IServiceProvider serviceProvider, IConfiguration configuration, ApplicationDbContext context)
        {
            using (var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var UserManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

                {
                    //creating a customer
                    string userPassword = "123@User";
                    string userEmail = "user1@mail.com";

                    var poweruser = new ApplicationUser
                    {
                        UserName = userEmail,
                        Email = userEmail,
                        FirstName = "User1",
                        LastName = "User1",
                        Address = "Some Address",
                        PhoneNumber = "0123"
                    };

                    var user = await UserManager.FindByEmailAsync(userEmail);

                    if (user == null)
                    {
                        //System.Diagnostics.Debug.WriteLine("Creating user");
                        var createPowerUser = await UserManager.CreateAsync(poweruser, userPassword);
                        if (createPowerUser.Succeeded)
                        {
                            //here we tie the new user to the "SysAdmin" role 
                            await UserManager.AddToRoleAsync(poweruser, "User");

                        }
                    }

                }

                {
                    //creating a customer
                    string userPassword = "123@User";
                    string userEmail = "user2@mail.com";

                    var poweruser = new ApplicationUser
                    {
                        UserName = userEmail,
                        Email = userEmail,
                        FirstName = "User2",
                        LastName = "User2",
                        Address = "Some Address",
                        PhoneNumber = "0123"
                    };

                    var user = await UserManager.FindByEmailAsync(userEmail);

                    if (user == null)
                    {
                        //System.Diagnostics.Debug.WriteLine("Creating user");
                        var createPowerUser = await UserManager.CreateAsync(poweruser, userPassword);
                        if (createPowerUser.Succeeded)
                        {
                            //here we tie the new user to the "SysAdmin" role 
                            await UserManager.AddToRoleAsync(poweruser, "User");

                        }
                    }
                }

                context.SaveChanges();

                context.Friendships.Add(
                    new Friendship
                    {
                        Approved = true,
                        Created = new DateTime(2016, 1, 1, 0, 0, 0),
                        Sender = context.Users.Where(t => t.UserName == "user1@mail.com").First(),
                        Receiver = context.Users.Where(t => t.UserName == "user2@mail.com").First()
                    }
                );

                context.SaveChanges();
            }
        }

        private static void SeedReservationsAndRatings(ApplicationDbContext context)
        {
            {
                Reservation reservation = new Reservation
                {
                    Created = new DateTime(2016, 1, 1, 0, 0, 0),
                    ApplicationUser = context.Users.Where(t => t.UserName == "user1@mail.com").First(),
                    TotalPrice = 50,
                    Airline = context.Airlines.Find("Airline 1")
                };

                context.Reservations.Add(reservation);
                context.SaveChanges();

                SeatDiscount seatDiscount = new SeatDiscount
                {
                    Passport = "123",
                    Price = 50,
                    Reservation = reservation,
                    Flight = context.Flights.Find("U2 1005"),
                    Seat = context.Seats.Find("Airplane 1", "Economy", "1A")
                };

                context.SeatDiscounts.Add(seatDiscount);
                context.SaveChanges();

                context.Ratings.AddRange(
                    new Rating()
                    {
                        Value = 5,
                        Ratable = seatDiscount.Flight.Ratable,
                        Reservation = reservation,
                        ApplicationUser = context.Users.Where(t => t.UserName == "user1@mail.com").First()
                    },
                    new Rating()
                    {
                        Value = 5,
                        Ratable = seatDiscount.Flight.Airplane.Airline.Ratable,
                        Reservation = reservation,
                        ApplicationUser = context.Users.Where(t => t.UserName == "user1@mail.com").First()
                    }
                );

                context.SaveChanges();
            }
        }

        private static String NumToLetter(int broj)

        {

            Char c = (Char)(65 + (broj - 1));

            return c.ToString();

        }
    }
}