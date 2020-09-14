﻿using Roommates.Repositories;
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
        private int _roommateId;

        public RoommateDetailManager(IUserInterfaceManager parentUI, string connectionString, int roommateId)
        {
            _parentUI = parentUI;
            _roommateRepository = new RoommateRepository(connectionString);
            _roomRepository = new RoomRepository(connectionString);
            _roommateId = roommateId;
        }

        public IUserInterfaceManager Execute()
        {
            Roommate roommate = _roommateRepository.GetById(_roommateId);
            Console.WriteLine($"{roommate.FullName} Details");
            Console.WriteLine(" 1) View");
            Console.WriteLine(" 0) Go Back");

            Console.Write("> ");
            switch (choice)
            {
                case "1":
                    View();
                    return this;
                case "2":
                    ViewBlogPosts();
                    return this;
                case "3":
                    AddTag();
                    return this;
                case "4":
                    RemoveTag();
                    return this;
                case "0":
                    return _parentUI;
                default:
                    Console.WriteLine("Invalid Selection");
                    return this;
            }
        }


    }
}