using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.Web;

public class SaveManager : MonoBehaviour
{
    private EditFurniture editFurniture;

    
    void Start()
    {

    }

    public void setEditFurnitureObject(EditFurniture editFurniture){
        this.editFurniture = editFurniture;
    }


    [Serializable]
    private class FurnitureJSON{
        public string name;
        public string id;

        public float xPos;
        public float yPos;

        public float xRot;
        public float yRot;

        public FurnitureJSON(){
            this.name = "";
            this.id = "";

            this.xPos = 0.0f;
            this.yPos = 0.0f;

            this.xRot = 0.0f;
            this.yRot = 0.0f;
        }
    }
    //Save room to firebase 
    public void saveFurniture(UnityMessageManager messenger, FurnitureData furnitureData){
        if(furnitureData != null){

            FurnitureJSON data = new FurnitureJSON();
            data.name = furnitureData.name.ToLower().Replace(' ', '_');
            data.id = furnitureData.furnitureID;
            data.xPos = furnitureData.pos.x;
            data.yPos = furnitureData.pos.y;

            data.xRot = 0.0f;
            data.yRot = furnitureData.rot.z;

            //Convert object into json
            string json = JsonUtility.ToJson(data);
            //Send it to flutter app
            messenger.SendMessageToFlutter("saveFurniture:"+json);
        }
                
    }

    void Update()
    {
        
    }
}
