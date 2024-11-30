using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemsModelling_Kursach.BuildingBlocks.PetriNet.Elements
{
    public class Place
    {
        private string _name;
        private int _markers;

        public int MarkerCount =>_markers;
        public string Name => _name;
        public void IncreaseMark() => _markers++;
        public void DecreaseMark()
        {
            if (_markers == 0) throw new ArgumentException("Markers count can't be negative");
            _markers--;
        }

        public Place(string name, int markers = 0)
        {
            _markers = markers;
            _name = name;
        }
    }
}
