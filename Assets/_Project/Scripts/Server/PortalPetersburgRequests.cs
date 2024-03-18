using System; 
using UnityEngine;
using UnityEngine.Networking;

namespace Server
{
    [Serializable]
    public class PortalPetersburgRequests  
    {
        [SerializeField, TextArea(10, 30)] private string apiKey;
  
        public string API_KEY_TRANSPORT = "https://spb-transport.gate.petersburg.ru/";// маршруты 
        public string API_KEY_CULTURE_OBJECT = "https://yazzh.gate.petersburg.ru/";// культурные объекты питера
        public string API_KEY_NEWS = "https://yazzh.gate.petersburg.ru/"; // новости питера
        public string API_KEY_SIGHTS_OBJECT = "https://spb-classif.gate.petersburg.ru/api/v2/";//достопримечательности
        public string API_KEY_KITCHEN = "https://spb-classif.gate.petersburg.ru/api/v2/"; //кухни с адресами
        public string API_MEMORABLE = "https://yazzh.gate.petersburg.ru/"; //памятные события питера
        MonoBehaviour mono;
        public PortalPetersburgRequests ( MonoBehaviour mono)
        {
           this.mono = mono; 
        }
       
        private void GetTransportCoordinatesInfo(double minLongitude,double minLatitude, double maxLongitude, double maxLatitude)
        {
          mono.  StartCoroutine(Server.Send("", Server.GET, API_KEY_TRANSPORT + $"api/vehicles/{minLongitude},{minLatitude},{maxLongitude},{maxLatitude}", apiKey, Callback));
        }
        private void GetSightsInfo()
        {
            mono.StartCoroutine(Server.Send("", Server.GET, API_KEY_SIGHTS_OBJECT + "datasets/134/versions/latest/data/157", apiKey, Callback));
        }
        private void GetMemoriesInfo()
        {
            mono.StartCoroutine(Server.Send("", Server.GET, API_MEMORABLE + "memorable_dates", apiKey, Callback));
        }

        private void GetNews()
        {
            mono.StartCoroutine(Server.Send("", Server.GET, API_KEY_NEWS + "news", apiKey, Callback));
        }
        private void GetKitchenInfo()
        {
            mono.StartCoroutine(Server.Send("", Server.GET, API_KEY_KITCHEN + "datasets/143/versions/latest/data/570", apiKey, Callback));
        }
        private void GetCultureObjectsInfo(int idObject)
        {
            mono.StartCoroutine(Server.Send("", Server.GET, API_KEY_CULTURE_OBJECT + "okn/id/" + "?id=" + idObject, apiKey, Callback));
        }
        private void GetTransportInfo()
        {
            mono.StartCoroutine(Server.Send("", Server.GET, API_KEY_TRANSPORT + "api/routes", apiKey, Callback));
        }

        private void Callback(UnityWebRequest request)
        {
            if(request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log(request.downloadHandler.text);
            }
            else
            {
                Debug.LogError("error " + request.error);
            }
        }
    }
}