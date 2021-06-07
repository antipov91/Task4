using UnityEngine;
using UnityEngine.UI;

public class MenuPanel : MonoBehaviour
{
    [SerializeField] private Button quitBtn;

    private void Start()
    {
        quitBtn.onClick.AddListener(QuitHandle);
    }

    private void QuitHandle()
    {
        Application.Quit();
    }

    private void OnDestroy() 
    {
        quitBtn.onClick.RemoveListener(QuitHandle);     
    }
}
