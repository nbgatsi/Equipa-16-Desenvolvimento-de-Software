using System.Collections.Generic;
using System.Linq;
using Stride.Core.Mathematics;
using Stride.Engine;
using Stride.Input;
using Stride.Content;

namespace MotorDeFisica.SistemaDeEntrada
{
    /// <summary>
    /// Classe responsável pela gestão de inputs do utilizador,
    /// usando a API nativa do Stride (Stride.Input) para teclado, rato e gamepads.
    /// Herda de SyncScript, para poder ser anexada a uma entidade no Stride.
    /// </summary>
    public class InputManager : SyncScript
    {
        private Dictionary<string, List<Keys>> keyMappings;
        private Dictionary<string, List<GamePadButton>> gamepadMappings;

        public override void Start()
        {
            // Carregamento do asset InputMappings gerado a partir do JSON
            var asset = Content.Load<InputMappingsAsset>("InputMappings");

            keyMappings = asset.Actions
                .ToDictionary(
                    action => action.Name,
                    action => action.Gestures
                        .Where(g => g.HasKey)
                        .Select(g => g.Key)
                        .ToList());

            gamepadMappings = asset.Actions
                .ToDictionary(
                    action => action.Name,
                    action => action.Gestures
                        .Where(g => g.HasGamepadButton)
                        .Select(g => g.GamepadButton)
                        .ToList());
        }

        public override void Update()
        {
            CapturarTeclado();
            CapturarRato();
            CapturarGamepad();
        }

        private void CapturarTeclado()
        {
            var keyboard = Input?.Keyboard;
            if (keyboard == null)
                return;

            foreach (var mapping in keyMappings)
            {
                foreach (var tecla in mapping.Value)
                {
                    if (keyboard.IsKeyPressed(tecla))
                    {
                        DispararAcao(mapping.Key);
                        break;
                    }
                }
            }
        }

        private void CapturarRato()
        {
            var mouse = Input?.Mouse;
            if (mouse == null)
                return;

            if (mouse.IsButtonPressed(MouseButton.Left))
            {
                DispararAcao("Fire");
            }
        }

        private void CapturarGamepad()
        {
            for (int i = 0; i < Input.GamePadCount; i++)
            {
                var gamepad = Input.GetGamePad(i);
                if (gamepad == null)
                    continue;

                foreach (var mapping in gamepadMappings)
                {
                    foreach (var botao in mapping.Value)
                    {
                        if (gamepad.IsButtonPressed(botao))
                        {
                            DispararAcao(mapping.Key);
                            break;
                        }
                    }
                }
            }
        }

        private void DispararAcao(string acao)
        {
            DebugText.Print($"Ação detectada: {acao}", new Int2(10, 10));
            // Controller.ProcessarAcao(acao);
        }
    }
}
