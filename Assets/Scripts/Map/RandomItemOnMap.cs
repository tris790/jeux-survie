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
        Vector2 position = new Vector2(pos.transform.position.x, pos.transform.position.y);
        points = PoissonDiscSampling.GeneratePoints(radius, regionSize, position, rejectionSamples);
        if (points != null)
        {
            foreach (Vector2 point in points)
            {
                //code popant les item sur la carte depuis une liste prochain pr
                
            }
        }
    }

    void OnValidate()
    {
        Vector2 position = new Vector2(pos.transform.position.x, pos.transform.position.y);
        points = PoissonDiscSampling.GeneratePoints(radius, regionSize, position, rejectionSamples);
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
