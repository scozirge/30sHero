using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public struct PropertyText
{
    public string Text;
    public string ColorCode;
    public Comparator Comparison;
    public PropertyText(string _text, string _colorCode, Comparator _comparison)
    {
        Text = _text;
        ColorCode = _colorCode;
        Comparison = _comparison;
    }
}
