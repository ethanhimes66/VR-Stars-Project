using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.UI;
using TMPro;

public class Hand : MonoBehaviour
{
    // public GameObject golfClubPrefab;  // Drag your golf club prefab here in the Inspector
    public GameObject[] chipperPrefabs;
    public GameObject[] putterPrefabs;

    private GameObject spawnedPutter;
    private GameObject spawnedChipper;

    private ActionBasedController controller;
    public Vector3 positionOffset = new Vector3(0f, -0.2f, 0.5f); // Adjust these values for position in front of the controller
    public Vector3 rotationOffset = new Vector3(0f, -90f, 0f);    // Adjust these values for holding angle

    private int currentPutterIndex = 1; // Index to track the current putter prefab
    private int currentChipperIndex = 1; // Index to track the current putter prefab
    private int puttOrChip = 0;


    void Start()
    {
        controller = GetComponent<ActionBasedController>();
    }

    void Update()
    {
        if (controller)
        {
            // Check if the trigger button is pressed to spawn or despawn the current club
            if (controller.activateAction.action.ReadValue<float>() > 0.1f)
            {
                if (spawnedPutter == null && puttOrChip == 0)
                {
                    SpawnGolfClub();
                } else if (spawnedChipper == null && puttOrChip == 1) {
                    SpawnChipper();
                }
            }
            else if (controller.activateAction.action.ReadValue<float>() <= 0.1f)
            {
                DespawnGolfClub();
                DespawnChipper();
            }
        }
    }

    void SpawnGolfClub()
    {
        // Destroy the currently spawned putter if there is one
        if (spawnedPutter != null)
        {
            Destroy(spawnedPutter);
        }
        if (spawnedChipper != null)
        {
            Destroy(spawnedChipper);
        }

        // Ensure the current club prefab exists
        if (putterPrefabs.Length > 0)
        {
            GameObject club = putterPrefabs[currentPutterIndex];

            if (currentPutterIndex < 1)
            {
                positionOffset.Set(0.15f, 0.15f, 0.07f);


                Vector3 spawnPosition = transform.position + transform.forward * positionOffset.z
                                    + transform.right * positionOffset.x
                                    + transform.up * positionOffset.y;

                Quaternion spawnRotation = transform.rotation * Quaternion.Euler(rotationOffset);

                spawnedPutter = Instantiate(club, spawnPosition, spawnRotation);

                Rigidbody rb = spawnedPutter.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.isKinematic = true;
                }

                spawnedPutter.transform.parent = transform;
            }
            else if (currentPutterIndex == 1)
            {
                positionOffset.Set(0.15f, 0.15f, 0.2f);


                Vector3 spawnPosition = transform.position + transform.forward * positionOffset.z
                                    + transform.right * positionOffset.x
                                    + transform.up * positionOffset.y;

                Quaternion spawnRotation = transform.rotation * Quaternion.Euler(rotationOffset);

                spawnedPutter = Instantiate(club, spawnPosition, spawnRotation);

                Rigidbody rb = spawnedPutter.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.isKinematic = true;
                }

                spawnedPutter.transform.parent = transform;
            }
            else if (currentPutterIndex > 1)
            {
                positionOffset.Set(0.15f, 0.15f, 0.4f);


                Vector3 spawnPosition = transform.position + transform.forward * positionOffset.z
                                    + transform.right * positionOffset.x
                                    + transform.up * positionOffset.y;

                Quaternion spawnRotation = transform.rotation * Quaternion.Euler(rotationOffset);

                spawnedPutter = Instantiate(club, spawnPosition, spawnRotation);

                Rigidbody rb = spawnedPutter.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.isKinematic = true;
                }

