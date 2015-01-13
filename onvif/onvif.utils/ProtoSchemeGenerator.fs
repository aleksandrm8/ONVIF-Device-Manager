module ProtoSchemeGenerator
    open System
    open System.Collections.Generic
    open System.Linq
    open System.Xml
    open System.Xml.Schema
    open System.Xml.Linq
    
    open utils
    open utils.fsharp

    let rec CreateProtoElement(schemaElement:XmlSchemaElement):XElement = 
        if schemaElement.ElementSchemaType |> NotNull then
            let nodes = CreateProtoAnyType(schemaElement.ElementSchemaType)
            new XElement(schemaElement.QualifiedName.ToXName(), nodes |> List.toArray)
        else
            failwith "invalid schema type"

    and CreateProtoAnyType(schemaType:XmlSchemaType):list<XObject> = 
        match schemaType with
        | :? XmlSchemaComplexType as complexType ->
            CreateProtoComplexType(complexType)
        | :? XmlSchemaSimpleType as simpleType ->
            let value = CreateProtoSimpleType(simpleType)
            [new XText(value) :> XNode]
        | _ -> 
            failwith "invalid schema type"

    and CreateProtoComplexType(complexType:XmlSchemaComplexType):list<XObject> = 
        if complexType.ContentModel |> NotNull then
            match complexType.ContentModel with
            | :? XmlSchemaSimpleContent as simpleContent -> 
                CreateProtoSimpleContent(simpleContent, complexType.BaseXmlSchemaType)
            | :? XmlSchemaComplexContent as complexContent -> 
                CreateProtoComplexContent(complexContent, complexType.BaseXmlSchemaType)
            | _ -> 
                failwith ("not implemented")
        else
            let complexContentExt = new XmlSchemaComplexContentExtension()
            if complexType.BaseXmlSchemaType |> NotNull then
                complexContentExt.BaseTypeName <- complexType.BaseXmlSchemaType.QualifiedName
            else
                complexContentExt.BaseTypeName <- null
            
            if complexType.Attributes |> NotNull then
                for i in complexType.Attributes do
                    complexContentExt.Attributes.Add(i) |> ignore
                    
            complexContentExt.Particle <- complexType.Particle
            let complexContent = new XmlSchemaComplexContent()
            complexContent.Content <- complexContentExt
            CreateProtoComplexContent(complexContent, complexType.BaseXmlSchemaType)
        
    and CreateProtoSimpleType(simpleType:XmlSchemaSimpleType):string = 
        if simpleType.QualifiedName.Namespace = XmlSchema.Namespace then
            CreateProtoXsdType(simpleType.QualifiedName.Name)
        else
            match simpleType.Content with 
            | :? XmlSchemaSimpleTypeRestriction as restricted ->
                let facets = restricted.Facets.OfType<XmlSchemaFacet>()
                seq{
                    for facet in facets do
                        match facet with
                        | :? XmlSchemaEnumerationFacet as enumeration ->
                            yield enumeration.Value
                        | :? XmlSchemaMinInclusiveFacet as minIncl ->
                            yield minIncl.Value
                        | :? XmlSchemaMaxLengthFacet as maxLen ->
                            yield ""
                        | _ -> ()
                    failwith "not implemented"
                } |> Seq.head
            | :? XmlSchemaSimpleTypeUnion as union ->
                CreateProtoSimpleType(union.BaseMemberTypes |> Seq.head)
            | _ -> failwith "not implemented"

    and CreateProtoXsdType(name:string):string = 
        match name with
        | "integer" -> "0"
        | "int" -> "0"
        | "unsignedLong" -> "0"
        | "nonNegativeInteger" -> "0"
        | "float" -> "0.0"
        | "double" -> "0.0"
        | "boolean" -> "false"
        | "duration" -> "PT0S"
        | "string" -> "text"
        | "anyURI" -> "uri"
        | "QName" -> "qname"
        | "NCName" -> "ncname"
        | "dateTime" -> XmlConvert.ToString(new DateTime(), XmlDateTimeSerializationMode.Unspecified)
        | _ -> failwith "not implemented"
    
    and CreateProtoParticle(particle:XmlSchemaParticle):list<XElement> = Seq.toList(seq{
        match particle with
        | null -> ()
        | :? XmlSchemaSequence as sequence ->
            yield! CreateProtoSequence(sequence)
        | :? XmlSchemaChoice as choice ->
            yield! CreateProtoChoice(choice)
        | :? XmlSchemaAll as all ->
            yield! CreateProtoAll(all)
        | :? XmlSchemaAny as any ->
            yield new XElement(XName.Get("any"))
        | :? XmlSchemaElement as element ->
            yield CreateProtoElement(element)
        | _ ->
            failwith ("not implemented")
    })

    and CreateProtoAll(all:XmlSchemaAll):list<XElement> = Seq.toList(seq{
        for item in all.Items do
            let particle = item :?> XmlSchemaParticle
            yield! seq{
                yield! CreateProtoParticle(particle)
            } |> Seq.repeat particle.MinOccurs
    })

    and CreateProtoSequence(sequence:XmlSchemaSequence):list<XElement> = Seq.toList(seq{
        for item in sequence.Items do
            let particle = item :?> XmlSchemaParticle
            yield! seq{
                yield! CreateProtoParticle(particle)
            } |> Seq.repeat particle.MinOccurs
    })
    
    and CreateProtoChoice(choice:XmlSchemaChoice):list<XElement> = Seq.toList(seq{
        let particle = choice.Items.OfType<XmlSchemaParticle>().First()
        yield! seq{
            yield! CreateProtoParticle(particle)
        } |> Seq.repeat particle.MinOccurs
    })
        
    and CreateProtoSimpleContent(simpleContent:XmlSchemaSimpleContent, baseType:XmlSchemaType):list<XObject> = Seq.toList(seq{
        let content = simpleContent.Content :?> XmlSchemaSimpleContentExtension

        if baseType |> NotNull then
            yield! CreateProtoAnyType(baseType)

        for a in content.Attributes do
            match a with
            | :? XmlSchemaAttribute as attribute ->
                if attribute.Use = XmlSchemaUse.Required then
                    let name = attribute.QualifiedName.ToXName()
                    let content = 
                        if attribute.FixedValue |> NotNull then 
                            failwith ("not implemented")
                        elif attribute.DefaultValue |> NotNull then 
                            attribute.DefaultValue
                        else
                            CreateProtoSimpleType(attribute.AttributeSchemaType)
                    yield new XAttribute(name, content) :> XObject
                //failwith ("not implemented")
            | _ -> 
                failwith ("not implemented")
    })
            
    and CreateProtoComplexContent(complexContent:XmlSchemaComplexContent, baseType:XmlSchemaType):list<XObject> = Seq.toList(seq{
        let content = complexContent.Content :?> XmlSchemaComplexContentExtension

        if baseType |> NotNull then
            yield! CreateProtoAnyType(baseType)

        for a in content.Attributes do
            match a with
            | :? XmlSchemaAttribute as attribute ->
                if attribute.Use = XmlSchemaUse.Required then
                    let name = attribute.QualifiedName.ToXName()
                    let content = 
                        if attribute.FixedValue |> NotNull then 
                            failwith ("not implemented")
                        elif attribute.DefaultValue |> NotNull then 
                            attribute.DefaultValue
                        else
                            CreateProtoSimpleType(attribute.AttributeSchemaType)
                    yield new XAttribute(name, content) :> XObject
                //failwith ("not implemented")
            | _ -> 
                failwith ("not implemented")

        if content.Particle |> NotNull then
            let particle = content.Particle
            yield! seq{
                yield! CreateProtoParticle(particle) |> Seq.map (fun x->x:>XObject)
            } |> Seq.repeat particle.MinOccurs
    })

