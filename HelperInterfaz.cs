using Microsoft.Xna.Framework;
using System;
using System.Reflection;
using Terraria;
using Terraria.ID; 
using Terraria.GameContent; 
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;

namespace ConsumableTracker
{
    public static class HelperInterfaz
    {
        public static void AgregarFila(UI_MejorasPermanentes ui, int itemId, string nombrePorDefecto, int actual, int maximo)
        {
            UIElement fila = new UIElement();
            fila.Width.Set(410f, 0f); 
            fila.Height.Set(34f, 0f);

            Item itemTemp = new Item();
            itemTemp.netDefaults(itemId);
            UIItemIcon icono = new UIItemIcon(itemTemp, false);
            icono.Left.Set(5f, 0f);
            icono.Top.Set(0f, 0f);
            fila.Append(icono);

            string nombreTraducido = Lang.GetItemNameValue(itemId);
            if (string.IsNullOrEmpty(nombreTraducido)) {
                nombreTraducido = nombrePorDefecto;
            }

            UIText textoNombre = new UIText(nombreTraducido, 0.8f);
            textoNombre.Left.Set(45f, 0f);
            textoNombre.Top.Set(8f, 0f);
            fila.Append(textoNombre);

            string textoIndicador = actual >= maximo ? "✔" : $"{actual}/{maximo}";
            Color colorIndicador = actual >= maximo ? Color.LimeGreen : (actual > 0 ? Color.Yellow : Color.LightSlateGray);

            UIText contador = new UIText(textoIndicador, 0.8f);
            contador.Left.Set(380f, 0f); 
            contador.Top.Set(8f, 0f);
            contador.TextColor = colorIndicador;
            fila.Append(contador);

            ui.listaElementos.Add(fila);
        }

        public static void AgregarFilaMod(UI_MejorasPermanentes ui, string modNombre, string itemNombreID, string campoBooleano, int maximo, int conteoForzado = -1, string nombreVisibleDefecto = "")
        {
            if (ModLoader.TryGetMod(modNombre, out Mod modObtenido))
            {
                int resolvedID = 0;
                string nombreAMostrar = string.IsNullOrEmpty(nombreVisibleDefecto) ? itemNombreID : nombreVisibleDefecto;

                try
                {
                    if (modObtenido.TryFind<ModItem>(itemNombreID, out ModItem modItem))
                    {
                        resolvedID = modItem.Type;
                        if (modItem.DisplayName != null && !string.IsNullOrEmpty(modItem.DisplayName.Value))
                        {
                            nombreAMostrar = modItem.DisplayName.Value;
                        }
                    }
                }
                catch (Exception)
                {
                    resolvedID = 0;
                }

                if (resolvedID > 0)
                {
                    int actual = conteoForzado != -1 ? conteoForzado : (EscanearBoolMod(Main.LocalPlayer, modNombre, campoBooleano) ? 1 : 0);
                    AgregarFila(ui, resolvedID, nombreAMostrar, actual, maximo);
                }
            }
        }

        // ¡AGREGADA DE VUELTA!: Función para que TrackerVanilla pueda leer los booleanos del jugador nativo
        public static bool EscanearBool(Player player, string campoNombre)
        {
            FieldInfo campo = player.GetType().GetField(campoNombre, BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);
            if (campo != null) return (bool)campo.GetValue(player);
            return false;
        }

        public static bool EscanearBoolMod(Player player, string modNombre, string variableCampo)
        {
            foreach (var modPlayer in player.ModPlayers)
            {
                if (modPlayer.Mod.Name == modNombre)
                {
                    FieldInfo campo = modPlayer.GetType().GetField(variableCampo, BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);
                    if (campo != null) return (bool)campo.GetValue(modPlayer);
                }
            }
            return false;
        }

        public static int EscanearEnteroMod(Player player, string modNombre, string variableCampo)
        {
            foreach (var modPlayer in player.ModPlayers)
            {
                if (modPlayer.Mod.Name == modNombre)
                {
                    FieldInfo campo = modPlayer.GetType().GetField(variableCampo, BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);
                    if (campo != null && campo.FieldType == typeof(int)) return (int)campo.GetValue(modPlayer);
                }
            }
            return 0;
        }
    }
}
