class Hotel
{
    public ICollection<Room> rooms { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public List<Room> Rooms { get; set; }
    public List<Client> Clients { get; set; }
}

class Room
{
    public string Number { get; set; }
    public int Capacity { get; set; }
    public Boolean Occupied { get; set; }
    public List<Reservation> Reservations { get; set; }
}

class Client
{
    public string Name { get; set; }
    public long CreditCard { get; set; }
    public List <Reservation> Reservations { get; set; }
}
class Reservation
{
    public DateTime Date { get; set; }
    public int Occupants { get; set; }
    public bool isCurrent { get; set; }
    public Client client { get; set; }
    public Room room { get; set; }
}

class VIPClient : Client
{
    public int VIPNumber { get; set; }
    public int VIPPoints { get; set; }
}

class PremiumRoom : Room
{
    public string AdditionalAmenities { get; set; }
    public int VIPValue { get; set; }
}