using TMPro;
using UnityEngine;

public class ScoreText : MonoBehaviour
{
    public const string Score_Key = "Score";

    private static ScoreText instance;
    public static ScoreText Instance => instance ?? (instance = FindObjectOfType<ScoreText>());

    [SerializeField] private TextMeshProUGUI textScore;
    public int score;

    void Start() {
        instance = this;
        GameManager.OnCubeSpawned += OnCubeSpawned;
        PlayerPrefs.DeleteAll();
    }
    void OnDestroy() => GameManager.OnCubeSpawned -= OnCubeSpawned;
    private void OnCubeSpawned() => textScore.text = $"Score {++score}";
    public void InitializeScore() => textScore.text = "Score 0";
}