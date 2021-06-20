using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelBookingApi.Models
{
    public class Room
    {
        public int RoomNumber { get; set; }
        public RoomType Type { get; set; }
        public int Occupancy { get; set; }
        public List<Booking> Bookings { get; set; }
    }

    public enum RoomType
    {
        singleRoom = 0,
        doubleRoom = 1,
        deluxeRoom = 2
    }
}
