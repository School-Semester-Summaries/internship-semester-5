using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarTaskButtonCollection : MonoBehaviour
{
    public GridLayoutGroup GridLayoutGroup; // the grid inside unity that makes them appear nicely under each other
    public List<CarTaskButton> CarTaskButtonList = new List<CarTaskButton>();
    private void Start()
    {
        CreateCarTaskButtons();
    }

    /// <summary>Creates a button for each existing CarTask</summary> // if you want to add a cartask you should be at CarTaskCollection
    void CreateCarTaskButtons()
    {
        List<CarTask> TaskList = new CarTaskCollection().AllTasks;
        foreach (CarTask carTask in TaskList)
        {
            CarTaskButton g = CreateButton(carTask);
            CarTaskButtonList.Add(g);
        }
    }
    CarTaskButton CreateButton(CarTask carTask)
    {
        GameObject prefab = Resources.Load("prefabs/CarTaskButton") as GameObject;
        GameObject button = Instantiate(prefab);
        button.transform.SetParent(GridLayoutGroup.transform);
        CarTaskButton carTaskButton = button.GetComponent<CarTaskButton>();
        carTaskButton.SetCarTask(carTask);
        carTaskButton.SetButtonText(carTask.Name);
        return carTaskButton;
    } // single button
}
