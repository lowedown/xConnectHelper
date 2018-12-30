using Sitecore.XConnect;

namespace Sitecore.SharedSource.XConnectHelper.ContactRepository
{
    public interface IContactRepository
    {
        /// <summary>
        /// Returns the xConnect contact for the current contact loaded into the tracker. This method ensures that a new contact is saved first.
        /// </summary>
        /// <param name="FacetKey">Name of facet to load</param>
        /// <returns></returns>
        Contact GetCurrentContact(string FacetKey);

        /// <summary>
        /// Saves a facet to the specified contact
        /// </summary>
        /// <typeparam name="T">Object inheriting from Facet</typeparam>
        /// <param name="contact">Contact to save to</param>
        /// <param name="FacetKey">Targeted FacetKey</param>
        /// <param name="Facet">Facet object</param>
        /// <returns></returns>
        bool SaveFacet<T>(Contact contact, string FacetKey, T Facet) where T : Facet;

        T GetFacet<T>(Contact contact, string FacetKey) where T : Facet;

        /// <summary>
        /// Reloads the the contact into session. Call this after updating facets.
        /// </summary>
        /// <returns></returns>
        bool ReloadCurrentContact();
    }
}
