using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro; // Import TextMeshPro namespace
using System;
using UnityEngine.XR;
using TMPro;

public class GolfBall : MonoBehaviour
{
    private int holeCount;
    private int totalPars;
    private GameObject[] holes;
    private Vector3 holePos;
    private Vector3 ballPos;
    private Vector3 previousBallPos;
    private float holeRadius;
    private int holePar;
    private int holeScore;

    public TextMeshProUGUI npcTextBox;
    private int hitValue;


    // Controllers for vibrations
    private InputDevice leftController;
    private InputDevice rightController;
    private bool hasCollided = false;

    public AudioSource hitSound;

    // TextMeshProUGUI for displaying messages
    public TextMeshProUGUI hitMessageText;

    private void Start()
    {
        npcTextBox.gameObject.SetActive(false);
        hitValue = 0;
        InitializeControllers();
        NewGame();

        // Hide the message initially
        if (hitMessageText != null)
        {
            hitMessageText.text = "";
            hitMessageText.enabled = false;
        }
    }

    private void InitializeControllers()
    {
        var inputDevices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.HeldInHand | InputDeviceCharacteristics.Controller, inputDevices);

        foreach (var device in inputDevices)
        {
            if (device.characteristics.HasFlag(InputDeviceCharacteristics.Right))
                rightController = device;
            else if (device.characteristics.HasFlag(InputDeviceCharacteristics.Left))
                leftController = device;
        }
    }

    private void TriggerHaptic(InputDevice controller)
    {
        if (controller.isValid)
        {
            float intensity = 0.5f;
            float duration = 0.2f;
            controller.SendHapticImpulse(0, intensity, duration);
            Debug.Log("Vibration sent");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((other.tag == "Golf Club" || other.tag == "Chipper") && !hasCollided)
        {
            hasCollided = true;

            previousBallPos = transform.position;


            Debug.Log("Golf ball hit detected!");  // Log when the ball hits the NPC
            System.Random random = new System.Random();

            hitValue = random.Next(1, 7);
            Debug.Log(hitValue);
            switch (hitValue)
            {
                case 1: 
                    npcTextBox.text ="Nice Hit!";
                    break;

                case 2:
                    npcTextBox.text = "Wow!";
                    break;
                case 3:
                    npcTextBox.text = "Nice Swing!";
                    break;
                case 4:
                    npcTextBox.text = "Fore!";
                    break;
                case 5:
                    npcTextBox.text = "Five!";
                    break;
                case 6:
                    npcTextBox.text = "May the course be with you";
                    break;
                    
            }

            if (npcTextBox != null)
            {
                npcTextBox.gameObject.SetActive(true); // Show the text
            }

            holeScore++;
            holes[holeCount].GetComponent<Hole>().UpdateScore(holeScore);
            Debug.Log(holeScore);
            // Changes how fast the ball gets hit when colliding with golf club
            GetComponent<Rigidbody>().velocity = other.GetComponent<GolfClub>().getVelocity() * 1.4f;
            TriggerHaptic(rightController);

            hitSound.Play();

            StartCoroutine(ResetCollisionFlag());
        } else if (other.tag == "Chipper" && !hasCollided) {
            hasCollided = true;

            previousBallPos = transform.position;

            holeScore++;
            holes[holeCount].GetComponent<Hole>().UpdateScore(holeScore);
            Debug.Log(holeScore);

            Vector3 clubVelocity = other.GetComponent<GolfClub>().getVelocity();
            Vector3 upwardForce = other.tag == "Chipper" ? new Vector3(0, 3, 0) : Vector3.zero;
            GetComponent<Rigidbody>().velocity = clubVelocity * 1.4f + upwardForce;

            TriggerHaptic(rightController);
            hitSound.Play();

            DisplayHitMessage($"Ball hit! Stroke: {holeScore}");

            StartCoroutine(ResetCollisionFlag());
        }
        else if (other.tag == "Water Trap" || other.tag == "Rough")
        {
            transform.position = previousBallPos;
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

            holeScore += 2;
            holes[holeCount].GetComponent<Hole>().UpdateScore(holeScore);
            Debug.Log("Penalty! Stroke count increased by 2.");

            DisplayHitMessage("Penalty! Stroke count increased by 2.");
        }

        TriggerHaptic(rightController);
        StartCoroutine(ResetCollisionFlag());
    }

    private IEnumerator ResetCollisionFlag()
    {
        yield return new WaitForSeconds(0.1f);
        hasCollided = false;
    }

    private void DisplayHitMessage(string message)
    {
        if (hitMessageText != null)
        {
            hitMessageText.text = message;
            hitMessageText.enabled = true;
            StartCoroutine(HideHitMessageAfterDelay(2f)); // Hide after 2 seconds
        }
    }

    private IEnumerator HideHitMessageAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (hitMessageText != null)
        {
            hitMessageText.text = "";
            hitMessageText.enabled = false;
        }
    }

    public void GetHole(int num)
    {
        if (num >= 0 && num <= holes.Length - 1 && holes[num] != null)
        {
            ballPos = holes[num].GetComponent<Hole>().GetBallPos() + new Vector3(0, transform.localScale.y / 2, 0);
            holePos = holes[num].transform.position;
            holeRadius = holes[num].transform.localScale.x / 2;
            holePar = holes[num].GetComponent<Hole>().holePar;
            holeScore = 0;
            Debug.Log("Hole #: " + holeCount + "\nHole Par :" + holePar);
        }
    }

    public void NewGame()
    {
        holeCount = 0;
        totalPars = 0;
        holes = GameObject.FindGameObjectsWithTag("Hole").OrderBy(go => go.name).ToArray();
        foreach (GameObject hole in holes)
        {
            totalPars += hole.GetComponent<Hole>().holePar;
            hole.GetComponent<Hole>().UpdateScore(0);
        }
        GetHole(holeCount);
        holeScore = 0;
    }
}
