
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class VideoaEscena : MonoBehaviour
{
    public VideoPlayer videoPlayer; // Referencia al Video Player
    private bool isEventAttached = false; // Control para el evento

    void Start()
    {
        // Vincular el evento solo si no está ya vinculado
        if (!isEventAttached)
        {
            videoPlayer.loopPointReached += OnVideoEnd;
            isEventAttached = true; // Marcar el evento como vinculado
        }
    }

    void Update()
    {
        // Verifica si se presiona la tecla Escape
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CargarSiguienteEscena();
        }
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        CargarSiguienteEscena();
    }

    void CargarSiguienteEscena()
    {
        // Puedes cambiar "Main" por el nombre real de tu escena
        SceneManager.LoadScene("Main");
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
