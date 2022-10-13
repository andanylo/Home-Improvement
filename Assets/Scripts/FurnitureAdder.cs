using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json; 
using Newtonsoft.Json.Linq; 

public class FurnitureAdder : MonoBehaviour
{

 

    public UIManager uimanager;
    // Start is called before the first frame update
    void Start()
    {

       // recieveFurnituresFromDatabase(" {\"kitchen_sink\":{\"4f03410a-22a2-417a-8bc2-fa97cd60f3eb\": {\"room_ID\": \"n/a\",\"xPos\": 2.092411994934082,\"xRot\": 0,\"yPos\": 0.10121464729309082,\"yRot\": 0}}}");
    }



    // Update is called once per frame
    void Update()
    {
        
    }
    //Get furniture data object based on parameters
    private FurnitureData getFurnitureData(string name, string furnitureID, Dictionary<string, JValue> parameters){
        FurnitureData furnitureData = new FurnitureData();
        furnitureData.name = name;
        furnitureData.furnitureID = furnitureID;

        //Get parameters
        foreach(KeyValuePair<string, JValue> parameter in parameters){
            switch(parameter.Key){
                case "xPos":
                    furnitureData.pos.x = parameter.Value.ToObject<float>();
                    break;
                case "yPos":
                    furnitureData.pos.y = parameter.Value.ToObject<float>();
                    break;
                case "xRot":
                    break;
                case "yRot":
                    furnitureData.rot.z = parameter.Value.ToObject<float>();
                    break;
                default:
                    break;
            }
        }
        
        return furnitureData;
    }

    public void recieveFurnituresFromDatabase(string jsonFurniture){
        Dictionary<string, JObject> values = JsonConvert.DeserializeObject<Dictionary<string, JObject>>(jsonFurniture);

        List<FurnitureData> list = new List<FurnitureData>();
        // //Foreach furniture type
        foreach(KeyValuePair<string, JObject> entry in values){
            string name = entry.Key;

            


           Dictionary<string, JObject> obj = entry.Value.ToObject<Dictionary<string, JObject>>();
            
            
             //Foreach furniture object
             foreach(KeyValuePair<string, JObject> furniture in obj){
                 string furnitureID = furniture.Key;


                Dictionary<string, JValue> parameters = furniture.Value.ToObject<Dictionary<string, JValue>>();
                
                
                FurnitureData furnitureData = getFurnitureData(name, furnitureID, parameters);
                if(furnitureData != null){
                    list.Add(furnitureData);
                }
                
            }
         }
         
         fetchFurniture(list);

    }

    public void addFurniture(FurnitureData data){
        GameObject furniture;


        Debug.Log(data.name);
        GameObject prefab = Resources.Load<GameObject>("Prefabs/Furnitures/" + data.name);
        
        furniture = Instantiate(prefab) as GameObject;
        furniture.AddComponent<FurnitureScript>();
        furniture.name = "Furniture_Object";
        
        furniture.GetComponent<FurnitureScript>().furnitureData = data;

        uimanager.furnitures.Add(furniture);
    }


    public void updateFurniture(FurnitureData newData){
        foreach(GameObject furniture in uimanager.furnitures){
            furniture.SetActive(true);
            FurnitureScript furnitureScript = furniture.GetComponent<FurnitureScript>();
            if(furnitureScript != null){

                //Check ids of furniture data
                if(furnitureScript.furnitureData.furnitureID == newData.furnitureID){
                    furnitureScript.furnitureData = newData;
                }
            }
        }
    }

    //Add furniture from fdatabase based on list of data
    public void fetchFurniture(List<FurnitureData> furnitureList){
        foreach(GameObject furnitureGameObject in uimanager.furnitures){
            Destroy(furnitureGameObject);
        }

        foreach(FurnitureData data in furnitureList){
   
            addFurniture(data);
        }
    }

}

