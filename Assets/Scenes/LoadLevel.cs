using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LoadLevel : MonoBehaviour
{
    // Start is called before the first frame update
    void OnCollisionEnter(Collision collision){
        Debug.Log("Tocuhed");
        SceneManager.LoadScene("HouseBasement");
    }
}
