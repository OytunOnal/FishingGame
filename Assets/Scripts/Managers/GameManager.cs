public class GameManager : Singleton<GameManager>
{
    private LevelManager levelManager;
    private PlayerInputManager playerInputManager;

    private void Start() 
    {
        levelManager = FindObjectOfType<LevelManager>();
        playerInputManager = FindObjectOfType<PlayerInputManager>();
        LevelSelectionManager.OnPlayClicked += StartGame;
    }

    private void OnDisable() => LevelSelectionManager.OnPlayClicked -= StartGame;

    private void StartGame(int level)
    {
        levelManager.OpenLevel(level);
        StartGame();
    }

    private void StartGame()
    {
        UIManager.Instance.InitializeGameUI();
        ScoreManager.Instance.ResetCurrentScore();
        EnablePlayerInput();
    }

    public void RestartGame()
    {
        UIManager.Instance.ClearScene();
        StartGame();
    }

    public void NextLevel()
    {
        UIManager.Instance.ClearScene();
        levelManager.OpenNextLevel();
        StartGame();
    }

    public void Win()
    {
        DisablePlayerInput();
        UIManager.Instance.PlayWinEffects();

        CommonLevelSavingManager.Instance.LevelCompleted(
            new CompletedLevelData(false, 0,
                    "Level " + (levelManager.GetLevelIndex() + 1),
                    levelManager.GetLevelIndex(), "PurrfectCatch"));
    }

    public void Lose()
    {
        DisablePlayerInput();
        UIManager.Instance.PlayLoseEffects();
    }

    private void EnablePlayerInput() => playerInputManager.EnableInput();
    private void DisablePlayerInput() => playerInputManager.DisableInput();

}
