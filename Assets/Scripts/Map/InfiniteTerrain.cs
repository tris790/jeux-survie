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
    int chunksVisibleInViewDst;

    private ChunkUtility chunkUtil = new ChunkUtility();
    private Dictionary<Vector2, GameObject> terrainChunkDict = new Dictionary<Vector2, GameObject>();
    private List<Vector2> removeKey = new List<Vector2>();


    private void Start()
    {
        mapGenerator = FindObjectOfType<MapGenerator>();
        mapDisplay = FindObjectOfType<MapDisplay>();
        chunksVisibleInViewDst = Mathf.RoundToInt(maxViewDst / MapGenerator.mapChunkSize);
        viewerPosition = new Vector2(viewer.position.x, viewer.position.y);
    }

    private void Update()
    {
            viewerPosition.Set(viewer.position.x, viewer.position.y);
            UpdateVisibleChunks();
    }

    void UpdateVisibleChunks()
    {
        int currentChunkCoordX = Mathf.RoundToInt(viewerPosition.x / MapGenerator.mapChunkSize);
        int currentChunkCoordY = Mathf.RoundToInt(viewerPosition.y / MapGenerator.mapChunkSize);

        //évite qu'un chunk reste afficher si le joueur est rendu trop loin pour le voir
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
                    chunkUtil.UpdateTerrainChunk(chunk, viewedChunkCoord, MapGenerator.mapChunkSize);
                }
            }
        }

    }

    public class ChunkUtility
    {
        Vector3 posv3 = new Vector3();
        void OnMapDataReceived(MapData mapData, GameObject chunk, MapGenerator.DrawMode drawMode)
        {
            Renderer meshRenderer = chunk.GetComponent<Renderer>();
            Texture2D texture;
            if (drawMode == MapGenerator.DrawMode.NoiseMap)
                texture = TextureGenerator.TextureFromNoiseMap(mapData.heightMap, meshRenderer.material.mainTexture);
            else
                texture = TextureGenerator.TextureFromColorMap(mapData.colorMap, MapGenerator.mapChunkSize / 10, MapGenerator.mapChunkSize / 10, meshRenderer.material.mainTexture);
            meshRenderer.material.mainTexture = texture;
        }

        public void UpdateTerrainChunk(GameObject chunk, Vector2 coord, int size)
        {
            Vector2 pos = coord * size;
            posv3.Set(pos.x, pos.y, 0);
            chunk.transform.localScale = Vector3.one * size / 10.0f;
            chunk.transform.position = posv3;
            mapGenerator.RequestMapData(chunk, pos, OnMapDataReceived);
            chunk.SetActive(true);
        }
    }
}


