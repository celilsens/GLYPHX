using System;

[Serializable]
public class StatValue
{
    public StatType Type;

    private float _floatValue;
    private bool _boolValue;
    private int _intValue;

    public StatValue(float value)
    {
        Type = StatType.Float;
        _floatValue = value;
    }

    public StatValue(bool value)
    {
        Type = StatType.Bool;
        _boolValue = value;
    }

    public StatValue(int value)
    {
        Type = StatType.Int;
        _intValue = value;
    }

    public float GetFloat() => _floatValue;
    public bool GetBool() => _boolValue;
    public int GetInt() => _intValue;

    public void SetFloat(float value)
    {
        if (Type == StatType.Float) _floatValue = value;
    }

    public void SetBool(bool value)
    {
        if (Type == StatType.Bool) _boolValue = value;
    }

    public void SetInt(int value)
    {
        if (Type == StatType.Int) _intValue = value;
    }
}
