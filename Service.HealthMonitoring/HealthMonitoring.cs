namespace MeetEric
{
    using System.Fabric;
    using System.Threading.Tasks;
    using MeetEric.Services;
    using Workers;

    internal sealed class HealthMonitoring : MeetEricStatelessWorker
    {
        public HealthMonitoring(StatelessServiceContext context)
            : base(context)
        { }

        protected override Task<IWorkerRole> CreateWorker()
        {
            IWorkerRole worker = new MachineHealthWorker(this.Log);
            return Task.FromResult(worker);
        }
    }
}
