using Assets.GameMains.Scripts;
using Assets.YG.Scripts;

using TMPro;

using UnityEngine;

namespace Assets.GemHunterMatch.Scripts.UI
{
    public class VictoryPopup : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI title;
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private UIButtonEntry nextButton;

        public void Init(GlobalMediator mediator, int level, int score)
        {
            title.text = Languages.GetContent("Level ") + level.ToString();
            scoreText.text = Languages.GetContent("Score: ") + score.ToString();

            nextButton.Init(mediator,ButtonType.Next);
        }
    }
}