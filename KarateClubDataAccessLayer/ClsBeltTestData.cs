using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using Dtos.BeltTestsDTOS;

namespace KarateClubDataAccessLayer
{
    public class ClsBeltTestData
    {
        private static readonly string _Connstring = clsDataSetting.ConnectionString;

        // ===============================
        // ADD
        // ===============================
        public static int AddNewTest(BeltTestDTO test)
        {
            int testId = -1;

            string query = @"
                INSERT INTO BeltTests
                (MemberID, RankID, Result, TestDate, TestByInstructor, PaymentID)
                VALUES
                (@MemberID, @RankID, @Result, @TestDate, @TestByInstructor, @PaymentID);

                SELECT SCOPE_IDENTITY();
            ";

            using (SqlConnection conn = new SqlConnection(_Connstring))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@MemberID", test.MemberID);
                cmd.Parameters.AddWithValue("@RankID", test.RankID);
                cmd.Parameters.AddWithValue("@Result", test.Result);
                cmd.Parameters.AddWithValue("@TestDate", test.TestDate);
                cmd.Parameters.AddWithValue("@TestByInstructor", test.TestByInstructor);
                cmd.Parameters.AddWithValue("@PaymentID", (object?)test.PaymentID ?? DBNull.Value);

                conn.Open();
                object result = cmd.ExecuteScalar();

                if (result != null)
                    testId = Convert.ToInt32(result);
            }

            return testId;
        }

