using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Schema;

namespace utils
{
    public static class XmlSchemaExtensions
    {
        public static bool TryGetAllowedValuesOfGlobalType(this XmlSchemaSet schema, XmlQualifiedName typeName, out string[] values) {
            values = new string[0];

            bool found = false;
            try {
                var type = GetSimpleType(schema, typeName);
                if (type != null) {
                    if (type.Content is XmlSchemaSimpleTypeRestriction) {
                        var content = type.Content as XmlSchemaSimpleTypeRestriction;

                        if (content.BaseTypeName == new XmlQualifiedName("string", @"http://www.w3.org/2001/XMLSchema")) {
                            var vals = new List<string>();
                            foreach (var facetItem in content.Facets) {
                                if (facetItem is XmlSchemaEnumerationFacet) {
                                    var val = ((XmlSchemaEnumerationFacet)facetItem).Value;
                                    vals.Add(val);
                                }
                            }
                            if (vals.Count > 0) {
                                values = vals.ToArray();
                                found = true;
                            }
                        }
                    }
                }
            }
            catch (Exception) { 
            }
            return found;
        }

        public static XmlSchemaSimpleType GetSimpleType(XmlSchemaSet schema, XmlQualifiedName typeName) { 
            var types = schema.GlobalTypes.Values;
            foreach (var typeItem in types) {
                if (typeItem is XmlSchemaSimpleType) {
                    var type = typeItem as XmlSchemaSimpleType;

                    if (type.QualifiedName == typeName)
                        return type;
                }
            }
            return null;
        }

        public static bool TryGetIntLimitsOfGlobalType(this XmlSchemaSet schema, XmlQualifiedName typeName, out int minimum, out int maximum)
        {
            minimum = 0;
            maximum = 0;

            bool found = false;
            try {
                var type = GetSimpleType(schema, typeName);
                if (type != null) {
                    if (type.Content is XmlSchemaSimpleTypeRestriction) {
                        var content = type.Content as XmlSchemaSimpleTypeRestriction;

                        if (content.BaseTypeName == new XmlQualifiedName("int", @"http://www.w3.org/2001/XMLSchema")) {
                            int? min = null, max = null;
                            foreach (var facetItem in content.Facets) {
                                if (facetItem is XmlSchemaMinInclusiveFacet)
                                    min = int.Parse(((XmlSchemaMinInclusiveFacet)facetItem).Value);
                                else if (facetItem is XmlSchemaMaxInclusiveFacet)
                                    max = int.Parse(((XmlSchemaMaxInclusiveFacet)facetItem).Value);
                            }
                            if (min != null && max != null) {
                                minimum = min.Value;
                                maximum = max.Value;
                                found = true;
                            }
                        }
                    }
                }
            }
            catch (Exception) {
            }
            return found;
        }
    }
}
