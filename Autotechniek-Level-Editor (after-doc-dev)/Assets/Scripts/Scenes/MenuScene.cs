using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScene : MonoBehaviour
{
    void Start()
    {
        GameModeManager.SetGamemode(Gamemodes.Menu); // switching gamemode is important since it unlocks/disables certain blocks of code
    }
}
