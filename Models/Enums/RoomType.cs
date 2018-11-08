using System.ComponentModel.DataAnnotations;

namespace ISA.Enums
{
    public enum RoomType
    {
        [Display(Name = "Double Room")]
        DoubleRoom = 0,

        [Display(Name = "Single Room")]
        SingleRoom = 1,

        [Display(Name = "Lux suite: King Bed")]
        KingBed = 2
    }
}