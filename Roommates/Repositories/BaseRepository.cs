using Microsoft.Data.SqlClient;

namespace Roommates.Repositories
{
    /// <summary>
    ///  A base class for every other Repository class to inherit from.
    ///  This class is responsible for providing a database connection to each of the repository subclasses
    /// </summary>
    public class BaseRepository
    {
        /// <summary>
        ///  A "connection string" is the address of the database.
        /// </summary>
        private string _connectionString;


        /// <summary>
        ///  This BaseRepository CONSTRUCTOR will be invoked by subclasses (takes in the _connectionString as argument).
        ///  It will save the connection string for later use.
        /// </summary>
        public BaseRepository(string connectionString)
        {
            _connectionString = connectionString;
        }


        /// <summary>
        ///  Represents a connection to the database from your C# application.
        ///   This is a "tunnel" to connect the application to the database.
        ///   All communication between the application and database passes through this connection.
        ///   Type of property = SqlConnection, new SqlConnection class taking in the _connectionString
        ///   the SqlConnection property is computed meaning any time the Connection property gets referenced it will create a new tunnel between your app and the database
        ///   (usually can execute a single command (ie INSERT or SELECT) -- usually open another new tunnel for each execution of a command
        /// </summary>
        protected SqlConnection Connection => new SqlConnection(_connectionString);
    }
}