using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpenAnimation : MonoBehaviour
{
    private Animator mAnimator;
    
    private void OnEnable()
    {
        TempleDoorOpen.OnDoorOpen += animateDoorOpen;
    }
    
    private void OnDisable()
    {
        TempleDoorOpen.OnDoorOpen -= animateDoorOpen;
    }
    void Awake()
    {
        mAnimator = GetComponent<Animator>();
    }
    
    void animateDoorOpen()
    {
        // Debug.Log("OnDoorOpen triggered");
        if (mAnimator != null)
        {
            mAnimator.SetTrigger("OpenTrigger");
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
