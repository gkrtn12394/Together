using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuitPopupOptions
{
    public System.Action cancelButtonDelegate;
    public System.Action okButtonDelegate;
}

public class QuitPopup : MonoBehaviour
{
    public GameObject okButton;
    public GameObject cancelButton;

    private static GameObject prefab;
    private System.Action cancelButtonDelegate;
    private System.Action okButtonDelegate;

    public static QuitPopup show(QuitPopupOptions options)
    {
        if (prefab == null)
        {
            prefab = Resources.Load("QuitPopup") as GameObject;
        }

        GameObject popupPrefab = Instantiate(prefab) as GameObject;
        QuitPopup popup = popupPrefab.GetComponent<QuitPopup>();
        popup.updateContent(options);

        return popup;
    }

    public void updateContent(QuitPopupOptions options)
    {
        cancelButtonDelegate = options.cancelButtonDelegate;
        okButtonDelegate = options.okButtonDelegate;
    }

    public void dismiss()
    {
        Destroy(gameObject);
    }

    public void OnPressCancelButton()
    {
        cancelButtonDelegate.Invoke();

        dismiss();
    }

    public void OnPressOkButton()
    {
        okButtonDelegate.Invoke();

        dismiss();
    }
}
