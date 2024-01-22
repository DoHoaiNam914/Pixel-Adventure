using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("Main Menu")]
    [SerializeField]
    private GameObject _mainMenu;
    [SerializeField]
    private Button _startButton;
    [SerializeField]
    private Button _quitButton;


    [Header("Level Menu")]
    [SerializeField]
    private GameObject _levelMenu;
    [SerializeField]
    private Button _homeButton;
    [SerializeField]
    private Transform _levelList;
    [SerializeField]
    private GameObject _levelButton;

    public int UnlockedLevel {
        get => PlayerPrefs.GetInt("UnlockedLevel", 1);
        set => PlayerPrefs.SetInt("UnlockedLevel", value);
    }

    public void OnStartButtonClick()
    {
        if (UnlockedLevel == 1)
        {
            SceneManager.LoadScene($"Level1");
            return;
        }

        foreach (Transform item in _levelList)
        {
            Destroy(item.gameObject);
        }

        for (int i = 0; i < UnlockedLevel; i++)
        {
            GameObject levelButton = Instantiate(_levelButton, Vector3.zero, Quaternion.identity, _levelList);

            int level = i + 1;
            levelButton.GetComponentInChildren<Text>().text = $"Level {level}";
            levelButton.GetComponent<Button>().onClick.AddListener(() => SceneManager.LoadScene(level));
        }

        _mainMenu.SetActive(false);
        _levelMenu.SetActive(true);
    }

    public void OnHomeButtonClick()
    {
        _levelMenu.SetActive(false);
        _mainMenu.SetActive(true);
    }

    // Start is called before the first frame update
    void Start()
    {
        _startButton.onClick.AddListener(OnStartButtonClick);
        _quitButton.onClick.AddListener(() => Application.Quit());
        _homeButton.onClick.AddListener(OnHomeButtonClick);
    }
}
