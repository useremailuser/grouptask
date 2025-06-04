using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using System;


public class AbilityCD : MonoBehaviour
{
    public TextMeshProUGUI displayText;

    public int charges = 3;
    public float targetTime = 3.0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKeyDown("space"))
        {
            if(charges > 0)
            {
                charges -= 1;
                displayText.text = charges.ToString();

                Debug.Log("space");
            }

            if (charges < 3)
            {
                targetTime -= Time.deltaTime;


            }




        }

        if (targetTime <= 0.0f)
        {

            AddCD();
            Debug.Log("addCD");
        }
    }
    void AddCD()
    { 
       charges += 1;
        Debug.Log("CDadded");
        targetTime = 3.0f;
        displayText.text = charges.ToString();

    }
}
