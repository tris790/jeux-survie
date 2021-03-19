using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteTerrain : MonoBehaviour
{
    //Distance à partir de laquel un chunk est afficher
    public float maxViewDst = 40;
    public Transform viewer;
    public Material mapMaterial;

    public static Vector2 viewerPosition;
    static MapGenerator mapGenerator;
    static MapDisplay mapDisplay;
    int chunkSize;
    int chunksVisibleInViewDst;

    private Dictionary<Vector2, GameObject> terrainChunkDict = new Dictionary<Vector2, GameObject>();
    private List<GameObject> terrainChunksVisibleLastUpdate = new List<GameObject>();


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
        //terrainChunksVisibleLastUpdate.Clear();
        ChunkUtility chunkUtil = new ChunkUtility();
        int currentChunkCoordX = Mathf.RoundToInt(viewerPosition.x / chunkSize);
        int currentChunkCoordY = Mathf.RoundToInt(viewerPosition.y / chunkSize);

        //évite qu'un chunk reste afficher si le joueur est rendu trop loin pour le voir
        List<Vector2> removeKey = new List<Vector2>();
        foreach (var lastChunk in terrainChunkDict)
        {
            Vector2 pos = new Vector2(currentChunkCoordX, currentChunkCoordY);
            if (lastChunk.Key.x < pos.x - chunksVisibleInViewDst || lastChunk.Key.y < pos.y - chunksVisibleInViewDst ||
                lastChunk.Key.x > pos.x + chunksVisibleInViewDst || lastChunk.Key.y > pos.y + chunksVisibleInViewDst)
            {
                lastChunk.Value.SetActive(false);
                removeKey.Add(lastChunk.Key);
            }
        }
        foreach (var i in removeKey)
        {
            terrainChunkDict.Remove(i);
        }
        removeKey.Clear();

        for (int yOffset = -chunksVisibleInViewDst; yOffset <= chunksVisibleInViewDst; yOffset++)
        {
            for (int xOffset = -chunksVisibleInViewDst; xOffset <= chunksVisibleInViewDst; xOffset++)
            {
                Vector2 viewedChunkCoord = new Vector2(currentChunkCoordX + xOffset, currentChunkCoordY + yOffset);
                GameObject chunk = ObjectPoolManager.Instance.GetNextPooledObjectByTag("Chunk");
                if (chunk != null && !terrainChunkDict.ContainsKey(viewedChunkCoord))
                {
                    terrainChunkDict.Add(viewedChunkCoord, chunk);
                    chunkUtil.UpdateTerrainChunk(chunk, viewedChunkCoord, chunkSize, mapMaterial);
                }
            }
        }
    }

    public class ChunkUtility
    {
        void OnMapDataReceived(MapData mapData, GameObject chunk, MapGenerator.DrawMode drawMode)
        {
            Texture2D texture;
            if (drawMode == MapGenerator.DrawMode.NoiseMap)
                texture = TextureGenerator.TextureFromNoiseMap(mapData.heightMap);
            else
                texture = TextureGenerator.TextureFromColorMap(mapData.colorMap, MapGenerator.mapChunkSize / 10, MapGenerator.mapChunkSize / 10);
            Renderer meshRenderer = chunk.GetComponent<Renderer>();
            meshRenderer.material.mainTexture = texture;
        }

        public void UpdateTerrainChunk(GameObject chunk, Vector2 coord, int size, /*Transform parent,*/ Material material)
        {
            Vector2 pos = coord * size;

            Vector3 posv3 = new Vector3(pos.x, pos.y, 0);

            Renderer meshRenderer = chunk.GetComponent<Renderer>();
            MeshFilter meshFilter = chunk.GetComponent<MeshFilter>();
            meshRenderer.material = material;
            chunk.transform.localScale = Vector3.one * size / 10.0f;
            chunk.transform.position = posv3;
            //chunk.transform.Rotate(-90.0f, 0.0f, 0.0f, Space.Self);
            mapGenerator.RequestMapData(chunk, pos, OnMapDataReceived);
            chunk.SetActive(true);

        }
    }
}


