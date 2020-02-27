using System.Threading.Tasks;
using SharpenUp.Common.Models.Accounts;
using SharpenUp.Common.Models.Alerts;
using SharpenUp.Common.Models.MaintenanceWindows;
using SharpenUp.Common.Models.Monitors;
using SharpenUp.Common.Models.PublicStatusPages;

namespace SharpenUp.Common
{
    public interface IUptimeManager
    {
        /// <summary>
        /// Account details (max number of monitors that can be added and number of up/down/paused monitors) can be grabbed using this method.
        /// </summary>
        Task<AccountDetailsResult> GetAccountDetailsAsync();

        #region Alert Contacts

        /// <summary>
        /// The list of alert contacts can be called with this method.
        /// </summary>
        Task<AlertContactsResult> GetAlertContactsAsync();

        #endregion

        #region Monitors

        /// <summary>
        /// This is a Swiss-Army knife type of a method for getting any information on monitors.
        /// By default, it lists all the monitors in a user's account, their friendly names, types (http, keyword, port, etc.), statuses (up, down, etc.) and uptime ratios.
        /// There are optional parameters which lets the getMonitors method to output information on any given monitors rather than all of them.
        /// And also, parameters exist for getting the notification logs (alerts) for each monitor and even which alert contacts were alerted on each notification.
        /// </summary>
        Task<MonitorsResult> GetMonitorsAsync();

        /// <summary>
        /// This is a Swiss-Army knife type of a method for getting any information on monitors.
        /// By default, it lists all the monitors in a user's account, their friendly names, types (http, keyword, port, etc.), statuses (up, down, etc.) and uptime ratios.
        /// There are optional parameters which lets the getMonitors method to output information on any given monitors rather than all of them.
        /// And also, parameters exist for getting the notification logs (alerts) for each monitor and even which alert contacts were alerted on each notification.
        /// </summary>
        /// <param name="request">A Monitors Request object.</param>
        Task<MonitorsResult> GetMonitorsAsync( MonitorsRequest request );

        #endregion

        #region Maintenance Windows

        /// <summary>
        /// The list of maintenance windows can be called with this method.
        /// </summary>
        Task<MaintenanceWindowsResult> GetMaintenanceWindowsAsync();

        /// <summary>
        /// The list of maintenance windows can be called with this method.
        /// </summary>
        /// <param name="request">Maintenance Windows Request object</param>
        Task<MaintenanceWindowsResult> GetMaintenanceWindowsAsync( MaintenanceWindowsRequest request );

        #endregion

        #region Public Status Pages

        /// <summary>
        /// The list of public status pages can be called with this method.
        /// </summary>
        Task<PublicStatusPagesResult> GetPublicStatusPagesAsync();

        /// <summary>
        /// The list of public status pages can be called with this method.
        /// </summary>
        /// <param name="request">A Public Status Pages Request object.</param>
        Task<PublicStatusPagesResult> GetPublicStatusPagesAsync( PublicStatusPagesRequest request );

        #endregion
    }
}
