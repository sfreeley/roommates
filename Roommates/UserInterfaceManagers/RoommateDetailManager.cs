using Roommates.Repositories;
using System;
using System.Collections.Generic;
using Roommates.Models;
using System.Data.Common;
using System.Text;

namespace Roommates.UserInterfaceManagers
{
    class RoommateDetailManager : IUserInterfaceManager
    {
        private IUserInterfaceManager _parentUI;
        private RoommateRepository _roommateRepository;
        private RoomRepository _roomRepository;
        private ChoreRepository _choreRepository;
        private int _roommateId;

        public RoommateDetailManager(IUserInterfaceManager parentUI, string connectionString, int roommateId)
        {
            _parentUI = parentUI;
            _roommateRepository = new RoommateRepository(connectionString);
            _roomRepository = new RoomRepository(connectionString);
            _choreRepository = new ChoreRepository(connectionString);
            _roommateId = roommateId;
        }

        public IUserInterfaceManager Execute()
        {
            Roommate roommate = _roommateRepository.GetById(_roommateId);
            Console.WriteLine($"{roommate.FullName} Details");
            Console.WriteLine(" 1) View");
            Console.WriteLine(" 2) Add Chore To Roommate");
            Console.WriteLine(" 3) Remove Chore From Roommate");
            Console.WriteLine(" 0) Go Back");

            Console.Write("> ");
            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                   View();
                   return this;
                case "2":
                    AddChore();
                    return this;
                case "3":
                    RemoveChore();
                    return this;
                case "0":
                    return _parentUI;
                default:
                    Console.WriteLine("Invalid Selection");
                    return this;
            }
        }

        private void View()
        {
            Roommate roommate = _roommateRepository.GetById(_roommateId);
            Console.WriteLine($"Name: {roommate.FullName}");
            Console.WriteLine($"Rent Portion: {roommate.RentPortion}");
            Console.WriteLine($"Move In Date: {roommate.MoveInDate}");
            Console.WriteLine($"Assigned Room: {roommate.Room.Name}");
            Console.WriteLine($"Assigned Chore(s):");
            foreach (Chore chore in roommate.Chores)
            {
                Console.WriteLine(" " + chore.Name);
            }
            Console.WriteLine();
            
        }

        private void AddChore()
        {
            Roommate roommate = _roommateRepository.GetById(_roommateId);

            Console.WriteLine($"Choose the chore would you like to add to {roommate.FullName}");
            List<Chore> chores = _choreRepository.GetAll();

            //iterate through list of chores
            for (int i = 0; i < chores.Count; i++)
            {
                //display each chore name and index number + 1
                Chore chore = chores[i];
                Console.WriteLine($" {i + 1} {chore.Name}");
            }
            Console.Write("> ");

            //read user input for specific chore index
            string input = Console.ReadLine();
            try
            {
                //parse user's choice to int index
                int choice = int.Parse(input);
                //chore will take the value of the chore at that specific index user chose
                Chore chore = chores[choice - 1];
                _roommateRepository.InsertChore(roommate, chore);
            }
            catch(Exception ex)
            {
                Console.WriteLine("Invalid Selection. Will not add the chore");
            }
        }


        private void RemoveChore()
        {
            Roommate roommate = _roommateRepository.GetById(_roommateId);

            Console.WriteLine($"Chore you would like to remove from {roommate.FullName}");
           
            //this will be a list of chores that are assigned to this individual roommate; there can be many chores assigned to many roommates and vice versa
            List<Chore> chores = roommate.Chores;


            for (int i = 0; i < chores.Count; i++)
            {
                Chore chore = chores[i];
                Console.WriteLine($" {i + 1}) {chore.Name}");
            }
            Console.Write("> ");


            string input = Console.ReadLine();
            try
            {
                //
                int choice = int.Parse(input);
                Chore chore = chores[choice - 1];
                _roommateRepository.DeleteChore(roommate.Id, chore.Id);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Invalid Selection. Won't remove any tags.");
            }




        }
    }
}
