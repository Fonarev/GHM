using Assets.YG.Scripts;

using TMPro;

using UnityEngine;

namespace Assets.GemHunterMatch.Scripts.UI
{
    public class LBUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI rankPlayer;
        [SerializeField] private TextMeshProUGUI scorePlayer;
        public void Init()
        {
            if (YandexGame.Instance.lbData.thisPlayer != null)
            {
                rankPlayer.text = YandexGame.Instance.lbData.thisPlayer.rank.ToString();
                scorePlayer.text = YandexGame.Instance.lbData.thisPlayer.score.ToString();
            }
            else
            {
                rankPlayer.text = YandexGame.Instance.lbData.thisPlayer.rank.ToString();
                scorePlayer.text = YandexGame.Instance.progressData.Score.ToString();
            }

        }
    }
}