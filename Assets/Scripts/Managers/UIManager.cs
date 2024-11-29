using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private List<ParticleSystem> winParticles;

    private TimerDisplay timerDisplay;
    public string EnvironmentName = "Ocean";
    public List<SeaImage> seaImages;

    private void Start()
    {
        timerDisplay = FindObjectOfType<TimerDisplay>();
        Application.targetFrameRate = 60;
    }

    public void SetEnvironment(string environmentName)
    {
        EnvironmentName = environmentName;
        foreach (SeaImage seaImage in seaImages)
        {
            seaImage.EnvironmentSet(environmentName);
        }
    }

    public void InitializeGameUI()
    {
        CommonUIManager.Instance.SwitchPanel(CommonPanelType.Game);
        StartSpawning();
        RestartTimer();

        CatAnimation.Instance.CastFishingLine();
    }

    public void PlayWinEffects()
    {
        timerDisplay.StopTimer();
        foreach (var particle in winParticles) particle.Play();
        AudioManager.Instance.PlayEffect(SoundEffectType.Confetti);
        CatAnimation.Instance.Happy();
        StartCoroutine(EndGame(CommonPanelType.LevelCompleted));
    }

    public void PlayLoseEffects()
    {
        AudioManager.Instance.PlayEffect(SoundEffectType.GameOver);
        CatAnimation.Instance.Idle();
        StartCoroutine(EndGame(CommonPanelType.Lose));
    }

    IEnumerator EndGame(CommonPanelType panelType)
    {
        yield return new WaitForSeconds(1);
        CommonUIManager.Instance.SwitchPanel(panelType);
    }

    // Timer Controls
    public void SetTimer(float duration) => timerDisplay.SetTimer(duration);
    public void StopTimer() => timerDisplay.StopTimer();
    public void RestartTimer() => timerDisplay.RestartTimer();

    // Spawning Controls
    private void StartSpawning() => SpawnManager.Instance.StartSpawning();
    public void ClearScene()
    {
        CatAnimation.Instance.Idle();
        SpawnManager.Instance.ClearScene();

        foreach (var fish in GameObject.FindGameObjectsWithTag("Fish"))
        {
            fish.GetComponent<Fish>().Despawn();
        }
    }
}
