
using UnityEngine;


[System.Serializable]
public class MinMaxValue <T> where T: unmanaged
{
    [SerializeField] T minValue;
    [SerializeField] T minLimit;
    [SerializeField] T maxValue;
    [SerializeField] T maxLimit;

    public T Min { get => minValue; set => minValue = value; }
    public T Max { get => maxValue; set => maxValue = value; }

    public MinMaxValue(T minLimit, T maxLimit)
    {
        this.minLimit = minLimit;
        this.maxLimit = maxLimit;
    }

    public MinMaxValue(T minLimit, T maxLimit, T minValue, T maxValue)
    {
        this.minLimit = minLimit;
        this.maxLimit = maxLimit;
        this.minValue = minValue;
        this.maxValue = maxValue;
    }

    public void SetLimits(T min, T max)
    {
        minLimit = min;
        maxLimit = max;
    }

    // Do not use >.<'''
    public float GetRandomValue()
    {
        if (minLimit is float && maxLimit is float)
        {
            T f1 = (T)(object)minLimit;
            float f2 = (float)(object)f1;
            T f3 = (T)(object)minLimit;
            float f4 = (float)(object)f3;
            return Random.Range(f2, f4);
        }
        return 0;
    }
}


