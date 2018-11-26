using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public partial class Debugger : MonoBehaviour
{


    // Update is called once per frame
    void KeyDetector()
    {
        /*
        Scene scene = SceneManager.GetActiveScene();
        if (scene.name=="Battle")
        {

        }
        */
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Player.ShowBaseProperties();
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            Player.ShowEquipProperties();
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            Player.ShowStrengthenProperties();
        }
        else if (Input.GetKeyDown(KeyCode.V))
        {
            Player.ShowTotalProperties();
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            BattleManage.BM.MyPlayer.AddBuffer(RoleBuffer.Freeze,5);
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            BattleManage.BM.MyPlayer.AddBuffer(RoleBuffer.Burn,5);
        }
        else if (Input.GetKeyDown(KeyCode.T))
        {
            int damage = 10;
            BattleManage.BM.MyPlayer.BeAttack(Force.Enemy, ref damage, Vector2.zero);
        }
    }
}
