using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.Web;

public class SaveManager 
{

    
 



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


    [Serializable]
    private class RoomJSON{
        public string id;
        public string key_word;

        public float xPos;
        public float yPos;

        public float xRot;
        public float yRot;

        public List<ConnectionJSON> connections;
    }

    [Serializable]
    private class ConnectionJSON{
        public string roomID;
        public string direction;
    }
    //Convert room to JSON
    public static void saveOrUpdateRoom(UnityMessageManager messenger, RoomData roomData, bool isUpdating){
        if(roomData != null){
            RoomJSON data = new RoomJSON();
            data.id = roomData.id;
            data.key_word = roomData.key_word;

            data.xPos = roomData.pos.x;
            data.yPos = roomData.pos.y;

            data.xRot = 0.0f;
            data.yRot = roomData.rot.z;

            data.connections = new List<ConnectionJSON>();
            foreach(Connection connection in roomData.connections){
                ConnectionJSON connectionjson = new ConnectionJSON();
                connectionjson.roomID = connection.id;
                connectionjson.direction = DirectionManager.convertToString(connection.direction);
                data.connections.Add(connectionjson);
            }

            string json = JsonUtility.ToJson(data);
            Debug.Log(json);
            // if(isUpdating){
            //     messager.SendMessageToFlutter("saveRoom:"+json);
            // }
            // else{

            // }
        }
    }



    //Save room to firebase 
    public static void saveOrUpdateFurniture(UnityMessageManager messenger, FurnitureData furnitureData, bool isUpdating){
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

            if(isUpdating){
                messenger.SendMessageToFlutter("updateFurniture:"+json);
            }
            else{
                messenger.SendMessageToFlutter("saveFurniture:"+json);
            }
            
        }
                
    }

  
}
