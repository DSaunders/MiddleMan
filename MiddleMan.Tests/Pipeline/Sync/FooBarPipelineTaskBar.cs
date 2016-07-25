namespace MiddleMan.Tests.Pipeline.Sync
{
    using MiddleMan.Pipeline;
    using MiddleMan.Pipeline.Tasks;

    public class FooBarPipelineTaskBar : PipelineTaskBase<FooBarPipelineMessage>
    {
        public override void Run(FooBarPipelineMessage message)
        {
            message.TasksRun.Add("Bar");
            Next(message);
        }
    }
}