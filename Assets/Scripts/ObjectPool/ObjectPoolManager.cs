using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Manager/Object Pool")]
public class ObjectPoolManager : Singleton<ObjectPoolManager>
{
    public List<ObjectPoolItem> itemsToPool;

    private List<GameObject> _pooledObjects;

    // Start is called before the first frame update
    public override void Initialize()
    {
        _pooledObjects = new List<GameObject>();

        itemsToPool.ForEach((item) =>
        {
            for (int i = 0; i < item.amountToPool; i++)
            {
                GameObject objectToInstantiate = Instantiate(item.objectToPool);
                objectToInstantiate.SetActive(false);
                _pooledObjects.Add(objectToInstantiate);
            }
        });
    }

    public GameObject GetNextPooledObjectByTag(string tag)
    {
        GameObject item = _pooledObjects.Find(i => i.CompareTag(tag) && !i.activeInHierarchy);

        if (item == null)
        {
            return null;
        }

        return item;
    }
}
