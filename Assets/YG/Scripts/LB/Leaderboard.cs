using System.Runtime.InteropServices;

using UnityEngine;

namespace Assets.YG.Scripts.LB
{
    public class Leaderboard 
    {
        private readonly YandexGame yandex;

        public Leaderboard(YandexGame YG) => yandex = YG;

        [DllImport("__Internal")] private static extern void InitLeaderboard();
        [DllImport("__Internal")] private static extern void SetLeaderboardScores(string nameLB, int score);
        [DllImport("__Internal")] private static extern void GetLeaderboardScores(string nameLB, int maxQuantityPlayers, int quantityTop, int quantityAround, string photoSizeLB, bool auth);

        public void InitializeLB()
        {
#if !UNITY_EDITOR
        InitLeaderboard();
#endif
        }

        public void SetScores(string nameLB, int score)
        {
#if !UNITY_EDITOR
            SetLeaderboardScores(nameLB, score);
#endif
            yandex.Message($"Set Liderboard '{nameLB}' Record: {score}");
        }

        public void GetScores(string nameLB, int maxQuantityPlayers, int quantityTop, int quantityAround, string photoSizeLB, bool auth)
        {
#if !UNITY_EDITOR
            GetLeaderboardScores(nameLB, maxQuantityPlayers, quantityTop, quantityAround, photoSizeLB, auth);
#endif
            yandex.Message("Get Leaderboard");
        }

        public void LeaderboardEntries(string data)
        {
            yandex.Message($"{data}");
            JsonLB jsonLB = JsonUtility.FromJson<JsonLB>(data);

            LBData lbData = new LBData()
            {
                technoName = jsonLB.technoName,
                isDefault = jsonLB.isDefault,
                isInvertSortOrder = jsonLB.isInvertSortOrder,
                decimalOffset = jsonLB.decimalOffset,
                type = jsonLB.type,
                entries = jsonLB.entries,
                players = new LBPlayerData[jsonLB.names.Length],
                thisPlayer = null
            };

            for (int i = 0; i < jsonLB.names.Length; i++)
            {
                lbData.players[i] = new LBPlayerData();
                lbData.players[i].name = jsonLB.names[i];
                lbData.players[i].rank = jsonLB.ranks[i];
                lbData.players[i].score = jsonLB.scores[i];
                lbData.players[i].photo = jsonLB.photos[i];
                lbData.players[i].uniqueID = jsonLB.uniqueIDs[i];


                if (jsonLB.uniqueIDs[i] == yandex.player.playerId)
                {
                    lbData.thisPlayer = new LBThisPlayerData
                    {
                        rank = jsonLB.ranks[i],
                        score = jsonLB.scores[i]
                    };
                }
            }
            yandex.SetLB(lbData);
        }

    }
}
