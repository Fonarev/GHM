using UnityEngine;

namespace Assets.GemHunterMatch.Scripts.GenerateGridBoard
{
    public class HintShowMatches
    {
        private  float sinceLastHint;
        private GameObject hintIndicator;

        private readonly float inactivityBeforeHint;
        private readonly MatchHandler matchHandler;
        private readonly Grid grid;

        public HintShowMatches(MatchHandler matchHandler,Grid grid, float inactivityBeforeHint)
        {
            this.matchHandler = matchHandler;
            this.grid = grid;
            this.inactivityBeforeHint = inactivityBeforeHint;
        }

        public void Instatiate(GameObject prefab)
        {
            hintIndicator = Object.Instantiate(prefab);
            hintIndicator.SetActive(false);
        }

        public void Show(bool incrementHintTimer)
        {
            if (incrementHintTimer)
            {
                //Nothing happened this frame, but the board was changed since last possible match check, so need to refresh
                if (matchHandler.boardChanged)
                {
                    matchHandler.FindAllPossibleMatch();
                    matchHandler.boardChanged = false;
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
        
        private void ShowHint(PossibleSwap match)
        {
            if (hintIndicator.activeSelf)
            {
                var startPos = grid.GetCellCenterWorld(match.StartPosition);
                var endPos = grid.GetCellCenterWorld(match.StartPosition + match.Direction);

                var current = hintIndicator.transform.position;
                current = Vector3.MoveTowards(current, endPos, 1.0f * Time.deltaTime);

                hintIndicator.transform.position = current == endPos ? startPos : current;
            }
            else
            {
                sinceLastHint += Time.deltaTime;
                if (sinceLastHint >= inactivityBeforeHint)
                {
                    hintIndicator.transform.position = grid.GetCellCenterWorld(match.StartPosition);
                    hintIndicator.SetActive(true);
                }
            }
        }
    }
}