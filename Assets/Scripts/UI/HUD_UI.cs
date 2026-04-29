using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD_UI : MonoBehaviour
{
    public Text txtCoinCounter;
    public Slider sldLifeBar;
    // Start is called before the first frame update
    void Start()
    {
        SetCoinCounterText(5);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetCoinCounterText(int coins)
    {
        txtCoinCounter.text = coins.ToString("00");
    }

    public void SetLifeBarSlider(float life)
    {
        sldLifeBar.value = life/100;
    }
}
