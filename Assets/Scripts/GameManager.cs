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

    [Header ("PANELES UI")]
    public GameObject pnlPause;

    [Header ("BANDERAS")]
    public bool enJuego = false;


    [SerializeField]
    private bool estaPausado = false;

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

    void Start()
    {
        UsoCursor(0);
        IniciarJuego(); //SE DEBE BORRAR CUANDO QUEDE HECHA LA TRANSICIÓN
    }

    
    void Update()
    {
        ActivarPausa();
    }

    void ActivarPausa(){
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            estaPausado = !estaPausado;
            Pausa();
        }
    }

    public void Pausa()
    {
        if (estaPausado)
        {
            Time.timeScale = 0;
            pnlPause.SetActive(true);
            UsoCursor(1);
            enJuego = false;
        }
        else
        {
            Time.timeScale = 1;
            pnlPause.SetActive(false);
            UsoCursor(0);
            enJuego = true;
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

    void IniciarJuego(){
        enJuego = true;
        //AQUI PONES TODA LA TRANSICIÓN DE CINEMÁTICA -> INSTRUCCIONES
    }
}
