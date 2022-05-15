using Microsoft.SharePoint.Administration;

namespace SP.Portal.Core.Receivers
{
    public class ModificationEntry
    {
        public string Name { get; set; }
        public string XPath { get; set; }
        public string Value { get; set; }
        public SPWebConfigModification.SPWebConfigModificationType ModType { get; set; }
        public uint Sequence { get; set; }

        public ModificationEntry()
        {

        }

        public ModificationEntry(string name, string xPath, string value, SPWebConfigModification.SPWebConfigModificationType modType, uint sequence)
        {
            Name = name;
            XPath = xPath;
            Value = value;
            ModType = modType;
            Sequence = sequence;
        }

        public ModificationEntry(string name, string xPath, string value, SPWebConfigModification.SPWebConfigModificationType modType)
            : this(name, xPath, value, modType, uint.MinValue)
        {
        }

        public ModificationEntry(string name, string xPath, string value)
            : this(name, xPath, value, SPWebConfigModification.SPWebConfigModificationType.EnsureChildNode, uint.MinValue)
        {

        }

        public ModificationEntry(string name, string xPath, string value, uint sequence)
            : this(name, xPath, value, SPWebConfigModification.SPWebConfigModificationType.EnsureChildNode, sequence)
        {

        }

    }
}
