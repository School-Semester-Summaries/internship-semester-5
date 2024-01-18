using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayScene : MonoBehaviour
{
    public GameObject MainCamera;
    public GameObject Minigame1Camera; // you can make a CarTaskCamera that gets the camera values from the corresponding cartask, instead of making Minigame1Camera, Minigame2Camera, Minigame3Camera etc.

    void Start()
    {
        GameModeManager.SetGamemode(Gamemodes.Play); // setting a gamemode prevents a lot of accidental stuff not to happen, lots of times we first check the current gamemode before we let something happen
    }
    public void SwitchToPlayCam()
    {
        MainCamera.transform.position = new Vector3(0, 0, -10);
        GameObject Inventory = GameObject.FindGameObjectWithTag("Inventory");
        Inventory.transform.position = new Vector3(835, 0, 2);
    } // the play section camera
}
