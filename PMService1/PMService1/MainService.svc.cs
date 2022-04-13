﻿using PMService1.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Data.SQLite;
using System.IO;
using System.Reflection;
using System.Windows.Threading;
using System.Threading;
using System.Threading.Tasks;

namespace PMService1
{
    public enum Status
    {
        Offline,
        Online,
        Idle,
        Busy,
        Invisible
    }
    public enum ServiceType
    {
        Online,
        Offline
    }
    public struct UserStatus
    {
        public User User { get; set; }
        public Status Status { get; set; }
    }
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "MainService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select MainService.svc or MainService.svc.cs at the Solution Explorer and start debugging.
    public class MainService : IMainService
    {
        public static String DBName { get; set; } = "PMService1DB.db"; // Server
        private static List<User> lodedUsers = new List<User>();
        private static List<UserStatus> undedUsers = new List<UserStatus>();
        private static string pathTODB = "";

        public static void Log(string v)
        {
            Console.WriteLine($"[{DateTime.Now.ToString("G")}] >> {v}");
        }

        /// <summary>
        /// Initialises the Server database
        /// </summary>
        public void IntializeDatabaseService()
        {
            try
            {
                var dbFile = File.Open(DBName, FileMode.OpenOrCreate, FileAccess.ReadWrite);

                string path = Path.GetDirectoryName(Assembly.GetAssembly(typeof(MainService)).CodeBase);
                pathTODB = Path.Combine(path, DBName);
                Log("Database Path Set to " + pathTODB);

                dbFile.Close();

                using (SQLiteConnection con = new SQLiteConnection($"Data Source={DBName}; Version=3;"))
                {
                    con.Open();
                    string dbScript = "CREATE TABLE IF NOT EXISTS " +
                    "user (" +
                        "Email TEXT, " +
                        "Name TEXT, " +
                        "Password TEXT ); " +
                    "CREATE TABLE IF NOT EXISTS " +
                        "chat (" +
                        "SentOn TEXT, " +       // The date sent
                        "Sender TEXT, " +       // Auther of the message
                        "ProjectID TEXT, " +    // if the message is on the project discussion, the project id
                        "ReceiverID TEXT, " +   // If it is a direct message, the ID of the receiver (Email)
                        "Content TEXT, " +      // Message body
                        "IsEmogi INTEGER ); " + // is this a emogi / Sticker
                        "CREATE TABLE IF NOT EXISTS " +
                    "DirectUsers (" +   // Saved Direct users
                        "AdedBy TEXT, " +
                        "Name TEXT ); ";

                    SQLiteCommand sqliteCommand = new SQLiteCommand(dbScript, con);
                    sqliteCommand.ExecuteNonQuery();

                    Log("Server Database Initialized");

                    dbFile.Close();
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Log("ERROR: " + ex.Message);
                Console.ResetColor();
            }

            StartUserTimers();
        }

        public void StartUserTimers()
        {
            Timer UserTimer = new Timer(UserTimer_Tick, null, 0, 30000);
            Log("User Reset Timers Started");
        }

        private void UserTimer_Tick(object sender)
        {
            Log("Resetting User Status");
            FetchUsers();
            foreach (User user in lodedUsers)
            {
                undedUsers.Add(new UserStatus() { User = user, Status = Status.Offline });
            }
        }

        public bool SetUserStatus(User user, Status status)
        {
            Log($"User {user.Name} connected.");
            FetchUsers();
            foreach (UserStatus ustatus in undedUsers)
            {
                if (ustatus.User == user)
                {
                    undedUsers.Remove(ustatus);
                    undedUsers.Add(new UserStatus() { User = user, Status = status });
                    Log($"User {user.Name} set {status.ToString()}");
                    return true;
                }
            }
            Console.ForegroundColor = ConsoleColor.Yellow;
            Log($"⚠️ User {user.Name} is not in the database.");
            Console.ResetColor();
            return false;
        }

        public Status? GetUserStatus(string email)
        {
            Log("User Status Requested");
            foreach (UserStatus ustatus in undedUsers)
            {
                if (ustatus.User.Email == email)
                {
                    Log($"User {email} is {ustatus.Status.ToString()}");
                    return ustatus.Status;
                }
            }
            Log($"User {email} Not found");
            return null;
        }

        public bool RegisterUser(string email, string name, byte[] imageBuffer, string password)
        {
            Log("User Registration Invoked");
            if (!string.IsNullOrEmpty(email) &&
                !string.IsNullOrEmpty(name) &&
                !string.IsNullOrEmpty(password))
            {

                var newImage = File.Create(Path.Combine("ProfilePics", name + ".png"));
                newImage.Write(imageBuffer, 0, imageBuffer.Length);
                newImage.Close();
                Log($"Saved User Image for {name}");

                using (SQLiteConnection con = new SQLiteConnection($"filename={pathTODB}"))
                {
                    try
                    {
                        con.Open();

                        SQLiteCommand cmd = con.CreateCommand();
                        cmd.CommandText = "INSERT INTO user VALUES(@email, @name, @image, @password);";
                        cmd.Parameters.AddWithValue("@email", email);
                        cmd.Parameters.AddWithValue("@name", name);
                        cmd.Parameters.AddWithValue("@password", password.ToUpper());
                        cmd.Connection = con;
                        cmd.ExecuteNonQuery();
                        Log($"Successfully Registered User {name}");
                        return true;
                    }
                    catch (Exception ex)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Log($"ERROR! {ex.Message}");
                        Console.ResetColor();
                    }
                }

                
            }
            Console.ForegroundColor = ConsoleColor.Red;
            Log($"Failed to register user {name}");
            Console.ResetColor();
            return false;
        }

