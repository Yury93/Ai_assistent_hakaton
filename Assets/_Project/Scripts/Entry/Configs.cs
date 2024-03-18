//using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;

namespace Core.Configs
{
    [Serializable]
    public class Configs
    {
        [SerializeField] private CSVReader csvReader; 
        [field: SerializeField] public Hakaton hakaton { get; private set; }
        public const string HAKATON_CSV = "Hakaton"; 
        public const string HAKATON_URL = "https://docs.google.com/spreadsheets/d/e/2PACX-1vQLKuN3FgUYaqkexUOx1GRAyrHz4OLu4XnForwfCUXu5hoy_vUURmMABA2Og6lNAWoONt2K3Dozz8Rq/pub?gid=1618490673&single=true&output=csv";

        private string text;

        public async UniTask Load()
        {
            hakaton.entities = await AsyncGetConfig<HakatonEntity>(HAKATON_CSV, HAKATON_URL);
        }

        public async UniTask<List<T>> AsyncGetConfig<T>(string nameConfig,string url)
        {
            var entities = csvReader.ReadCSV<T>(nameConfig);  
            UnityWebRequest request = UnityWebRequest.Get(HAKATON_URL);  
            try
            {
                await request.SendWebRequest();
            }
            catch (Exception e)
            {
                CancellationToken token = new CancellationToken();
                Debug.LogError("fail request");
                token.Register(() => Debug.LogError("cancel method/ error" + e.ToString()));

            }
            if (request.result == UnityWebRequest.Result.Success)
            {
                text = request.downloadHandler.text;

                TextAsset asset = new TextAsset(text);
                var hakatonEntity = csvReader.ReadCSVOnline<T>(asset);
                return  hakatonEntity;
            }
            else
            {
                Debug.LogError("fail: " + request.downloadHandler.text);
                return entities;
            }
        } 
    }
} 