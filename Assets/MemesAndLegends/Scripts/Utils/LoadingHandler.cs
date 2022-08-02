using UnityEngine.UI;
using UnityEngine;

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
            if(PhotonManager.Instance.isConnected)
            {
                fillBar.fillAmount = 1f;
               // PhotonManager.Instance.LoadSceneAsync(NextScreen);
            }
            else
            {
                if(PhotonManager.Instance.IsNetworkError())
                    ErrorPopup.SetActive(true);
            }
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
        PhotonManager.Instance.Reconnect();

    }
}
