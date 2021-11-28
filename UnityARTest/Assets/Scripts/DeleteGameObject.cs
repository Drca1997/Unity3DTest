using UnityEngine;

public class DeleteGameObject : MonoBehaviour, IClickable
{
    private GameObject model;

    public GameObject Model { get => model; set => model = value; }

   

    void FixedUpdate()
    {
        transform.LookAt(Camera.main.transform); //makes the button always face the user
    }

    /*
     * After button getting clicked, it destroys the model and itself
     */
    public void OnClick()
    {
        Destroy(Model);
        Destroy(gameObject);
    }

}
