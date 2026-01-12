using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Microsoft.Data.SqlClient;
using Dtos.PaymentsDTOS;

namespace KarateClubDataAccessLayer
{
    public class clsPaymentData
    {
        private static readonly string _Connstring = clsDataSetting.ConnectionString;

        // ===============================
        // ADD
        // ===============================
        public static int AddNewPayment(PaymentDTO payment)
        {
            int paymentId = -1;

            try
            {
                string query = @"
                    INSERT INTO Payments (Amount, Date, MemberID)
                    VALUES (@Amount, @Date, @MemberID);

                    SELECT SCOPE_IDENTITY();
                ";

                using (SqlConnection conn = new SqlConnection(_Connstring))
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Amount", payment.Amount);
                    cmd.Parameters.AddWithValue("@Date", payment.Date);
                    cmd.Parameters.AddWithValue("@MemberID", payment.MemberID);

                    conn.Open();

                    object result = cmd.ExecuteScalar();

                    if (result != null)
                        paymentId = Convert.ToInt32(result);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error while inserting payment", ex);
            }

            return paymentId;
        }

        // ===============================
        // UPDATE
        // ===============================
        public static bool UpdatePayment(PaymentDTO payment)
        {
            int result = 0;

            string query = @"
                UPDATE Payments
                SET Amount = @Amount,
                    Date = @Date,
                    MemberID = @MemberID
                WHERE PaymentID = @PaymentID";

            using (SqlConnection conn = new SqlConnection(_Connstring))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@PaymentID", payment.PaymentID);
                    cmd.Parameters.AddWithValue("@Amount", payment.Amount);
                    cmd.Parameters.AddWithValue("@Date", payment.Date);
                    cmd.Parameters.AddWithValue("@MemberID", payment.MemberID);

                    result = cmd.ExecuteNonQuery();
                }
            }

            return result > 0;
        }

        // ===============================
        // GET BY ID
        // ===============================
        public static PaymentDTO? FindPayment(int paymentId)
        {
            string query = @"SELECT * FROM Payments WHERE PaymentID = @PaymentID";

            try
            {
                using (SqlConnection conn = new SqlConnection(_Connstring))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@PaymentID", paymentId);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new PaymentDTO
                                {
                                    PaymentID = reader.GetInt32(reader.GetOrdinal("PaymentID")),
                                    Amount = reader.GetDecimal(reader.GetOrdinal("Amount")),
                                    Date = reader.GetDateTime(reader.GetOrdinal("Date")),
                                    MemberID = reader.GetInt32(reader.GetOrdinal("MemberID"))
                                };
                            }
                        }
                    }
                }
            }
            catch
            {
                // ignore or log error
            }

            return null;
        }

        // ===============================
        // GET ALL
        // ===============================
        public static List<PaymentDTO> GetAll()
        {
            List<PaymentDTO> payments = new List<PaymentDTO>();

            string query = @"SELECT * FROM Payments";

            using (SqlConnection conn = new SqlConnection(_Connstring))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        payments.Add(new PaymentDTO
                        {
                            PaymentID = reader.GetInt32(reader.GetOrdinal("PaymentID")),
                            Amount = reader.GetDecimal(reader.GetOrdinal("Amount")),
                            Date = reader.GetDateTime(reader.GetOrdinal("Date")),
                            MemberID = reader.GetInt32(reader.GetOrdinal("MemberID"))
                        });
                    }
                }
            }

            return payments;
        }

        // ===============================
        // GET ALL PAYMENTS BY MEMBER ID
        // ===============================
        public static List<PaymentDTO> GetPaymentsByMemberId(int memberId)
        {
            List<PaymentDTO> payments = new List<PaymentDTO>();

            string query = @"SELECT * FROM Payments WHERE MemberID = @MemberID";

            using (SqlConnection conn = new SqlConnection(_Connstring))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@MemberID", memberId);

                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        payments.Add(new PaymentDTO
                        {
                            PaymentID = reader.GetInt32(reader.GetOrdinal("PaymentID")),
                            Amount = reader.GetDecimal(reader.GetOrdinal("Amount")),
                            Date = reader.GetDateTime(reader.GetOrdinal("Date")),
                            MemberID = reader.GetInt32(reader.GetOrdinal("MemberID"))
                        });
                    }
                }
            }

            // Return empty list if no payments found
            return payments;
        }

        // ===============================
        // IS EXIST
        // ===============================
        public static bool IsPaymentExist(int paymentId)
        {
            string query = @"SELECT 1 FROM Payments WHERE PaymentID = @PaymentID";

            using (SqlConnection conn = new SqlConnection(_Connstring))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@PaymentID", paymentId);

                conn.Open();

                object result = cmd.ExecuteScalar();

                return result != null;
            }
        }

        // ===============================
        // DELETE
        // ===============================
        public static bool DeletePayment(int paymentId)
        {
            string query = @"DELETE FROM Payments WHERE PaymentID = @PaymentID";

            try
            {
                using (SqlConnection conn = new SqlConnection(_Connstring))
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@PaymentID", paymentId);

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
