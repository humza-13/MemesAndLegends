using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingHandler : MonoBehaviour
{
    public Image fillBar;
    public string NextScreen;
    public GameObject ErrorPopup;
    private bool isNetworkConnected;
    private void Start()
    {
        fillBar.fillAmount = 0.1f;
        CheckNetwork();
    }
    private void Update()
    {
        if (fillBar.fillAmount >= 0.8f)
        {
            if (isNetworkConnected)
            {
                fillBar.fillAmount = 1f;
                SceneManager.LoadScene(NextScreen, LoadSceneMode.Single);
            }
        }
        else
        {
            fillBar.fillAmount += 0.03f;
        }
    }
    void CheckNetwork()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            isNetworkConnected = false;
            ErrorPopup.SetActive(true);

        }
        else
            isNetworkConnected = true;
        
    }
    void onNetworkFailed()
    {
        Application.Quit();
    }
}
