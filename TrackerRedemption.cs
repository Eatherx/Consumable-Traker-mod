using Terraria;
using Terraria.ModLoader;

namespace ConsumableTracker
{
    public static class TrackerRedemption
    {
        public static void CargarElementos(UI_MejorasPermanentes ui, Player player)
        {
            if (!ModLoader.HasMod("Redemption")) return;

            // Mejoras de Poder del Mod of Redemption
            HelperInterfaz.AgregarFilaMod(ui, "Redemption", "GalaxyHeart", "galaxyHeart", 1, -1, "Galaxy Heart");
            HelperInterfaz.AgregarFilaMod(ui, "Redemption", "MedicKit", "medicKit", 1, -1, "Medic Kit");
        }
    }
}
