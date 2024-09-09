using Assets.GameMains.Scripts.AudiosSources;
using Assets.GameMains.Scripts.Expansion;

using System.Collections;

using UnityEngine;
using UnityEngine.UI;

namespace Assets.GemHunterMatch.Scripts.UI
{
    [RequireComponent(typeof(Button))]
    public class UISelectLevelEntry : MonoBehaviour
    {
       private Button button => GetComponent<Button>();
    
       public void Init()
       {
            button.onClick.AddListener(() => AudioManager.instance.PlayEffect(EffectClip.click));
       }
        
    }
}