using System.Collections.Generic;
using Oxide.Core;
using Oxide.Core.Configuration;
using Oxide.Core.Plugins;
using Rust;

namespace Oxide.Plugins
{
    [Info("AutoDrop", "Hazmad", "1.0.3")]
    [Description("Configurable filter for automatically dropping items from a player's inventory / on pickup.")]
    class AutoDrop : RustPlugin
    {
        private Dictionary<ulong, List<string>> autoDropItems = new Dictionary<ulong, List<string>>();

        protected override void LoadDefaultConfig()
        {
            Config.Clear();
            SaveConfig();
        }

        private void LoadData()
        {
            autoDropItems = Interface.Oxide.DataFileSystem.ReadObject<Dictionary<ulong, List<string>>>("AutoDropItems");
        }

        private void SaveData()
        {
            Interface.Oxide.DataFileSystem.WriteObject("AutoDropItems", autoDropItems);
        }

        private void OnServerInitialized()
        {
            LoadData();
            permission.RegisterPermission("autodrop.use", this);
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
                    List<string> itemShortnames = new List<string>();
                    foreach (string arg in args)
                    {
                        ItemDefinition itemDef = ItemManager.FindItemDefinition(arg.ToLower());
                        if (itemDef != null)
                        {
                            itemShortnames.Add(itemDef.shortname);
                        }
                    }

                    if (itemShortnames.Count > 0)
                    {
                        autoDropItems[player.userID] = itemShortnames;
                        SaveData();
                        player.ChatMessage("AutoDrop: Items registered for automatic drop.");
                    }
                    else
                    {
                        player.ChatMessage("AutoDrop: Invalid item shortnames provided.");
                    }
                }
                else
                {
                    player.ChatMessage("AutoDrop: Usage: /autodrop <itemShortname1> <itemShortname2> ...");
                }
            }
            else
            {
                player.ChatMessage("AutoDrop: You don't have permission to use this command.");
            }
        }

        private void OnItemAddedToContainer(ItemContainer container, Item item)
        {
            if (container.playerOwner != null)
            {
                List<string> itemShortnames;
                if (autoDropItems.TryGetValue(container.playerOwner.userID, out itemShortnames))
                {
                    if (itemShortnames.Contains(item.info.shortname))
                    {
                        item.Drop(container.playerOwner.transform.position, container.playerOwner.transform.forward * 2f);
                    }
                }
            }
        }
    }
}
