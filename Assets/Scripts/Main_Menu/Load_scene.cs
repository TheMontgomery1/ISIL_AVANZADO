using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class Load_scene : MonoBehaviour
{
  
    public void Scene_Load (string Name)
    {
        SceneManager.LoadScene(Name);
    }

}
