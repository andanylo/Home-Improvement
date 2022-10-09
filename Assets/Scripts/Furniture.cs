using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Furniture
{
   public string id;
   public string name;
   public string imageName;

   public Furniture(string id, string name, string imageName){
    this.id = id;
    this.name = name;
    this.imageName = imageName;
   }
}