        /// <summary>
        /// Fetch all users
        /// </summary>
        /// <returns></returns>
        public List<User> FetchUsers()
        {
            Log("Fetching Users From The Database");
            // sync users 
            List<User> users = new List<User>();

            using (SQLiteConnection con = new SQLiteConnection($"Data Source={DBName}; Version=3;"))
            {
                if (con.State == System.Data.ConnectionState.Closed)
                    con.Open();

                string dbScript = "SELECT Email, Name, Password FROM user";
                SQLiteCommand sqliteCommand = new SQLiteCommand(dbScript, con);
                SQLiteDataReader reader = sqliteCommand.ExecuteReader();

                while (reader.Read())
                {
                    users.Add(new User() { Email = reader.GetString(0), Name = reader.GetString(1), Password = reader.GetString(2) });
                }

                lodedUsers = users;
                Log($"Found {lodedUsers.Count} users");
                con.Close();
            }

            return users;
        }

        /// <summary>
        /// Validate the user wether exists on the database with the entered username and password
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool ValidateUser(string email, string password)
        {
            Log($"Validate Requested for {email}");
            FetchUsers();
            foreach (User user in lodedUsers)
            {
                if (user.Email == email && user.Password.ToUpper() == password.ToUpper())
                {
                    Log($"Credentials Matched");
                    return true;
                }
            }
            Console.ForegroundColor = ConsoleColor.Yellow;
            Log("Credentials NoT Matched!");
            Console.ResetColor();
            return false;
        }

        /// <summary>
        /// Check wether the entered emaill address is already in the database
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public bool IsUserRegistered(string email)
        {
            Log($"User {email} Register Status Requested");
            FetchUsers();
            foreach (User user in lodedUsers)
            {
                if (user.Email == email)
                {
                    Log($"User {email} is Registered");
                    return true;
                }
            }

            Log($"User {email} is a new User");
            return false;
        }

        /// <summary>
        /// Get the user
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public User GetUser(string email, string password)
        {
            Log("Get User Requested");
            FetchUsers();
            Log($"Searching for user {email}");
            foreach (User user in lodedUsers)
            {
                if (user.Email == email && user.Password == password.ToUpper())
                {
                    Log($"User {email} Found");
                    return user;
                }
            }
            Log($"User {email} Not Found");
            return null;
        }

