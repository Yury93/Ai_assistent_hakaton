using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro; 
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class Keyboard 
{
    [SerializeField] private Transform keyboardTransform;
    [SerializeField] private Transform activeKeyboardTransform, deactiveKeyboardTransform;
    [SerializeField] private Transform BotMessagePanel;
    [SerializeField] private TextMeshProUGUI answerBot;
    [SerializeField] private Image background;
    [Header(" ")]
    [SerializeField] private List<KeyButton> buttons;
    [SerializeField] private List<string> keys;
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private Button sendTextButton;
    [SerializeField] private Button deleteKeyButton;
    [SerializeField] private Button clearButton;
    [SerializeField] private Button closeKeyButton,closeBotMessagePanel;
    [SerializeField] private Color colorActiveSend,colorDeactiveSend;
    private StringBuilder inputBuilder,answerBuilder;
    Coroutine corAnswerText;
    private Sequence sequence;
    public event Action<string> onSendText;
    public Action onTapField,onTapClose;
    private bool botPanelIsOpened;
    public void Init()
    {
        foreach (var button in buttons)
        {
            button.onClick += OnClickButton;
            button.Init();
        }
        KeySetValues();
        sendTextButton.onClick.AddListener(SendText);
        inputBuilder =  new StringBuilder();
        answerBuilder = new StringBuilder();
        deleteKeyButton.onClick.AddListener(DeleteKey);
        clearButton.onClick.AddListener(ClearField);
        inputField.onSelect.AddListener(OnTap);
        closeKeyButton.onClick.AddListener(()=>CloseKeyboard());
        closeBotMessagePanel.onClick.AddListener(()=>CloseBotMessagePanel());
        CloseKeyboard();
        MainContainer.instance.InputMessageSystem.AudioRecorder.onStartRecord += OnStartRecord;
    }

    private void OnStartRecord()
    {
        if(botPanelIsOpened)
        {
            CloseBotMessagePanel();
        }
    }

    private void CloseBotMessagePanel(Action action = null)
    {
        answerBuilder = answerBuilder.Clear();
        sequence?.Kill();
        sequence = DOTween.Sequence();
        sequence.Append(BotMessagePanel.DOMoveY(deactiveKeyboardTransform.position.y, 1f))
              .AppendCallback(() =>
              {
                  background.gameObject.SetActive(false);
                  sequence?.Kill(); 
                  
                  action?.Invoke();
              });
        botPanelIsOpened = false;
    }

    private void OpenBotPanel(string message)
    {
        answerBuilder = answerBuilder.Clear();
        CloseKeyboard(()=>
        {
            sequence?.Kill();
            sequence = DOTween.Sequence();
            sequence.Append(BotMessagePanel.DOMoveY(activeKeyboardTransform.position.y, 1f)) 
          .AppendCallback(() =>
          {
            if(corAnswerText != null) { MainContainer.instance.StopCoroutine(corAnswerText); }
              corAnswerText = MainContainer.instance.StartCoroutine(CorBotWrite(message));
              sequence?.Kill();
          });
        });
        botPanelIsOpened = true;
        MainContainer.instance.ChatHandler.onChatMessage -= OpenBotPanel;
    }
    IEnumerator CorBotWrite(string message)
    {
        int i = 0;
        while (message.Length > answerBuilder.ToString().Length)
        {
            answerBuilder = answerBuilder.Append(message[i]);
            answerBot.text = answerBuilder.ToString();  
            yield return new WaitForSeconds(0.04f);
            i++;
        }
    }
    private void CloseKeyboard(Action action = null)
    { 
        sequence?.Kill();
        sequence = DOTween.Sequence();
        sequence.Append(keyboardTransform.DOMoveY(deactiveKeyboardTransform.position.y, 1f))
              .AppendCallback(() =>
              {
                  background.gameObject.SetActive(false);
                  sequence?.Kill();
                  if(action != null) {
                      action?.Invoke();
                  }
              });

        sendTextButton.interactable = false;
        ClearField();
        onTapClose?.Invoke();
    }

    private void OnTap(string arg0)
    {
        if (botPanelIsOpened == true)
        {
            CloseBotMessagePanel(()=>OnTap(""));
            return;
        }
        sendTextButton.interactable = true;
        background.gameObject.SetActive(true);
        sequence?.Kill();
        sequence = DOTween.Sequence();
        sequence.Append(keyboardTransform.DOMoveY(activeKeyboardTransform.position.y, 1f))
            .Append(sendTextButton.transform.DOShakePosition(3F, 10f))
            .AppendCallback(()=>
            {
                sequence?.Kill();
            });
        onTapField?.Invoke();
    }

    public void ClearField()
    {
        if (inputBuilder.Length > 0)
        {
            inputBuilder = inputBuilder.Clear();
            inputField.text = inputBuilder.ToString();
           
        }
        sendTextButton.image.color = colorDeactiveSend;
    }
    public void DeleteKey()
    {
        if (inputBuilder.Length > 0)
        {
            inputBuilder = inputBuilder.Remove(inputBuilder.Length - 1, 1);
            inputField.text = inputBuilder.ToString();
            if (inputBuilder.Length == 0)
            {
                sendTextButton.image.color = colorDeactiveSend;
            }
        }
        else
        {
            sendTextButton.image.color = colorDeactiveSend;
        }
    }
     
    private void OnClickButton(string key)
    {
        inputBuilder = inputBuilder.Append(key);
        inputField.text = inputBuilder.ToString();
        sendTextButton.image.color = colorActiveSend;
    }

    [ContextMenu("SetKeyValue")]
    public void KeySetValues()
    {
        foreach(var button in buttons)
        {
            button.gameObject.SetActive(false); 
        }
        for (int i = 0; i < buttons.Count; i++)
        {
            buttons[i].SetValue(keys[i]);
        }
    }
    private void SendText()
    {
        if (MainContainer.instance.InputMessageSystem.AudioRecorder.IsRecProcess) return;
        if (botPanelIsOpened == true) return;
        if (string.IsNullOrEmpty(inputField.text) == false)
        {
            MainContainer.instance.ChatHandler.onChatMessage += OpenBotPanel;
            onSendText?.Invoke(inputField.text);
            inputField.text = inputBuilder.Clear().ToString();
        }  
    }

   

    public void Dispose()
    {
        sequence?.Kill();
    }
}
