using Assets.GameMains.Scripts;
using Assets.YG.Scripts.LB;

using Newtonsoft.Json;

using System;
using System.Collections;
using System.Runtime.InteropServices;

using UnityEngine;

namespace Assets.YG.Scripts
{
    public class YandexGame : MonoBehaviour
    {
        public event Action<int> OnRewardedVideo;
        public event Action<bool> OnNowAdsShow;

        [SerializeField] private bool singleton = true;
        [SerializeField] private bool isMessage;
        [SerializeField] private bool switchInLangRu;

        public static YandexGame Instance;
        public ProgressData progressData;

        public PlayerAuth player { get; private set; }
        public LBData lbData{ get; private set; }
        public bool isAllowed { get; private set; }
        public bool isLoading { get; set; }
        public string Language { get => language; private set => language = value; }

        public bool Auth { get => auth; }
        public bool nowAdsShow
        {
            get
            {
                if (NowFullAd || NowVideoAd)
                    return false;
                else
                    return true;
            }
        }

        public bool NowFullAd { get => nowFullAd; set { nowFullAd = value; mediator.StateNowAdsShow = value; OnNowAdsShow?.Invoke(nowFullAd); Message($"{NowFullAd}"); } }
        public bool NowVideoAd { get => nowVideoAd; set { nowVideoAd = value; mediator.StateNowAdsShow = value; OnNowAdsShow?.Invoke(nowVideoAd); Message($"{NowVideoAd}"); } }

        private CloudStorage storage;
        private FullAd fullAd;
        private RewardeVideo rv;
        private Leaderboard lb;
        private Languages langs;

        private bool auth;
        private bool gameReadyUsed;
        private bool nowFullAd;
        private bool nowVideoAd;
        private string language = "en";
        private GlobalMediator mediator;

        private void Awake()
        {
            #region singleton
            if (singleton)
            {
                if (Instance != null)
                {
                    Destroy(gameObject);
                }
                else
                {
                    Instance = this;
                    DontDestroyOnLoad(gameObject);
                }
            }
            else
            {
                Instance = null;
                DontDestroyOnLoad(gameObject);
            }
            #endregion

            progressData = new();
            storage = new(this);
            fullAd = new(this);
            rv = new(this);
            lb = new(this);
            langs = new(this);
            Message($"Created:{gameObject.name},singleton:{singleton},isDebug:{isMessage}");
        }

        public void Initialize(GlobalMediator mediator)
        {
            this.mediator = mediator;
            Message("Initialize YG");
        }
        public IEnumerator LoadDatas()
        {
            Load();
            InitPlayer();
            InitLB();
            SetLanguage();
            //GetPayments();

            while (!isLoading)
                yield return new WaitForSeconds(0.1f);

            FullAdShow();
            GameReady();
        }
        #region FullAd
        public void FullAdShow() => fullAd.Show();
        public void OpenFullAd() => fullAd.Open();
        public void CloseFullAd(string wasShown) => fullAd.Close(wasShown);
        public void ErrorFullAd() => fullAd.Error();
        #endregion

        #region RewardeVideo
        public void RewardShow(int id) => rv.Show(id);
        public void OpenVideo() => rv.Open();
        public void CloseVideo() => rv.Close("true");
        public void RewardVideo(int id) => rv.RewardVideo(id);
        public void ErrorVideo() => rv.Error();
        public void RewardedVideo(int id) => OnRewardedVideo?.Invoke(id);
        #endregion

        #region LB
        public void InitLB() => lb.InitializeLB();
        public void InitializedLB() => Message("InitLB");

        public void NewLeaderboardScores(string nameLB, int score) => lb.SetScores(nameLB, score);
        public void GetLeaderboard(string nameLB, int maxQuantityPlayers, int quantityTop, int quantityAround, string photoSizeLB) =>
        lb.GetScores(nameLB, maxQuantityPlayers, quantityTop, quantityAround, photoSizeLB, Auth);
        public void LeaderboardEntries(string data) => lb.LeaderboardEntries(data);
        public void SetLB(LBData lbData)=> this.lbData = lbData;
        #endregion

        #region Storege

        public void InitStorege() => storage.Initialize();

        public void Save(object progressData) => storage.Save(progressData);
        public void Save() => storage.Save(progressData);
        public void Load() => storage.Load();

        public void OnLoadData(string data) => storage.SetLoadedData(data);

        #endregion

        # region Language
        public void SetLanguage()
        {
#if !UNITY_EDITOR
            language = langs.GetLangs();
              if (Language == "ru") langs.CreateLangs();
#else
            Language = switchInLangRu == true ? "ru" : "en";
            if (Language == "ru") langs.CreateLangs();
#endif
        }
        #endregion

        #region PlayerAuth
        [DllImport("__Internal")] private static extern void RequestAuth(bool sendback);
        public void RequestPlayer()
        {
#if !UNITY_EDITOR
            RequestAuth(true);
#endif
        }

        [DllImport("__Internal")] private static extern string InitPlayer_js();
        public void InitPlayer()
        {
#if !UNITY_EDITOR
            string playerData = InitPlayer_js();
            player = JsonConvert.DeserializeObject<PlayerAuth>(playerData);

            TryAuth();
            Message(playerData);
#else
            player = new PlayerAuth() { playerAuth = "true", playerId = "1", };
#endif
        }

        public void SetInitializationSDK(string playerData)
        {
            player = JsonConvert.DeserializeObject<PlayerAuth>(playerData);

            TryAuth();
            Message(playerData);
        }

        private void TryAuth() => auth = player.playerAuth == "rejected" ? false : true;

#endregion

        [DllImport("__Internal")] private static extern void GameReadyAPI();
        public void GameReady()
        {
#if !UNITY_EDITOR
            if (!gameReadyUsed)
            {
                gameReadyUsed = true;
                GameReadyAPI();
            }
#endif
        }

        public void Message(string message)
        {
            if (isMessage) Debug.Log(message);
        }

        private void Update()
        {
            //if (Input.GetKeyDown(KeyCode.D))
            //{
            //    NewLeaderboardScores("Score", 0);
            //    progressData.Reset();
            //    Save();
            //}
        }
    }
}
