using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;


namespace Serwer
{
    using QueryResult = List<Dictionary<string, string>>;
    class DatabaseConnection
    {
        private MySqlDataReader _reader;
        private MySqlConnection _dbConnection;
        private MySqlCommand _command;
        private bool _inProgress;

        public DatabaseConnection()
        {
            Connect();
            _dbConnection.Open();
            _command = new MySqlCommand("", this.Connection);
        }

        public MySqlConnection Connection
        {
            get
            {
                return _dbConnection;
            }
        }

        public void Connect()
        {
            try
            {
                MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder();
                builder.Server = "localhost";
                builder.UserID = "root";
                builder.Password = "";
                builder.AllowZeroDateTime = true;
                builder.ConvertZeroDateTime = true;
                builder.Database = "ChatDB";

                _dbConnection = new MySqlConnection(builder.ToString());
                Console.WriteLine("Polaczono z baza danych");
            }
            catch (MySqlException e)
            {
                // Emit event here
                Console.WriteLine(e.Message);
            }
        }

        public bool InsertUser(string login, string shrtpass = "")
        {
            try
            {
                if (!UserExists(login))
                {
                    string tmp = "";
                    tmp += "INSERT INTO chatdb.users(login, shrtpass) VALUES (\"" + login + "\", \"" + shrtpass + "\");";
                    _command.CommandText = tmp;
                    _command.ExecuteNonQuery();
                    Console.WriteLine("Dodano uzytkownika");
                    //inform about successed register and login user
                }
                else
                {
                    Console.WriteLine("Nie udalo sie dodac uzytkownika do bazy danych.");
                    //inform about failed register
                }
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
        public bool UserExists(string login, string shrtpass = "")
        {
            try
            {
                string tmp = "SELECT * FROM chatdb.users where login=\"" + login;
                if (shrtpass != "")
                    tmp += "\" and shrtpass=\"" + shrtpass;
                tmp += "\";";
                _command.CommandText = tmp;
                _reader = _command.ExecuteReader();
                while (_reader.Read())
                {
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
            finally
            {
                _reader.Close();
            }
        }

        #region OfflineMessage Methods
        public bool InsertOfflineMessage(string type, string author, string receiverS, string content)
        {
            try
            {
            foo:
                if (!_inProgress)
                {
                    _inProgress = true;
                    string tmp = "";
                    tmp += "INSERT INTO chatdb.offlinemessages(type, author, receivers, content) VALUES (\"" + type + "\", \"" + author + "\", \"" + receiverS + "\", \"" + content + "\");";
                    _command.CommandText = tmp;
                    _command.ExecuteNonQuery();
                    if (_reader != null)
                        _reader.Close();
                    _inProgress = false;
                    return true;
                }
                else
                {
                    System.Threading.Thread.Sleep(1);
                    goto foo;
                }
            }
            catch (Exception e)
            {
               Console.WriteLine(e.Message);
                return false;
            }
        }
        public bool InsertOfflineMessage(string[] order)
        {
            return InsertOfflineMessage(order[0], order[1], order[2], order[3]);
        }
        public bool CheckOfflineMessages(string login)
        {
            try
            {
                string tmp = "SELECT * FROM chatdb.offlinemessages where receiverS like \"" + login + "%\";";
                _command.CommandText = tmp;
                _reader = _command.ExecuteReader();
                while (_reader.Read())
                {
                    _reader.Close();
                    return true;
                }

                _reader.Close();
                return false;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
            finally
            {
                _reader.Close();
            }
        }
        public string GetOfflineMessage(string login)
        {
            try
            {
                string tmp = "SELECT * FROM chatdb.offlinemessages where receiverS like \"" + login + "\" limit 1;";
                _command.CommandText = tmp;
                _reader = _command.ExecuteReader();
                while (_reader.Read())
                {
                    string type = _reader.GetString(0);
                    string author = _reader.GetString(1);
                    string receiverS = _reader.GetString(2);
                    string content = _reader.GetString(3);
                    tmp = type + ":" + author + ":" + receiverS + ":" + content;
                    //send this message
                    _reader.Close();
                    Console.WriteLine("offlineorder: " + tmp);
                    DeleteOfflineMessage(type, author, receiverS, content);
                }
                return tmp;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return "";
            }
            finally
            {
                _reader.Close();
            }
        }
        public bool DeleteOfflineMessage(string type, string author, string receiverS, string content)
        {
            try
            {
                string tmp = "";
                tmp += "DELETE FROM chatdb.offlinemessages WHERE type = \"" + type + "\" and author = \"" + author + "\" and receiverS = \"" + receiverS + "\" and content = \"" + content + "\" limit 1;";
                _command.CommandText = tmp;
                _command.ExecuteNonQuery();
                Console.WriteLine("Wiadomosc offline usunieta");

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
        #endregion
    }
}
