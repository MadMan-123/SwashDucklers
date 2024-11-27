using UnityEngine;

/// <summary>
/// GM: This base class (`StaticInstance`) creates a static instance of the class, 
/// GM: allowing easy access to a single instance from anywhere in the project.
/// GM: It's an abstract class, so it cannot be used directly but can be extended.
/// GM: Code structure is kept simple for easy understanding and usage.
/// </summary>
public abstract class StaticInstance<T> : MonoBehaviour where T : MonoBehaviour
{
    // GM: Static property to store the single instance of the class.
    public static T instance { get; private set; }

    // GM: This method is called when the script is loaded.
    // GM: It assigns the current object (`this`) as the static instance.
    protected virtual void Awake() => instance = this as T;

    // GM: This method is triggered when the application is quitting.
    // GM: It clears the static instance reference and destroys the game object to prevent errors.
    protected virtual void onApplicationQuit()
    {
        instance = null; // GM: Remove the reference to the instance when quitting.
        Destroy(gameObject); // GM: Destroy the object to clean up memory.
    }
}

/// <summary>
/// GM: This `Singleton` class builds upon `StaticInstance` to enforce the singleton pattern.
/// GM: It ensures that only one instance of the class exists by destroying duplicates.
/// GM: This class is abstract and must be extended by other classes.
/// </summary>
public abstract class Singleton<T> : StaticInstance<T> where T : MonoBehaviour
{
    // GM: Override the `Awake` method to check for duplicate instances.
    protected override void Awake()
    {
        // GM: If an instance already exists, destroy this game object.
        if (instance != null) Destroy(gameObject);

        // GM: Call the base class's `Awake` to assign the instance.
        base.Awake();
    }
}

/// <summary>
/// GM: This `SingletonPersistent` class extends `Singleton` to create a persistent singleton.
/// GM: It ensures that the object persists across scene loads using `DontDestroyOnLoad`.
/// GM: This is ideal for managers or objects that should exist throughout the entire game lifecycle.
/// </summary>
public abstract class SingletonPersistent<T> : Singleton<T> where T : MonoBehaviour
{
    // GM: Override the `Awake` method to make the object persistent across scenes.
    protected override void Awake()
    {
        // GM: Destroy duplicates to enforce a single instance.
        if (instance != null) Destroy(gameObject);

        // GM: Prevent this object from being destroyed when loading a new scene.
        DontDestroyOnLoad(gameObject);

        // GM: Call the base class's `Awake` to assign the instance.
        base.Awake();
    }
}

