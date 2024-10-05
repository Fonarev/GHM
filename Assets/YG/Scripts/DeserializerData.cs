using Newtonsoft.Json;

using System;

using UnityEngine;

namespace Assets.YG.Scripts
{
    public class DeserializerData
    {
        private ProgressData Deserialize(string data)
        {
            try
            {
                ProgressData cloudData = JsonConvert.DeserializeObject<ProgressData>(data, new MissionConverter())!;
                return cloudData;
            }
            catch (Exception e)
            {
                Debug.LogError("Cloud Load Error: " + e.Message);
                return null;
            }
        }

    }
    public class MissionConverter : JsonConverter
    {
        public override bool CanWrite => false;
        public override bool CanConvert(Type objectType)
        {
            //return objectType == typeof(Mission); 
            return true;
        }
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) { }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            //JObject jo = JObject.Load(reader);
            //string typeName = jo["typeGoal"].ToString();

            //Mission mission;

            //switch (typeName)
            //{
            //    case "b":
            //        mission = new BarrierJumpMission();
            //        break;
            //    case "m":
            //        mission = new MultiplierMission();
            //        break;
            //    case "s":
            //        mission = new SingleRunMission();
            //        break;
            //    case "sl":
            //        mission = new SlidingMission();
            //        break;
            //    case "p":
            //        mission = new PickupMission();
            //        break;

            //    default:
            //        throw new Exception(string.Format("Unexpected typeGoal name '{0}'", typeName));
            //}

            //serializer.Populate(jo.CreateReader(), mission);

            //return mission;
            return null;
        }
    }
}