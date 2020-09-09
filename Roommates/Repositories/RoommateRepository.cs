using Microsoft.Data.SqlClient;
using System;
using Roommates.Models;
using System.Collections.Generic;
using System.Text;

namespace Roommates.Repositories
{
    public class RoommateRepository : BaseRepository
    {
        //if using/inheriting a constructor in the base class that accepts a paramter, you have to make sure that your child class also allows that
        //pass in the connectionString and pass that into the base class --> connectionString = address of the database; place we will go in our tunnel
        public RoommateRepository(string connectionString) : base(connectionString) { }

        public List<Roommate> GetAll()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id, Firstname, Lastname, RentPortion, MoveInDate FROM Roommate";

                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Roommate> roommates = new List<Roommate>();

                    while (reader.Read())
                    {
                        int idColumnPosition = reader.GetOrdinal("Id");

                        int idValue = reader.GetInt32(idColumnPosition);

                        int firstNameColumnPosition = reader.GetOrdinal("FirstName");
                        string firstNameValue = reader.GetString(firstNameColumnPosition);

                        int lastNameColumnPosition = reader.GetOrdinal("LastName");
                        string lastNameValue = reader.GetString(lastNameColumnPosition);

                        int rentPortionColumnPosition = reader.GetOrdinal("RentPortion");
                        int rentPortionValue = reader.GetInt32(rentPortionColumnPosition);
                        
                        int moveInDateColumnPosition = reader.GetOrdinal("MoveInDate");
                        DateTime moveInDateValue = reader.GetDateTime(moveInDateColumnPosition);

                        Roommate newRoommate = new Roommate()
                        {
                            Id = idValue,
                            Firstname = firstNameValue,
                            Lastname = lastNameValue,
                            RentPortion = rentPortionValue,
                            MoveInDate = moveInDateValue,
                            Room = null
                        };

                        roommates.Add(newRoommate);


                    }
                    reader.Close();

