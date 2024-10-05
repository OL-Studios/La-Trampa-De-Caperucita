using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Jugador;
using Enemigos;
using Assets.Scripts.Interfaz;
using UnityEngine.Video;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header ("UI")]
    public GameObject pnlMenu;
    public GameObject pnlPause;
    public GameObject panelDerrota;  // Panel de Derrota
    public GameObject panelVictoria;  // Panel de Derrota
    public GameObject panelCinemática; 
    //public GameObject hudJugador;
    //public GameObject hudLobo;
    public GameObject cuadroDialogos;
    //public TMP_Text txtEstadoCarga;
    public GameObject imgCarga;
    public Image imgEstadoCarga;
    public TMP_Text txtPorcentajeCarga;
    public VideoPlayer videoCinematica;

    [Header ("BANDERAS")]
    public bool enJuego = false;
    public bool cargaJuego = false;
    [SerializeField]
    private bool estaPausado = false;

    public SistemaDialogos sistemaDialogos;
    public Animator puerta;


    [Header("JUGADORES")]
    public JugadorVida jugador;  
    public LoboJefe loboJefe;    
    [SerializeField]
    private bool terminoJuego = false;


    public void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject); 
        }
    }

    void Update()
    {
        ActivarPausa();
        RevisarSaludJugador();
        RevisarDerrotaJefe();
    }
    public void CargarMenu()
    {
        SceneManager.LoadScene("MenuInicial");
        StartCoroutine(ReiniciarValores());
    }

    private IEnumerator ReiniciarValores()
    {
        Time.timeScale = 1;
        sistemaDialogos.ReiniciarDialogos();
        estaPausado = false;
        pnlMenu.SetActive(true);
        JugadorVida.Instance.saludActual = JugadorVida.Instance.saludMax;
        enJuego = false;
        
        yield return new WaitForSeconds(0.1f);  

        cuadroDialogos.SetActive(false); 
        pnlPause.SetActive(false);
        panelDerrota.SetActive(false);
        panelVictoria.SetActive(false);
    }

    void ActivarPausa(){
        if (Input.GetKeyDown(KeyCode.Escape) && enJuego)
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

    public void IniciarJuego(){
        cargaJuego = false;
        videoCinematica.Pause();
        StartCoroutine(LoadSceneAsync("Main"));
    }

    IEnumerator LoadSceneAsync(string sceneName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);      // Inicia la carga asíncrona de la escena
        while (!operation.isDone)                                               // Mientras la escena se carga, actualiza la barra de progreso
        {
            imgCarga.SetActive(true);
            imgEstadoCarga.fillAmount = operation.progress;                     // Asigna el progreso a la barra de carga
            float progreso = Mathf.Clamp01(operation.progress / 0.9f) * 100f;   // El progreso puede ir de 0 a 1, por lo que multiplicamos por 100 para convertirlo a porcentaje
            txtPorcentajeCarga.text = progreso.ToString("F0") + "%";            // F0 formatea a número entero
        
            yield return null;                                  
        }

        // Estas líneas se ejecutarán cuando la escena esté completamente cargada
        enJuego = true;
        imgCarga.SetActive(false);
        cuadroDialogos.SetActive(true);
        sistemaDialogos.EstadoInstrucciones(0);
        jugador = GameObject.FindWithTag("Player").GetComponent<JugadorVida>();
        loboJefe = GameObject.FindWithTag("Jefe").GetComponent<LoboJefe>();
        puerta = GameObject.FindWithTag("Puerta").GetComponent<Animator>();
        cuadroDialogos.SetActive(true);
        imgCarga.SetActive(false);
        panelCinemática.SetActive(false);
    }


    // Proceso panel derrota y victoria
    void RevisarSaludJugador()
    {
        if(enJuego == true){
            if (jugador.saludActual <= 0)
            {
                terminoJuego = true;
                MostrarPanelDerrota();
            }
        }
    }

    void RevisarDerrotaJefe()
    {
        if(enJuego == true){

            if (loboJefe.saludActualJefe <= 0)
            {
                terminoJuego = true;
                MostrarPanelVictoria();
            }
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
