using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadAssetBundle : MonoBehaviour
{
    [SerializeField]
    RuntimeAnimatorController controller;

    string bundleURL = "https://drive.google.com/uc?export=download&id=1AKvG42ETGUatIw6O_sbyhM5hWSwYkA06";
    int version = 12;

    GameObject temp;
    UnityAction action;

    public void ButtonClick()
    {
        Settings.CurrentLevel = "MainMenu";
        SceneManager.LoadScene("LoadingScreen");
    }

    IEnumerator DownloadAndCache()
    {
        while (!Caching.ready)
            yield return null;

        var www = WWW.LoadFromCacheOrDownload(bundleURL, version);
        yield return www;

        if (!string.IsNullOrEmpty(www.error))
        {
            Debug.Log(www.error);
            yield break;
        }
        Debug.Log("Bundle Downloaded!!!");
        var assetbundle = www.assetBundle;

        var manpf = assetbundle.LoadAssetAsync("ManPf", typeof(GameObject));
        yield return manpf;

        var terrain = assetbundle.LoadAssetAsync("Terrain", typeof(GameObject));
        yield return terrain;

        var coin = assetbundle.LoadAssetAsync("CoinPF", typeof(GameObject));
        yield return coin;

        var AllRoutes = assetbundle.LoadAssetAsync("AllRoutes", typeof(GameObject));
        yield return AllRoutes;

        var canvas = assetbundle.LoadAssetAsync("Scene2_UI", typeof(GameObject));
        yield return canvas;

        //UI
        temp = Instantiate(canvas.asset as GameObject, new Vector3(0, 0, 0), Quaternion.identity);
        //Non of these worls  -_-"
        //Method 1
        /*
        temp.transform.GetChild(1).gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
        action += temp.GetComponent<AudioSource>().Play;
        action += gameObject.GetComponent<Scene2>().ButtonMenu;
        temp.transform.GetChild(1).gameObject.GetComponent<Button>().onClick.AddListener(action);
        */
        //method 2
        /*
        temp.transform.GetChild(1).gameObject.GetComponent<Button>().onClick.AddListener(gameObject.GetComponent<Scene2>().ButtonMenu);
        temp.transform.GetChild(1).gameObject.GetComponent<Button>().onClick.AddListener(temp.GetComponent<AudioSource>().Play);
        */

        //coin
        GameObject CoinGO = coin.asset as GameObject;
        CoinGO.AddComponent<Rotation>().y = 0.5f;

        //terrain
        temp = Instantiate(terrain.asset as GameObject, new Vector3(0, 0, 0), Quaternion.identity);
        temp.AddComponent<CoinSpawner>().Coin = CoinGO;
        temp.isStatic = true;

        //routes
        GameObject Routes = Instantiate(AllRoutes.asset as GameObject, new Vector3(0, 0, 0), Quaternion.identity);

        //manPF
        temp = Instantiate(manpf.asset as GameObject, new Vector3(0, 0, 0), Quaternion.identity);
        //shuld change scale in assetbundle prefab, but, lazy me)
        temp.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);

        temp.tag = "Player";
        temp.AddComponent<BezierFollow>().speed = 0.3f;
        temp.GetComponent<BezierFollow>().routes.Initialize();
        temp.GetComponent<BezierFollow>().routes[0] = Routes.transform.GetChild(0).transform;
        temp.GetComponent<BezierFollow>().routes[1] = Routes.transform.GetChild(1).transform;
        temp.GetComponent<BezierFollow>().routes[2] = Routes.transform.GetChild(2).transform;

        temp.GetComponent<Animator>().runtimeAnimatorController = controller;
    }
}
