using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using MySql.Data.MySqlClient;

namespace Flux.Server
{
    public class DB
    {
        private object DBLock = "";

        public static DB Current;

        private MySqlConnection MySQL;

        public DB()
        {
            lock (DBLock)
            {
                Log.Info("Initializing Database...");
                DB.Current = this;
                string connectionString = "SERVER=localhost;DATABASE=flux;UID=root;PASSWORD=";
                MySqlConnection conn = new MySqlConnection(connectionString);
                try
                {
                    conn.Open();
                    MySQL = conn;
                    Log.Info("Connection to MySQL successful");
                }
                catch
                {
                    Log.Error("Connection to MySQL failed!");
                }
            }
        }

        private int GetAccessLevel(int FluxID, int AppID)
        {
            lock (DBLock)
            {
                MySqlCommand cmd = this.MySQL.CreateCommand();
                cmd.CommandText = "SELECT * FROM access WHERE fluxid='" + FluxID + "' AND appid='" + AppID + "'";
                MySqlDataReader Reader;
                Reader = cmd.ExecuteReader();
                if (Reader.HasRows)
                {
                    Reader.Read();
                    return Reader.GetInt32("level");
                }
                else
                {
                    return 0;
                }
            }

        }

        public UserInfo AuthorizeUser(string username, string password)
        {
            lock (DBLock)
            {
                UserInfo ui = new UserInfo();
                ui.FluxID = 0;
                MySqlCommand cmd = this.MySQL.CreateCommand();
                cmd.CommandText = "SELECT * FROM users WHERE account_name=\"" + username.ToLower() + "\"";
                MySqlDataReader Reader;
                Reader = cmd.ExecuteReader();
                if (Reader.HasRows)
                {
                    Reader.Read();
                    if (password.ToLower() == Reader.GetString("password").ToLower())
                    {
                        ui.LoginResponse = LoginResponseTypeEnum.LoginValidated;
                        ui.FluxID = Reader.GetInt32("fluxid");
                    }
                    else
                        ui.LoginResponse = LoginResponseTypeEnum.LoginInvalid;
                }
                else
                    ui.LoginResponse = LoginResponseTypeEnum.LoginInvalid;
                Reader.Close();

                return ui;
            }
        }
    }
}
