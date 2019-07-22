using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupOptions
{
    public System.Action okButtonDelegate;
}

public class Popup : MonoBehaviour
{
    private static GameObject prefab;
    private System.Action okButtonDelegate;

    public static Popup show(PopupOptions options)
    {
        if(prefab == null)
        {
            prefab = Resources.Load("Popup") as GameObject;
        }

        GameObject popupPrefab = Instantiate(prefab) as GameObject;
        Popup popup = popupPrefab.GetComponent<Popup>();
        popup.updateContent(options);

        return popup;
    }

    public void updateContent(PopupOptions options)
    {
        okButtonDelegate = options.okButtonDelegate;
    }

    public void dismiss()
    {
        Destroy(gameObject);
    }

    public void OnPressOkButton()
    {
        okButtonDelegate.Invoke();

        string input = GameObject.Find("Player Name text").GetComponent<Text>().text;

        if(input != "")
        {
            dismiss();
        }
    }
}
