
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
            TeclaMenu = KeybindLoader.RegisterKeybind(
                this,
                "Abrir Menu",
                Keys.K
            );

            if (!Main.dedServ)
            {
                InterfazUsuario = new UserInterface();

                EstadoInterfaz = new UI_MejorasPermanentes();
                EstadoInterfaz.Activate();

                // La interfaz comienza cerrada.
                InterfazUsuario.SetState(null);
            }
        }

        public override void Unload()
        {
            TeclaMenu = null;
            InterfazUsuario = null;
            EstadoInterfaz = null;
        }
    }

    public class SistemaInterfaz : ModSystem
    {
        public override void OnWorldLoad()
        {
            ReiniciarInterfaz();
        }

        public override void OnWorldUnload()
        {
            ReiniciarInterfaz();
        }

        private void ReiniciarInterfaz()
        {
            var mod = ModContent.GetInstance<ConsumableTracker>();

            if (mod.InterfazUsuario != null)
                mod.InterfazUsuario.SetState(null);

            mod.EstadoInterfaz?.ReiniciarEstado();
        }

        public override void ModifyInterfaceLayers(
            List<GameInterfaceLayer> layers)
        {
            int index = layers.FindIndex(
                layer => layer.Name.Equals("Vanilla: Mouse Text")
            );

            if (index != -1)
            {
                layers.Insert(
                    index,
                    new LegacyGameInterfaceLayer(
                        "ConsumableTracker: UI",
                        delegate
                        {
                            var mod = ModContent.GetInstance<ConsumableTracker>();

                            if (mod.InterfazUsuario?.CurrentState != null)
                            {
                                mod.InterfazUsuario.Draw(
                                    Main.spriteBatch,
                                    new GameTime()
                                );
                            }

                            return true;
                        },
                        InterfaceScaleType.UI
                    )
                );
            }
        }

        public override void UpdateUI(GameTime gameTime)
        {
            var mod = ModContent.GetInstance<ConsumableTracker>();

            if (mod.InterfazUsuario == null)
                return;

            // Cerrar la interfaz en el menú principal.
            if (Main.gameMenu)
            {
                if (mod.InterfazUsuario.CurrentState != null)
                    mod.InterfazUsuario.SetState(null);

                return;
            }

            // CONTROL DE LA TECLA K.
            if (ConsumableTracker.TeclaMenu != null &&
                ConsumableTracker.TeclaMenu.JustPressed &&
                Main.myPlayer != -1)
            {
                if (mod.InterfazUsuario.CurrentState == null)
                {
                    // Abrir nuestra interfaz.
                    mod.InterfazUsuario.SetState(mod.EstadoInterfaz);

                    // Abrir el inventario.
                    Main.playerInventory = true;
                }
                else
                {
                    // Cerrar solamente nuestra interfaz.
                    // El inventario permanece abierto.
                    mod.InterfazUsuario.SetState(null);
                }
            }

            if (mod.InterfazUsuario.CurrentState != null)
            {
                mod.InterfazUsuario.Update(gameTime);
            }
        }
    }
}
