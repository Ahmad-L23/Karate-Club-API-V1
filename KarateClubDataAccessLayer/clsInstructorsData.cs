using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using Dtos.InstructorDTOS;

namespace KarateClubDataAccessLayer
{
    public class clsInstructorsData
    {
        private static readonly string _Connstring = clsDataSetting.ConnectionString;

        // ============================
        // ADD
        // ============================
        public static int AddNewInstructor(CreateInstructorDTO instructor)
        {
            int instructorId = -1;

            try
            {
                string query = @"
                    INSERT INTO Instructors (PersonID, Qualification)
                    VALUES (@PersonID, @Qualification);

                    SELECT SCOPE_IDENTITY();
                ";

                using (SqlConnection con = new SqlConnection(_Connstring))
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.Add("@PersonID", SqlDbType.Int).Value = instructor.PersonID;
                    cmd.Parameters.Add("@Qualification", SqlDbType.NVarChar).Value = instructor.Qualification;

                    con.Open();
                    object result = cmd.ExecuteScalar();

                    if (result != null)
                        instructorId = Convert.ToInt32(result);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error while inserting instructor", ex);
            }

            return instructorId;
        }

        // ============================
        // UPDATE
        // ============================
        public static bool UpdateInstructor(CreateInstructorDTO instructor)
        {
            int result = 0;

            string query = @"
                UPDATE Instructors
                SET PersonID = @PersonID,
                    Qualification = @Qualification
                WHERE InstructorID = @InstructorID
            ";

            using (SqlConnection conn = new SqlConnection(_Connstring))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@InstructorID", instructor.InstructorID);
                    cmd.Parameters.AddWithValue("@PersonID", instructor.PersonID);
                    cmd.Parameters.AddWithValue("@Qualification", instructor.Qualification);

                    result = cmd.ExecuteNonQuery();
                }
            }

            return result > 0;
        }

        // ============================
        // FIND BY ID
        // ============================
        public static CreateInstructorDTO? FindInstructor(int instructorId)
        {
            string query = @"SELECT * FROM Instructors WHERE InstructorID = @InstructorID";

            try
            {
                using (SqlConnection conn = new SqlConnection(_Connstring))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@InstructorID", instructorId);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new CreateInstructorDTO
                                {
                                    InstructorID = reader.GetInt32(reader.GetOrdinal("InstructorID")),
                                    PersonID = reader.GetInt32(reader.GetOrdinal("PersonID")),
                                    Qualification = reader["Qualification"] == DBNull.Value
                                                    ? null
                                                    : reader["Qualification"].ToString()
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
            }

            return null;
        }

        // ============================
        // GET ALL
        // ============================
        public static List<CreateInstructorDTO> GetAll()
        {
            List<CreateInstructorDTO> instructors = new List<CreateInstructorDTO>();

            string query = @"SELECT InstructorID, PersonID, Qualification FROM Instructors";

            using (SqlConnection connection = new SqlConnection(_Connstring))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        CreateInstructorDTO instructor = new CreateInstructorDTO
                        {
                            InstructorID = reader.GetInt32(reader.GetOrdinal("InstructorID")),
                            PersonID = reader.GetInt32(reader.GetOrdinal("PersonID")),
                            Qualification = reader["Qualification"] == DBNull.Value
                                            ? null
                                            : reader["Qualification"].ToString()
                        };

                        instructors.Add(instructor);
                    }
                }
            }

            return instructors;
        }

        // ============================
        // DELETE
        // ============================
        public static bool DeleteInstructor(int instructorId)
        {
            string query = @"DELETE FROM Instructors WHERE InstructorID = @InstructorID";

            try
            {
                using (SqlConnection connection = new SqlConnection(_Connstring))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@InstructorID", SqlDbType.Int).Value = instructorId;

                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();

                    return rowsAffected > 0;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
