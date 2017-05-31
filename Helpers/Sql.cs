using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Text;



namespace dcbadge.Helpers
{
    public class Sql
    {

        private static string DataSource = Startup.dburi;
        private static string UserID = Startup.dbuser;
        private static string Password = Startup.dbpass;
        private static string db = Startup.dbname;
        private static string table = Startup.dbtable;

        public bool verifyCode(String code)
        {
            bool rtnvalue = false;

            try  
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.DataSource = DataSource;
                builder.UserID = UserID;
                builder.Password = Password;
                builder.InitialCatalog = db;

                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    connection.Open();
                    StringBuilder sb = new StringBuilder();
                    sb.Append("SELECT COUNT ([ID])");
                    sb.Append("FROM " + table);
                    sb.Append("WHERE [requestcode] = '" + code + "';");
                    String sql = sb.ToString();

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int i = Convert.ToInt32(reader[0].ToString());
                                rtnvalue = Convert.ToBoolean(i);
                            }
                        }
                    }
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }

            return rtnvalue;

        }

        public bool verifyQR(String code)
        {
            bool rtnvalue = false;

            try
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.DataSource = DataSource;
                builder.UserID = UserID;
                builder.Password = Password;
                builder.InitialCatalog = db;

                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    connection.Open();
                    StringBuilder sb = new StringBuilder();
                    sb.Append("SELECT COUNT ([ID])");
                    sb.Append("FROM " + table);
                    sb.Append("WHERE [qrcode] = '" + code + "';");
                    String sql = sb.ToString();

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int i = Convert.ToInt32(reader[0].ToString());
                                rtnvalue = Convert.ToBoolean(i);
                            }
                        }
                    }
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }

            return rtnvalue;

        }

        public int badgesQR(String code)
        {
            int rtnvalue = 0;

            try
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.DataSource = DataSource;
                builder.UserID = UserID;
                builder.Password = Password;
                builder.InitialCatalog = db;

                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    connection.Open();
                    StringBuilder sb = new StringBuilder();
                    sb.Append("SELECT [Qantity]");
                    sb.Append("FROM " + table);
                    sb.Append("WHERE [qrcode] = '" + code + "' AND [collected] = 0;");
                    String sql = sb.ToString();

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string read = reader[0].ToString();
                                if (!String.IsNullOrEmpty(read))
                                {
                                    rtnvalue = Convert.ToInt32(read);
                                }
                                else
                                {
                                    rtnvalue = 0;
                                }
                            }
                        }
                    }
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }

            return rtnvalue;

        }

        public bool codeUsed(String code)
        {
            bool rtnvalue = true;

            try
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.DataSource = DataSource;
                builder.UserID = UserID;
                builder.Password = Password;
                builder.InitialCatalog = db;

                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    connection.Open();
                    StringBuilder sb = new StringBuilder();
                    sb.Append("SELECT [codeused]");
                    sb.Append("FROM " + table);
                    sb.Append("WHERE [requestcode] = '" + code + "';");
                    String sql = sb.ToString();

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                rtnvalue = Convert.ToBoolean(reader[0].ToString());
                            }
                        }
                    }
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }

            return rtnvalue;

        }

        public int maxBadges(String code)
        {
            int rtnvalue = 0;

            try
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.DataSource = DataSource;
                builder.UserID = UserID;
                builder.Password = Password;
                builder.InitialCatalog = db;

                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    connection.Open();
                    StringBuilder sb = new StringBuilder();
                    sb.Append("SELECT [maxqantity]");
                    sb.Append("FROM " + table);
                    sb.Append("WHERE [requestcode] = '" + code + "';");
                    String sql = sb.ToString();

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                rtnvalue = Convert.ToInt32(reader[0].ToString());
                            }
                        }
                    }
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }

            return rtnvalue;

        }

        public int getPrice(String code)
        {
            int rtnvalue = 0;

            try
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.DataSource = DataSource;
                builder.UserID = UserID;
                builder.Password = Password;
                builder.InitialCatalog = db;

                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    connection.Open();
                    StringBuilder sb = new StringBuilder();
                    sb.Append("SELECT [price]");
                    sb.Append("FROM " + table);
                    sb.Append("WHERE [requestcode] = '" + code + "';");
                    String sql = sb.ToString();

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string read = reader[0].ToString();
                                if(!String.IsNullOrEmpty(read))
                                {
                                    rtnvalue = Convert.ToInt32(read);
                                }
                                else
                                {
                                    rtnvalue = 0;
                                }
                            }
                        }
                    }
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }

            return rtnvalue;

        }

        public string[] getRecover(String code, String email)
        {

            string[] rtnvalue = new string[3];
            rtnvalue[0] = "";
            rtnvalue[1] = "";

            try
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.DataSource = DataSource;
                builder.UserID = UserID;
                builder.Password = Password;
                builder.InitialCatalog = db;

                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    connection.Open();
                    StringBuilder sb = new StringBuilder();
                    sb.Append("SELECT [qantity], [qrcode]");
                    sb.Append("FROM " + table);
                    sb.Append("WHERE [requestcode] = '" + code + "' AND [email] = '" + email + "';");
                    String sql = sb.ToString();

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                rtnvalue[0] = reader[0].ToString();
                                rtnvalue[1] = reader[1].ToString();
                            }
                        }
                    }
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }

            return rtnvalue;

        }
 
        public string getID(String code)
        {
            string rtnvalue = "";

            try
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.DataSource = DataSource;
                builder.UserID = UserID;
                builder.Password = Password;
                builder.InitialCatalog = db;

                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    connection.Open();
                    StringBuilder sb = new StringBuilder();
                    sb.Append("SELECT [ID]");
                    sb.Append("FROM " + table);
                    sb.Append("WHERE [requestcode] = '" + code + "';");
                    String sql = sb.ToString();

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                rtnvalue = reader[0].ToString();
                            }
                        }
                    }
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }

            return rtnvalue;

        }

        public void updatePrice(String code, int qantity, int price)
        {

            try
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.DataSource = DataSource;
                builder.UserID = UserID;
                builder.Password = Password;
                builder.InitialCatalog = db;

                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    connection.Open();
                    StringBuilder sb = new StringBuilder();
                    sb.Append("UPDATE " + table + " SET Price = '" + price + "', Qantity = '" + qantity + "' WHERE [requestcode] = '" + code + "';");
                    String sql = sb.ToString();
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {

                        int rowsAffected = command.ExecuteNonQuery();

                    }
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }



        }

        public void updateCollected(String code)
        {

            try
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.DataSource = DataSource;
                builder.UserID = UserID;
                builder.Password = Password;
                builder.InitialCatalog = db;

                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    connection.Open();
                    StringBuilder sb = new StringBuilder();
                    sb.Append("UPDATE " + table + " SET [collected] = 1 WHERE [qrcode] = '" + code + "';");
                    String sql = sb.ToString();
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {

                        int rowsAffected = command.ExecuteNonQuery();

                    }
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }



        }

        public void updateSale(String code, string email, string custcode, string paycode, string qrcode)
        {

            try
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.DataSource = DataSource;
                builder.UserID = UserID;
                builder.Password = Password;
                builder.InitialCatalog = db;

                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    connection.Open();
                    StringBuilder sb = new StringBuilder();
                    sb.Append("UPDATE " + table + " SET codeused = 1, email = '" + email + "', custcode = '" + custcode + "', paycode = '" + paycode + "', qrcode = '" + qrcode + "', datepayed = CURRENT_TIMESTAMP WHERE [requestcode] = '" + code + "';");
                    String sql = sb.ToString();
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {

                        int rowsAffected = command.ExecuteNonQuery();

                    }
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }



        }



    }
}
