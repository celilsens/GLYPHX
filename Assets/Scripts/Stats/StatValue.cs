using System;

[Serializable]
public class StatValue
{
    public StatType Type;

    private float floatValue;
    private bool boolValue;
    private int intValue;

    public StatValue(float value)
    {
        Type = StatType.Float;
        floatValue = value;
    }

    public StatValue(bool value)
    {
        Type = StatType.Bool;
        boolValue = value;
    }

    public StatValue(int value)
    {
        Type = StatType.Int;
        intValue = value;
    }

    public float GetFloat() => floatValue;
    public bool GetBool() => boolValue;
    public int GetInt() => intValue;

    public void SetFloat(float value)
    {
        if (Type == StatType.Float) floatValue = value;
    }

    public void SetBool(bool value)
    {
        if (Type == StatType.Bool) boolValue = value;
    }

    public void SetInt(int value)
    {
        if (Type == StatType.Int) intValue = value;
    }
}
