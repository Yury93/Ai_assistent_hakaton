using Server;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;

public class ChatHandler
{
    public YandexRequests YandexRequests { get; private set; }
    private MonoBehaviour mono;
    public event Action<string> onChatMessage;
    public Action doRecord;
    public ChatHandler(MonoBehaviour mono)
    {
        this.YandexRequests = new YandexRequests();
        this.mono = mono;
    }
    public void Init()
    {
        MainContainer.instance.InputMessageSystem.AudioRecorder.onStopRecord += SendAudioToRec;
        MainContainer.instance.InputMessageSystem.Keyboard.onSendText += (t) => YandexRequests.SendMessageToGPT(mono,t,OnAnswerGPT);
   
    }
    public void SendAudioToRec(byte[] bytes)
    {
        doRecord?.Invoke();
        YandexRequests.SendAudioToRecognition(mono, bytes, OnRecognition);
    }
    private void OnRecognition(UnityWebRequest request)
    {
        if (request.result == UnityWebRequest.Result.Success)
        {
            ResultRecognizedMessage result = JsonUtility.FromJson<ResultRecognizedMessage>(request.downloadHandler.text);
            YandexRequests.SendMessageToGPT(mono, result.result, OnAnswerGPT);
        }
        else
        {
            Debug.Log("Response: " + request.downloadHandler.text);
        }
    }

    private void OnAnswerGPT(UnityWebRequest request)
    {
        if (request.result == UnityWebRequest.Result.Success)
        {
            try
            {
                string jsonResponse = request.downloadHandler.text;
                string[] jsonResponses = jsonResponse.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                string[] texts = new string[jsonResponses.Length];
                string message = "";
                foreach (var response in jsonResponses)
                {
                    texts = ExtractTextFromJsonResponse(response);
                }
                message = texts.LastOrDefault(t => string.IsNullOrEmpty(t) == false);
                if (string.IsNullOrEmpty(message) == false)
                {
                    string pattern = @"[\n\r*\n\n]";
                    string replacement = "";
                    Regex regex = new Regex(pattern);
                    string result = regex.Replace(message, replacement);

                    string newResult = new string(result.Where(c => c != 'n' && c != '/' && c != '\n').ToArray());
                    onChatMessage?.Invoke(newResult);
                } 
            }
            catch  
            {
                Debug.Log(request.downloadHandler.text);
            }
        }
        else
        {
            Debug.Log("Response: " + request.downloadHandler.text);
        }
    }

    static string[] ExtractTextFromJsonResponse(string jsonResponse)
    {
        List<string> texts = new List<string>();
        string pattern = "\"text\":\"(.*?)\"";


        MatchCollection matches = Regex.Matches(jsonResponse, pattern);

        foreach (Match match in matches)
        {
            string textValue = match.Groups[1].Value;
            if (!string.IsNullOrEmpty(textValue))
            {
                texts.Add(textValue);
            }
        }

        return texts.ToArray();
    }
    public void Dispose()
    {
        MainContainer.instance.InputMessageSystem.AudioRecorder.onStopRecord -= SendAudioToRec;
    }
}
