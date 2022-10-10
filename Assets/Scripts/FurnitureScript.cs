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
        transform.localScale = new Vector2(1.2372f, 1.6968f);
        transform.position = this.furnitureData.pos;
        transform.Rotate(this.furnitureData.rot);
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
