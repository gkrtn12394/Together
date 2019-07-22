using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ErrorPopupOptions
{
    public System.Action popupDelegate;
    public string message;
}

public class ErrorPopup : MonoBehaviour
{
    public Text message;

    private static GameObject prefab;
    private System.Action popupDelegate;

    public static ErrorPopup show(ErrorPopupOptions options)
    {
        if (prefab == null)
        {
            prefab = Resources.Load("ErrorPopup") as GameObject;
        }

        GameObject popupPrefab = Instantiate(prefab) as GameObject;
        ErrorPopup popup = popupPrefab.GetComponent<ErrorPopup>();
        popup.updateContent(options);

        return popup;
    }

    public void updateContent(ErrorPopupOptions options)
    {
        options.popupDelegate.Invoke();
        message.text = options.message;
    }

    public void dismiss()
    {
        Destroy(gameObject);
    }

    public void OnPressOkButton()
    {
        dismiss();
    }
}
