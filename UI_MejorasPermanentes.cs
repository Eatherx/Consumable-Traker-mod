
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;

namespace ConsumableTracker;

public class UI_MejorasPermanentes : UIState
{
    private UIPanel panelFondo;
    public UIList listaElementos;
    private UIScrollbar barraDesplazamiento;

    private UIPanel botonArrastrar;

    private int pestanaActiva;
    private Vector2 compensacionArraste;
    private bool arrastrando;

    private int ticksRegeneracion;

    private readonly List<BotonPestanaInfo> botonesPestana = new();

    private const float TAMANO_BOTON = 36f;
    private const float TAMANO_EXPANDIDO = 125f;
    private const float SEPARACION_BOTONES = 6f;

    private class BotonPestanaInfo
    {
        public UIPanel Panel;
        public int Id;
        public string Nombre;
        public Color ColorActivo;
        public Color ColorInactivo;
        public UIText Texto;
        public float AnchoActual;
    }

    public override void OnInitialize()
    {
        panelFondo = new UIPanel();
        panelFondo.SetPadding(10f);
        panelFondo.Left.Set(450f, 0f);
        panelFondo.Top.Set(200f, 0f);
        panelFondo.Width.Set(510f, 0f);
        panelFondo.Height.Set(490f, 0f);
        panelFondo.BackgroundColor = new Color(13, 18, 38, 240);

        CrearBotonPestana(
            "Terraria", "Terraria", 0,
            new Color(35, 65, 135, 255),
            new Color(25, 30, 50, 255)
        );

        if (ModLoader.HasMod("CalamityMod"))
        {
            CrearBotonPestana(
                "Calamity", "Calamity", 1,
                new Color(165, 55, 45, 255),
                new Color(40, 40, 50, 255)
            );
        }

        if (ModLoader.HasMod("ThoriumMod"))
        {
            CrearBotonPestana(
                "Thorium", "Thorium", 2,
                new Color(55, 145, 85, 255),
                new Color(40, 40, 50, 255)
            );
        }

        if (ModLoader.HasMod("Redemption"))
        {
            CrearBotonPestana(
                "Redemption", "Redemption", 3,
                new Color(135, 35, 145, 255),
                new Color(40, 40, 50, 255)
            );
        }

        if (ModLoader.HasMod("SOTS"))
        {
            CrearBotonPestana(
                "SOTS", "SOTS", 4,
                new Color(145, 125, 35, 255),
                new Color(40, 40, 50, 255)
            );
        }

        // BOTÓN PARA ARRASTRAR LA VENTANA
        botonArrastrar = new UIPanel();
        botonArrastrar.Width.Set(24f, 0f);
        botonArrastrar.Height.Set(24f, 0f);
        botonArrastrar.Left.Set(465f, 0f);
        botonArrastrar.Top.Set(13f, 0f);
        botonArrastrar.BackgroundColor = Color.White;

        UIText iconoMover = new UIText("✛", 0.8f, false);
        iconoMover.HAlign = 0.5f;
        iconoMover.VAlign = 0.5f;
        iconoMover.TextColor = Color.Black;

        botonArrastrar.Append(iconoMover);

        botonArrastrar.OnLeftMouseDown += (evt, element) =>
        {
            compensacionArraste = new Vector2(
                evt.MousePosition.X - panelFondo.Left.Pixels,
                evt.MousePosition.Y - panelFondo.Top.Pixels
            );

            arrastrando = true;
        };

        botonArrastrar.OnLeftMouseUp += (evt, element) =>
        {
            arrastrando = false;
        };

        panelFondo.Append(botonArrastrar);

        // LISTA DE MEJORAS
        listaElementos = new UIList();
        listaElementos.Top.Set(55f, 0f);
        listaElementos.Left.Set(5f, 0f);
        listaElementos.Width.Set(450f, 0f);
        listaElementos.Height.Set(410f, 0f);
        listaElementos.ListPadding = 8f;

        panelFondo.Append(listaElementos);

        // BARRA DE DESPLAZAMIENTO
        barraDesplazamiento = new UIScrollbar();
        barraDesplazamiento.Top.Set(55f, 0f);
        barraDesplazamiento.Left.Set(465f, 0f);
        barraDesplazamiento.Width.Set(12f, 0f);
        barraDesplazamiento.Height.Set(410f, 0f);

        panelFondo.Append(barraDesplazamiento);
        listaElementos.SetScrollbar(barraDesplazamiento);

        Append(panelFondo);

        ActualizarPosicionesBotones();
    }

