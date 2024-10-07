using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Audio;

namespace Match3
{
    /// <summary>
    /// Contains all the settings for the game so they can be found in a single place. This is stored on the GameManager
    /// in the Resource folder if you need to edit it.
    /// </summary>
    [System.Serializable]
    public class GameSettings
    {
        public float InactivityBeforeHint = 2.0f;
    
        public VisualSetting VisualSettings;
        public BonusSetting BonusSettings;
        public ShopSetting ShopSettings;
        public SoundSetting SoundSettings;
    }

    /// <summary>
    /// Visual Settings are all the parameters used in the Visual effect of the game like fall speed, bounce curve and vfx
    /// </summary>
    [System.Serializable]
    public class VisualSetting
    {
        [Header("OnFall")]
        [Range(5.0f, 15.0f)] public float FallSpeed = 10.0f;
        public AnimationCurve FallAccelerationCurve;
        public AnimationCurve BounceCurve;
        public AnimationCurve SquishCurve;

        [Header("OnMath")]
        public AnimationCurve MatchFlyCurve;
        public AssetReference MatchEffect;
        //public GameObject BonusModePrefab;

        [Header("Hint")]
        [Range(5.0f, 10.0f)] public float inactivityTimeBeforeHint = 8.0f;
        public AssetReference HintReference;
       
        [Header("InputEffect")]
        public AssetReference GemHold;
        public AssetReference HoldTrail;

        [Header("CoinEffect")]
        public AssetReference CoinVFX;
        public AnimationCurve CoinFlyCurve;

        [Header("EndEffect")]
        public AssetReference WinEffect;
        public AssetReference LoseEffect;
        public AssetReference Bubbles;
    }

    /// <summary>
    /// Setting related to bonus BonusGem, list all the existing bonus gems. 
    /// </summary>
    [System.Serializable]
    public class BonusSetting
    {
        public BonusGem[] Bonuses;
    }

    /// <summary>
    /// Settings related to the Shop, list all ShopItems.
    /// </summary>
    [System.Serializable]
    public class ShopSetting
    {
        public abstract class ShopItem : ScriptableObject
        { 
            public Sprite ItemSprite;
            public string ItemName;
            public int Price;

            //public virtual bool CanBeBought()
            //{
            //    return GameManager.Instance.Coins >= Price; 
            //}
        
            public abstract void Buy();
        }

        public ShopItem[] Items;
    }

    [System.Serializable]
    public class SoundSetting
    {
        public AudioMixer Mixer;
        
        public AudioSource SFXSourcePrefab;
        public AudioSource MusicSourcePrefab;

        public AudioClip MenuSound;
        
        public AudioClip SwipSound;
        public AudioClip FallSound;
        public AudioClip CoinSound;

        public AudioClip WinVoice;
        public AudioClip LooseVoice;

        public AudioClip WinSound;
        public AudioClip LooseSound;
    }
}