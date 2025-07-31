using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComprehensivePlayrightAuto.Infra.AiService.AiAgent
{

        public class SessionMemoryManager
        {
            private readonly Dictionary<string, List<dynamic>> _sessions = new();
            private string _currentSessionId = string.Empty;

            public string GetCurrentSessionId()
            {
                if (string.IsNullOrEmpty(_currentSessionId))
                {
                    _currentSessionId = Guid.NewGuid().ToString();
                }
                return _currentSessionId;
            }

            public string StartNewSession()
            {
                _currentSessionId = Guid.NewGuid().ToString();
                _sessions[_currentSessionId] = new List<dynamic>();
                return _currentSessionId;
            }

            public List<dynamic> GetCurrentSessionHistory()
            {
                string sessionId = GetCurrentSessionId();
                if (!_sessions.ContainsKey(sessionId))
                {
                    _sessions[sessionId] = new List<dynamic>();
                }
                return _sessions[sessionId];
            }

            public void AppendToCurrent(string role, string content)
            {
                GetCurrentSessionHistory().Add(new { role, content });
            }

        public List<dynamic> GetHistory(string sessionId)
            {
                if (!_sessions.ContainsKey(sessionId))
                {
                    _sessions[sessionId] = new List<dynamic>();
                }
                return _sessions[sessionId];
            }

            public void Append(string sessionId, string role, string content)
            {
                GetHistory(sessionId).Add(new { role, content });
            }
        }

    
}
