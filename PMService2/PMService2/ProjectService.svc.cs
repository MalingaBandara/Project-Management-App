﻿using PMService2.Model;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;


namespace PMService2
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ProjectService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select ProjectService.svc or ProjectService.svc.cs at the Solution Explorer and start debugging.
    public class ProjectService : IProjectService
    {
        public static String DBName { get; set; } = "PMService2DB.db";
        public static List<Message> messagesCache = new List<Message>();
        public static void Log(string v)
        {
            Console.WriteLine($"[{DateTime.Now.ToString("G")}] >> {v}");
            Console.ResetColor();
        }

        public bool RequestState(string DeviceID)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Log($"PC Client {DeviceID} Requested Server State");
            return true;
        }

        public void IntializeDatabaseService()
        {
            //Directory.CreateDirectory("ProfilePics");
            if (!File.Exists(DBName))
                File.Create(DBName);

            Log("Database Path Set");

            try
            {
                using (SQLiteConnection con = new SQLiteConnection($"Data Source={DBName}; Version=3;"))
                {
                    con.Open();
                    string dbScript = "CREATE TABLE IF NOT EXISTS " +
                    "message ( " +
                        "MessageContent TEXT, " +
                        "isSticker int, " +
                        "sender TEXT, " +
                        "receiver TEXT, " +
                        "Time TEXT, " +
                        "MentionedUsers TEXT ); " +
                    "CREATE TABLE IF NOT EXISTS " +
                    "project ( " +
                        "ProjectID TEXT, " +
                        "CreatedOn TEXT, " +
                        "CreatedBy TEXT, " +
                        "Title TEXT, " +
                        "ProjectManager TEXT, " +
                        "Members TEXT, " +
                        "Description TEXT, " +
                        "Category TEXT, " +
                        "StartDate TEXT, " +
                        "EndDate TEXT, " +
                        "Stats TEXT );";

                    SQLiteCommand SQLiteCommand = new SQLiteCommand(dbScript, con);
                    SQLiteCommand.ExecuteNonQuery();
                    Log("Server Database Initialized");
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Log(ex.ToString());
            }

            FetchAllMessages();

        }

        public List<Message> FindDirectMessagesFor(string user, DateTime lastMessage)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Log($"Requested Direct messages For the relation {user}");

            Log("FetchingMessages from the database");
            var allMessagesForUser = new List<Message>();
            var newMessages = new List<Message>();
            using (SQLiteConnection con = new SQLiteConnection($"Data Source={DBName}; Version=3;"))
            {
                if (con.State == System.Data.ConnectionState.Closed)
                    con.Open();

                string dbScript = "SELECT MessageContent, isSticker, sender, receiver, Time, MentionedUsers FROM message WHERE sender = @sender OR receiver = @rsender";
                SQLiteCommand sqliteCommand = new SQLiteCommand(dbScript, con);
                sqliteCommand.Parameters.AddWithValue("@sender", user);
                sqliteCommand.Parameters.AddWithValue("@rsender", user);

                SQLiteDataReader reader = sqliteCommand.ExecuteReader();

                while (reader.Read())
                {
                    var MentionedUserList = new List<string>();
                    var mentionedUsers = reader.GetString(5);
                    if (mentionedUsers.Contains(','))
                    {
                        MentionedUserList = mentionedUsers.Split(',').ToList();
                    }
                    else if (mentionedUsers.Length > 0)
                    {
                        MentionedUserList.Add(mentionedUsers);
                    }
                    else
                    {
                        MentionedUserList = new List<string>();
                    }

                    allMessagesForUser.Add(new Message()
                    {
                        MessageContent = reader.GetString(0),
                        isSticker = reader.GetInt32(1) == 0 ? false : true,
                        sender = reader.GetString(2),
                        receiver = reader.GetString(3),
                        Time = DateTime.Parse(reader.GetString(4)),
                        MentionedUsers = MentionedUserList
                    }) ;
                }
                Log($"Found {allMessagesForUser.Count} Messages For the Requsted Relation");
                con.Close();
            }

            if (lastMessage != null)
            {
                Log($"Filtering Messages From {lastMessage.ToString("g")}");
                foreach (var message in allMessagesForUser)
                {
                    if (message.Time > lastMessage)
                    {
                        newMessages.Add(message);
                    }
                    
                }

            }
            else
            {
                newMessages = allMessagesForUser;
                
            }
            Log($"Found {newMessages.Count} new messages");

            return newMessages;

            // Last message

            // if the last message is null, Send the whole list.
            // if the last message is not null, send the messages with a larger timestamp. 

        }

        public List<Message> FetchAllMessages()
        {
            messagesCache.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Log($"Updating Message Cache");

            Log("Fetching Messages from the database");
            var allMessagesForUser = new List<Message>();
            using (SQLiteConnection con = new SQLiteConnection($"Data Source={DBName}; Version=3;"))
            {
                if (con.State == System.Data.ConnectionState.Closed)
                    con.Open();

                string dbScript = "SELECT MessageContent, isSticker, sender, receiver, Time, MentionedUsers FROM message";
                SQLiteCommand sqliteCommand = new SQLiteCommand(dbScript, con);

                SQLiteDataReader reader = sqliteCommand.ExecuteReader();

                while (reader.Read())
                {
                    var MentionedUserList = new List<string>();
                    var mentionedUsers = reader.GetString(5);
                    if (mentionedUsers.Contains(','))
                    {
                        MentionedUserList = mentionedUsers.Split(',').ToList();
                    }
                    else if (mentionedUsers.Length > 0)
                    {
                        MentionedUserList.Add(mentionedUsers);
                    }
                    else
                    {
                        MentionedUserList = new List<string>();
                    }

                    messagesCache.Add(new Message()
                    {
                        MessageContent = reader.GetString(0),
                        isSticker = reader.GetInt32(1) == 0 ? false : true,
                        sender = reader.GetString(2),
                        receiver = reader.GetString(3),
                        Time = DateTime.Parse(reader.GetString(4)),
                        MentionedUsers = MentionedUserList
                    });
                }
                Log($"Found {allMessagesForUser.Count} Total Messages");
                con.Close();
            }

            

            return messagesCache;

            // Last message

            // if the last message is null, Send the whole list.
            // if the last message is not null, send the messages with a larger timestamp. 

        }

        public bool CheckNewMessagesFor(string username, DateTime latestMessageTime)
        {
            //Console.ForegroundColor = ConsoleColor.Blue;
            //Log($"Requested Count Of New Messages For {username} From {latestMessageTime.ToShortTimeString()}");

            //Log("FetchingMessages from the database");
            var newMessages = 0;

            foreach (var message in messagesCache)
            {
                if ((message.sender == username || message.receiver == username) && message.Time > latestMessageTime)
                {
                    newMessages++;
                }
            }

            if (newMessages > 0)
            {
                Console.BackgroundColor = ConsoleColor.DarkCyan;
                Log($"{newMessages} New Message(s) For {username} as {latestMessageTime.ToString("G")}");
                return true;
            }

            return false;
        }

        public bool DeleteMessagesFrom(string sender, string receiver)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Log($"Requested Delete Direct messages For the relation {sender} and {receiver}");

            try
            {
                Log("Deleting Messages from the database");
                var allMessagesForUser = new List<Message>();
                var newMessages = new List<Message>();
                using (SQLiteConnection con = new SQLiteConnection($"Data Source={DBName}; Version=3;"))
                {
                    if (con.State == System.Data.ConnectionState.Closed)
                        con.Open();

                    string dbScript = "DELETE FROM message WHERE (sender=@sender AND receiver=@receiver) OR (sender=@sreceiver AND receiver=@rsender);";
                    SQLiteCommand sqliteCommand = new SQLiteCommand(dbScript, con);
                    sqliteCommand.Parameters.AddWithValue("@sender", sender);
                    sqliteCommand.Parameters.AddWithValue("@receiver", receiver);
                    sqliteCommand.Parameters.AddWithValue("@rsender", sender);
                    sqliteCommand.Parameters.AddWithValue("@sreceiver", receiver);

                    var affRows = sqliteCommand.ExecuteNonQuery();

                    Log($"Deleted {affRows} Messages From the Database");
                    con.Close();
                    FetchAllMessages();
                    return true;
                }
            }
            catch(Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Log(ex.ToString());
                return false;
            }
            

            // Last message

            // if the last message is null, Send the whole list.
            // if the last message is not null, send the messages with a larger timestamp. 
        }

        public bool NewMessage(Message message)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Log("New Message Invoked");
            var CommandString = "INSERT INTO message VALUES(@messageContent, @isSticker, @sender, @receiver, @time, @mentionedUsers);";

            using (SQLiteConnection con = new SQLiteConnection($"Data Source={DBName}; Version=3;"))
            {
                try
                {
                    con.Open();
                    Log($"Saving Message From {message.sender}");
                    SQLiteCommand command = con.CreateCommand();
                    command.CommandText = CommandString;
                    command.Parameters.AddWithValue("@messageContent", message.MessageContent);
                    command.Parameters.AddWithValue("@isSticker", message.isSticker ? 1 : 0);
                    command.Parameters.AddWithValue("@sender", message.sender);
                    command.Parameters.AddWithValue("@receiver", message.receiver);
                    command.Parameters.AddWithValue("@time", message.Time.ToString("G"));
                    if (message.MentionedUsers != null)
                    {
                        Log("Saving Mentioned Users");
                        if (message.MentionedUsers.Count > 1)
                        {
                            Log($"Found {message.MentionedUsers.Count} Mentioned Users");
                            StringBuilder mentionUserString = new StringBuilder();
                            foreach (var user in message.MentionedUsers)
                            {
                                mentionUserString.Append(user + ",");
                            }
                            command.Parameters.AddWithValue("@mentionedUsers", mentionUserString.ToString().Substring(0, mentionUserString[mentionUserString.Length - 1]));
                        }
                        else if(message.MentionedUsers.Count > 0)
                        {
                            Log($"Found {message.MentionedUsers.Count} Mentioned Users");
                            command.Parameters.AddWithValue("@mentionedUsers", message.MentionedUsers.ToArray()[0]);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@mentionedUsers", "");
                        }
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@mentionedUsers", "");
                    }

                    var affRows = command.ExecuteNonQuery();

                    if (affRows > 0)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Log("Successfully saved the message");
                        con.Close();
                        FetchAllMessages();
                        return true;
                    }
                    
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Log(ex.ToString());
                }
            }
            FetchAllMessages();
            return false;
        }


        public bool CreateProject(Project project)
        {
            throw new NotImplementedException();
        }

        public bool UpdateProject(Project project)
        {
            throw new NotImplementedException();
        }

        public bool DeleteProject(string projectID, string username)
        {
            throw new NotImplementedException();
        }
    }
}
