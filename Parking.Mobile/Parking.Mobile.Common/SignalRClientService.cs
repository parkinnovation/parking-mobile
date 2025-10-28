using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Parking.Mobile.Common
{
    public class SignalRClientService
    {
        private readonly string _hubUrl;
        private HubConnection _connection;
        private readonly SemaphoreSlim _connectionLock = new SemaphoreSlim(1, 1);

        private bool _isConnecting;
        private bool _shouldReconnect = true;

        public bool IsConnected => _connection?.State == HubConnectionState.Connected;

        public event Action<string, object> OnServerMessage;
        public event Action OnConnected;
        public event Action OnDisconnected;

        public SignalRClientService(string hubUrl)
        {
            _hubUrl = hubUrl;
        }

        public async Task StartAsync()
        {
            if (_isConnecting || IsConnected)
                return;

            _isConnecting = true;
            await _connectionLock.WaitAsync();

            try
            {
                if (_connection == null)
                {
                    _connection = new HubConnectionBuilder()
                        .WithUrl(_hubUrl)
                        .Build();

                    _connection.Closed += async (error) =>
                    {
                        Console.WriteLine("⚠️ Conexão com SignalR perdida.");
                        OnDisconnected?.Invoke();
                        await HandleReconnectAsync();
                    };

                    _connection.On<string, object>("ReceiveMessage", (method, data) =>
                    {
                        OnServerMessage?.Invoke(method, data);
                    });
                }

                await _connection.StartAsync();
                Console.WriteLine("✅ Conectado ao SignalR Hub");
                OnConnected?.Invoke();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Erro ao conectar ao SignalR: {ex.Message}");
                await HandleReconnectAsync();
            }
            finally
            {
                _connectionLock.Release();
                _isConnecting = false;
            }
        }

        private async Task HandleReconnectAsync()
        {
            if (!_shouldReconnect)
                return;

            int attempt = 0;
            while (_shouldReconnect && !IsConnected)
            {
                attempt++;
                int delay = Math.Min(30000, 2000 * attempt); // cresce até 30s

                Console.WriteLine($"🔁 Tentando reconectar (tentativa {attempt}) em {delay / 1000}s...");

                await Task.Delay(delay);

                try
                {
                    await _connection.StartAsync();
                    Console.WriteLine("✅ Reconectado ao SignalR com sucesso.");
                    OnConnected?.Invoke();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"⚠️ Falha ao reconectar: {ex.Message}");
                }
            }
        }

        public async Task StopAsync()
        {
            _shouldReconnect = false;
            if (_connection != null)
            {
                await _connection.StopAsync();
                Console.WriteLine("🛑 Conexão com SignalR encerrada manualmente.");
            }
        }

        public async Task<T> SendMessageAsync<T>(string method, params object[] args)
        {
            if (!IsConnected)
            {
                Console.WriteLine("⚠️ Não é possível enviar mensagem — desconectado do SignalR.");
                throw new Exception($"Sem comunicação com o servidor");
            }

            try
            {
                var result = await _connection.InvokeCoreAsync<T>(
                    method,
                    args
                );

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Erro ao enviar mensagem SignalR: {ex.Message}");
                throw new Exception(ex.Message);
            }
        }

    }
}
