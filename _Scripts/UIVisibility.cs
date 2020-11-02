using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIVisibility : MonoBehaviour
{
    [SerializeField] private GameObject buttons;

    public void UnhideButtons()
    {
        buttons.gameObject.SetActive(true);
    }

    public void HideButtons()
    {
        buttons.gameObject.SetActive(false);
    }
}
