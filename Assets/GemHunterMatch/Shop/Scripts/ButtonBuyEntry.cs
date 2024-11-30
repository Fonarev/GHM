using System;
using System.Collections;

using UnityEngine;
using UnityEngine.UI;

namespace Assets.GemHunterMatch.Shop.Scripts
{
    [RequireComponent(typeof(Button))]
    public class ButtonBuyEntry : MonoBehaviour
    {
        public ButtonBuyType typeBuy;
        private Button button => GetComponent<Button>();

        public void Init()
        {
            button.onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
          
        }
    }
}