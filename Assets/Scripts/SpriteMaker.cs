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
        byte[] textureTmp = cloth.GetRawTextureData();

        var height = skin.height;
        var width = skin.width;

        Texture2D skinTex = new Texture2D(width, height, TextureFormat.RGBA32, false);
        Texture2D clothTex = new Texture2D(width, height, TextureFormat.RGBA32, false);
        Texture2D mixTex = new Texture2D(width, height, TextureFormat.RGBA32, false);
        clothTex.LoadRawTextureData(textureTmp);
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
    public Sprite AverageSprite(Sprite sprite, FilterMode mode)
    {
        var width = sprite.texture.width;
        var height = sprite.texture.height;
        Texture2D texture = new Texture2D(width, height, TextureFormat.RGBA32, false);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                var closedPixels = new List<Color>();
                closedPixels.Add(sprite.texture.GetPixel(x, y));

                if (x > 0) closedPixels.Add(sprite.texture.GetPixel(x - 1, y));
                if (x < width - 1) closedPixels.Add(sprite.texture.GetPixel(x - 1, y));
                if (y > 0) closedPixels.Add(sprite.texture.GetPixel(x, y - 1));
                if (y < height - 1) closedPixels.Add(sprite.texture.GetPixel(x, y + 1));

                var rValue = 0f;
                var gValue = 0f;
                var bValue = 0f;
                var aValue = 0f;

                foreach (Color color in closedPixels)
                {
                    rValue += color.r;
                    gValue += color.g;
                    bValue += color.b;
                    aValue += color.a;

                }
                var r = rValue / closedPixels.Count;
                var g = gValue / closedPixels.Count;
                var b = bValue / closedPixels.Count;
                var a = aValue / closedPixels.Count;

                texture.SetPixel(x, y, new Color(r, g, b, a));
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
    public Sprite AvegerageSpriteColorStepByStep(Texture2D texture, int x, int y, FilterMode mode)
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

    public Sprite ColorSprite(Sprite sprite, Color color, float intensity, FilterMode mode)
    {
        var importedTexture = sprite.texture;
        var width = importedTexture.width;
        var height = importedTexture.height;

        byte[] textureTmp = importedTexture.GetRawTextureData();


        Texture2D oldTexture = new Texture2D(width, height, TextureFormat.RGBA32, false);
        Texture2D newTexture = new Texture2D(width, height, TextureFormat.RGBA32, false);

        oldTexture.LoadRawTextureData(textureTmp);

        for (int x = 0; x < newTexture.width; x++)
        {
            for (int y = 0; y < newTexture.height; y++)
            {
                var currColor = oldTexture.GetPixel(x, y);
                var outputColor = (currColor * (1f-intensity) + color * intensity);
                newTexture.SetPixel(x, y, outputColor);

            }
        }

        newTexture.filterMode = mode;
        newTexture.Apply();

        Sprite newSprite = Sprite.Create(newTexture, new Rect(0, 0, newTexture.width, newTexture.height), Vector2.one * 0.5f);

        return newSprite;

    }

    public Sprite ColorSaturateSprite(Sprite sprite, Color color, FilterMode mode)
    {
        var importedTexture = sprite.texture;
        var width = importedTexture.width;
        var height = importedTexture.height;

        byte[] textureTmp = importedTexture.GetRawTextureData();


        Texture2D oldTexture = new Texture2D(width, height, TextureFormat.RGBA32, false);
        Texture2D newTexture = new Texture2D(width, height, TextureFormat.RGBA32, false);

        oldTexture.LoadRawTextureData(textureTmp);

        for (int x = 0; x < newTexture.width; x++)
        {
            for (int y = 0; y < newTexture.height; y++)
            {
                var currColor = oldTexture.GetPixel(x, y);
                if (currColor.a == 1f)
                {
                    var h = 0f;
                    var s = 0f;
                    var v = 0f;
                    Color.RGBToHSV(currColor, out h, out s, out v);
                    var outputColor = (currColor * (1f - s) + color * s);
                    newTexture.SetPixel(x, y, outputColor);
                }
                else newTexture.SetPixel(x, y, new Color(0f, 0f, 0f ,0f));

            }
        }

        newTexture.filterMode = mode;
        newTexture.Apply();

        Sprite newSprite = Sprite.Create(newTexture, new Rect(0, 0, newTexture.width, newTexture.height), Vector2.one * 0.5f);

        return newSprite;

    }
}
