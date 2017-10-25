using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PaddleGame
{
    public class PaddlePlayer : IEquatable<PaddlePlayer>
    {
        public string Licence { get; set; }

        public string Name { get; set; }

        public bool Equals(PaddlePlayer other)
        {
            return Licence.Equals(other.Licence);
        }
    }
}
