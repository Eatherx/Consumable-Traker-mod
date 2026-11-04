using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace ConsumableTracker
{
    public class ConsumableTracker : Mod
    {
        internal UserInterface InterfazUsuario;
        internal UI_MejorasPermanentes EstadoInterfaz;
        public static ModKeybind TeclaMenu;

        public override void Load()
        {
            TeclaMenu = KeybindLoader.RegisterKeybind(this, "Abrir Menu", Keys.F8);

            if (!Main.dedServ)
            {
                InterfazUsuario = new UserInterface();
                EstadoInterfaz = new UI_MejorasPermanentes();
                EstadoInterfaz.Activate();
            }
        }

        public override void Unload()
        {
            TeclaMenu = null;
        }
    }

    public class SistemaInterfaz : ModSystem
    {
        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int index = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
            if (index != -1)
            {
                layers.Insert(index, new LegacyGameInterfaceLayer(
                    "ConsumableTracker: UI",
                    delegate
                    {
                        var mod = ModContent.GetInstance<ConsumableTracker>();
                        if (mod.InterfazUsuario?.CurrentState != null)
                        {
                            mod.InterfazUsuario.Draw(Main.spriteBatch, new GameTime());
                        }
                        return true;
                    },
                    InterfaceScaleType.UI)
                );
            }
        }

        public override void UpdateUI(GameTime gameTime)
        {
            var mod = ModContent.GetInstance<ConsumableTracker>();
            
            if (ConsumableTracker.TeclaMenu.JustPressed && Main.gameMenu == false && Main.myPlayer != -1)
            {
                if (mod.InterfazUsuario.CurrentState == null)
                {
                    mod.InterfazUsuario.SetState(mod.EstadoInterfaz);
                }
                else
                {
                    mod.InterfazUsuario.SetState(null);
                }
            }

            if (mod.InterfazUsuario?.CurrentState != null)
            {
                mod.InterfazUsuario.Update(gameTime);
            }
        }
    }
}
