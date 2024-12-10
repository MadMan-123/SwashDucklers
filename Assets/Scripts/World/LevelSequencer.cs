using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vector3 = System.Numerics.Vector3;

public class LevelSequencer : MonoBehaviour
{
    public List<Section> sections = new List<Section>();

    private void Start()
    {
        
    }

    [Serializable]
    public class Section
    {
        public Transition Transition;
        public GameObject Effected;
        public float waitTime;
    }
    [Serializable]
    public struct Transition
    {
        public Vector3 Start , End;
        public AnimationCurve TimeCurve;
    }
}


