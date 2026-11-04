using Terraria;
using Terraria.ID;

namespace ConsumableTracker
{
    public static class TrackerVanilla
    {
        public static void CargarElementos(UI_MejorasPermanentes ui, Player player)
        {
            // Lógica matemática directa e inalterable por otros archivos
            int cristalesVidaMaximo = 15;
            int cristalesVidaConsumidos = Utils.Clamp((player.statLifeMax - 100) / 20, 0, cristalesVidaMaximo);
            
            int frutasVidaMaximo = 20;
            int frutasVidaConsumidas = player.statLifeMax >= 400 ? Utils.Clamp((player.statLifeMax - 400) / 5, 0, frutasVidaMaximo) : 0;
            
            int cristalesManaMaximo = 9;
            int cristalesManaConsumidos = Utils.Clamp((player.statManaMax - 20) / 20, 0, cristalesManaMaximo);

            // Cargar renglones nativos fijos
            HelperInterfaz.AgregarFila(ui, ItemID.LifeCrystal, "Life Crystal", cristalesVidaConsumidos, cristalesVidaMaximo);
            HelperInterfaz.AgregarFila(ui, ItemID.LifeFruit, "Life Fruit", frutasVidaConsumidas, frutasVidaMaximo);
            HelperInterfaz.AgregarFila(ui, ItemID.ManaCrystal, "Mana Crystal", cristalesManaConsumidos, cristalesManaMaximo);
            
            HelperInterfaz.AgregarFila(ui, ItemID.DemonHeart, "Demon Heart", player.extraAccessory ? 1 : 0, 1);
            HelperInterfaz.AgregarFila(ui, ItemID.TorchGodsFavor, "Torch God's Favor", player.unlockedBiomeTorches ? 1 : 0, 1);
            HelperInterfaz.AgregarFila(ui, ItemID.CombatBook, "Advanced Combat Tech", NPC.combatBookWasUsed ? 1 : 0, 1);
            HelperInterfaz.AgregarFila(ui, ItemID.CombatBookVolumeTwo, "Combat Tech Vol. 2", NPC.combatBookVolumeTwoWasUsed ? 1 : 0, 1);
            
            HelperInterfaz.AgregarFila(ui, ItemID.ArtisanLoaf, "Artisan Loaf", HelperInterfaz.EscanearBool(player, "usedArtisanLoaf") ? 1 : 0, 1);
            HelperInterfaz.AgregarFila(ui, ItemID.AegisCrystal, "Vital Crystal", HelperInterfaz.EscanearBool(player, "usedVitalCrystal") ? 1 : 0, 1);
            HelperInterfaz.AgregarFila(ui, ItemID.ArcaneCrystal, "Arcane Crystal", HelperInterfaz.EscanearBool(player, "usedArcaneCrystal") ? 1 : 0, 1);
            HelperInterfaz.AgregarFila(ui, ItemID.AegisFruit, "Aegis Fruit", HelperInterfaz.EscanearBool(player, "usedAegisFruit") ? 1 : 0, 1);
            HelperInterfaz.AgregarFila(ui, ItemID.Ambrosia, "Ambrosia", HelperInterfaz.EscanearBool(player, "usedAmbrosia") ? 1 : 0, 1);
            HelperInterfaz.AgregarFila(ui, ItemID.PeddlersSatchel, "Peddler's Satchel", HelperInterfaz.EscanearBool(player, "usedPeddlersSatchel") ? 1 : 0, 1);
            HelperInterfaz.AgregarFila(ui, ItemID.GummyWorm, "Gummy Worm", HelperInterfaz.EscanearBool(player, "usedGummyWorm") ? 1 : 0, 1);
            HelperInterfaz.AgregarFila(ui, ItemID.GalaxyPearl, "Galaxy Pearl", HelperInterfaz.EscanearBool(player, "usedGalaxyPearl") ? 1 : 0, 1);
        }
    }
}
