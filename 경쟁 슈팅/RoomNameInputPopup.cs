using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomNameInputPopupOptions
{
    public System.Action okButtonDelegate;
    public string infoString;
}

public class RoomNameInputPopup : MonoBehaviour
{
    public Text text;

    private static GameObject prefab;
    private System.Action okButtonDelegate;

    public static RoomNameInputPopup show(RoomNameInputPopupOptions options)
    {
        if (prefab == null)
        {
            prefab = Resources.Load("RoomNameInputPopup") as GameObject;
        }

        GameObject popupPrefab = Instantiate(prefab) as GameObject;
        RoomNameInputPopup popup = popupPrefab.GetComponent<RoomNameInputPopup>();
        popup.updateContent(options);

        return popup;
    }

    public void updateContent(RoomNameInputPopupOptions options)
    {
        okButtonDelegate = options.okButtonDelegate;
        text.text = options.infoString;
    }

    public void dismiss()
    {
        Destroy(gameObject);
    }

    public void OnPressOkButton()
    {
        okButtonDelegate.Invoke();

        string input = GameObject.Find("Game Name text").GetComponent<Text>().text;

        if (input != "")
        {
            dismiss();
        }
    }
}

