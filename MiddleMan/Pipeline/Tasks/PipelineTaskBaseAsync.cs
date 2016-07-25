namespace MiddleMan.Pipeline.Tasks
{
    using System;
    using System.Threading.Tasks;

    public abstract class PipelineTaskBaseAsync<TPipelineMessage> : IPipelineTaskAsync<TPipelineMessage>
        where TPipelineMessage : class, IPipelineMessage
    {
        private Func<TPipelineMessage, Task> _next;

        public void Setup(Func<TPipelineMessage, Task> nextTask)
        {
            _next = nextTask;
        }

        public abstract Task Run(TPipelineMessage message);

        protected async Task Next(TPipelineMessage message)
        {
            await _next.Invoke(message).ConfigureAwait(false);
        }
    }
}