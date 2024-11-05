using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour { 

    public static T instance {  get; private set; }
    protected virtual void Awake() => instance = this as T;

    protected virtual void onApplicationQuit()
    {
        instance = null;
        Destroy(gameObject);
    }

    
}

public abstract class SingletonPersistent<T> : Singleton<T> where T : MonoBehaviour 
{
    protected override void Awake()
    {
        base.Awake() {
            if (instance != null) Destroy(gameObject);
            DontDestroyOnLoad(gameObject); 
            base.Awake();
        }
    }
}