                spawnedPutter.transform.parent = transform;
            }
        }
    }

    void SpawnChipper()
    {
        // Destroy the currently spawned putter if there is one
        if (spawnedChipper != null)
        {
            Destroy(spawnedChipper);
        }
        if (spawnedPutter != null)
        {
            Destroy(spawnedPutter);
        }


        // Ensure the current club prefab exists
        if (chipperPrefabs.Length > 0)
        {
            GameObject club = chipperPrefabs[currentChipperIndex];

            if (currentChipperIndex < 1)
            {
                positionOffset.Set(0.15f, 0.15f, 0.07f);


                Vector3 spawnPosition = transform.position + transform.forward * positionOffset.z
                                    + transform.right * positionOffset.x
                                    + transform.up * positionOffset.y;

                Quaternion spawnRotation = transform.rotation * Quaternion.Euler(rotationOffset);

                spawnedChipper = Instantiate(club, spawnPosition, spawnRotation);

                Rigidbody rb = spawnedChipper.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.isKinematic = true;
                }

                spawnedChipper.transform.parent = transform;
            }
            else if (currentChipperIndex == 1)
            {
                positionOffset.Set(0.15f, 0.15f, 0.2f);


                Vector3 spawnPosition = transform.position + transform.forward * positionOffset.z
                                    + transform.right * positionOffset.x
                                    + transform.up * positionOffset.y;

                Quaternion spawnRotation = transform.rotation * Quaternion.Euler(rotationOffset);

                spawnedChipper = Instantiate(club, spawnPosition, spawnRotation);

                Rigidbody rb = spawnedChipper.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.isKinematic = true;
                }

                spawnedChipper.transform.parent = transform;
            }
            else if (currentChipperIndex > 1)
            {
                positionOffset.Set(0.15f, 0.15f, 0.4f);


                Vector3 spawnPosition = transform.position + transform.forward * positionOffset.z
                                    + transform.right * positionOffset.x
                                    + transform.up * positionOffset.y;

                Quaternion spawnRotation = transform.rotation * Quaternion.Euler(rotationOffset);

                spawnedChipper = Instantiate(club, spawnPosition, spawnRotation);

                Rigidbody rb = spawnedChipper.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.isKinematic = true;
                }

                spawnedChipper.transform.parent = transform;
            }
        }
    }

    void DespawnGolfClub()
    {
        if (spawnedPutter != null)
        {
            Destroy(spawnedPutter);
            spawnedPutter = null;
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

    public void SwitchToPutter()
    {
        puttOrChip = 0;
    }

    public void SwitchToChipper()
    {
        puttOrChip = 1;
    }

    // Function to increase the putter index and switch the putter prefab
    public void IncreasePutter()
    {
        if (currentPutterIndex + 1 < putterPrefabs.Length)
        {
            currentPutterIndex = (currentPutterIndex + 1);
            Debug.Log("current putter index:" + currentPutterIndex);
            SpawnGolfClub();
        }
        SpawnGolfClub();
    }

    // Function to decrease the putter index and switch the putter prefab
    public void DecreasePutter()
    {
        if (currentPutterIndex - 1 >= 0)
        {
            Debug.Log("putter greater than 0!!!!!!!!!");
            currentPutterIndex = (currentPutterIndex - 1);
            Debug.Log("current putter index:" + currentPutterIndex);
            SpawnGolfClub();
        }
        SpawnGolfClub();
    }

    // Function to increase the putter index and switch the putter prefab
    public void IncreaseChipper()
    {
        if (currentChipperIndex + 1 < chipperPrefabs.Length)
        {
            currentChipperIndex = (currentChipperIndex + 1);
            Debug.Log("current putter index:" + currentChipperIndex);
            SpawnChipper();
        }
        SpawnChipper();
    }

    // Function to decrease the putter index and switch the putter prefab
    public void DecreaseChipper()
    {
        if (currentChipperIndex - 1 >= 0)
        {
            Debug.Log("putter greater than 0!!!!!!!!!");
            currentChipperIndex = (currentChipperIndex - 1);
            Debug.Log("current putter index:" + currentChipperIndex);
            SpawnChipper();
        }
        SpawnChipper();
    }

}
