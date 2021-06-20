using HotelBookingApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelBookingApi.Repositories
{
    public interface IHotelRepository
    {
        // GET
        List<Hotel> GetHotels();
        Hotel GetHotelByName(string hotelName);
        List<Room> GetRooms(string hotelName);
        List<Room> GetAvailableRooms(DateTime checkin, DateTime checkout, string hotelName, int guests);
        Booking GetBookingByRef(string hotelName, int roomNumber, string bookingRef);

        // CREATE
        Booking CreateBooking(Booking booking, string hotelName, int roomNumber);

        // UPDATE
        string UpdateBooking(Booking booking, string hotelName, int roomNumber);

        // DELETE
        string DeleteBooking(string hotelName, int roomNumber, string bookingRef);
    }
}