    private void CrearBotonPestana(
        string nombre,
        string archivoLogo,
        int idDestino,
        Color colorActivo,
        Color colorInactivo)
    {
        UIPanel boton = new UIPanel();

        boton.Width.Set(TAMANO_BOTON, 0f);
        boton.Height.Set(TAMANO_BOTON, 0f);
        boton.Top.Set(10f, 0f);
        boton.BackgroundColor = colorInactivo;
        boton.SetPadding(0f);

        // CARGAR LOGO
        var textura = ModContent.Request<Texture2D>(
            "ConsumableTracker/Assets/Logos/" + archivoLogo
        );

        UIImage imagen = new UIImage(textura);
        imagen.Width.Set(28f, 0f);
        imagen.Height.Set(28f, 0f);
        imagen.Left.Set(4f, 0f);
        imagen.Top.Set(4f, 0f);
        imagen.ScaleToFit = true;

        boton.Append(imagen);

        // NOMBRE DEL MOD
        UIText texto = new UIText(nombre, 0.75f, false);
        texto.Left.Set(40f, 0f);
        texto.VAlign = 0.5f;
        texto.TextColor = Color.Transparent;
        texto.IgnoresMouseInteraction = true;

        boton.Append(texto);

        boton.OnLeftClick += (evt, element) =>
        {
            if (pestanaActiva != idDestino)
            {
                pestanaActiva = idDestino;
                listaElementos.Clear();
            }
        };

        panelFondo.Append(boton);

        botonesPestana.Add(new BotonPestanaInfo
        {
            Panel = boton,
            Id = idDestino,
            Nombre = nombre,
            ColorActivo = colorActivo,
            ColorInactivo = colorInactivo,
            Texto = texto,
            AnchoActual = TAMANO_BOTON
        });
    }

    private void ActualizarPosicionesBotones()
    {
        float posicionX = 10f;

        foreach (var boton in botonesPestana)
        {
            boton.Panel.Left.Set(posicionX, 0f);

            posicionX += boton.AnchoActual + SEPARACION_BOTONES;
        }
    }

    private void ActualizarBotones()
    {
        foreach (var boton in botonesPestana)
        {
            bool expandido = boton.Panel.IsMouseHovering;

            float anchoDeseado = expandido
                ? TAMANO_EXPANDIDO
                : TAMANO_BOTON;

            boton.AnchoActual = anchoDeseado;
            boton.Panel.Width.Set(anchoDeseado, 0f);

            // OCULTAR EL NOMBRE CUANDO EL BOTÓN ESTÁ CONTRAÍDO
            boton.Texto.TextColor = expandido
                ? Color.White
                : Color.Transparent;

            boton.Panel.BackgroundColor =
                pestanaActiva == boton.Id
                    ? boton.ColorActivo
                    : boton.ColorInactivo;
        }

        ActualizarPosicionesBotones();
    }

    public void ReiniciarEstado()
    {
        arrastrando = false;
        ticksRegeneracion = 0;

        if (listaElementos != null)
            listaElementos.Clear();
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        if (Main.gameMenu || Main.LocalPlayer == null)
            return;

        if (panelFondo.IsMouseHovering || arrastrando)
            Main.LocalPlayer.mouseInterface = true;

        if (!Main.mouseLeft)
            arrastrando = false;

        // ARRASTRAR VENTANA
        if (arrastrando)
        {
            panelFondo.Left.Set(
                Main.mouseX - compensacionArraste.X,
                0f
            );

            panelFondo.Top.Set(
                Main.mouseY - compensacionArraste.Y,
                0f
            );

            Recalculate();
        }

        // ACTUALIZAR BOTONES
        ActualizarBotones();

        panelFondo.Recalculate();

        Player player = Main.LocalPlayer;

        // ACTUALIZACIÓN AUTOMÁTICA CADA 30 TICKS
        ticksRegeneracion++;

        if (ticksRegeneracion >= 30)
        {
            ticksRegeneracion = 0;
            listaElementos.Clear();
        }

        if (listaElementos.Count == 0)
        {
            switch (pestanaActiva)
            {
                case 0:
                    TrackerVanilla.CargarElementos(this, player);
                    break;

                case 1:
                    if (ModLoader.HasMod("CalamityMod"))
                        TrackerCalamity.CargarElementos(this, player);
                    break;

                case 2:
                    if (ModLoader.HasMod("ThoriumMod"))
                        TrackerThorium.CargarElementos(this, player);
                    break;

                case 3:
                    if (ModLoader.HasMod("Redemption"))
                        TrackerRedemption.CargarElementos(this, player);
                    break;

                case 4:
                    if (ModLoader.HasMod("SOTS"))
                        TrackerSOTS.CargarElementos(this, player);
                    break;
            }
        }
    }
}
