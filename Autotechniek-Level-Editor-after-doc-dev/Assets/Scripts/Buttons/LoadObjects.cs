using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadObjects : MonoBehaviour 
{
    public Button LoadButton; // the load button
    public CarTaskButtonCollection CarTaskButtonCollection;
    GameObject[] prefabList;
    void Start()
    {
        Button btn = LoadButton.GetComponent<Button>();
        btn.onClick.AddListener(ButtonClick); // assign click event to the button
        prefabList = Resources.LoadAll<GameObject>("prefabs/items");
    }
    void ButtonClick()
    {
        LoadObjectsFromSave();
        DisableButton();
        if (GameModeManager.Gamemode == Gamemodes.Level_Editor) 
        {
            EnableCarTaskButtons();
            LoadSelectedTasks();
        }
    }
    void DisableButton()
    {
        GetComponent<Button>().interactable = false;
    }
    void LoadObjectsFromSave()
    {
        // parent of all loaded objects
        GameObject loadedObjects = new GameObject();
        loadedObjects.name = "Dynamically Loaded Objects";

        List<(int id, int x, int y)> data = Data.GridData.LoadAsTupleList();
        while (data.Count > 0)
        {
            GameObject prefab = FindPrefabByID(data[0].id);
            GameObject instantiatedPrefab = Instantiate(prefab, new Vector3(data[0].x, data[0].y), new Quaternion());
            instantiatedPrefab.transform.parent = loadedObjects.transform;
            data.RemoveAt(0); // remove first data entry so we can keep on getting the first one till its empty
        }
    }
    GameObject FindPrefabByID(int id)
    {
        Debug.Log("Finding prefab with ID = " + id);
        GameObject correctPrefab = null;
        foreach (GameObject prefab in prefabList)
        {
            int prefabID = prefab.GetComponent<Item>().PrefabID;
            Debug.Log(prefabID + ", " + id);
            if (prefabID == id)
            {
                correctPrefab = prefab;
                break;
            }
        }
        Debug.Log("Found prefab: " + correctPrefab);
        return correctPrefab;
    }
    void LoadSelectedTasks()
    {
        List<CarTask> selectedTasks = Data.CarTaskData.LoadCarTasksFromTextFile();


        foreach (CarTaskButton carTaskButton in CarTaskButtonCollection.CarTaskButtonList)
        {
            foreach (CarTask carTask in selectedTasks)
            {
                if (carTaskButton.CarTask.ID == carTask.ID) // als de button in de list, een cartask id bevat die identiek is aan de id in selectedtask, maak deze button dan groen
                {
                    carTaskButton.gameObject.GetComponent<Image>().color = Color.green;
                    break;
                }
            }
        }
    }
    void EnableCarTaskButtons()
    {
        foreach (CarTaskButton carTaskButton in CarTaskButtonCollection.CarTaskButtonList)
        {
            carTaskButton.gameObject.GetComponent<Button>().interactable = true;
        }
    }
}
