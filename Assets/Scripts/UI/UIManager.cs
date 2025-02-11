using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("[REFERENCES] Scriptable Objects")]
    [SerializeField] public PlayerInputActions playerInput;
    [SerializeField] public ResourceStats resourceStats;
    [SerializeField] public PowerNodeStats powerNodeStats;
    [SerializeField] public UIStats UIStats;
    [SerializeField] public GameState gameState;

    [Header("[REFERENCES] build phase UIs")]
    [SerializeField] public GameObject[] buildPhaseUIs;

    [Header("[REFERENCES] Wave Related UI")]
    [SerializeField] public GameObject _waveStartConfirmationUI;
    [SerializeField] public TMP_Text _currentWave;
    [SerializeField] public TMP_Text _totalWaves;
    [SerializeField] public TMP_Text _totalEnemiesAliveThisWave;
    

    [Header("[REFERENCES] Resource/Upkeep UI")]
    [SerializeField] public TMP_Text _resourcesTxt;
    [SerializeField] public TMP_Text _maxUpkeepTxt;
    [SerializeField] public TMP_Text _upkeepTxt;

    [Header("[REFERENCES] UI Buttons")]
    [SerializeField] public Button _startWaveButton;
    [SerializeField] public Button _menuButton;

    [Header("[DEBUG] private variables")]
    [SerializeField] private int _intResources;
    [SerializeField] private int _intUpkeep;
    [SerializeField] private int _intMaxUpkeep;

    private InputAction pause;

    // Start is called before the first frame update

    private void Awake()
    {
        _startWaveButton.interactable = true;
        _waveStartConfirmationUI.SetActive(false);
        playerInput = new PlayerInputActions();
        TryGetComponent<Canvas>(out UIStats.canvas);
    }

    private void OnEnable()
    {
        pause = playerInput.Gameplay.Pause;
        pause.Enable();
        pause.performed += PauseGame;
    }

    private void OnDisable()
    {
        pause.performed -= PauseGame;
        pause.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        DisplayNodeInfo();
        DisplayWaveInfo();

        if (!gameState.IsPaused())
        {
            _menuButton.interactable = true;
        }
        else
        {
            _menuButton.interactable = false;
        }

        if (gameState.BuildPhase)
        {
            _startWaveButton.interactable = true;
            ActivateBuildPhaseUI();
        }
        else
        {
            DeactivateBuildPhaseUI();
        }

    }

    void DisplayNodeInfo()
    {
        _intUpkeep = Mathf.RoundToInt(powerNodeStats.GetUpkeep());
        _intMaxUpkeep = Mathf.RoundToInt(powerNodeStats.GetMaxUpkeep());
        _intResources = Mathf.RoundToInt(resourceStats.GetTotalResources());

        _upkeepTxt.text = _intUpkeep.ToString();
        _maxUpkeepTxt.text = _intMaxUpkeep.ToString();
        _resourcesTxt.text = _intResources.ToString();


        if (powerNodeStats.IsOverHardCap())
        {
            _upkeepTxt.color = Color.red;
        }
        else if (powerNodeStats.IsOverCapped())
        {
            Color orangeColor = new Color(1f, 0.5f, 0f);
            _upkeepTxt.color = orangeColor;
        }
        else
        {
            Color customColor = new Color(50f / 255f, 242f / 255f, 255f / 255f);
            _upkeepTxt.color = customColor;
        }


        if (_intUpkeep >= 999)
        {
            _upkeepTxt.text = "999";
        }        
        if (_intMaxUpkeep >= 999)
        {
            _maxUpkeepTxt.text = "999";
        }
        if (_intResources >= 9999999)
        {
            _resourcesTxt.text = "9999999";
        }
    }

    void DisplayWaveInfo()
    {
        _currentWave.text = gameState._currentWave.ToString();
        _totalWaves.text = gameState._totalWaves.ToString();
        _totalEnemiesAliveThisWave.text = gameState._totalEnemiesThisWave.ToString();

        if (gameState._currentWave == gameState._totalWaves)
        {
            _currentWave.color = Color.red;
            _totalWaves.color = Color.red;
        }
        else
        {
            _currentWave.color = Color.white;
            _totalWaves.color = Color.white;
        }
    }

    void PauseGame(InputAction.CallbackContext context)
    {
        gameState.SetPaused(!gameState.IsPaused());
    }

    public void PauseGameThroughMenu()
    {
        gameState.SetPaused(true);
    }

    public void StartWavePhase()
    {
        _waveStartConfirmationUI.SetActive(false);
        _startWaveButton.interactable = false;
        gameState.BuildPhase = false;
    }

    public void ReturnToBuildPhase()
    {
        _waveStartConfirmationUI.SetActive(false);
        _startWaveButton.interactable = true;
    }

    public void StartWaveButton()
    {
        _startWaveButton.interactable = false;
        _waveStartConfirmationUI.SetActive(true);
    }

    public void DeactivateBuildPhaseUI()
    {
        foreach (GameObject uiObject in buildPhaseUIs)
        {
            DisableIconScript(uiObject);
        }
    }    
    
    public void ActivateBuildPhaseUI()
    {
        foreach (GameObject uiObject in buildPhaseUIs)
        {
            EnableIconScript(uiObject);
        }
    }

    private void DisableIconScript(GameObject structureUI)
    {
        structureUI.GetComponent<IconScript>().enabled = false;
    }

    private void EnableIconScript(GameObject structureUI)
    {
        structureUI.GetComponent<IconScript>().enabled = true;
    }


}
