using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;  

public class MenuSeleccionPersonajes : MonoBehaviour
{
    private int index;
    [SerializeField] private Image imagen;
    [SerializeField] private TextMeshProUGUI nombre;
    private GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.Instance;
        index = PlayerPrefs.GetInt("JugadorIndex");
        if (index > gameManager.personajes.Count - 1)
        {
            index = 0;
        }
        CambiarPantalla();
    }
    private void CambiarPantalla()
    {
        PlayerPrefs.SetInt("JugadorIndex", index);
        imagen.sprite = gameManager.personajes[index].imagen;
        nombre.text = gameManager.personajes[index].nombre;
    }

    public void SiguientePersonaje()
    {
        if (index < gameManager.personajes.Count - 1)
        {
            index = 0;
        }
        else
        {
            index += 1;
        }
        CambiarPantalla();
    }

    public void AnteriorPersonaje()
    {
        if (index <= 0)
        {
            index = gameManager.personajes.Count - 1;
        }
        else
        {
            index -= 1;
        }
        CambiarPantalla();
    }
    public void IniciarJuego()
    {
        SceneManager.LoadScene("LauraPruebas");
    }
}
