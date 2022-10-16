using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomData
{
    public string id;
    public string key_word;
    public Vector2 pos;
    public Vector3 rot;

    //Connections with other rooms
    public List<Connection> connections;

    public RoomData(){
        this.id = "";
        this.key_word = "";
        this.pos = new Vector2(0f, 0f);
        this.rot = new Vector3(0f, 0f, 0f);

        this.connections = new List<Connection>();
    }
}
public class Connection{
    public string id;
    public Direction direction;

    public Connection(string id, Direction direction){
        this.id = id;
        this.direction = direction;
    }
}