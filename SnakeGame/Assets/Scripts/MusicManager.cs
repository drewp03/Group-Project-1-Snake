using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private static MusicManager song;

    private void Awake()
    {
        if(song == null)
        {
            song = this;
            DontDestroyOnLoad(gameObject);
            GetComponent<AudioSource>().Play();
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
