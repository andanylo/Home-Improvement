using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction{
    top, bottom, left, right

    
}

public class DirectionManager{

    private static DirectionManager instance = null;

 

    public static DirectionManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new DirectionManager();
            }
            return instance;
        }
    }

    public List<Direction> directionList = new List<Direction>();

    public DirectionManager(){
        directionList.Add(Direction.right);
        directionList.Add(Direction.bottom);
        directionList.Add(Direction.left);
        directionList.Add(Direction.top);
    }

    public Direction getDirectionFromNumber(int number){
        return directionList[number];
    }

    

    public Direction findOppositeDirection(Direction fromDirection){
        switch(fromDirection){
            case Direction.right:
                return Direction.left;
            case Direction.top:
                return Direction.bottom;
            case Direction.left:
                return Direction.right;
            case Direction.bottom:
                return Direction.top;
        }
        return Direction.right;
    }

    //Get new direction on rotation
    public Direction getNewDirectionFromDegrees(float rotationDegree, Direction forDirection){
        int directionNumber = (((int)rotationDegree) / 90) % 4;

        int currentDirectionIndex = directionList.FindIndex(direction => direction == forDirection);

        int newIndex = currentDirectionIndex - directionNumber;
        if(newIndex < 0){

            //Get index from top of the list instead
            newIndex = directionList.Count + newIndex;
        }
        return getDirectionFromNumber(newIndex);
    }

    //Get direction from rotation degrees based on list position
    public Direction convertFromRotationDegrees(float rotationDegree){
        int directionNumber = (((int)rotationDegree) / 90) % 4;
        
        return getDirectionFromNumber(directionNumber);
    }


    public static string convertToString(Direction direction){
        return Direction.GetName(typeof (Direction), direction);
    }

    //Converting direction into rotation degrees (assuming that right direction is default)
    // public float convertDirectionIntoRotationDegrees(Direction direction){
        
    //     return 0f;
    // }
}