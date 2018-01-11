using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using Assets.Scripts.GameLogic;
using Assets.Scripts.Vuforia;
using Assets.Vuforia.Scripts;
using GameLogic;
using UnityEngine.UI;
using Vuforia;
using Image = UnityEngine.UI.Image;
using Random = UnityEngine.Random;

public class Quickspawn : MonoBehaviour
{

    public Transform Tile;
    public Transform Layout;
    public Transform MemLayout;

    public GameObject Target;
    public static GameObject[] mTiles;
    public static bool _readyToSpawn = false;
    private static bool _spawn = false;
    public static bool _spawnOnce = false;
    private IList<Texture> allTextures;
    private Shader _transparent;
    public static IList<Texture> TargetTextures { get; private set; }

    private static float _spacing = 0.35f;//space between the tiles
    private static float _xoffset = -1.9f;

    private float startingX;
    private int _columns;
    private int _rows;

    private Level _config;
    public static bool respawn { get; set; }

    public static bool spawnNext { get; set; }

    void Awake()
    {
        respawn = false;
        spawnNext = false;
    }

    void Start()
    {
        _config = StartUp.LevelConfig();
        _readyToSpawn = true;
        _spawnOnce = true;
        _rows = 0;
        _columns = 0;
        _transparent = Shader.Find("Transparent/Diffuse") as Shader;
        TargetTextures = new List<Texture>();
        mTiles = GameObject.FindGameObjectsWithTag("Tile");
        allTextures = new List<Texture>();
        startingX = -0.41f;
    }

    void Update()
    {
        if (spawnNext)
        {
            _spawn = false;
            Spawn();
            _readyToSpawn = false;
            spawnNext = false;
        }
        if (_readyToSpawn && _spawn)
        {
            _spawn = false;
            Spawn();
            _readyToSpawn = false;
        }
        if (respawn)
        {
            Respawn();
            respawn = false;
        }
        if (!_spawnOnce) return;
        _spawn = true;
        _spawnOnce = false;
    }

    private void Spawn()
    {
        _config = StartUp.LevelConfig();
        //if(_config == null) Application.LoadLevel("StartScreen");
        var row = 1;
        var i = 0;
        TargetTextures = new List<Texture>();
        if (_config != null)
        {
            CountSessionTime.sessionTime = _config.levelTime;
            _columns = _config.columns;
            _rows = _config.rows;
        }
        TargetTextures.Clear(); ;
        var textureList = new List<Texture>();
        var totalTiles = (_columns * _rows) - 1;
        for (var x = 0; x < mTiles.Length; x++)
        {
            mTiles[x].SetActive(true);
            if (x > (totalTiles))
            {
                mTiles[x].SetActive(false);
            }
        }
        SpacingAndOffSet();
        foreach (var tile in mTiles)
        {
            if (!tile.activeSelf) break;

            var tileTransform = tile.transform;

            if (tileTransform == null) return;

            tileTransform.localScale = new Vector3(0.01f, 0.01f, 0.01f);

            var z = GetZ(row);
            var x = startingX + _spacing * i;
            tileTransform.localPosition = new Vector3(x, 0, z);

            i++;

            tileTransform.localRotation = Quaternion.identity;

            var textureDisplay = tileTransform.Find("ImageDisplay").gameObject;
            var texture = LoadManager.Instance.RandomTexture();

            while (textureList.Contains(texture))
                texture = LoadManager.Instance.RandomTexture();
            textureDisplay.renderer.material.mainTexture = texture;
            textureList.Add(texture);
            textureDisplay.renderer.material.shader = _transparent;
            textureDisplay.SetActive(true);
            foreach (var rend in tile.GetComponentsInChildren<Renderer>())
            {
                rend.enabled = false;
            }
            if (spawnNext)
            {
                textureDisplay.GetComponent<Renderer>().enabled = true;
                tile.GetComponent<Renderer>().enabled = true;
            }
            else
            {
                textureDisplay.GetComponent<Renderer>().enabled = false;
                tile.GetComponent<Renderer>().enabled = false;
            }

            if (z == -0.13f)
                tile.GetComponentInChildren<TilePress>().row = 2;
            else if (z == -0.26f)
                tile.GetComponentInChildren<TilePress>().row = 1;
            else if (z == 0.13f)
                tile.GetComponentInChildren<TilePress>().row = 4;
            else
                tile.GetComponentInChildren<TilePress>().row = 3;
            if (i == _columns)
            {
                row++;
                i = 0;
            }

            if (_columns > 7)
            {
                tile.transform.localScale = new Vector3(0.008f, 0.008f, 0.008f);
            }
            tile.GetComponentInChildren<VirtualButtonBehaviour>().enabled = true;
            tile.GetComponentInChildren<TilePress>().Pressed = false;
        }
        allTextures.Clear();
        allTextures = textureList;
        _spawn = false;
        ShowOptions();
    }

