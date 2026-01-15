using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using Dtos.SubscriptionPeriodDTOS;

namespace KarateClubDataAccessLayer
{
    public class clsSubscriptionPeriods
    {
        private static readonly string _Connstring = clsDataSetting.ConnectionString;

        // ===============================
        // ADD
        // ===============================
        public static int AddNewSubscriptionPeriod(SubscriptionPeriodDTO period)
        {
            int periodId = -1;

            try
            {
                string query = @"
                    INSERT INTO SubscriptionPeriods 
                        (startDate, endDate, Fees, memberId, PaymentID)
                    VALUES
                        (@startDate, @endDate, @Fees, @memberId, @PaymentID);

                    SELECT SCOPE_IDENTITY();
                ";

                using (SqlConnection conn = new SqlConnection(_Connstring))
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@startDate", period.startDate);
                    cmd.Parameters.AddWithValue("@endDate", period.endDate);
                    cmd.Parameters.AddWithValue("@Fees", period.Fees);
                    cmd.Parameters.AddWithValue("@memberId", period.memberId);
                    cmd.Parameters.AddWithValue("@PaymentID", period.PaymentID);

                    conn.Open();

                    object result = cmd.ExecuteScalar();

                    if (result != null)
                        periodId = Convert.ToInt32(result);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error while inserting subscription period", ex);
            }

            return periodId;
        }

        // ===============================
        // UPDATE
        // ===============================
        public static bool UpdateSubscriptionPeriod(SubscriptionPeriodDTO period)
        {
            int result = 0;

            string query = @"
                UPDATE SubscriptionPeriods
                SET startDate = @startDate,
                    endDate = @endDate,
                    Fees = @Fees,
                    memberId = @memberId,
                    PaymentID = @PaymentID
                WHERE PeriodID = @PeriodID
            ";

            using (SqlConnection conn = new SqlConnection(_Connstring))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@PeriodID", period.PeriodID);
                    cmd.Parameters.AddWithValue("@startDate", period.startDate);
                    cmd.Parameters.AddWithValue("@endDate", period.endDate);
                    cmd.Parameters.AddWithValue("@Fees", period.Fees);
                    cmd.Parameters.AddWithValue("@memberId", period.memberId);
                    cmd.Parameters.AddWithValue("@PaymentID", period.PaymentID);

                    result = cmd.ExecuteNonQuery();
                }
            }

