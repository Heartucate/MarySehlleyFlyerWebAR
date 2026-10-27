using UnityEngine;

public class WebsiteButton : MonoBehaviour
{
    [SerializeField] private string websiteURL = "https://www.deine-webseite.de";
    public void OpenWebsite()
    {
        Application.OpenURL(websiteURL);
    }
}