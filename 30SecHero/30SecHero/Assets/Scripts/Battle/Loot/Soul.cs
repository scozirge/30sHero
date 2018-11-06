using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
public class Soul : MonoBehaviour
{
    [Tooltip("靈魂ICON")]
    [SerializeField]
    SpriteRenderer SoulIcon;
    [Tooltip("距離目標位置的亂數最大距離")]
    [SerializeField]
    int SurroundRange;
    [Tooltip("改變目標間隔")]
    [SerializeField]
    float SurroundInterval;
    [Tooltip("移動速度")]
    [SerializeField]
    int MoveSpeed;
    [Tooltip("轉向係數")]
    [SerializeField]
    protected float RotateFactor;

    PlayerRole Target;
    Vector2 TargetOffset;
    Vector2 TargetPos;
    MyTimer RandomPosTimer;
    Rigidbody2D MyRigid;


    void Start()
    {
        MyRigid = GetComponent<Rigidbody2D>();
        RandomPosTimer = new MyTimer(SurroundInterval, SetRandomOffsetSurroundTarget, false, false);
        RandomPosTimer.StartRunTimer = true;
        SetRandomOffsetSurroundTarget();
    }
    void Update()
    {
        RandomPosTimer.RunTimer();
    }
    void FixedUpdate()
    {
        FollowTarge();
    }
    void FollowTarge()
    {
        if (!Target)
            return;
        TargetPos = (Vector2)Target.transform.position + TargetOffset;
        MyRigid.velocity = Vector2.Lerp(MyRigid.velocity, (TargetPos - (Vector2)transform.position).normalized * (MoveSpeed + Target.ExtraMoveSpeed), RotateFactor);
        if (MyRigid.velocity.x > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }
    void SetRandomOffsetSurroundTarget()
    {
        int randX = Random.Range(-SurroundRange, SurroundRange);
        int randY = Random.Range(-SurroundRange, SurroundRange);
        TargetOffset = new Vector2(randX, randY);
        RandomPosTimer.StartRunTimer = true;
    }
    public void Init(PlayerRole _target, string _spriteName)
    {
        Target = _target;
        if (SoulIcon != null || _spriteName == "")
        {
            SoulIcon.sprite = Resources.Load<Sprite>(_spriteName);
        }
    }
    public void SelfDestroy()
    {
        Destroy(gameObject);
    }
}
