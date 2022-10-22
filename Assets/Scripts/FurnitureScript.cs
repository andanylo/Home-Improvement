using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurnitureScript : MonoBehaviour
{
    private FurnitureData _furnitureData;
    public FurnitureData furnitureData
    {
    get => _furnitureData;
    set{
        this._furnitureData = value;
        transform.position = this._furnitureData.pos;
        transform.transform.eulerAngles = this._furnitureData.rot;
    }
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
