using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Schema;
using System.Xml;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;
using utils;
using System.ComponentModel;
using Microsoft.Windows.Controls;
using System.Xml.Serialization;
using System.CodeDom.Compiler;
using Microsoft.CSharp;
using System.IO;
using System.CodeDom;
using onvif.services;
using System.Xml.Linq;

namespace odm.ui.controls {
    public class XmlParserFactory {
        public XmlParserFactory(global::onvif.services.Config config, global::onvif.services.ConfigDescription description, XmlSchemaSet schema) {
            this.config = config;
            this.schema = schema;
            this.description = description;
            
        }
        global::onvif.services.ConfigDescription description;
        global::onvif.services.Config config;
        XmlSchemaSet schema;

		//Simple items parsing
		List<itemDescr> InitSimpleItems() {
			List<itemDescr> simpleItemsList = new List<itemDescr>();

			description.parameters.simpleItemDescription.ForEach(x => {
				XmlSchemaType xsdType = XmlSchemaType.GetBuiltInSimpleType(XmlTypeCode.String);
				if (x.type.Namespace == XmlSchema.Namespace) {
					xsdType = XmlSchemaType.GetBuiltInSimpleType(x.type);
				} else {
					if (schema.GlobalTypes.Contains(x.type)) {
						schema.GlobalTypes.Values.ForEach(sc => {
							var obj = (XmlSchemaObject)sc;

						});
					}
				}
				simpleItemsList.Add(new itemDescr() { xtype = xsdType, xname = x.name });
			});
			simpleItemsList.ForEach(x => {
				if (config.parameters != null && config.parameters.simpleItem != null && config.parameters.simpleItem.FirstOrDefault(si => si.name == x.xname) != null) {
					x.isPresent = true;
				} else
					x.isPresent = false;
			});

			return simpleItemsList;
		}
		public FrameworkElement GetSimpleItemsControl() {
			StackPanel spanel = new StackPanel();
			spanel.Orientation = Orientation.Vertical;

			var simplelist = InitSimpleItems();

			simplelist.Where(x => x.isPresent).ForEach(x => {
				var sitem = config.parameters.simpleItem.FirstOrDefault(si => si.name == x.xname);
				if (sitem != null) {
					spanel.Children.Add(GetSimpleEditorCtrl(x.xtype, sitem.name, sitem));
				}
			});
			simplelist.Where(x => !x.isPresent).ForEach(x => {
				var sitem = new ItemList.SimpleItem() {
					name = x.xname,
					value = ""
				};

				FrameworkElement elem = GetSimpleEditorCtrl(x.xtype, x.xname, sitem);

				CheckBox chBox = new CheckBox();
				chBox.Margin = new Thickness(40, 0, 0, 0);
				chBox.Content = "Is element enabled";
				chBox.CreateBinding(CheckBox.IsCheckedProperty, (itemDescr)x, s => {
					elem.IsEnabled = s.isPresent;
					return s.isPresent;
				}, (o, v) => {
					o.isPresent = v;
					if (v) {
						elem.IsEnabled = true;
						if (config.parameters != null && config.parameters.simpleItem != null) {
							var items = config.parameters.simpleItem;
							config.parameters.simpleItem = items.Append(sitem).ToArray();
						} else {
							if (config.parameters == null) {
								config.parameters = new global::onvif.services.ItemList();
							}
							config.parameters.simpleItem = new ItemList.SimpleItem[] { sitem };
						}
					} else {
						elem.IsEnabled = false;
						if (config.parameters.simpleItem.Contains(sitem)) {
							int count = config.parameters.simpleItem.Count();
							var arr = new ItemList.SimpleItem[count - 1];
							int cnt = 0;
							for (int i = 0; i < count; i++) {
								if (config.parameters.simpleItem[i] != sitem) {
									arr[cnt] = config.parameters.simpleItem[i];
									cnt++;
								}
							}
							config.parameters.simpleItem = arr;
						}
					}
				});
				StackPanel tmppanel = new StackPanel();
				tmppanel.Orientation = Orientation.Horizontal;
				tmppanel.Children.Add(elem);
				tmppanel.Children.Add(chBox);

				spanel.Children.Add(tmppanel);
			});

			return spanel;
		}
		FrameworkElement GetSimpleEditorCtrl(XmlSchemaType xtype, string name, ItemList.SimpleItem sitem) {
			StackPanel spanel = new StackPanel();
			spanel.Orientation = Orientation.Horizontal;

			TextBlock tblock = new TextBlock();
			tblock.Margin = new Thickness(3);
			tblock.Text = name;
			tblock.MinWidth = 150;
			tblock.Foreground = Brushes.DarkGray;
			tblock.FontWeight = FontWeights.Bold;
			tblock.VerticalAlignment = System.Windows.VerticalAlignment.Center;

			spanel.Children.Add(tblock);

			spanel.Children.Add(GetTypeEditor(xtype, sitem));
			return spanel;
		}
		FrameworkElement CreateTextEditor(ItemList.SimpleItem sitem) {
			var tedit = new TextBox() {
				Text = sitem.value,
				MinWidth = 50,
				VerticalAlignment = VerticalAlignment.Center
			};
			tedit.SetUpdateTrigger(
				TextBox.TextProperty,
				(string v) => {
					sitem.value = v;
				}
			);
			return tedit;
		}
		FrameworkElement GetTypeEditor(XmlSchemaType xtype, ItemList.SimpleItem sitem) {
			FrameworkElement felement = null;
			if (XmlSchemaType.IsDerivedFrom(xtype, XmlSchemaType.GetBuiltInSimpleType(XmlTypeCode.Boolean), XmlSchemaDerivationMethod.All)) {
				try {
					var val = XmlConvert.ToBoolean(sitem.value);
					felement = new CheckBox() {
						IsChecked = val,
						Margin = new Thickness(3)
					};
					felement.SetUpdateTrigger(
						CheckBox.IsCheckedProperty,
						(bool v) => {
							sitem.value = XmlConvert.ToString(v);
						}
					);
				} catch {
					felement = CreateTextEditor(sitem);
				}
			} else if (XmlSchemaType.IsDerivedFrom(xtype, XmlSchemaType.GetBuiltInSimpleType(XmlTypeCode.Integer), XmlSchemaDerivationMethod.All)) {
				try {
					var val = XmlConvert.ToInt32(sitem.value);
					felement = new IntegerUpDown() {
						Value = val,
						Margin = new Thickness(3),
						Increment = 1,
						VerticalAlignment = VerticalAlignment.Center,
						MinWidth = 50
					};
					felement.SetUpdateTrigger(
						IntegerUpDown.ValueProperty,
						(int v) => {
							sitem.value = v.ToString();
						}
					);
				} catch {
					felement = CreateTextEditor(sitem);
				}
			} else if (XmlSchemaType.IsDerivedFrom(xtype, XmlSchemaType.GetBuiltInSimpleType(XmlTypeCode.Float), XmlSchemaDerivationMethod.All)) {
				try {
					var val = XmlConvert.ToDouble(sitem.value);
					felement = new DoubleUpDown() {
						Value = val,
						CultureInfo = System.Globalization.CultureInfo.InvariantCulture,
						Margin = new Thickness(3),
						Increment = 0.1,
						FormatString = "F1",
						VerticalAlignment = VerticalAlignment.Center,
						MinWidth = 50
					};
					felement.SetUpdateTrigger(
						DoubleUpDown.ValueProperty,
						(double v) => {
							sitem.value = v.ToString();
						}
					);
				} catch {
					felement = CreateTextEditor(sitem);
				}
			} else if (XmlSchemaType.IsDerivedFrom(xtype, XmlSchemaType.GetBuiltInSimpleType(XmlTypeCode.Double), XmlSchemaDerivationMethod.All)) {
				try {
					var val = XmlConvert.ToDouble(sitem.value);
					felement = new DoubleUpDown() {
						Value = val,
						CultureInfo = System.Globalization.CultureInfo.InvariantCulture,
						Margin = new Thickness(3),
						Increment = 0.1,
						FormatString = "F2",
						VerticalAlignment = VerticalAlignment.Center,
						MinWidth = 50
					};
					felement.SetUpdateTrigger(
						DoubleUpDown.ValueProperty,
						(double v) => {
							sitem.value = v.ToString();
						}
					);
				} catch {
					felement = CreateTextEditor(sitem);
				}
			} else {
				felement = CreateTextEditor(sitem);
			}

			return felement;
		}

