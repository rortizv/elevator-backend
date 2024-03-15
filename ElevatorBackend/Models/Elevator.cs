namespace ElevatorBackend.Models
{
    public class Elevator
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required int Location { get; set; }
        public required bool DoorsOpen { get; set; }
    }
}
