using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public float moveSpeed; // amount of tiles player moves, setted in runtime
    public Transform movePoint; // player transform
    public SpriteRenderer spriteRenderer; // player sprite
    public LayerMask stopMovementLayer; // StopMovementLayer is the layer that stops the player form moving, for example the blue blocks in the game contain this layey. Same for the black boundaries
    public List<CarTask> TaskList { get; private set; } // a list with all assigned cartasks. these get loaded from a data source (currently textfile)
    Inventory _inventory; // player inventory

    private void Start()
    {
        _inventory = new Inventory();
        TaskList = Data.CarTaskData.LoadCarTasksFromTextFile(); // load cartasks from datasource
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
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<PlayMode>().SwitchToPlayCam();
    }

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
        print("You didn't collect the required tools. Required Tools: ");
        foreach (var item in TaskList[0].RequiredTools)
        {
            if (!_inventory.ItemList.Contains(item)) // if you didn't collect the required tool, print it, again preferably put it in UI for better gameplay
            {
                Debug.Log(item);
            }
        }
    } // might wannna show on screen
    void LoadInNextTask()
    {
        GameObject.FindGameObjectWithTag("LoadSaveButton").GetComponent<LoadTasks>().LoadInTask(TaskList[0]);
    }
    public void UseItem(Items item, UIItem uiItem)
    {
        _inventory.RemoveItem(item); // removes item in the code
        Destroy(uiItem.gameObject); // removes item onscreen
    }


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
                if (!Physics2D.OverlapCircle(movePoint.position + new Vector3(moveSpeed, 0f, 0f), float.MinValue, stopMovementLayer))
                {
                    MoveForward();
                }
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            {
                FaceLeft();
                if (!Physics2D.OverlapCircle(movePoint.position - new Vector3(moveSpeed, 0f, 0f), float.MinValue, stopMovementLayer))
                {
                    MoveForward();
                }
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
            {
                FaceDown();
                if (!Physics2D.OverlapCircle(movePoint.position - new Vector3(0f, moveSpeed, 0f), float.MinValue, stopMovementLayer))
                {
                    MoveForward();
                }
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
            {
                FaceUp();
                if (!Physics2D.OverlapCircle(movePoint.position + new Vector3(0f, moveSpeed, 0f), float.MinValue, stopMovementLayer))
                {
                    MoveForward();
                }
            }
        }
    }
    void MoveUp()
    {
        movePoint.position += new Vector3(0f, moveSpeed, 0f);
    }
    void MoveRight()
    {
        movePoint.position += new Vector3(moveSpeed, 0f, 0f);
    }
    void MoveDown()
    {
        movePoint.position -= new Vector3(0f, moveSpeed, 0f);
    }
    void MoveLeft()
    {
        movePoint.position -= new Vector3(moveSpeed, 0f, 0f);
    }
    /// <summary>Makes the player move the direction it's facing a tile</summary>
    void MoveForward()
    {
        if (movePoint.rotation.z == 0)
        {
            MoveUp();
        }
        else if (Math.Round(movePoint.rotation.w, 2) == -0.71)
        {
            MoveRight();
        }
        else if (movePoint.rotation.z == 1)
        {
            MoveDown();
        }
        else if (Math.Round(movePoint.rotation.w, 2) == 0.71)
        {
            MoveLeft();
        }
    }

    // Directions
    void FaceUp()
    {
        movePoint.rotation = Quaternion.Euler(movePoint.rotation.x, movePoint.rotation.y, 0f);
    }
    void FaceRight()
    {
        movePoint.rotation = Quaternion.Euler(movePoint.rotation.x, movePoint.rotation.y, 270f);
    }
    void FaceDown()
    {
        movePoint.rotation = Quaternion.Euler(movePoint.rotation.x, movePoint.rotation.y, 180f);
    }
    void FaceLeft()
    {
        movePoint.rotation = Quaternion.Euler(movePoint.rotation.x, movePoint.rotation.y, 90f);
    }
}
