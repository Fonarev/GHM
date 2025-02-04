using TMPro;

using UnityEngine;

namespace Assets.GemHunterMatch.Scripts.UI
{
    public class VictoryPopup : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI title;
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private UIButtonEntry nextButton;

        public void Init(int level, int score)
        {
            title.text = "level " + level.ToString();
            scoreText.text = "Score:" + score.ToString();

            nextButton.Init(ButtonType.Next);
        }
    }
}