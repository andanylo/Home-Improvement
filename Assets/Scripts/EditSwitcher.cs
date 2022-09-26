using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditSwitcher : MonoBehaviour
{

    public GameObject player;
    public GameObject canvas;


    public void changeEditingStatus(string isEditing){
        bool editing = isEditing == "true";
        player.SetActive(!editing);
        canvas.SetActive(!editing);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
