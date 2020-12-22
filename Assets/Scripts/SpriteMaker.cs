using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteMaker : MonoBehaviour
{
    SpriteRenderer rend;
    private void Awake()
    {
        rend = GetComponent<SpriteRenderer>();
    }

    public Texture2D OverwriteSprite(Sprite overwrotten, Sprite overwriter)
    {
        var cloth = overwriter.texture;
        var skin = overwriter.texture;

        byte[] skinTmp = skin.GetRawTextureData();
        byte[] clothTmp = cloth.GetRawTextureData();

        var height = skin.height;
        var width = skin.width;

        Texture2D skinTex = new Texture2D(width, height, TextureFormat.RGBA32, false);
        Texture2D clothTex = new Texture2D(width, height, TextureFormat.RGBA32, false);
        Texture2D mixTex = new Texture2D(width, height, TextureFormat.RGBA32, false);
        clothTex.LoadRawTextureData(clothTmp);
        skinTex.LoadRawTextureData(skinTmp);

        for (int x = 0; x < mixTex.width; x++)
        {
            for (int y = 0; y < mixTex.height; y++)
            {
                mixTex.SetPixel(x, y, skinTex.GetPixel(x, y));
                if (clothTex.GetPixel(x, y).a >= 0.1f) mixTex.SetPixel(x, y, clothTex.GetPixel(x, y));
            }
        }
        mixTex.Apply();

        return mixTex;

    }


    public Sprite CreateSprite(int height, int width, FilterMode mode)
    {
        Texture2D texture = new Texture2D(width, height, TextureFormat.RGBA32, false);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y <height; y++)
            {
                var r = Random.Range(0f, 1f);
                var g = Random.Range(0f, 1f);
                var b = Random.Range(0f, 1f);
                var a = Random.Range(1f, 1f);

                texture.SetPixel(x, y, new Color(r,g,b,a));
            }
        }
        texture.filterMode = mode;
        texture.Apply();

        return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), Mathf.Max(texture.width, texture.height));

    }
    public Sprite CreateWhiteSprite(int height, int width, FilterMode mode)
    {
        Texture2D texture = new Texture2D(width, height, TextureFormat.RGBA32, false);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                texture.SetPixel(x, y, new Color(1f, 1f, 1f, 1f));
            }
        }
        texture.filterMode = mode;
        texture.Apply();
        return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), Mathf.Max(texture.width, texture.height));
    }

    public Sprite CreateSpriteStepByStep(Texture2D texture, int x, int y, FilterMode mode)
    {
        var r = Random.Range(0f, 1f);
        var g = Random.Range(0f, 1f);
        var b = Random.Range(0f, 1f);
        var a = Random.Range(1f, 1f);

        texture.SetPixel(x, y, new Color(r, g, b, a));
        texture.filterMode = mode;
        texture.Apply();
        return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), Mathf.Max(texture.width, texture.height));
    }
    public Texture2D CreateTexture(int height, int width, FilterMode mode)
    {
        Texture2D texture = new Texture2D(width, height, TextureFormat.RGBA32, false);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                var r = Random.Range(0f, 1f);
                var g = Random.Range(0f, 1f);
                var b = Random.Range(0f, 1f);
                var a = Random.Range(1f, 1f);

                texture.SetPixel(x, y, new Color(r, g, b, a));
            }
        }
        texture.filterMode = mode;
        texture.Apply();

        return texture;

    }
    public Texture2D CreateTextureStepByStep(Texture2D texture, int x, int y, FilterMode mode)
    {
        var r = Random.Range(0f, 1f);
        var g = Random.Range(0f, 1f);
        var b = Random.Range(0f, 1f);
        var a = Random.Range(1f, 1f);

        texture.SetPixel(x, y, new Color(r, g, b, a));
        texture.filterMode = mode;
        texture.Apply();
        return texture;
    }


    public Sprite ColorSprite(Sprite sprite, Color color)
    {
        var cloth = sprite.texture;

        byte[] clothTmp = cloth.GetRawTextureData();

        var height = cloth.height;
        var width = cloth.width;

        Texture2D clothTex = new Texture2D(width, height, TextureFormat.RGBA32, false);
        Texture2D colorTex = new Texture2D(width, height, TextureFormat.RGBA32, false);

        clothTex.LoadRawTextureData(clothTmp);

        for (int x = 0; x < clothTex.width; x++)
        {
            for (int y = 0; y < clothTex.height; y++)
            {
                if (clothTex.GetPixel(x, y).a >= 0.1f) colorTex.SetPixel(x, y, color);
                else colorTex.SetPixel(x, y, Color.clear);
            }
        }

        colorTex.Apply();

        Sprite newSprite = Sprite.Create(colorTex, new Rect(0, 0, colorTex.width, colorTex.height), Vector2.one * 0.5f);

        return newSprite;

    }

    public void SetRend(Sprite sprite)
    {
        rend.sprite = sprite;
    }
}
