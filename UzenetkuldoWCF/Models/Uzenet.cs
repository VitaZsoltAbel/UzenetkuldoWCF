using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using MySql.Data.MySqlClient;
namespace UzenetkuldoConsole.Models
{
    [DataContract]
    public class Uzenet
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Szoveg { get; set; }

        [DataMember]
        public DateTime KuldesIdo { get; set; }

        [DataMember]
        public string UzenetTipus { get; set; }

        [DataMember]
        public string Telefon { get; set; }

        [DataMember]
        public string Email { get; set; }
    }
}