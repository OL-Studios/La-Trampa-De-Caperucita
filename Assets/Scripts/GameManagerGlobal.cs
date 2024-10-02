using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerGlobal : MonoBehaviour
{
    //public static GameManagerGlobal Instance { get; private set; }

    [Header("PANELES UI")]
    public GameObject canvasCreditos; // Canvas de créditos
    public GameObject canvasCinematica; // Canvas de créditos
    public GameObject canvasMenu;

    public void CargarCinamatica()
    {
        //SceneManager.LoadScene("IntroScene");
        canvasCinematica.SetActive(true);
        canvasMenu.SetActive(false);
    }

    public void Quit()
    {
        #if UNITY_STANDALONE
            Application.Quit();
        #endif
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }

    public void MostrarCreditos()
    {
        if (canvasCreditos != null)
        {
            canvasCreditos.SetActive(true);
            canvasMenu.SetActive(false);
        }
    }

    public void OcultarCreditos()
    {
        if (canvasCreditos != null)
        {
            canvasCreditos.SetActive(false);
            canvasMenu.SetActive(true);
        }
    }
}

