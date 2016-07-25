namespace MiddleMan.Tests.Pipeline.MultiplePipelines
{
    using MiddleMan.Pipeline;
    using MiddleMan.Pipeline.Builder;

    public class MultiplePipelineMessagePipeline2 : IPipeline<MultiplePipelineMessage>
    {
        public void GetPipelineTasks(PipelineBuilder<MultiplePipelineMessage> builder)
        {

        }
    }
}