using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateDeleteButton : MonoBehaviour, IClickable
{
    private bool isButtonCreated;

    private void Start()
    {
        isButtonCreated = false;
    }

    /*
     * After the gameobject getting clicked by the user, 
     * creates the delete button under the model, if it was not created yet
     */
    public void OnClick()
    {
        if (!isButtonCreated)
        {
            GameObject template = Resources.Load("DeleteButton") as GameObject;
            Vector3 buttonPos = new Vector3(transform.position.x, transform.position.y - 0.04f, transform.position.z);
            GameObject deleteButton = Instantiate(template, buttonPos, Quaternion.identity);
            deleteButton.GetComponent<DeleteGameObject>().Model = gameObject;
            isButtonCreated = true;
        }  
    }
}
