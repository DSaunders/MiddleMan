namespace MiddleMan.Pipeline
{
    using System.Threading.Tasks;

    public interface IPipelineTask
    {
        
    }

    public interface IPipelineTask<in TPipelineMessage> : IPipelineTask where TPipelineMessage : class, IPipelineMessage
    {
        Task Run(TPipelineMessage message);
    }
}
