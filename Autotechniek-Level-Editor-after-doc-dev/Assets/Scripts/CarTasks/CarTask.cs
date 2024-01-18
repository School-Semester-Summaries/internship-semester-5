using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CarTask : MonoBehaviour // you can probably move more stuff into the base class, the more you can put in base class the better
{
    public int ID { get; internal set; } // ID of the car task
    public string Name { get; internal set; } // name of the cartask
    /// <summary>The Item the player has to collide with in the scene to start the corresponding CarTask</summary>
    public Items StartItem { get; internal set; }
    /// <summary>Explains the player what to do inside the CarTask</summary>
    public string Description { get; internal set; }
    /// <summary>The Required Tools to start the corresponding CarTask</summary>
    public Items[] RequiredTools { get; internal set; }

    public CarTask(int id, string name, Items startItem, string description, params Items[] itemArray)
    {
        ID = id;
        Name = name;
        Description = description;
        StartItem = startItem;
        RequiredTools = itemArray;
    }
    /// <summary>Only use this when you need to assign</summary>
    public abstract void Activate();
    public abstract void Deactivate();
}
