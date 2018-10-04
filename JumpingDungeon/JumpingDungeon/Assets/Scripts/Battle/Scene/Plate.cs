using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Plate : MonoBehaviour
{
    [SerializeField]
    bool RandomFlip;
    [SerializeField]
    bool RandomRotate;
    [SerializeField]
    Image BottomImage;
    [SerializeField]
    Color[] Colors;

    public int CurColumn { get; private set; }
    int MaxColumn { get; set; }
    public void Init(int _column, int _maxColumn)
    {
        CurColumn = _column;
        MaxColumn = _maxColumn;
        RandomTransform();
    }
    public void LevelUp()
    {
        CurColumn--;
        if (CurColumn < 0)
        {
            CurColumn = MaxColumn - 1;
            RandomTransform();
        }
    }
    public void LevelDown()
    {
        CurColumn++;
        if (CurColumn > MaxColumn - 1)
        {
            CurColumn = 0;
            RandomTransform();
        }
    }
    void RandomTransform()
    {
        Flip();
        Rotate();
    }
    void Flip()
    {
        if (!RandomFlip)
            return;
        int rnd = Random.Range(0, 2);
        if (rnd == 0)
            transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
        else
            transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
    }
    void Rotate()
    {
        if (!RandomRotate)
            return;
        int rnd = Random.Range(0, 2);
        float angle = 180 * rnd;
        transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.x, transform.rotation.y, angle));
    }

}
