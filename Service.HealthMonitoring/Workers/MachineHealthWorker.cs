namespace MeetEric.Workers
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;
    using Common;
    using Diagnostics;
    using Security;
    using Services;
    using Watchdogs;

    public class MachineHealthWorker : WorkerBase
    {
        public MachineHealthWorker(ILoggingContext logger)
            : base(logger)
        {
            var idFactory = MeetEricFactory.GetService<IIdentityFactory>();

            Watchdogs = new List<IWatchdog>
            {
                new HardDriveWatchdog(idFactory.Parse("SystemDrive"), ReadDrive(Environment.SystemDirectory), "System Drive"),
                new HardDriveWatchdog(idFactory.Parse("DeploymentDrive"), ReadDrive(Assembly.GetExecutingAssembly().Location), "Deployment Drive"),
            }.AsReadOnly();
        }

        private IList<IWatchdog> Watchdogs { get; }

        protected override async Task RunInternal(CancellationToken cancelled)
        {
            var context = MeetEricFactory.GetService<IWatchdogLoggingFactory>().CreateWatchdogContext();

            while (!cancelled.WaitHandle.WaitOne(TimeSpan.FromSeconds(3)))
            {
                foreach (var watchDog in Watchdogs)
                {
                    await watchDog.Examine(context);
                }
            }
        }

        private static DriveInfo ReadDrive(string path)
        {
            return new DriveInfo(Path.GetPathRoot(path));
        }
    }
}

