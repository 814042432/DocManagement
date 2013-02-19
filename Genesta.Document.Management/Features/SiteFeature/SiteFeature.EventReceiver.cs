using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Navigation;
using Genesta.Document.Management.Code;

namespace Genesta.Document.Management.Features.SiteFeature
{
    /// <summary>
    /// This class handles events raised during feature activation, deactivation, installation, uninstallation, and upgrade.
    /// </summary>
    /// <remarks>
    /// The GUID attached to this class may be used during packaging and should not be modified.
    /// </remarks>

    [Guid("908b4fde-dd6c-473b-add8-0f78a2890040")]
    public class SiteFeatureEventReceiver : SPFeatureReceiver
    {
        // Uncomment the method below to handle the event raised after a feature has been activated.

        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            using(var site = properties.Feature.Parent as SPSite)
            {
               CreateTermSet(site);

            }
        }

        private void CreateTermSet(SPSite site)
        {
            var termsetDocumentTypes = site.CreateTermSet(Constants.TermGroupGenesta, Constants.TermSetDocumentTypes);
            site.CreateTerm(termsetDocumentTypes, "Budget");
            site.CreateTerm(termsetDocumentTypes, "Business case");
            site.CreateTerm(termsetDocumentTypes, "Other");
            site.CreateTerm(termsetDocumentTypes, "Summary report");
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
    }
}
