using UnityEngine;

public class LevelGoal : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            //LevelManager.Instance.CompleteLevel();
            CompleteLevel();
        }
    }

    private void CompleteLevel()
    {
        //LevelManager.Instance.CompleteLevel();
        Debug.Log("Level Passed!");

        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}
