using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Book : MonoBehaviour
{
    [SerializeField]
    GameObject BookObj;
    [SerializeField]
    List<GameObject> PageList;
    [SerializeField]
    bool Loop;
    [SerializeField]
    bool OpenAtFirstPage;
    [SerializeField]
    Text PageText;

    int MaxPage;
    int CurrentPageIndex;
    bool IsInit;

    // Start is called before the first frame update
    void Start()
    {

        if (PageList != null)
            PageList.RemoveAll(item => item == null);
        if (PageList.Count == 0)
            return;
        if (BookObj == null)
            BookObj = gameObject;
        MaxPage = PageList.Count;
        CurrentPageIndex = 0;
        IsInit = true;
        ShowCurrentPage();
    }
    public void ShowCurrentPage()
    {
        for (int i = 0; i < MaxPage; i++)
        {
            if (i != CurrentPageIndex)
                PageList[i].SetActive(false);
            else
                PageList[i].SetActive(true);
        }
        RefreshPage();
    }
    void OnEnable()
    {
        if (!IsInit)
            return;
        if (OpenAtFirstPage)
        {
            GoToPage(0);
        }
    }
    public void NextPage()
    {
        if (!IsInit)
            return;
        PageList[CurrentPageIndex].SetActive(false);
        CurrentPageIndex++;
        if (CurrentPageIndex >= MaxPage)
            if (Loop)
                CurrentPageIndex = 0;
            else
                CurrentPageIndex = MaxPage - 1;
        PageList[CurrentPageIndex].SetActive(true);
        RefreshPage();
    }
    public void PreviousPage()
    {
        if (!IsInit)
            return;
        PageList[CurrentPageIndex].SetActive(false);
        CurrentPageIndex--;
        if (CurrentPageIndex < 0)
            if (!Loop)
                CurrentPageIndex = 0;
            else
                CurrentPageIndex = MaxPage - 1;
        PageList[CurrentPageIndex].SetActive(true);
        RefreshPage();
    }
    public void GoToPage(int _index)
    {
        if (!IsInit)
            return;
        if (_index > MaxPage || _index < 0)
            return;
        PageList[CurrentPageIndex].SetActive(false);
        CurrentPageIndex = _index;
        PageList[CurrentPageIndex].SetActive(true);
        RefreshPage();
    }
    void RefreshPage()
    {
        PageText.text = string.Format("{0}{1}{2}", CurrentPageIndex + 1, StringData.GetString("Divide"), MaxPage);
    }
    public void OpenBook(bool _bool)
    {
        BookObj.SetActive(_bool);
    }
}
