using System.Collections.Generic;
using System.Linq;
using ComradeVanti.CSharpTools;
using Dev.ComradeVanti;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace TeamShrimp.GGJ23
{
    public class MapKeeper : MonoBehaviour
    {
        private const string TileTypeResourcePath = "TileTypes";
        private const string StructureTypeResourcePath = "StructureTypes";

        [SerializeField] private Tilemap groundTilemap;

        private readonly Dictionary<Vector2Int, ShroomBase> shroomsByPosition =
            new Dictionary<Vector2Int, ShroomBase>();

        private IReadOnlyDictionary<string, StructureType> structureTypesByName;
        private IReadOnlyDictionary<string, TileType> tileTypesByName;

        private void Awake()
        {
            LoadTileTypes();
            LoadStructureTypes();
        }

        private void Start()
        {
            InstantiateMapWith(10);
        }

        public IOpt<ShroomBase> TryFindShroom(Vector2Int pos) =>
            shroomsByPosition.TryGet(pos);

        public void AddShroom(ShroomBase shroom)
        {
            var pos = shroom.ShroomPosition;
            shroomsByPosition.Add(pos, shroom);
        }

        public bool CanPlace(ShroomType type, Vector2Int pos) =>
            TryFindShroom(pos).IsSome();

        private void InstantiateMapWith(int size)
        {
            var defaultTile = tileTypesByName.Values.First();
            var homeStructure = structureTypesByName["Home"];
            var genParams =
                new MapGen.GenerationParams(size, defaultTile, homeStructure);
            var map = MapGen.GenerateMap(genParams);
            InstantiateMap(map);
        }

        private void InstantiateMap(MapGen.Map map)
        {
            foreach (var (pos, tile) in map.TilesByPosition)
            {
                var variant = tile.Type.Variants.ElementAt(tile.VariantIndex);
                groundTilemap.SetTile(pos.To3Int(), variant);
            }

            foreach (var (pos, structure) in map.StructuresByPosition)
            {
                var go = Instantiate(structure.Type.Prefab, pos.To3(),
                    Quaternion.identity);
                go.TryGetComponent<ShroomBase>()
                    .Iter(AddShroom);
            }
        }

        private void LoadTileTypes()
        {
            tileTypesByName = Resources.LoadAll<TileType>(TileTypeResourcePath)
                .ToDictionary(t => t.name, t => t);
            if (tileTypesByName.Count == 0)
                Debug.LogWarning("No tile-types found in resources!");
        }

        private void LoadStructureTypes()
        {
            structureTypesByName = Resources
                .LoadAll<StructureType>(StructureTypeResourcePath)
                .ToDictionary(t => t.name, t => t);
            if (structureTypesByName.Count == 0)
                Debug.LogWarning("No structure-types found in resources!");
        }
    }
}