using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{

    [SerializeField]
    private Text timeRemainingDisplayText;

    [SerializeField]
    private GameObject questionDisplay;

    [SerializeField]
    private GameObject roundEndDisplay;

    [SerializeField]
    private Text scoreDisplayText;

    [SerializeField]
    private Text questionDisplayText;

    [SerializeField]
    private SimpleObjectPool answerButtonObjectPool;

    [SerializeField]
    private Transform answerButtonParent;

    private DataController _dataController;
    private RoundData _currentRoundData;
    private QuestionData[] questionPool;

    private bool isRoundActive;
    private float timeRemaining;
    private int questionIndex;
    private int playerScore;
    private List<GameObject> answerButtonGameObjects = new List<GameObject>();


    // Start is called before the first frame update
    void Start()
    {
        _dataController = FindObjectOfType<DataController>();
        _currentRoundData = _dataController.GetCurrentRoundData(); 
        questionPool = _currentRoundData.questions; 
        timeRemaining = _currentRoundData.timeLimitInSeconds;    
        UpdateTimeRemainingDisplay();
        playerScore = 0;
        questionIndex = 0;
        ShowQuestion();
        isRoundActive = true;
    }

    private void ShowQuestion () {
        RemoveAnswerButtons();
        QuestionData questionData = questionPool[questionIndex];
        questionDisplayText.text = questionData.questionText;
        for (int i = 0; i < questionData.answers.Length; i++)
        {
            GameObject answerButtonGameObject = answerButtonObjectPool.GetObject();
            answerButtonGameObjects.Add(answerButtonGameObject);
            answerButtonGameObject.transform.SetParent(answerButtonParent);

            AnswerButton answerButton = answerButtonGameObject.GetComponent<AnswerButton>();
            answerButton.Setup(questionData.answers[i]); 
        }

    }

    public void AnsweButtonClicked (bool isCorrect) {
        if (isCorrect)
        {
            playerScore += _currentRoundData.pointsAddedForCorrectAnswer;
            scoreDisplayText.text = "Баллы: " + playerScore.ToString();
        }  

        if (questionPool.Length > questionIndex + 1)
        {
            questionIndex++;
            ShowQuestion();
        }  
        else {
            EndRound();
        }
    }

    public void EndRound () { 
        isRoundActive = false;
        questionDisplay.SetActive(false);
        roundEndDisplay.SetActive(true);
    }

    public void ReturnToMenu(){
        SceneManager.LoadScene("MenuScreen");
    }

    private void RemoveAnswerButtons(){
        while(answerButtonGameObjects.Count > 0) 
        {
            answerButtonObjectPool.ReturnObject(answerButtonGameObjects[0]);
            answerButtonGameObjects.RemoveAt(0);
        }
    }

    private void UpdateTimeRemainingDisplay()
    {
        timeRemainingDisplayText.text = "Время: " + Mathf.Round (timeRemaining).ToString ();
    }

    // Update is called once per frame
    void Update()
    {
        if (isRoundActive) 
        {
            timeRemaining -= Time.deltaTime;
            UpdateTimeRemainingDisplay();
            if (timeRemaining <= 0f)
            {
                EndRound();
            }
        }
    }
}
