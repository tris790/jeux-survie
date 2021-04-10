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
                Sprite po = Sprite.Create(heart, new Rect(0, 0, heart.width, heart.height), new Vector2(heart.width/2,heart.height/2),100);

                //GameObject.CreatePrimitive(PrimitiveType.);
                //srHeart.sprite = po;
                //srHeart.transform.position = new Vector3(position.x+point.x- regionSize.x/2, position.y+point.y- regionSize.y/2, 0);
                //po.transform.eulerAngles=new Vector3(-90,0,0);
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
