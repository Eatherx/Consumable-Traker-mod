using Terraria;
using Terraria.ModLoader;

namespace ConsumableTracker;

public static class TrackerRedemption
{
	public static void CargarElementos(UI_MejorasPermanentes ui, Player player)
	{
		if (ModLoader.HasMod("Redemption"))
		{
			// 1. MEDIC KIT (Aparece primero en la UI por orden de progresión)
			int kitConsumido = HelperInterfaz.EscanearBoolNativo(player, "medKit") ? 1 : 0;
			
			if (ModLoader.GetMod("Redemption").TryFind<ModItem>("MedicKit", out ModItem kitItem))
			{
				HelperInterfaz.AgregarFila(ui, kitItem.Type, "Medic Kit", kitConsumido, 1);
			}

			// 2. GALAXY HEART (Aparece debajo del Medic Kit)
			HelperInterfaz.AgregarFilaMod(ui, "Redemption", "GalaxyHeart", "galaxyHeart", 1);
		}
	}
}
