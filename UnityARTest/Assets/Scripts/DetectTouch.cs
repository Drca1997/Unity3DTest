using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectTouch : MonoBehaviour
{

    /*
     * Every frame, detects if there was a touch on the screen, and 
     * if the touch collided with something
     */
    private void Update()
    {
        if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit)) //if collision happened
            {
                //Evokes the OnClick method, if the gameobject has the IClickable interface
                hit.collider.gameObject.GetComponent<IClickable>()?.OnClick();
            }

        }
    }
}
