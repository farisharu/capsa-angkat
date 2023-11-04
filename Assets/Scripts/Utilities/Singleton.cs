using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                // Check if an instance of the derived class already exists in the scene
                instance = FindObjectOfType<T>();

                if (instance == null)
                {
                    // If not found, create a new GameObject and attach the derived class as a component
                    GameObject singletonObject = new GameObject(typeof(T).Name);
                    instance = singletonObject.AddComponent<T>();
                }
            }

            return instance;
        }
    }

    protected virtual void Awake()
    {
        // Ensure the instance is set and persists through scenes
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        instance = this as T;
        DontDestroyOnLoad(this.gameObject);
    }
}
