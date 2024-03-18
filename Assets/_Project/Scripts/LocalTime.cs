using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LocalTime : MonoBehaviour
{
    public TextMeshProUGUI timeText;
    public List<TextMeshProUGUI> itemTimes; 
    public List<DateTime> dateTimes;
    private void Start()
    {
        Init();
    }

    private void Init()
    {
        dateTimes = new List<DateTime>();
        foreach (var itemTransport in itemTimes)
        {
            DateTime dateTime = DateTime.Now.AddHours(UnityEngine.Random.Range(1.20f, 3f));
            dateTimes.Add(dateTime);
        }
        TimeUpdate();
    }

   
    private void FixedUpdate()
    {
        TimeUpdate();
    }

    private void TimeUpdate()
    {
        timeText.text = DateTime.Now.ToString("HH:mm:ss");

        for (int i = 0; i < itemTimes.Count; i++)
        {
            TimeSpan timeLeft = dateTimes[i] - DateTime.Now;
            if (timeLeft.TotalSeconds <= 50)
            {
                Init();
            }
            if (timeLeft.TotalHours > 0)
            {
                itemTimes[i].text = timeLeft.ToString("hh\\:mm");
            }
            else
            {
                itemTimes[i].text = timeLeft.Minutes.ToString() + "мин.";
            }
        }
    }
}
