using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TestEditorClass
{
    public bool useAngleLimits;
    public MinMaxValue<float> angleLimits = new MinMaxValue<float>(-359, 359);


    
}
