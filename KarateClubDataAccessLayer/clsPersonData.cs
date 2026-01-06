using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Microsoft.Data.SqlClient;
using Dtos.PersonDTOS;


namespace KarateClubDataAccessLayer
{
    public class clsPersonData
    {
        private static readonly string _Connstring = clsDataSetting.ConnectionString;


        public static int AddNewPerson(CreatePersonDTO person)
        {
            int personId = -1;

            try
            {
                string query = @"
            INSERT INTO People (Name, Address, ContactInfo)
            VALUES (@Name, @Address, @ContactInfo);

            SELECT SCOPE_IDENTITY();
        ";

                using (SqlConnection con = new SqlConnection(_Connstring))
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.Add("@Name", SqlDbType.NVarChar).Value = person.Name;
                    cmd.Parameters.Add("@Address", SqlDbType.NVarChar).Value = person.Address;
                    cmd.Parameters.Add("@ContactInfo", SqlDbType.NVarChar).Value = person.ContactInfo;

                    con.Open();

                    object result = cmd.ExecuteScalar();

                    if (result != null)
                    {
                        personId = Convert.ToInt32(result);
                    }
                }
            }
            catch (Exception ex)
            {
                // لا تخفي الخطأ
                throw new Exception("Error while inserting person", ex);
            }

            return personId;
        }




        public static bool UpdatePerson(CreatePersonDTO Person)
        {
            int result = 0;
            string Query = @"Update People set Name = @Name, Address = @Address, ContactInfo =@ContactInfo where PesrsonId = @personID";

            using (SqlConnection conn = new SqlConnection(_Connstring))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand(Query, conn))
                {
                    cmd.Parameters.AddWithValue("@PersonId", Person.PersonID);
                    cmd.Parameters.AddWithValue("@Name", Person.Name);
                    cmd.Parameters.AddWithValue("@Address", Person.Address);
                    cmd.Parameters.AddWithValue("@ContactInfo", Person.ContactInfo);

                    result = cmd.ExecuteNonQuery();
                }
            }
            return result > 0;
        }


        public static CreatePersonDTO FindPerson(int personId)
        {
            string query = @"SELECT * FROM People WHERE PesrsonId = @PersonId";

            try
            {
                using (SqlConnection conn = new SqlConnection(_Connstring))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        // Add the parameter to avoid SQL injection
                        cmd.Parameters.AddWithValue("@PersonId", personId);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read()) 
                            {
                                var person = new CreatePersonDTO
                                {
                                    PersonID = reader.GetInt32(reader.GetOrdinal("PesrsonId")),
                                    Name = reader.GetString(reader.GetOrdinal("Name")),
                                    Address = reader.GetString(reader.GetOrdinal("Address")),
                                    ContactInfo = reader.GetString(reader.GetOrdinal("ContactInfo"))
                                };

                                return person;
                            }
                        }
                    }
                }
            }

            catch(Exception ex)
            {

            }
            return null;
        }


        public static List<CreatePersonDTO> GetAll()
        {
            List<CreatePersonDTO> persons = new List<CreatePersonDTO>();


            string query = @"SELECT PesrsonId, Name, Address, ContactInfo 
                     FROM people";

            using (SqlConnection connection = new SqlConnection(_Connstring))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            CreatePersonDTO person = new CreatePersonDTO
                            {
                                PersonID = reader.GetInt32(reader.GetOrdinal("PesrsonId")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                Address = reader["Address"] == DBNull.Value
                                            ? null
                                            : reader["Address"].ToString(),
                                ContactInfo = reader["ContactInfo"] == DBNull.Value
                                            ? null
                                            : reader["ContactInfo"].ToString()
                            };

                            persons.Add(person);
                        }
                    }
                }
            }

            return persons;
        }



        public static bool DeletePerson(int personID)
        {
            string query = @"DELETE FROM people WHERE PesrsonId = @PersonID";

            try
            {
                using (SqlConnection connection = new SqlConnection(_Connstring))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@PersonID", System.Data.SqlDbType.Int).Value = personID;

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