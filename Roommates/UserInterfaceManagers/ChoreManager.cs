using Roommates.Repositories;
using System;
using System.Collections.Generic;
using Roommates.Models;
using System.Data.Common;
using System.Text;

namespace Roommates.UserInterfaceManagers
{
    class ChoreManager : IUserInterfaceManager
    {
        private readonly IUserInterfaceManager _parentUI;
        private ChoreRepository _choreRepository;
        private string _connectionString;

        public ChoreManager(IUserInterfaceManager parentUI, string connectionString)
        {
            _parentUI = parentUI;
            _choreRepository = new ChoreRepository(connectionString);
            _connectionString = connectionString;
        }

        public IUserInterfaceManager Execute()
        {
            Console.WriteLine("Chore Menu");
            Console.WriteLine(" 1) List Chores");
            Console.WriteLine(" 2) Add A Chore");
            Console.WriteLine(" 3) Edit A Chore");
            Console.WriteLine(" 4) Remove A Chore");
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

        public void List()
        {
            List<Chore> chores = _choreRepository.GetAll();

            foreach (Chore chore in chores)
            {
                Console.WriteLine($"{chore.Name}");
            }
        }

        public Chore ChooseChore(string prompt = null)
        {
            if (prompt == null)
            {
                prompt = "Please chood a chore";
            }

            Console.WriteLine(prompt);

            List<Chore> chores = _choreRepository.GetAll();
            for (int i = 0; i < chores.Count; i++)
            {
                Chore chore = chores[i];
                Console.WriteLine($"{i + 1} {chore.Name}");
            }
            Console.Write("> ");

            string input = Console.ReadLine();
            try
            {
                int choice = int.Parse(input);
                return chores[choice - 1];
            }
            catch (Exception ex)
            {
                Console.WriteLine("Invalid Selection");
                return null;
            }
        }

        public void Add()
        {
            Chore chore = new Chore();
            do
            {
                Console.WriteLine("New Chore");

                Console.Write("Chore Name: ");
                chore.Name = Console.ReadLine();

            }
            while (chore.Name == "");

            _choreRepository.Insert(chore);
        }

        private void Edit()
        {
            Chore choreToEdit = ChooseChore("Which chore would you like to edit?");
            if (choreToEdit == null)
            {
                return;
            }

            Console.WriteLine();
            Console.Write("New Chore Name (blank to leave unchanged): ");
            string choreName = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(choreName))
            {
                choreToEdit.Name = choreName;
            }
           
            _choreRepository.Update(choreToEdit);
        }


        private void Remove()
        {
            Chore choreToDelete = ChooseChore("Which chore would you like to remove?");
            if (choreToDelete != null)
            {
                _choreRepository.Delete(choreToDelete.Id);
            }
        }

    }
}
