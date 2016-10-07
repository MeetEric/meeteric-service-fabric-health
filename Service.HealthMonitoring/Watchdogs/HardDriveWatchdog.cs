namespace MeetEric.Watchdogs
{
    using System.IO;
    using System.Threading.Tasks;
    using Diagnostics;
    using Examiners;
    using Security;

    public class HardDriveWatchdog : IWatchdog
        {
            public HardDriveWatchdog(IIdentifier id, DriveInfo drive, string driveName)
            {
                Drive = drive;
                DriveName = driveName;
                ErrorLevel = 90;
                WarningLevel = 85;
                Id = id;
            }

            public IIdentifier Id { get; }

            private long ErrorLevel { get; }

            private long WarningLevel { get; }

            private string DriveName { get; }

            private DriveInfo Drive { get; }

            public async Task Examine(IWatchdogContext context)
            {
                var result = HardDriveExaminer.ExamimeDrive(Drive, ErrorLevel, WarningLevel);

                var freePercent = (Drive.TotalFreeSpace * 100) / Drive.TotalSize;
                var message = $"{DriveName} drive ({Drive.Name}) has {freePercent}% of free space";

                switch (result)
                {
                    case HardDriveExaminer.Result.Error:
                        await context.Node.ReportError(Id, message);
                        break;
                    case HardDriveExaminer.Result.Warning:
                        await context.Node.ReportWarning(Id, message);
                        break;
                    default:
                        await context.Node.ReportInformation(Id, message);
                        break;
                }
            }
        }
    }
