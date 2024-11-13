using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Hand : MonoBehaviour
{
    public GameObject[] clubPrefabs; // Array for different club prefabs (set golf club and chipper in Inspector)
    
    private int currentClubIndex = 0; // Tracks which club is currently selected
    private GameObject spawnedGolfClub;
    private ActionBasedController controller;
    public Vector3 positionOffset = new Vector3(0f, -0.2f, 0.5f); // Adjust these values for position in front of the controller
    public Vector3 rotationOffset = new Vector3(0f, -90f, 0f);    // Adjust these values for holding angle

    void Start()
    {
        controller = GetComponent<ActionBasedController>();
    }

    void Update()
    {
        if (controller)
        {
            // Check if the trigger button is pressed to spawn or despawn the current club
            if (controller.activateAction.action.ReadValue<float>() > 0.1f && spawnedGolfClub == null)
            {
                SpawnGolfClub();
            }
            else if (controller.activateAction.action.ReadValue<float>() <= 0.1f && spawnedGolfClub != null)
            {
                DespawnGolfClub();
            }

            // Check if the grip button is pressed to cycle the club type
            if (controller.selectAction.action.triggered && spawnedGolfClub == null)
            {
                CycleClub();
            }
        }
    }

    void CycleClub()
    {
        // Cycle to the next club in the array
        currentClubIndex = (currentClubIndex + 1) % clubPrefabs.Length;
    }

    void SpawnGolfClub()
    {
        // Ensure the current club prefab exists
        if (clubPrefabs.Length > 0 && clubPrefabs[currentClubIndex] != null)
        {
            Vector3 spawnPosition = transform.position + transform.forward * positionOffset.z
                                    + transform.right * positionOffset.x
                                    + transform.up * positionOffset.y;

            Quaternion spawnRotation = transform.rotation * Quaternion.Euler(rotationOffset);

            spawnedGolfClub = Instantiate(clubPrefabs[currentClubIndex], spawnPosition, spawnRotation);

            Rigidbody rb = spawnedGolfClub.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true;
            }

            spawnedGolfClub.transform.parent = transform;
        }
    }

    void DespawnGolfClub()
    {
        if (spawnedGolfClub != null)
        {
            Destroy(spawnedGolfClub);
            spawnedGolfClub = null;
        }
    }
}
