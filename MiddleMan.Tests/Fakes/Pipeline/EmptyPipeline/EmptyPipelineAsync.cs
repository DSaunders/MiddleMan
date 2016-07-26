namespace MiddleMan.Tests.Fakes.Pipeline.EmptyPipeline
{
    using MiddleMan.Pipeline;
    using MiddleMan.Pipeline.Builder;

    public class EmptyPipelineAsync : IPipelineAsync<EmptyPipelineMessage>
    {
        public void GetPipelineTasks(PipelineBuilderAsync<EmptyPipelineMessage> builder)
        {
     
        }
    }
}