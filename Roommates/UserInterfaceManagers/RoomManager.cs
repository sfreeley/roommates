using System;
using System.Collections.Generic;
using Roommates.Models;
using System.Text;
using Roommates.Repositories;
using System.Data.Common;

namespace Roommates.UserInterfaceManagers
{
    class RoomManager : IUserInterfaceManager
    {
        private readonly IUserInterfaceManager _parentUI;

        private RoomRepository _roomRepository;

        private string _connectionString;

        public RoomManager(IUserInterfaceManager parentUI, string connectionString)
        {
            _parentUI = parentUI;
            _roomRepository = new RoomRepository(connectionString);
            _connectionString = connectionString;
        }

        public IUserInterfaceManager Execute()
        {
            Console.WriteLine("Room Menu");
            Console.WriteLine(" 1) List Rooms");
            Console.WriteLine(" 2) Add A Room");
            Console.WriteLine(" 3) Edit A Room");
            Console.WriteLine(" 4) Remove A Room");
            Console.WriteLine(" 5) Room Details");
            Console.WriteLine(" 0) Go Back");

            Console.Write("> ");
            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    List();
                    return this;
                case "2":
                    Add();
                    return this;
                case "3":
                    Edit();
                    return this;
                case "4":
                    Remove();
                    return this;
                case "0":
                    return _parentUI;
                default:
                    Console.WriteLine("Invalid Selection");
                    return this;
            }
        }

        private void List()
        {
            List<Room> rooms = _roomRepository.GetAll();

            foreach (Room room in rooms)
            {
                Console.WriteLine($"{room.Name} \n Max Occupancy: {room.MaxOccupancy}");
                Console.WriteLine("-----------------------");
            }
        }

        private Room ChooseRoom(string prompt = null)
        {
            if (prompt == null)
            {
                prompt = "Please choose a Room";
            }

            Console.WriteLine(prompt);

            List<Room> rooms = _roomRepository.GetAll();
            for (int i = 0; i < rooms.Count; i++)
            {
                Room room = rooms[i];
                Console.WriteLine($"{i + 1} {room.Name}");
            }
            Console.Write("> ");

            //reads the user's room input
            string input = Console.ReadLine();
            try
            {
                //if the choice is not valid choice and throws an exception (ie not in the list of rooms or not a valid int)
                int choice = int.Parse(input);
                //if valid choice, return the value of rooms list at particular index - 1
                return rooms[choice - 1];
            }
            catch (Exception ex)
            {
                //catch this exception and display this message and return null
                Console.WriteLine("Invalid Selection");
                return null;
            }
        }

        private void Add()
        {
            Room room = new Room();
            string maxOcc;
            int respMaxOcc;
            do
            {
                Console.WriteLine("New Room");

                Console.Write("Room Name: ");
                room.Name = Console.ReadLine();

                Console.Write("Max Occupancy: ");
                maxOcc = Console.ReadLine();
                
            }
            while (string.IsNullOrWhiteSpace(room.Name) || !Int32.TryParse(maxOcc, out respMaxOcc));

            room.MaxOccupancy = respMaxOcc;
            _roomRepository.Insert(room);
        }
           
            
        private void Edit()
        {
            Room roomToEdit = ChooseRoom("Which room would you like to edit?");
            if (roomToEdit == null)
            {
                return;
            }

            Console.WriteLine();
            Console.Write("New Room Name (blank to leave unchanged): ");
            string name = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(name))
            {
                roomToEdit.Name = name;
            }
            Console.Write("New Max Occupancy (blank to leave unchanged): ");
            string maxOccupancy = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(maxOccupancy))
            {
                roomToEdit.MaxOccupancy = int.Parse(maxOccupancy);
            }
            
            _roomRepository.Update(roomToEdit);
        }

        private void Remove()
        {
            Room roomToDelete = ChooseRoom("Which room would you like to remove?");
            if (roomToDelete != null)
            {
                _roomRepository.Delete(roomToDelete.Id);
            }
        }

    }
}
