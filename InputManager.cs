using System.Collections.Generic;
using Stride.Core.Mathematics;
using Stride.Engine;
using Stride.Input;

namespace MotorDeFisica.SistemaDeEntrada
{
    /// <summary>
    /// Classe responsável pela gestão de inputs do utilizador,
    /// usando a API nativa do Stride (Stride.Input) para teclado, rato e gamepads.
    /// Herda de SyncScript, para poder ser anexada a uma entidade no Stride.
    /// </summary>
    public class InputManager : SyncScript
    {
        // Exemplo de mapeamento para teclas
        private Dictionary<string, List<Keys>> keyMappings;

        // Opcional: mapeamento para botões de gamepad
        private Dictionary<string, List<GamePadButton>> gamepadMappings;

        public InputManager()
        {
            // Exemplo de configurações iniciais.
            // Podes carregar isto de um ficheiro JSON (InputMappings.json)
            keyMappings = new Dictionary<string, List<Keys>>
            {
                { "MoveLeft",  new List<Keys> { Keys.A, Keys.Left } },
                { "MoveRight", new List<Keys> { Keys.D, Keys.Right } },
                { "Jump",      new List<Keys> { Keys.Space } }
            };

            gamepadMappings = new Dictionary<string, List<GamePadButton>>
            {
                { "Jump", new List<GamePadButton> { GamePadButton.A } },
                { "Fire", new List<GamePadButton> { GamePadButton.RightShoulder } }
            };
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
                string acao = mapping.Key;
                var teclas = mapping.Value;
                foreach (var tecla in teclas)
                {
                    if (keyboard.IsKeyPressed(tecla))
                    {
                        DispararAcao(acao);
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

            // Exemplos:
            // Se quiseres detetar clique esquerdo:
            // if (mouse.IsButtonPressed(MouseButton.Left)) { DispararAcao("Fire"); }

            // Se quiseres ler a posição do rato:
            // var pos = mouse.Position;
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
                    string acao = mapping.Key;
                    var botoes = mapping.Value;
                    foreach (var botao in botoes)
                    {
                        if (gamepad.IsButtonPressed(botao))
                        {
                            DispararAcao(acao);
                            break;
                        }
                    }
                }

                // Se quiseres ler os eixos analógicos, ex.:
                // var leftStick = gamepad.LeftThumbAxis;
            }
        }

        private void DispararAcao(string acao)
        {
            // Exemplo de debug no ecrã
            DebugText.Print($"Ação detectada: {acao}", new Int2(10, 10));

            // Aqui poderias chamar um método do Controller, p.ex.:
            // Controller.ProcessarAcao(acao);
        }
    }
}
