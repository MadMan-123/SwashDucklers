using UnityEngine;

/// <summary>
/// GM: this singleton allows us to have a single static instance of this class that can be easily accessed anywhere
/// GM: code structure should be kept simple :D
/// </summary>

public abstract class StaticInstance<T> : MonoBehaviour where T : MonoBehaviour 
{
  

        public static T instance { get; private set; }
        protected virtual void Awake() => instance = this as T;

        protected virtual void onApplicationQuit()
        {
            instance = null;
            Destroy(gameObject);
        }


    
}

/// <summary>
/// GM: this stransforms the static instance into a basic singleton, this will destroy any
/// new versions created, leaving the original isntance intact :D
/// </summary>
public abstract class Singleton<T> : StaticInstance<T> where T : MonoBehaviour
{
    protected override void Awake()
    {
        if (instance != null) Destroy(gameObject);
        base.Awake();
    }
}

/// <summary>
/// GM: this singleton is more like the standard type of singleton as there is onle a single because we ensure there is onle ony version of the actual object
/// GM: as well as there is a call for "dontdestroyonload(gameobject)"
/// </summary>
public abstract class SingletonPersistent<T> : Singleton<T> where T : MonoBehaviour //the the reason for the "persistent" is because this will persist throughout scenes
{
    protected override void Awake()
    {
       
            if (instance != null) Destroy(gameObject);
            DontDestroyOnLoad(gameObject); 
            base.Awake();
    }
}
