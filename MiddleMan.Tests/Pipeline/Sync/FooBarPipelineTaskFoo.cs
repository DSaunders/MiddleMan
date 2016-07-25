namespace MiddleMan.Tests.Pipeline.Sync
{
    using MiddleMan.Pipeline;
    using MiddleMan.Pipeline.Tasks;

    public class FooBarPipelineTaskFoo : PipelineTaskBase<FooBarPipelineMessage>
    {
        public override void Run(FooBarPipelineMessage message)
        {
            message.TasksRun.Add("Foo");
            Next(message);
        }
    }
}