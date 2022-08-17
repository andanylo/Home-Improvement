using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class TaskMenu : MonoBehaviour
{
    public Text header;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void changeHeader(string headerText){
        header.text = headerText;
    }
}
