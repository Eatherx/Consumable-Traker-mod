
using System;
using System.Reflection;
using Terraria;
using Terraria.ModLoader;

namespace ConsumableTracker
{
    public static class TrackerCalamity
    {
        private const string MOD_CALAMITY = "CalamityMod";

        public static void CargarElementos(
            UI_MejorasPermanentes ui,
            Player player)
        {
            if (ui == null || player == null)
                return;

            if (!ModLoader.HasMod(MOD_CALAMITY))
                return;

            // ==========================================
            // MEJORAS PERMANENTES DE VIDA
            // ==========================================

            HelperInterfaz.AgregarFilaMod(
                ui, MOD_CALAMITY, "SanguineTangerine",
                "", 1, LeerBoolCalamity(player, "sTangerine") ? 1 : 0);

            HelperInterfaz.AgregarFilaMod(
                ui, MOD_CALAMITY, "MiracleFruit",
                "", 1, LeerBoolCalamity(player, "mFruit") ? 1 : 0);

            HelperInterfaz.AgregarFilaMod(
                ui, MOD_CALAMITY, "TaintedCloudberry",
                "", 1, LeerBoolCalamity(player, "tCloudberry") ? 1 : 0);

            HelperInterfaz.AgregarFilaMod(
                ui, MOD_CALAMITY, "SacredStrawberry",
                "", 1, LeerBoolCalamity(player, "sStrawberry") ? 1 : 0);


            // ==========================================
            // MEJORAS PERMANENTES DE MANÁ
            // ==========================================

            HelperInterfaz.AgregarFilaMod(
                ui, MOD_CALAMITY, "CometShard",
                "", 1, LeerBoolCalamity(player, "cShard") ? 1 : 0);

            HelperInterfaz.AgregarFilaMod(
                ui, MOD_CALAMITY, "EtherealCore",
                "", 1, LeerBoolCalamity(player, "eCore") ? 1 : 0);

            HelperInterfaz.AgregarFilaMod(
                ui, MOD_CALAMITY, "PhantomHeart",
                "", 1, LeerBoolCalamity(player, "pHeart") ? 1 : 0);


            // ==========================================
            // MEJORAS PERMANENTES DE RAGE
            // ==========================================

            HelperInterfaz.AgregarFilaMod(
                ui, MOD_CALAMITY, "MushroomPlasmaRoot",
                "", 1, LeerBoolCalamity(player, "rageBoostOne") ? 1 : 0);

            HelperInterfaz.AgregarFilaMod(
                ui, MOD_CALAMITY, "InfernalBlood",
                "", 1, LeerBoolCalamity(player, "rageBoostTwo") ? 1 : 0);

            HelperInterfaz.AgregarFilaMod(
                ui, MOD_CALAMITY, "RedLightningContainer",
                "", 1, LeerBoolCalamity(player, "rageBoostThree") ? 1 : 0);


            // ==========================================
            // MEJORAS PERMANENTES DE ADRENALINE
            // ==========================================

            HelperInterfaz.AgregarFilaMod(
                ui, MOD_CALAMITY, "ElectrolyteGelPack",
                "", 1, LeerBoolCalamity(player, "adrenalineBoostOne") ? 1 : 0);

            HelperInterfaz.AgregarFilaMod(
                ui, MOD_CALAMITY, "StarlightFuelCell",
                "", 1, LeerBoolCalamity(player, "adrenalineBoostTwo") ? 1 : 0);

            HelperInterfaz.AgregarFilaMod(
                ui, MOD_CALAMITY, "Ectoheart",
                "", 1, LeerBoolCalamity(player, "adrenalineBoostThree") ? 1 : 0);


            // ==========================================
            // CELESTIAL ONION
            // ==========================================

            bool onionConsumida =
                (Main.masterMode && player.extraAccessory) ||
                LeerBoolCalamity(player, "extraAccessoryML");

            HelperInterfaz.AgregarFilaMod(
                ui, MOD_CALAMITY, "CelestialOnion",
                "", 1, onionConsumida ? 1 : 0);
        }


        // ==============================================
        // LECTURA DE BOOLEANOS DEL CALAMITY PLAYER
        // ==============================================

        private static bool LeerBoolCalamity(
            Player player,
            string nombreCampo)
        {
            if (player == null ||
                string.IsNullOrEmpty(nombreCampo))
                return false;

            try
            {
                foreach (ModPlayer modPlayer in player.ModPlayers)
                {
                    if (modPlayer == null || modPlayer.Mod == null)
                        continue;

                    if (!string.Equals(
                        modPlayer.Mod.Name,
                        MOD_CALAMITY,
                        StringComparison.OrdinalIgnoreCase))
                        continue;

                    Type tipo = modPlayer.GetType();

                    // Recorre también las clases base por seguridad.
                    while (tipo != null)
                    {
                        FieldInfo campo = tipo.GetField(
                            nombreCampo,
                            BindingFlags.IgnoreCase |
                            BindingFlags.Instance |
                            BindingFlags.Public |
                            BindingFlags.NonPublic |
                            BindingFlags.DeclaredOnly);

                        if (campo != null &&
                            campo.FieldType == typeof(bool))
                        {
                            return (bool)campo.GetValue(modPlayer);
                        }

                        PropertyInfo propiedad = tipo.GetProperty(
                            nombreCampo,
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
                                modPlayer, null);
                        }

                        tipo = tipo.BaseType;
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }

            return false;
        }
    }
}
