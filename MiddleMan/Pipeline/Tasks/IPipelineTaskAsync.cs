namespace MiddleMan.Pipeline.Tasks
{
    using System;
    using System.Threading.Tasks;

    public interface IPipelineTaskAsync<TPipelineMessage> : IPipelineTask
        where TPipelineMessage : class, IPipelineMessage
    {
        void Setup(Func<TPipelineMessage, Task> nextTask);
        Task Run(TPipelineMessage message);
    }
}