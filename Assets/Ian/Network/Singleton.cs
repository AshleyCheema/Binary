using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour 
{
    private static T mInstance;
    public static T Instance
    {
        get
        {
            if(mInstance == null)
            {
                mInstance = FindObjectOfType<T>();
            }

            if(mInstance == null)
            {
                var singletonObj = new GameObject();
                mInstance = singletonObj.AddComponent<T>();
                singletonObj.name = typeof(T).ToString() + " (Singleton)";

                DontDestroyOnLoad(singletonObj);
            }
            return mInstance;
        }
    }
}
