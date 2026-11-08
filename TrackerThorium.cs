
using System;
using System.Reflection;
using Terraria;
using Terraria.ModLoader;

namespace ConsumableTracker
{
    public static class TrackerThorium
    {
        private const string MOD_THORIUM = "ThoriumMod";

        public static void CargarElementos(
            UI_MejorasPermanentes ui,
            Player player)
        {
            if (ui == null || player == null)
                return;

            if (!ModLoader.TryGetMod(
                MOD_THORIUM,
                out Mod thoriumMod))
                return;

            ModPlayer thoriumPlayer =
                ObtenerThoriumPlayer(player);

            if (thoriumPlayer == null)
                return;

            // Fragment, Shard y Crystal utilizan
            // InspirationConsumableBase.GetConsumeParameters.

            AgregarConsumibleInspiracion(
                ui,
                thoriumMod,
                player,
                "InspirationFragment",
                "Inspiration Fragment");

            AgregarConsumibleInspiracion(
                ui,
                thoriumMod,
                player,
                "InspirationShard",
                "Inspiration Shard");

            AgregarConsumibleInspiracion(
                ui,
                thoriumMod,
                player,
                "InspirationCrystalNew",
                "Inspiration Crystal");

            // La gema utiliza un booleano independiente.
            bool gemaConsumida = LeerBool(
                thoriumPlayer,
                "consumedInspirationGem");

            HelperInterfaz.AgregarFilaMod(
                ui,
                MOD_THORIUM,
                "InspirationGem",
                "",
                1,
                gemaConsumida ? 1 : 0);
        }

        private static void AgregarConsumibleInspiracion(
            UI_MejorasPermanentes ui,
            Mod thoriumMod,
            Player player,
            string nombreInterno,
            string nombreVisible)
        {
            if (!thoriumMod.TryFind(
                nombreInterno,
                out ModItem modItem))
            {
                return;
            }

            int actual = 0;
            int total = 10;

            ObtenerProgresoInspiracion(
                thoriumMod,
                modItem,
                player,
                out actual,
                out total);

            HelperInterfaz.AgregarFilaMod(
                ui,
                MOD_THORIUM,
                nombreInterno,
                "",
                total,
                actual);
        }

        private static bool ObtenerProgresoInspiracion(
            Mod thoriumMod,
            ModItem modItem,
            Player player,
            out int actual,
            out int total)
        {
            actual = 0;
            total = 10;

            try
            {
                Type tipoBase = thoriumMod.GetType()
                    .Assembly.GetType(
                        "ThoriumMod.Items.BardItems.InspirationConsumableBase");

                if (tipoBase == null)
                    return false;

                MethodInfo metodo = tipoBase.GetMethod(
                    "GetConsumeParameters",
                    BindingFlags.Public |
                    BindingFlags.Static);

                if (metodo == null)
                    return false;

                // Parámetros:
                // InspirationConsumableBase modItem
                // Player player
                // out int count
                // out int total

                object[] parametros =
                {
                    modItem,
                    player,
                    0,
                    10
                };

                metodo.Invoke(null, parametros);

                actual = Convert.ToInt32(parametros[2]);
                total = Convert.ToInt32(parametros[3]);

                return true;
            }
            catch
            {
                actual = 0;
                total = 10;
                return false;
            }
        }

        private static ModPlayer ObtenerThoriumPlayer(
            Player player)
        {
            if (player == null)
                return null;

            foreach (ModPlayer mp in player.ModPlayers)
            {
                if (mp == null || mp.Mod == null)
                    continue;

                if (!string.Equals(
                    mp.Mod.Name,
                    MOD_THORIUM,
                    StringComparison.OrdinalIgnoreCase))
                    continue;

                if (string.Equals(
                    mp.GetType().Name,
                    "ThoriumPlayer",
                    StringComparison.OrdinalIgnoreCase))
                {
                    return mp;
                }
            }

            return null;
        }

        private static bool LeerBool(
            object instancia,
            string nombre)
        {
            if (instancia == null ||
                string.IsNullOrEmpty(nombre))
                return false;

            Type tipo = instancia.GetType();

            while (tipo != null)
            {
                try
                {
                    FieldInfo campo = tipo.GetField(
                        nombre,
                        BindingFlags.IgnoreCase |
                        BindingFlags.Instance |
                        BindingFlags.Public |
                        BindingFlags.NonPublic |
                        BindingFlags.DeclaredOnly);

                    if (campo != null &&
                        campo.FieldType == typeof(bool))
                    {
                        return (bool)campo.GetValue(instancia);
                    }

                    PropertyInfo propiedad = tipo.GetProperty(
                        nombre,
                        BindingFlags.IgnoreCase |
                        BindingFlags.Instance |
                        BindingFlags.Public |
                        BindingFlags.NonPublic |
                        BindingFlags.DeclaredOnly);

                    if (propiedad != null &&
                        propiedad.PropertyType == typeof(bool) &&
                        propiedad.GetIndexParameters().Length == 0)
                    {
                        return (bool)propiedad.GetValue(
                            instancia,
                            null);
                    }
                }
                catch
                {
                    return false;
                }

                tipo = tipo.BaseType;
            }

            return false;
        }
    }
}
