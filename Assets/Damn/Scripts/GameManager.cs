using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameState CurrentState { get; private set; }

    [Header("UI")]
    public GameObject winPanel;

    void Awake()
    {
        Instance = this;
        SetState(GameState.Idle);
    }

    public void SetState(GameState newState)
    {
        CurrentState = newState;

        winPanel.SetActive(newState == GameState.Win);
    }

    public bool IsRunning()
    {
        return CurrentState == GameState.Running;
    }
}