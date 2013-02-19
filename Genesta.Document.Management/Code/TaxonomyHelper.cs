namespace Genesta.Document.Management.Code
{
    using System;
    using System.Linq;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Taxonomy;

    public static class TaxonomyHelper
    {
        public static void ConnectTaxonomyField(SPSite site, Guid fieldId, string termGroup, string termSetName)
        {
            if (site.RootWeb.Fields.Contains(fieldId))
            {
                var session = new TaxonomySession(site);

                if (session.TermStores.Count != 0)
                {
                    var termStore = session.TermStores[Constants.TaxonomyTermStore];
                    var group = termStore.Groups[termGroup];
                    var termSet = group.TermSets[termSetName];

                    var field = site.RootWeb.Fields[fieldId] as TaxonomyField;
                    if (field != null)
                    {
                        field.SspId = termSet.TermStore.Id;
                        field.TermSetId = termSet.Id;
                        field.TargetTemplate = string.Empty;
                        field.AnchorId = Guid.Empty;
                        field.Update(true);
                    }
                }
                else
                {
                    throw new TermStoreNotFoundException(string.Format("Managed Metadata Service TermStore not found in site {0}", site.Url));
                }
            }
            else
            {
                throw new ArgumentException(string.Format("Field {0} not found in site {1}", fieldId, site.Url), "fieldId");
            }
        }
    }

    [Serializable]
    public class TermStoreNotFoundException : Exception
    {
        public TermStoreNotFoundException() { }
        public TermStoreNotFoundException(string message) : base(message) { }
        public TermStoreNotFoundException(string message, Exception inner) : base(message, inner) { }
        protected TermStoreNotFoundException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }

    public static class TaxonomyExtensions
    {
        public static Group GetByName(this GroupCollection groupCollection, string name)
        {
            if (String.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Taxonomy group name cannot be empty", "name");
            }
            foreach (var group in groupCollection.Where(group => group.Name == name))
            {
                return group;
            }
            throw new ArgumentOutOfRangeException("name", name, "Could not find the taxonomy group");
        }

        public static TermSet GetByName(this TermSetCollection termSets, string name)
        {
            if (String.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Term set name cannot be empty", "name");
            }
            foreach (var termSet in termSets.Where(termSet => termSet.Name == name))
            {
                return termSet;
            }
            throw new ArgumentOutOfRangeException("name", name, "Could not find the term set");
        }


        public static Group CreateTermGroup(this SPSite site, string name)
        {
            var session = new TaxonomySession(site);

            if (session.TermStores.Count != 0 && session.TermStores.Any(x => x.Name == Constants.TaxonomyTermStore))
            {
                var termStore = session.TermStores[Constants.TaxonomyTermStore];
                var groups = termStore.Groups;
                if (groups.All(x => x.Name != name))
                {
                    var group = termStore.CreateGroup(name);
                    termStore.CommitAll();
                    return group;
                }

                return groups.FirstOrDefault(x => x.Name == name);
            }

            throw new TermStoreNotFoundException(
                string.Format("Managed Metadata Service TermStore not found in site {0}", site.Url));


        }

        public static TermSet CreateTermSet(this SPSite site, string termGroup, string name)
        {
            var session = new TaxonomySession(site);

            if (session.TermStores.Count != 0 && session.TermStores.Any(x => x.Name == Constants.TaxonomyTermStore))
            {
                var termStore = session.TermStores[Constants.TaxonomyTermStore];
                var group = termStore.Groups[termGroup];
                if (@group.TermSets.All(x => x.Name != name))
                {
                    var termset = group.CreateTermSet(name);
                    termStore.CommitAll();
                    return termset;
                }

                return group.TermSets.FirstOrDefault(x => x.Name == name);
            }

            throw new TermStoreNotFoundException(
                string.Format("Managed Metadata Service TermStore not found in site {0}", site.Url));
        }

        public static Term CreateTerm(this SPSite site, TermSet termset, string name)
        {
            if (termset.Terms.All(x => x.Name != name))
            {
                var term = termset.CreateTerm(name, int.Parse(site.RootWeb.Language.ToString()));
                term.TermStore.CommitAll();
                return term;
            }

            return termset.Terms.FirstOrDefault(x => x.Name == name);
        }

        public static Term CreateTerm(this SPSite site, Term term, string name)
        {
            if (term.Terms.All(x => x.Name != name))
            {
                var returnterm = term.CreateTerm(name, int.Parse(site.RootWeb.Language.ToString()));
                returnterm.TermStore.CommitAll();
                return returnterm;
            }

            return term.Terms.FirstOrDefault(x => x.Name == name);
        }
    }
}
