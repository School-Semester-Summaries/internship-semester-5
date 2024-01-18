using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarTaskCollection : MonoBehaviour
{
    /// <summary>List filled with all existing CarTasks</summary>
    public List<CarTask> AllTasks { get; private set; }
    /// <summary>Contains the CarTasks you selected inside the Level Editor</summary>
    public List<CarTask> SelectedTasks { get; private set; }
    public CarTaskCollection()
    {
        AllTasks = new List<CarTask>();

        // These are in Dutch because they will be shown inside the game (sorry)
        // also, you might be able to get the text out of the code, i dont know how exactly but there is a more efficient way of doing this. Ask your teacher for advice.
        AllTasks.Add(new CarTask1(0, "Verwissel een Wiel", Items.Wheel, "Pak de Kruissleutel en de Momentsleutel en interact met het wiel", Items.CrossSocketWrench, Items.TorqueWrench, Items.Wheel));
        AllTasks.Add(new CarTask2(1, "Verwissel de Accu", Items.EngineHood, "Pak de Accu en de Steeksleutel en interact met de motorkap", Items.CarBattery, Items.Wrench));
    } // Don't forget to add new CarTasks to this constructor
    public void SelectTask(int i)
    {
        if (!SelectedTasks.Contains(AllTasks[i]))
        {
            SelectedTasks.Add(AllTasks[i]);
        }
        else
        {
            Debug.Log("Task Already Added");
        }
    } // Select task so it can get saved in the data later on
}
