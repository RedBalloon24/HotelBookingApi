using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelBookingApi.Models
{
    public class Hotel
    {
        public string HotelName { get; set; }
        public int HotelId { get; set; }
        public List<Room> Rooms { get; set; }
    }
}