        // ===============================
        // UPDATE
        // ===============================
        public static bool UpdateTest(BeltTestDTO test)
        {
            if (test == null || test.TestID <= 0)
                throw new ArgumentException("Invalid test data");

            string query = @"
                UPDATE BeltTests
                SET
                    MemberID = @MemberID,
                    RankID = @RankID,
                    Result = @Result,
                    TestDate = @TestDate,
                    TestByInstructor = @TestByInstructor,
                    PaymentID = @PaymentID
                WHERE TestID = @TestID
            ";

            using (SqlConnection conn = new SqlConnection(_Connstring))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@TestID", test.TestID);
                cmd.Parameters.AddWithValue("@MemberID", test.MemberID);
                cmd.Parameters.AddWithValue("@RankID", test.RankID);
                cmd.Parameters.AddWithValue("@Result", test.Result);
                cmd.Parameters.AddWithValue("@TestDate", test.TestDate);
                cmd.Parameters.AddWithValue("@TestByInstructor", test.TestByInstructor);
                cmd.Parameters.AddWithValue("@PaymentID", (object?)test.PaymentID ?? DBNull.Value);

                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();

                return rowsAffected > 0;
            }
        }

        // ===============================
        // DELETE
        // ===============================
        public static bool DeleteTest(int testId)
        {
            if (testId <= 0)
                throw new ArgumentException("Invalid test ID");

            string query = "DELETE FROM BeltTests WHERE TestID = @TestID";

            using (SqlConnection conn = new SqlConnection(_Connstring))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@TestID", testId);

                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();

                return rowsAffected > 0;
            }
        }

        // ===============================
        // EXISTS
        // ===============================
        public static bool IsExist(int testId)
        {
            if (testId <= 0)
                return false;

            string query = "SELECT COUNT(1) FROM BeltTests WHERE TestID = @TestID";

            using (SqlConnection conn = new SqlConnection(_Connstring))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@TestID", testId);
                conn.Open();

                int count = Convert.ToInt32(cmd.ExecuteScalar() ?? 0);
                return count > 0;
            }
        }

        // ===============================
        // FIND BY ID (WITH NAMES & InstructorID)
        // ===============================
        public static BeltTestViewDTO? Find(int testId)
        {
            if (testId <= 0)
                return null;

            string query = @"
                SELECT 
                    bt.TestID,
                    bt.MemberID,
                    pm.Name AS MemberName,
                    bt.RankID,
                    br.RankName,
                    bt.Result,
                    bt.TestDate,
                    pi.Name AS InstructorName,
                    i.InstructorID
                FROM BeltTests bt
                INNER JOIN Members m ON bt.MemberID = m.MemberID
                INNER JOIN People pm ON m.PesrsonId = pm.PesrsonId
                INNER JOIN BeltRanks br ON bt.RankID = br.BeltRankID
                INNER JOIN Instructors i ON bt.TestByInstructor = i.InstructorID
                INNER JOIN People pi ON i.PersonID = pi.PesrsonId
                WHERE bt.TestID = @TestID
            ";

            using (SqlConnection conn = new SqlConnection(_Connstring))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@TestID", testId);
                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new BeltTestViewDTO
                        {
                            TestID = reader.GetInt32(reader.GetOrdinal("TestID")),
                            MemberID = reader.GetInt32(reader.GetOrdinal("MemberID")),
                            MemberName = reader["MemberName"].ToString()!,
                            RankID = reader.GetInt32(reader.GetOrdinal("RankID")),
                            RankName = reader["RankName"].ToString()!,
                            Result = reader.GetBoolean(reader.GetOrdinal("Result")),
                            TestDate = reader.GetDateTime(reader.GetOrdinal("TestDate")),
                            InstructorName = reader["InstructorName"].ToString()!,
                            InstructorID = reader.GetInt32(reader.GetOrdinal("InstructorID"))
                        };
                    }
                }
            }

            return null;
        }

        // ===============================
        // GET TESTS FOR MEMBER (WITH NAMES & InstructorID)
        // ===============================
        public static List<BeltTestViewDTO> GetTestsForMember(int memberId)
        {
            List<BeltTestViewDTO> tests = new List<BeltTestViewDTO>();

            string query = @"
                SELECT 
                    bt.TestID,
                    bt.MemberID,
                    pm.Name AS MemberName,
                    bt.RankID,
                    br.RankName,
                    bt.Result,
                    bt.TestDate,
                    pi.Name AS InstructorName,
                    i.InstructorID
                FROM BeltTests bt
                INNER JOIN Members m ON bt.MemberID = m.MemberID
                INNER JOIN People pm ON m.PesrsonId = pm.PesrsonId
                INNER JOIN BeltRanks br ON bt.RankID = br.BeltRankID
                INNER JOIN Instructors i ON bt.TestByInstructor = i.InstructorID
                INNER JOIN People pi ON i.PersonID = pi.PesrsonId
                WHERE bt.MemberID = @MemberID
                ORDER BY bt.TestDate DESC
            ";

            using (SqlConnection conn = new SqlConnection(_Connstring))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@MemberID", memberId);
                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        tests.Add(new BeltTestViewDTO
                        {
                            TestID = reader.GetInt32(reader.GetOrdinal("TestID")),
                            MemberID = reader.GetInt32(reader.GetOrdinal("MemberID")),
                            MemberName = reader["MemberName"].ToString()!,
                            RankID = reader.GetInt32(reader.GetOrdinal("RankID")),
                            RankName = reader["RankName"].ToString()!,
                            Result = reader.GetBoolean(reader.GetOrdinal("Result")),
                            TestDate = reader.GetDateTime(reader.GetOrdinal("TestDate")),
                            InstructorName = reader["InstructorName"].ToString()!,
                            InstructorID = reader.GetInt32(reader.GetOrdinal("InstructorID"))
                        });
                    }
                }
            }

            return tests;
        }

        // ===============================
        // GET ALL TESTS (WITH NAMES & InstructorID)
        // ===============================
        public static List<BeltTestViewDTO> GetAllWithNames()
        {
            List<BeltTestViewDTO> tests = new List<BeltTestViewDTO>();

            string query = @"
                SELECT 
                    bt.TestID,
                    bt.MemberID,
                    pm.Name AS MemberName,
                    bt.RankID,
                    br.RankName,
                    bt.Result,
                    bt.TestDate,
                    pi.Name AS InstructorName,
                    i.InstructorID
                FROM BeltTests bt
                INNER JOIN Members m ON bt.MemberID = m.MemberID
                INNER JOIN People pm ON m.PesrsonId = pm.PesrsonId
                INNER JOIN BeltRanks br ON bt.RankID = br.BeltRankID
                INNER JOIN Instructors i ON bt.TestByInstructor = i.InstructorID
                INNER JOIN People pi ON i.PersonID = pi.PesrsonId
                ORDER BY bt.TestDate DESC
            ";

            using (SqlConnection conn = new SqlConnection(_Connstring))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        tests.Add(new BeltTestViewDTO
                        {
                            TestID = reader.GetInt32(reader.GetOrdinal("TestID")),
                            MemberID = reader.GetInt32(reader.GetOrdinal("MemberID")),
                            MemberName = reader["MemberName"].ToString()!,
                            RankID = reader.GetInt32(reader.GetOrdinal("RankID")),
                            RankName = reader["RankName"].ToString()!,
                            Result = reader.GetBoolean(reader.GetOrdinal("Result")),
                            TestDate = reader.GetDateTime(reader.GetOrdinal("TestDate")),
                            InstructorName = reader["InstructorName"].ToString()!,
                            InstructorID = reader.GetInt32(reader.GetOrdinal("InstructorID"))
                        });
                    }
                }
            }

            return tests;
        }

        // ===============================
        // GET LAST TEST FOR MEMBER (WITH NAMES & InstructorID)
        // ===============================
        // ===============================
        // GET LAST TEST FOR MEMBER (RAW FIELDS ONLY, NO JOIN)
        // ===============================
        public static BeltTestDTO? GetLastTestForMember(int memberId)
        {
            if (memberId <= 0)
                return null;

            string query = @"
        SELECT TOP 1
            TestID,
            MemberID,
            RankID,
            Result,
            TestDate,
            TestByInstructor,
            PaymentID
        FROM BeltTests
        WHERE MemberID = @MemberID
        ORDER BY TestDate DESC
    ";

            using (SqlConnection conn = new SqlConnection(_Connstring))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@MemberID", memberId);
                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new BeltTestDTO
                        {
                            TestID = reader.GetInt32(reader.GetOrdinal("TestID")),
                            MemberID = reader.GetInt32(reader.GetOrdinal("MemberID")),
                            RankID = reader.GetInt32(reader.GetOrdinal("RankID")),
                            Result = reader.GetBoolean(reader.GetOrdinal("Result")),
                            TestDate = reader.GetDateTime(reader.GetOrdinal("TestDate")),
                            TestByInstructor = reader.GetInt32(reader.GetOrdinal("TestByInstructor")),
                            PaymentID = reader.IsDBNull(reader.GetOrdinal("PaymentID")) ? null : (int?)reader.GetInt32(reader.GetOrdinal("PaymentID"))
                        };
                    }
                }
            }

            return null;
        }

    }
}
