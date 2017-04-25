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

        public int verifyCode(String code)
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




    }
}
