using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
    public static GameState Instance;

    public bool hasExitKey = false;

    void Awake()
    {
        Instance = this;
    }
}
