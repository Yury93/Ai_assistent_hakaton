using System;
using System.Collections;
using System.Collections.Generic; 
using UnityEngine;
using UnityEngine.Networking;

namespace Server
{
    [Serializable]
    public class YandexRequests  
    {
        public static string API_CHAT = "Api-Key AQVN2Q6IVbs2UiKPUb2E1EuNfK5tpqryiW7D6C0Z";
        public static string API_SPEECH_TO_TEXT = "Api-Key AQVN0IcF9hPZkXFBZ_WPcfnWQ6XMXSO9eHeWj0_1";
        public static string API_TEXT_TO_SPEECH = "Api-Key AQVN31qa8JFdMZacThlNAiPS29ctXDNMPfEUxWeS";
        public static string FOLDER_ID = "b1g33jl6heg3dn3secec";
        public const string NAME_HEADER = "x-folder-id";
        public const string API_GPT_URL = "https://llm.api.cloud.yandex.net/foundationModels/v1/completion";


        public void SendTextToSpeech(MonoBehaviour mono, string message, Action<UnityWebRequest> callback)
        {
            mono.StartCoroutine(SendTextToSpeech(message, callback));
        }

        public IEnumerator SendTextToSpeech(string message, Action<UnityWebRequest> callback)
        {
            if (!string.IsNullOrEmpty(message))
            {

                string voice = "filipp";
               if (ConfigsContainer.Instance != null && ConfigsContainer.Instance.Hakaton != null)
                {
                    voice = ConfigsContainer.Instance.Hakaton.entities[0].Voice;
                }

                var values = new List<IMultipartFormSection>
        {
            new MultipartFormDataSection("text", $"{message}"),
            new MultipartFormDataSection("lang", "ru-RU"),
            new MultipartFormDataSection("voice",  voice)
        };

                using (UnityWebRequest request = UnityWebRequest.Post("https://tts.api.cloud.yandex.net/speech/v1/tts:synthesize", values))
                {
                    request.SetRequestHeader("Authorization", API_TEXT_TO_SPEECH);
                    yield return request.SendWebRequest();

                   callback?.Invoke(request);
                }
            }
            else
            {
                Debug.Log("message empty " + message);
            }
        }

        public void SendMessageToGPT(MonoBehaviour mono,string message, Action<UnityWebRequest> callback)
        {
            if (!string.IsNullOrEmpty(message))
            {
                UserMessage messageEntity = new UserMessage();
                messageEntity = new UserMessage();
                messageEntity.completionOptions = new CompletionOptions();
                messageEntity.messages = new MessageEntity[1];
                messageEntity.messages[0] = new MessageEntity { role = "user", text = message };

                string promptJson = JsonUtility.ToJson(messageEntity);

              //  string apiUrl = "https://llm.api.cloud.yandex.net/foundationModels/v1/completion";
                Dictionary<string, string> headers = new Dictionary<string, string>();
                headers.Add(NAME_HEADER, FOLDER_ID);
                mono.StartCoroutine(Server.Send(promptJson, Server.POST, API_GPT_URL, API_CHAT, callback, headers));
            }
            else
            {
                Debug.Log("message empty " + message);
            }
        }
        public  void SendAudioToRecognition(MonoBehaviour mono, byte[] audioData,Action<UnityWebRequest> callback)
        {
            mono.StartCoroutine(SendAudioToRecognition( audioData,callback));
        }
        private IEnumerator SendAudioToRecognition(byte[] audioData,Action<UnityWebRequest> callback)
        {
            string url = "https://stt.api.cloud.yandex.net/speech/v1/stt:recognize?folderId=" + FOLDER_ID + "&lang=ru-RU";

            using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
            {
                request.method = "POST"; 
                request.uploadHandler = new UploadHandlerRaw(audioData);
                request.downloadHandler = new DownloadHandlerBuffer();
                request.SetRequestHeader("Authorization", API_SPEECH_TO_TEXT);
                request.SetRequestHeader("Content-Type", "audio/wav");

                yield return request.SendWebRequest();

               callback?.Invoke(request);
            }
        }
    }



    [Serializable]
    public class ResultRecognizedMessage
    { 
        public string result;
    }
    [Serializable]
    public class ResultGPTAnswerMessage
    {
        [SerializeField]public Result result;
        [SerializeField] public Alternatives[] alternatives;
        [SerializeField] public Usage usage;
        [SerializeField] public string modelVersion;
        [Serializable]
        public class Result
        {
            [SerializeField] public Alternatives[] alternatives;
            [SerializeField] public Usage usage;
            [SerializeField] public string modelVersion;
        }
        [Serializable]
        public class Alternatives
        {
            [SerializeField] public Message message;
            [SerializeField] public string status;
        }
        [Serializable]
        public class Message
        {
            [SerializeField] public string role;
            [SerializeField] public string text;
        }
        [Serializable]
        public class Usage
        {
            [SerializeField] public string inputTextTokens;
            [SerializeField] public string completionTokens;
            [SerializeField] public string totalTokens;
        }
    }



    [Serializable]
    public class UserMessage
    {
        public CompletionOptions completionOptions;
        public string modelUri = "gpt://b1g33jl6heg3dn3secec/yandexgpt-lite";
        public MessageEntity[] messages;
    }
    [Serializable]
    public class CompletionOptions
    {
        public bool stream = true;
        public double temperature = 0.6d;
        public Int64 maxTokens = 2000;
    }
    [Serializable]
    public class MessageEntity
    {
        public string role = "system";
        public string text = "Расскажи что нибудь";
    }
}