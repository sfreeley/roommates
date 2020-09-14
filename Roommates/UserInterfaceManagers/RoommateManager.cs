using Roommates.Repositories;
using System;
using System.Collections.Generic;
using Roommates.Models;
using System.Text;
using System.Threading;

namespace Roommates.UserInterfaceManagers
{
    class RoommateManager : IUserInterfaceManager
    {
        //private readonly field with type IUserInterfaceManager that will bring user back to designated menu view (based on argument passed in the constructor);
        private readonly IUserInterfaceManager _parentUI;

        //Type Roommate Repository 
        private RoommateRepository _roommateRepository;

        private RoomRepository _roomRepository;

        private string _connectionString;

        // RoommateManager constructor that takes in the parentUI
        public RoommateManager(IUserInterfaceManager parentUI, string connectionString)
        {
            _parentUI = parentUI;
            _roommateRepository = new RoommateRepository(connectionString);
            _roomRepository = new RoomRepository(connectionString);
            _connectionString = connectionString;
        }

        //using this interface method to show user the roommate sub-menu
        public IUserInterfaceManager Execute()
        {
            Console.WriteLine("Roommate Menu");
            Console.WriteLine(" 1) List Roommates");
            Console.WriteLine(" 2) Add A Roommate");
            Console.WriteLine(" 3) Edit A Roommate");
            Console.WriteLine(" 4) Remove A Roommate");
            Console.WriteLine(" 5) Roommate Details");
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
                case "5":
                    //if "5" is chosen, this allows the user to choose the specific person they want to view details for;
                    Roommate roommate = ChooseRoommate();
                    if (roommate == null)
                    {
                        return this;
                    }
                    else
                    {
                        return new RoommateDetailManager(this, _connectionString, roommate.Id);
                    }
                case "0":
                    return _parentUI;
                default:
                    Console.WriteLine("Invalid Selection");
                    return this;
            }
        }

        //listing the roommates by first and last name and room name ;
        private void List()
        {
            List<Roommate> roommates = _roommateRepository.GetRoommatesWithRoom();
           

            foreach (Roommate roommate in roommates)
            { 
                Console.WriteLine($"{roommate.FullName} \n Assigned Room: {roommate.Room.Name}");
            }
        }

        //returns value of a type Roommate at the particular index that user chooses from the roommates list;
        private Roommate ChooseRoommate(string prompt = null)
        {
            if (prompt == null)
            {
                prompt = "Please choose a Roommate";
            }

            Console.WriteLine(prompt);

            List<Roommate> roommates = _roommateRepository.GetAll();

            for (int i = 0; i < roommates.Count; i++)
            {
                Roommate roommate = roommates[i];
                Console.WriteLine($"{i + 1} {roommate.Firstname} {roommate.Lastname}");
            }
            Console.Write("> ");

            // reads the user's roommate input
            string input = Console.ReadLine();
            try
            {
                // if the choice is not valid choice and throws an exception (ie not in the list of roommates or not a valid int)
                // will trigger catch
                // if able to parse this will parse the string choice into proper int
                int choice = int.Parse(input);
                // return the value of roommates list at that particular index - 1 since added 1 to i previously in order to not show user 0 as option;
                return roommates[choice - 1];
            }
            catch (Exception ex)
            {
                //c atch this exception and display this message and return null
                Console.WriteLine("Invalid Selection");
                return null;
            }
        }

        //returns value of type Room at the particular index that user chooses from the rooms list
        private Room ChooseRoom(string prompt = null)
        {
            if (prompt == null)
            {
                prompt = "Please choose a Room";
            }

            Console.WriteLine(prompt);

            RoomRepository roomRepository = new RoomRepository(_connectionString);
            List<Room> rooms = roomRepository.GetAll();

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
                //if the choice is not valid choice and throws an exception (ie not in the list of rooms or not a valid input)
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

        // method to add a roommate;
        private void Add()
        {
            DateTime respDate;
            int respPortion;
            //create new instance of Room that is set to the value of the user's choice from method ChooseRoom() below;
            Room newRoom;
            string rentPortion;
            string moveInDate;
            //create new instance of a roommate
            Roommate roommate= new Roommate();
            
            do
            {
            Console.WriteLine("New Roommate");

            Console.Write("First Name: ");
            roommate.Firstname = Console.ReadLine();

            Console.Write("Last Name: ");
            roommate.Lastname = Console.ReadLine();

            Console.Write("RentPortion: ");
            rentPortion = Console.ReadLine();

            Console.Write("Move-in Date (MM/DD/YYY): ");
            moveInDate = Console.ReadLine();

            newRoom = ChooseRoom();

            }
            while (roommate.Firstname == "" || roommate.Lastname == "" || !Int32.TryParse(rentPortion, out respPortion) || !DateTime.TryParse(moveInDate, out respDate) || newRoom == null);
        
            roommate.RentPortion = respPortion;
            roommate.MoveInDate = respDate;

            //newRoom will hold the value of method from ChooseRoom() where user assigns room to the roommate;
            roommate.Room = newRoom;
            _roommateRepository.Insert(roommate);
        }

        private void Edit()
        {
            Roommate roommateToEdit = ChooseRoommate("Which roommate would you like to edit?");
            if (roommateToEdit == null)
            {
                return;
            }

            Console.WriteLine();
            Console.Write("New first name (blank to leave unchanged): ");
            string firstName = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(firstName))
            {
                roommateToEdit.Firstname = firstName;
            }
            Console.Write("New last name (blank to leave unchanged): ");
            string lastName = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(lastName))
            {
                roommateToEdit.Lastname = lastName;
            }
            Console.Write("New Rent Portion (blank to leave unchanged): ");
            string rentPortion = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(rentPortion))
            {
                try
                {
                    roommateToEdit.RentPortion = int.Parse(rentPortion);
                }
                catch(Exception ex)
                {
                    Console.WriteLine("Invalid Input. Please Try Again.");
                    Execute();
                }
               
            }
            Console.Write("New Move In Date MM-DD-YYYY (blank to leave unchanged): ");
            string moveInDate = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(moveInDate))
            {
                try
                {
                    roommateToEdit.MoveInDate = DateTime.Parse(moveInDate);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Invalid Date Input. Please Try Again.");
                    Execute();
                }
                
            }

            //get all room in list
            Console.Write("Choose a New Room: ");
            
            List<Room> rooms = _roomRepository.GetAll();

            
            //iterate through the the rooms list and display each room's name and index +1;
            for (int i = 0; i < rooms.Count; i++)
            {
                Room room = rooms[i];
                Console.WriteLine($"{i + 1} {room.Name}");
            }
            Console.WriteLine(">");
            //read the user's choice for the room;
            string userRoomChoice = Console.ReadLine();



            //if user does not choose anything, then the current room will stay the same;
            if (!string.IsNullOrWhiteSpace(userRoomChoice))
            {
                //will catch
                try
                {
                    int userRoomChoiceIndex = int.Parse(userRoomChoice);
                    roommateToEdit.Room = rooms[userRoomChoiceIndex - 1];
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Invalid Room Choice. Please Try Again.");
                    Execute();
                }
            }
            else 
            {
                Console.WriteLine("Please choose a valid Room Choice.");
                Execute();
            }

            _roommateRepository.Update(roommateToEdit);
        }

        private void Remove()
        {
            Roommate roommateToDelete = ChooseRoommate("Which Roommate would you like to remove?");
            if (roommateToDelete != null)
            {
                _roommateRepository.Delete(roommateToDelete.Id);
            }
        }

    }
}
