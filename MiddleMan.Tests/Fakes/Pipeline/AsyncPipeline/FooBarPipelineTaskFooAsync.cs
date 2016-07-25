namespace MiddleMan.Tests.Fakes.Pipeline.AsyncPipeline
{
    using System.Threading.Tasks;
    using MiddleMan.Pipeline.Tasks;

    public class FooBarPipelineTaskFooAsync : PipelineTaskBaseAsync<FooBarPipelineMessage>
    {
        public override async Task Run(FooBarPipelineMessage message)
        {
            message.TasksRun.Add("Foo");
            await Next(message);
        }
    }
}