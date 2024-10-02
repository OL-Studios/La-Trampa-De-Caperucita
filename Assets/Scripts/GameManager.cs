using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Jugador;
using Enemigos;
using DotLiquid.Util;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header ("PANELES UI")]
    public GameObject pnlPause;
    public GameObject panelDerrota;  // Panel de Derrota
    public GameObject panelVictoria; // Panel de Victoria

    [Header ("BANDERAS")]
    public bool enJuego = false;
    [SerializeField]
    private bool estaPausado = false;

    [Header("JUGADORES")]
    public JugadorVida jugador;  // Referencia al script JugadorVida
    public LoboJefe loboJefe;    // Referencia al script del LoboJefe
    [SerializeField]
    private bool terminoJuego = false;


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
        //IniciarJuego(); //SE DEBE BORRAR CUANDO QUEDE HECHA LA TRANSICIÓN
    }

    public void CargarMenu()
    {
        Debug.Log("Menu button pressed");
        SceneManager.LoadScene("MenuInicial");
    }

    void Update()
    {
        ActivarPausa();
        RevisarSaludJugador();
        RevisarDerrotaJefe();

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
        if (estaPausado && !terminoJuego)
        {
            Time.timeScale = 0;
            pnlPause.SetActive(true);
            UsoCursor(1);
            enJuego = false;
        }
        else //if (!estaPausado && !terminoJuego)
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
                if (Input.GetKeyDown(KeyCode.Escape)|| terminoJuego)
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




    // Proceso panel derrota y victoria
    void RevisarSaludJugador()
    {
        if (jugador.saludActual <= 0)
        {
            terminoJuego = true;
            MostrarPanelDerrota();
        }
    }

    void RevisarDerrotaJefe()
    {
        if (loboJefe.saludActualJefe <= 0)
        {
            terminoJuego = true;
            MostrarPanelVictoria();
        }
    }

    void MostrarPanelDerrota()
    {
        // Otras acciones como pausar el juego
        if (terminoJuego)
        {
            Time.timeScale = 0;
            panelDerrota.SetActive(true);
            UsoCursor(1);
            enJuego = false;
        }
    }

    void MostrarPanelVictoria()
    {
 
        if (terminoJuego)
        {
            Time.timeScale = 0;
            panelVictoria.SetActive(true);
            UsoCursor(1);
            enJuego = false;
        }
    }
}
