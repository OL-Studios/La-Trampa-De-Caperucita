using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Jugador;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    void Start()
    {
        UsoCursor(0);
    }

    
    void Update()
    {
        UsoCursor(1);
    }

    public void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
    }    

    void UsoCursor(int estado){
        switch(estado){
            case 0:
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            break;

            case 1:
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                }
            break;
        }
    }
}
