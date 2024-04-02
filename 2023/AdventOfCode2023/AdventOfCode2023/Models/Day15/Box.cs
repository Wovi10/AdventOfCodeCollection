﻿namespace AdventOfCode2023_1.Models.Day15;

public class Box
{
    private readonly List<Lens> _lenses = new();

    public void DoOperation(Lens lens)
    {
        switch (lens.Operation)
        {
            case Operation.Add:
                TryAdd(lens);
                break;
            case Operation.Remove:
                TryRemove(lens);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void TryAdd(Lens newLens)
    {
        if (_lenses.Any(lens => lens.Label == newLens.Label ))
        {
            _lenses.First(lens => lens.Label == newLens.Label).FocalLength = newLens.FocalLength;
            return;
        }
        
        _lenses.Add(newLens);
    }

    private void TryRemove(Lens lens)
    {
        if (_lenses.Any(l => l.Label == lens.Label)) 
            _lenses.Remove(_lenses.First(l => l.Label == lens.Label));
    }
    
    public int GetFocusingPower(int boxNumber)
    {
        var focusingPower = 0;
        for (var i = 0; i < _lenses.Count; i++)
        {
            focusingPower += _lenses[i].GetFocusingPower(boxNumber, i+1);
        }

        return focusingPower;
    }
}