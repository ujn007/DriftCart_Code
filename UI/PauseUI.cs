using UnityEngine;
using UnityEngine.SceneManagement;
using Button = UnityEngine.UI.Button;

public class PauseUI : MonoSingleton<PauseUI>
{
    [SerializeField] private Button resumeBtn;
    [SerializeField] private Button settingBtn;
    [SerializeField] private Button mainBtn;
    private CanvasGroup canvasGroup;

    [SerializeField] private GameObject settingPanel;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        resumeBtn.onClick.AddListener(() => Resume());
        settingBtn.onClick.AddListener(() => Setting(true));
        mainBtn.onClick.AddListener(() => Main());
    }

    private void Resume()
    {
        Time.timeScale = 1;
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        AudioManager.Instance.PauseAllSound(false);
        GameManager.Instance.pauseToggle = false;
    }

    public void Setting(bool v)
    {
        settingPanel.SetActive(v);
    }

    private void Main()
    {
        SceneManager.LoadScene(SceneNames.MenuScene);
    }

    public void ShowPanel(bool toggle)
    {
        int num = toggle ? 0 : 1;
        int alpha = toggle ? 1 : 0;

        Time.timeScale = num;

        canvasGroup.alpha = alpha;
        canvasGroup.interactable = toggle;
        canvasGroup.blocksRaycasts = toggle;

        AudioManager.Instance.PauseAllSound(toggle);
        Setting(false);
    }
}
