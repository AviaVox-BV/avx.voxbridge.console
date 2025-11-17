using Microsoft.AspNetCore.SignalR.Client;
﻿namespace SignalRConsoleClient.Services
{
    public class HealthCheckService
    {
        private readonly SignalRService _signalRService;
        private readonly AuthService _authService;

        public HealthCheckService(SignalRService signalRService, AuthService authService)
        {
            _signalRService = signalRService;
            _authService = authService;
        }

        public async Task<bool> CheckConnectionAsync()
        {
            try
            {
                var token = await _authService.GetTokenAsync();
                await _signalRService.ConnectAsync(token);

                return _signalRService.State == HubConnectionState.Connected;
            }
            catch
            {
                return false;
            }
            finally
            {
                await _signalRService.DisconnectAsync();
            }
        }
    }
}
