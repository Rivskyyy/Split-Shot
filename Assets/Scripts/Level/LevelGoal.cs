using UnityEngine;

public class LevelGoal : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Player"))
        {
            CompleteLevel(other.gameObject);
        }
    }

    private void CompleteLevel(GameObject player)
    {
        Debug.Log("Level Passed!");

      
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ShowWin();
        }

      
        PlayerController pc = player.GetComponent<PlayerController>();
        if (pc != null)
        {
            pc.enabled = false;
        }

        Rigidbody rb = player.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }
}