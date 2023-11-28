using UnityEngine;

public class VSync : MonoBehaviour
{
    [SerializeField] bool enableVSync = true;

    private void Start()
    {
        ToggleVSync(enableVSync);
    }

    // Call this method to enable or disable V-Sync.
    public void ToggleVSync(bool enableVSync)
    {
        if (enableVSync)
        {
            QualitySettings.vSyncCount = 1; // 1 means V-Sync is enabled
            Application.targetFrameRate = 60;
        }
        else
        {
            QualitySettings.vSyncCount = 0; // 0 means V-Sync is disabled
            Application.targetFrameRate = -1;
        }
    }
}
