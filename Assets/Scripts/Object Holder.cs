using DefaultNamespace;
using NUnit.Framework.Internal;
using UnityEngine;

public class ObjectHolder : MonoBehaviour
{
    public GameObject holding;
    public Transform[] children;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for(int i = 0; i < children.Length; i++)
        {
            children[i].gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void TurnOn(string name)
    {
        holding = transform.Find(name).gameObject;
        holding.SetActive(true);
    }
    void SwitchObj(string name1, string name2)
    {
        holding = transform.Find(name1).gameObject;
        holding.SetActive(false);
        holding = transform.Find(name2).gameObject;
        holding.SetActive(true);
    }
}
