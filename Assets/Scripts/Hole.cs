using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Hole : MonoBehaviour
{
    public TextMeshProUGUI scoreBoard;
    public TextMeshProUGUI finalScoreBoard;
    public GameObject puttArea;
    public int holeNum;
    public int holePar;
    public int holeScore;

    void Start()
    {
        //Initiate score and scoreboard text
        holeScore = 0;
        scoreBoard.text = holePar + "\n" + holeScore;
        // finalScoreBoard.text = holeNum + "\n" + holePar + "\n" + holeScore;
    }

    //Update score and scoreboard text
    public void UpdateScore(int num)
    {
        holeScore = num;
        scoreBoard.text = holePar + "\n" + holeScore;
        // finalScoreBoard.text = holeNum + "\n" + holePar + "\n" + holeScore;
    }

    //Returns the starting position of the ball
    public Vector3 GetBallPos() {
        return puttArea.transform.position;
    }
}
