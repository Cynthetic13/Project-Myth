using UnityEngine;
using TMPro;

namespace Myth.Misc
{
    public class VersionText : MonoBehaviour
    {
        private void Start()
        {
            TextMeshProUGUI versionText = GetComponent<TextMeshProUGUI>();
            versionText.text = $"Version: {Application.version}";
            SafeDestroy(gameObject.GetComponent<VersionText>());
        }

        private static void SafeDestroy(Object target)
        {
            if (Application.isEditor)
                Object.DestroyImmediate(target);
            else
                Object.Destroy(target);
        }
    }
}