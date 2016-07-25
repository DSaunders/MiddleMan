namespace MiddleMan.Pipeline.Tasks
{
    using System;

    public abstract class PipelineTaskBase<TPipelineMessage> : IPipelineTask<TPipelineMessage> 
        where TPipelineMessage : class, IPipelineMessage
    {
        private Action<TPipelineMessage> _next;

        public void Setup(Action<TPipelineMessage> nextTask)
        {
            _next = nextTask;
        }

        public abstract void Run(TPipelineMessage message);
        
        protected void Next(TPipelineMessage message)
        {
            _next.Invoke(message);
        }
    }
}