		//Element items parsing
		SchemaParser xsdParse = new SchemaParser();

		public FrameworkElement GetElementItemsControl() {
			StackPanel spanel = new StackPanel();
			spanel.Orientation = Orientation.Vertical;

			InitElementItems().ForEach(felem => {
				spanel.Children.Add(felem);
			});

			return spanel;
		}

		FrameworkElement GetXmlElementView(elementDescr element, string parentName) {
			FrameworkElement felement = null;
			if (element.HasChilds) {
				Expander grbox = new Expander();
				//grbox.Padding = new Thickness(20, 0, 0, 0);
				//if (parentName != "__Top") {
				//    //grbox.Margin = new Thickness(10, 0, 0, 0);
				//}
				
				grbox.Header = element.Name;

				StackPanel spanel = new StackPanel();
				spanel.Orientation = Orientation.Vertical;

				element.Childs.ForEach(child => {
					spanel.Children.Add(GetXmlElementView(child, element.Name));
				});
				
				spanel.Margin = new Thickness(20, 0, 0, 0);

				grbox.Content = spanel;

				felement = grbox;
			} else{
				StackPanel spanel = new StackPanel();
				spanel.Orientation = Orientation.Horizontal;

				TextBlock tblock = new TextBlock();
				tblock.Margin = new Thickness(3);
				tblock.Text = element.Name;
				tblock.MinWidth = 150;
				tblock.Foreground = Brushes.DarkGray;
				tblock.FontWeight = FontWeights.Bold;
				tblock.VerticalAlignment = System.Windows.VerticalAlignment.Center;

				spanel.Children.Add(tblock);
				spanel.Children.Add(GetValueEditor(element));

				spanel.Margin = new Thickness(20, 0, 0, 0);

				felement = spanel;
			}
			return felement;
		}
		FrameworkElement GetValueEditor(elementDescr element) {
			FrameworkElement felem = null;

			switch (element.QName.Name) { 
				case "int":
					IntegerUpDown numUp = new IntegerUpDown();
					numUp.Margin = new Thickness(3);
					numUp.Increment = 1;
					numUp.VerticalAlignment = VerticalAlignment.Center;
					numUp.MinWidth = 50;
					numUp.CreateBinding(IntegerUpDown.ValueProperty, element, x => {
						int val = 0;
						int.TryParse(element.Value, out val);
						return val;
					}, (o, v) => {
						o.Value = v.ToString();
					});
					felem = numUp;
					break;
				case "float":
					DoubleUpDown doubleUp = new DoubleUpDown();
					doubleUp.CultureInfo = System.Globalization.CultureInfo.InvariantCulture;
					doubleUp.Margin = new Thickness(3);
					doubleUp.Increment = 0.01f;
					doubleUp.FormatString = "F3";
					doubleUp.VerticalAlignment = VerticalAlignment.Center;
					doubleUp.MinWidth = 50;
					doubleUp.CreateBinding(DoubleUpDown.ValueProperty, element, x => {
						float val = 0;
						//float.TryParse(element.Value, out val);
						try {
							val = XmlConvert.ToSingle(element.Value);
						}catch(Exception err){
							dbg.Error(err);
						}
						return val;
					}, (o, v) => {
						o.Value = XmlConvert.ToString(v);
					});
					felem = doubleUp;
					break;
				case "boolean":
					CheckBox chBox = new CheckBox();
					chBox.Margin = new Thickness(3);
					chBox.CreateBinding(CheckBox.IsCheckedProperty, element, x => {
						bool val = false;
						bool.TryParse(element.Value, out val);
						return val;
					}, (o, v) => {
						o.Value = v.ToString();
					});
					felem = chBox;
					break;
				default:
					TextBox tbox = new TextBox();
					tbox.Margin = new Thickness(3);
					tbox.CreateBinding(TextBox.TextProperty, element, x => x.Value, (o, v) => {
						o.Value = v;
					});
					tbox.MinWidth = 50;
					tbox.VerticalAlignment = System.Windows.VerticalAlignment.Center;
					felem = tbox;
					break;
			}
			return felem;
		}
		List<FrameworkElement> InitElementItems() {
			List<FrameworkElement> elementsList = new List<FrameworkElement>();
			
			//parse schema
			xsdParse.Parse(schema);
					
			//parse top elements in XML data
			config.parameters.elementItem.ForEach(elemItem => {
				//for each of top elements: (AntishakerCrop; MarkerCalibration; ...)
				var telem = ParseXmlItem(elemItem);
				xmlElements.Add(telem);
			});

			//compare xsd data with xml
			//find mismatches
			//and prepare binding list

			PrepareXmlElemets();

			xmlElements.ForEach(xel => { 
				elementsList.Add(GetXmlElementView(xel, "__Top"));
			});

            return elementsList;
        }

