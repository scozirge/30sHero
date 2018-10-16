using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectEmitter : MonoBehaviour
{
    static Transform MySelf;
    void Awake()
    {
        MySelf = transform;
    }
    public static ParticleSystem EmitParticle(string _effectName, Vector3 _pos, Vector3 _dir, Transform _parent)
    {
        GameObject particlePrefab = Resources.Load<ParticleSystem>(string.Format("Particles/{0}/{0}", _effectName)).gameObject;
        if (particlePrefab == null)
        {
            Debug.LogWarning("No particle prefab are assigned:" + string.Format("Particles/{0}/{0}", _effectName));
            return null;
        }
        GameObject particleGo = Instantiate(particlePrefab.gameObject, Vector3.zero, Quaternion.identity) as GameObject;
        if (_parent)
            particleGo.transform.SetParent(_parent);
        else
            particleGo.transform.SetParent(MySelf);

        particleGo.transform.localPosition = _pos;
        particleGo.transform.localRotation = Quaternion.Euler(_dir);
        particleGo.AddComponent<ParticleManager>();
        ParticleSystem ps= particleGo.GetComponent<ParticleSystem>();
        return ps;
    }
    public static ParticleSystem EmitParticle(ParticleSystem _particle, Vector3 _pos, Vector3 _dir, Transform _parent)
    {
        ParticleSystem particle = Instantiate(_particle, Vector3.zero, Quaternion.identity) as ParticleSystem;
        if (_parent)
            particle.transform.SetParent(_parent);
        else
            particle.transform.SetParent(MySelf);

        particle.transform.localPosition = _pos;
        particle.transform.localRotation = Quaternion.Euler(_dir);
        particle.gameObject.AddComponent<ParticleManager>();
        return particle;
    }

}
