using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using ImGuiNET;

namespace CherryEngine.Gui
{
    class DebugOverlay
    {
        //TODO: Limpiar el codigo
        private float prueba = 0.0f;
        private int counter = 0;
        private int location = 3;
        ImGuiWindowFlags window_flags = ImGuiWindowFlags.NoDecoration | ImGuiWindowFlags.NoDocking |
                                            ImGuiWindowFlags.NoDocking | ImGuiWindowFlags.AlwaysAutoResize |
                                            ImGuiWindowFlags.NoSavedSettings | ImGuiWindowFlags.NoNav;
        ImGuiIOPtr io = ImGui.GetIO();
        private float transaparency = 0.35f;

        public DebugOverlay()
        {
            // Initialize any necessary resources here
        }

        public void Draw()
        {
            // Remove NoMove from window_flags before setting it conditionally
            window_flags &= ~ImGuiWindowFlags.NoMove;

            if (location >= 0)
            {
                const float PAD = 10.0f;
                ImGuiViewportPtr viewport = ImGui.GetMainViewport();
                Vector2 work_pos = new Vector2(viewport.WorkPos.X, viewport.WorkPos.Y); // Use work area to avoid menu-bar/task-bar, if any!
                Vector2 work_size = new Vector2(viewport.WorkSize.X, viewport.WorkSize.Y);

                Vector2 window_pos, window_pos_pivot;
                window_pos.X = (location & 1) != 0 ? work_pos.X + work_size.X - PAD : work_pos.X + PAD;
                window_pos.Y = (location & 2) != 0 ? work_pos.Y + work_size.Y - PAD : work_pos.Y + PAD;
                window_pos_pivot.X = (location & 1) != 0 ? 1.0f : 0.0f;
                window_pos_pivot.Y = (location & 2) != 0 ? 1.0f : 0.0f;
                ImGui.SetNextWindowPos(window_pos, ImGuiCond.Always, window_pos_pivot);
                ImGui.SetNextWindowViewport(viewport.ID); // Fix: Access ID from ImGuiViewportPtr
                window_flags |= ImGuiWindowFlags.NoMove;
            }
            else if (location == -2)
            {
                // Center window
                ImGui.SetNextWindowPos(ImGui.GetMainViewport().GetCenter(), ImGuiCond.Always, new Vector2(0.5f, 0.5f));
                window_flags |= ImGuiWindowFlags.NoMove;
            }

            ImGui.SetNextWindowBgAlpha(transaparency); // Transparent background

            if (!ImGui.Begin("Informacion de Juego", window_flags))
                return;


            ImGui.Text("(click-derecho para cambiar de posición)");
            ImGui.Separator();
            if (ImGui.IsMousePosValid())
                ImGui.Text($"Posicion de mouse: ({io.MousePos.X:F1},{io.MousePos.Y:F1})");
            else
                ImGui.Text("Posicion de mouse: <invalid>");

            ImGui.Text("Texto de prueba :D"); // Display some text
            ImGui.SliderFloat("float", ref prueba, 0.0f, 1.0f); // Edit 1 float using a slider from 0.0f to 1.0f

            if (ImGui.Button("Incrementador")) // Buttons return true when clicked
                counter++;
            ImGui.SameLine();
            ImGui.Text($"contador = {counter}");

            ImGui.Text($"Promedio de frames {1000.0f / io.Framerate:F3} ms/frame ({io.Framerate:F1} FPS)");

            if (ImGui.BeginPopupContextWindow())
            {
                if (ImGui.MenuItem("Personalizado", null, location == -1)) location = -1;
                if (ImGui.MenuItem("Centro", null, location == -2)) location = -2;
                if (ImGui.MenuItem("Arriba-izquierda", null, location == 0)) location = 0;
                if (ImGui.MenuItem("Arriba-derecha", null, location == 1)) location = 1;
                if (ImGui.MenuItem("Abajo-izquierda", null, location == 2)) location = 2;
                if (ImGui.MenuItem("Abajo-derecha", null, location == 3)) location = 3;
                ImGui.EndPopup();
            }

            ImGui.End();
        }
    }
}
