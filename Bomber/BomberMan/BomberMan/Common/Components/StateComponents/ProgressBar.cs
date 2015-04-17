using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BomberMan.Common.Components.StateComponents
{
    public class ProgressBar : Component
    {
        public bool IsVertical { get; set; }
        public float UsedPercentage { get; set; }
        public void Update(int windowWidth, int windowHeight)
        { }

    }
}
