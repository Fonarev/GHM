using Assets.AssetLoaders;
using Assets.GameMains.Scripts;
using Assets.GameMains.Scripts.AudiosSources;
using Assets.GameMains.Scripts.Expansion;
using Assets.GemHunterMatch.ShopStore.Scripts;
using Assets.GemHunterMatch.UI;
using Assets.YG.Scripts;

using Match3;

using System;
using System.Collections;
using System.Collections.Generic;

using TMPro;

using UnityEngine;
using UnityEngine.AddressableAssets;


namespace Assets.GemHunterMatch.Scripts.UI
{
    public class UIGamePlay : MonoBehaviour
    {
        [SerializeField] private UIPopupEntryHandler popupHandler;
        [SerializeField] private AssetReference popupLevelGoals;
        [SerializeField] private AssetReference popupWin;
        [SerializeField] private TextMeshProUGUI levelNumber;
        [SerializeField] private OpenWindowButton settingsButton;
        private GlobalMediator mediator;
        private GamePlay gamePlay;
        private LevelConfig level;
        public RectTransform rootGoals;
        [SerializeField] private RectTransform containerPopups;
        private Dictionary<int,UIGoalEntry> goals = new();
        public TextMeshProUGUI moveCounter;
        public TextMeshProUGUI coins;
        public TextMeshProUGUI score;
        public UIBonusGroup bonusGroup;
        private GameSetitngsUI settihgs;
        private ShopUI shop;

        private void OnDisable()
        {
            mediator.OnCoinsChanged -= ValueChange;
            mediator.OnOpenedShop -= OpenShop;
            gamePlay.OnGoalChanged -= GoalChange;
            gamePlay.OnMoveHappened -= MoveHappen;
            gamePlay.OnMoveTriger -= GamePlay_OnMoveTriger;
            gamePlay.OnAllGoalFinished -= Finished;
            gamePlay.OnShowMessages -= ShowMessages;
            gamePlay.OnMachtedGem -= MatchEffect;
            gamePlay.OnMachted -= OnMachted;
            gamePlay.OnAddScore -= OnAddScore;
        }

        private void OnAddScore(int oldScore, int newScore)
        {
            //score.text = "Score: " + newScore.ToString();
            StartCoroutine(ScrollScore(oldScore, newScore));
        }
        private IEnumerator ScrollScore(int oldScore,int newScore)
       {
            var content = Languages.GetContent("Score: ");
            var scroll = newScore - oldScore;

            while (scroll > 0)
            {
                var amount = oldScore += 1;
                score.text = content + amount.ToString();

                yield return scroll--;
            }
       }
        public void Initialize(GlobalMediator mediator, GamePlay gamePlay, LevelConfig level)
        {
            this.mediator = mediator;
            this.gamePlay = gamePlay;
       
            this.level = level;

            levelNumber.text = Languages.GetContent("Level ") + level.level.ToString();
            moveCounter.text = level.MaxMove.ToString();
            coins.text = mediator.Coins.ToString();
            InitSettingsButton();

            mediator.OnCoinsChanged += ValueChange;
            mediator.OnOpenedShop += OpenShop;

            gamePlay.OnGoalChanged += GoalChange;
            gamePlay.OnMoveHappened += MoveHappen;
            gamePlay.OnMoveTriger += GamePlay_OnMoveTriger;
            gamePlay.OnAllGoalFinished += Finished;
            gamePlay.OnShowMessages += ShowMessages;
            gamePlay.OnMachtedGem += MatchEffect;
            gamePlay.OnMachted += OnMachted;
            gamePlay.OnAddScore += OnAddScore;
            
            score.text = Languages.GetContent("Score: ") + 0.ToString();
            bonusGroup.Init(mediator, gamePlay);

            InitGoals(gamePlay);

            CoroutineHandler.StartRoutine(LoaderAsset.InstantiateAsset<UIPopupLevelGoals>(popupLevelGoals, containerPopups, op => op.Init(gamePlay)));
        }

        private void OpenShop(BonusGemBonusItem obj)
        {
            if (shop == null)
            {
                CoroutineHandler.StartRoutine(LoaderAsset.InstantiateAsset<ShopUI>("Shop", containerPopups, op =>
                {
                    shop = op;
                    op.Init(mediator);
                }));
            }
            else
            {
                shop.gameObject.SetActive(true);
            }

        }

        private void InitGoals(GamePlay gamePlay)
        {
            foreach (var goal in gamePlay.Goals)
            {
                CoroutineHandler.StartRoutine(LoaderAsset.InstantiateAsset<UIGoalEntry>("GoalEntry", rootGoals, op =>
                {
                    op.Init(goal);
                    goals[op.GetTypeGoal()] = op;
                }));
            }
        }

        private void InitSettingsButton()
        {
            settingsButton.Init((type) =>
            {
                AudioManager.instance.PlayEffect(EffectClip.click);

                if (settihgs != null)
                {
                    settihgs.gameObject.SetActive(!settihgs.gameObject.activeSelf);
                }
                else
                {
                    CoroutineHandler.StartRoutine(LoaderAsset.InstantiateAsset<GameSetitngsUI>("GameSettings", containerPopups, op =>
                    {
                        settihgs = op;
                        op.Init(mediator, true, (type) =>
                        {
                            AudioManager.instance.PlayEffect(EffectClip.click);
                            if (shop != null)
                            {
                                shop.gameObject.SetActive(!shop.gameObject.activeSelf);
                                settihgs.gameObject.SetActive(false);
                            }
                            else
                            {
                                CoroutineHandler.StartRoutine(LoaderAsset.InstantiateAsset<ShopUI>("Shop", containerPopups, op =>
                                {
                                    shop = op;
                                    settihgs.gameObject.SetActive(false);
                                    op.Init(mediator);
                                }));
                            }
                        });
                    }));

                }
            });
        }

        private void ShowMessages(string message)
        {
            CoroutineHandler.StartRoutine(LoaderAsset.InstantiateAsset<MessagesPopup>("MessagesPopup", containerPopups, op =>
            {
                if (YandexGame.Instance.Language == "ru")
                    op.Init();
            }));
        }

        private void OnMachted(UnderGem underGem)
        {
            popupHandler.Show(underGem.UISprite, underGem.transform.position, goals[underGem.GemType].transform.position,false);
        }

        private void GamePlay_OnMoveTriger(int moves)
        {
            StartCoroutine(LoaderAsset.InstantiateAsset("PopupWarning",containerPopups));
        }

        private void ValueChange(int value)
        {
            coins.text = value.ToString();
        }

        private void Finished(bool isCondition)
        {
            if (isCondition)
            {
                StartCoroutine(LoaderAsset.InstantiateAsset<VictoryPopup>(popupWin, containerPopups, op =>
                {
                    op.Init(mediator,level.level,gamePlay.Score);
                }));
            }
            else
                StartCoroutine(LoaderAsset.InstantiateAsset<PopupDefeat>("PopupDefeat", containerPopups, op => { op.Init(mediator,gamePlay); op.Show(gamePlay.Goals); })); 
        }

        private void MoveHappen(int move)
        {
            moveCounter.text = move.ToString();
        }

        public void MatchEffect(Gem gem)
        {
            if (gem != null)
                popupHandler.Show(gem.UISprite, gem.transform.position, goals[gem.GemType].transform.position);

        }

        private void GoalChange(int type, int count, bool isExecut)
        {
            if(goals.TryGetValue(type, out UIGoalEntry entry)) 
            {
                entry.Change(count,isExecut);
            }
            else
            {
                Debug.Log($"No type {type} UIGoalEntry");
            }
        }
    }
}