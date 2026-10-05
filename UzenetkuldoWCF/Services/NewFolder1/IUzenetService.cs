using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.ServiceModel.Web;
using UzenetkuldoConsole.Models;

namespace UzenetkuldoConsole.Services
{
    [ServiceContract]
    public interface IUzenetService
    {
        [OperationContract]
        [WebGet(UriTemplate = "Uzenetek", ResponseFormat = WebMessageFormat.Json)]
        List<Uzenet> GetUzenetek();

        [OperationContract]
        [WebGet(UriTemplate = "Uzenetek/{id}", ResponseFormat = WebMessageFormat.Json)]
        Uzenet GetUzenetById(string id);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "Uzenetek", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string AddUzenet(Uzenet uzenet);

        [OperationContract]
        [WebInvoke(Method = "PUT", UriTemplate = "Uzenetek/{id}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string UpdateUzenet(string id, Uzenet uzenet);

        [OperationContract]
        [WebInvoke(Method = "DELETE", UriTemplate = "Uzenetek/{id}", ResponseFormat = WebMessageFormat.Json)]
        string DeleteUzenet(string id);
    }
}