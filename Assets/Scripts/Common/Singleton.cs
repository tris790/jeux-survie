using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Component
{
    private static T _instance;

    public static bool m_isQuitting;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                // Making sure that there's not other instances of the same type in memory.
                _instance = FindObjectOfType<T>();

                if (_instance == null)
                {
                    // Initializing a new game object to contain a singular instance.
                    GameObject obj = new GameObject();
                    obj.name = typeof(T).Name;
                    _instance = obj.AddComponent<T>();
                }
            }

            return _instance;
        }
    }

    public virtual void Awake()
    {
        if (_instance == null)
        {
            // If null, this instance is now the Singleton instance of the assigned type.
            _instance = this as T;

            // Making sure that Singleton instance will persist in memory across every scene.
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            // Destroy current instance because it must be a duplicate.
            Destroy(gameObject);
        }
    }
}
