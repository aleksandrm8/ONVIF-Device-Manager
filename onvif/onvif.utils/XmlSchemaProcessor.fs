module XmlSchemaProcessor
//    open System
//    open System.Collections.Generic
//    open System.Linq
//    open System.Xml
//    open System.Xml.Schema
//    open System.Xml.Linq
//    
//    open utils
//    
//    type IXmlSchemaContentBuilder<'T> = interface
//        abstract ProcessXsdType: name:string->'T
//        abstract ProcessEnumerationContent: values:seq<string>->'T
//        abstract ProcessPatternContent: pattern:string->'T
//        abstract ProcessNumericContent: pattern:string->'T
//        
//    end
//
//    type IXmlSchemaProcessor<'E> = interface
//        abstract ProcessElement: schemaElement:XmlSchemaElement*content:seq<'E>->'E
//        abstract ProcessSequence: sequence:XmlSchemaSequence->seq<'E>
//        abstract ProcessChoice: choice:XmlSchemaChoice->seq<'E>
//        abstract ProcessAll: all:XmlSchemaAll->seq<'E>
//        abstract ProcessAny: any:XmlSchemaAny->'E
//
//        abstract ProcessAttribute: attribute:XmlSchemaAttribute->'E
//        abstract ProcessAnyAttribute: anyAttribute:XmlSchemaAnyAttribute->'E
//        
//        abstract ProcessXsdType: name:string->'E
//        abstract ProcessEnumerationContent: values:seq<string>->'E
//        abstract ProcessPatternContent: pattern:string->'E
//        abstract ProcessNumericContent: pattern:string->'E
//        
//    end
//
//    let rec ProcessElement(processor:IXmlSchemaProcessor<'T>, schemaElement:XmlSchemaElement):'T = 
//        if schemaElement.ElementSchemaType<>null then
//            let content = seq{
//                yield! CreateProtoAnyType(processor, schemaElement.ElementSchemaType)
//            }
//            processor.ProcessElement(schemaElement, content)
//            //new XElement(schemaElement.QualifiedName.ToXName(), nodes |> List.toArray)
//        else
//            failwith "invalid schema type"        
//
//    and CreateProtoAnyType(processor:IXmlSchemaProcessor<'T>, schemaType:XmlSchemaType) = 
//        match schemaType with
//        | :? XmlSchemaComplexType as complexType ->
//            CreateProtoComplexType(processor, complexType)
//        | :? XmlSchemaSimpleType as simpleType ->
//            CreateProtoSimpleType(processor, simpleType)
//            //let value = CreateProtoSimpleType(processor, simpleType)
//            //[new XText(value) :> XNode]
//        | _ -> 
//            failwith "invalid schema type"
//
//    and CreateProtoComplexType(processor:IXmlSchemaProcessor, complexType:XmlSchemaComplexType) = 
//        if complexType.ContentModel<>null then
//            match complexType.ContentModel with
//            | :? XmlSchemaSimpleContent as simpleContent -> 
//                CreateProtoSimpleContent(processor, simpleContent, complexType.BaseXmlSchemaType)
//            | :? XmlSchemaComplexContent as complexContent -> 
//                CreateProtoComplexContent(processor, complexContent, complexType.BaseXmlSchemaType)
//            | _ -> 
//                failwith ("not implemented")
//        else
//            let complexContentExt = new XmlSchemaComplexContentExtension()
//            if complexType.BaseXmlSchemaType<>null then
//                complexContentExt.BaseTypeName <- complexType.BaseXmlSchemaType.QualifiedName
//            else
//                complexContentExt.BaseTypeName <- null
//            
//            if complexType.Attributes<> null then
//                for i in complexType.Attributes do
//                    complexContentExt.Attributes.Add(i) |> ignore
//                    
//            complexContentExt.Particle <- complexType.Particle
//            let complexContent = new XmlSchemaComplexContent()
//            complexContent.Content <- complexContentExt
//            CreateProtoComplexContent(processor, complexContent, complexType.BaseXmlSchemaType)
//        
//    and CreateProtoSimpleType(processor:IXmlSchemaProcessor<'T>, simpleType:XmlSchemaSimpleType) = 
//        if simpleType.QualifiedName.Namespace = XmlSchema.Namespace then
//            processor.ProcessXsdType(simpleType.QualifiedName.Name)
//            //CreateProtoXsdType(simpleType.QualifiedName.Name)
//        else
//            match simpleType.Content with 
//            | :? XmlSchemaSimpleTypeRestriction as restricted ->
//                let facets = restricted.Facets.OfType<XmlSchemaFacet>()
//                let enumeration = restricted.Facets.OfType<XmlSchemaEnumerationFacet>()
//                if not(Seq.isEmpty(enumeration)) then
//                    
//                seq{
//                    for facet in facets do
//                        match facet with
//                        | :? XmlSchemaEnumerationFacet as enumeration ->
//                            yield enumeration.Value
//                        | :? XmlSchemaMinInclusiveFacet as minIncl ->
//                            yield minIncl.Value
//                        | :? XmlSchemaMaxLengthFacet as maxLen ->
//                            yield ""
//                        | _ -> ()
//                    failwith "not implemented"
//                } |> Seq.head
//            | :? XmlSchemaSimpleTypeUnion as union ->
//                CreateProtoSimpleType(union.BaseMemberTypes |> Seq.head)
//            | _ -> failwith "not implemented"
//
//    //and CreateProtoXsdType(processor:IXmlSchemaProcessor<'T>, name:string) = 
//        
////        match name with
////        | "integer" -> "0"
////        | "int" -> "0"
////        | "unsignedLong" -> "0"
////        | "nonNegativeInteger" -> "0"
////        | "float" -> "0.0"
////        | "boolean" -> "false"
////        | "duration" -> "PT0S"
////        | "string" -> "text"
////        | "anyURI" -> "uri"
////        | "QName" -> "qname"
////        | "NCName" -> "ncname"
////        | "dateTime" -> XmlConvert.ToString(new DateTime(), XmlDateTimeSerializationMode.Unspecified)
////        | _ -> failwith "not implemented"
//    
//    and CreateProtoParticle(processor:IXmlSchemaProcessor, particle:XmlSchemaParticle):list<XElement> = Seq.toList(seq{
//        match particle with
//        | null -> ()
//        | :? XmlSchemaSequence as sequence ->
//            yield! CreateProtoSequence(processor, sequence)
//        | :? XmlSchemaChoice as choice ->
//            yield! CreateProtoChoice(processor, choice)
//        | :? XmlSchemaAll as all ->
//            yield! CreateProtoAll(processor, all)
//        | :? XmlSchemaAny as any ->
//            processor.P
//            yield new XElement(XName.Get("any"))
//        | :? XmlSchemaElement as element ->
//            yield CreateProtoElement(processor, element)
//        | _ ->
//            failwith ("not implemented")
//    })
//
//    and CreateProtoAll(processor:IXmlSchemaProcessor, all:XmlSchemaAll) = 
//        let contentBuilder(processor) = 
//            for item in all.Items do
//                let particle = item :?> XmlSchemaParticle
//                CreateProtoParticle(processor, particle)
//        processor.ProcessAll(processor, contentBuilder)
//            yield! seq{
//                yield! CreateProtoParticle(particle)
//            } |> Seq.repeat particle.MinOccurs
//
//    and CreateProtoSequence(processor:IXmlSchemaProcessor, sequence:XmlSchemaSequence) = 
//        let itemsBuilder(processor) = seq{
//            for item in sequence.Items do
//                let particle = item :?> XmlSchemaParticle
//                yield CreateProtoParticle(particle)
//        }
//    
//    and CreateProtoChoice(choice:XmlSchemaChoice):list<XElement> = Seq.toList(seq{
//        let particle = choice.Items.OfType<XmlSchemaParticle>().First()
//        yield! seq{
//            yield! CreateProtoParticle(particle)
//        } |> Seq.repeat particle.MinOccurs
//    })
//        
//    and CreateProtoSimpleContent(simpleContent:XmlSchemaSimpleContent, baseType:XmlSchemaType):list<XObject> = Seq.toList(seq{
//        let content = simpleContent.Content :?> XmlSchemaSimpleContentExtension
//
//        if baseType<>null then
//            yield! CreateProtoAnyType(baseType)
//
//        for a in content.Attributes do
//            match a with
//            | :? XmlSchemaAttribute as attribute ->
//                if attribute.Use = XmlSchemaUse.Required then
//                    let name = attribute.QualifiedName.ToXName()
//                    let content = 
//                        if attribute.FixedValue <> null then 
//                            failwith ("not implemented")
//                        elif attribute.DefaultValue <> null then 
//                            attribute.DefaultValue
//                        else
//                            CreateProtoSimpleType(attribute.AttributeSchemaType)
//                    yield new XAttribute(name, content) :> XObject
//                //failwith ("not implemented")
//            | _ -> 
//                failwith ("not implemented")
//    })
//            
//    and CreateProtoComplexContent(complexContent:XmlSchemaComplexContent, baseType:XmlSchemaType):list<XObject> = Seq.toList(seq{
//        let content = complexContent.Content :?> XmlSchemaComplexContentExtension
//
//        if baseType<> null then
//            yield! CreateProtoAnyType(baseType)
//
//        for a in content.Attributes do
//            match a with
//            | :? XmlSchemaAttribute as attribute ->
//                if attribute.Use = XmlSchemaUse.Required then
//                    let name = attribute.QualifiedName.ToXName()
//                    let content = 
//                        if attribute.FixedValue <> null then 
//                            failwith ("not implemented")
//                        elif attribute.DefaultValue <> null then 
//                            attribute.DefaultValue
//                        else
//                            CreateProtoSimpleType(attribute.AttributeSchemaType)
//                    yield new XAttribute(name, content) :> XObject
//                //failwith ("not implemented")
//            | _ -> 
//                failwith ("not implemented")
//
//        if content.Particle<>null then
//            let particle = content.Particle
//            yield! seq{
//                yield! CreateProtoParticle(particle) |> Seq.map (fun x->x:>XObject)
//            } |> Seq.repeat particle.MinOccurs
//    })
//
