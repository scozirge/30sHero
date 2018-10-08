using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PropertyItem : Item
{
    [SerializeField]
    Text PropertyText;
    [SerializeField]
    Image Arrow;
    [SerializeField]
    Sprite UpwardArrow;
    [SerializeField]
    Sprite DownwardArrow;


    public void SetString(PropertyText _data)
    {
        PropertyText.text = _data.Text;
        Arrow.gameObject.SetActive(true);
        switch (_data.Comparison)
        {
            case Comparator.Equal:
                Arrow.gameObject.SetActive(false);
                break;
            case Comparator.Greater:
                Arrow.sprite = UpwardArrow;
                break;
            case Comparator.Less:
                Arrow.sprite = DownwardArrow;
                break;
        }
        Color color;
        if (ColorUtility.TryParseHtmlString(_data.ColorCode, out color))
        {
            PropertyText.color = color;
        }
        else
        {
            Debug.LogWarning(string.Format("無效的色碼:{0}", _data.ColorCode));
            PropertyText.color = Color.red;
        }
    }
}
