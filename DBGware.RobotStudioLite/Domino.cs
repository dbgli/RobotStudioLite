using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBGware.RobotStudioLite
{
    public class Domino
    {
        public int ID { get; set; }
        public DominoSize Size { get; set; } = new();
        public DominoPosition Position { get; set; } = new();
    }
}