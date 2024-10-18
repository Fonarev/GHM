using TMPro;

using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.GemHunterMatch.Scripts.UI
{
    public class UIPopupWin : MonoBehaviour
    {
        [SerializeField] private Image spriteTitle;
        [SerializeField] private TextMeshProUGUI title;
        [SerializeField] private TextMeshProUGUI score;
        [SerializeField] private TextMeshProUGUI coinsAmount;
        [SerializeField] private UIButtonEntry next;
        [SerializeField] private UIButtonEntry exitMenu;

        public void Init(bool isConditions)
        {
            if (isConditions)
            {
                next.Init(ButtonType.Next);
            }
            else
            {
                next.Init(ButtonType.Restart);
            }
            exitMenu.Init(ButtonType.Menu);
        }
    }
}