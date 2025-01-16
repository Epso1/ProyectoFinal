using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Personajes", menuName = "Personajes")]
public class Personajes : ScriptableObject
{
    public GameObject personajeJugable;
    public Sprite imagen;
    public string nombre;
}
