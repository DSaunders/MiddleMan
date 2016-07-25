namespace MiddleMan.Tests.Pipeline.MultiplePipelines
{
    using MiddleMan.Pipeline;
    using MiddleMan.Pipeline.Builder;

    public class MultiplePipelineMessagePipelineAsync2 : IPipelineAsync<MultiplePipelineMessage>
    {
        public void GetPipelineTasks(PipelineBuilderAsync<MultiplePipelineMessage> builder)
        {

        }
    }
}