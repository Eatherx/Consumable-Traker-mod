using System;
using System.Reflection;
using Terraria;
using Terraria.ModLoader;

namespace ConsumableTracker
{
    public static class TrackerSOTS
    {
        public static void CargarElementos(UI_MejorasPermanentes ui, Player player)
        {
            // Verificar si el mod SOTS está activo mediante la API de tModLoader
            if (!ModLoader.TryGetMod("SOTS", out Mod sotsMod)) return;

            // Buscamos el componente de datos que SOTS le pega al jugador (SOTSPlayer)
            ModPlayer sotsPlayerInstance = null;
            foreach (var mp in player.ModPlayers)
            {
                if (mp.Mod.Name == "SOTS" && mp.GetType().Name.Contains("Player"))
                {
                    sotsPlayerInstance = mp;
                    break;
                }
            }

            // Variables para guardar si el jugador los consumió o no
            int scarletConsumido = 0;
            int violetConsumido = 0;
            int soulHeartConsumido = 0;
            int ankhConsumidos = 0;

            // Si encontramos el componente en el jugador, extraemos de forma manual usando las variables del código fuente original
            if (sotsPlayerInstance != null)
            {
                Type tipoSOTSPlayer = sotsPlayerInstance.GetType();

                // CORREGIDO: Busca "CrimsonCrescent" (v2026) y si no existe busca "crimsonCrescent"
                FieldInfo fScarlet = tipoSOTSPlayer.GetField("CrimsonCrescent", BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);
                if (fScarlet == null) fScarlet = tipoSOTSPlayer.GetField("crimsonCrescent", BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);
                if (fScarlet != null) scarletConsumido = (bool)fScarlet.GetValue(sotsPlayerInstance) ? 1 : 0;

                // CORREGIDO: Busca "CorruptCrescent" (v2026) y si no existe busca "corruptCrescent"
                FieldInfo fViolet = tipoSOTSPlayer.GetField("CorruptCrescent", BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);
                if (fViolet == null) fViolet = tipoSOTSPlayer.GetField("corruptCrescent", BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);
                if (fViolet != null) violetConsumido = (bool)fViolet.GetValue(sotsPlayerInstance) ? 1 : 0;

                // CORREGIDO: Busca "SoulHeart" (v2026) y si no existe busca "soulHeart"
                FieldInfo fSoul = tipoSOTSPlayer.GetField("SoulHeart", BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);
                if (fSoul == null) fSoul = tipoSOTSPlayer.GetField("soulHeart", BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);
                if (fSoul != null) soulHeartConsumido = (bool)fSoul.GetValue(sotsPlayerInstance) ? 1 : 0;

                // CORREGIDO: Busca "VoidAnkhs" (v2026), "voidenAnkhsUsed" o "voidAnkhsUsed"
                FieldInfo fAnkh = tipoSOTSPlayer.GetField("VoidAnkhs", BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);
                if (fAnkh == null) fAnkh = tipoSOTSPlayer.GetField("voidenAnkhsUsed", BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);
                if (fAnkh == null) fAnkh = tipoSOTSPlayer.GetField("voidAnkhsUsed", BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);
                
                if (fAnkh != null) ankhConsumidos = (int)fAnkh.GetValue(sotsPlayerInstance);
            }

            // Inyectamos las filas apuntando a los nombres de los ítems y pasando las variables reales corregidas
            HelperInterfaz.AgregarFilaMod(ui, "SOTS", "ScarletCrescent", "", 1, scarletConsumido, "Scarlet Crescent");
            HelperInterfaz.AgregarFilaMod(ui, "SOTS", "VioletCrescent", "", 1, violetConsumido, "Violet Crescent");
            HelperInterfaz.AgregarFilaMod(ui, "SOTS", "SoulHeart", "", 1, soulHeartConsumido, "Soul Heart");
            HelperInterfaz.AgregarFilaMod(ui, "SOTS", "VoidenAnkh", "", 5, ankhConsumidos, "Voiden Ankh");
        }
    }
}
