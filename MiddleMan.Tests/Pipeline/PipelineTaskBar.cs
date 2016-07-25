namespace MiddleMan.Tests.Pipeline
{
    using System.Threading.Tasks;
    using MiddleMan.Pipeline;

    public class PipelineTaskBar : IPipelineTask<PipelineMessage>
    {
        public async Task Run(PipelineMessage message)
        {
            message.TasksRun.Add("Bar");
        }
    }
}