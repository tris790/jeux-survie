using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;

public class MapGenerator : MonoBehaviour
{
    public enum DrawMode { NoiseMap,ColorMap };
    public DrawMode drawMode;
    public Noise.NormalizeMode normalizeMode;


    //Propriété de la map pouvant etre éditer depuis l'editeur Unity
    public int seed;
    //must be a multiple de 10
    public const int mapChunkSize=40;
    [Min(1)]
    public int mapWidth;
    [Min(1)]
    public int mapHeight;
    public float noiseScale;
    [Range(0,33)]
    public int octaves;
    [Range(0,1)]
    public float persistance;
    [Min(1)]
    public float lacunarity;
    public Vector2 offset;
    public TerrainType[] regions;
    public bool autoUpdate;

    Queue<MapThreadInfo<MapData,DrawMode>> mapDataThreadInfos = new Queue<MapThreadInfo<MapData,DrawMode>>();

    public void RequestMapData(Vector2 center,Action<MapData,DrawMode> callback)
    {
        ThreadStart threadStart = delegate
        {
            MapDataThread(center, callback);
        };

        new Thread(threadStart).Start();
    }

    private void MapDataThread(Vector2 center, Action<MapData,DrawMode> callback)
    {
        MapData mapData = GenerateMapData(center);
        //zone critique
        //évite l'appelle simultané entre 2 threead
        lock (mapDataThreadInfos) 
        {
            mapDataThreadInfos.Enqueue(new MapThreadInfo<MapData,DrawMode>(callback, mapData,drawMode));
        }
    }

    private void Update()
    {
        if (mapDataThreadInfos.Count>0)
        {
            for (int i = 0; i < mapDataThreadInfos.Count; i++)
            {
                var threadInfo=mapDataThreadInfos.Dequeue();
                threadInfo.callback(threadInfo.param,drawMode);
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
                for (int i = 0; i < regions.Length; i++)
                {
                    if (currentHeight <= regions[i].height)
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

    struct MapThreadInfo<T,DrawMode>
    {
        public readonly Action<T,DrawMode> callback;
        public readonly T param;
        public readonly DrawMode param2;
        public MapThreadInfo(Action<T,DrawMode> callback,T param,DrawMode param2)
        {
            this.callback = callback;
            this.param = param;
            this.param2 = param2;
        }
    }

}

[System.Serializable]
public struct TerrainType
{
    public string name;
    [Range(0,1)]
    public float height;
    public Color color;
}

public struct MapData
{
    public readonly float[,] heightMap;
    public readonly Color[] colorMap;
    public MapData(float[,] heightMap,Color[] colorMap)
    {
        this.heightMap = heightMap;
        this.colorMap = colorMap;
    }
}