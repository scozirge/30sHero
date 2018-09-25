using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerPanel : MonoBehaviour
{
    [SerializeField]
    Text GoldText;
    [SerializeField]
    Text EmeraldText;

    public static PlayerPanel MySelf;
    public void Start()
    {
        MySelf = this;
        UpdateResource();
    }

    public static void UpdateResource()
    {
        if (MySelf == null)
            return;
        MySelf.GoldText.text = Player.Gold.ToString();
        MySelf.EmeraldText.text = Player.Emerald.ToString();
    }
}
