using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameWindow : Window
{
    [field: SerializeField] public QuizSystem QuizSystem { get;private set; }
    [SerializeField] private Button closeButton;
    public void Init()
    {
        QuizSystem.Init(this);
        closeButton.onClick.AddListener(CloseWindow);
    }
    public override void OpenWindow()
    {
        base.OpenWindow();
        QuizSystem.NewGame();
    }
}
