namespace MiddleMan.Pipeline
{
    using Builder;

    public interface IPipelineAsync<TPipelineMessage> : IPipeline
        where TPipelineMessage : class, IPipelineMessage
    {
        void GetPipelineTasks(PipelineBuilderAsync<TPipelineMessage> builder);
    }
}