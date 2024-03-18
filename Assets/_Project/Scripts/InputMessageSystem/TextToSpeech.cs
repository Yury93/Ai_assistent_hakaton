
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

        string pleaseWaitSpeek = "��...��� �� ������������� ������� ��������, ��� ��������? ���� � ������ ���.";
        MainContainer.instance.ChatHandler.doRecord += () =>
        {
            List<string> list = new List<string>();
            string pleaseWaitSpeek = "��...��� �� ������������� ������� ��������, ��� ��������? ���� � ������ ���. ���������� ���������, � ������ ���� ������� ����������";

            string pleaseWaitSpeek1 = "� ������ ����� ��� ������ ����� ������� ����. ��������� �������...� ����������� ������ ������ �������� ��������, ��� ��";

            string pleaseWaitSpeek2 = "���� ������� ������� ����� ������������� � �� �� ����� � ���� � �������� ���. ��������� ���������� ���� ����, � ������ ��� ����� ��� ������ ��������.";

            string pleaseWaitSpeek3 = "������� ��� ������� ���. � ��� ��� ����� � ��� ��� � ���� ����� ���������� ���� �� ���������� �����. ��������� � � ��� ������.";
            string pleaseWaitSpeek4 = "������� ��� ������� ���. � ��� ��� ����� � ��� ��� � ���� ����� ���������� ���� �� ���������� �����. ��������� � � ��� ������.";
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
        // �������� ������ WAV �� �����
        WWW www = new WWW("file://" + filePath);
        yield return www;

        if (!string.IsNullOrEmpty(www.error))
        {
            Debug.LogError("������ �������� �����: " + www.error);
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
            Debug.LogError("�� ������� ��������� ����� �� �����.");
        }
    }
}