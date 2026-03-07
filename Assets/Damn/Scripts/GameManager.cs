using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameState CurrentState { get; private set; }

    [Header("UI")]
    public GameObject winPanel;

    [Header("Level")]
    public float winDelay = 1.5f;

    void Awake()
    {
        Instance = this;
        SetState(GameState.Idle);
    }

    public void SetState(GameState newState)
    {
        CurrentState = newState;

        winPanel.SetActive(newState == GameState.Win);

        if (newState == GameState.Win)
        {
            StartCoroutine(LoadNextLevel());
        }
    }

    IEnumerator LoadNextLevel()
    {
        yield return new WaitForSeconds(winDelay);

        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        int nextIndex = currentIndex + 1;

        if (nextIndex >= SceneManager.sceneCountInBuildSettings)
        {
            Debug.Log("WINNN!!");
            yield break;
        }

        SceneManager.LoadScene(nextIndex);
    }

    public bool IsRunning()
    {
        return CurrentState == GameState.Running;
    }
}