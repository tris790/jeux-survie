using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomItemOnMap : MonoBehaviour
{
    [Min(1)]
    public float radius = 1;
    public Vector2 regionSize = new Vector2(MapGenerator.mapChunkSize, MapGenerator.mapChunkSize);
    public Transform pos;
    public int rejectionSamples = 30;
    public float displayRadius = 1;
    public List<GameObject> items;

    List<Vector2> points;

    public void generateItem(Vector2 pos)
    {
        points = generateDot(pos);
        if (points != null)
        {
            foreach (Vector2 point in points)
            {
                var index = Random.Range(0, items.Count);
                var item = items[index];
                Vector2 instPoint = pos + point - regionSize / 2;
                Instantiate(item, new Vector3(instPoint.x, instPoint.y), new Quaternion());
            }
        }
    }

    private List<Vector2> generateDot(Vector2 pos)
    {
        int x = Mathf.RoundToInt(pos.x / MapGenerator.mapChunkSize);
        int y = Mathf.RoundToInt(pos.y / MapGenerator.mapChunkSize);
        return PoissonDiscSampling.GeneratePoints(radius, regionSize, (x + 1) + MapGenerator.mgseed * (y + 1), rejectionSamples);
    }

    void OnValidate()
    {
        points = generateDot(pos.position);
    }

    void OnDrawGizmos()
    {
        Vector2 position = new Vector2(pos.transform.position.x, pos.transform.position.y);

        Gizmos.DrawWireCube(new Vector2(pos.transform.position.x, pos.transform.position.y), regionSize);
        if (points != null)
        {
            foreach (Vector2 point in points)
            {
                Gizmos.DrawSphere(position + point - regionSize / 2, displayRadius);
            }
        }
    }
}
