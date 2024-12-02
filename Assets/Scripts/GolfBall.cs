using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using UnityEngine;
using System;
using UnityEngine.XR;

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
    // Controllers for vibrations
    private InputDevice leftController;
    private InputDevice rightController;
    private bool hasCollided = false;

    public AudioSource hitSound;
    private void Start()
    {
        InitializeControllers();
        NewGame();
    }

    private void InitializeControllers()
    {
        // Fetch the connected devices
        var inputDevices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.HeldInHand | InputDeviceCharacteristics.Controller, inputDevices);

        // Assign the controllers (assuming the player is using both hands)
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
            // Intensity of the vibration (0.0f to 1.0f)
            float intensity = 0.5f;
            // Duration of the vibration (in seconds)
            float duration = 0.2f;

            // Send a haptic impulse to the controller
            controller.SendHapticImpulse(0, intensity, duration);
            Debug.Log("Vibration sent");
        }
    }

    
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Golf Club" && !hasCollided) 
        {
            hasCollided = true;

            previousBallPos = transform.position;

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
        
            Vector3 upwardForce = new Vector3(0, 3, 0); // This adds an upward motion
        
            GetComponent<Rigidbody>().velocity = clubVelocity * 1.4f + upwardForce; // Combine both velocities

            TriggerHaptic(rightController);

            hitSound.Play();

            StartCoroutine(ResetCollisionFlag());

        } else if (other.tag == "Water Trap" || other.tag == "Rough") {
            
            // Reset to the previous position and add 2 penalty strokes
            transform.position = previousBallPos;
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

            holeScore += 2;
            holes[holeCount].GetComponent<Hole>().UpdateScore(holeScore);
            Debug.Log(previousBallPos);
            Debug.Log("Penalty! Stroke count increased by 2.");
        } 
        // else if (other.CompareTag("Hole")) {
        //     holeSound.Play();
        // }

        // Trigger haptic feedback for right controller
        TriggerHaptic(rightController);

        StartCoroutine(ResetCollisionFlag());
    }

    private IEnumerator ResetCollisionFlag()
    {
        yield return new WaitForSeconds(0.1f);  // Adjust the delay as needed
        hasCollided = false;
    }


    private void Update()
    {
        //If past the edge of the hole
        // if (Vector3.Distance(transform.position, holePos) < holeRadius)
        // {
            //Turn off collider
            // GetComponent<SphereCollider>().enabled = false;

            // Debug.Log(PrintScore());
            // Destroy(gameObject);
        // }
    }

    public void GetHole(int num)
    {
        if (num >= 0 && num <= holes.Length - 1 && holes[num] != null)
        {
            //Set ball position
            ballPos = holes[num].GetComponent<Hole>().GetBallPos() + new Vector3(0, transform.localScale.y / 2, 0);
            //Set hole position
            holePos = holes[num].transform.position;
            //Set hole radius
            holeRadius = holes[num].transform.localScale.x / 2;
            //Set hole par
            holePar = holes[num].GetComponent<Hole>().holePar;
            //Set hole score
            holeScore = 0;
            //DEBUG
            Debug.Log("Hole #: " + holeCount + "\nHole Par :" + holePar);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Border")) {
            GetComponent<Rigidbody>().velocity = Vector3.Reflect(GetComponent<Rigidbody>().velocity, collision.contacts[0].normal) * 1F;
        }
    }

    public void Reset()
    {
        if (holeCount == holes.Length)
        {
            NewGame();
        }
        else
        {
            transform.position = ballPos;
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        }
    }

    public void NewGame()
    {
        //Set initial hole count
        holeCount = 0;
        //Set initial total pars
        totalPars = 0;
        //Find all holes
        holes = GameObject.FindGameObjectsWithTag("Hole").OrderBy(go => go.name).ToArray();
        //DEBUG
        //Debug.Log("Number of holes: " + holes.Length);
        foreach (GameObject hole in holes)
        {
            totalPars = totalPars + hole.GetComponent<Hole>().holePar;
            hole.GetComponent<Hole>().UpdateScore(0);
        }
        GetHole(holeCount);
        holeScore = 0;
    }

    public bool IsInPlay()
    {
        if (holeScore != 0)
        {
            return true;
        }
        return false;
    }

    private string PrintScore() {
        string Score = "";
        if (holeScore == 1) {
            Score = "Hole in one!";
        } else {
            float strokeCount = holeScore - holePar;
            if (strokeCount < 0)
            {
                Score = Math.Abs(strokeCount) + " under par!";
            }
            else if (strokeCount == 0)
            {
                Score = "Par.";
            }
            else
            {
                Score = strokeCount + " over par.";
            }
        }
        return Score;
    }
}

