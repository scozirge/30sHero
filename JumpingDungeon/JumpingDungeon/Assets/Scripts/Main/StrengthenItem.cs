using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StrengthenItem : Item
{
    [SerializeField]
    Image Icon;
    const string ImagePath = "Images/Main/";
    new protected StrengthData MyData;
    int LV;


    public override void Set(Data _data)
    {
        base.Set(_data);
        LV = MyData.LV;
        Icon.sprite = Resources.Load<Sprite>(string.Format(ImagePath + "{0}", MyData.ImageName));
    }
}
