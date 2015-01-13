using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml;
using System.Windows;
using System.Windows.Controls;

using utils;
using onvif.services;

namespace odm.ui.controls {
    public  class XmlParser {

        public XElement CreateProtoElement(XmlSchemaElement schemaElement) {
            if (schemaElement.ElementSchemaType != null) {
                var nodes = CreateProtoAnyType(schemaElement.ElementSchemaType);
                return new XElement(schemaElement.QualifiedName.ToXName(), nodes);
            } else {
                throw new Exception("invalid schema type");
            }
        }

        XObject[] CreateProtoAnyType(XmlSchemaType schemaType) {
            if ((schemaType as XmlSchemaComplexType) != null) {
                return CreateProtoComplexType(schemaType as XmlSchemaComplexType);
            } else if ((schemaType as XmlSchemaSimpleType) != null) {
                var value = CreateProtoSimpleType(schemaType as XmlSchemaSimpleType);
                return new XObject[] { new XText(value) };
            } else {
                throw new Exception("invalid schema type");
            }
        }

        XObject[] CreateProtoComplexType(XmlSchemaComplexType complexType) {
            if (complexType.ContentModel != null) {
                if ((complexType.ContentModel as XmlSchemaSimpleContent) != null) {
                    return CreateProtoSimpleContent((complexType.ContentModel as XmlSchemaSimpleContent), complexType.BaseXmlSchemaType).ToArray();
                } else if ((complexType.ContentModel as XmlSchemaComplexContent) != null) {
                    return CreateProtoComplexContent((complexType.ContentModel as XmlSchemaComplexContent), complexType.BaseXmlSchemaType).ToArray();
                } else {
                    throw new Exception("not implemented");
                }
            } else {
                var complexContentExt = new XmlSchemaComplexContentExtension();
                if (complexType.BaseXmlSchemaType != null) {
                    complexContentExt.BaseTypeName = complexType.BaseXmlSchemaType.QualifiedName;
                } else {
                    complexContentExt.BaseTypeName = null;
                }

                if (complexType.Attributes != null) {
                    foreach (var i in complexType.Attributes) {
                        complexContentExt.Attributes.Add(i);
                    }
                }
                complexContentExt.Particle = complexType.Particle;
                var complexContent = new XmlSchemaComplexContent();
                complexContent.Content = complexContentExt;
                return CreateProtoComplexContent(complexContent, complexType.BaseXmlSchemaType).ToArray();
            }
        }

        string CreateProtoSimpleType(XmlSchemaSimpleType simpleType) {
            if (simpleType.QualifiedName.Namespace == XmlSchema.Namespace) {
                return CreateProtoXsdType(simpleType.QualifiedName.Name);
            } else if ((simpleType.Content as XmlSchemaSimpleTypeRestriction) != null) {
                var facets = (simpleType.Content as XmlSchemaSimpleTypeRestriction).Facets.OfType<XmlSchemaFacet>();
                foreach (var facet in facets) {
                    if ((facet as XmlSchemaEnumerationFacet) != null) {
                        return (facet as XmlSchemaEnumerationFacet).Value;
                    } else if ((facet as XmlSchemaMinInclusiveFacet) != null) {
                        return (facet as XmlSchemaMinInclusiveFacet).Value;
                    } else if ((facet as XmlSchemaMaxLengthFacet) != null) {
                        return (facet as XmlSchemaMaxLengthFacet).Value;
                    }
                }
                throw new Exception("not implemented");
            } else if ((simpleType.Content as XmlSchemaSimpleTypeUnion) != null) {
                return CreateProtoSimpleType((simpleType.Content as XmlSchemaSimpleTypeUnion).BaseMemberTypes.First());
            } else {
                throw new Exception("not implemented");
            }
        }

        string CreateProtoXsdType(string name) {
            switch (name) {
                case "integer":
                    return "0";
                case "int":
                    return "0";
                case "unsignedLong":
                    return "0";
                case "nonNegativeInteger":
                    return "0";
                case "float":
                    return "0.0";
                case "boolean":
                    return "false";
                case "duration":
                    return "PT0S";
                case "string":
                    return "test";
                case "anyURI":
                    return "uri";
                case "QName":
                    return "qname";
                case "NCName":
                    return "ncname";
                case "dateTime":
                    return XmlConvert.ToString(new System.DateTime(), XmlDateTimeSerializationMode.Unspecified);
            }
            throw new Exception("not implemented");
        }

