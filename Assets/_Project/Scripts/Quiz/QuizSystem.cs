using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
public enum StateQuiz { correctAnswer, nonCorrectAnswer, quizProcess }
public class QuizSystem : MonoBehaviour
{
    [Serializable]
    public class Question
    {
        [TextArea(2, 5)]
        public string InputQuest;
        [TextArea(2, 4)]
        public List<string> InputAnswer = new List<string>(3);
    }
    [SerializeField] private List<Question> questions;
    [SerializeField] private TextMeshProUGUI quests;
    [SerializeField] private List<AnswerButton> answersButton;
    [SerializeField] private TextMeshProUGUI recordText;
    private List<Question> cashQuestion = new List<Question>();
    public List<AnswerButton> AnswerButtons => answersButton;


    private int rnd;
    private string rightAnswer;
    private AnswerButton buttonAnswer;
    private Coroutine CoroutineRightAnswer;
    public const string LAST_RECORD = "LAST_RECORD";
    public const string LAST_DAY = "LAST_DAY";
    public int currentRecord;
    public int lastRecord;
    public Action<string> onGeneratedQuiz;

    public event Action<StateQuiz> OnPlayerReplied;
    public void Init(GameWindow gameWindow)
    {
        answersButton.ForEach(a => a.Init(gameWindow));
        foreach (var item in questions)
        {
            var q = new Question();
            q.InputQuest = item.InputQuest;
            q.InputAnswer = new List<string>();
            foreach (var answ in item.InputAnswer)
            {
                string a = answ;
                q.InputAnswer.Add(a);
            }
            cashQuestion.Add(q);
        }
        QuestionGenerate();
    }
    public void OnSelectAnswer()
    {
        ClickAnswer();
        answersButton.ForEach(b => b.Button.interactable = false);

        StartCoroutine(CorDelay());

        IEnumerator CorDelay()
        {
            yield return new WaitForSecondsRealtime(2f);
            OnPlayerReplied?.Invoke(StateQuiz.quizProcess);
            QuestionGenerate();
            quests.enabled = true;
            answersButton.ForEach(b => b.Button.image.color = Color.white);
            answersButton.ForEach(b => b.DoFadeButton(0.5f));
            yield return new WaitForSecondsRealtime(0.5f);
            answersButton.ForEach(b => b.Button.interactable = true);
        }
    } 
    public void NewGame()
    {
        if(PlayerPrefs.HasKey(LAST_RECORD))
        {
            lastRecord = PlayerPrefs.GetInt(LAST_RECORD);
        }
        if(lastRecord > 0)
        {
            recordText.text = "Максимальный рекорд за сегодня " + lastRecord;
        }
        else
        {
            recordText.text = "";
        }
      
        DateTime dateTime = DateTime.Now;
        if(PlayerPrefs.HasKey(LAST_DAY))
        {
            if(dateTime.Day != PlayerPrefs.GetInt(LAST_DAY))
            {
                PlayerPrefs.DeleteKey(LAST_DAY);
                PlayerPrefs.DeleteKey(LAST_RECORD);
            }
        }
        else
        {
            PlayerPrefs.GetInt(LAST_DAY, dateTime.Day);
        }
        QuestionGenerate();
    }
    private void QuestionGenerate()
    {
        if (CoroutineRightAnswer != null)
        {
            StopCoroutine(CoroutineRightAnswer);
            CoroutineRightAnswer = null;
        }
        answersButton.ForEach(b => b.SetCheckerRightAnswer(false));
      
        if (questions.Count > 2)
        {
            rnd = UnityEngine.Random.Range(0, questions.Count);
            quests.text = questions[rnd].InputQuest;
            if (MainContainer.instance.MenuWindow.GameWindow.IsOpened)
            {
                onGeneratedQuiz?.Invoke(quests.text);
            }
            rightAnswer = questions[rnd].InputAnswer[0];

            for (int i = 0; i < answersButton.Count; i++)
            {
                var r = UnityEngine.Random.Range(0, questions[rnd].InputAnswer.Count);
                answersButton[i].TextButton.text = questions[rnd].InputAnswer[r];
                if (answersButton[i].TextButton.text == rightAnswer)
                {
                    answersButton[i].SetCheckerRightAnswer(true);
                  
                }
                questions[rnd].InputAnswer.RemoveAt(r);
            }
            questions.RemoveAt(rnd);
        }
        else
        {
            //QuestionGenerate();

            foreach (var item in cashQuestion)
            {
                var q = new Question();
                q.InputQuest = item.InputQuest;
                q.InputAnswer = new List<string>();
                foreach (var answ in item.InputAnswer)
                {
                    string a = answ;
                    q.InputAnswer.Add(a);
                }
                questions.Add(q);
            }
            foreach (var item in cashQuestion)
            {
                var q = new Question();
                q.InputQuest = item.InputQuest;
                q.InputAnswer = new List<string>();
                foreach (var answ in item.InputAnswer)
                {
                    string a = answ;
                    q.InputAnswer.Add(a);
                }
                questions.Add(q);
            }


            QuestionGenerate();

            return;

        }
    }

    private void ReadBotText(UnityWebRequest request)
    {
        throw new NotImplementedException();
    }

    private void ScoreUp()
    {
        currentRecord += 1;

       
    }

    private void ShowScoreText()
    {
        if (currentRecord > lastRecord)
        {
            recordText.text = "Новый рекорд " + currentRecord + "!";
            PlayerPrefs.SetInt(LAST_RECORD, currentRecord);
        }
        else if (lastRecord > 0)
        {
            recordText.text = "Счёт " + currentRecord + "/" + lastRecord;
        }
        else
        {
            recordText.text = "Счёт " + currentRecord;
        }
    }

    public void ClickAnswer()
    {
        var text = buttonAnswer.TextButton.text;

        if (rightAnswer == text)
        {
            OnPlayerReplied?.Invoke(StateQuiz.correctAnswer);
            buttonAnswer.Button.image.color = Color.green;
            answersButton.ForEach(b => b.DoFadeButton(0f));
            
            ScoreUp();
        }
        else
        {
            OnPlayerReplied?.Invoke(StateQuiz.nonCorrectAnswer);
            buttonAnswer.Button.image.color = Color.red;
            answersButton.ForEach(b => b.DoFadeButton(0f));
           
            ShowRightAnswer();
        }
        ShowScoreText();
    }

    private void ShowRightAnswer()
    {
        var b = answersButton.FirstOrDefault(a => a.IsRightAnswer);
        if (b == null) return;
        CoroutineRightAnswer = StartCoroutine(CorShowRightAnswer(b));
    }
    IEnumerator CorShowRightAnswer(AnswerButton answerButton)
    {
        //answerButton.Button.image.color = Color.green;

        answerButton.Button.image.color = Color.white;
        yield return new WaitForSeconds(0.2f);
        answerButton.Button.image.color = Color.green;
        yield return new WaitForSeconds(0.2f);
        answerButton.Button.image.color = Color.white;
        yield return new WaitForSeconds(0.2f);
        answerButton.Button.image.color = Color.green;
        yield return new WaitForSeconds(0.2f);
        answerButton.Button.image.color = Color.white;
        yield return new WaitForSeconds(0.2f);
        answerButton.Button.image.color = Color.green;
        yield return new WaitForSeconds(0.2f);

    }

    public void SetButtonAnswer(AnswerButton buttonAnswer)
    {
        this.buttonAnswer = buttonAnswer;
    }
}