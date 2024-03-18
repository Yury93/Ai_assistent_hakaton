using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapWindow : Window
{
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private Button closeButton;

    public void Init()
    {
        closeButton.onClick.AddListener(CloseWindow);
    }
    public override void OpenWindow()
    {
        base.OpenWindow();
        scrollRect.horizontalScrollbar.value = 0.968f;
        scrollRect.verticalScrollbar.value = 0f;
    }

}
