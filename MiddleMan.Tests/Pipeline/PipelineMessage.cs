namespace MiddleMan.Tests.Pipeline
{
    using System.Collections.Generic;
    using MiddleMan.Pipeline;

    public class PipelineMessage : IPipelineMessage
    {
        public IList<string> TasksRun { get; set; }

        public PipelineMessage()
        {
            TasksRun = new List<string>();
        }
    }
}
