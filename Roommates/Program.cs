﻿using System;
using System.Collections.Generic;
using Roommates.Models;
using Roommates.UserInterfaceManagers;
using Roommates.Repositories;

namespace Roommates
{
    class Program
    {
        /// <summary>
        ///  This is the address of the database.
        ///  We define it here as a constant since it will never change.
        /// </summary>
        //private const string CONNECTION_STRING = @"server=localhost\SQLExpress;database=Roommates;integrated security=true";

        static void Main(string[] args)
        {
            // MainMenuManager implements the IUserInterfaceManager interface
            IUserInterfaceManager ui = new MainMenuManager();
            while (ui != null)
            {
                // Each call to Execute will return the next IUserInterfaceManager we should execute
                // When it returns null, we should exit the program;
                ui = ui.Execute();
            }



            //creating new instance of a RoomRepository and passing in the connection string (brings you to the roommaterepository --> the connectionstring then invoked in baserepo);
            //RoomRepository roomRepo = new RoomRepository(CONNECTION_STRING);

            //Console.WriteLine("Getting All Rooms:");
            //Console.WriteLine();

            //calling GetAll() method on the new instance of RoomRepository
            //List<Room> allRooms = roomRepo.GetAll();

            //since getting all the rooms need to iterate through the whole list in order to display all the rooms
            //foreach (Room room in allRooms)
            //{
            //    Console.WriteLine($"{room.Id} {room.Name} {room.MaxOccupancy}");
            //}

            //getting room by Id
            //Console.WriteLine("----------------------------");
            //Console.WriteLine("Getting Room with Id 1");

            //Room singleRoom = roomRepo.GetById(1);

            //Console.WriteLine($"{singleRoom.Id} {singleRoom.Name} {singleRoom.MaxOccupancy}");

            //INSERT
            //Room bathroom = new Room
            //{
            //    Name = "Bathroom",
            //    MaxOccupancy = 3
            //};

            //inserting bathroom as new Room with an id we got from our Insert method in RoomRepository
            //roomRepo.Insert(bathroom);

            // Console.WriteLine("-------------------------------");
            // Console.WriteLine($"Added the new Room with id {bathroom.Id}");

            //UPDATE 
            // roomRepo.Update(bathroom);

            // Console.WriteLine("-------------------------------");
            // Console.WriteLine($"Updated the new Room to hold max occupancy of 1 to {bathroom.MaxOccupancy}");

            //DELETE
            //roomRepo.Delete(8);

            //Console.WriteLine("-------------------------------");
            //Console.WriteLine($"Just deleted my new bathroom {bathroom.Id}");

            //Getting ALL ROOMMATES
            //RoommateRepository roommateRepo = new RoommateRepository(CONNECTION_STRING);
            //Console.WriteLine("Getting all Roommates");
            //Console.WriteLine();

            //List<Roommate> allRoommates = roommateRepo.GetAll();

            //foreach (Roommate roommate in allRoommates)
            //{
            //    Console.WriteLine($"Roommate number {roommate.Id}'s first name is: {roommate.Firstname} \n last name is: {roommate.Lastname} \n move-in-date: {roommate.MoveInDate} \n rent portion: {roommate.RentPortion}");
            //}

            //Getting ROOMMATES by Id

            //Console.WriteLine("----------------------------");
            //Console.WriteLine("Getting Roommate with Id 2");

            //Roommate singleRoommate = roommateRepo.GetById(2);

            //Console.WriteLine($" BY ID: Roommate number {singleRoommate.Id}'s first name is: {singleRoommate.Firstname} \n last name is: {singleRoommate.Lastname} \n move-in-date: {singleRoommate.MoveInDate} \n rent portion: {singleRoommate.RentPortion}");

            //Getting all the ROOMMATES based on roomId
            //Console.WriteLine("----------------------------");
            //Console.WriteLine("Getting all roommates and all their room info");

            //List<Roommate> allRoommatesWithRoom = roommateRepo.GetAllWithRoom(1);

            //foreach (Roommate roommateWithRoom in allRoommatesWithRoom)
            //{ 
            //    Console.WriteLine($"{roommateWithRoom.Id}: {roommateWithRoom.Firstname} {roommateWithRoom.Lastname} is assigned {roommateWithRoom.Room.Name}; \n This person pays {roommateWithRoom.RentPortion} and their move-in-date: {roommateWithRoom.MoveInDate}");
            //}

            //INSERT new Roommate
            //Roommate newRoommate = new Roommate
            //{
            //    Firstname = "Frodossss",
            //    Lastname = "Baggins",
            //    MoveInDate = new DateTime(2019, 4, 22),
            //    RentPortion = 140,
            //    Room = singleRoom

            //};

            //roommateRepo.Insert(newRoommate);

            //update
            //roommateRepo.Update(newRoommate);
            //Console.WriteLine("----------------------------");
            //Console.WriteLine($"updated Frodo to {newRoommate.Firstname}");

            //delete
            //roommateRepo.Delete(7);
            //Console.WriteLine("----------------------------");
            //Console.WriteLine($"Just deleted {newRoommate.Firstname} {newRoommate.Lastname}");

        }
    }
}