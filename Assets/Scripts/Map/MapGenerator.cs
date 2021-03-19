using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;

public class MapGenerator : MonoBehaviour
{
    public enum DrawMode { NoiseMap, ColorMap };
    public DrawMode drawMode;
    public Noise.NormalizeMode normalizeMode;

    // Propriete de la map pouvant etre editer depuis l'editeur Unity
    public int seed;
    // Grandeur d'un chunk. Must be a multiple of 10
    public const int mapChunkSize = 40;
    [Min(1)]
    public int mapWidth;
    [Min(1)]
    public int mapHeight;
    public float noiseScale;
    [Range(0, 33)]
    public int octaves;
    [Range(0, 1)]
    public float persistance;
    [Min(1)]
    public float lacunarity;
    public Vector2 offset;
    public TerrainType[] regions;
    public bool autoUpdate;

    Queue<MapThreadInfo<MapData,GameObject, DrawMode>> mapDataThreadInfos = new Queue<MapThreadInfo<MapData,GameObject, DrawMode>>();

    public void RequestMapData(GameObject chunk,Vector2 center, Action<MapData,GameObject, DrawMode> callback)
    {
        ThreadStart threadStart = delegate
        {
            MapDataThread(chunk,center, callback);
        };

        new Thread(threadStart).Start();
    }

    private void MapDataThread(GameObject chunk,Vector2 center, Action<MapData,GameObject, DrawMode> callback)
    {
        MapData mapData = GenerateMapData(center);

        // Zone critique
        // Evite l'appelle simultane entre 2 thread
        lock (mapDataThreadInfos)
        {
            mapDataThreadInfos.Enqueue(new MapThreadInfo<MapData,GameObject, DrawMode>(callback, chunk, mapData, drawMode));
        }
    }

    private void Update()
    {
        if (mapDataThreadInfos.Count > 0)
        {
            for (int i = 0; i < mapDataThreadInfos.Count; i++)
            {
                var threadInfo = mapDataThreadInfos.Dequeue();
                threadInfo.callback(threadInfo.mapData,threadInfo.chunk, drawMode);
            }
        }
    }

    private MapData GenerateMapData(Vector2 center)
    {
        float[,] noiseMap = Noise.GenerateNoiseMap(mapWidth, mapHeight, seed, noiseScale, octaves, persistance, lacunarity, center + offset, normalizeMode);
        Color[] colorMap = new Color[mapWidth * mapHeight];

        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                float currentHeight = noiseMap[x, y];

                colorMap[y * mapWidth + x] = regions[regions.Length - 1].color;
                for (int i = regions.Length-1; i >= 0; i--)
                {
                    if (currentHeight >= regions[i].height)
                    {
                        colorMap[y * mapWidth + x] = regions[i].color;
                        break;
                    }
                }
            }
        }

        return new MapData(noiseMap, colorMap);
    }

    public void DrawMapInEditor()
    {
        MapData mapData = GenerateMapData(Vector2.zero);
        MapDisplay display = FindObjectOfType<MapDisplay>();

        if (drawMode == DrawMode.NoiseMap)
        {
            display.DrawTexture(TextureGenerator.TextureFromNoiseMap(mapData.heightMap));
        }
        else if (drawMode == DrawMode.ColorMap)
        {
            display.DrawTexture(TextureGenerator.TextureFromColorMap(mapData.colorMap, mapWidth, mapHeight));
        }
    }

    struct MapThreadInfo<T, X, DrawMode>
    {
        public readonly Action<T, X, DrawMode> callback;
        public readonly T mapData;
        public readonly X chunk;
        public readonly DrawMode param2;

        public MapThreadInfo(Action<T, X, DrawMode> callback,X chunk, T mapData, DrawMode drawMode)
        {
            this.callback = callback;
            this.mapData = mapData;
            this.chunk = chunk;
            this.param2 = drawMode;
        }
    }

}

[Serializable]
public struct TerrainType
{
    public string name;
    [Range(0, 1)]
    public float height;
    public Color color;
}

public struct MapData
{
    public readonly float[,] heightMap;
    public readonly Color[] colorMap;

    public MapData(float[,] heightMap, Color[] colorMap)
    {
        this.heightMap = heightMap;
        this.colorMap = colorMap;
    }
}