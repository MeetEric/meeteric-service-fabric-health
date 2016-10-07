namespace MeetEric.Examiners
{
        using System;
        using System.Collections.Generic;
        using System.IO;
        using System.Linq;
        using System.Text;
        using System.Threading.Tasks;
        using Security;

        public class HardDriveExaminer
        {
            public enum Result
            {
                Ok,
                Warning,
                Error
            }

            /// <summary>
            /// Examines the hard drive and reports Error, Warning or Ok
            /// </summary>
            /// <param name="drive"></param>
            /// <param name="errorPercent">The percent of disk spaced used. For example 80
            /// will return an error when over 80% has been consumed.</param>
            /// <param name="warningPercent">The percent of disk spaced used. For example 80
            /// will return a warning when over 80% has been consumed.</param>
            /// <returns></returns>
            public static Result ExamimeDrive(DriveInfo drive, long errorPercent, long warningPercent)
            {
                var result = Result.Ok;
                var baseSize = drive.TotalSize / 100;
                var errorLimit = baseSize * (100 - errorPercent);
                var warningLimit = baseSize * (100 - warningPercent);

                if (drive.AvailableFreeSpace <= errorLimit)
                {
                    result = Result.Error;
                }
                else if (drive.AvailableFreeSpace <= warningLimit)
                {
                    result = Result.Warning;
                }

                return result;
            }
        }
    }
