using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.SceneManagement;


public class EndLevelScript : MonoBehaviour
{
   // private AssetBundle myLoadedAssetBundle;
    private string[] scenePaths;
    public TextMeshProUGUI CoinText;
    public GameObject winScreen;

    CoinCounter coinCounter;

    //static int number = 0;


    //void Update()
    //{
    //    EndLevelScript.number++;
    // }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //SceneManager.LoadScene(1, LoadSceneMode.Single);
        //Debug.LogWarning("NUMBER " + EndLevelScript.number);
        //EndLevelScript.number = 0;
        winScreen.SetActive(false);
        coinCounter = FindFirstObjectByType<CoinCounter>();
       // myLoadedAssetBundle = AssetBundle.LoadFromFile("Assets/AssetBundles/Scenes");
       // scenePaths = myLoadedAssetBundle.GetAllScenePaths();
        //winScreen.SetActive(false);

    }

    void OnTriggerEnter()
    {
        Destroy(gameObject);

        if (coinCounter.CoinCount < 18)
        {
            SceneManager.LoadScene(2, LoadSceneMode.Single);
        }
        else if (coinCounter.CoinCount >= 18)
        {
            SceneManager.LoadScene(3, LoadSceneMode.Single);
            
        }
        if (coinCounter.CoinCount >= 25)
        {
            Time.timeScale = 0;
            winScreen.SetActive(true);
        }
        
    }

}
