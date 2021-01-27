using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public static class LoadAssetBundle
{
    //open these for future purposes(for another levels, for exmpl.)
    public static string manifestURL = "https://drive.google.com/uc?export=download&id=1LmGKAk6xwuA0GHQS8C7ODQpPjwnj-ynu";
    public static string bundleURL = "https://drive.google.com/uc?export=download&id=1AKvG42ETGUatIw6O_sbyhM5hWSwYkA06";
    public static bool running = false;
    public static IEnumerator DownloadAndCache()
    {
        while (!Caching.ready)
            yield return null;

        running = true;

        //checking version
        var request = UnityWebRequest.Get(manifestURL);
        yield return request.SendWebRequest();
        running = false;

        Hash128 hash = default;

        var hashRow = request.downloadHandler.text.ToString().Split("\n".ToCharArray())[5];
        hash = Hash128.Parse(hashRow.Split(':')[1].Trim());

        if (hash.isValid == true)
        {
            request.Dispose();
            //checking hash version and download new if needed
            request = UnityWebRequestAssetBundle.GetAssetBundle(bundleURL, hash, 0);

            yield return request.SendWebRequest();
            running = false;

            if (request.result != UnityWebRequest.Result.ProtocolError && request.result != UnityWebRequest.Result.ConnectionError)
            {
                Settings.assetBundle = DownloadHandlerAssetBundle.GetContent(request);
                request.Dispose();
                yield return null;
                running = false;
            }
            else
            {
                yield return null;
                running = false;
            }
        }
        else
        {
            yield return null;
            running = false;
        }
    }
}
