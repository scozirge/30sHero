using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectPlayer : MonoBehaviour
{

    [SerializeField]
    List<EffectData> EffectList;

    public void PlayEffect(string _effectKey)
    {
        if (EffectList == null)
        {
            Debug.LogWarning("EffectList is null");
            return;
        }
        for (int i = 0; i < EffectList.Count; i++)
        {
            if (EffectList[i].Name == _effectKey)
            {
                EffectEmitter.EmitParticle(EffectList[i].Particle, EffectList[i].Offset, EffectList[i].Roration, EffectList[i].TargetParent);
                break;
            }
        }
    }

}