		void PrepareXmlElemets() {
			xmlElements.ForEach(xmlElement => {
				//find description
				var schemaelem = xsdParse.ElementsDict.FirstOrDefault(pair => pair.Key == xmlElement.Name);
				if (schemaelem.Value == null)
					return;

				PrepareChilds(xmlElement, schemaelem.Value);
			});
		}
		void PrepareChilds(elementDescr xmlElement, odm.ui.controls.SchemaParser.xsdElement schemaElement) {
			if (xmlElement.HasChilds) {
				xmlElement.Childs.ForEach(chld => {
					var icomplex = schemaElement.Type as odm.ui.controls.SchemaParser.IComplexType;
					var isimple = schemaElement.Type as odm.ui.controls.SchemaParser.ISimpleType;
					var ipure = schemaElement.Type as odm.ui.controls.SchemaParser.IPureType;

					if(icomplex != null){
						var content =  icomplex.Content.FirstOrDefault(schemel => schemel.Name == chld.Name);
						if (content == null)
							return;
						if (content.Type.ContentType == SchemaParser.ContentTypes.pureType) {
							chld.QName = content.Type.QName;
						} else {
							PrepareChilds(chld, content);
						}
					} else if (isimple != null) {
						chld.Defaults = isimple.SimpleValues;
					} else if (ipure != null) {
						int i = 0;
					}
				});
			} else {
				var isimple = schemaElement.Type as odm.ui.controls.SchemaParser.ISimpleType;
				if (isimple != null) {
					xmlElement.QName = isimple.QName;
					xmlElement.Defaults = isimple.SimpleValues;
				}
			}
		}

