using System;
using System.Collections.Generic;

namespace ParkingLot_Better
{
    class Program
    {
        public static Dictionary<ParkingType, int> availableSpace = new Dictionary<ParkingType, int>()
        {
            { ParkingType.Electric, 4 },
            { ParkingType.FourWheeler, 4 },
            { ParkingType.HeavyVehicle, 4 },
            { ParkingType.Scooter, 4 }
        };

        public static Dictionary<Guid, Space> vehicleSpaceMapper = new Dictionary<Guid, Space>();

        static void Main(string[] args)
        {
            Console.WriteLine("Parking Application!");
            while (true)
            {
                Console.WriteLine("Enter your choice");
                Console.WriteLine("1. Park. 2. Remove. 3. Exit 4. Clear 5. Show Parked vehicles list 6. Show Availibilty\n");
                var choice = Convert.ToInt32(Console.ReadLine());
                ParkingLot parking = new ParkingLot();
                switch (choice)
                {
                    case 1:
                        Console.WriteLine("Enter Vehicle Type\n 1. Scooter 2.FourWheeler 3.HeavyVehicle 4.Electric\n");
                        var type = Console.ReadLine();
                        ParkingType parkingType;

                        Enum.TryParse(type, out parkingType);
                        Vehicle vehicle = new Vehicle(Guid.NewGuid());
                        parking.Park(vehicle, parkingType);
                        break;
                    case 2:
                        Console.WriteLine("Enter Vehicle Number to remove");
                        Guid number;
                        Guid.TryParse(Console.ReadLine(), out number);
                        parking.Remove(new Vehicle(number));
                        break;
                    case 3:
                        Environment.Exit(1);
                        break;
                    case 4:
                        Console.Clear();
                        break;
                    case 5:
                        foreach(var i in vehicleSpaceMapper)
                        {
                            Console.WriteLine($" vehicle {i.Key} is of type {i.Value.ParkingType.ToString()} and parked at {i.Value.Floor} floor");
                        }
                        break;
                    case 6:
                        foreach(var i in availableSpace)
                        {
                            Console.WriteLine($"Type {i.Key.ToString()} has {i.Value} parking spaces left");
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        public enum ParkingType
        {
            Scooter = 1,
            FourWheeler = 2,
            HeavyVehicle = 3,
            Electric = 4
        }
        public class Vehicle
        {
            public Guid Number { get; set; }

            public Vehicle(Guid no)
            {
                Number = no;
            }
        }

        public class Space
        {
            public int Floor { get; set; }
            public Vehicle Vehicle { get; set; }
            public ParkingType ParkingType { get; set; }

        }

        public interface IParkingLot
        {
            bool Park(Vehicle vehicle, ParkingType parkingType);
            bool Remove(Vehicle vehicle);
        }

        public class ParkingLot : IParkingLot
        {
            public bool Park(Vehicle vehicle, ParkingType parkingType)
            {
                int available = availableSpace[parkingType];
                if (available > 0)
                {
                    Space space = new Space();
                    space.Vehicle = vehicle;
                    space.Floor = new Random().Next(1, 4);
                    space.ParkingType = parkingType;
                    vehicleSpaceMapper.Add(vehicle.Number, space);
                    availableSpace[parkingType] = available - 1;
                    Console.WriteLine("Added");
                    return true;
                }
                else
                {
                    Console.WriteLine("Parking is Full");
                    return false;
                }
            }

            public bool Remove(Vehicle vehicle)
            {
                var space = vehicleSpaceMapper[vehicle.Number];
                if (space != null)
                {
                    vehicleSpaceMapper.Remove(vehicle.Number);
                    int available = availableSpace[space.ParkingType];
                    availableSpace[space.ParkingType] = available + 1;
                    Console.WriteLine("Removed");
                    return true;
                }
                else
                {
                    Console.WriteLine($"The vehicle number - {vehicle.Number} you are trying to remove is not available");
                    return false;
                }
            }
        }
    }
}
