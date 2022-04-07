using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitch : MonoBehaviour
{
    private int sceneID;

    // Start is called before the first frame update
    void Start()
    {
        sceneID = SceneManager.GetActiveScene().buildIndex;
    }

    //methods for Main Menu buttons
    public void SelectApartment()
    {
        SceneManager.LoadScene("Apartment");

        sceneID = SceneManager.GetActiveScene().buildIndex;
    }

    public void SelectHouse()
    {
        SceneManager.LoadScene("HouseGroundFloor");

        sceneID = SceneManager.GetActiveScene().buildIndex;
    }

    //method to change from ground floor to basement and vice versa in house level
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(sceneID == 2)
        {
            SceneManager.LoadScene(3);
        }
        else
        {
            SceneManager.LoadScene(2);
        }

        sceneID = SceneManager.GetActiveScene().buildIndex;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
