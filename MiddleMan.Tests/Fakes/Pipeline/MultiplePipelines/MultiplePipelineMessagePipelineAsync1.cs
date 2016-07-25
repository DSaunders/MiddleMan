namespace MiddleMan.Tests.Fakes.Pipeline.MultiplePipelines
{
    using MiddleMan.Pipeline;
    using MiddleMan.Pipeline.Builder;

    public class MultiplePipelineMessagePipelineAsync1 : IPipelineAsync<MultiplePipelineMessage>
    {
        public void GetPipelineTasks(PipelineBuilderAsync<MultiplePipelineMessage> builder)
        {

        }
    }
}