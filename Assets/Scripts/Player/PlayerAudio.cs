using System.Security.Cryptography;
using System.Text;
using JetBrains.Annotations;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    public AudioClip[] AC;
    public TimeController TC;
    private AudioSource AS;
    public int count = 1;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AS = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if((int)TC.CurrentTime / UnityEngine.Random.Range(15, 30) == count)
        {
            count++;
            AS.pitch = UnityEngine.Random.Range(0.5f,1.5f);
            AS.PlayOneShot(AC[UnityEngine.Random.Range(1, 4)]);
        }
    }

}
