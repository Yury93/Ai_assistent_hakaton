using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InputMessageSytem : MonoBehaviour
{
    [SerializeField] private Keyboard keyboard;
    [SerializeField] private AudioRecorder audioRecorder;
   
    public Keyboard Keyboard => keyboard;
    public AudioRecorder AudioRecorder => audioRecorder;

    public void Init()
    {
        AudioRecorder.Init();
        keyboard.Init();    
        
    }
    private void OnDestroy()
    {
        keyboard.Dispose();
    }
}
