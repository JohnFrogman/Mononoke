using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using MonoGame.Framework;

namespace Mononoke
{
    internal class Apprentice
    {
        int Eloquence;
        int Athletics;
        int Memory;
        int Articulation;
        public string Portrait { get; private set;}
        public string Name { get; private set; }

        public Vector2 Location;
        public Apprentice(string name, string portrait)
        {
            Name = name;
            Portrait = portrait;
        }
    }
}