		elementDescr ParseXmlItem(ItemList.ElementItem elemItem) {
			var element = elemItem.any;
			elementDescr elem = new elementDescr() { Name = element.LocalName, XmlElement = element };

			ParseElementDescr(elem);
			
			return elem;
		}
		void ParseElementDescr(elementDescr parentDescr) {
			var xmlLinkedNode = parentDescr.XmlElement as XmlLinkedNode;
			if (xmlLinkedNode != null) {
				xmlLinkedNode.ForEach(nodes => {
					var element = nodes as XmlElement;
					var text = nodes as XmlText;

					if (element != null) {
						elementDescr elDescr = new elementDescr() { Name = element.Name, XmlElement = element };

						ParseElementDescr(elDescr);

						parentDescr.Childs.Add(elDescr);
					}
					if (text != null) {
						parentDescr.Value = text.Value;
					}

				});
			} else {
				int i = 0;
			}
		}
		
		List<elementDescr> xmlElements = new List<elementDescr>();
		
        FrameworkElement FillSimpleValue(XmlLinkedNode linkedNode) {
            StackPanel nsp = new StackPanel();
            nsp.Orientation = Orientation.Horizontal;
            nsp.Children.Add(new Label() { MinWidth = 100, Content = linkedNode.ParentNode.Name });
			//TextBlock tb = new TextBlock();
			//tb.MinWidth = 50;
			//tb.CreateBinding(TextBlock.TextProperty, linkedNode, x => x.Value, (m, v) => { m.Value = v; });
            nsp.Children.Add(new TextBox() { MinWidth = 50, Text = linkedNode.Value });

            return nsp;
        }
        FrameworkElement FillLinkedNode(XmlLinkedNode linkedNode) {
            StackPanel commonPanel = new StackPanel();
            commonPanel.Orientation = Orientation.Vertical;

            if (linkedNode.Value != null) {
                commonPanel.Children.Add(FillSimpleValue(linkedNode));
            } else {
                Expander childExpander = new Expander();
                childExpander.ExpandDirection = ExpandDirection.Down;
                childExpander.Header = linkedNode.Name;
				
                StackPanel childPanel = new StackPanel();
                childPanel.Orientation = Orientation.Vertical;

                linkedNode.ForEach(lnode => {
                    var res = lnode as System.Xml.XmlLinkedNode;
                    childPanel.Children.Add(FillLinkedNode(res));
                });
                if (childPanel.Children.Count == 1)
                    commonPanel.Children.Add(childPanel);
                else {
                    childPanel.Margin = new Thickness(20, 0, 0, 0);
                    childExpander.Content = childPanel;
                    commonPanel.Children.Add(childExpander);
                }
            }
            return commonPanel;
        }
    }
	public class elementDescr : INotifyPropertyChanged {
		public XmlQualifiedName QName { get; set; }

