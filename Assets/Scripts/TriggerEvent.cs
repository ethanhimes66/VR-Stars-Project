using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class TriggerEvent : MonoBehaviour
{
   public UnityEvent onTrigger;

    void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            if (onTrigger != null) {
                onTrigger.Invoke();
            }
        }
    }
}
