using Terraria;
using Terraria.ModLoader;

namespace ConsumableTracker
{
    public static class TrackerCalamity
    {
        public static void CargarElementos(UI_MejorasPermanentes ui, Player player)
        {
            if (!ModLoader.HasMod("CalamityMod")) return;

            // --- MEJORAS DE VIDA MAXIMA ---
            HelperInterfaz.AgregarFilaMod(ui, "CalamityMod", "BloodOrange", "bloodOrange", 1, -1, "Blood Orange");
            HelperInterfaz.AgregarFilaMod(ui, "CalamityMod", "MiracleFruit", "miracleFruit", 1, -1, "Miracle Fruit");
            HelperInterfaz.AgregarFilaMod(ui, "CalamityMod", "Elderberry", "elderberry", 1, -1, "Tainted Cloudberry");
            HelperInterfaz.AgregarFilaMod(ui, "CalamityMod", "SacredStrawberry", "sacredStrawberry", 1, -1, "Sacred Strawberry"); // ¡Añadida!
            HelperInterfaz.AgregarFilaMod(ui, "CalamityMod", "DragonFruit", "dragonfruit", 1, -1, "Dragon Fruit");

            // --- MEJORAS DE MANA MAXIMO ---
            HelperInterfaz.AgregarFilaMod(ui, "CalamityMod", "EnchantedStarfish", "enchantedStarfish", 1, -1, "Enchanted Starfish");
            HelperInterfaz.AgregarFilaMod(ui, "CalamityMod", "CometShard", "cometShard", 1, -1, "Comet Shard");
            HelperInterfaz.AgregarFilaMod(ui, "CalamityMod", "EtherealCore", "etherealCore", 1, -1, "Ethereal Core");
            HelperInterfaz.AgregarFilaMod(ui, "CalamityMod", "PhantomHeart", "phantomHeart", 1, -1, "Phantom Heart");

            // --- MEJORAS DE RAGE / ADRENALINA ---
            HelperInterfaz.AgregarFilaMod(ui, "CalamityMod", "MushroomPlasmaRoot", "mushroomPlasmaRoot", 1, -1, "Mushroom Plasma Root");
            HelperInterfaz.AgregarFilaMod(ui, "CalamityMod", "InfernalBlood", "infernalBlood", 1, -1, "Infernal Blood");
            HelperInterfaz.AgregarFilaMod(ui, "CalamityMod", "RedLightningContainer", "redLightningContainer", 1, -1, "Red Lightning Container");
            HelperInterfaz.AgregarFilaMod(ui, "CalamityMod", "ElectrolyteGelPack", "electrolyteGelPack", 1, -1, "Electrolyte Gel Pack");
            HelperInterfaz.AgregarFilaMod(ui, "CalamityMod", "StarlightFuelCell", "starlightFuelCell", 1, -1, "Starlight Fuel Cell");
            HelperInterfaz.AgregarFilaMod(ui, "CalamityMod", "Ectoheart", "ectoheart", 1, -1, "Ectoheart");
            HelperInterfaz.AgregarFilaMod(ui, "CalamityMod", "CelestialOnion", "celestialOnion", 1, -1, "Celestial Onion");
        }
    }
}
