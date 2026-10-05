using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.ServiceModel.Web;
using MySql.Data.MySqlClient;
using UzenetkuldoConsole.Models;

namespace UzenetkuldoConsole.Services
{
    public class UzenetService : IUzenetService
    {
        public List<Uzenet> GetUzenetek()
        {
            List<Uzenet> lista = new List<Uzenet>();

            using (MySqlConnection conn = Database.Database.GetConnection())
            {
                conn.Open();
                string query = "SELECT Id, Szoveg, KuldesIdo, UzenetTipus, Telefon, Email FROM uzenet";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(new Uzenet
                            {
                                Id = reader.GetInt32("Id"),
                                Szoveg = reader.IsDBNull(reader.GetOrdinal("Szoveg")) ? null : reader.GetString("Szoveg"),
                                KuldesIdo = reader.GetDateTime("KuldesIdo"),
                                UzenetTipus = reader.GetString("UzenetTipus"),
                                Telefon = reader.IsDBNull(reader.GetOrdinal("Telefon")) ? null : reader.GetString("Telefon"),
                                Email = reader.IsDBNull(reader.GetOrdinal("Email")) ? null : reader.GetString("Email")
                            });
                        }
                    }
                }
            }
            return lista;
        }

        public Uzenet GetUzenetById(string id)
        {
            if (!int.TryParse(id, out int uzenetId))
            {
                if (WebOperationContext.Current != null)
                    WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.BadRequest;
                return null;
            }

            using (MySqlConnection conn = Database.Database.GetConnection())
            {
                conn.Open();
                string query = "SELECT Id, Szoveg, KuldesIdo, UzenetTipus, Telefon, Email FROM uzenet WHERE Id = @Id";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", uzenetId);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Uzenet
                            {
                                Id = reader.GetInt32("Id"),
                                Szoveg = reader.IsDBNull(reader.GetOrdinal("Szoveg")) ? null : reader.GetString("Szoveg"),
                                KuldesIdo = reader.GetDateTime("KuldesIdo"),
                                UzenetTipus = reader.GetString("UzenetTipus"),
                                Telefon = reader.IsDBNull(reader.GetOrdinal("Telefon")) ? null : reader.GetString("Telefon"),
                                Email = reader.IsDBNull(reader.GetOrdinal("Email")) ? null : reader.GetString("Email")
                            };
                        }
                    }
                }
            }

            if (WebOperationContext.Current != null)
                WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.NotFound;
            return null;
        }

        public string AddUzenet(Uzenet uzenet)
        {
            if (uzenet == null || string.IsNullOrEmpty(uzenet.UzenetTipus))
            {
                if (WebOperationContext.Current != null)
                    WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.BadRequest;
                return "Sikertelen rögzítés: hiányos adatok.";
            }

            using (MySqlConnection conn = Database.Database.GetConnection())
            {
                conn.Open();
                string query = "INSERT INTO uzenet (Szoveg, KuldesIdo, UzenetTipus, Telefon, Email) " +
                               "VALUES (@Szoveg, @KuldesIdo, @UzenetTipus, @Telefon, @Email)";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Szoveg", (object)uzenet.Szoveg ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@KuldesIdo", uzenet.KuldesIdo == default ? DateTime.Now : uzenet.KuldesIdo);
                    cmd.Parameters.AddWithValue("@UzenetTipus", uzenet.UzenetTipus);
                    cmd.Parameters.AddWithValue("@Telefon", (object)uzenet.Telefon ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Email", (object)uzenet.Email ?? DBNull.Value);

                    int rows = cmd.ExecuteNonQuery();
                    if (rows > 0)
                    {
                        if (WebOperationContext.Current != null)
                            WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.Created;
                        return "Üzenet sikeresen rögzítve.";
                    }
                }
            }

            if (WebOperationContext.Current != null)
                WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.InternalServerError;
            return "Sikertelen mentés.";
        }

        public string UpdateUzenet(string id, Uzenet uzenet)
        {
            if (!int.TryParse(id, out int uzenetId) || uzenet == null)
            {
                if (WebOperationContext.Current != null)
                    WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.BadRequest;
                return "Érvénytelen azonosító vagy adatok.";
            }

            using (MySqlConnection conn = Database.Database.GetConnection())
            {
                conn.Open();
                string query = "UPDATE uzenet SET Szoveg = @Szoveg, KuldesIdo = @KuldesIdo, " +
                               "UzenetTipus = @UzenetTipus, Telefon = @Telefon, Email = @Email WHERE Id = @Id";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", uzenetId);
                    cmd.Parameters.AddWithValue("@Szoveg", (object)uzenet.Szoveg ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@KuldesIdo", uzenet.KuldesIdo == default ? DateTime.Now : uzenet.KuldesIdo);
                    cmd.Parameters.AddWithValue("@UzenetTipus", uzenet.UzenetTipus);
                    cmd.Parameters.AddWithValue("@Telefon", (object)uzenet.Telefon ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Email", (object)uzenet.Email ?? DBNull.Value);

                    int rows = cmd.ExecuteNonQuery();
                    if (rows > 0)
                    {
                        return "Üzenet sikeresen frissítve.";
                    }
                }
            }

            if (WebOperationContext.Current != null)
                WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.NotFound;
            return "A megadott azonosítójú üzenet nem található.";
        }

        public string DeleteUzenet(string id)
        {
            if (!int.TryParse(id, out int uzenetId))
            {
                if (WebOperationContext.Current != null)
                    WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.BadRequest;
                return "Érvénytelen azonosító.";
            }

            using (MySqlConnection conn = Database.Database.GetConnection())
            {
                conn.Open();
                string query = "DELETE FROM uzenet WHERE Id = @Id";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", uzenetId);
                    int rows = cmd.ExecuteNonQuery();
                    if (rows > 0)
                    {
                        return "Üzenet sikeresen törölve.";
                    }
                }
            }

            if (WebOperationContext.Current != null)
                WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.NotFound;
            return "A megadott azonosítójú üzenet nem található.";
        }
    }
}