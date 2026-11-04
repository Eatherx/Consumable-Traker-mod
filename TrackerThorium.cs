using Terraria;
using Terraria.ModLoader;

namespace ConsumableTracker
{
    public static class TrackerThorium
    {
        public static void CargarElementos(UI_MejorasPermanentes ui, Player player)
        {
            if (!ModLoader.HasMod("ThoriumMod")) return;

            // Progresión de Inspiración del Bardo (Límite de 10)
            int fragmentos = HelperInterfaz.EscanearEnteroMod(player, "ThoriumMod", "inspirationFragments");
            HelperInterfaz.AgregarFilaMod(ui, "ThoriumMod", "InspirationFragment", "inspirationFragments", 10, fragmentos, "Inspiration Fragment");

            int esquirlas = HelperInterfaz.EscanearEnteroMod(player, "ThoriumMod", "inspirationShards");
            HelperInterfaz.AgregarFilaMod(ui, "ThoriumMod", "InspirationShard", "inspirationShards", 10, esquirlas, "Inspiration Shard");

            int cristales = HelperInterfaz.EscanearEnteroMod(player, "ThoriumMod", "inspirationCrystals");
            HelperInterfaz.AgregarFilaMod(ui, "ThoriumMod", "InspirationCrystal", "inspirationCrystals", 10, cristales, "Inspiration Crystal");

            // Gema de Inspiración (Consumo único máximo: 1)
            int gemas = HelperInterfaz.EscanearEnteroMod(player, "ThoriumMod", "inspirationGems");
            HelperInterfaz.AgregarFilaMod(ui, "ThoriumMod", "InspirationGem", "inspirationGems", 1, gemas, "Inspiration Gem");

            // Mejoras permanentes de movimiento y olas
            int wave = HelperInterfaz.EscanearEnteroMod(player, "ThoriumMod", "crystalWave");
            HelperInterfaz.AgregarFilaMod(ui, "ThoriumMod", "CrystalWave", "crystalWave", 5, wave, "Crystal Wave");

            bool astral = HelperInterfaz.EscanearBoolMod(player, "ThoriumMod", "astralWave");
            HelperInterfaz.AgregarFilaMod(ui, "ThoriumMod", "AstralWave", "astralWave", 1, astral ? 1 : 0, "Astral Wave");
        }
    }
}
