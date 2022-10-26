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
        transform.position = new Vector3(this._furnitureData.pos.x, this._furnitureData.pos.y, 0.0f);
    
        transform.transform.eulerAngles = this._furnitureData.rot;
    }
    }

    public GameObject exclamationMark;
    public UIManager manager;

    private PlayerTask playerTask{
        get{
            return manager.taskManager.getPlayerTaskFromFurniture(furnitureData);
        }
    }

    private bool? _isExclamationMarkHidden;
    private bool? isExclamationMarkHidden{
        get{
            return _isExclamationMarkHidden;
        }
        set{
            this._isExclamationMarkHidden = value;
            if(exclamationMark != null){
                exclamationMark.SetActive(!(value == true));
            }
        }
    }
    public void didChangeCompleteTaskStatus(bool complete){
        isExclamationMarkHidden = complete;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if(isExclamationMarkHidden == null){
            if(playerTask != null){
                if(playerTask.didChangeCompleteStatus == null){
                    playerTask.didChangeCompleteStatus = new DidChangeCompleteStatus();
                    playerTask.didChangeCompleteStatus.AddListener(didChangeCompleteTaskStatus);
                    isExclamationMarkHidden = playerTask.complete_Status;
            
                
                }
            }
        }
    }
}
