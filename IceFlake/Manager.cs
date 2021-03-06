﻿using GreyMagic;
using IceFlake.Client;
using IceFlake.Client.Objects;
using IceFlake.Client.Collections;
using IceFlake.Client.Scripts;
using IceFlake.DirectX;
using IceFlake.Runtime;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Diagnostics;
using System.Reflection;
using System.Collections.Generic;

namespace IceFlake
{
    internal static class Manager
    {
        internal static void Initialize()
        {
            Memory = new InProcessMemoryReader(Process.GetCurrentProcess());

            Direct3D.OnFirstFrame += Start;
            Direct3D.OnLastFrame += Stop;
            Direct3D.Initialize();
        }

        internal static void Start(object sender, EventArgs e)
        {
            Stopwatch sw = Stopwatch.StartNew();

            Direct3D.RegisterCallbacks(
                ObjectManager = new ObjectManager(),
                ExecutionQueue = new EndSceneExecute(),
                Movement = new Movement(),
                Events = new WoWEvents(),
                Spellbook = new SpellCollection(),
                Scripts = new ScriptManager()
                );

            Helper.Initialize();
            DBC = new WoWDB();
            Quests = new QuestCollection();
            Inventory = new WoWInventory();
            Camera = new WoWCamera();

            sw.Stop();
            Log.WriteLine(LogType.Good, "Initialization took {0} ms", sw.ElapsedMilliseconds);
        }

        internal static void Stop(object sender, EventArgs e)
        {
            Log.WriteLine(LogType.Information, "Shutting down IceFlake");
            Events = null;
            Spellbook = null;
            Movement = null;
            DBC = null;
            ObjectManager = null;
            ExecutionQueue = null;

            Memory.Detours.RemoveAll();
            Memory.Patches.RemoveAll();

            Memory = null;

            GC.Collect();
            // We need something clever here...
        }

        internal static InProcessMemoryReader Memory { get; private set; }
        internal static ObjectManager ObjectManager { get; private set; }
        internal static EndSceneExecute ExecutionQueue { get; private set; }
        internal static WoWDB DBC { get; private set; }
        internal static Movement Movement { get; private set; }
        internal static SpellCollection Spellbook { get; private set; }
        internal static QuestCollection Quests { get; private set; }
        internal static WoWInventory Inventory { get; private set; }
        internal static WoWCamera Camera { get; private set; }
        internal static WoWEvents Events { get; private set; }
        internal static ScriptManager Scripts { get; private set; }

        internal static WoWLocalPlayer LocalPlayer
        {
            get { return ObjectManager.LocalPlayer; }
        }
    }
}
