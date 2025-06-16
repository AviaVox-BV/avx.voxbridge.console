namespace SignalRConsoleClient.Services
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
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
