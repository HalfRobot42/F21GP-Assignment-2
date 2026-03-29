using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpenAnimation : MonoBehaviour
{
    private Animator mAnimator;
    public AudioClip openSound;
    public GameObject doorCentre;

    //public GameObject templeDoor;

    private void OnEnable()
    {
        //TempleDoorOpen.OnDoorOpen += animateDoorOpen;
        GetComponentInChildren<TempleDoorOpen>().OnDoorOpen += animateDoorOpen;
    }
    
    private void OnDisable()
    {
        //TempleDoorOpen.OnDoorOpen -= animateDoorOpen;
        GetComponentInChildren<TempleDoorOpen>().OnDoorOpen -= animateDoorOpen;
    }
    void Awake()
    {
        mAnimator = GetComponent<Animator>();
    }
    
    void animateDoorOpen()
    {
        Debug.Log("OnDoorOpen triggered");
        if (mAnimator != null)
        {
            mAnimator.SetTrigger("OpenTrigger");
            AudioSource.PlayClipAtPoint(openSound, doorCentre.transform.position, 1.0F);
        }
    }

    // Testing code
    // void Update()
    // {
    //     if (Input.GetKeyDown(KeyCode.E))
    //     {
    //         mAnimator.SetTrigger("OpenTrigger");
    //     }
    // }
}
