using UnityEngine;
using DefaultNamespace;
using System.Collections;

public class AirLockManager : MonoBehaviour, IInteractable
{
    public string InteractMessage => objectInteractMessage;
    public string objectInteractMessage;

    public GameObject Outer;
    public GameObject Inner;
    private bool isAnimating;

    void Start()
    {
        Outer.GetComponent<Animator>().SetBool("open", !GameManager.isInside);
        Inner.GetComponent<Animator>().SetBool("open", GameManager.isInside);
    }
    public void Interact()
    {
        if (isAnimating) return;
        isAnimating = true;
        Outer.GetComponent<Animator>().SetBool("open", false);
        Inner.GetComponent<Animator>().SetBool("open", false);
        StartCoroutine(WaitForDoorAnimation(Inner.GetComponent<Animator>()));
    }

    IEnumerator WaitForDoorAnimation(Animator targetAnimator)
    {
        yield return null;
        
        float animationLength = targetAnimator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(animationLength);
        GetComponent<AudioSource>().Play();
        isAnimating = false;
        OnDoorAnimationFinish();
        
    }

    public void OnDoorAnimationFinish()
    {
        Outer.GetComponent<Animator>().SetBool("open", GameManager.isInside);
        Inner.GetComponent<Animator>().SetBool("open", !GameManager.isInside);
        GameManager.isInside = !GameManager.isInside;
    }

    
}
