namespace MiddleMan.Pipeline
{
    using System;
    using System.Collections.Generic;

    public class PipelineBuilder
    {
        protected List<Type> Pipeline;

        internal List<Type> GetPipelineTaskTypesInOrder()
        {
            return Pipeline;
        }
    }

    public class PipelineBuilder<TPipelineMessage> : PipelineBuilder 
        where TPipelineMessage : class, IPipelineMessage
    {
        public PipelineBuilder()
        {
            Pipeline = new List<Type>();
        }

        public void Add<T>() where T: IPipelineTask<TPipelineMessage>
        {
            Pipeline.Add(typeof(T));
        }
    }
}