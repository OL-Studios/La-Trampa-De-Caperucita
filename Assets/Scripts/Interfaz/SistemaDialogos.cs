using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Assets.Scripts.Interfaz
{
    public class SistemaDialogos : MonoBehaviour
    {   
        public string[] frases = {
            "¡Hola, valiente! Soy Caperucita y necesito tu ayuda para enfrentar los peligros del bosque. ¡Juntos podemos derrotar a los lobos y salvar a mi abuela! Confío en ti, sigue mis pasos y usa tus habilidades para superar cada reto. ¡Buena suerte!",
            "Moverse: Usa WASD o las flechas para caminar y girar.",
            "Saltar: Presiona la barra espaciadora para saltar.",
            "Rodar: Presiona Alt izquierdo para hacer una voltereta.",
            "Recolectar manzanas: Usa la tecla E para recoger manzanas.",
            "Activar manzana: Usa el scroll adelante para equipar una manzana.",
            "Desactivar manzana: Usa el scroll atrás para guardar la manzana.",
            "Lanzar manzana: Presiona el click derecho para lanzarla.",
            "Atacar: Usa el click izquierdo para atacar con la espada.",
            "¡Has completado el tutorial! Ahora estás listo para adentrarte en el bosque y enfrentar a los lobos. ¡Buena suerte, y no olvides, la astucia siempre será tu mejor aliada!"
        };
        
        public TMP_Text texto;
        
        private int indiceFraseActual = 0;

        void Start()
        {
            StartCoroutine(MostrarFrase(frases[indiceFraseActual]));
        }

        public void MostrarSiguienteFrase()
        {
            // Cambiar al siguiente diálogo solo si hay más frases
            if (indiceFraseActual < frases.Length - 1)
            {
                indiceFraseActual++;
                texto.text = "";                        // Limpiar el texto antes de mostrar la siguiente frase
                StopAllCoroutines();                    // Detener cualquier animación de texto anterior
                StartCoroutine(MostrarFrase(frases[indiceFraseActual]));
            }
        }

        IEnumerator MostrarFrase(string frase)
        {
            foreach (char caracter in frase)
            {
                texto.text += caracter;
                yield return new WaitForSeconds(0.05f);
            }
        }
    }
}