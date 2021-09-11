using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Incapsulation.Failures
{
    public enum FailureType
    {
        UnexpectedShutdown,
        ShortNonResponding,
        HardwareFailures,
        ConnectionProblems
    }

    public class ReportMaker
    {
        /// <summary>
        /// </summary>
        /// <param name="day"></param>
        /// <param name="failureTypes">
        /// 0 for unexpected shutdown, 
        /// 1 for short non-responding, 
        /// 2 for hardware failures, 
        /// 3 for connection problems
        /// </param>
        /// <param name="deviceId"></param>
        /// <param name="times"></param>
        /// <param name="devices"></param>
        /// <returns></returns>
        public static List<string> FindDevicesFailedBeforeDateObsolete(
            int day,
            int month,
            int year,
            int[] failureTypes, 
            int[] deviceId, 
            object[][] times,
            List<Dictionary<string, object>> devices)
        {
            var dateBeforeWhich = new DateTime(year, month, day);
            var listOfDeviceFailuresWithDates = new List<DeviceFailureWithDate>();
            if ((failureTypes.Length == deviceId.Length) && (failureTypes.Length == times.GetLength(0)))
            {
                for (int i = 0; i < failureTypes.Length; i++)
                {
                    var device = new Device(deviceId[i], (string)devices[i]["Name"]);
                    var failure = new Failure(failureTypes[i]);
                    var dateOfFailure = new DateTime((int)times[i][2], (int)times[i][1], (int)times[i][0]);
                    listOfDeviceFailuresWithDates.Add(new DeviceFailureWithDate(device, failure, dateOfFailure));
                }
            }
            else throw new ArgumentException("Array sizes don't match");
            return FindDevicesFailedBeforeDate(dateBeforeWhich, listOfDeviceFailuresWithDates);
        }

        public static List<string> FindDevicesFailedBeforeDate(DateTime date, List<DeviceFailureWithDate> listOfDeviceFailuresWithDates)
        {
            var result = new List<string>();
            foreach (var deviceFailure in listOfDeviceFailuresWithDates)
            {
                if (Failure.IsFailureSerious((FailureType)deviceFailure.Failure.FailureType) 
                    && DateTime.Compare(deviceFailure.date, date) < 0)
                {
                    result.Add(deviceFailure.Device.Name);
                }
            }
            return result;
        }      
    }

    public class DeviceFailureWithDate
    {
        Device device;
        public Device Device
        {
            get { return device; }
            set { if (value == null) throw new ArgumentNullException(); else device = value; }
        }

        Failure failure;
        public Failure Failure
        {
            get { return failure; }
            set { if (value == null) throw new ArgumentNullException(); else failure = value; }
        }
        public readonly DateTime date;

        public DeviceFailureWithDate(Device device, Failure failure, DateTime date)
        {
            Device = device;
            Failure = failure;
            this.date = date;
        }
    }

    public class Failure
    {
        private FailureType failureType; 
        public int FailureType 
        { 
            get 
            {
                return (int)failureType; 
            }

            set
            {
                if (value > 3 || value < 0)
                    throw new ArgumentException("Incorrect Failure Type");
                else
                    failureType = (FailureType)value;
            } 
        }

        public Failure(int failureType)
        {
            FailureType = failureType;
        }

        public static bool IsFailureSerious(FailureType failureType)
        {
            return (int)failureType == 0 || (int)failureType == 2;
        }
    }

    public class Device
    {
        private int deviceId;
        public int DeviceId { get; set; }

        private string name;
        public string Name { get; set; }

        public Device(int id, string name)
        {
            DeviceId = id;
            Name = name;
        }
    }
}
