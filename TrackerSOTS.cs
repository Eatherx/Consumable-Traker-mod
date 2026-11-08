using System;
using System.Reflection;
using Terraria;
using Terraria.ModLoader;

namespace ConsumableTracker;

public static class TrackerSOTS
{
	public static void CargarElementos(UI_MejorasPermanentes ui, Player player)
	{
		Mod sotsMod = default;
		// SOLUCIONADO: Cambiado 'ref' por 'out' para cumplir con la API de tModLoader
		if (!ModLoader.TryGetMod("SOTS", out sotsMod))
		{
			return;
		}
		ModPlayer voidPlayerInstance = null;
		ReadOnlySpan<ModPlayer>.Enumerator enumerator = player.ModPlayers.GetEnumerator();
		while (enumerator.MoveNext())
		{
			ModPlayer mp = enumerator.Current;
			if (((ModType)mp).Mod.Name == "SOTS" && ((object)mp).GetType().Name == "VoidPlayer")
			{
				voidPlayerInstance = mp;
				break;
			}
		}
		int starConsumido = 0;
		int soulHeartConsumido = 0;
		int ankhConsumidos = 0;
		if (voidPlayerInstance != null)
		{
			Type type = ((object)voidPlayerInstance).GetType();
			FieldInfo fStar = type.GetField("voidStar", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			if (fStar != null)
			{
				starConsumido = (int)fStar.GetValue(voidPlayerInstance);
			}
			FieldInfo fSoul = type.GetField("voidSoul", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			if (fSoul != null)
			{
				soulHeartConsumido = (int)fSoul.GetValue(voidPlayerInstance);
			}
			FieldInfo fAnkh = type.GetField("voidAnkh", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			if (fAnkh != null)
			{
				ankhConsumidos = (int)fAnkh.GetValue(voidPlayerInstance);
			}
		}
		HelperInterfaz.AgregarFilaMod(ui, "SOTS", "ScarletStar", "", 1, starConsumido);
		HelperInterfaz.AgregarFilaMod(ui, "SOTS", "VioletStar", "", 1, starConsumido);
		HelperInterfaz.AgregarFilaMod(ui, "SOTS", "SoulHeart", "", 1, soulHeartConsumido);
		HelperInterfaz.AgregarFilaMod(ui, "SOTS", "VoidenAnkh", "", 5, ankhConsumidos);
	}
}
