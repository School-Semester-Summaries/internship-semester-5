using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

// This class is pretty chaotic, sorry

// I've put the Dutch names of tools next to it since you will encounter them in strings in the application
public enum Items
{
    None = -1,
    Player = 0,
    BlueBlock = 1,
    GreenBlock = 2,
    PinkBlock = 3,
    Drill = 4, // Boor
    Car = 5, // Auto
    Wheel = 6, // Wiel
    Bolt = 7, // Schroef
    BoltHole = 8, // Gat zonder schroef
    CrossSocketWrench = 9, // Kruissleutel
    TorqueWrench = 10, // Moment Sleutel
    EngineHood = 11, // Motor Kap
    CarBattery = 12, // Accu
    Wrench = 13, // Steek Sleutel
}

public class Item : MonoBehaviour
{
    public int PrefabID { get { return (int)ItemType; } }
    public bool Collectable; // WARNING - since this property is assigned inside unity it will take a very longtime to re-assign everything, so try not to rename this property
    public Items ItemType; // WARNING - since this property is assigned inside unity it will take a very longtime to re-assign everything, so try not to rename this property
    UIItemCollection _uiItemCollection;
    CarTask1 _carTask1; // you might be able to change this type to CarTask instead of CarTask1

    private void Start()
    {
        _uiItemCollection = GameObject.FindGameObjectWithTag("UIItemCollection").GetComponent<UIItemCollection>();
    }
    private void OnCollisionEnter2D(Collision2D col)
    {
        // Destroy an collectable item if the player collides with it (because it gets put in the inventory, get it? destroy in scene, add to inventory)
        if (col.gameObject.tag == "Player" && Collectable == true && GameModeManager.Gamemode == Gamemodes.Play)
        {
            Destroy(this.gameObject);
        }
    }
    public void OnMouseDown()
    {
        // if you are currently in a cartask, then run this code
        if (GameModeManager.Gamemode == Gamemodes.CarTask)
        {
            _carTask1 = (CarTask1)GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().TaskList[0]; // might be able to cast to CarTask
            _carTask1.Minigame_Item(PrefabID, this.gameObject); // again you might be able to change the variable to _carTask form the type CarTask
        }

        // if you click on an item, but the eraser has been selected, then run this code
        if (_uiItemCollection.EraserSelected == true && GameModeManager.Gamemode == Gamemodes.Level_Editor)
        {
            GameObject destroyedObject = gameObject; // save the gameobject we are destroying so we can also remove it from the data
            Destroy(gameObject);
            RemoveGameObjectFromData(destroyedObject);
        }
        Debug.Log("Clicked on " + ItemType);
    }
    async void RemoveGameObjectFromData(GameObject destroyedObject)
    {
        Item destroyedItem = destroyedObject.GetComponent<Item>();
        List<(int id, int x, int y)> data = Data.GridData.LoadAsTupleList(); // load the grid data
        // find the corret data entry to remove
        (int id, int x, int y) removeThisEntry = (-1, -1, -1);
        int i = 0;
        foreach ((int id, int x, int y) entry in data)
        {
            if (entry.id == destroyedItem.PrefabID && entry.x == destroyedObject.transform.position.x && entry.y == destroyedObject.transform.position.y)
            {
                removeThisEntry = entry;
                break;
            }
            i++;
        }
        data.Remove(removeThisEntry); // remove the data
        await Data.GridData.Overwrite(data); // overwrite the data (so we overwrite the exact same data but without the entry we removed)
    }
}

