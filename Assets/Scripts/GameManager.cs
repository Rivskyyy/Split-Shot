using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("UI Setup")]
    [SerializeField] private GameObject generalUI;   
    [SerializeField] private GameObject gameOverUI;  
    [SerializeField] private GameObject levelCompletedUI;       

    public static GameManager Instance { get; private set; }

    private bool canRestart = false;

    private void Awake()
    {
        Instance = this;
        Application.targetFrameRate = 60;

        if (generalUI != null) generalUI.SetActive(false);
        if (gameOverUI != null) gameOverUI.SetActive(false);
        if (levelCompletedUI != null) levelCompletedUI.SetActive(false);
    }

    private void Update()
    {
       
        if (canRestart && Input.GetMouseButtonDown(0))
        {
            RestartLevel();
        }
    }

    public void ShowGameOver()
    {
        if (generalUI != null)
        {
            generalUI.SetActive(true);     
            gameOverUI.SetActive(true); 
            levelCompletedUI.SetActive(false);    

            canRestart = true;
        }
    }

    public void ShowWin()
    {
        if (generalUI != null)
        {
            generalUI.SetActive(true);      
            levelCompletedUI.SetActive(true);     
            gameOverUI.SetActive(false); 

            canRestart = true;
        }
    }

    public void RestartLevel()
    {
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}