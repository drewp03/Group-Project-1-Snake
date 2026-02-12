using UnityEngine;

public class QuackKey : MonoBehaviour
{
    public AudioSource audioSource;
   


    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
            audioSource.Play();
    }
}