        XElement[] CreateProtoParticle(XmlSchemaParticle particle) {
            if ((particle as XmlSchemaSequence) != null) {
                return CreateProtoSequence((particle as XmlSchemaSequence)).ToArray();
            } else if ((particle as XmlSchemaChoice) != null) {
                return CreateProtoChoice((particle as XmlSchemaChoice)).ToArray();
            } else if ((particle as XmlSchemaAll) != null) {
                return CreateProtoAll((particle as XmlSchemaAll)).ToArray();
            } else if ((particle as XmlSchemaAny) != null) {
                return new XElement[] { new XElement(XName.Get("any")) };
            } else if ((particle as XmlSchemaElement) != null) {
                return new XElement[] { CreateProtoElement((particle as XmlSchemaElement)) };
            } else {
                throw new Exception("not implemented");
            }
        }

        IEnumerable<XElement> CreateProtoAll(XmlSchemaAll all) {
            foreach (XmlSchemaParticle particle in all.Items) {
                if (((int)particle.MinOccurs) > 0) {
                    foreach (var val in CreateProtoParticle(particle).Repeat((int)particle.MinOccurs)) {
                        yield return val;
                    }
                }
            }
        }

        IEnumerable<XElement> CreateProtoSequence(XmlSchemaSequence sequence) {
            foreach (XmlSchemaParticle particle in sequence.Items) {
                if (((int)particle.MinOccurs) > 0) {
                    foreach (var val in CreateProtoParticle(particle).Repeat((int)particle.MinOccurs)) {
                        yield return val;
                    }
                }
            }
        }

        IEnumerable<XElement> CreateProtoChoice(XmlSchemaChoice choice) {
            var particle = choice.Items.OfType<XmlSchemaParticle>().First();
            if (((int)particle.MinOccurs) > 0) {
                foreach (var val in CreateProtoParticle(particle).Repeat((int)particle.MinOccurs)) {
                    yield return val;
                }
            }
        }

        IEnumerable<XObject> CreateProtoSimpleContent(XmlSchemaSimpleContent simpleContent, XmlSchemaType baseType) {
            var content = simpleContent.Content as XmlSchemaSimpleContentExtension;

            if (baseType != null) {
                foreach (var x in CreateProtoAnyType(baseType)) {
                    yield return x;
                }
            }

            foreach (XmlSchemaAttribute attr in content.Attributes) {
                if (attr.Use == XmlSchemaUse.Required) {
                    var name = attr.QualifiedName.ToXName();
                    string retres = null;
                    if (attr.FixedValue != null) {
                        throw new Exception("not implemented");
                    } else if (attr.DefaultValue != null) {
                        retres = attr.DefaultValue;
                    } else {
                        retres = CreateProtoSimpleType(attr.AttributeSchemaType);
                    }
                    yield return new XAttribute(name, retres);
                }
            }
        }

        IEnumerable<XObject> CreateProtoComplexContent(XmlSchemaComplexContent complexContent, XmlSchemaType baseType) {
            var content = complexContent.Content as XmlSchemaComplexContentExtension;

            if (baseType != null) {
                foreach (var x in CreateProtoAnyType(baseType)) {
                    yield return x;
                }
            }

            foreach (XmlSchemaAttribute attr in content.Attributes) {
                if (attr.Use == XmlSchemaUse.Required) {
                    var name = attr.QualifiedName.ToXName();
                    string retres = null;
                    if (attr.FixedValue != null) {
                        throw new Exception("not implemented");
                    } else if (attr.DefaultValue != null) {
                        retres = attr.DefaultValue;
                    } else {
                        retres = CreateProtoSimpleType(attr.AttributeSchemaType);
                    }
                    yield return new XAttribute(name, retres);
                }
            }
            if (content.Particle != null) {
                var particle = content.Particle;
                if ((int)particle.MinOccurs > 0) {
                    foreach (var retval in CreateProtoParticle(particle).Repeat((int)particle.MinOccurs)) {
                        yield return retval;
                    }
                }
            }
        }
    }
}
