using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadSaveButton : MonoBehaviour // this class is a mess, inside the application you manually have to give all prefabs (its hardcoded), try to remove that from the application and make it dynamic
{
    public Button loadSaveButtonClick; // the load button
    public List<GameObject> prefabList = new List<GameObject>(); // WARNING - since this property is assigned inside unity it will take a very longtime to re-assign everything, so try not to rename this property
    public CarTaskButtonCollection CarTaskButtonCollection;
    void Start()
    {
        Button btn = loadSaveButtonClick.GetComponent<Button>();
        btn.onClick.AddListener(ButtonClick); // assign click event to the button
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
        List<(int id, int x, int y)> data = Data.GridData.LoadAsTupleList();
        while (data.Count > 0)
        {
            Debug.Log("Finding Prefab by ID");
            GameObject prefab = FindPrefabByID(data[0].id);
            Debug.Log("Found ID: " + data[0].id);
            Debug.Log("Found prefab: " + prefab);
            Vector3 pos = new Vector3(data[0].x, data[0].y);
            Instantiate(prefab, pos, new Quaternion());
            data.RemoveAt(0);
        }
    }
    GameObject FindPrefabByID(int id)
    {
        Debug.Log("PrefabList[0]: " + prefabList[0]);
        Debug.Log("PrefabList[1]: " + prefabList[1]);
        GameObject correctPrefab = null;
        foreach (GameObject prefab in prefabList)
        {
            int prefabID = prefab.GetComponent<Item>().PrefabID;
            Debug.Log(prefabID + ", " + id);
            if (prefabID == id)
            {
                correctPrefab = prefab;
                break; // if this method fucks up, remove this line
            }
        }
        return correctPrefab;
    } // we can find prefabs by id because of the hardcoded prefabList

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
