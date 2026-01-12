using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using Dtos.BeltRankDTOS;

namespace KarateClubDataAccessLayer
{
    public class clsBeltRankData
    {
        private static readonly string _Connstring = clsDataSetting.ConnectionString;

        // ============================
        // ADD
        // ============================
        public static int AddNewBeltRank(CreateBeltRankDTO beltRank)
        {
            int beltRankId = -1;

            try
            {
                string query = @"
                    INSERT INTO BeltRanks (RankName, TestFees)
                    VALUES (@RankName, @TestFees);

                    SELECT SCOPE_IDENTITY();
                ";

                using (SqlConnection con = new SqlConnection(_Connstring))
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.Add("@RankName", SqlDbType.NVarChar).Value = beltRank.RankName;
                    cmd.Parameters.Add("@TestFees", SqlDbType.Decimal).Value = beltRank.TestFees;

                    con.Open();
                    object result = cmd.ExecuteScalar();

                    if (result != null)
                        beltRankId = Convert.ToInt32(result);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error while inserting belt rank", ex);
            }

            return beltRankId;
        }

        // ============================
        // UPDATE
        // ============================
        public static bool UpdateBeltRank(CreateBeltRankDTO beltRank)
        {
            int result = 0;

            string query = @"
                UPDATE BeltRanks
                SET RankName = @RankName,
                    TestFees = @TestFees
                WHERE BeltRankid = @BeltRankID
            ";

            using (SqlConnection conn = new SqlConnection(_Connstring))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@BeltRankID", beltRank.BeltRankID);
                    cmd.Parameters.AddWithValue("@RankName", beltRank.RankName);
                    cmd.Parameters.AddWithValue("@TestFees", beltRank.TestFees);

                    result = cmd.ExecuteNonQuery();
                }
            }

            return result > 0;
        }

        // ============================
        // FIND BY ID
        // ============================
        public static CreateBeltRankDTO? FindBeltRank(int beltRankId)
        {
            string query = @"SELECT * FROM BeltRanks WHERE BeltRankid = @BeltRankID";

            try
            {
                using (SqlConnection conn = new SqlConnection(_Connstring))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@BeltRankID", beltRankId);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new CreateBeltRankDTO
                                {
                                    BeltRankID = reader.GetInt32(reader.GetOrdinal("BeltRankid")),
                                    RankName = reader.GetString(reader.GetOrdinal("RankName")),
                                    TestFees = reader.GetDouble(reader.GetOrdinal("TestFees"))
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                // optional logging
            }

            return null;
        }

        // ============================
        // GET ALL
        // ============================
        public static List<CreateBeltRankDTO> GetAll()
        {
            List<CreateBeltRankDTO> beltRanks = new List<CreateBeltRankDTO>();

            string query = @"SELECT BeltRankid, RankName, TestFees FROM BeltRanks";

            using (SqlConnection connection = new SqlConnection(_Connstring))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        CreateBeltRankDTO beltRank = new CreateBeltRankDTO
                        {
                            BeltRankID = reader.GetInt32(reader.GetOrdinal("BeltRankid")),
                            RankName = reader.GetString(reader.GetOrdinal("RankName")),
                            TestFees = reader.GetDouble(reader.GetOrdinal("TestFees"))
                        };

                        beltRanks.Add(beltRank);
                    }
                }
            }

            return beltRanks;
        }

        // ============================
        // DELETE
        // ============================
        public static bool DeleteBeltRank(int beltRankId)
        {
            string query = @"DELETE FROM BeltRanks WHERE BeltRankid = @BeltRankID";

            try
            {
                using (SqlConnection connection = new SqlConnection(_Connstring))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@BeltRankID", SqlDbType.Int).Value = beltRankId;

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
