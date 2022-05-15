using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint.Security;

namespace SP.Portal.Core.Receivers
{
    /// <summary>
    /// Helper class to modify the web .config during the activation and deactivation of features
    /// Original sample  : http://www.codeplex.com/sharepointajax
    /// (Daniel Larson & Tony Bierman)
    /// </summary>
    public abstract class WebConfigEngineFeatureReceiver : SPFeatureReceiver
    {
        /// <summary>
        /// Store the current list of modifications
        /// </summary>
        protected List<ModificationEntry> Entries = new List<ModificationEntry>();

        /// <summary>
        /// Gets the owner modif.
        /// </summary>
        /// <value>The owner modif.</value>
        abstract protected string OwnerModif { get; }

        protected virtual SPWebApplication WebApp { get; set; }

        #region Events
        /// <summary>
        /// Occurs when a Feature is deactivated.
        /// </summary>
        /// <param name="properties">An <see cref="T:Microsoft.SharePoint.SPFeatureReceiverProperties"></see> object that represents the properties of the event.</param>
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        {
            CheckWebApplication(properties);
            RemoveConfigurationHandler();
        }

        /// <summary>
        /// Handles the Feature Activation
        /// </summary>
        /// <param name="properties">SPFeatureReceiverProperties passes in the "context" 
        /// including the web application reference</param>
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            CheckWebApplication(properties);
            AddConfigurationHandler(properties);
        }

        private void CheckWebApplication(SPFeatureReceiverProperties properties)
        {
            if (properties == null)
                throw new ArgumentNullException("properties");

            #region Get SPWebApplication

            SPSite siteCollection = properties.Feature.Parent as SPSite;
            if (siteCollection == null)
            {
                SPWeb web = properties.Feature.Parent as SPWeb;
                if (web != null)
                    WebApp = web.Site.WebApplication;
                else
                    WebApp = properties.Feature.Parent as SPWebApplication;
            }
            else
                WebApp = siteCollection.WebApplication;

            if (WebApp == null)
            {
                throw new Exception("Web application is Null");
            }

            //throw new CustomAttributeFormatException();
            #endregion

        }
        /// <summary>
        /// Occurs after a Feature is installed.
        /// </summary>
        /// <param name="properties">An <see cref="T:Microsoft.SharePoint.SPFeatureReceiverProperties"></see> object that represents the properties of the event.</param>
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public override void FeatureInstalled(SPFeatureReceiverProperties properties)
        {
            // DoNothing();
        }

        /// <summary>
        /// Occurs when a Feature is uninstalled.
        /// </summary>
        /// <param name="properties">An <see cref="T:Microsoft.SharePoint.SPFeatureReceiverProperties"></see> object that represents the properties of the event.</param>
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public override void FeatureUninstalling(SPFeatureReceiverProperties properties)
        {
            // DoNothing();
        }

        #endregion

        /// <summary>
        /// Adds web.config modifications handler.
        /// </summary>
        protected virtual void AddConfigurationHandler(SPFeatureReceiverProperties properties)
        {
            InitializeModificationEntries(properties);
            foreach (ModificationEntry modEntry in Entries)
            {
                WebApp.WebConfigModifications.Add(CreateModification(modEntry));
            }


            WebApp.WebService.ApplyWebConfigModifications();
            WebApp.Update();
        }

        protected virtual void InitializeModificationEntries(SPFeatureReceiverProperties properties)
        {
            InitializeModificationEntries();
        }

        protected virtual void InitializeModificationEntries()
        {
        }

        /// <summary>
        /// Remove web.config modifications handler.
        /// </summary>
        protected virtual void RemoveConfigurationHandler()
        {
            RemoveConfigurationToWebConfig(WebApp, OwnerModif);
        }


        private SPWebConfigModification CreateModification(ModificationEntry modEntry)
        {
            SPWebConfigModification modification;

            modification = new SPWebConfigModification(modEntry.Name, modEntry.XPath);
            modification.Owner = OwnerModif;
            modification.Sequence = modEntry.Sequence;
            modification.Type = modEntry.ModType;
            modification.Value = modEntry.Value;

            return modification;
        }

        /// <summary>
        /// Removes the web config entries.
        /// </summary>
        /// <param name="oWebApp">The web app.</param>
        /// <param name="owner">The owner.</param>
        /// <remarks>
        /// Tips from Vincent Rothwell 
        /// http://blog.thekid.me.uk/archive/2007/03/20/removing-web-config-entries-from-sharepoint-using-spwebconfigmodification.aspx
        /// </remarks>
        protected void RemoveConfigurationToWebConfig(SPWebApplication oWebApp, string owner)
        {
            Collection<SPWebConfigModification> oCollection = oWebApp.WebConfigModifications;
            int iStartCount = oCollection.Count;
            for (int c = iStartCount - 1; c >= 0; c--)
            {
                SPWebConfigModification oModification = oCollection[c];
                if (oModification.Owner == owner)
                    oCollection.Remove(oModification);
            }
            // Apply changes only if any items were removed.
            if (iStartCount > oCollection.Count)
            {
                //                oWebApp.Farm.Services.GetValue<SPWebService>().ApplyWebConfigModifications();
                oWebApp.WebService.ApplyWebConfigModifications();
                oWebApp.Update();
            }

        }

    }
}