    private void Spawn(bool next)
    {
        var row = 1;
        _config = StartUp.LevelConfig();
        var i = 0;
        TargetTextures = new List<Texture>();
        Score.correct = 0;
        Score.wrong = 0;
        CountSessionTime.sessionTime = _config.levelTime;
        TargetTextures.Clear(); ;
        var textureList = new List<Texture>();
        _columns = _config.columns;
        _rows = _config.rows;
        var totalTiles = _columns * _rows - 1;
        for (var x = 0; x < mTiles.Length; x++)
        {
            mTiles[x].SetActive(true);
            if (x > (totalTiles))
            {
                mTiles[x].SetActive(false);
            }
        }
        SpacingAndOffSet();
        foreach (var tile in mTiles)
        {
            if (!tile.activeSelf) break;
            var tileTransform = tile.transform;

            if (tileTransform == null) return;

            tileTransform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
            var z = GetZ(row);

            var x = -0.41f + _spacing * i;

            tileTransform.localPosition = new Vector3(x, 0, z);
            i++;

            tileTransform.localRotation = Quaternion.identity;

            var textureDisplay = tileTransform.Find("ImageDisplay").gameObject;
            var texture = LoadManager.Instance.RandomTexture();
            while (textureList.Contains(texture))
                texture = LoadManager.Instance.RandomTexture();
            textureDisplay.renderer.material.mainTexture = texture;
            textureList.Add(texture);
            textureDisplay.renderer.material.shader = _transparent;
            textureDisplay.SetActive(true);
            textureDisplay.GetComponent<Renderer>().enabled = true;
            tile.GetComponent<Renderer>().enabled = true;
            if (z == -0.13f)
                tile.GetComponentInChildren<TilePress>().row = 2;
            else if (z == -0.26f)
                tile.GetComponentInChildren<TilePress>().row = 1;
            else if (z == 0.13f)
                tile.GetComponentInChildren<TilePress>().row = 4;
            else
                tile.GetComponentInChildren<TilePress>().row = 3;
           
            if (_columns > 7)
            {
                tile.transform.localScale = new Vector3(0.008f, 0.008f, 0.008f);
            }
            if (i == _columns)
            {
                row++;
                i = 0;
            }
        }
        allTextures.Clear();
        allTextures = textureList;
        _spawn = false;
        ShowOptions();
    }

    void ShuffleTargets()
    {
        for (var i = 0; i < allTextures.Count; i++)
        {
            var temp = allTextures[i];
            var randomIndex = Random.Range(i, allTextures.Count);
            allTextures[i] = allTextures[randomIndex];
            allTextures[randomIndex] = temp;
        }
        Debug.Log(allTextures.Count);
        if(_config != null)
            for (var i = 0; i < _config.correctChoices; i++)
            {
                TargetTextures.Add(allTextures[i]);
            }
    }

    void ShowOptions()
    {
        var previousTargets = new List<GameObject>();
        foreach (Transform child in Layout.transform)
            previousTargets.Add(child.gameObject);
        previousTargets.ForEach(Destroy);
        ShuffleTargets();
        for (int i = 0; i < TargetTextures.Count; i++)
        {
            var newTarget = Instantiate(Target) as GameObject;
            //if (StartUp.LevelToLoad % 2 != 0)
            newTarget.transform.SetParent(Layout, false);
            /*else
            {
                MemoryTimer.sessionTime = _config.picTimer;
                MemoryTimer.start = true;
                newTarget.transform.SetParent(MemLayout, false);
            }*/
            var sprite = new Sprite();
            var tempText = TargetTextures[i] as Texture2D;
            sprite = Sprite.Create(tempText, new Rect(0, 0, -tempText.width, -tempText.height), new Vector2(0, 0), 100.0f);

            Image[] uiSprites = newTarget.GetComponentsInChildren<Image>();

            foreach (Image uiSprite in uiSprites)
            {
                if (uiSprite.gameObject.transform.parent.name == newTarget.name)
                    uiSprite.sprite = sprite; //this gameObject is a child, because its transform.parent is not null  
            }
        }
    }

    private void Respawn()
    {
        var i = 0;
        TargetTextures = new List<Texture>();
        Score.correct = 0;
        Score.wrong = 0;
        if(_config != null)
        CountSessionTime.sessionTime = _config.levelTime;
        TargetTextures.Clear(); ;
        var textureList = new List<Texture>();
        var totalTiles = _columns * _rows - 1;
        for (var x = 0; x < mTiles.Length; x++)
        {
            mTiles[x].SetActive(true);
            if (x > (totalTiles))
            {
                mTiles[x].SetActive(false);
            }
        }

        foreach (var tile in mTiles)
        {
            if (!tile.activeSelf) break;
            var tileTransform = tile.transform;
            var textureDisplay = tileTransform.Find("ImageDisplay").gameObject;
            tileTransform.FindChild("frame").GetComponent<Renderer>().enabled = false;
            var texture = LoadManager.Instance.RandomTexture();
            while (textureList.Contains(texture))
                texture = LoadManager.Instance.RandomTexture();
            textureDisplay.renderer.material.mainTexture = texture;
            i++;
            /*if (i <= _config.correctChoices)
                TargetTextures.Add(texture);*/
            textureList.Add(texture);
            textureDisplay.renderer.material.shader = _transparent;
            textureDisplay.SetActive(true);
            textureDisplay.GetComponent<Renderer>().enabled = false;
            tile.GetComponent<Renderer>().enabled = false;
        }
        allTextures.Clear();
        allTextures = textureList;
        ShowOptions();
    }

    private void SpacingAndOffSet()
    {
        switch (_columns)
        {
            case 4:
                startingX = -.35f;
                _spacing = .2f;
                break;
            case 5:
                startingX = -0.41f;
                _spacing = .16f;
                break;
            case 6:
                startingX = -0.41f;
                _spacing = .14f;
                break;
            case 7:
                startingX = -0.41f;
                _spacing = .12f;
                break;
            case 8:
                startingX = -0.41f;
                _spacing = .1f;
                break;
            case 9:
                startingX = -0.41f;
                _spacing = .08f;
                break;
            default:
                startingX = -0.41f;
                _spacing = 0.2f;
                break;
        }
    }

    private float GetZ(int row)
    {
        switch (row)
        {
            case 2:
                return 0.13f;
            case 3:
                return -0.13f;
            case 4:
                return -0.26f;
            default:
                return 0.0f;
        }
    }

}
