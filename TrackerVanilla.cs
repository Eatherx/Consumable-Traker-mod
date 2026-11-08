using Terraria;

namespace ConsumableTracker;

public static class TrackerVanilla
{
	public static void CargarElementos(UI_MejorasPermanentes ui, Player player)
	{
		int cristalesVidaMaximo = 15;
		int cristalesVidaConsumidos = Utils.Clamp((player.statLifeMax - 100) / 20, 0, cristalesVidaMaximo);
		int frutasVidaMaximo = 20;
		int frutasVidaConsumidas = ((player.statLifeMax >= 400) ? Utils.Clamp((player.statLifeMax - 400) / 5, 0, frutasVidaMaximo) : 0);
		int cristalesManaMaximo = 9;
		int cristalesManaConsumidos = Utils.Clamp((player.statManaMax - 20) / 20, 0, cristalesManaMaximo);
		
		HelperInterfaz.AgregarFila(ui, 29, "Life Crystal", cristalesVidaConsumidos, cristalesVidaMaximo);
		HelperInterfaz.AgregarFila(ui, 1291, "Life Fruit", frutasVidaConsumidas, frutasVidaMaximo);
		HelperInterfaz.AgregarFila(ui, 109, "Mana Crystal", cristalesManaConsumidos, cristalesManaMaximo);
		
		HelperInterfaz.AgregarFila(ui, 3335, "Demon Heart", player.extraAccessory ? 1 : 0, 1);
		HelperInterfaz.AgregarFila(ui, 5043, "Torch God's Favor", player.unlockedBiomeTorches ? 1 : 0, 1);
		HelperInterfaz.AgregarFila(ui, 4382, "Advanced Combat Tech", NPC.combatBookWasUsed ? 1 : 0, 1);
		HelperInterfaz.AgregarFila(ui, 5336, "Combat Tech Vol. 2", NPC.combatBookVolumeTwoWasUsed ? 1 : 0, 1);
		
		int loaf = (HelperInterfaz.EscanearBoolNativo(player, "ateArtisanLoaf") ? 1 : 0);
		int vital = (HelperInterfaz.EscanearBoolNativo(player, "shimmerMaxHealth") ? 1 : 0);
		int arcane = (HelperInterfaz.EscanearBoolNativo(player, "shimmerMaxMana") ? 1 : 0);
		int aegis = (HelperInterfaz.EscanearBoolNativo(player, "usedAegisFruit") ? 1 : 0);
		int ambrosia = (HelperInterfaz.EscanearBoolNativo(player, "usedAmbrosia") ? 1 : 0);
		int satchel = (HelperInterfaz.EscanearBoolNativo(player, "usedPeddlersSatchel") ? 1 : 0);
		int worm = (HelperInterfaz.EscanearBoolNativo(player, "usedGummyWorm") ? 1 : 0);
		int pearl = (HelperInterfaz.EscanearBoolNativo(player, "usedGalaxyPearl") ? 1 : 0);
		
		HelperInterfaz.AgregarFila(ui, 5326, "Artisan Loaf", loaf, 1);
		HelperInterfaz.AgregarFila(ui, 5337, "Vital Crystal", vital, 1);
		HelperInterfaz.AgregarFila(ui, 5339, "Arcane Crystal", arcane, 1);
		HelperInterfaz.AgregarFila(ui, 5338, "Aegis Fruit", aegis, 1);
		HelperInterfaz.AgregarFila(ui, 5342, "Ambrosia", ambrosia, 1);
		HelperInterfaz.AgregarFila(ui, 5343, "Peddler's Satchel", satchel, 1);
		HelperInterfaz.AgregarFila(ui, 5341, "Gummy Worm", worm, 1);
		HelperInterfaz.AgregarFila(ui, 5340, "Galaxy Pearl", pearl, 1);
	}
}
