using System.Collections;

using UnityEngine;
using UnityEngine.UI;

namespace Assets.DailyRewards.Scripts.UI
{
    public abstract class PopupWin : MonoBehaviour
    {
        [SerializeField] protected Button closeButton;

        protected virtual void Open()
        {

        }
        protected virtual void Close()
        {

        }
    }
}