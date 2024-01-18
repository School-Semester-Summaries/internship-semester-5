using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum CarTask2Tasks
{
    RemovePlusAndMin,
    RemoveOldBattery,
    AttachNewBattery,
    AttachPlus,
    AttachMin,
}

public class CarTask2 : CarTask
{
    public CarTask2(int id, string name, Items startItem, string description, params Items[] itemArray) : base(id, name, startItem, description, itemArray)
    {
        ID = id;
        Name = name;
        StartItem = startItem;
        Description = description;
        RequiredTools = itemArray;
    }
    public override void Activate()
    {
        Debug.Log("Good Job! Task 2 Finished!");
        Deactivate(); // when you make a minigame, put this in the last line right after finishing the minigame
    } // Activates the CarTask, doesn't contain code
    public override void Deactivate()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().FinishCurrentTask();
    } // Deactivates CarTask
}



