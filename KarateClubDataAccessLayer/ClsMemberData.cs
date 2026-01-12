using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Microsoft.Data.SqlClient;
using Dtos.MembersDTOS;

namespace KarateClubDataAccessLayer
{
    public class ClsMemberData
    {
        private static readonly string _Connstring = clsDataSetting.ConnectionString;


        // ===============================
        // ADD
        // ===============================
        public static int AddNewMember(MemberDTO member)
        {
            int memberId = -1;

            try
            {
                string query = @"
                    INSERT INTO Members (PersonId, EmergencyContactInfo, LastBeltRank, IsActive)
                    VALUES (@PersonId, @EmergencyContactInfo, @LastBeltRank, @IsActive);

                    SELECT SCOPE_IDENTITY();
                ";

                using (SqlConnection conn = new SqlConnection(_Connstring))
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@PersonId", member.PersonId);
                    cmd.Parameters.AddWithValue("@EmergencyContactInfo", member.EmergencyContactInfo);
                    cmd.Parameters.AddWithValue("@LastBeltRank", member.LastBeltRank);
                    cmd.Parameters.AddWithValue("@IsActive", member.isActive);

                    conn.Open();

                    object result = cmd.ExecuteScalar();

                    if (result != null)
                        memberId = Convert.ToInt32(result);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error while inserting member", ex);
            }

            return memberId;
        }


        // ===============================
        // UPDATE
        // ===============================
        public static bool UpdateMember(MemberDTO member)
        {
            int result = 0;

            string query = @"UPDATE Members 
                             SET PersonId = @PersonId,
                                 EmergencyContactInfo = @EmergencyContactInfo,
                                 LastBeltRank = @LastBeltRank,
                                 IsActive = @IsActive
                             WHERE MemberId = @MemberId";

            using (SqlConnection conn = new SqlConnection(_Connstring))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MemberId", member.MemberID);
                    cmd.Parameters.AddWithValue("@PersonId", member.PersonId);
                    cmd.Parameters.AddWithValue("@EmergencyContactInfo", member.EmergencyContactInfo);
                    cmd.Parameters.AddWithValue("@LastBeltRank", member.LastBeltRank);
                    cmd.Parameters.AddWithValue("@IsActive", member.isActive);

                    result = cmd.ExecuteNonQuery();
                }
            }

            return result > 0;
        }


        // ===============================
        // GET BY ID
        // ===============================
        public static MemberDTO? FindMember(int memberId)
        {
            string query = @"SELECT * FROM Members WHERE MemberId = @MemberId";

            try
            {
                using (SqlConnection conn = new SqlConnection(_Connstring))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MemberId", memberId);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new MemberDTO
                                {
                                    MemberID = reader.GetInt32(reader.GetOrdinal("MemberId")),
                                    PersonId = reader.GetInt32(reader.GetOrdinal("PersonId")),
                                    EmergencyContactInfo = reader["EmergencyContactInfo"] == DBNull.Value
                                                            ? null
                                                            : reader["EmergencyContactInfo"].ToString(),
                                    LastBeltRank = reader.GetInt32(reader.GetOrdinal("LastBeltRank")),
                                    isActive = reader.GetBoolean(reader.GetOrdinal("IsActive"))
                                };
                            }
                        }
                    }
                }
            }
            catch
            {
            }

            return null;
        }


        // ===============================
        // GET ALL
        // ===============================
        public static List<MemberDTO> GetAll()
        {
            List<MemberDTO> members = new List<MemberDTO>();

            string query = @"SELECT * FROM Members";

            using (SqlConnection conn = new SqlConnection(_Connstring))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        members.Add(new MemberDTO
                        {
                            MemberID = reader.GetInt32(reader.GetOrdinal("MemberId")),
                            PersonId = reader.GetInt32(reader.GetOrdinal("PersonId")),
                            EmergencyContactInfo = reader["EmergencyContactInfo"] == DBNull.Value
                                                    ? null
                                                    : reader["EmergencyContactInfo"].ToString(),
                            LastBeltRank = reader.GetInt32(reader.GetOrdinal("LastBeltRank")),
                            isActive = reader.GetBoolean(reader.GetOrdinal("IsActive"))
                        });
                    }
                }
            }

            return members;
        }


        // ===============================
        // IS EXIST
        // ===============================
        public static bool IsMemberExist(int memberId)
        {
            string query = @"SELECT 1 FROM Members WHERE MemberId = @MemberId";

            using (SqlConnection conn = new SqlConnection(_Connstring))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@MemberId", memberId);

                conn.Open();

                object result = cmd.ExecuteScalar();

                return result != null;
            }
        }


        // ===============================
        // DELETE (HARD DELETE)
        // ===============================
        public static bool DeleteMember(int memberId)
        {
            string query = @"DELETE FROM Members WHERE MemberId = @MemberId";

            try
            {
                using (SqlConnection conn = new SqlConnection(_Connstring))
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MemberId", memberId);

                    conn.Open();

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            catch
            {
                return false;
            }
        }


        // ===============================
        // DEACTIVATE (SOFT DELETE)
        // ===============================
        public static bool DeactivateMember(int memberId)
        {
            string query = @"UPDATE Members 
                             SET IsActive = 0 
                             WHERE MemberId = @MemberId";

            try
            {
                using (SqlConnection conn = new SqlConnection(_Connstring))
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MemberId", memberId);

                    conn.Open();

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
