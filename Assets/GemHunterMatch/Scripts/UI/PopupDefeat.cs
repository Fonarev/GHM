using Assets.AssetLoaders;
using Assets.GameMains.Scripts.Bank;
using Assets.GameMains.Scripts.Expansion;
using Assets.GemHunterMatch.Scripts;
using Assets.GemHunterMatch.Scripts.UI;

using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace Assets.GemHunterMatch.UI
{
    public class PopupDefeat : MonoBehaviour
    {
        [SerializeField] private UIButtonBayEntry bayMoves;
        [SerializeField] private UIButtonBayEntry RewardAdsMoves;
        [SerializeField] private UIButtonEntry exit;
        [SerializeField] private RectTransform container;

        public void Init(GamePlay gamePlay, Wallet wallet)
        {
            exit.Init(ButtonType.Menu);
            bayMoves.Init(BayType.Moves, gamePlay, wallet,gameObject);
            RewardAdsMoves.Init(BayType.RewardAds, gamePlay, wallet, gameObject);
        }

        public void Show(List<Goals> goals)
        {
            foreach (var goal in goals)
            {
               CoroutineHandler.StartRoutine(LoaderAsset.InstantiateAsset<UIGoalEntry>("GoalEntry", container, op => op.Init(goal)));
            }
        }
    }
}