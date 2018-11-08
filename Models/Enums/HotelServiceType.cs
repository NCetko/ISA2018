using System.ComponentModel.DataAnnotations;
namespace ISA.Enums
{
    public enum HotelServiceType
    {
        Breakfast = 0,
        [Display(Name = "Hotel restaurant")]
        HotelRestaurant = 1,
        [Display(Name = "Airport transfer")]
        AirportTransfer = 2,
        Pool = 3,
        [Display(Name = "Wellness & Spa")]
        WellnessAndSpa = 3,
        WiFi = 4
    }
}