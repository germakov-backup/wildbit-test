namespace EmailSender.PipelineHandlers.Config
{
    public class PriorityScoreRouterOptions
    {
        public int PriorityThreshold { get; set; }

        public string PriorityAddress { get; set; }

        public string RegularAddress { get; set; }
    }
}
