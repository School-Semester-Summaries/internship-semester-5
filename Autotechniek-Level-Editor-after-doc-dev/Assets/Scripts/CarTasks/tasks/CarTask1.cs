using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Minigame1Tasks
{
    Task1,
    Task2,
    Task3,
    Task4,
    Task5
}

// minigame is hardcoded, as soon as you make more minigames, it would be nice to code them all oop since you can reuse scripts where you plug in or out smaller objects like bolts n stuff

public class CarTask1 : CarTask
{
    const string TAG_UIITEM_COLLECTION = "UIItemCollection";
    const string TAG_PLAYER = "Player";
    const string TAG_LOAD_BUTTON = "LoadSaveButton";

    public Minigame1Tasks currentTask = Minigame1Tasks.Task1;
    bool _firstBoltTask4 = true;
    int _nBolts = 5;
    int _nPreviousBolt = -1;
    GameObject _mainWheel;

    public CarTask1(int id, string name, Items startItem, string description, params Items[] itemArray) : base(id, name, startItem, description, itemArray)
    {
        ID = id;
        Name = name;
        StartItem = startItem;
        Description = description;
        RequiredTools = itemArray;
    }

    public void RemoveBolt()
    {
        _nBolts--;
        if (_nBolts == 0)
        {
            NextPhase();
        }
    }
    public void RemoveWheel()
    {
        NextPhase();
    }
    public void PlaceWheel()
    {
        NextPhase();
    }
    public void PlaceBolt()
    {
        _nBolts++;
        if (_nBolts == 5)
        {
            NextPhase();
        }
    }
    public void NextPhase()
    {
        int i = (int)currentTask;
        i++;
        currentTask = (Minigame1Tasks)i;
        Debug.Log(currentTask);
    }
    public void Minigame_Item(int prefabID, GameObject gameObject)
    {
        UIItemCollection _uiItemCollection = GameObject.FindGameObjectWithTag(TAG_UIITEM_COLLECTION).GetComponent<UIItemCollection>();

        if (prefabID == (int)Items.Bolt && currentTask == Minigame1Tasks.Task1 && _uiItemCollection.SelectedItemType == Items.CrossSocketWrench)
        {
            gameObject.SetActive(false);
            RemoveBolt();
        }
        else if (prefabID == (int)Items.Wheel && currentTask == Minigame1Tasks.Task2)
        {
            _mainWheel = gameObject;
            gameObject.SetActive(false);
            RemoveWheel();
        }
        else if (prefabID == (int)Items.BoltHole && currentTask == Minigame1Tasks.Task4 && _uiItemCollection.SelectedItemType == Items.TorqueWrench)
        {
            // bolthole id 8
            string currentBolt = gameObject.transform.parent.gameObject.tag;
            int nCurrentBolt = System.Convert.ToInt32(currentBolt.Replace("Bolt ", ""));


            if (_firstBoltTask4)
            {
                gameObject.SetActive(false);
                PlaceBolt();
                _nPreviousBolt = nCurrentBolt;
                _firstBoltTask4 = false;
            }
            // vanaf nu gaan we kijken of je kruislinks te werk gaat
            else
            {
                if (KruisLinksBoltCheck(nCurrentBolt, _nPreviousBolt))
                {
                    gameObject.SetActive(false);
                    PlaceBolt();
                    _nPreviousBolt = nCurrentBolt;
                }
                else
                {
                    LoadTasks loadTasks = GameObject.FindGameObjectWithTag(TAG_LOAD_BUTTON).GetComponent<LoadTasks>();
                    loadTasks.SetTaskTip("Je moet ze kruislinks erin schroeven met de momentsleutel");
                }
            }
        }
        else if (currentTask == Minigame1Tasks.Task5)
        {
            Deactivate();
        }
        else
        {
            Debug.Log("PrefabID: " + prefabID + ", currentTask: " + currentTask + ", _uiItemCollection.SelectedItemType:" + _uiItemCollection.SelectedItemType);
        }
    } // can be moved to CarTask base class
    public void Minigame_UIItem(GameObject prefab, UIItem uiItem)
    {
        Item prefabItem = prefab.GetComponent<Item>();
        if (prefab == null)
        {
            return;
        }
        else if (prefabItem.ItemType == Items.Wheel && currentTask == Minigame1Tasks.Task3)
        {
            _mainWheel.SetActive(true);
            GameObject.FindGameObjectWithTag(TAG_PLAYER).GetComponent<Player>().UseItem(Items.Wheel, uiItem);
            PlaceWheel();
        }
    } // can be moved to CarTask base class
    bool KruisLinksBoltCheck(int nCurrentBolt, int nPreviousBolt)
    {
        if (System.Math.Abs(nCurrentBolt - nPreviousBolt) == 2 || System.Math.Abs(nCurrentBolt - nPreviousBolt) == 3) // draw out this problem to better understand it
        {
            return true; // indeed kruislinks
        }
        else
        {
            return false; // not kruislinks
        }
    }
    public override void Activate()
    {
        // move cam
        GameObject MainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        MainCamera.transform.position = new Vector3(0, -1500, 10);
        // move inventory
        GameObject InventoryObject = GameObject.FindGameObjectWithTag("Inventory");
        InventoryObject.transform.position = new Vector3(850, -1500, 15);
        // move buttonventory
        GameObject ButtonVentory = GameObject.FindGameObjectWithTag("Buttonventory");
        ButtonVentory.transform.position = new Vector3(-850, -1500, 15);
        // log
        Debug.Log("Activate Minigame");
    }
    public override void Deactivate()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().FinishCurrentTask();
    }
}
