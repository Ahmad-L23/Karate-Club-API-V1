using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Microsoft.Data.SqlClient;
using Dtos.MemberInstructorDTOS;

namespace KarateClubDataAccessLayer
{
    public class clsMemberInstructorData
    {
        private static readonly string _Connstring = clsDataSetting.ConnectionString;

        // ===============================
        // ADD
        // ===============================
        public static bool AddMemberInstructor(MemberInstructorDTO mi)
        {
            string query = @"
                INSERT INTO MemberInstructors (MemberID, InstructorID, AssignDate)
                VALUES (@MemberID, @InstructorID, @AssignDate)";

            try
            {
                using (SqlConnection conn = new SqlConnection(_Connstring))
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MemberID", mi.MemberID);
                    cmd.Parameters.AddWithValue("@InstructorID", mi.InstructorID);
                    cmd.Parameters.AddWithValue("@AssignDate", mi.AssignDate);

                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();

                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error while inserting MemberInstructor record", ex);
            }
        }

        // ===============================
        // UPDATE
        // ===============================
        public static bool UpdateMemberInstructor(MemberInstructorDTO mi)
        {
            string query = @"
                UPDATE MemberInstructors
                SET AssignDate = @AssignDate
                WHERE MemberID = @MemberID AND InstructorID = @InstructorID";

            try
            {
                using (SqlConnection conn = new SqlConnection(_Connstring))
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@AssignDate", mi.AssignDate);
                    cmd.Parameters.AddWithValue("@MemberID", mi.MemberID);
                    cmd.Parameters.AddWithValue("@InstructorID", mi.InstructorID);

                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();

                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error while updating MemberInstructor record", ex);
            }
        }

        // ===============================
        // DELETE
        // ===============================
        public static bool DeleteMemberInstructor(int memberId, int instructorId)
        {
            string query = @"
                DELETE FROM MemberInstructors
                WHERE MemberID = @MemberID AND InstructorID = @InstructorID";

            try
            {
                using (SqlConnection conn = new SqlConnection(_Connstring))
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MemberID", memberId);
                    cmd.Parameters.AddWithValue("@InstructorID", instructorId);

                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();

                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error while deleting MemberInstructor record", ex);
            }
        }

        // ===============================
        // GET BY IDs
        // ===============================
        public static MemberInstructorDTO? FindMemberInstructor(int memberId, int instructorId)
        {
            string query = @"
                SELECT MemberID, InstructorID, AssignDate
                FROM MemberInstructors
                WHERE MemberID = @MemberID AND InstructorID = @InstructorID";

            try
            {
                using (SqlConnection conn = new SqlConnection(_Connstring))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MemberID", memberId);
                        cmd.Parameters.AddWithValue("@InstructorID", instructorId);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new MemberInstructorDTO
                                {
                                    MemberID = reader.GetInt32(reader.GetOrdinal("MemberID")),
                                    InstructorID = reader.GetInt32(reader.GetOrdinal("InstructorID")),
                                    AssignDate = reader.GetDateTime(reader.GetOrdinal("AssignDate"))
                                };
                            }
                        }
                    }
                }
            }
            catch
            {
                // optionally log error
            }

            return null;
        }

        // ===============================
        // GET ALL
        // ===============================
        public static List<MemberInstructorDTO> GetAll()
        {
            List<MemberInstructorDTO> list = new List<MemberInstructorDTO>();

            string query = @"SELECT MemberID, InstructorID, AssignDate FROM MemberInstructors";

            using (SqlConnection conn = new SqlConnection(_Connstring))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new MemberInstructorDTO
                        {
                            MemberID = reader.GetInt32(reader.GetOrdinal("MemberID")),
                            InstructorID = reader.GetInt32(reader.GetOrdinal("InstructorID")),
                            AssignDate = reader.GetDateTime(reader.GetOrdinal("AssignDate"))
                        });
                    }
                }
            }

            return list;
        }

        // ===============================
        // IS EXIST
        // ===============================
        public static bool IsExist(int memberId, int instructorId)
        {
            string query = @"
                SELECT 1 FROM MemberInstructors 
                WHERE MemberID = @MemberID AND InstructorID = @InstructorID";

            using (SqlConnection conn = new SqlConnection(_Connstring))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@MemberID", memberId);
                cmd.Parameters.AddWithValue("@InstructorID", instructorId);

                conn.Open();

                object result = cmd.ExecuteScalar();

                return result != null;
            }
        }

        // ===============================
        // GET ALL MEMBERS for an Instructor with their person info
        // ===============================
        public static List<MemberWithPersonInfoDTO> GetMembersByInstructor(int instructorId)
        {
            var list = new List<MemberWithPersonInfoDTO>();

            string query = @"
                SELECT m.MemberID, p.Name, m.EmergencyContactInfo, m.LastBeltRank, m.isActive
                FROM MemberInstructors mi
                INNER JOIN Members m ON mi.MemberID = m.MemberID
                INNER JOIN People p ON m.PersonId = p.PesrsonId
                WHERE mi.InstructorID = @InstructorID";

            using (SqlConnection conn = new SqlConnection(_Connstring))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@InstructorID", instructorId);

                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new MemberWithPersonInfoDTO
                        {
                            MemberID = reader.GetInt32(reader.GetOrdinal("MemberID")),
                            Name = reader["Name"] == DBNull.Value ? null : reader["Name"].ToString(),
                            EmergencyContactInfo = reader["EmergencyContactInfo"] == DBNull.Value ? null : reader["EmergencyContactInfo"].ToString(),
                            LastBeltRank = reader.GetInt32(reader.GetOrdinal("LastBeltRank")),
                            IsActive = reader.GetBoolean(reader.GetOrdinal("isActive"))
                        });
                    }
                }
            }

            return list;
        }

        // ===============================
        // GET ALL INSTRUCTORS for a Member with their person info
        // ===============================
        public static List<InstructorWithPersonInfoDTO> GetInstructorsByMember(int memberId)
        {
            var list = new List<InstructorWithPersonInfoDTO>();

            string query = @"
                SELECT i.InstructorID, p.Name, i.Qualification
                FROM MemberInstructors mi
                INNER JOIN Instructors i ON mi.InstructorID = i.InstructorID
                INNER JOIN People p ON i.PersonID = p.PesrsonId
                WHERE mi.MemberID = @MemberID";

            using (SqlConnection conn = new SqlConnection(_Connstring))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@MemberID", memberId);

                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new InstructorWithPersonInfoDTO
                        {
                            InstructorID = reader.GetInt32(reader.GetOrdinal("InstructorID")),
                            Name = reader["Name"] == DBNull.Value ? null : reader["Name"].ToString(),
                            Qualification = reader["Qualification"] == DBNull.Value ? null : reader["Qualification"].ToString()
                        });
                    }
                }
            }

            return list;
        }
    }
}
