using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;

public class ColliderSpeech : MonoBehaviour
{
    public TextMeshProUGUI textMesh;
    // Start is called before the first frame update
    void Start()
    {
        // if (textMesh != null)
        // {
        //     textMesh.gameObject.SetActive(false); 
        // }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the player entered the trigger area
        if (other.CompareTag("Player"))
        {
            if (textMesh != null)
            {
                textMesh.gameObject.SetActive(true); // Show the text
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Hide the text when the player exits the trigger area
        if (other.CompareTag("Player"))
        {
            textMesh.gameObject.SetActive(false);  
        }
    }
}
