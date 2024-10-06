
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class VideoaEscena : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    private bool isEventAttached = false; // Control para el evento

    void Start()
    {
        // Reasignar la cámara principal en cada inicio de escena
        if (videoPlayer.targetCamera == null || videoPlayer.targetCamera != Camera.main)
        {
            videoPlayer.targetCamera = Camera.main;
        }

        // Si estamos en la escena "MenuInicial", intentar asignar la cámara
        if (SceneManager.GetActiveScene().name == "MenuInicial")
        {
            AsignarMainCamera();
        }

        // Vincular el evento si no lo hemos hecho aún
        if (!isEventAttached)
        {
            videoPlayer.loopPointReached += OnVideoEnd; // Vincula el evento al final del video
            isEventAttached = true;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            CargarSiguienteEscena();
        }
        // Solo buscar la cámara si estamos en la escena "MenuInicial" y aún no está asignada
        if (SceneManager.GetActiveScene().name == "MenuInicial" && videoPlayer.targetCamera == null)
        {
            AsignarMainCamera();
        }
    }
    void AsignarMainCamera()
    {
        if (Camera.main != null)
        {
            videoPlayer.targetCamera = Camera.main;
        }
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        CargarSiguienteEscena();
    }

    void CargarSiguienteEscena()
    {
        GameManager.Instance.cargaJuego = true;
        SceneManager.LoadScene("Main");
        GameManager.Instance.IniciarJuego();
    }

    private void OnDestroy()
    {
        // Desvincular el evento para evitar problemas
        if (isEventAttached)
        {
            videoPlayer.loopPointReached -= OnVideoEnd;
            isEventAttached = false; // Restablecer el control
        }
    }
}
