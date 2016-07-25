namespace MiddleMan.Pipeline.Tasks
{
    using System;

    public interface IPipelineTask
    {
        
    }

    public interface IPipelineTask<TPipelineMessage> : IPipelineTask 
        where TPipelineMessage : class, IPipelineMessage
    {
        void Setup(Action<TPipelineMessage> nextTask);
        void Run(TPipelineMessage message);
    }
}
