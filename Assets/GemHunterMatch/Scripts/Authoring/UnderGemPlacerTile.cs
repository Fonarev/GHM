using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.GemHunterMatch.Scripts.Authoring
{
    [CreateAssetMenu(fileName = "UnderGemPlacer", menuName = "2D Match/Tile/UnderGem")]
    public class UnderGemPlacerTile : TileBase
    {
        public Sprite PreviewEditorSprite;
        public UnderGem prefab;

        public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
        {
            //tileData.sprite = !Application.isPlaying ? PreviewEditorSprite : null;
        }

        public override bool StartUp(Vector3Int position, ITilemap tilemap, GameObject go)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
                return false;
#endif

            var newUnderGem = Instantiate(prefab);
            newUnderGem.Init(position);

            return true;
        }
    }
}