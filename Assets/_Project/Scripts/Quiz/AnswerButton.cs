using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AnswerButton : MonoBehaviour
{
    public Image uzor,button2;
    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI textButton;
    public bool IsRightAnswer { get; private set; }
    public Button Button => button;
    public TextMeshProUGUI TextButton => textButton;
    private GameWindow gameWindow;
    public void Init(GameWindow gameWindow)
    {
        this.gameWindow = gameWindow;
        button.onClick.AddListener(GetTextButton);
        button.onClick.AddListener(gameWindow.QuizSystem.OnSelectAnswer);
    }
    public void SetCheckerRightAnswer(bool rightAnswer)
    {
        IsRightAnswer = rightAnswer;
    }
    public void GetTextButton()
    {
        gameWindow.QuizSystem.SetButtonAnswer(this);
    }
    public void DoFadeButton(float alfa)
    {
        //Color colorUzor = new Color(uzor.color.r, uzor.color.g, uzor.color.b, alfa);
        //Color colorButton = new Color(button2.color.r, button2.color.g, button2.color.b, alfa);

        //uzor.DOColor(colorUzor, 1f);
        //button2.DOColor(colorUzor, 1f);
    }
}
