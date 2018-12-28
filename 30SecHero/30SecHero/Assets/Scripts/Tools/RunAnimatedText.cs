using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class RunAnimatedText : MonoBehaviour
{
    Dictionary<string, AnimatedNumber> AnimatedNumberDic;
    public float Interval = 0.1f;
    public float PeriodTime = 2;
    public void Clear()
    {
        AnimatedNumberDic = new Dictionary<string, AnimatedNumber>();
    }
    public void SetAnimatedText(string _name, int _startNumber, int _endNumber, Text _text)
    {
        if (AnimatedNumberDic == null)
            AnimatedNumberDic = new Dictionary<string, AnimatedNumber>();
        AnimatedNumber an = new AnimatedNumber();
        an.Init(_startNumber, _endNumber, _text);
        an.AddNumber = (int)((float)(_endNumber - _startNumber) / (float)(PeriodTime / Interval));
        if (an.AddNumber < 1)
            an.AddNumber = 1;
        if (!AnimatedNumberDic.ContainsKey(_name))
            AnimatedNumberDic.Add(_name, an);
    }

    public void Play(string _name)
    {
        StartCoroutine(RunNumberText(_name));
    }

    IEnumerator RunNumberText(string _name)
    {
        if (AnimatedNumberDic != null && AnimatedNumberDic.ContainsKey(_name))
        {
            yield return new WaitForSeconds(Interval);
            AnimatedNumberDic[_name].RunText();
            if (!AnimatedNumberDic[_name].CheckIfEnd())
                Play(_name);
        }
    }
}
public class AnimatedNumber
{
    public int StartNumber;
    public int EndNumber;
    public Text NumberText;
    public int AddNumber;
    public int CurNumber;


    public void Init(int _startNumber, int _endNumber, Text _text)
    {
        StartNumber = _startNumber;
        EndNumber = _endNumber;
        NumberText = _text;
        NumberText.text = StartNumber.ToString();
        CurNumber = StartNumber;
    }
    public void RunText()
    {
        CurNumber += AddNumber;
        if (CurNumber > EndNumber)
            CurNumber = EndNumber;
        NumberText.text = CurNumber.ToString();
    }
    public bool CheckIfEnd()
    {
        return (CurNumber == EndNumber);
    }
}
