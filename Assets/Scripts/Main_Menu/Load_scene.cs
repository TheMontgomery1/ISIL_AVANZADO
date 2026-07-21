using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System;

public class Load_scene : MonoBehaviour
{
  
    public void Scene_Load (string Name)
    {
        SceneManager.LoadScene(Name);
    }

    public void Exit()
    {
        Console.WriteLine("saliendo del juego...");
        Application.Quit();
    }

}
