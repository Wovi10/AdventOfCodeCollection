﻿using _2023.Models.Day20;
using AOC.Utils;
using UtilsCSharp;

namespace _2023;

public class Day20() : DayBase("20","Pulse Propagation")
{
    protected override Task<object> PartOne()
    {
        var result = GetTotalPulses();
        
        return Task.FromResult<object>(result);
    }
    
    protected override Task<object> PartTwo()
    {
        var result = GetButtonsPressed();
        
        return Task.FromResult<object>(result);
    }

    private static long GetTotalPulses()
    {
        var desertMachineHeadquarters = new DesertMachineHeadquarters(Input);
        desertMachineHeadquarters.PressButton(1000);
        
        return DesertMachineHeadquarters.GetTotalPulses();
    }

    private static long GetButtonsPressed()
    {
        var desertMachineHeadquarters = new DesertMachineHeadquarters(Input);
        desertMachineHeadquarters.PressButton();

        return MathUtils.Lcm(desertMachineHeadquarters.HelperCounter);
    }
}