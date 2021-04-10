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
    public List<Item> items;
    Texture2D heart;
    SpriteRenderer srHeart;

    List<Vector2> points;

    void Awake()
    {
        heart = new Texture2D(10,10);
        srHeart = gameObject.AddComponent<SpriteRenderer>() as SpriteRenderer;
    }

    public void generateItem()
    {
        points = generateDot();
        if (points != null)
        {
            foreach (Vector2 point in points)
            {

                //code popant les item sur la carte depuis une liste prochain pr
                
            }
        }
    }

    private List<Vector2> generateDot()
    {
        Vector2 position = new Vector2(pos.transform.position.x, pos.transform.position.y);
        int x = Mathf.RoundToInt(position.x / MapGenerator.mapChunkSize);
        int y = Mathf.RoundToInt(position.y / MapGenerator.mapChunkSize);
        return PoissonDiscSampling.GeneratePoints(radius, regionSize, position, (x + 1) + MapGenerator.mgseed * (y + 1), rejectionSamples);
    }

    void OnValidate()
    {
        points = generateDot();
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
