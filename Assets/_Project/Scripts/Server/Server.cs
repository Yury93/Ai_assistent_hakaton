using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Server
{
    [Serializable]
    public class Server 
    { 
        public const string GET = "GET";
        public const string POST = "POST";
         
      
        public static IEnumerator Send(string body, string restRequest, string apiUrl, string apiKey
            , Action<UnityWebRequest> callback, Dictionary<string, string> additionalHeaders = null)
        {
            UnityWebRequest request = new UnityWebRequest(apiUrl, restRequest);

            if (string.IsNullOrEmpty(body) == false)
            {
                LoadBody(body, request);
            } 
            request.downloadHandler = new DownloadHandlerBuffer();
            AddHeaders(apiKey, additionalHeaders, request); 
            yield return request.SendWebRequest();

            callback?.Invoke(request); 
        }

        private static void LoadBody(string body, UnityWebRequest request)
        {
            string filePath = Application.persistentDataPath + "/file.json";
            System.IO.File.WriteAllText(filePath, body);
            byte[] jsonData = System.IO.File.ReadAllBytes(filePath);
            UploadHandlerRaw uploadHandler = new UploadHandlerRaw(jsonData);
            uploadHandler.contentType = "application/json";
            request.uploadHandler = uploadHandler;
         
        }

        private static void AddHeaders(string apiKey, Dictionary<string, string> additionalHeaders, UnityWebRequest request)
        {
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", apiKey);

            if (additionalHeaders != null)
            {
                foreach (var header in additionalHeaders)
                {
                    request.SetRequestHeader(header.Key, header.Value);
                }
            }
        }
    }
 
}