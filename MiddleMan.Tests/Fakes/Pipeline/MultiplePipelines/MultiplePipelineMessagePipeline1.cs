namespace MiddleMan.Tests.Fakes.Pipeline.MultiplePipelines
{
    using MiddleMan.Pipeline;
    using MiddleMan.Pipeline.Builder;

    public class MultiplePipelineMessagePipeline1 : IPipeline<MultiplePipelineMessage>
    {
        public void GetPipelineTasks(PipelineBuilder<MultiplePipelineMessage> builder)
        {
            
        }
    }
}