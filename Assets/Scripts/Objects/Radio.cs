using System.Text;
using UnityEngine;

public class Radio : MonoBehaviour
{
    private PlayerStats PS;
    public AudioSource AS;

    public AudioClip song;
    public AudioClip stat;    

    private bool played = false;

    private TaskManager TM;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AS = GetComponent<AudioSource>();
        PS = GameObject.FindWithTag("Player").GetComponent<PlayerStats>();
        TM = GameObject.FindWithTag("Player").GetComponent<TaskManager>();

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
        for (int i = 0; i < TM.activeTasks.Length; i++)
        {
            if (TM.activeTasks[i].name != "Broken Radio")
            {
                AS.clip = song;
            } else
            {
                AS.clip = stat;
            }
        }
        if (!played)
        {
            AS.Play();
        }
        played = true;
    }
    void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player") && !PS.IsParanoid)
        {
            played = false;
            PS.IsParanoid = true;
        }
    }
}