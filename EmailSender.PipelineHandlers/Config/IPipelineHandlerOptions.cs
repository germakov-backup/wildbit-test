using System.Collections.Generic;

namespace EmailSender.PipelineHandlers.Config
{
    public interface IPipelineHandlerOptions
    {
        ICollection<string> InputChannels { get; }
    }
}
