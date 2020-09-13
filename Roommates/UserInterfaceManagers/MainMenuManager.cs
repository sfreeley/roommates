using System;
using System.Collections.Generic;
using System.Text;

namespace Roommates.UserInterfaceManagers
{

    //will take care of main menu interface and hold connectionstring, which will allow us to connect to database and perform CRUD functions
    //the RoomManager and RoommateManager classes will hold their respective repositories that allow connection with database
    public class MainMenuManager : IUserInterfaceManager
    {
        private const string CONNECTION_STRING =
           @"server=localhost\SQLExpress;database=Roommates;integrated security=true";

        //once invoked will display main menu options; once user click ie room management it will invoke that classes Execute method and invoke that;
        public IUserInterfaceManager Execute()
        {
            Console.WriteLine("Main Menu");

            Console.WriteLine(" 1) Room Management");
            Console.WriteLine(" 2) Roommate Management");
            Console.WriteLine(" 3) Change Background Color");
            Console.WriteLine(" 4) Other function");
            Console.WriteLine(" 0) Exit");

            Console.Write("> ");
            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1": return new RoomManager(this, CONNECTION_STRING);
                case "2": return new RoommateManager(this, CONNECTION_STRING);
                case "3": throw new NotImplementedException();
                case "4": throw new NotImplementedException();
                case "0":
                    Console.WriteLine("Good bye");
                    return null;
                default:
                    Console.WriteLine("Invalid Selection");
                    return this;
            }
          }

       }
    }
