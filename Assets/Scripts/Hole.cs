using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Hole : MonoBehaviour
{
    public TextMeshProUGUI scoreBoard;
    public GameObject puttArea;
    public int holePar;
    public int holeScore;
    public AudioSource holeSound;

    void Start()
    {
        //Initiate score and scoreboard text
        holeScore = 0;
        scoreBoard.text = holePar + "\n" + holeScore;
    }

    //Update score and scoreboard text
    public void UpdateScore(int num)
    {
        holeScore = num;
        scoreBoard.text = holePar + "\n" + holeScore;
    }

    //Returns the starting position of the ball
    public Vector3 GetBallPos() {
        return puttArea.transform.position;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Golf Ball")) {
            holeSound.Play();

            GameObject.FindWithTag("Golf Ball").GetComponent<SphereCollider>().enabled = false;

            Destroy(GameObject.FindWithTag("Golf Ball"));
        }
    }
}
