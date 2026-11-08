
using System;
using System.Reflection;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;

namespace ConsumableTracker
{
    public static class HelperInterfaz
    {
        public static void AgregarFila(
            UI_MejorasPermanentes ui,
            int itemId,
            string nombrePorDefecto,
            int actual,
            int maximo)
        {
            UIElement fila = new UIElement();
            fila.Width.Set(410f, 0f);
            fila.Height.Set(34f, 0f);

            Item val = new Item();
            val.netDefaults(itemId);

            UIItemIcon icono = new UIItemIcon(val, false);
            icono.Left.Set(5f, 0f);
            icono.Top.Set(0f, 0f);
            fila.Append(icono);

            string nombreTraducido = Lang.GetItemNameValue(itemId);

            if (string.IsNullOrEmpty(nombreTraducido) ||
                nombreTraducido == "Unnamed")
            {
                nombreTraducido = nombrePorDefecto;
            }

            UIText textoNombre = new UIText(
                nombreTraducido, 0.8f, false);

            textoNombre.Left.Set(45f, 0f);
            textoNombre.Top.Set(8f, 0f);
            fila.Append(textoNombre);

            string obj = actual >= maximo
                ? "✔"
                : $"{actual}/{maximo}";

            Color colorIndicador =
                actual >= maximo
                    ? Color.LimeGreen
                    : actual > 0
                        ? Color.Yellow
                        : Color.LightSlateGray;

            UIText contador = new UIText(obj, 0.8f, false);

            contador.Left.Set(380f, 0f);
            contador.Top.Set(8f, 0f);
            contador.TextColor = colorIndicador;

            fila.Append(contador);

            ui.listaElementos.Add(fila);
        }

        public static void AgregarFilaMod(
            UI_MejorasPermanentes ui,
            string modNombre,
            string itemNombreID,
            string campoNombreMemoria,
            int maximo,
            int conteoForzado = -1)
        {
            if (!ModLoader.TryGetMod(
                modNombre, out Mod modObtenido))
                return;

            // Buscar exclusivamente el nombre solicitado.
            if (!modObtenido.TryFind(
                itemNombreID, out ModItem modItem))
            {
                return;
            }

            int resolvedID = modItem.Type;

            string nombreAMostrar = itemNombreID;

            if (modItem.DisplayName != null &&
                !string.IsNullOrEmpty(
                    modItem.DisplayName.Value))
            {
                nombreAMostrar = modItem.DisplayName.Value;
            }

            int actual = 0;

            if (conteoForzado != -1)
            {
                actual = conteoForzado;
            }
            else if (!string.IsNullOrEmpty(
                campoNombreMemoria))
            {
                actual = EscanearValorModFijo(
                    Main.LocalPlayer,
                    modNombre,
                    campoNombreMemoria);
            }

            AgregarFila(
                ui,
                resolvedID,
                nombreAMostrar,
                actual,
                maximo);
        }

        public static bool EscanearBoolNativo(
            Player player,
            string campoNombre)
        {
            if (player == null ||
                string.IsNullOrEmpty(campoNombre))
                return false;

            try
            {
                foreach (var modPlayer in player.ModPlayers)
                {
                    if (modPlayer == null)
                        continue;

                    Type tipoMP = modPlayer.GetType();

                    FieldInfo campoMod = tipoMP.GetField(
                        campoNombre,
                        BindingFlags.IgnoreCase |
                        BindingFlags.Instance |
                        BindingFlags.Public |
                        BindingFlags.NonPublic);

                    if (campoMod != null &&
                        campoMod.FieldType == typeof(bool))
                    {
                        return (bool)campoMod.GetValue(modPlayer);
                    }

                    PropertyInfo propMod = tipoMP.GetProperty(
                        campoNombre,
                        BindingFlags.IgnoreCase |
                        BindingFlags.Instance |
                        BindingFlags.Public |
                        BindingFlags.NonPublic);

                    if (propMod != null &&
                        propMod.PropertyType == typeof(bool))
                    {
                        return (bool)propMod.GetValue(
                            modPlayer, null);
                    }
                }

                PropertyInfo prop = typeof(Player).GetProperty(
                    campoNombre,
                    BindingFlags.Instance |
                    BindingFlags.Public |
                    BindingFlags.IgnoreCase);

                if (prop != null &&
                    prop.PropertyType == typeof(bool))
                {
                    return (bool)prop.GetValue(player, null);
                }

                FieldInfo campo = typeof(Player).GetField(
                    campoNombre,
                    BindingFlags.Instance |
                    BindingFlags.Public |
                    BindingFlags.NonPublic |
                    BindingFlags.IgnoreCase);

                if (campo != null &&
                    campo.FieldType == typeof(bool))
                {
                    return (bool)campo.GetValue(player);
                }
            }
            catch
            {
            }

            return false;
        }

        private static int EscanearValorModFijo(
            Player player,
            string modNombre,
            string campoNombre)
        {
            if (player == null ||
                string.IsNullOrEmpty(modNombre) ||
                string.IsNullOrEmpty(campoNombre))
                return 0;

            try
            {
                foreach (var modPlayer in player.ModPlayers)
                {
                    if (modPlayer == null ||
                        modPlayer.Mod == null)
                        continue;

                    if (!string.Equals(
                        modPlayer.Mod.Name,
                        modNombre,
                        StringComparison.OrdinalIgnoreCase))
                        continue;

                    Type tipo = modPlayer.GetType();

                    FieldInfo campo = tipo.GetField(
                        campoNombre,
                        BindingFlags.IgnoreCase |
                        BindingFlags.Instance |
                        BindingFlags.Public |
                        BindingFlags.NonPublic);

                    if (campo != null)
                    {
                        object val = campo.GetValue(modPlayer);

                        if (val is int i)
                            return i;

                        if (val is bool b)
                            return b ? 1 : 0;
                    }

                    PropertyInfo propiedad = tipo.GetProperty(
                        campoNombre,
                        BindingFlags.IgnoreCase |
                        BindingFlags.Instance |
                        BindingFlags.Public |
                        BindingFlags.NonPublic);

                    if (propiedad != null &&
                        propiedad.GetIndexParameters().Length == 0)
                    {
                        object val = propiedad.GetValue(
                            modPlayer, null);

                        if (val is int i)
                            return i;

                        if (val is bool b)
                            return b ? 1 : 0;
                    }
                }
            }
            catch
            {
            }

            return 0;
        }
    }
}
