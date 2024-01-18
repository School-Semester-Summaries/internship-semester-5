using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    const string TAG_LOAD_BUTTON = "LoadSaveButton";
    const string TAG_MAIN_CAMERA = "MainCamera";

    public float MoveSpeed; // amount of tiles player moves, setted in runtime
    public Transform MovePoint; // player transform
    public LayerMask StopMovementLayer; // StopMovementLayer is the layer that stops the player form moving, for example the blue blocks in the game contain this layey. Same for the black boundaries
    public List<CarTask> TaskList { get; private set; } // a list with all assigned cartasks, these get loaded from a data source (currently textfile)

    LoadTasks _loadTasks;
    Inventory _inventory; // player inventory

    private void Start()
    {
        _inventory = new Inventory();
        _loadTasks = GameObject.FindGameObjectWithTag(TAG_LOAD_BUTTON).GetComponent<LoadTasks>();
        TaskList = _loadTasks.TaskList; // load tasks form LoadTasks (which loads it from datafile)

        //TaskList = Data.CarTaskData.LoadCarTasksFromTextFile(); // load cartasks from datafile
        //TaskList = GameObject.FindGameObjectWithTag("LoadSaveButton").GetComponent<LoadTasks>().TaskList; // HARDCODE: Assign all tasks to player, only use for testing (this is used in itch)
    }
    void Update()
    {
        Movement();
    }
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (GameModeManager.Gamemode == Gamemodes.Play) // these only get triggered if you are in the play section
        {
            Item item = col.gameObject.GetComponent<Item>(); // get the item you collided with
            Debug.Log("Collided with: " + item.ItemType);

            // Collect any collectable item
            if (item.Collectable == true)
            {
                _inventory.AddItem(item);
            }
            // if the item you collided with is a StartItem it checks if its the correct StartItem for the current CarTask
            else if (item.ItemType == TaskList[0].StartItem)
            {
                if (_inventory.HasItem(TaskList[0].RequiredTools)) // does your inventory contains the required items for this task?
                {
                    DisableControls(); // so you cant accidentally move during task
                    TaskList[0].Activate(); // activate the current task
                }
                else
                {
                    PrintRequiredTools(); // print the tools you didnt collect yet, try to show this in the UI somehow for better gameplay
                }
            }
        }
        else
        {
            Debug.Log(GameModeManager.Gamemode);
        }
    }
    void CameraBackToPlaySection()
    {
        GameObject.FindGameObjectWithTag(TAG_MAIN_CAMERA).GetComponent<PlayScene>().SwitchToPlayCam();
    } // after a minigame, this method returns the camera to the original position.

    // CarTask/Items
    public void FinishCurrentTask()
    {
        TaskList.Remove(TaskList[0]); // remove current task form the tasklist
        CameraBackToPlaySection();
        LoadInNextTask();
        EnableControls();
    }
    void PrintRequiredTools()
    {
        string taskTip = "Je hebt de volgende tools niet opgepakt: ";
        foreach (var item in TaskList[0].RequiredTools)
        {
            if (!_inventory.ItemList.Contains(item)) // if you didn't collect the required tool, print it, again preferably put it in UI for better gameplay
            {
                taskTip += item.ToString();
                taskTip += ", ";
            }
        }
        _loadTasks.SetTaskTip(taskTip);
    } // try to show this on the screen
    void LoadInNextTask()
    {
        LoadTasks loadTasks = GameObject.FindGameObjectWithTag(TAG_LOAD_BUTTON).GetComponent<LoadTasks>();
        if (loadTasks.TaskList.Count > 0)
        {
            GameObject.FindGameObjectWithTag(TAG_LOAD_BUTTON).GetComponent<LoadTasks>().LoadInTask(TaskList[0]); 
        }
        else
        {
            loadTasks.Finish();
        }
    }
    public void UseItem(Items item, UIItem uiItem)
    {
        _inventory.RemoveItem(item); // removes item in the code
        Destroy(uiItem.gameObject); // removes item onscreen
    }
    public void SetTaskList(List<CarTask> t)
    {
        TaskList = t;
    } // sets the TaskList if the player

    // Controls
    public void DisableControls()
    {
        print("Disabling Controls");
        GameModeManager.SetGamemode(Gamemodes.CarTask);
    }
    public void EnableControls()
    {
        print("Enabling Controls");
        GameModeManager.SetGamemode(Gamemodes.Play);
    }

    // Movement
    /// <summary>Moves the player in a direction using the arrow keys</summary>
    void Movement()
    {
        // only enable movement inside play mode
        if (GameModeManager.Gamemode == Gamemodes.Play)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            {
                FaceRight();
                // Check if the space you try to move in contains an object with the layer "StopMovement" (Or what ever the variable stopMovementLayer has been assigned to)
                if (!Physics2D.OverlapCircle(MovePoint.position + new Vector3(MoveSpeed, 0f, 0f), float.MinValue, StopMovementLayer))
                {
                    MoveForward();
                }
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            {
                FaceLeft();
                if (!Physics2D.OverlapCircle(MovePoint.position - new Vector3(MoveSpeed, 0f, 0f), float.MinValue, StopMovementLayer))
                {
                    MoveForward();
                }
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
            {
                FaceDown();
                if (!Physics2D.OverlapCircle(MovePoint.position - new Vector3(0f, MoveSpeed, 0f), float.MinValue, StopMovementLayer))
                {
                    MoveForward();
                }
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
            {
                FaceUp();
                if (!Physics2D.OverlapCircle(MovePoint.position + new Vector3(0f, MoveSpeed, 0f), float.MinValue, StopMovementLayer))
                {
                    MoveForward();
                }
            }
        }
    }
    void MoveUp()
    {
        MovePoint.position += new Vector3(0f, MoveSpeed, 0f);
    }
    void MoveRight()
    {
        MovePoint.position += new Vector3(MoveSpeed, 0f, 0f);
    }
    void MoveDown()
    {
        MovePoint.position -= new Vector3(0f, MoveSpeed, 0f);
    }
    void MoveLeft()
    {
        MovePoint.position -= new Vector3(MoveSpeed, 0f, 0f);
    }
    /// <summary>Makes the player move the direction it's facing a tile</summary>
    void MoveForward()
    {
        if (MovePoint.rotation.z == 0)
        {
            MoveUp();
        }
        else if (Math.Round(MovePoint.rotation.w, 2) == -0.71)
        {
            MoveRight();
        }
        else if (MovePoint.rotation.z == 1)
        {
            MoveDown();
        }
        else if (Math.Round(MovePoint.rotation.w, 2) == 0.71)
        {
            MoveLeft();
        }
    }

    // Directions
    void FaceUp()
    {
        MovePoint.rotation = Quaternion.Euler(MovePoint.rotation.x, MovePoint.rotation.y, 0f);
    }
    void FaceRight()
    {
        MovePoint.rotation = Quaternion.Euler(MovePoint.rotation.x, MovePoint.rotation.y, 270f);
    }
    void FaceDown()
    {
        MovePoint.rotation = Quaternion.Euler(MovePoint.rotation.x, MovePoint.rotation.y, 180f);
    }
    void FaceLeft()
    {
        MovePoint.rotation = Quaternion.Euler(MovePoint.rotation.x, MovePoint.rotation.y, 90f);
    }
}
