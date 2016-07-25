namespace MiddleMan.Tests.Pipeline
{
    using System.Threading.Tasks;
    using MiddleMan.Pipeline;

    public class PipelineTaskFoo : IPipelineTask<PipelineMessage>
    {
        public async Task Run(PipelineMessage message)
        {
            message.TasksRun.Add("Foo");
        }
    }
}