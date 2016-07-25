﻿namespace MiddleMan.Tests.Pipeline.Sync
{
    using MiddleMan.Pipeline;
    using MiddleMan.Pipeline.Builder;

    public class FooBarPipeline : IPipeline<FooBarPipelineMessage>
    { 
        public void GetPipelineTasks(PipelineBuilder<FooBarPipelineMessage> builder)
        {
            builder.Add<FooBarPipelineTaskFoo>();
            builder.Add<FooBarPipelineTaskBar>();
        }
    }
}