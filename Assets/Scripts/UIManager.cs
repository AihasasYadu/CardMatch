using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static Action<int> TurnsCounterUpdated;
    public static Action<int> MatchesCounterUpdated;
    public static Action<int> ScoreCounterUpdated;
    public static Action PlayButtonTapped;
    public static Action HomeButtonTapped;
    public static Action NextLevelButtonTapped;

    [SerializeField]
    private GameObject mainMenuPanel = null;

    [SerializeField]
    private GameObject levelPanel = null;

    [SerializeField]
    private GameObject levelCompletePanel = null;

    [SerializeField]
    private TextMeshProUGUI turnsCounterText = null;

    [SerializeField]
    private TextMeshProUGUI matchesCounterText = null;

    [SerializeField]
    private TextMeshProUGUI currentLevelScoreText = null;

    [SerializeField]
    private Button playButton = null;

    [SerializeField]
    private Button homeButton = null;

    [SerializeField]
    private Button nextLevelButton = null;

    public void Start()
    {
        GameManager.OnLevelCompleted += ResetCounters;
        GameManager.OnLevelCompleted += ShowLevelCompletePanel;
        GameManager.OnGameCompleted += ShowMainMenu;
        TurnsCounterUpdated += UpdateTurnsCounter;
        MatchesCounterUpdated += UpdateMatchesCounter;
        ScoreCounterUpdated += UpdateCurrentLevelScore;
        playButton.onClick.AddListener(OnPlayButtonTapped);
        homeButton.onClick.AddListener(OnHomeButtonTapped);
        nextLevelButton.onClick.AddListener(OnNextLevelButtonTapped);
        ShowMainMenu();
    }

    private void UpdateTurnsCounter(int turns)
    {
        turnsCounterText.text = $"{turns}";
    }

    private void UpdateMatchesCounter(int matches)
    {
        matchesCounterText.text = $"{matches}";
    }

    private void UpdateCurrentLevelScore(int score)
    {
        currentLevelScoreText.text = $"{score}";
    }

    private void ResetCounters()
    {
        UpdateTurnsCounter(0);
        UpdateMatchesCounter(0);
        UpdateCurrentLevelScore(0);
    }

    private void OnPlayButtonTapped()
    {
        ShowLevelPanel();
        ResetCounters();
        PlayButtonTapped?.Invoke();
    }

    private void OnHomeButtonTapped()
    {
        ShowMainMenu();
        HomeButtonTapped?.Invoke();
    }

    private void OnNextLevelButtonTapped()
    {
        ShowLevelPanel();
        NextLevelButtonTapped?.Invoke();
    }

    private void ShowMainMenu()
    {
        mainMenuPanel.SetActive(true);
        levelPanel.SetActive(false);
        levelCompletePanel.SetActive(false);
    }

    private void ShowLevelPanel()
    {
        levelPanel.SetActive(true);
        mainMenuPanel.SetActive(false);
        levelCompletePanel.SetActive(false);
    }

    private void ShowLevelCompletePanel()
    {
        levelCompletePanel.SetActive(true);
        mainMenuPanel.SetActive(false);
        levelPanel.SetActive(false);
    }
}