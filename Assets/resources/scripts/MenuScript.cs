using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
   public void Jouer()
    {
        SceneManager.LoadScene("level");
    }
    public void Quitter()
    {
        Debug.Log("you quit");
        Application.Quit();
    }
}
