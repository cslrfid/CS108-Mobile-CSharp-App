using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CSLibrary.Net
{
	public class NetFinder
	{
		/// <summary>
		/// Netfinder information return from device
		/// </summary>
		public class DeviceInfomation
		{
			/// <summary>
			/// Device name, user can change it.
			/// </summary>
			public string DeviceName;

			public Guid UUID;

			public int RSSI;
		}

		/// <summary>
		/// DeviceFinder Argument
		/// </summary>
		public class DeviceFinderArgs : EventArgs
		{
			private DeviceInfomation _data;
			/// <summary>
			/// Device Finder 
			/// </summary>
			/// <param name="data"></param>
			public DeviceFinderArgs(DeviceInfomation data)
			{
				_data = data;
			}
			/// <summary>
			/// Device finder information
			/// </summary>
			public DeviceInfomation Found
			{
				get { return _data; }
				set { _data = value; }
			}
		}

		/// <summary>
		/// Result
		/// </summary>
		public enum AssignResult
		{
			/// <summary>
			/// Accepted from device
			/// </summary>
			ACCEPTED,
			/// <summary>
			/// Rejected from device
			/// </summary>
			REJECTED,
			/// <summary>
			/// response timeout
			/// </summary>
			TIMEOUT,
			/// <summary>
			/// assignment started, please wait to finished
			/// </summary>
			STARTED,
			/// <summary>
			/// Unknown
			/// </summary>
			UNKNOWN,
		}
		
		/// <summary>
		/// Result Argument
		/// </summary>
		public class ResultArgs : EventArgs
		{
			private AssignResult m_Result;
			/// <summary>
			/// Constructor
			/// </summary>
			/// <param name="Result"></param>
			public ResultArgs(AssignResult Result)
			{
				m_Result = Result;
			}
			/// <summary>
			/// Result
			/// </summary>
			public AssignResult Result
			{
				get { return m_Result; }
				set { m_Result = value; }
			}
		}

		/// <summary>
		/// Search device callback event
		/// </summary>
		public event EventHandler<DeviceFinderArgs> OnSearchCompleted;
		/// <summary>
		/// Assign device callback event
		/// </summary>
		public event EventHandler<ResultArgs> OnAssignCompleted;

		protected readonly Plugin.BLE.Abstractions.Contracts.IAdapter Adapter;

		public void SearchDevice()
		{
		}

		public void Stop()
		{

		}

		private void RaiseEvent<T>(EventHandler<T> eventHandler, object sender, T e)
			where T : EventArgs
		{
			if (eventHandler != null)
			{
				eventHandler(sender, e);
			}
			return;
		}

		private void OnDeviceDiscovered(object sender, Plugin.BLE.Abstractions.EventArgs.DeviceEventArgs args)
		{
			DeviceInfomation entry = new DeviceInfomation();

			entry.DeviceName = args.Device.Name;
			entry.UUID = args.Device.Id;
			entry.RSSI = args.Device.Rssi;

			RaiseEvent<DeviceFinderArgs>(OnSearchCompleted, this, new DeviceFinderArgs(entry));
		}




	}
}
