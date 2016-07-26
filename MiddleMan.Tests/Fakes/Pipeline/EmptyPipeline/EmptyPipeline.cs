namespace MiddleMan.Tests.Fakes.Pipeline.EmptyPipeline
{
    using MiddleMan.Pipeline;
    using MiddleMan.Pipeline.Builder;

    public class EmptyPipeline : IPipeline<EmptyPipelineMessage>
    {
        public void GetPipelineTasks(PipelineBuilder<EmptyPipelineMessage> builder)
        {
            
        }
    }
}