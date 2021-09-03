namespace EmailSender.Config
{
    public class TasksPollTriggerOptions
    {
        public const string Key = "TasksPollTrigger";

        public int DelaySeconds { get; set; }

        public int BatchSize { get; set; }
    }
}
