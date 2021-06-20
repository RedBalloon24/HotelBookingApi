using HotelBookingApi.Models;
using HotelBookingApi.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelBookingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelController : Controller
    {
        private readonly IHotelRepository _hotelRepository;
        public HotelController(IHotelRepository hotelRepository)
        {
            _hotelRepository = hotelRepository;
        }

        [HttpGet]
        public List<Hotel> GetHotels()
        {
            return _hotelRepository.GetHotels();
        }

        [HttpGet("{hotelName}")]
        public ActionResult<Hotel> GetHotelByName(string hotelName)
        {
            var hotel = _hotelRepository.GetHotelByName(hotelName);
            if (hotel == null)
                return NotFound();

            return hotel;
        }

        [HttpGet("{hotelName}/rooms")]
        public ActionResult<List<Room>> GetRooms(string hotelName)
        {
            return _hotelRepository.GetRooms(hotelName);
        }

        [HttpGet("{hotelName}/rooms/available/{checkin}-{checkout}-{nbGuests}")]
        public ActionResult<List<Room>> GetavailableRooms(string hotelName, DateTime checkin, DateTime checkout, int nbGuests)
        {
            return _hotelRepository.GetAvailableRooms(checkin, checkout, hotelName, nbGuests);
        }

        [HttpGet("{hotelName}/rooms/{roomNumber}/bookings/{bookingRef}")]
        public ActionResult<Booking> GetBookingById(string hotelName, int roomNumber, string bookingRef)
        {
            return _hotelRepository.GetBookingByRef(hotelName, roomNumber, bookingRef);
        }

        [HttpPost("{hotelName}/rooms/{roomNumber}/bookings/create")]
        public ActionResult<string> CreateBooking(Booking booking, string hotelName, int roomNumber)
        {
            _hotelRepository.CreateBooking(booking, hotelName, roomNumber);
            return "Booking reference: " + booking.BookingRef;
        }

        [HttpPut("{hotelName}/rooms/{roomNumber}/bookings/{bookingRef}")]
        public ActionResult<string> UpdateBooking(Booking booking, string bookingRef, string hotelName, int roomNumber)
        {
            // Check if booking ids match
            if (bookingRef != booking.BookingRef)
                return BadRequest();

            // Get current booking by id
            var currentBooking = _hotelRepository.GetBookingByRef(hotelName, roomNumber, bookingRef);
            if (currentBooking == null)
                return NotFound();

            // Update booking
            return _hotelRepository.UpdateBooking(booking, hotelName, roomNumber);
        }

        [HttpDelete("{hotelName}/rooms/{roomNumber}/bookings/{bookingRef}")]
        public ActionResult<string> DeleteBooking(string hotelName, int roomNumber , string bookingRef)
        {
            // Get current booking by id
            var currentBooking = _hotelRepository.GetBookingByRef(hotelName, roomNumber, bookingRef);
            if (currentBooking == null)
                return NotFound();

            // Deleted booking
            return _hotelRepository.DeleteBooking(hotelName, roomNumber, bookingRef);
        }
    }
}
