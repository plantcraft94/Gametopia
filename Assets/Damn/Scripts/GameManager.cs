using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameState CurrentState { get; private set; }

    [Header("UI")]
    public GameObject winPanel;
    public GameObject failPanel;

    void Awake()
    {
        Instance = this;
        SetState(GameState.Idle);
    }

    public void SetState(GameState newState)
    {
        CurrentState = newState;

        winPanel.SetActive(newState == GameState.Win);
        failPanel.SetActive(newState == GameState.Fail);
    }

    public bool IsRunning()
    {
        return CurrentState == GameState.Running;
    }
}