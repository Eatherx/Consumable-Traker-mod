using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;

namespace ConsumableTracker
{
    public class UI_MejorasPermanentes : UIState
    {
        private UIPanel panelFondo;
        public UIList listaElementos;
        private UIScrollbar barraDesplazamiento;
        
        private UIPanel botonVanilla;
        private UIPanel botonCalamity;
        private UIPanel botonThorium;
        private UIPanel botonRedemption;
        private UIPanel botonSOTS;
        
        private int pestanaActiva = 0;
        private Vector2 compensacionArraste;
        private bool arrastrando = false;

        public override void OnInitialize()
        {
            panelFondo = new UIPanel();
            panelFondo.SetPadding(10);
            panelFondo.Left.Set(450f, 0f); 
            panelFondo.Top.Set(200f, 0f);  
            panelFondo.Width.Set(480f, 0f); 
            panelFondo.Height.Set(490f, 0f);
            panelFondo.BackgroundColor = new Color(13, 18, 38, 240);
            
            panelFondo.OnLeftMouseDown += (evt, element) => {
                compensacionArraste = new Vector2(evt.MousePosition.X - panelFondo.Left.Pixels, evt.MousePosition.Y - panelFondo.Top.Pixels);
                arrastrando = true;
            };
            panelFondo.OnLeftMouseUp += (evt, element) => arrastrando = false;

            botonVanilla = CrearBotonPestana("Terraria", 10f, 0);
            if (ModLoader.HasMod("CalamityMod")) botonCalamity = CrearBotonPestana("Calamity", 100f, 1);
            if (ModLoader.HasMod("ThoriumMod")) botonThorium = CrearBotonPestana("Thorium", 190f, 2);
            if (ModLoader.HasMod("Redemption")) botonRedemption = CrearBotonPestana("Redem.", 280f, 3);
            if (ModLoader.HasMod("SOTS")) botonSOTS = CrearBotonPestana("SOTS", 370f, 4);

            listaElementos = new UIList();
            listaElementos.Top.Set(55f, 0f);
            listaElementos.Left.Set(5f, 0f);
            listaElementos.Width.Set(430f, 0f); 
            listaElementos.Height.Set(410f, 0f);
            listaElementos.ListPadding = 8f; 
            panelFondo.Append(listaElementos);

            barraDesplazamiento = new UIScrollbar();
            barraDesplazamiento.Top.Set(55f, 0f);
            barraDesplazamiento.Left.Set(445f, 0f);
            barraDesplazamiento.Width.Set(12f, 0f);
            barraDesplazamiento.Height.Set(410f, 0f);
            panelFondo.Append(barraDesplazamiento);

            listaElementos.SetScrollbar(barraDesplazamiento);
            Append(panelFondo);
        }

        private UIPanel CrearBotonPestana(string nombre, float xPos, int idDestino)
        {
            UIPanel boton = new UIPanel();
            boton.Width.Set(85f, 0f);
            boton.Height.Set(30f, 0f);
            boton.Left.Set(xPos, 0f);
            boton.Top.Set(10f, 0f);
            boton.BackgroundColor = new Color(25, 30, 50, 255);
            boton.OnLeftClick += (evt, element) => {
                pestanaActiva = idDestino;
            };
            
            UIText texto = new UIText(nombre, 0.75f);
            texto.HAlign = 0.5f;
            texto.VAlign = 0.5f;
            boton.Append(texto);
            panelFondo.Append(boton);
            return boton;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (Main.gameMenu || Main.LocalPlayer == null) return;

            // SOLUCIÓN COMPLETA: Si el cursor del mouse está sobre el panel de la UI o se está arrastrando,
            // le indicamos a Terraria que bloquee las acciones físicas y de ataque del personaje.
            if (panelFondo.IsMouseHovering || arrastrando)
            {
                Main.LocalPlayer.mouseInterface = true;
            }

            if (arrastrando)
            {
                panelFondo.Left.Set(Main.mouseX - compensacionArraste.X, 0f);
                panelFondo.Top.Set(Main.mouseY - compensacionArraste.Y, 0f);
                Recalculate();
            }
            
            listaElementos.Clear();
            Player player = Main.LocalPlayer;

            if (botonVanilla != null) botonVanilla.BackgroundColor = pestanaActiva == 0 ? new Color(35, 65, 135, 255) : new Color(25, 30, 50, 255);
            if (botonCalamity != null) botonCalamity.BackgroundColor = pestanaActiva == 1 ? new Color(165, 55, 45, 255) : new Color(40, 40, 50, 255);
            if (botonThorium != null) botonThorium.BackgroundColor = pestanaActiva == 2 ? new Color(45, 120, 85, 255) : new Color(40, 40, 50, 255);
            if (botonRedemption != null) botonRedemption.BackgroundColor = pestanaActiva == 3 ? new Color(110, 45, 140, 255) : new Color(40, 40, 50, 255);
            if (botonSOTS != null) botonSOTS.BackgroundColor = pestanaActiva == 4 ? new Color(210, 140, 45, 255) : new Color(40, 40, 50, 255);

            if (pestanaActiva == 0) TrackerVanilla.CargarElementos(this, player);
            else if (pestanaActiva == 1) TrackerCalamity.CargarElementos(this, player);
            else if (pestanaActiva == 2) TrackerThorium.CargarElementos(this, player);
            else if (pestanaActiva == 3) TrackerRedemption.CargarElementos(this, player);
            else if (pestanaActiva == 4) TrackerSOTS.CargarElementos(this, player);

            listaElementos.Recalculate();
        }
    }
}
