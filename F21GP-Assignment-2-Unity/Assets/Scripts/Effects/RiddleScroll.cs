using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiddleScroll : MonoBehaviour
{
    public GameObject riddleUI;
    // Start is called before the first frame update
   private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (riddleUI != null) riddleUI.SetActive(true);
            
         //Hide the "Press E" prompt
            if (InteractionUI.Instance != null) InteractionUI.Instance.Hide();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (riddleUI != null) riddleUI.SetActive(false);
        }
    }
}
