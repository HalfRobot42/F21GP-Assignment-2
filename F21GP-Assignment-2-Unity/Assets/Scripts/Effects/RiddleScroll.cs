using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RiddleScroll : MonoBehaviour
{
    public TMP_Text plinthUI;
    public string plinthText;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            plinthUI.text = plinthText;
            plinthUI.gameObject.SetActive(true);
            
            //Hide the "Press E" prompt
            //if (InteractionUI.Instance != null) InteractionUI.Instance.Hide();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            plinthUI.gameObject.SetActive(false);
        }
    }
}
