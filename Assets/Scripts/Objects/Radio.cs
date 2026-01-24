using System.Text;
using UnityEngine;

public class Radio : MonoBehaviour
{
    public PlayerStats PS;
    public AudioSource AS;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AS = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Player") && PS.IsParanoid)
        {
            PS.IsParanoid = false;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player") && !PS.IsParanoid)
        {
            PS.IsParanoid = true;
        }
    }
}