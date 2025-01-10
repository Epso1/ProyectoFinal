using UnityEngine;
using UnityEngine.Tilemaps;

public class ProceduralTilemap : MonoBehaviour
{
    [SerializeField] private Tilemap tilemap; // Referencia al Tilemap
    [SerializeField] private Sprite[] sprites; // Array de sprites del spritesheet
    [SerializeField] private float[] spritesProbabilities = { 0.8f, 0.01f, 0.09f, 0.1f }; // Probabilidad de generación de cada sprite
    [SerializeField] private Vector2Int gridSize = new Vector2Int(32, 16); // Tamaño del mapa (ancho x alto)


    void Start()
    {
        // Crear tiles desde los sprites
        TileBase[] tiles = CreateTilesFromSprites(sprites);

        // Dibujar los tiles en el Tilemap
        PopulateTilemap(tilemap, tiles, gridSize);
    }

    // Crea una lista de tiles a partir de los sprites
    private TileBase[] CreateTilesFromSprites(Sprite[] sprites)
    {
        TileBase[] tiles = new TileBase[sprites.Length];

        for (int i = 0; i < sprites.Length; i++)
        {
            // Crear un Tile para cada sprite
            Tile tile = ScriptableObject.CreateInstance<Tile>();
            tile.sprite = sprites[i];
            tiles[i] = tile;
        }

        return tiles;
    }

    // Llena el Tilemap con los tiles
    private void PopulateTilemap(Tilemap tilemap, TileBase[] tiles, Vector2Int gridSize)
    {
        // Limpiar el Tilemap existente
        tilemap.ClearAllTiles();

        // Calcular el desplazamiento para centrar el Tilemap
        int offsetX = -gridSize.x / 2;
        int offsetY = -gridSize.y / 2;

        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                // Seleccionar un tile basado en probabilidades
                TileBase selectedTile = SelectTileBasedOnProbability(tiles, spritesProbabilities);

                // Calcular la posición centrada
                Vector3Int position = new Vector3Int(x + offsetX, y + offsetY, 0);

                // Colocar el tile en la posición actual
                tilemap.SetTile(position, selectedTile);
            }
        }
    }

    // Método para seleccionar un Tile basado en probabilidades
    private TileBase SelectTileBasedOnProbability(TileBase[] tiles, float[] probabilities)
    {
        // Calcular la suma total de probabilidades
        float totalProbability = 0;
        foreach (float prob in probabilities)
            totalProbability += prob;

        // Elegir un número aleatorio entre 0 y el total de probabilidades
        float randomValue = Random.Range(0, totalProbability);

        // Determinar qué Tile corresponde al valor aleatorio
        float cumulativeProbability = 0;
        for (int i = 0; i < tiles.Length; i++)
        {
            cumulativeProbability += probabilities[i];
            if (randomValue <= cumulativeProbability)
                return tiles[i];
        }

        // Fallback: devolver el primer Tile si ocurre algún error
        return tiles[0];
    }

}
