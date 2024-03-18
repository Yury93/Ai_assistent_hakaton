using System; 
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KeyButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI valueText;
    [SerializeField] private Button button;
    public string KeyValue { get; set; }
    public Action<string> onClick;
    public void Init()
    { 
        button.onClick.AddListener(Click);
    }
    public void SetValue(string keyValue)
    {
        gameObject.SetActive(true);
        KeyValue = keyValue;
        valueText.text = KeyValue; 
    }

    private void Click()
    {
        onClick?.Invoke(KeyValue);
    }
}