        public bool UpdateUser(User loggedUser, User tempUser, byte[] image)
        {
            Log("Update User Requested");
            if (lodedUsers != null && tempUser != null)
            {
                using (SQLiteConnection con = new SQLiteConnection($"Data Source={DBName}; Version=3;"))
                {
                    try
                    {
                        con.Open();

                        Log($"Updating User {loggedUser.Name}");
                        SQLiteCommand cmd = con.CreateCommand();
                        cmd.CommandText = "UPDATE user SET Email=@email, Name=@name, Password=@password WHERE Email=@cemail AND Password=@cpassword";
                        cmd.Parameters.AddWithValue("@email", tempUser.Email);
                        cmd.Parameters.AddWithValue("@name", tempUser.Name);
                        cmd.Parameters.AddWithValue("@password", tempUser.Password.ToUpper());
                        cmd.Parameters.AddWithValue("@cemail", loggedUser.Email);
                        cmd.Parameters.AddWithValue("@cpassword", loggedUser.Password);
                        cmd.Connection = con;
                        var affectedRows = cmd.ExecuteNonQuery();

                        if (loggedUser.Name != tempUser.Name)
                        {
                            Log("Detected Username Change");
                            try
                            {
                                Log("Deleted Old Image");
                                File.Delete(Path.Combine("ProfilePics", loggedUser.Name + ".png"));

                            }
                            catch (Exception _ex)
                            {

                            }


                            var newImage = File.Create(Path.Combine("ProfilePics", tempUser.Name + ".png"));
                            newImage.Write(image, 0, image.Length);
                            newImage.Close();
                            Log($"Updated User Image for {loggedUser.Name}");
                        }

                        if (affectedRows != 0)
                        {
                            Log($"User {loggedUser.Name} updated successfully");
                            return true;
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Log($"User {loggedUser.Name} Update Error!");
                            Console.ResetColor();
                            return false;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Log($"ERROR! {ex.Message}");
                        Console.ResetColor();
                    }
                }
            }
            Console.ForegroundColor = ConsoleColor.Red;
            Log($"Unknown Issue");
            Console.ResetColor();
            return false;
        }

        public byte[] GetBytesFromFile(FileStream fileStream)
        {
            Log("Converting file to bytes");
            // Read the source file into a byte array.
            byte[] bytes = new byte[fileStream.Length];
            int numBytesToRead = (int)fileStream.Length;
            int numBytesRead = 0;
            while (numBytesToRead > 0)
            {
                // Read may return anything from 0 to numBytesToRead.
                int n = fileStream.Read(bytes, numBytesRead, numBytesToRead);

                // Break when the end of the file is reached.
                if (n == 0)
                    break;

                numBytesRead += n;
                numBytesToRead -= n;
            }
            numBytesToRead = bytes.Length;
            Log("Conversation Success");
            return bytes;
        }

        public byte[] RequestUserImage(string username)
        {
            Log($"User Image Requested for {username}");
            FileStream ProfilePicFile;

            if (File.Exists(Path.Combine("ProfilePics", username + ".png")))
            {
                Log("User Image Found");
                ProfilePicFile = File.OpenRead(Path.Combine("ProfilePics", username + ".png"));
                return GetBytesFromFile(ProfilePicFile);
            }
            else
            {
                Log("User Image Not Found. Sending Default Image");
                var defaultImage = File.OpenRead(Path.Combine("Images", "user.png"));
                return GetBytesFromFile(defaultImage);
            }

        }


        public void GetSqliteVersion()
        {
            string cs = $"Data Source={DBName}";
            string stm = "SELECT SQLITE_VERSION()";

            using (var con = new SQLiteConnection(cs))
            {
                con.Open();
                using (var cmd = new SQLiteCommand(stm, con))
                {
                    string version = cmd.ExecuteScalar().ToString();
                    Console.WriteLine($"SQLite version: {version}");
                }
            }
        }
    }
}
