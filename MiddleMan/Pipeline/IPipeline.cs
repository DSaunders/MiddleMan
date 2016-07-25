namespace MiddleMan.Pipeline
{
    using Builder;

    public interface IPipeline { }

    public interface IPipeline<TPipelineMessage> : IPipeline
        where TPipelineMessage : class, IPipelineMessage
    {
        void GetPipelineTasks(PipelineBuilder<TPipelineMessage> builder);
    }
}