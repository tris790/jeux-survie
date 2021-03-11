using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteTerrain : MonoBehaviour
{
    public const float maxViewDst = 250;
    public Transform viewer;
    public Material mapMaterial;

    public static Vector2 viewerPosition;
    static MapGenerator mapGenerator;
    static MapDisplay mapDisplay;
    int chunkSize;
    int chunksVisibleInViewDst;

    //static MapDisplay mapDisplay;
    private Dictionary<Vector2, TerrainChunk> terrainChunkDict = new Dictionary<Vector2, TerrainChunk>();
    private List<TerrainChunk> terrainChunksVisibleLastUpdate = new List<TerrainChunk>();


    private void Start()
    {
        chunkSize = MapGenerator.mapChunkSize;
        mapGenerator = FindObjectOfType<MapGenerator>();
        mapDisplay = FindObjectOfType<MapDisplay>();
        chunksVisibleInViewDst = Mathf.RoundToInt(maxViewDst / chunkSize);
    }

    private void Update()
    {
        viewerPosition = new Vector2(viewer.position.x, viewer.position.y);
        UpdateVisibleChunks();
    }

    void UpdateVisibleChunks()
    {
        foreach (TerrainChunk chunk in terrainChunksVisibleLastUpdate)
        {
            chunk.SetVisible(false);
        }
        terrainChunksVisibleLastUpdate.Clear();

        int currentChunkCoordX = Mathf.RoundToInt(viewerPosition.x / chunkSize);
        int currentChunkCoordY = Mathf.RoundToInt(viewerPosition.y / chunkSize);

        for(int yOffset = -chunksVisibleInViewDst; yOffset<=chunksVisibleInViewDst;yOffset++)
        {
            for (int xOffset = -chunksVisibleInViewDst; xOffset <= chunksVisibleInViewDst; xOffset++)
            {
                Vector2 viewedChunkCoord = new Vector2(currentChunkCoordX + xOffset, currentChunkCoordY + yOffset);

                if(terrainChunkDict.ContainsKey(viewedChunkCoord))
                {
                    terrainChunkDict[viewedChunkCoord].UpdateTerrainChunk();
                    if(terrainChunkDict[viewedChunkCoord].IsVisible())
                    {
                        terrainChunksVisibleLastUpdate.Add(terrainChunkDict[viewedChunkCoord]);
                    }
                }
                else
                {
                    terrainChunkDict.Add(viewedChunkCoord, new TerrainChunk(viewedChunkCoord, chunkSize, transform, mapMaterial));
                }
            }
        }
    }

    public class TerrainChunk
    {
        GameObject meshObject;
        Vector2 pos;
        Bounds bounds;

        Renderer meshRenderer;
        MeshFilter meshFilter;


        public TerrainChunk(Vector2 coord, int size, Transform parent, Material material)
        {

            pos = coord * size;
            bounds = new Bounds(pos, Vector2.one * size);

            Vector3 posv3 = new Vector3(pos.x, pos.y, 0);

            meshObject = GameObject.CreatePrimitive(PrimitiveType.Plane);
            meshRenderer = meshObject.GetComponent<Renderer>();
            meshFilter = meshObject.GetComponent<MeshFilter>();
            meshRenderer.material = material;
            meshObject.transform.localScale = Vector3.one*size/10.0f;
            meshObject.transform.position = posv3;
            meshObject.transform.Rotate(-90.0f, 0.0f, 0.0f, Space.Self);
            meshObject.transform.parent = parent;
            SetVisible(false);

            mapGenerator.RequestMapData(pos, OnMapDataReceived);
        }

        void OnMapDataReceived(MapData mapData, MapGenerator.DrawMode drawMode)
        {
            Texture2D texture;
            if (drawMode==MapGenerator.DrawMode.NoiseMap)
                texture = TextureGenerator.TextureFromNoiseMap(mapData.heightMap);
            else
                texture = TextureGenerator.TextureFromColorMap(mapData.colorMap, MapGenerator.mapChunkSize / 10, MapGenerator.mapChunkSize / 10);
            meshRenderer.material.mainTexture = texture;
        }

        public void UpdateTerrainChunk()
        {
            float viewerDstFromNearestEdge = Mathf.Sqrt(bounds.SqrDistance(viewerPosition));
            bool visible = viewerDstFromNearestEdge <= maxViewDst;
            SetVisible(visible);
        }

        public void SetVisible(bool visible)
        {
            meshObject.SetActive(visible);
        }

        public bool IsVisible()
        {
            return meshObject.activeSelf;
        }
    }
}


