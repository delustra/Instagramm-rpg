using System.Collections;
using MoreMobs.Model;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using MoreMobs.MobQ;

public class ImageLoader : MonoBehaviour
{

    public MobQueue MobQ;

    public void DownloadTexture(string inpack_id, string imglink)

    {
        StartCoroutine(GetTexture1(inpack_id, imglink));
    }


    private IEnumerator GetTexture1(string inpack_id, string url)

    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Texture2D texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            Rect rec = new Rect(0, 0, texture.width, texture.height);
            Sprite spriteToUse = Sprite.Create(texture, rec, new Vector2(0.5f, 0.5f), 100);

            MobQ.MobSprites.Add(
                    new MobSprite {
                        inpack_id = inpack_id,
                        Sprite = spriteToUse
                    });
            
           Debug.Log("Downloaded image.");

            www.Dispose();
            www = null;
        }

    }
}
