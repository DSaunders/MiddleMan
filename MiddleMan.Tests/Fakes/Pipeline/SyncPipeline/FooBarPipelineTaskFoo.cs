namespace MiddleMan.Tests.Fakes.Pipeline.SyncPipeline
{
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