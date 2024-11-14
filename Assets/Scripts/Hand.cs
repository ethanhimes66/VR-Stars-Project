using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Hand : MonoBehaviour
{
    public GameObject golfClubPrefab;  // Drag your golf club prefab here in the Inspector
    public GameObject chipperPrefab;

    private GameObject spawnedGolfClub;
    private GameObject spawnedChipper;
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
            else if (controller.selectAction.action.ReadValue<float>() > 0.1f && spawnedChipper == null)
            {
                SpawnChipper();
            }
            else if (controller.selectAction.action.ReadValue<float>() <= 0.1f && spawnedChipper != null)
            {
                DespawnChipper();
            }
        }
    }

    void SpawnGolfClub()
    {
        // Ensure the current club prefab exists
        if (golfClubPrefab != null)
        {
            Vector3 spawnPosition = transform.position + transform.forward * positionOffset.z
                                    + transform.right * positionOffset.x
                                    + transform.up * positionOffset.y;

            Quaternion spawnRotation = transform.rotation * Quaternion.Euler(rotationOffset);

            spawnedGolfClub = Instantiate(golfClubPrefab, spawnPosition, spawnRotation);

            Rigidbody rb = spawnedGolfClub.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true;
            }

            spawnedGolfClub.transform.parent = transform;
        }
    }

    void SpawnChipper()
    {
        if (chipperPrefab != null)
        {
            // Calculate spawn position with an offset in front of the controller
            Vector3 spawnPosition = transform.position + transform.forward * positionOffset.z
                                + transform.right * positionOffset.x
                                + transform.up * positionOffset.y;

            // Calculate spawn rotation with an additional rotation offset
            Quaternion spawnRotation = transform.rotation * Quaternion.Euler(rotationOffset);

            spawnedChipper = Instantiate(chipperPrefab, spawnPosition, spawnRotation);

            // Make it kinematic so it doesn't fall
            Rigidbody rb = spawnedChipper.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true;
            }

            // Parent the golf club to the controller so it follows it
            spawnedChipper.transform.parent = transform;
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

    void DespawnChipper()
    {
        if (spawnedChipper != null)
        {
            Destroy(spawnedChipper);
            spawnedChipper = null;
        }
    }

}
