﻿using System;
using System.Windows;

namespace selfInteriorSimulation
{
    class Sofa : InteriorObject
    {
        public Sofa(Point point) : base(point)
        {
            isType = IsType.Sofa;
            setImg("sofa_dan.PNG");
        }
    }
}
