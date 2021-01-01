using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureMaker : MonoBehaviour
{
    SpriteRenderer rend;
    private void Awake()
    {
        rend = GetComponent<SpriteRenderer>();
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
    public Texture2D AverageTexture(Texture2D texture, FilterMode mode)
    {
        var width = texture.width;
        var height = texture.height;
        Texture2D newTexture = new Texture2D(width, height, TextureFormat.RGBA32, false);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                var closedPixels = new List<Color>();
                closedPixels.Add(texture.GetPixel(x, y));

                if (x > 0) closedPixels.Add(texture.GetPixel(x - 1, y));
                if (x < width - 1) closedPixels.Add(texture.GetPixel(x - 1, y));
                if (y > 0) closedPixels.Add(texture.GetPixel(x, y - 1));
                if (y < height - 1) closedPixels.Add(texture.GetPixel(x, y + 1));

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

                newTexture.SetPixel(x, y, new Color(r, g, b, a));
            }
        }
        newTexture.filterMode = mode;
        newTexture.Apply();

        return texture;

    }
}
