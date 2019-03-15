using UnityEngine;

/**
 * A singleton class in which the singleton
 * only exists for the specific scene it has
 * an instance in. This singleton is able to
 * destroyed and recreated on scene load
 */
public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    private static T m_Instance = null;
    public static T Instance
    {
        get
        {
            if (m_Instance == null)
            {
                m_Instance = FindObjectOfType<T>();
                // fallback, might not be necessary.
                if (m_Instance == null)
                    m_Instance = new GameObject(typeof(T).Name).AddComponent<T>();
            }
            return m_Instance;
        }
    }
}