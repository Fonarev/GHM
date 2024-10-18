using Assets.AssetLoaders;
using Assets.GameMains.Scripts.Expansion;

using Match3;

using UnityEngine;

namespace Assets.GemHunterMatch.Scripts.GenerateGridBoard
{
    public class HintShowMatches
    {
        private  float sinceLastHint;
        private GameObject hintIndicator;

        private readonly MatchHandler matchHandler;
        private readonly GridBoard gridBoard;
        private readonly VisualSetting visual;

        public HintShowMatches(MatchHandler matchHandler,GridBoard gridBoard, VisualSetting visual)
        {
            this.matchHandler = matchHandler;
            this.gridBoard = gridBoard;
            this.visual = visual;
        }

        public void Instatiate(Transform container = null)
        {
            CoroutineHandler.StartRoutine(LoaderAsset.InstantiateAsset(visual.HintReference, container, op =>
            {
                hintIndicator = op;
                hintIndicator.SetActive(false);
            }));
        }

        public void Show(bool incrementHintTimer)
        {
            if (hintIndicator != null)
            {
                if (incrementHintTimer)
                {
                    //Nothing happened this frame, but the board was changed since last possible match check, so need to refresh
                    if (gridBoard.BoardChanged)
                    {
                        matchHandler.FindAllPossibleMatch();
                        gridBoard.BoardChanged = false;
                    }

                    var match = matchHandler.GetMatch();

                    if (match != null)
                        ShowHint(match);
                }
                else
                {
                    hintIndicator.SetActive(false);
                    sinceLastHint = 0.0f;
                }
            }
        }

        private void ShowHint(PossibleSwap match)
        {
            if (hintIndicator.activeSelf)
            {
                var startPos = GridBoard.Instance.Grid.GetCellCenterWorld(match.StartPosition);
                var endPos = GridBoard.Instance.Grid.GetCellCenterWorld(match.StartPosition + match.Direction);

                var current = hintIndicator.transform.position;
                current = Vector3.MoveTowards(current, endPos, 1.0f * Time.deltaTime);

                hintIndicator.transform.position = current == endPos ? startPos : current;
            }
            else
            {
                sinceLastHint += Time.deltaTime;
                if (sinceLastHint >= visual.inactivityTimeBeforeHint)
                {
                    hintIndicator.transform.position = GridBoard.Instance.Grid.GetCellCenterWorld(match.StartPosition);
                    hintIndicator.SetActive(true);
                }
            }
        }
    }
}