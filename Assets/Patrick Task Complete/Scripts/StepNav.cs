using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StepNav : MonoBehaviour
{

    [SerializeField] public GameObject[] steps;
    int index;

    // Start is called before the first frame update
    void Start()
    {
        index = 0;
    }

    // Update is called once per frame
    void Update()
    {

        if (index >= steps.Length - 1){
            index = steps.Length -1;
        }

        if(index < 0){
            index = 0;
        }

        if (index == 0)
        {
            steps[0].gameObject.SetActive(true);
        }
    }

    public void Next()
    {
        index += 1;

        for(int i = 0; i < steps.Length; i++)
        {
            steps[i].gameObject.SetActive(false);
            steps[index].gameObject.SetActive(true);
        }
        
    }

    public void Back()
    {
        index -= 1;

        for(int i = 0; i < steps.Length; i++)
        {
            steps[i].gameObject.SetActive(false);
            steps[index].gameObject.SetActive(true);
        }
        
    }
}
