using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room
{
    public string id;
    public string key_word;
    public Room(){
      this.id = "";
      this.key_word = "";
    }
    public Room(string id, string key_word){
      this.id = id;
      this.key_word = key_word;
    }
}
