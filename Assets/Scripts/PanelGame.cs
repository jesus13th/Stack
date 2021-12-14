using TMPro;

using UnityEngine;
using UnityEngine.SceneManagement;

public class PanelGame : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI ScoreText;
    [SerializeField] private TextMeshProUGUI highScoreText;
    [SerializeField] internal int score;
    [SerializeField] internal int highScore;

    private void Start() {
        ScoreText.text = score.ToString();        
        highScoreText.text = highScore.ToString();        
    }
    public void Play() => SceneManager.LoadScene(0);
    public void ShowLeaderBoard() { 
    
    }
    public void ShowSettings() { 
    
    }
}
