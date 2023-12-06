﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Mononoke
{
    internal class DoodadAssetManager
    {
        static Dictionary<string, DoodadTemplate> Doodads = new();

        public DoodadTemplate GetDoodadTemplate(string name)
        { 
            if (!Doodads.ContainsKey(name))
            {
                LoadDoodad(name);
            }
            return Doodads[name];
        }
        void LoadDoodad(string name)
        {
            Doodads.Add(name, DoodadTemplate.fromJson(JsonDocument.Parse(File.ReadAllText("data/doodads/" + name)).RootElement));
        }
    }
}