                    return roommates;
                }
            }
        }

        //returning a value Roommate;
        public Roommate GetById(int id)
        {
            //use a connection (ie tunnel) --> real physical connection most likely through internet (if something is external to the program that you're using, you wrap it
            //in a 'using' block (saying when I am connecting to database, do this between this block and close it when I'm not using it); Connection from baserepository 
            using (SqlConnection conn = Connection)
            {
                //open the tunnel
                conn.Open();
                //connection knows how to create the command with (ie conn.CreateCommand());
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    //have to tell the command what to get from the database-- your query (test your query in your query page)
                    cmd.CommandText = "SELECT Firstname, Lastname, RentPortion, MoveInDate FROM Roommate WHERE Id = @id";
                    
                    //because we need to add the value of Id, we use AddWithValue and pass it a value;
                    cmd.Parameters.AddWithValue("@id", id);

                    //execute by sending it down the "tunnel" SQL command is happening in the database (VS tells SQL database what to get back with the query --> acting like a client (we are also building a client to talk with SQL server)
                    //reader object gives us access to the result coming back from database;
                    SqlDataReader reader = cmd.ExecuteReader();


                    Roommate roommate = null;

                    // if reader.Read() comes back false, it will be empty response (ie I don't have it)
                    if (reader.Read())
                    {
                        //since you're returning a roommate in this method, you want to instanstiate a new Roommate (new type of class)
                       roommate = new Roommate()
                        {
                           // The "ordinal" is the numeric position of the column in the query results.
                           //  For our query, "Id" has an ordinal value of 0 and "Name" is 1 (this is based on the order of how you typed your query).
                           // get the ordinal number of your id column
                            Id = id,
                            Firstname = reader.GetString(reader.GetOrdinal("Firstname")),
                            Lastname = reader.GetString(reader.GetOrdinal("Lastname")),
                            RentPortion = reader.GetInt32(reader.GetOrdinal("RentPortion")),
                            MoveInDate = reader.GetDateTime(reader.GetOrdinal("MoveInDate")),
                            // Room is a type of class and you can always set a class to null (but beware of the usage of null-more on that later);
                            Room = null
                        };

                    }

                    //don't explicitly have to close the connection (the last curly brace will tell using block to close connection);
                    reader.Close();
                    //because your return value with type Roommate, you are returning a Roommate
                    return roommate;
                }
            }
        }

        public List<Roommate> GetAllWithRoom(int roomId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Roommate.Id AS RoommateId, Firstname, Lastname, RentPortion, MoveInDate, Name, MaxOccupancy, Room.Id AS RoomId FROM Roommate JOIN Room ON Roommate.RoomId = Room.id WHERE Room.id = @id";
                    cmd.Parameters.AddWithValue("@id", roomId);
                    SqlDataReader reader = cmd.ExecuteReader();

                    Roommate roommate = null;

                    List<Roommate> roommates = new List<Roommate>();
                   

                    while (reader.Read())
                    {
                        roommate = new Roommate
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("RoommateId")),
                            Firstname = reader.GetString(reader.GetOrdinal("Firstname")),
                            Lastname = reader.GetString(reader.GetOrdinal("Lastname")),
                            RentPortion = reader.GetInt32(reader.GetOrdinal("RentPortion")),
                            MoveInDate = reader.GetDateTime(reader.GetOrdinal("MoveInDate")),
                            Room = new Room()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("RoomId")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                MaxOccupancy = reader.GetInt32(reader.GetOrdinal("MaxOccupancy"))

                            }
                        };

                        roommates.Add(roommate);
                    }
                    reader.Close();
                    return roommates;
                }
            }

        }

        public void Insert(Roommate roommate)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    // These SQL parameters are annoying. Why can't we use string interpolation?
                    // ... sql injection attacks!!!
                    cmd.CommandText = @"INSERT INTO Roommate (Firstname, Lastname, RentPortion, MoveInDate, RoomId) 
                                         OUTPUT INSERTED.Id 
                                         VALUES (@Firstname, @Lastname, @RentPortion, @MoveInDate, @RoomId)";
                    cmd.Parameters.AddWithValue("@Firstname", roommate.Firstname);
                    cmd.Parameters.AddWithValue("@Lastname", roommate.Lastname);
                    cmd.Parameters.AddWithValue("@RentPortion", roommate.RentPortion); 
                    cmd.Parameters.AddWithValue("@MoveInDate", roommate.MoveInDate);
                    cmd.Parameters.AddWithValue("@RoomId", roommate.Room.Id);
                    //executes the SQL command against the database; and RETURNS the first column and first row in the result set (additional columns or rows are ignored), which is the first thing in the database
                    int id = (int)cmd.ExecuteScalar();


                    //setting room Id after inserting in to database (database is where each room's id is getting created); the room parameter that gets passed into the method doesn't
                    //have an id when the method begins, but once it gets returned, the id will be included (the job of OUTPUT INSERTED.Id) --usually with an INSERT statement, no records come back and nothing gets returned
                    roommate.Id = id;
                    
                }
            }
        }

        public void Update(Roommate roommate)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"UPDATE Roommate
                                    SET Firstname = @Firstname,
                                        Lastname = @Lastname,
                                        RentPortion = @RentPortion,
                                        MoveInDate = @MoveInDate,
                                        RoomId = @RoomId
                                    WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@Firstname", roommate.Firstname);
                    cmd.Parameters.AddWithValue("@Lastname", roommate.Lastname);
                    cmd.Parameters.AddWithValue("@RentPortion", roommate.RentPortion);
                    cmd.Parameters.AddWithValue("@MoveInDate", roommate.MoveInDate);
                    cmd.Parameters.AddWithValue("@RoomId", roommate.Room.Id);
                    cmd.Parameters.AddWithValue("@id", roommate.Id);

                    //calling this method when we want to execute a SQL command, but we don't expect anything back from the database;
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Delete(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "DELETE FROM Roommate WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
