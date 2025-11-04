using System;
using System.Collections.Generic;

namespace Lab03_Bai04
{
    [Serializable]
    public class Ticket
    {
        public string SeatNumber { get; set; }
        public double Price { get; set; }
        public bool IsBooked { get; set; }
        public string CustomerName { get; set; }
        public DateTime BookedTime { get; set; }
    }

    [Serializable]
    public class Room
    {
        public string RoomName { get; set; }
        public string MovieName { get; set; }
        public double BasePrice { get; set; }
        public List<Ticket> Tickets { get; set; }
    }

    [Serializable]
    public class TicketMessage
    {
        public string Command { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
        public string MovieName { get; set; }
        public string RoomName { get; set; }
        public string SeatNumber { get; set; }
        public string CustomerName { get; set; }
        public Dictionary<string, List<string>> MovieRoomsData { get; set; }
        public List<string> Movies { get; set; }
        public List<string> Rooms { get; set; }
        public List<Ticket> Tickets { get; set; }
    }
}