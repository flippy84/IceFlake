﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using IceFlake.Client;
using IceFlake.Client.API;
using IceFlake.Client.Scripts;
using IceFlake.Client.Objects;
using IceFlake.Client.Patchables;

namespace IceFlake.Scripts
{
    public class ItemScript : Script
    {
        public ItemScript()
            : base("ItemInfo", "Uncatalogued")
        { }

        private const int ITEM_ID = 78680;

        public override void OnStart()
        {
            //var sw = Stopwatch.StartNew();
            var itemrec = WoWItem.GetItemRecordFromId(ITEM_ID);
            //var itemsparserec = WoWItem.GetItemSparseRecordFromId(ITEM_ID);
            //sw.Stop();
            //Print("Got ItemRec and ItemSparserec in {0}ms", sw.ElapsedMilliseconds);

            dynamic subclass = itemrec.SubClassId;
            switch (itemrec.Class)
            {
                case ItemClass.Weapon:
                    subclass = itemrec.WeaponClass;
                    break;
                case ItemClass.Armor:
                    subclass = itemrec.ArmorClass;
                    break;
                case ItemClass.Gem:
                    subclass = itemrec.GemClass;
                    break;
            }

            Print("#{0}: {1} | Class: {2} - SubClass: {3}", ITEM_ID, itemrec.Name, itemrec.Class, subclass);

            //sw = Stopwatch.StartNew();
            Print("Stats:");
            //if (itemrec.Class == ItemClass.Weapon)
            //    Print("\tDPS: {0}", itemrec.DPS);
            //if (itemrec.Class == ItemClass.Armor)
            //    Print("\tArmor: {0}", itemrec.Armor);
            foreach (var s in itemrec.Stats)
                Print("\t{0}: {1}", s.Key, s.Value);
            //sw.Stop();
            //Print("Dumped stats in {0}ms", sw.ElapsedMilliseconds);

            Print("Sockets:");
            foreach (var s in itemrec.Sockets)
                Print("\t{0}: {1}", s.Key, s.Value);

            //Print("Fits in:");
            //foreach (var es in WoWItem.GetInventorySlotsByEquipSlot(itemrec.InventoryType))
            //    Print("\t{0}", es);

            //GameError g_err;
            //var equippable = Manager.LocalPlayer.CanUseItem(WoWItem.GetItemSparseRecordPointerFromId(ITEM_ID), out g_err);
            //if (equippable) Print("This item is equippable");
            //else Print("This item is not equippable: {0}", g_err);

            Stop();
        }
    }
}