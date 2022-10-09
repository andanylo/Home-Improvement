using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class FurnitureData
{
    public string name;
    public Vector2 pos;
    public Vector3 rot;
    public string furnitureID;

    public FurnitureData(){
        this.name = "";
        this.pos = new Vector2(0.0f, 0.0f);
        this.rot = new Vector3(0.0f, 0.0f, 0.0f);
        this.furnitureID = "";
    }
}
