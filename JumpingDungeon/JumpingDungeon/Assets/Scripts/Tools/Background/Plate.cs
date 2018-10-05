using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plate : MonoBehaviour
{
    [SerializeField]
    bool RandomFlip;
    [SerializeField]
    bool RandomRotate;


    protected int CurPlate;
    public int ColumnRank { get; private set; }
    int MaxColumn { get; set; }
    public virtual void Init(int _column, int _maxColumn)
    {
        ColumnRank = _column;
        MaxColumn = _maxColumn;
        RandomTransform();
    }
    public virtual void LevelUp()
    {
        ColumnRank--;
        if (ColumnRank < 0)
        {
            ColumnRank = MaxColumn - 1;
            RandomTransform();
            CurPlate += MaxColumn;
        }
    }
    public virtual void LevelDown()
    {
        ColumnRank++;
        if (ColumnRank > MaxColumn - 1)
        {
            ColumnRank = 0;
            RandomTransform();
            CurPlate -= MaxColumn;
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
