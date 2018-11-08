using System.ComponentModel.DataAnnotations;
namespace ISA.Enums
{
    public enum RatingType
    {
        Airline = 0,
        Flight = 1,
        Hotel = 2,
        Room = 3,
        [Display(Name = "Rent-a-Car")]
        RAC = 4,
        Vehicle = 5
    }
}