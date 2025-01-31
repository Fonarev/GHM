using Assets.AssetLoaders;
using Assets.GameMains.Scripts.Expansion;
using Assets.GemHunterMatch.Scripts.GenerateGridBoard;

using System.Collections;

using UnityEngine;
using UnityEngine.UI;

namespace Assets.GemHunterMatch.Scripts.UI
{
    public class ButtonOpenWindow : MonoBehaviour
    {
        public OpenButtonType type;
        [SerializeField] private Button button;
        private GameObject win;
        private Transform container;

        public virtual void Init(Transform container)
        {
            this.container = container;
            button.onClick.AddListener(onClick);
        }

        public void onClick()
        {
            switch (type)
            {
                case OpenButtonType.Settings:
                    {
                        if (win != null)
                        {
                            win.SetActive(!win.activeSelf);
                           
                        }
                        else
                        {
                            CoroutineHandler.StartRoutine(LoaderAsset.InstantiateAsset<GameSetitngsUI>("GameSettingsUI", container, op =>
                            {
                                win = op.gameObject;
                                op.Init();
                            }));
                        }

                        break;
                    }
            

            }
        }

    }

}
