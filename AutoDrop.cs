using System.Collections.Generic;
using Oxide.Core;
using Oxide.Core.Configuration;
using Oxide.Core.Plugins;
using Rust;

namespace Oxide.Plugins
{
    [Info("AutoDrop", "Hazmad", "1.0.0")]
    [Description("Configurable filter for automatically dropping items from a player's inventory / on pickup.")]
    class AutoDrop : RustPlugin
    {
        private Dictionary<ulong, List<int>> autoDropItems = new Dictionary<ulong, List<int>>();

        protected override void LoadDefaultConfig()
        {
            Config.Clear();
            SaveConfig();
        }

        private void LoadData()
        {
            autoDropItems = Interface.Oxide.DataFileSystem.ReadObject<Dictionary<ulong, List<int>>>("AutoDropItems");
        }

        private void SaveData()
        {
            Interface.Oxide.DataFileSystem.WriteObject("AutoDropItems", autoDropItems);
        }

        private void OnServerInitialized()
        {
            LoadData();
        }

        private void Unload()
        {
            SaveData();
        }

        [ChatCommand("autodrop")]
        private void AutoDropCommand(BasePlayer player, string cmd, string[] args)
        {
            if (permission.UserHasPermission(player.UserIDString, "autodrop.use"))
            {
                if (args.Length > 0)
                {
                    List<int> itemIDs = new List<int>();
                    foreach (string arg in args)
                    {
                        if (int.TryParse(arg, out int itemID))
                        {
                            itemIDs.Add(itemID);
                        }
                    }

                    if (itemIDs.Count > 0)
                    {
                        autoDropItems[player.userID] = itemIDs;
                        SaveData();
                        player.ChatMessage("AutoDrop: Items registered for automatic drop.");
                    }
                    else
                    {
                        player.ChatMessage("AutoDrop: Invalid item IDs provided.");
                    }
                }
                else
                {
                    player.ChatMessage("AutoDrop: Usage: /autodrop <itemID1> <itemID2> ...");
                }
            }
            else
            {
                player.ChatMessage("AutoDrop: You don't have permission to use this command.");
            }
        }

        private void OnPlayerLootEnd(PlayerLoot playerLoot)
        {
            if (autoDropItems.TryGetValue(playerLoot.gameObject.GetComponent<BasePlayer>().userID, out List<int> itemIDs))
            {
                ItemContainer mainInventory = playerLoot.GetComponent<PlayerInventory>().containerMain;
                ItemContainer wearInventory = playerLoot.GetComponent<PlayerInventory>().containerWear;

                // Check main inventory
                foreach (Item item in mainInventory.itemList)
                {
                    if (itemIDs.Contains(item.info.itemid))
                    {
                        item.Drop(playerLoot.gameObject.transform.position, playerLoot.gameObject.transform.forward * 2f);
                    }
                }

                // Check wear (equipment) inventory
                foreach (Item item in wearInventory.itemList)
                {
                    if (itemIDs.Contains(item.info.itemid))
                    {
                        item.Drop(playerLoot.gameObject.transform.position, playerLoot.gameObject.transform.forward * 2f);
                    }
                }
            }
        }


    }
}
