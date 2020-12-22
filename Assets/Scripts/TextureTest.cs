using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TextureTest : MonoBehaviour
{
    SpriteMaker maker;
    SpriteRenderer rend;
    [SerializeField] int height;
    [SerializeField] int width;
    [SerializeField] bool create;
    [SerializeField] bool module;
    [SerializeField] Button butCreate;
    [SerializeField] Button butModule;
    [SerializeField] InputField heightTxt;
    [SerializeField] InputField widthTxt;
    [SerializeField] Dropdown modeDropdown;
    [SerializeField] Scrollbar speedScrollbar;
    int currX = 0;
    int currY = 0;
    [SerializeField] FilterMode filterMode = FilterMode.Point;
    [SerializeField] float tick = 0.1f;

    private void Start()
    {
        Time.fixedDeltaTime = tick;
        maker = GetComponent<SpriteMaker>();
        rend = GetComponent<SpriteRenderer>();
        rend.size = new Vector2(height, height);
        if (!create) rend.sprite = maker.CreateWhiteSprite(height, width, filterMode);
        else rend.sprite = maker.CreateSprite(height, width, filterMode);
        butModule.onClick.AddListener(delegate () { ModulateOnClick(); });
        butCreate.onClick.AddListener(delegate () { CreateOnClick(); });
        modeDropdown.onValueChanged.AddListener(delegate { SetMode(modeDropdown.value); });
        speedScrollbar.onValueChanged.AddListener(delegate { SetSpeed(speedScrollbar.value); });
        heightTxt.onEndEdit.AddListener(delegate { SetHeight(int.Parse(heightTxt.text)); });
        heightTxt.onEndEdit.AddListener(delegate { SetWidth(int.Parse(widthTxt.text)); });
    }


    private void CreateOnClick()
    {
        rend.size = new Vector2(height, height);
        rend.sprite = maker.CreateSprite(height, width, filterMode);
    }
    private void ModulateOnClick()
    {
        module = !module;
    }
    private void SetHeight(int height)
    {
        this.height = height;
    }
    private void SetWidth(int width)
    {
        this.width = width;
    }
    private void SetSpeed(float speed)
    {
        Time.fixedDeltaTime = 0.0001f * 1/speed;
    }
    private void SetMode(int value)
    {
        if (value == 0) filterMode = FilterMode.Point;
        else if (value == 1) filterMode = FilterMode.Bilinear;
        else filterMode = FilterMode.Trilinear;
    }
    private void FixedUpdate()
    {
        if ((currX == width && currY == height) || !module) return;
        rend.sprite = maker.CreateSpriteStepByStep(rend.sprite.texture, currX, currY, filterMode);
        if (currY == height) { currY = 0; currX = 0; }
        else if (currX == width) { currY++; currX = 0; }
        else currX++;
    }

}
