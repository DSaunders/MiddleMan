namespace MiddleMan.Pipeline.Builder
{
    using System;
    using System.Collections.Generic;
    using Tasks;

    public class PipelineBuilder<TPipelineMessage> 
        where TPipelineMessage : class, IPipelineMessage
    {
        private readonly List<Type> _pipeline;

        public PipelineBuilder()
        {
            _pipeline = new List<Type>();
        }

        public void Add<T>() where T : class, IPipelineTask<TPipelineMessage>
        {
            _pipeline.Add(typeof(T));
        }

        internal IEnumerable<Type> Get()
        {
            return _pipeline;
        }
    }
}