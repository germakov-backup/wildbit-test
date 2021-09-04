using System.Threading.Tasks;

namespace EmailSender.Abstractions.Handlers
{
    public interface IPipelineFilter<in TInput, in TConfig>
    {
        Task Handle(TInput input, TConfig config);

        string FilterName { get; }
    }
}