		public string Name { get; set; }
		public XmlElement XmlElement { get; set; }

		public List<SchemaParser.SimpleValue> Defaults;

		public string Value {
			get {
				string val = "";
				if (XmlElement.HasChildNodes && XmlElement.ChildNodes.Count == 1) {
					val = XmlElement.FirstChild.Value;
				}
				return val;
			}
			set {
				if (XmlElement.HasChildNodes && XmlElement.ChildNodes.Count == 1) {
					XmlElement.FirstChild.Value = value;
					NotifyPropertyChanged("Value");
				}
			}
		}
		public XmlQualifiedName xmlName { get; set; }

		public bool HasChilds { 
			get {
				return childs != null;
			} 
		}
		List<elementDescr> childs;
		public List<elementDescr> Childs {
			get {
				if (childs == null)
					childs = new List<elementDescr>();
				return childs;
			}
		}
		public void AddChild(elementDescr child) {
			childs.Add(child);
			NotifyPropertyChanged("Childs");
		}
		public void RemoveChild(elementDescr child) {
			childs.Remove(child);
			NotifyPropertyChanged("Childs");
		}

		//InotifyPropertyChanged
		public event PropertyChangedEventHandler PropertyChanged;
		private void NotifyPropertyChanged(String info) {
			var prop_changed = this.PropertyChanged;
			if (prop_changed != null) {
				prop_changed(this, new PropertyChangedEventArgs(info));
			}
		}
	}

	public class itemDescr : INotifyPropertyChanged {
		XmlSchemaType _xtype;
		public XmlSchemaType xtype {
			get { return _xtype; }
			set {
				_xtype = value;
				NotifyPropertyChanged("xtype");
			}
		}
		string _xname;
		public string xname {
			get { return _xname; }
			set {
				_xname = value;
				NotifyPropertyChanged("xname");
			}
		}
		bool _isPresent;
		public bool isPresent {
			get { return _isPresent; }
			set {
				_isPresent = value;
				NotifyPropertyChanged("isPresent");
			}
		}

