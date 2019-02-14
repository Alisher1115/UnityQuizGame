using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnswerButton : MonoBehaviour
{
    [SerializeField]
    private Text answerText;
    private AnswerData _answerData;
    private GameController gameController;

    void Start()
    {
        gameController = FindObjectOfType<GameController>();
    }

    public void Setup (AnswerData answerData) {
        _answerData = answerData;
        answerText.text = _answerData.answerText;
    }

    public void HandleClick(){
        gameController.AnsweButtonClicked(_answerData.isCorrect);
    }
}
