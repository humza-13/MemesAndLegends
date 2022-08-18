using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingHandler : MonoBehaviour
{
    private bool allowLoading;
    public Image fillBar;
    public string NextScreen;
    public GameObject ErrorPopup;
    private void Start()
    {
        allowLoading = false;
        fillBar.fillAmount = 0.1f;
        
    }
    private void Update()
    {
        if (fillBar.fillAmount >= 0.8f)
        {
          
              fillBar.fillAmount = 1f;
            SceneManager.LoadScene(NextScreen, LoadSceneMode.Additive);

        }
        else
        {
            fillBar.fillAmount += 0.03f;
        }

    }

    public void Reconnect()
    {
        fillBar.fillAmount = 0.1f;
        ErrorPopup.SetActive(false);

    }
}
