using OCBC.BusinessEntity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCBC.DataAccess
{
    public class OCBCDataAccess
    {
        public string SaveRegisterDetails(User user)
        {
            try
            {
                var result = "";
                //connect to data base
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["OCBCDemoDB"].ConnectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("SaveRegisterDetails"))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = conn;

                        cmd.Parameters.AddWithValue("@Id", user.Id);
                        cmd.Parameters.AddWithValue("@FirstName", user.FirstName);
                        cmd.Parameters.AddWithValue("@LastName", user.LastName);
                        cmd.Parameters.AddWithValue("@BirthDate", user.BirthDate);
                        cmd.Parameters.AddWithValue("@Sexy", user.Sexy ?? "");
                        cmd.Parameters.AddWithValue("@Email", user.Email);
                        cmd.Parameters.AddWithValue("@PhoneNumber", user.PhoneNumber);
                        cmd.Parameters.AddWithValue("@Password", user.Password);
                        cmd.Parameters.AddWithValue("@Balance", user.Balance);
                     
                        result = (string)cmd.ExecuteScalar();
                    }
                    conn.Close();
                }
                return result;

            }
            catch (Exception)
            {
                //TODO add trace log
                return  "Error occurred, please contact with administrator";
            }
        }

        public string AddTransactionInfo(TransferHistory transferHistory)
        {
            try
            {
                var result = "";
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["OCBCDemoDB"].ConnectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("AddTransactionInfo"))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = conn;

                        cmd.Parameters.AddWithValue("@Id", transferHistory.Id);
                        cmd.Parameters.AddWithValue("@SenderId", transferHistory.SenderId);
                        cmd.Parameters.AddWithValue("@RecipientEmail", transferHistory.RecipientEmail);
                        cmd.Parameters.AddWithValue("@TransferAmount", transferHistory.TransferAmount);

                        result = (string)cmd.ExecuteScalar();
                    }
                    conn.Close();
                }
                return result;
            }
            catch (Exception ex)
            {
                //TODO add trace log
                return "Error occurred, please contact with administrator";
            }
        }


        public User ValidateUser(LoginViewModel model)
        {
            try
            {
                User user;
                string strSql = "select Id, FirstName, LastName, Email, PhoneNumber, Balance from [User] where Email = @Email AND Password = @Password";
                List<SqlParameter> listParameter = new List<SqlParameter>();
                listParameter.Add(new SqlParameter("@Email", model.Email));
                listParameter.Add(new SqlParameter("@Password", model.Password));
                using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql, listParameter.ToArray()))
                {
                    if (reader.HasRows)
                    {
                        user = new User();
                        while (reader.Read())
                        {
                            user.Id = reader.GetGuid(0);
                            user.FirstName = reader.GetString(1);
                            user.LastName = reader.GetString(2);
                            user.Email = reader.GetString(3);
                            user.Balance = reader.GetDecimal(5);
                        }
                        return user;
                    }
                    return null;
                }
            }
            catch (Exception)
            {
                //TODO add trace log
                return null;
            }

        }

        public string DepositMoney(string userId, decimal amount)
        {

            try
            {
                var result = "";
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["OCBCDemoDB"].ConnectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("UpdateBalance"))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = conn;

                        cmd.Parameters.AddWithValue("@Id", userId);
                        cmd.Parameters.AddWithValue("@Amount", amount);

                        result = (string)cmd.ExecuteScalar();
                    }
                    conn.Close();
                }
                return result;
            }
            catch (Exception ex)
            {
                //TODO add trace log
                return "Error occurred, please contact with administrator";
            }

        }
    }
}
