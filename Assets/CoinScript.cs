using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using System;


public class CoinScript : MonoBehaviour
{


 public TextMeshProUGUI CoinText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetComponentInParent<CoinCounter>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter()
    {
        GetComponentInParent<CoinCounter>().CoinCount += 1;
      CoinText.text = GetComponentInParent<CoinCounter>().CoinCount.ToString();
      Destroy(gameObject);


    }
}
