using Newtonsoft.Json;

using System;
using System.Runtime.InteropServices;

using UnityEngine;

namespace Assets.YG.Scripts
{
    public class CloudStorage : YandexBase
    {
        protected override string sourceMessage => "CloudStorege";

        public CloudStorage(YandexGame yandex, bool isMessage = true) : base(yandex, isMessage){ }
      
        [DllImport("__Internal")] private static extern string InitCloudStorage();
        [DllImport("__Internal")] private static extern void SaveData(string data, bool flush);
        [DllImport("__Internal")] private static extern void LoadData(bool sendback);

        public void Initialize()
        {
            InitCloudStorage();
        }

        public void Save(object progressData, bool flush = false)
        {
#if !UNITY_EDITOR
            var json = JsonConvert.SerializeObject(progressData);
            SaveData(json, flush);
#endif
        }

        public void Load(bool sendback = true)
        {
#if !UNITY_EDITOR
            LoadData(sendback);
#endif
        }

        public void SetLoadedData(string data)
        {
            Message(data);

            if (data != "noData")
            {
                data = CleanseData(data);

                try
                {
                    yandex.progressData = JsonConvert.DeserializeObject<ProgressData>(data);
                    yandex.isLoading = true;
                }
                catch (Exception e)
                {
                    Debug.LogError("Cloud Load Error: " + e.Message);
                }
            }
            else
            {
                yandex.progressData.CreateDefaultData();
                Message($"Created Default Data: {yandex.progressData}");
                yandex.isLoading = true;
            }
        }

        private string CleanseData(string data)
        {
            data = data.Remove(0, 2);
            data = data.Remove(data.Length - 2, 2);
            data = data.Replace(@"\\\", '\u0002'.ToString());
            data = data.Replace(@"\", "");
            data = data.Replace('\u0002'.ToString(), @"\");

            return data;
        }

    }
}
