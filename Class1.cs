using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oxide.Core;
using Oxide.Core.Plugins;
using Oxide.Game.Rust.Cui;
using TinyJSON;
using UnityEngine;
using VLB;

namespace Oxide.Plugins
{
    [Info("RustMod", "VSP", "1.0.0")]
    [Description("My first plugin")]
    class Class1 : RustPlugin
    {
        private ulong SkinID = 2095609692;
        private string CardPrefabName = "keycard_red";
        void OnEntityTakeDamage(BaseCombatEntity entity, HitInfo info)
        {
            var player = info.InitiatorPlayer;
            if (player && player.IsAlive())
            {
                CuiHelper.DestroyUi(player, "hitMarker");
                string json = CUI_HitMarker;
                if (entity.health < (entity.health * 0.7))
                    json = CUI_HitMarker.Replace("0.627451 0.654902 1 1", "1 0.8 0 1");
                if (entity.health < (entity.health / 0.3))
                    json = CUI_HitMarker.Replace("0.627451 0.654902 1 1", "0.0 0.1 0.2 1");

                CuiHelper.AddUi(player, json);
                timer.Once(0.5f, () => { CuiHelper.DestroyUi(player, "hitMarker"); });

            }
        }
        void OnEntityDeath(BaseCombatEntity entity, HitInfo info)
        {
            var player = info.InitiatorPlayer;
            if (player && player.IsAlive())
            {
                CuiHelper.DestroyUi(player, "hitMarker");
            }
        }
        string CUI_HitMarker = @"[{""name"":""hitMarker"",""parent"":""Overlay"",""components"":[{""type"":""UnityEngine.UI.Image"",""sprite"":""assets/icons/loading.png"",""material"":"""",""color"":""0.627451 0.654902 1 1""},{""type"":""RectTransform"",""anchormin"":""0.5 0.5"",""anchormax"":""0.5 0.5"",""offsetmin"":""-50 -50"",""offsetmax"":""50 50""}]}]";

        [ConsoleCommand("card")]
        void CardCommand(ConsoleSystem.Arg arg)
        {
            BasePlayer player = BasePlayer.FindByID(ulong.Parse(arg.Args[0]));
            if (player == null) return;
            CreateItem(player);
        }
        void CreateItem(BasePlayer player)
        {
            Item item = ItemManager.CreateByName(CardPrefabName, 1, SkinID);
            item.name = "Магнитная карта R.U.S.T.";
            item.text = "Карта позволяющая получить доступ к любой двери";
            player.GiveItem(item);
        }
        private object OnCardSwipe(CardReader cardReader, Keycard card, BasePlayer player)
        {
            if (card.skinID == SkinID)
            {
                var cards = card.GetItem();
                cardReader.Invoke(new Action(cardReader.GrantCard), 0.5f);
                cards.LoseCondition(0.5f);
                return true;
            }
            return null;
    }
    }

}

