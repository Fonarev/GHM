using Assets.AssetLoaders;
using Assets.DailyRewards.Scripts;
using Assets.DailyRewards.Scripts.UI;
using Assets.GameMains.Scripts;
using Assets.GameMains.Scripts.AudiosSources;
using Assets.GameMains.Scripts.Expansion;
using Assets.GemHunterMatch.ShopStore.Scripts;

using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Assets.GemHunterMatch.Scripts.UI
{
    public class OpenWindowButtonGroup : MonoBehaviour
    {
        public Action<OpenButtonType> OnOpenWindow;
        public Action<bool> OnChangedState;

        [SerializeField] private OpenWindowButton[] openWindow;
        private GlobalMediator mediator;
        private DailyRewardsService dailyService;
        private Transform container;
        private Dictionary<OpenButtonType, GameObject> openedWindows = new();
        private Dictionary<OpenButtonType, OpenWindowButton> openWindowButtons = new();
        //private void OnDisable()
        //{
        //    dailyService.OnClaimReward -= ChangeState;
        //    OnOpenWindow -= Open;
        //}
        public void Init(GlobalMediator mediator, DailyRewardsService dailyService,Transform container)
        {
            this.mediator = mediator;
            this.dailyService = dailyService;
            this.container = container;

            foreach (var button in openWindow)
            {
                openWindowButtons[button.type] = button;
                button.Init((type) =>
                {
                    AudioManager.instance.PlayEffect(EffectClip.click);
                    Open(type);
                });
            }
            dailyService.OnClaimReward += ChangeState;
            OnOpenWindow += Open;
        }
        private void ChangeState(OpenButtonType type,bool isState)
        {
            if (openWindowButtons.TryGetValue(type, out var button))
                button.State(isState);
        }
        public void Open(OpenButtonType type)
        {
            if (!openedWindows.ContainsKey(type))
            {
                switch (type)
                {
                    case OpenButtonType.Settings:
                        CoroutineHandler.StartRoutine(LoaderAsset.InstantiateAsset<GameSetitngsUI>("GameSettings", container, op =>
                        {
                            openedWindows[type] = op.gameObject;
                            op.Init(mediator, false,(type) => { Open(type); }); 
                        }));

                        break;

                    case OpenButtonType.Shop:
                        CoroutineHandler.StartRoutine(LoaderAsset.InstantiateAsset<ShopUI>("Shop", container, op =>
                        {
                            openedWindows[type] = op.gameObject;
                            op.Init(mediator);
                        }));
                        break;

                    case OpenButtonType.Reward:
                        CoroutineHandler.StartRoutine(LoaderAsset.InstantiateAsset<RewardPopupUI>("RewardPopup", container, op =>
                        {
                            var reward = dailyService.GetReward(false);
                            op.Init(reward, () => 
                            {
                                op.gameObject.SetActive(false);
                                mediator.Add(reward.amount);
                                Addressables.ReleaseInstance(op.gameObject);
                                Open(OpenButtonType.DailyRewards);
                            });
                        }));

                        break;

                    case OpenButtonType.DailyRewards:
                        CoroutineHandler.StartRoutine(LoaderAsset.InstantiateAsset<DailyRewardsUI>("DailyRewards", container, op =>
                        {
                            openedWindows[type] = op.gameObject;
                            op.Init(dailyService);
                        }));

                        break;

                }
            }
            else
            {
                openedWindows[type].SetActive(!openedWindows[type].activeSelf);
            }
          
        }
    }
}