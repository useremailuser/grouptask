using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.SceneManagement;


public class EndLevelScript : MonoBehaviour
{
    private AssetBundle myLoadedAssetBundle;
    private string[] scenePaths;
    public TextMeshProUGUI CoinText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetComponentInParent<CoinCounter>();
        myLoadedAssetBundle = AssetBundle.LoadFromFile("Assets/AssetBundles/Scenes");
        scenePaths = myLoadedAssetBundle.GetAllScenePaths();
    }

    void OnTriggerEnter()
    {
        Destroy(gameObject);

        if (GetComponentInParent<CoinCounter>().CoinCount < 18)
        {
            SceneManager.LoadScene(0, LoadSceneMode.Single);
        }
        else if (GetComponentInParent<CoinCounter>().CoinCount >= 18)
        {
            SceneManager.LoadScene(1, LoadSceneMode.Single);
            
        }
    }
}
