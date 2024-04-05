using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneController : MonoBehaviour
{
    public delegate void SceneLoadDelegate(SceneType _scene);

    public static SceneController instance;

    public bool debug;

    private PageController m_Menu;
    private SceneType m_TargetScene;
    private PageType m_LoadingPage;
    private SceneLoadDelegate m_SceneLoadDelegate;
    private bool m_SceneIsLoading;

    private PageController menu
    {
        get
        {
            if (m_Menu == null)
            {
                m_Menu = PageController.instance;
            }
            if (m_Menu == null)
            {
                LogWarning("You are trying to acces the PageController but no instance was found.");
            }
            return m_Menu;
        }
    }

    private string currentSceneName
    {
        get
        {
            return SceneManager.GetActiveScene().name;
        }
    }

    #region Unity Functions
    private void Awake()
    {
        if (!instance)
        {
            Configure();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDisable()
    {
        Dispose();
    }
    #endregion

    #region Public Functions
    public void Load(SceneType _scene, SceneLoadDelegate _sceneLoadDelegate = null, bool _reload = false, PageType _loadingPage = PageType.None)
    {
        if (_loadingPage != PageType.None && !menu)
        {
            return;
        }

        if (!SceneCanBeLoaded(_scene, _reload))
        {
            return;
        }

        m_SceneIsLoading = true;
        m_TargetScene = _scene;
        m_LoadingPage = _loadingPage;
        m_SceneLoadDelegate = _sceneLoadDelegate;
        StartCoroutine("LoadScene");
    }
    #endregion

    #region Private Functions

    private void Configure()
    {
        instance = this;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Dispose()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private async void OnSceneLoaded(UnityEngine.SceneManagement.Scene _scene, LoadSceneMode _mode)
    {
        if (m_TargetScene == SceneType.None)
        {
            return;
        }

        SceneType _sceneType = StringToSceneType(_scene.name);
        if (m_TargetScene != _sceneType)
        {
            return;
        }

        if (m_SceneLoadDelegate != null)
        {
            try
            {
                m_SceneLoadDelegate(_sceneType);
            }
            catch (System.Exception)
            {
                LogWarning($"Unable to respond with sceneLoadDelegate after scene {_sceneType} Loaded.");
            }
        }

        if (m_LoadingPage != PageType.None)
        {
            //Testing loading pages after instant scene loading, remove this delay when bigger scenes are loading
            await Task.Delay(2000);
            if (_sceneType == SceneType.MainMenu)
            {
                menu.TurnPageOff(m_LoadingPage);
            }
        }

        m_SceneIsLoading = false;
    }

    private IEnumerator LoadScene()
    {
        if (m_LoadingPage != PageType.None)
        {
            menu.TurnPageOn(m_LoadingPage);
            while (!menu.PageIsOn(m_LoadingPage))
            {
                yield return null;

            }
        }

        string _targetSceneName = SceneTypeToString(m_TargetScene);
        SceneManager.LoadScene(_targetSceneName);
    }

    private bool SceneCanBeLoaded(SceneType _scene, bool _reload)
    {
        string _targetSceneName = SceneTypeToString(_scene);
        if (currentSceneName == _targetSceneName && !_reload)
        {
            LogWarning($"You are trying to load a scene {_scene} which is already active");
            return false;
        }
        else if (_targetSceneName == string.Empty)
        {
            LogWarning($"The scene you are tring to load {_scene} is not valid.");
            return false;
        }
        else if (m_SceneIsLoading)
        {
            LogWarning($"Unable to load scene {_scene}. Another scene {m_TargetScene} is already loading.");
            return false;
        }
        return true;
    }

    private string SceneTypeToString(SceneType _scene)
    {
        switch (_scene)
        {
            //Add further scene types in here
            case SceneType.Dungeon: return "Dungeon";
            case SceneType.MainMenu: return "MainMenu";
            case SceneType.End: return "EndgameScreen";
            default:
                LogWarning($"Scene {_scene} does not contain a string for a valid scene.");
                return string.Empty;
        }
    }

    private SceneType StringToSceneType(string _scene)
    {
        switch (_scene)
        {
            //Add further scene types in here
            case "Dungeon": return SceneType.Dungeon;
            case "MainMenu": return SceneType.MainMenu;
            case "EndgameScreen": return SceneType.End;
            default:
                LogWarning($"Scene {_scene} does not contain a type for a valid scene.");
                return SceneType.None;
        }

    }

    private void Log(string _msg)
    {
        if (!debug)
        {
            return;
        }
        Debug.Log("[Scene Controller]: " + _msg);
    }

    private void LogWarning(string _msg)
    {
        if (!debug)
        {
            return;
        }
        Debug.LogWarning("[Scene Controller]: " + _msg);
    }

    #endregion
}