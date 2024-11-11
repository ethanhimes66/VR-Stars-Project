using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Hand : MonoBehaviour
{
    public GameObject golfClubPrefab;  // Drag your golf club prefab here in the Inspector

    private GameObject spawnedGolfClub;
    private ActionBasedController controller;
    public Vector3 positionOffset = new Vector3(0f, -0.2f, 0.5f); // Adjust these values to position in front of the controller
    public Vector3 rotationOffset = new Vector3(0f, -90f, 0f);    // Adjust these values for holding angle

    void Start()
    {
        controller = GetComponent<ActionBasedController>();
    }

    void Update()
    {
        if (controller)
        {
            if (controller.activateAction.action.ReadValue<float>() > 0.1f && spawnedGolfClub == null)
            {
                SpawnGolfClub();
            }
            else if (controller.activateAction.action.ReadValue<float>() <= 0.1f && spawnedGolfClub != null)
            {
                DespawnGolfClub();
            }
        }
    }

  

    void SpawnGolfClub()
    {
        if (golfClubPrefab != null)
        {
            // Calculate spawn position with an offset in front of the controller
            Vector3 spawnPosition = transform.position + transform.forward * positionOffset.z
                                + transform.right * positionOffset.x
                                + transform.up * positionOffset.y;

            // Calculate spawn rotation with an additional rotation offset
            Quaternion spawnRotation = transform.rotation * Quaternion.Euler(rotationOffset);

            spawnedGolfClub = Instantiate(golfClubPrefab, spawnPosition, spawnRotation);

            // Make it kinematic so it doesn't fall
            Rigidbody rb = spawnedGolfClub.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true;
            }

            // Parent the golf club to the controller so it follows it
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
