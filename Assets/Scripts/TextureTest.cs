using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine.Windows;
using HSVPicker;

public class TextureTest : MonoBehaviour
{
    SpriteMaker spriteMaker;
    TextureMaker textureMaker;
    SpriteRenderer rend;
    [SerializeField] int height;
    [SerializeField] float colorIntensity;
    [SerializeField] int width;
    [SerializeField] bool create;
    [SerializeField] bool module;
    [SerializeField] Button butSave;
    [SerializeField] Button butColor;
    [SerializeField] Button butCreate;
    [SerializeField] Button butModule;
    [SerializeField] Button butAverage;
    [SerializeField] Button butColorSaturation;
    [SerializeField] InputField exportName;
    [SerializeField] InputField heightTxt;
    [SerializeField] InputField widthTxt;
    [SerializeField] Dropdown filterModeDropdown;
    [SerializeField] Dropdown modeDropdown;
    [SerializeField] Scrollbar speedScrollbar;
    [SerializeField] Scrollbar colorIntensityScrollBar;
    [SerializeField] Text intensityValue;
    int currX = 0;
    int currY = 0;
    [SerializeField] FilterMode filterMode = FilterMode.Point;
    [SerializeField] Mode mode = Mode.Sprite;
    [SerializeField] float tick = 0.1f;
    [SerializeField] ColorPicker picker;
    [SerializeField] Color pickedColor;


    public enum Mode{Sprite,Texture}
    private void Start()
    {
        Time.fixedDeltaTime = tick;
        spriteMaker = GetComponent<SpriteMaker>();
        textureMaker = GetComponent<TextureMaker>();
        rend = GetComponent<SpriteRenderer>();
        butModule.onClick.AddListener(delegate { ModulateOnClick(mode); });
        butColor.onClick.AddListener(delegate { ColorSprite(); });
        butColorSaturation.onClick.AddListener(delegate { ColorSaturateSprite(); });
        butSave.onClick.AddListener(delegate { Save(rend.sprite.texture); });
        butCreate.onClick.AddListener(delegate { CreateOnClick(mode); });
        butAverage.onClick.AddListener(delegate { AverageOnClick(mode); });
        filterModeDropdown.onValueChanged.AddListener(delegate { SetFilterMode(filterModeDropdown.value); });
        modeDropdown.onValueChanged.AddListener(delegate { SetMode(modeDropdown.value); });
        speedScrollbar.onValueChanged.AddListener(delegate { SetSpeed(speedScrollbar.value); });
        colorIntensityScrollBar.onValueChanged.AddListener(delegate { SetIntensity(colorIntensityScrollBar.value); });
        heightTxt.onEndEdit.AddListener(delegate { SetHeight(int.Parse(heightTxt.text)); });
        widthTxt.onEndEdit.AddListener(delegate { SetWidth(int.Parse(widthTxt.text)); });
        picker.onValueChanged.AddListener(color =>{ pickedColor = color; });
    }


    private void CreateOnClick(Mode mode)
    {
        if (mode == Mode.Sprite) CreateSprite();
        else if (mode == Mode.Texture) CreateTexture();
    }
    private void ModulateOnClick(Mode mode)
    {
        module = !module;
        if (!module) { currX = 0; currY = 0; }
    }
    private void AverageOnClick(Mode mode)
    {
        if (mode == Mode.Sprite) rend.sprite = spriteMaker.AverageSprite(rend.sprite, filterMode);
        else if (mode == Mode.Texture)
        {
            MaterialPropertyBlock block = new MaterialPropertyBlock();
            block.SetTexture("_MainTex", textureMaker.AverageTexture(rend.sprite.texture, filterMode));
            rend.SetPropertyBlock(block);
        }
    }
    private void CreateSprite()
    {
        rend.size = new Vector2(width, width);
        rend.sprite = spriteMaker.CreateSprite(height, width, filterMode);
    }
    private void ColorSprite()
    {
        rend.sprite = spriteMaker.ColorSprite(rend.sprite, pickedColor, colorIntensity, filterMode);
    }
    private void ColorSaturateSprite()
    {
        rend.sprite = spriteMaker.ColorSaturateSprite(rend.sprite, pickedColor, filterMode);
    }
    private void CreateTexture()
    {
        MaterialPropertyBlock block = new MaterialPropertyBlock();
        block.SetTexture("_MainTex", textureMaker.CreateTexture(height, width, filterMode));
        rend.SetPropertyBlock(block);   
    }
    private void SetHeight(int height)
    {
        this.height = height;
    }
    private void SetWidth(int width)
    {
        this.width = width;
    }
    private void SetIntensity(float intensity)
    {
        this.colorIntensity = intensity;
        intensityValue.text = Mathf.CeilToInt(intensity * 100f).ToString();
    }
    private void SetSpeed(float speed)
    {
        Time.fixedDeltaTime = 0.0001f * 1/speed;
    }
    private void SetFilterMode(int value)
    {
        if (value == 0) filterMode = FilterMode.Point;
        else if (value == 1) filterMode = FilterMode.Bilinear;
        else filterMode = FilterMode.Trilinear;
    }
    private void SetMode(int value)
    {
        if (value == 0) mode = Mode.Sprite;
        else if (value == 1) mode = Mode.Texture;
        else  mode = Mode.Sprite;
    }
    private void FixedUpdate()
    {
        if ((currX == width && currY == height) || !module) return;
        if (mode == Mode.Sprite) rend.sprite = spriteMaker.CreateSpriteStepByStep(rend.sprite.texture, currX, currY, filterMode);
        else if (mode == Mode.Texture)
        {
            MaterialPropertyBlock block = new MaterialPropertyBlock();
            rend.GetPropertyBlock(block);
            block.SetTexture("_MainTex", textureMaker.CreateTextureStepByStep((Texture2D) block.GetTexture("_MainTex"), currX, currY, filterMode));
            rend.SetPropertyBlock(block);
        }
        if (currY == height) { currY = 0; currX = 0; }
        else if (currX == width) { currY++; currX = 0; }
        else currX++;
    }
    void Save(Texture2D texture)
    {
        var bytes = texture.EncodeToPNG();
        File.WriteAllBytes(EditorUtility.SaveFilePanel("Save PNG", Application.dataPath + "/../", exportName.text, "png"), bytes);
    }

}