		private void NotifyPropertyChanged(String info) {
			var prop_changed = this.PropertyChanged;
			if (prop_changed != null) {
				prop_changed(this, new PropertyChangedEventArgs(info));
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;
	}

	public class SchemaParser {
		public class SimpleValue {
			public SimpleValue(string val, SchemaParser.SimpleTypes tp) {
				Value = val;
				Type = tp;
			}
			public string Value;
			public SchemaParser.SimpleTypes Type;
		}
		public enum ContentTypes {
			complexType,
			simpleType,
			pureType
		}
		public enum SimpleTypes {
			Enumeration,
			MaxExclusive,
			MinExclusive,
			MaxInclusive,
			MinInclusive,
			FractionDigits,
			MaxLength,
			MinLength,
			Numeric,
			Pattern,
			TotalDigits
		}
		public interface IType {
			ContentTypes ContentType { get; set; }
			string Name { get; set; }
		}
		public interface IComplexType : IType {
			List<xsdElement> Content { get; }
		}
		public interface ISimpleType : IType {
			List<SimpleValue> SimpleValues { get; }
			XmlQualifiedName QName { get; set; }
		}
		public interface IPureType : IType {
			XmlQualifiedName QName { get; set; }
		}
		public class xsdType : IComplexType, ISimpleType, IPureType {
			ContentTypes contentType;
			public ContentTypes ContentType { get { return contentType; } set { contentType = value; } }

			List<SimpleValue> simpleValues;
			public List<SimpleValue> SimpleValues {
				get {
					if (simpleValues == null) {
						simpleValues = new List<SimpleValue>();
					}
					return simpleValues;
				}
			}

			List<xsdElement> content;
			public List<xsdElement> Content {
				get {
					if (content == null)
						content = new List<xsdElement>();
					return content;
				}
			}

			public string Name { get; set; }
			public XmlQualifiedName QName { get; set; }
		}


		public enum ElementTypes {
			top,
			sequence,
			choise
		}
		public class xsdElement {
			public string Name { get; set; }
			public xsdType Type { get; set; }

			public decimal MaxOccurs { get; set; }
			public decimal MinOccurs { get; set; }

			public ElementTypes ElementType { get; set; }
		}

		Dictionary<string, xsdType> typesDict = new Dictionary<string, xsdType>();
		Dictionary<string, xsdElement> elementsDict = new Dictionary<string, xsdElement>();

		public Dictionary<string, xsdType> TypesDict { get { return typesDict; } }
		public Dictionary<string, xsdElement> ElementsDict { get { return elementsDict; } }

		xsdType ParceXsdSimpleType(XmlSchemaSimpleType simpleType) {
			xsdType xsdtype = new xsdType();
			xsdtype.ContentType = ContentTypes.simpleType;
			xsdtype.Name = simpleType.Name;

			var simplerestrict = simpleType.Content as XmlSchemaSimpleTypeRestriction;
			var simpleunion = simpleType.Content as XmlSchemaSimpleTypeUnion;
			var simplelist = simpleType.Content as XmlSchemaSimpleTypeList;

			if (simplerestrict != null) {
				xsdtype.QName = simplerestrict.BaseTypeName;
				//xsdtype.Name = simplerestrict.BaseTypeName.Name;

				simplerestrict.Facets.ForEach(facet => {
					var enumFacet = facet as System.Xml.Schema.XmlSchemaEnumerationFacet;
					var maxExclusiveFacet = facet as System.Xml.Schema.XmlSchemaMaxExclusiveFacet;
					var minExclusiveFacet = facet as System.Xml.Schema.XmlSchemaMinExclusiveFacet;
					var maxInclusiveFacetFacet = facet as System.Xml.Schema.XmlSchemaMaxInclusiveFacet;
					var minInclusiveFacetFacet = facet as System.Xml.Schema.XmlSchemaMinInclusiveFacet;
					var fractionDigitsFacet = facet as System.Xml.Schema.XmlSchemaFractionDigitsFacet;
					var maxLengthFacet = facet as System.Xml.Schema.XmlSchemaMaxLengthFacet;
					var minLengthFacet = facet as System.Xml.Schema.XmlSchemaMinLengthFacet;
					var numericFacet = facet as System.Xml.Schema.XmlSchemaNumericFacet;
					var patternFacet = facet as System.Xml.Schema.XmlSchemaPatternFacet;
					var totalDigitsFacet = facet as System.Xml.Schema.XmlSchemaTotalDigitsFacet;

					if (enumFacet != null) {
						xsdtype.SimpleValues.Add(new SimpleValue(enumFacet.Value, SchemaParser.SimpleTypes.Enumeration));
					}
					if (maxExclusiveFacet != null) {
						xsdtype.SimpleValues.Add(new SimpleValue(maxExclusiveFacet.Value, SchemaParser.SimpleTypes.MaxExclusive));
					}
					if (minExclusiveFacet != null) {
						xsdtype.SimpleValues.Add(new SimpleValue(minExclusiveFacet.Value, SchemaParser.SimpleTypes.MinExclusive));
					}
					if (maxInclusiveFacetFacet != null) {
						xsdtype.SimpleValues.Add(new SimpleValue(maxInclusiveFacetFacet.Value, SchemaParser.SimpleTypes.MaxInclusive));
					}
					if (minInclusiveFacetFacet != null) {
						xsdtype.SimpleValues.Add(new SimpleValue(minInclusiveFacetFacet.Value, SchemaParser.SimpleTypes.MinInclusive));
					}
					if (fractionDigitsFacet != null) {
						xsdtype.SimpleValues.Add(new SimpleValue(fractionDigitsFacet.Value, SchemaParser.SimpleTypes.FractionDigits));
					}
					if (maxLengthFacet != null) {
						xsdtype.SimpleValues.Add(new SimpleValue(maxLengthFacet.Value, SchemaParser.SimpleTypes.MaxLength));
					}
					if (minLengthFacet != null) {
						xsdtype.SimpleValues.Add(new SimpleValue(minLengthFacet.Value, SchemaParser.SimpleTypes.MinLength));
					}
					if (numericFacet != null) {
						xsdtype.SimpleValues.Add(new SimpleValue(numericFacet.Value, SchemaParser.SimpleTypes.Numeric));
					}
					if (patternFacet != null) {
						xsdtype.SimpleValues.Add(new SimpleValue(patternFacet.Value, SchemaParser.SimpleTypes.Pattern));
					}
					if (totalDigitsFacet != null) {
						xsdtype.SimpleValues.Add(new SimpleValue(totalDigitsFacet.Value, SchemaParser.SimpleTypes.TotalDigits));
					}
				});
			}
			if (simpleunion != null) {
				//throw new Exception("different type");
			}
			if (simplelist != null) {
				//throw new Exception("different type");
			}

			return xsdtype;
		}
		xsdType ParceXsdComplexType(XmlSchemaComplexType complexType) {
			xsdType xsdtype = new xsdType();
			xsdtype.ContentType = ContentTypes.complexType;
			xsdtype.Name = complexType.Name;

			var sequence = complexType.Particle as XmlSchemaSequence;
			var choice = complexType.Particle as XmlSchemaChoice;

			if (sequence != null) {
				ParceXsdSchemaSequence(sequence, xsdtype);
			}
			if (choice != null) {
				ParceXsdSchemaChoise(choice, xsdtype);
			}

			return xsdtype;
		}
		void ParceXsdSchemaChoise(XmlSchemaChoice choise, xsdType xsdtype) {
			choise.Items.ForEach(chItem => {
				var schEl = chItem as XmlSchemaElement;
				var schAny = chItem as XmlSchemaAny;

				if (schEl != null) {
					xsdtype.Content.Add(ParceXsdElement(schEl, ElementTypes.choise));
				}
				if (schAny != null) {
				}
			});
		}
		void ParceXsdSchemaSequence(XmlSchemaSequence sequence, xsdType xsdtype) {
			sequence.Items.ForEach(seqItem => {
				var schEl = seqItem as XmlSchemaElement;
				var schAny = seqItem as XmlSchemaAny;

				if (schEl != null) {
					xsdtype.Content.Add(ParceXsdElement(schEl, ElementTypes.sequence));
				}
				if (schAny != null) {
				}
			});
		}

		xsdElement ParceXsdElement(XmlSchemaElement schemaElement, SchemaParser.ElementTypes eltype) {
			xsdElement xsdelement = new xsdElement();

			xsdelement.Name = schemaElement.Name;
			xsdelement.ElementType = eltype;
			xsdelement.MaxOccurs = schemaElement.MaxOccurs;
			xsdelement.MinOccurs = schemaElement.MinOccurs;

			var complext = schemaElement.SchemaType as XmlSchemaComplexType;
			var simplet = schemaElement.SchemaType as XmlSchemaSimpleType;

			if (complext != null) {
				xsdelement.Type = ParceXsdComplexType(complext);
			}
			if (simplet != null) {
				xsdelement.Type = ParceXsdSimpleType(simplet);
			}

			if (complext == null && simplet == null) {
				if (schemaElement.SchemaTypeName.Namespace == "http://www.w3.org/2001/XMLSchema") {
					xsdelement.Type = new xsdType() { ContentType = ContentTypes.pureType, QName = schemaElement.SchemaTypeName, Name = schemaElement.Name };
				} else {
					if (TypesDict.Keys.Contains(schemaElement.SchemaTypeName.Name)) {
						xsdelement.Type = TypesDict[schemaElement.SchemaTypeName.Name];
					}
				}
			}

			return xsdelement;
		}
		public void Parse(XmlSchemaSet schema) {
			try {
				//types parsing
				schema.GlobalTypes.Values.ForEach(item => {
					//Simple type
					var elstype = item as System.Xml.Schema.XmlSchemaSimpleType;
					if (elstype != null) {
						typesDict.Add(elstype.Name, ParceXsdSimpleType(elstype));
					}
					//Complex type
					var elctype = item as System.Xml.Schema.XmlSchemaComplexType;
					if (elctype != null) {
						if (elctype.Particle != null)
							typesDict.Add(elctype.Name, ParceXsdComplexType(elctype));
					}
				});


				//elements parsing
				schema.GlobalElements.Values.ForEach(item => {
					var schelem = item as XmlSchemaElement;
					if (schelem != null)
						ElementsDict.Add(schelem.Name, ParceXsdElement(schelem, ElementTypes.top));
				});
			} catch (Exception err) {
				//dbg.Error(err);
			}
		}
	}
}
