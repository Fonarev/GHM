using UnityEngine;
using UnityEngine.UI;

namespace Assets.GemHunterMatch.ShopStore.Scripts
{
    public class OpenButtonShop : MonoBehaviour
    {
        private Button button => GetComponent<Button>();

        public void Init(ShopUI shop)
        {

            button.onClick.AddListener(() => { shop.gameObject.SetActive(true); });
        }

      
    }
}