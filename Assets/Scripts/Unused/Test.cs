using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Test : MonoBehaviour
{
    [SerializeField, ShowIf("show")] MinMaxValue<float> scaleModifier = new MinMaxValue<float>(-2f,3f);
    [SerializeField] MinMaxValue<int> intV;
    //[SerializeField] MinMaxValue<bool> sizeBool;
    //public TestEditorClass testEditor;
    //public TestEditorClass testEditor2;
    [SerializeField] SpawnInformation info;
    [SerializeField] SpawnInformation[] infos;
    [SerializeField] List<SpawnInformation> infos3;

    //public BoolToggleField<MinMaxValue<float>> useAngleLimits;
    public bool show;
    [ShowWhen("show")]public MinMaxValue<float> numbers;


    public SpawnMask mask;
    public SpawnMaskMode mode;
    GameObject tmp;

    [ContextMenu("CheckME!")]
    public void CheckVal()
    {
        tmp = Instantiate(mask.GetSpawnMask(mode));
    }

    [ContextMenu("Limits!")]
    public void Lims()
    {
        DestroyImmediate(tmp);
    }

    private void Reset()
    {

        
    }
}
