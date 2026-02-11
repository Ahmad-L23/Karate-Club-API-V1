using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Data;
using Dtos.UsersDTOs;

namespace KarateClubDataAccessLayer
{
    public class ClsUserData
    {
        private static readonly string _Connstring = clsDataSetting.ConnectionString;

        // ===============================
        // ADD
        // ===============================
        public static int AddNew(UserWithPersonDTO user)
        {
            int userId = -1;

            using (SqlConnection conn = new SqlConnection(_Connstring))
            using (SqlCommand cmd = new SqlCommand("sp_InsertUser", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@UserName", user.UserName);
                cmd.Parameters.AddWithValue("@Password", user.Password);
                cmd.Parameters.AddWithValue("@PersonId", user.PersonId);

                conn.Open();

                object result = cmd.ExecuteScalar();

                if (result != null)
                    userId = Convert.ToInt32(result);
            }

            return userId;
        }

        // ===============================
        // UPDATE
        // ===============================
        public static bool Update(UserWithPersonDTO user)
        {
            if (user == null || user.UserId <= 0)
                throw new ArgumentException("Invalid user data");

            using (SqlConnection conn = new SqlConnection(_Connstring))
            using (SqlCommand cmd = new SqlCommand("sp_UpdateUser", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@UserId", user.UserId);
                cmd.Parameters.AddWithValue("@UserName", user.UserName);
                cmd.Parameters.AddWithValue("@Password", user.Password);
                cmd.Parameters.AddWithValue("@PersonId", user.PersonId);

                conn.Open();

                return cmd.ExecuteNonQuery() > 0;
            }
        }

        // ===============================
        // DELETE
        // ===============================
        public static bool Delete(int userId)
        {
            if (userId <= 0)
                throw new ArgumentException("Invalid User ID");

            using (SqlConnection conn = new SqlConnection(_Connstring))
            using (SqlCommand cmd = new SqlCommand("sp_DeleteUser", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@UserId", userId);

                conn.Open();

                return cmd.ExecuteNonQuery() > 0;
            }
        }

        // ===============================
        // EXISTS BY ID
        // ===============================
        public static bool IsExist(int userId)
        {
            if (userId <= 0)
                return false;

            using (SqlConnection conn = new SqlConnection(_Connstring))
            using (SqlCommand cmd = new SqlCommand("sp_UserExistsById", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@UserId", userId);

                conn.Open();

                return Convert.ToInt32(cmd.ExecuteScalar()) == 1;
            }
        }

        // ===============================
        // EXISTS BY USERNAME
        // ===============================
        public static bool IsUserNameExist(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName))
                return false;

            using (SqlConnection conn = new SqlConnection(_Connstring))
            using (SqlCommand cmd = new SqlCommand("sp_UserExists", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@UserName", userName);

                conn.Open();

                return Convert.ToInt32(cmd.ExecuteScalar()) == 1;
            }
        }

        // ===============================
        // FIND (NO PASSWORD)
        // ===============================
        public static UserViewWithPersonDTO? Find(int userId)
        {
            if (userId <= 0)
                return null;

            using (SqlConnection conn = new SqlConnection(_Connstring))
            using (SqlCommand cmd = new SqlCommand("sp_GetUserById", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@UserId", userId);

                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new UserViewWithPersonDTO
                        {
                            UserId = Convert.ToInt32(reader["UserId"]),
                            UserName = reader["UserName"].ToString()!,
                            PersonId = Convert.ToInt32(reader["PersonId"]),
                            PersonName = reader["PersonName"].ToString()!,
                            Email = reader["Email"].ToString()!,
                            Phone = reader["Phone"].ToString()!
                        };
                    }
                }
            }

            return null;
        }


        public static UserWithPersonDTO? FindByUserName(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName))
                return null;

            using (SqlConnection conn = new SqlConnection(_Connstring))
            using (SqlCommand cmd = new SqlCommand("sp_GetUserByUsername", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserName", userName);

                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new UserWithPersonDTO
                        {
                            UserId = Convert.ToInt32(reader["UserId"]),
                            UserName = reader["UserName"].ToString()!,
                            Password = reader["Password"].ToString()!,   // hashed password
                            PersonId = Convert.ToInt32(reader["PersonId"]),

                            // Person info
                            PersonName = reader["Name"].ToString()!,
                            Address = reader["Address"].ToString()!,
                            ContactInfo = reader["ContactInfo"].ToString()!
                        };
                    }
                }
            }

            return null;
        }

        // ===============================
        // GET ALL (NO PASSWORD)
        // ===============================
        public static List<UserViewWithPersonDTO> GetAll()
        {
            List<UserViewWithPersonDTO> users = new();

            using (SqlConnection conn = new SqlConnection(_Connstring))
            using (SqlCommand cmd = new SqlCommand("sp_GetAllUsers", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        users.Add(new UserViewWithPersonDTO
                        {
                            UserId = Convert.ToInt32(reader["UserId"]),
                            UserName = reader["UserName"].ToString()!,
                            PersonId = Convert.ToInt32(reader["PersonId"]),
                            PersonName = reader["PersonName"].ToString()!,
                            Email = reader["Email"].ToString()!,
                            Phone = reader["Phone"].ToString()!
                        });
                    }
                }
            }

            return users;
        }

        // ===============================
        // LOGIN (NO PASSWORD RETURNED)
        // ===============================
        public static UserViewWithPersonDTO? Login(LoginDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.UserName) ||
                string.IsNullOrWhiteSpace(dto.Password))
                return null;

            using (SqlConnection conn = new SqlConnection(_Connstring))
            using (SqlCommand cmd = new SqlCommand("sp_LoginUser", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@UserName", dto.UserName);
                cmd.Parameters.AddWithValue("@Password", dto.Password);

                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new UserViewWithPersonDTO
                        {
                            UserId = Convert.ToInt32(reader["UserId"]),
                            UserName = reader["UserName"].ToString()!,
                            PersonId = Convert.ToInt32(reader["PersonId"]),
                            PersonName = reader["PersonName"].ToString()!,
                            Email = reader["Email"].ToString()!,
                            Phone = reader["Phone"].ToString()!
                        };
                    }
                }
            }

            return null;
        }
    }
}