            return result > 0;
        }

        // ===============================
        // FIND BY ID (basic, no join)
        // ===============================
        public static SubscriptionPeriodDTO? FindSubscriptionPeriod(int periodId)
        {
            string query = @"SELECT * FROM SubscriptionPeriods WHERE PeriodID = @PeriodID";

            try
            {
                using (SqlConnection conn = new SqlConnection(_Connstring))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@PeriodID", periodId);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new SubscriptionPeriodDTO
                                {
                                    PeriodID = reader.GetInt32(reader.GetOrdinal("PeriodID")),
                                    startDate = reader.GetDateTime(reader.GetOrdinal("startDate")),
                                    endDate = reader.GetDateTime(reader.GetOrdinal("endDate")),
                                    Fees = reader.GetDecimal(reader.GetOrdinal("Fees")),
                                    memberId = reader.GetInt32(reader.GetOrdinal("memberId")),
                                    PaymentID = reader.GetInt32(reader.GetOrdinal("PaymentID"))
                                };
                            }
                        }
                    }
                }
            }
            catch
            {
                // Optional: log error
            }

            return null;
        }

        // ===============================
        // GET ALL (basic, no join)
        // ===============================
        public static List<SubscriptionPeriodDTO> GetAll()
        {
            List<SubscriptionPeriodDTO> periods = new List<SubscriptionPeriodDTO>();

            string query = @"SELECT * FROM SubscriptionPeriods";

            using (SqlConnection conn = new SqlConnection(_Connstring))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        periods.Add(new SubscriptionPeriodDTO
                        {
                            PeriodID = reader.GetInt32(reader.GetOrdinal("PeriodID")),
                            startDate = reader.GetDateTime(reader.GetOrdinal("startDate")),
                            endDate = reader.GetDateTime(reader.GetOrdinal("endDate")),
                            Fees = reader.GetDecimal(reader.GetOrdinal("Fees")),
                            memberId = reader.GetInt32(reader.GetOrdinal("memberId")),
                            PaymentID = reader.GetInt32(reader.GetOrdinal("PaymentID"))
                        });
                    }
                }
            }

            return periods;
        }

        // ===============================
        // IS EXIST
        // ===============================
        public static bool IsSubscriptionPeriodExist(int periodId)
        {
            string query = @"SELECT 1 FROM SubscriptionPeriods WHERE PeriodID = @PeriodID";

            using (SqlConnection conn = new SqlConnection(_Connstring))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@PeriodID", periodId);

                conn.Open();

                object result = cmd.ExecuteScalar();

                return result != null;
            }
        }

        // ===============================
        // DELETE (HARD DELETE)
        // ===============================
        public static bool DeleteSubscriptionPeriod(int periodId)
        {
            string query = @"DELETE FROM SubscriptionPeriods WHERE PeriodID = @PeriodID";

            try
            {
                using (SqlConnection conn = new SqlConnection(_Connstring))
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@PeriodID", periodId);

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
        // GET ACTIVE SUBSCRIPTIONS (WITH JOIN to get Name, ContactInfo, LastBeltRank, isActive)
        // ===============================
        public static List<SubscriptionWithBasicMemberInfoDTO> GetActiveSubscriptions()
        {
            List<SubscriptionWithBasicMemberInfoDTO> periods = new List<SubscriptionWithBasicMemberInfoDTO>();

            string query = @"
                SELECT sp.PeriodID, sp.startDate, sp.endDate, sp.Fees, sp.PaymentID,
                       p.Name, p.ContactInfo,
                       m.LastBeltRank, m.isActive AS MemberIsActive
                FROM SubscriptionPeriods sp
                INNER JOIN Members m ON sp.memberId = m.MemberID
                INNER JOIN People p ON m.PersonId = p.PersonId
                WHERE GETDATE() BETWEEN sp.startDate AND sp.endDate
            ";

            using (SqlConnection conn = new SqlConnection(_Connstring))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        periods.Add(new SubscriptionWithBasicMemberInfoDTO
                        {
                            PeriodID = reader.GetInt32(reader.GetOrdinal("PeriodID")),
                            startDate = reader.GetDateTime(reader.GetOrdinal("startDate")),
                            endDate = reader.GetDateTime(reader.GetOrdinal("endDate")),
                            Fees = reader.GetDecimal(reader.GetOrdinal("Fees")),
                            PaymentID = reader.GetInt32(reader.GetOrdinal("PaymentID")),

                            Name = reader["Name"] as string,
                            ContactInfo = reader["ContactInfo"] as string,
                            LastBeltRank = reader["LastBeltRank"] as string,
                            MemberIsActive = reader.GetBoolean(reader.GetOrdinal("MemberIsActive"))
                        });
                    }
                }
            }

            return periods;
        }

        // ===============================
        // GET SUBSCRIPTIONS BY MEMBER (WITHOUT JOIN - only subscription table fields)
        // ===============================
        public static List<SubscriptionPeriodDTO> GetSubscriptionsByMember(int memberId)
        {
            List<SubscriptionPeriodDTO> periods = new List<SubscriptionPeriodDTO>();

            string query = @"
                SELECT * FROM SubscriptionPeriods
                WHERE memberId = @MemberId
                ORDER BY startDate DESC
            ";

            using (SqlConnection conn = new SqlConnection(_Connstring))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@MemberId", memberId);
                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        periods.Add(new SubscriptionPeriodDTO
                        {
                            PeriodID = reader.GetInt32(reader.GetOrdinal("PeriodID")),
                            startDate = reader.GetDateTime(reader.GetOrdinal("startDate")),
                            endDate = reader.GetDateTime(reader.GetOrdinal("endDate")),
                            Fees = reader.GetDecimal(reader.GetOrdinal("Fees")),
                            memberId = reader.GetInt32(reader.GetOrdinal("memberId")),
                            PaymentID = reader.GetInt32(reader.GetOrdinal("PaymentID"))
                        });
                    }
                }
            }

            return periods;
        }

        // ===============================
        // GET UPCOMING EXPIRING SUBSCRIPTIONS (next 30 days) WITH JOIN to get Name, ContactInfo, LastBeltRank, isActive
        // ===============================
        public static List<SubscriptionWithBasicMemberInfoDTO> GetUpcomingExpiringSubscriptions()
        {
            List<SubscriptionWithBasicMemberInfoDTO> periods = new List<SubscriptionWithBasicMemberInfoDTO>();

            string query = @"
                SELECT sp.PeriodID, sp.startDate, sp.endDate, sp.Fees, sp.PaymentID,
                       p.Name, p.ContactInfo,
                       m.LastBeltRank, m.isActive AS MemberIsActive
                FROM SubscriptionPeriods sp
                INNER JOIN Members m ON sp.memberId = m.MemberID
                INNER JOIN People p ON m.PersonId = p.PersonId
                WHERE sp.endDate BETWEEN @Today AND @Next30Days
            ";

            using (SqlConnection conn = new SqlConnection(_Connstring))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                DateTime today = DateTime.Now.Date;
                DateTime next30Days = today.AddDays(30);

                cmd.Parameters.AddWithValue("@Today", today);
                cmd.Parameters.AddWithValue("@Next30Days", next30Days);

                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        periods.Add(new SubscriptionWithBasicMemberInfoDTO
                        {
                            PeriodID = reader.GetInt32(reader.GetOrdinal("PeriodID")),
                            startDate = reader.GetDateTime(reader.GetOrdinal("startDate")),
                            endDate = reader.GetDateTime(reader.GetOrdinal("endDate")),
                            Fees = reader.GetDecimal(reader.GetOrdinal("Fees")),
                            PaymentID = reader.GetInt32(reader.GetOrdinal("PaymentID")),

                            Name = reader["Name"] as string,
                            ContactInfo = reader["ContactInfo"] as string,
                            LastBeltRank = reader["LastBeltRank"] as string,
                            MemberIsActive = reader.GetBoolean(reader.GetOrdinal("MemberIsActive"))
                        });
                    }
                }
            }

            return periods;
        }

        // ===============================
        // GET TOTAL FEES PAID BY MEMBER 
        // ===============================
        public static decimal GetTotalFeesByMember(int memberId)
        {
            decimal totalFees = 0;

            string query = @"
                SELECT ISNULL(SUM(Fees), 0) AS TotalFees
                FROM SubscriptionPeriods
                WHERE memberId = @MemberId
            ";

            using (SqlConnection conn = new SqlConnection(_Connstring))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@MemberId", memberId);
                conn.Open();

                object result = cmd.ExecuteScalar();

                if (result != null && result != DBNull.Value)
                    totalFees = Convert.ToDecimal(result);
            }

            return totalFees;
        }
    }

}
