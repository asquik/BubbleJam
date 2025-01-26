using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManuHandler : MonoBehaviour
{
    [SerializeField] Button StartButton;
    [SerializeField] Button SettingsButton;
    [SerializeField] Button QuitButton;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartButton.onClick.AddListener(OnStart);
        QuitButton.onClick.AddListener(OnQuit);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnStart()
    {
        SceneManager.LoadScene("Level 1");
    }

    private void OnQuit()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
    }
}
