﻿namespace MiddleMan.Tests.Fakes.Pipeline.AsyncPipeline
{
    using System.Threading.Tasks;
    using MiddleMan.Pipeline.Tasks;

    public class FooBarPipelineTaskBarAsync : PipelineTaskBaseAsync<FooBarPipelineMessage>
    {
        public override async Task Run(FooBarPipelineMessage message)
        {
            message.TasksRun.Add("Bar");
            await Next(message);
        }
    }
}