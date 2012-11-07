using System;
using System.Collections.Generic;

namespace Omikron.FactFinder
{
    public interface IConfiguration
    {
        bool IsDebugModeOn { get; }

        string FactFinderVersion { get; }
        
        string RequestProtocol { get; }
        string ServerAddress { get; }
        string ServerPort { get; }
        string Context { get; }
        string Channel { get; }
        string Language { get; }
        
        string User { get; }
        string Password { get; }
        AuthenticationType AuthenticationType { get; }
        string AdvancedAuthPrefix { get; }
        string AdvancedAuthPostfix { get; }

        IDictionary<string, string> PageMappings { get; }
        IDictionary<string, string> ServerMappings { get; }
        ICollection<string> IgnoredPageParams { get; }
        ICollection<string> IgnoredServerParams { get; }
        IDictionary<string, string> RequiredPageParams { get; }
        IDictionary<string, string> RequiredServerParams { get; }
        
        string PageContentEncoding { get; }
        string PageUrlEncoding { get; }
        string ServerUrlEncoding { get; }

        string GetCustomValue(string key);
    }
}
