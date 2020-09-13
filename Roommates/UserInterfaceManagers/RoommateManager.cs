﻿using Roommates.Repositories;
using System;
using System.Collections.Generic;
using Roommates.Models;
using System.Text;
using System.Threading;

namespace Roommates.UserInterfaceManagers
{
    class RoommateManager : IUserInterfaceManager
    {
        //private readonly field with type IUserInterfaceManager that will bring user back to main menu (based on argument passed in the constructor);
        private readonly IUserInterfaceManager _parentUI;

        //Type Roommate Repository 
        public RoommateRepository _roommateRepository;

        private string _connectionString;

        // RoommateManager constructor that takes in the parentUI
        public RoommateManager(IUserInterfaceManager parentUI, string connectionString)
        {
            _parentUI = parentUI;
            _roommateRepository = new RoommateRepository(connectionString);
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
                case "0":
                    return _parentUI;
                default:
                    Console.WriteLine("Invalid Selection");
                    return this;
            }
        }

        //listing the roommates by first and last name;
        private void List()
        {
            List<Roommate> roommates = _roommateRepository.GetAll();

            foreach (Roommate roommate in roommates)
            { 
                Console.WriteLine($"{roommate.Firstname} {roommate.Lastname}");
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
                // this will parse the string choice into proper int
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

            List<Room> rooms = _roomRepository.GetAll();

            for (int i = 0; i < rooms.Count; i++)
            {
                Roommate room = rooms[i];
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

        // method to add a roommate;
        private void Add()
        {
            DateTime respDate;
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
                roommate.RentPortion = int.Parse(Console.ReadLine());

                Console.Write("Move-in Date (MM/DD/YYY): ");
                moveInDate = Console.ReadLine();
                roommate.MoveInDate = DateTime.Parse(moveInDate);
            }
            while (roommate.Firstname == "" || roommate.Lastname == "" || roommate.RentPortion < 0 || !DateTime.TryParse(moveInDate, out respDate));

            //create new instance of Room that is set to the value of the user's choice from method ChooseRoom();
            Room newRoom = ChooseRoom();
            while (newRoom == null)
            {
                Console.WriteLine("Choose a Room");
            }


            roommate.Room = newRoom;
            _roommateRepository.Insert(roommate);
        }

    }
}
