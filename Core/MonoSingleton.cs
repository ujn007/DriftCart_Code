using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance = null;
    private static bool IsDestroyed = false;

    public static T Instance
    {
        get
        {
            if (IsDestroyed)
            {
                instance = null;
            }

            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<T>();
                if (instance == null)
                {
                    Debug.LogError($"{typeof(T).Name} singleton is not exists!");
                }
                else
                {
                    IsDestroyed = false;
                }
            }
            return instance;
        }
    }

    private void OnDestroy()
    {
        IsDestroyed = true;
    }
}
