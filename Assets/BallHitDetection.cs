using UnityEngine;

public class BallHitDetection : MonoBehaviour
{
    public GameObject npcTextBox;  // Assign the NPC text box in the Inspector
    private bool isTextVisible = false;

    // Ensures the text box is hidden when the game starts
    void Start()
    {
        npcTextBox.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Triggered by: " + other.gameObject.name);  // Logs the name of the object

        if (other.gameObject.CompareTag("Golf Ball"))
        {
            Debug.Log("Golf ball hit detected!");  // Log when the ball hits the NPC
            ShowTextBox();
        }
    }


    void ShowTextBox()
    {
        if (!isTextVisible)
        {
            npcTextBox.SetActive(true);  // Show the text box
            isTextVisible = true;
            Invoke("HideTextBox", 3f);  // Hide after 3 seconds
        }
    }

    void HideTextBox()
    {
        npcTextBox.SetActive(false);  // Hide the text box
        isTextVisible = false;
    }
}
