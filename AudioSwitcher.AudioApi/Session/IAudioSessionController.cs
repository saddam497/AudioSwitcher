using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AudioSwitcher.AudioApi.Session
{
    public interface IAudioSessionController : IDisposable
    {

        IEnumerable<IAudioSession> All();
        Task<IEnumerable<IAudioSession>> AllAsync();

        IEnumerable<IAudioSession> GetActiveSessions();
        Task<IEnumerable<IAudioSession>> GetActiveSessionsAsync();

        IEnumerable<IAudioSession> GetInactiveSessions();
        Task<IEnumerable<IAudioSession>> GetInactiveSessionsAsync();

        IEnumerable<IAudioSession> GetExpiredSessions();
        Task<IEnumerable<IAudioSession>> GetExpiredSessionsAsync();
    }
}