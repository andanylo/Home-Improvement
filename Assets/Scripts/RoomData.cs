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

    public RoomData()
    {
        this.id = "";
        this.key_word = "";
        this.pos = new Vector2(0f, 0f);
        this.rot = new Vector3(0f, 0f, 0f);

        this.connections = new List<Connection>();
    }

    public RoomData(RoomJSON jsonValues)
    {
        this.id = "";
        this.key_word = "";

        this.pos = new Vector2(jsonValues.xPos, jsonValues.yPos);
        this.rot = new Vector3(0f, 0f, jsonValues.yRot);

        this.connections = new List<Connection>();
        foreach (ConnectionJSON connection in jsonValues.connections)
        {
            this.connections.Add(new Connection(connection));
        }
    }
}

public class Connection
{
    public string id;

    public Direction direction;

    public Connection(string id, Direction direction)
    {
        this.id = id;
        this.direction = direction;
    }

    public Connection(ConnectionJSON jsonValues)
    {
        this.id = jsonValues.roomID;
        System.Enum.TryParse(jsonValues.direction, out this.direction);
    }
}
