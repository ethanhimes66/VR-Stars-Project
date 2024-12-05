using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;

public class BallHitDetection : MonoBehaviour
{
    public TextMeshProUGUI npcTextBox;  // Assign the NPC text box in the Inspector
    private bool isTextVisible = false;
    private int hitValue;

    // Ensures the text box is hidden when the game starts

    void Start()
    {
        npcTextBox.gameObject.SetActive(false);
        hitValue = 0;
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Triggered by: " + other.gameObject.name);  // Logs the name of the object

        if (other.tag == "Golf Ball")
        {
            
            Debug.Log("Golf ball hit detected!");  // Log when the ball hits the NPC
            hitValue = (hitValue + 1) % 2;
            Debug.Log(hitValue);
            switch (hitValue)
            {
                case 1: 
                    npcTextBox.text ="Nice Hit!";
                    break;

                case 2:
                    npcTextBox.text = "Four!";
                    break;

                default:
                    break;
            }
            ShowTextBox();
        }
    }


    void ShowTextBox()
    {
        if (!isTextVisible)
        {
            npcTextBox.gameObject.SetActive(true);  // Show the text box
            isTextVisible = true;
            Invoke("HideTextBox", 3f);  // Hide after 3 seconds
        }
    }

    void HideTextBox()
    {
        npcTextBox.gameObject.SetActive(false);  // Hide the text box
        isTextVisible = false;
    }
}
