using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Navigation;

namespace Genesta.Document.Management.Features.WebFeature
{
    

    /// <summary>
    /// This class handles events raised during feature activation, deactivation, installation, uninstallation, and upgrade.
    /// </summary>
    /// <remarks>
    /// The GUID attached to this class may be used during packaging and should not be modified.
    /// </remarks>

    [Guid("b6de2438-51fd-4fab-b622-c9337c34bc77")]
    public class WebFeatureEventReceiver : SPFeatureReceiver
    {
        // Uncomment the method below to handle the event raised after a feature has been activated.

        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            using (var web = properties.Feature.Parent as SPWeb)
            {
                web.UIVersion = 3;
                web.UIVersionConfigurationEnabled = false;
                var masterUrl = web.ServerRelativeUrl + "/_catalogs/masterpage/frontpage.master";
                web.CustomMasterUrl = masterUrl;
                web.MasterUrl = masterUrl;
                web.Update();
                CreateQuickLaunchNavigation(web);
            }
        }


        // Uncomment the method below to handle the event raised before a feature is deactivated.

        //public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        //{
        //}


        // Uncomment the method below to handle the event raised after a feature has been installed.

        //public override void FeatureInstalled(SPFeatureReceiverProperties properties)
        //{
        //}


        // Uncomment the method below to handle the event raised before a feature is uninstalled.

        //public override void FeatureUninstalling(SPFeatureReceiverProperties properties)
        //{
        //}

        // Uncomment the method below to handle the event raised when a feature is upgrading.

        //public override void FeatureUpgrading(SPFeatureReceiverProperties properties, string upgradeActionName, System.Collections.Generic.IDictionary<string, string> parameters)
        //{
        //}

        #region Methods

        private void CreateQuickLaunchNavigation(SPWeb web)
        {
            var quickLaunch = web.Navigation.QuickLaunch;
            var archiveNode = CreateNode("Archive", web.ServerRelativeUrl + "/Lists/Archive");
            var documentsNode = CreateNode("Documents", web.ServerRelativeUrl + "/Lists/Documents");
            var issuesNode = CreateNode("Issues", web.ServerRelativeUrl + "/Lists/Issues");
            var registerNode = CreateNode("Register", web.ServerRelativeUrl + "/Lists/Register");

            quickLaunch.AddAsLast(archiveNode);
            quickLaunch.AddAsLast(documentsNode);
            quickLaunch.AddAsLast(issuesNode);
            quickLaunch.AddAsLast(registerNode);
        }

        private static SPNavigationNode CreateNode(string nodeTitle, string nodeUrl)
        {
            var node = new SPNavigationNode(nodeTitle, nodeUrl);
            return node;
        }

        #endregion
    }
}
