using Microsoft.Data.SqlClient;
using System;
using Roommates.Models;
using System.Collections.Generic;
using System.Text;

namespace Roommates.Repositories
{
    public class RoommateRepository : BaseRepository
    {

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

        public Roommate GetById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Firstname, Lastname, RentPortion, MoveInDate FROM Roommate WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);
                    SqlDataReader reader = cmd.ExecuteReader();

                    Roommate roommate = null;

                    if (reader.Read())
                    {
                        roommate = new Roommate
                        {
                            Id = id,
                            Firstname = reader.GetString(reader.GetOrdinal("Firstname")),
                            Lastname = reader.GetString(reader.GetOrdinal("Lastname")),
                            RentPortion = reader.GetInt32(reader.GetOrdinal("RentPortion")),
                            MoveInDate = reader.GetDateTime(reader.GetOrdinal("MoveInDate")),
                            Room = null
                        };

                    }

                    reader.Close();
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
                    cmd.CommandText = "SELECT Roommate.Id, Firstname, Lastname, RentPortion, MoveInDate, Name, MaxOccupancy FROM Roommate JOIN Room ON Roommate.RoomId = Room.id WHERE Room.id = @id";
                    cmd.Parameters.AddWithValue("@id", roomId);
                    SqlDataReader reader = cmd.ExecuteReader();

                   

                    List<Roommate> roommates = new List<Roommate>();
                    Roommate roommate = null;

                    while (reader.Read())
                    {
                        roommate = new Roommate
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Firstname = reader.GetString(reader.GetOrdinal("Firstname")),
                            Lastname = reader.GetString(reader.GetOrdinal("Lastname")),
                            RentPortion = reader.GetInt32(reader.GetOrdinal("RentPortion")),
                            MoveInDate = reader.GetDateTime(reader.GetOrdinal("MoveInDate")),
                            Room = new Room()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
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
                    cmd.CommandText = @"INSERT INTO Roommate (Firstname, Lastname, RentPortion, MoveInDate) 
                                         OUTPUT INSERTED.Id 
                                         VALUES (@Firstname, @Lastname, @RentPortion, @MoveInDate)";
                    cmd.Parameters.AddWithValue("@Firstname", roommate.Firstname);
                    cmd.Parameters.AddWithValue("@Lastname", roommate.Lastname);
                    cmd.Parameters.AddWithValue("@RentPortion", roommate.RentPortion);
                    cmd.Parameters.AddWithValue("@MoveInDate", roommate.MoveInDate);
                    //cmd.Parameters.AddWithValue("@id", roommate.Room);
                    //executes the SQL command against the database; and RETURNS the first column and first row in the result set (additional columns or rows are ignored), which is the first thing in the database
                    int id = (int)cmd.ExecuteScalar();


                    //setting room Id after inserting in to database (database is where each room's id is getting created); the room parameter that gets passed into the method doesn't
                    //have an id when the method begins, but once it gets returned, the id will be included (the job of OUTPUT INSERTED.Id) --usually with an INSERT statement, no records come back and nothing gets returned
                    roommate.Id = id;
                    
                }
            }
        }
    }
}
