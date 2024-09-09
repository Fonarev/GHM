using System.Collections;

using UnityEngine;

namespace Assets.GemHunterMatch.Scripts.UI
{
    public class UISelectLevels : MonoBehaviour
    {
       public UISelectLevelEntry[] levels;
       public void Init()
       {
            foreach (var level in levels)
            {
                level.Init();
            }
        }
    }
}