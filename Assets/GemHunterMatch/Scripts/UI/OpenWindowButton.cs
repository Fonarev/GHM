using System;

using UnityEngine;
using UnityEngine.UI;

namespace Assets.GemHunterMatch.Scripts.UI
{
    [RequireComponent(typeof(Button))]
    public class OpenWindowButton : MonoBehaviour
    {
        [field:SerializeField] public OpenButtonType type;
        private Button button => GetComponent<Button>();

        public void Init(Action<OpenButtonType> onClick)
        {
            button.onClick?.AddListener(() => onClick.Invoke(type));
        }

        public virtual void State(bool isState)
        {
            
        }
    }
}
