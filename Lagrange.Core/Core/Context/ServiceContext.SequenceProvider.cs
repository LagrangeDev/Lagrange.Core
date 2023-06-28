using System.Collections.Concurrent;

namespace Lagrange.Core.Core.Context;

internal partial class ServiceContext
{
    private class SequenceProvider
    {
        private readonly ConcurrentDictionary<string, int> _sessionSequence;
        
        private int _sequence;

        public SequenceProvider()
        {
            _sessionSequence = new ConcurrentDictionary<string, int>();
            _sequence = Random.Shared.Next(5000000, 9900000);
        }

        public int GetNewSequence()
        {
            Interlocked.CompareExchange(ref _sequence, 5000000, 9900000);
            return Interlocked.Increment(ref _sequence);
        }
        
        public int RegisterSession(string sessionId) => 
            _sessionSequence.GetOrAdd(sessionId, _ => GetNewSequence());
    }
}