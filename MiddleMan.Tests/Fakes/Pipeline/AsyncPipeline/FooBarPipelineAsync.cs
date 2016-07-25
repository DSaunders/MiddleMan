namespace MiddleMan.Tests.Fakes.Pipeline.AsyncPipeline
{
    using MiddleMan.Pipeline;
    using MiddleMan.Pipeline.Builder;

    public class FooBarPipelineAsync : IPipelineAsync<FooBarPipelineMessage>
    {
        public void GetPipelineTasks(PipelineBuilderAsync<FooBarPipelineMessage> builder)
        {
            builder.Add<FooBarPipelineTaskFooAsync>();
            builder.Add<FooBarPipelineTaskBarAsync>();
        }
    }
}