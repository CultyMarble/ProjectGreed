using UnityEngine;

public abstract class SingletonMonobehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
    //Private
    private static T instance;

    //Protected
    protected abstract void Awake();

    protected void Singleton()
    {

        if (instance == null)
            instance = this as T;
        else
            Destroy(this.gameObject);
    }

    //Public
    public static T Instance
    {
        get { return instance; }
    }
}
