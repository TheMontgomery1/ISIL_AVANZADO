using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    int coinCounter;
    HUD_UI hud_ui;

    public GameObject endGameMenu;
    public bool endGame = false;
    
    public GameObject endGameMetaMenu;
    public bool endMeta = false;


    public GameObject playerHud;
    public bool HUDActive = true;

    // Start is called before the first frame update
    void Start()
    {
        hud_ui = GetComponent<HUD_UI>();
        coinCounter = 0;
        if (!PlayerPrefs.HasKey("score_record"))
        {
            //Crear un nuevo player prefs
            PlayerPrefs.SetInt("score_record", 0);
            PlayerPrefs.Save();
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        hud_ui.SetCoinCounterText(coinCounter);
    }

    public void AddCoin()
    {
        coinCounter +=5;
    }

    public void UpdateLife(float life)
    {
        hud_ui.SetLifeBarSlider(life);
    }

    public void GameOver()
    {
        //Obtener el valor de un PlayerPref
        int score_record = PlayerPrefs.GetInt("score_record");
        print("Fin del juego. Hiciste "+coinCounter+ "monedas.");
        if (score_record < coinCounter)
        {
            PlayerPrefs.SetInt("score_record", coinCounter);
            PlayerPrefs.Save();
            print("Lograste un nuevo record. Ahora tu record es "+coinCounter);
        }

        endGameMenu.SetActive(true);
        endGame = true;

        playerHud.SetActive(false);
        HUDActive = false;
    }

    public void EndDemo()
    {
        endGameMetaMenu.SetActive(true);
        endMeta = true;


        playerHud.SetActive(false);
        HUDActive = false;
    }


}
