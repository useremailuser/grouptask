using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using System;


public class CoinScript : MonoBehaviour
{

 public float CoinCount = 0;
 public TextMeshProUGUI CoinText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter()
    {
      CoinCount += 1;
      CoinText.text = CoinCount.ToString();
      Destroy(gameObject);


    }
}
