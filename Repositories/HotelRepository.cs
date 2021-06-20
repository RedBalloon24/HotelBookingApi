using HotelBookingApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelBookingApi.Repositories
{
    public class HotelRepository : IHotelRepository
    {
        static List<Hotel> Hotels { get;  }
        static HotelRepository()
        {
            Hotels = new List<Hotel>
            {
                new Hotel {
                    HotelId = 1,
                    HotelName="Best Hotel",
                    Rooms = new List<Room> {
                        new Room { RoomNumber = 1, Type = RoomType.singleRoom, Occupancy = 1},
                        new Room { RoomNumber = 2, Type = RoomType.singleRoom, Occupancy = 1},
                        new Room { RoomNumber = 3, Type = RoomType.doubleRoom, Occupancy = 2, Bookings = new List<Booking> 
                            { new Booking { BookingRef = "abc123def456", Checkin = DateTime.Now, Checkout = DateTime.Now.AddDays(2), Guests = 2} 
                            }, 
                        },
                        new Room { RoomNumber = 4, Type = RoomType.doubleRoom, Occupancy = 2},
                        new Room { RoomNumber = 5, Type = RoomType.deluxeRoom, Occupancy = 4},
                        new Room { RoomNumber = 6, Type = RoomType.deluxeRoom, Occupancy = 4},
                    }
                },
                new Hotel {
                    HotelId = 2,
                    HotelName="Great Hotel",
                    Rooms = new List<Room> {
                        new Room { RoomNumber = 1, Type = RoomType.singleRoom, Occupancy = 1},
                        new Room { RoomNumber = 2, Type = RoomType.singleRoom, Occupancy = 1},
                        new Room { RoomNumber = 3, Type = RoomType.doubleRoom, Occupancy = 2},
                        new Room { RoomNumber = 4, Type = RoomType.doubleRoom, Occupancy = 2},
                        new Room { RoomNumber = 5, Type = RoomType.deluxeRoom, Occupancy = 4},
                        new Room { RoomNumber = 6, Type = RoomType.deluxeRoom, Occupancy = 4, Bookings = new List<Booking>
                            { new Booking { BookingRef = "ghi123jkl456", Checkin = DateTime.Now, Checkout = DateTime.Now.AddDays(1), Guests = 4}
                            }, },
                    }
                },
                new Hotel {
                    HotelId = 3,
                    HotelName="Amazing Hotel",
                    Rooms = new List<Room> {
                        new Room { RoomNumber = 1, Type = RoomType.singleRoom, Occupancy = 1},
                        new Room { RoomNumber = 2, Type = RoomType.singleRoom, Occupancy = 1, Bookings = new List<Booking>
                            { new Booking { BookingRef = "mno123pqr456", Checkin = DateTime.Now, Checkout = DateTime.Now.AddDays(3), Guests = 1}
                            }, },
                        new Room { RoomNumber = 3, Type = RoomType.doubleRoom, Occupancy = 2},
                        new Room { RoomNumber = 4, Type = RoomType.doubleRoom, Occupancy = 2},
                        new Room { RoomNumber = 5, Type = RoomType.deluxeRoom, Occupancy = 4},
                        new Room { RoomNumber = 6, Type = RoomType.deluxeRoom, Occupancy = 4},
                    }
                }
            };
        }

        public List<Hotel> GetHotels()
        {
            return Hotels;
        }

        public Hotel GetHotelByName(string name)
        {
            return Hotels.Find(h => h.HotelName == name);
        }

        public List<Room> GetRooms(string hotelName)
        {
            Hotel hotel = GetHotelByName(hotelName);
            return hotel.Rooms;
        }

        public List<Room> GetAvailableRooms(DateTime checkin, DateTime checkout, string hotelName, int guests)
        {
            Hotel hotel = Hotels.Find(h => h.HotelName == hotelName);
            List<Room> rooms = hotel.Rooms.FindAll(r => r.Occupancy <= guests);
            foreach (Room room in rooms)
            {
                // Filter by availability
                if (room.Bookings == null)
                    continue;

                room.Bookings = room.Bookings.FindAll(b => b.Checkin != checkin && b.Checkin != checkout || b.Checkout != checkin && b.Checkout != checkout);
            }

            return rooms;
        }

        public Booking GetBookingByRef(string hotelName, int roomNumber, string bookingRef)
        {
            Hotel hotel = Hotels.Find(h => h.HotelName == hotelName);
            Room room = hotel.Rooms.Find(r => r.RoomNumber == roomNumber);
            if (room.Bookings == null)
                room.Bookings = new List<Booking>();
            return room.Bookings.Find(b => b.BookingRef == bookingRef);
        }

        public Booking CreateBooking(Booking booking, string hotelName, int roomNumber)
        {
            // Check if hotel has available rooms
            List<Room> availableRooms = GetAvailableRooms(booking.Checkin, booking.Checkout, hotelName, booking.Guests);
            if (availableRooms.Count == 0)
                return null;
            
            // Check if room is already booked
            Room room = availableRooms.Find(r => r.RoomNumber == roomNumber);
            if (room == null)
                return null;

            // Make sure guests does not exceed occupancy
            if (booking.Guests > room.Occupancy)
                return null;


            // Add booking to bookings list
            booking.BookingRef = Guid.NewGuid().ToString();
            if (room.Bookings == null)
                room.Bookings = new List<Booking>();
            room.Bookings.Add(booking);
            
            return booking;
        }

        public string UpdateBooking(Booking booking, string hotelName, int roomNumber)
        {
            List<Room> rooms = GetRooms(hotelName);
            Room room = rooms.Find(r => r.RoomNumber == roomNumber);
            int index = room.Bookings.FindIndex(b => b.BookingRef == booking.BookingRef);
            if (index == -1)
                return "Booking not found. Please make sure you have the correct reference number";

            room.Bookings[index] = booking;
            return "Update successfull";
        }

        public string DeleteBooking(string hotelName, int roomNumber, string bookingRef)
        {
            Booking booking = GetBookingByRef(hotelName, roomNumber, bookingRef);
            if (booking != null)
            {
                // Delete booking info from room
                Hotel hotel = Hotels.Find(h => h.HotelName == hotelName);
                Room room = hotel.Rooms.Find(r => r.RoomNumber == roomNumber);

                // Delete booking
                room.Bookings.Remove(booking);
            }

            return "Booking deleted";
        }
    }
}
