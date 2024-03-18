
using Concentus.Structs;
using NAudio.Wave;
using Server;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using UnityEngine;
using UnityEngine.Networking;

public class TextToSpeech : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    private YandexRequests yandexRequests;
    private string message;
    public Action<string> onGetSpeech;
    public void Init()
    {
        yandexRequests = new YandexRequests();
        MainContainer.instance.ChatHandler.onChatMessage += OnGetMessage;
        MainContainer.instance.MenuWindow.GameWindow.QuizSystem.onGeneratedQuiz += OnGetMessage;

        string pleaseWaitSpeek = "Хм...Мне же позволительно немного подумать, что ответить? Ведь я просто кот.";
        MainContainer.instance.ChatHandler.doRecord += () =>
        {
            List<string> list = new List<string>();
            string pleaseWaitSpeek = "Хм...Мне же позволительно немного подумать, что ответить? Ведь я просто кот. Пожалуйста подождите, я достаю свой кошачий переводчик";

            string pleaseWaitSpeek1 = "Я отвечу сразу как только мысли догонят меня. Подождите немного...Я обязательно отвечу такому хорошему человеку, как вы";

            string pleaseWaitSpeek2 = "Меня сегодня слишком много подкармливали и из за этого я упал в сахарную яму. Подождите пожалуйста чуть чуть, я отвечу вам сразу как только выберусь.";

            string pleaseWaitSpeek3 = "Спасибо что сказали это. Я как раз думал о кое чём и ваши слова натолкнули меня на интересные мысли. Подождите и я вам отвечу.";
            string pleaseWaitSpeek4 = "Спасибо что сказали это. Я как раз думал о кое чём и ваши слова натолкнули меня на интересные мысли. Подождите и я вам отвечу.";
            list.Add(pleaseWaitSpeek);
            list.Add(pleaseWaitSpeek1);
            list.Add(pleaseWaitSpeek2);
            list.Add(pleaseWaitSpeek3);
            list.Add(pleaseWaitSpeek4);
          
            var index = UnityEngine.Random.Range(0, 4);

            string answer = list[index];
            message = answer;
            yandexRequests.SendTextToSpeech(this, answer, OnGetSpeech);
        };
    }
 
    private void OnGetMessage(string message)
    {
     
       this.message = message;  
        yandexRequests.SendTextToSpeech(this, message, OnGetSpeech);
    }

    private void OnGetSpeech(UnityWebRequest request)
    {
        if (request.result == UnityWebRequest.Result.Success)
        { 
            byte[] audioData = request.downloadHandler.data;
            ConvertOggToWav(audioData, Application.persistentDataPath + "/saves/", "ogg.wav");
            var filePath = Application.persistentDataPath + "/saves/";
            var fileWav = "ogg.wav";
            var audio = File.ReadAllBytes(Path.Combine(filePath, "ogg.wav"));
            var waveFileReader = new WaveFileReader(Application.persistentDataPath + "/saves/" + "ogg.wav");

            byte[] wavData = File.ReadAllBytes(Path.Combine(filePath, "ogg.wav"));

            StartCoroutine(LoadAudioClip(Application.persistentDataPath + "/saves/" + "ogg.wav"));

            onGetSpeech?.Invoke(message);
        }
        else
        {
            Debug.Log("Error: " + request.error);
        }
    }
    public void ConvertOggToWav(byte[] audioData, string filePath, string fileWav)
    {
        using (MemoryStream fileIn = new MemoryStream(audioData))
        using (MemoryStream pcmStream = new MemoryStream())
        {
            OpusDecoder decoder = OpusDecoder.Create(48000, 1);
            Concentus.Oggfile.OpusOggReadStream oggIn = new Concentus.Oggfile.OpusOggReadStream(decoder, fileIn);

            while (oggIn.HasNextPacket)
            {
                short[] packet = oggIn.DecodeNextPacket();

                if (packet != null)
                {
                    for (int i = 0; i < packet.Length; i++)
                    {
                        var bytes = BitConverter.GetBytes(packet[i]);
                        pcmStream.Write(bytes, 0, bytes.Length);
                    }
                }
            }

            pcmStream.Position = 0;
            try
            {
                var wavStream = new RawSourceWaveStream(pcmStream, new WaveFormat(48000, 1));
                var sampleProvider = wavStream.ToSampleProvider();


                WaveFileWriter.CreateWaveFile16(Path.Combine(filePath, fileWav), sampleProvider);
            }
            catch (Exception ex)
            {
                Debug.Log(ex);
            }
        } 
    }
    IEnumerator LoadAudioClip(string filePath)
    {
        // Загрузка данных WAV из файла
        WWW www = new WWW("file://" + filePath);
        yield return www;

        if (!string.IsNullOrEmpty(www.error))
        {
            Debug.LogError("Ошибка загрузки файла: " + www.error);
            yield break;
        }

        AudioClip audioClip = www.GetAudioClip(false, false, AudioType.WAV);

        if (audioClip != null)
        {
            audioSource.clip = audioClip;
            audioSource.Play();
        }
        else
        {
            Debug.LogError("Не удалось загрузить аудио из файла.");
        }
    }
}