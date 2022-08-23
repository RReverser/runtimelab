// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization.Schema;
using System.Xml;
using Xunit;
using Xunit.Abstractions;

namespace System.Runtime.Serialization.Schema.Tests
{
    public class RoundTripTest
    {
        private readonly ITestOutputHelper _output;

        public RoundTripTest(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        [ActiveIssue("https://github.com/dotnet/runtime/issues/73961", typeof(PlatformDetection), nameof(PlatformDetection.IsBuiltWithAggressiveTrimming), nameof(PlatformDetection.IsBrowser))]
        public void RountTripTest()
        {
            // AppContext SetSwitch seems to be unreliable in the unit test case. So let's not rely on it
            // for test coverage. But let's do look at the app switch to get our verification correct.
            AppContext.TryGetSwitch("Switch.System.Runtime.Serialization.DataContracts.Auto_Import_KVP", out bool autoImportKVP);

            XsdDataContractExporter exporter = new XsdDataContractExporter();
            exporter.Export(typeof(RootClass));
            XsdDataContractImporter importer = new XsdDataContractImporter();
            importer.Options = new ImportOptions();
            importer.Options.ImportXmlType = true;
            importer.Options.ReferencedTypes.Add(typeof(DBNull));
            importer.Options.ReferencedTypes.Add(typeof(DateTimeOffset));
            importer.Import(exporter.Schemas);

            string code = SchemaUtils.DumpCode(importer.CodeCompileUnit);
            _output.WriteLine(code);

            Assert.Contains(@"This code was generated by a tool.", code);
            Assert.Contains(@"namespace System.Runtime.Serialization.Schema.Tests", code);
            Assert.Contains(@"public partial class RoundTripTestRootClass : object, System.Runtime.Serialization.IExtensibleDataObject", code);
            Assert.Contains(@"[System.Xml.Serialization.XmlRootAttribute(ElementName=""SchemaDefinedType"", Namespace=""http://schemas.datacontract.org/2004/07/System.Runtime.Serialization"")]", code);
            Assert.Contains(@"public partial class dataSetType : object, System.Xml.Serialization.IXmlSerializable", code);
            Assert.Contains(@"[System.Runtime.Serialization.DataContractAttribute(Name=""RoundTripTest.DataContractClass"", Namespace=""http://schemas.datacontract.org/2004/07/System.Runtime.Serialization.Schema.Tests""", code);
            Assert.Contains(@"public partial class RoundTripTestDataContractClass : object, System.Runtime.Serialization.IExtensibleDataObject", code);
            Assert.Contains(@"[System.Runtime.Serialization.DataContractAttribute(Name=""RoundTripTest.DataContractStruct"", Namespace=""http://schemas.datacontract.org/2004/07/System.Runtime.Serialization.Schema.Tests""", code);
            Assert.Contains(@"public partial struct RoundTripTestDataContractStruct : System.Runtime.Serialization.IExtensibleDataObject", code);
            Assert.Contains(@"[System.Runtime.Serialization.DataContractAttribute(Name=""RoundTripTest.EmitDefaultClass"", Namespace=""http://schemas.datacontract.org/2004/07/System.Runtime.Serialization.Schema.Tests""", code);
            Assert.Contains(@"public partial class RoundTripTestEmitDefaultClass : object, System.Runtime.Serialization.IExtensibleDataObject", code);
            Assert.Contains(@"public System.Nullable<System.Runtime.Serialization.Schema.Tests.RoundTripTestDataContractStruct> NullableDataContractStruct2", code);
            Assert.Contains(@"[System.Runtime.Serialization.DataContractAttribute(Name=""RoundTripTest.EncodingMismatchClass"", Namespace=""http://schemas.datacontract.org/2004/07/System.Runtime.Serialization.Schema.Tests""", code);
            Assert.Contains(@"public partial class RoundTripTestEncodingMismatchClass : object, System.Runtime.Serialization.IExtensibleDataObject", code);
            Assert.Contains(@"public enum RoundTripTestMyEnum : int", code);
            Assert.Contains(@"TwoHundred = 200", code);
            Assert.Contains(@"public enum RoundTripTestMyFlagsEnum : int", code);
            Assert.Contains(@"Four = 4,", code);
            Assert.Contains(@"public class ArrayOfNullableOfRoundTripTestMyEnumho3BZmza : System.Collections.Generic.List<System.Runtime.Serialization.Schema.Tests.RoundTripTestMyEnum>", code);
            Assert.Contains(@"namespace schemas.microsoft.com._2003._10.Serialization.Arrays", code);
            Assert.Contains(@"public partial class ArrayOfKeyValueOfintArrayOfstringty7Ep6D1 : object, System.Xml.Serialization.IXmlSerializable", code);
            Assert.Contains(@"private static System.Xml.XmlQualifiedName typeName = new System.Xml.XmlQualifiedName(""ArrayOfKeyValueOfintArrayOfstringty7Ep6D1"", ""http://schemas.microsoft.com/2003/10/Serialization/Arrays"");", code);
            Assert.Contains(@"public partial class ArrayOfKeyValueOfNullableOfunsignedByteNullableOfunsignedByte_ShTDFhl_P : object, System.Xml.Serialization.IXmlSerializable", code);

            if (autoImportKVP)
            {
                Assert.Contains(@"public partial struct KeyValuePairOfintArrayOfstringty7Ep6D1 : System.Runtime.Serialization.IExtensibleDataObject", code);
                Assert.Contains(@"public partial struct KeyValuePairOfNullableOfunsignedByteNullableOfunsignedByte_ShTDFhl_P : System.Runtime.Serialization.IExtensibleDataObject", code);
                Assert.Contains(@"[System.Runtime.Serialization.DataContractAttribute(Name=""KeyValuePairOfstringNullableOfintU6ho3Bhd"", Namespace=""http://schemas.datacontract.org/2004/07/System.Collections.Generic"")]", code);
            }
            else
            {
                Assert.DoesNotContain(@"public partial struct KeyValuePairOfintArrayOfstringty7Ep6D1 : System.Runtime.Serialization.IExtensibleDataObject", code);
                Assert.DoesNotContain(@"public partial struct KeyValuePairOfNullableOfunsignedByteNullableOfunsignedByte_ShTDFhl_P : System.Runtime.Serialization.IExtensibleDataObject", code);
                Assert.DoesNotContain(@"[System.Runtime.Serialization.DataContractAttribute(Name=""KeyValuePairOfstringNullableOfintU6ho3Bhd"", Namespace=""http://schemas.datacontract.org/2004/07/System.Collections.Generic"")]", code);
            }
        }

        [Fact]
        public void IsReferenceType()
        {
            XsdDataContractExporter exporter = new XsdDataContractExporter();
            exporter.Export(typeof(RootIsReferenceContainer));
            XsdDataContractImporter importer = new XsdDataContractImporter();
            importer.Options = new ImportOptions();
            importer.Options.ImportXmlType = true;
            importer.Import(exporter.Schemas);
            string code = SchemaUtils.DumpCode(importer.CodeCompileUnit);
            _output.WriteLine(code);

            Assert.True(code.Length > 616);
        }


#pragma warning disable CS0169, CS0414, IDE0051, IDE1006
        #region RoundTripTest DataContracts
        [DataContract]
        public class RootClass
        {
            [DataMember] MyEnum myEnum;
            [DataMember] MyEnum[] arrayOfMyEnum;
            [DataMember] MyEnum? nullableOfMyEnum;
            [DataMember] MyEnum?[] arrayOfNullableOfMyEnum;
            [DataMember] MyFlagsEnum myFlagsEnum;
            [DataMember] XmlNode[] xmlNodes;
            [DataMember] XmlElement xmlElement;
            [DataMember] DataContractClass dataContractClass;
            [DataMember] DataContractClass[] arrayOfDataContractClass;
            [DataMember] DataContractStruct dataContractStruct;
            [DataMember] DataContractStruct[] arrayOfDataContractStruct;
            [DataMember] DataContractStruct? nullableOfDataContractStruct;
            [DataMember] DataContractStruct?[] arrayOfNullableOfDataContractStruct;
            [DataMember] DataSet dataSet;
            [DataMember] IList<IList<int>> intLists;
            [DataMember] IList<IDictionary<int, IEnumerable<string>>> dictionaries;
            [DataMember] IDictionary<string, int?> nullableValues;
            [DataMember] IDictionary<byte?, byte?> nullableKeyAndValues;
            [DataMember] EmitDefaultClass emitDefaultClass;
            [DataMember] EncodingMismatchClass encodingMismatchClass;
            [DataMember] DBNull dbnull;
        }

        public enum MyEnum { Hundred = 100, TwoHundred = 200 };
        [Flags]
        public enum MyFlagsEnum { Four = 4, Eight = 8 };

        [DataContract]
        public class DataContractClass
        {
            [DataMember] public int IntValue;
            [DataMember] public Guid GuidValue;
            [DataMember] TimeSpan timeSpanValue;
        }

        [DataContract]
        public struct DataContractStruct
        {
            [DataMember] public char CharValue;
            [DataMember] Decimal decimalValue;
            [DataMember] XmlQualifiedName qname;
        }


        [DataContract]
        public class EmitDefaultClass
        {
            [DataMember(EmitDefaultValue = false)]
            public string Name1;
            [DataMember(EmitDefaultValue = false)]
            public int Age1;
            [DataMember(EmitDefaultValue = false)]
            public int? Salary1;
            [DataMember(EmitDefaultValue = false)]
            public DataContractStruct DataContractStruct1;
            [DataMember(EmitDefaultValue = false)]
            public DataContractStruct? NullableDataContractStruct1;
            [DataMember(EmitDefaultValue = false)]
            public DateTime DateTime1;
            [DataMember(EmitDefaultValue = false)]
            public DateTimeOffset DateTimeOffset1;
            [DataMember(EmitDefaultValue = false)]
            public Guid Guid1;
            [DataMember(EmitDefaultValue = false)]
            public Decimal Decimal1;
            [DataMember(EmitDefaultValue = false)]
            public TimeSpan TimeSpan1;

            [DataMember(IsRequired = true, EmitDefaultValue = false)]
            public string Name2;
            [DataMember(IsRequired = true, EmitDefaultValue = false)]
            public int Age2;
            [DataMember(IsRequired = true, EmitDefaultValue = false)]
            public int? Salary2;
            [DataMember(IsRequired = true, EmitDefaultValue = false)]
            public DataContractStruct DataContractStruct2;
            [DataMember(IsRequired = true, EmitDefaultValue = false)]
            public DataContractStruct? NullableDataContractStruct2;
            [DataMember(IsRequired = true, EmitDefaultValue = false)]
            public DateTime DateTime2;
            [DataMember(IsRequired = true, EmitDefaultValue = false)]
            public DateTimeOffset DateTimeOffset2;
            [DataMember(IsRequired = true, EmitDefaultValue = false)]
            public Guid Guid2;
            [DataMember(IsRequired = true, EmitDefaultValue = false)]
            public Decimal Decimal2;
            [DataMember(IsRequired = true, EmitDefaultValue = false)]
            public TimeSpan TimeSpan2;

        }

        [DataContract]
        public class EncodingMismatchClass
        {
            [DataMember(Name = "a:b")]
            public int a;
            [DataMember(Name = "a_x003A_bc_x003C__x003B_")]
            public int b;
            [DataMember(Name = "a_x003a_bc_x003b__x003c_")]
            public int c;
        }
        #endregion

        #region IsReferenceType DataContracts
        [DataContract]
        class RootIsReferenceContainer
        {
            [DataMember]
            RefEdibleItem r = new RefEdibleItem();
            [DataMember]
            Fruit w = new Fruit();
            [DataMember]
            RefApple x = new RefApple();
            [DataMember]
            public RefCustomer customer = RefCustomer.CreateInstance();
            [DataMember]
            RefGrades grades = new RefGrades();
            [DataMember]
            CircularLinkedList_ContainsBackpointingRef clcb = new CircularLinkedList_ContainsBackpointingRef();
            [DataMember]
            RefCircularLinks_ContainsBackpointer rccb = new RefCircularLinks_ContainsBackpointer();
            [DataMember]
            RefCircularNodeA_ContainsRefWithBackpointer rcnacr = new RefCircularNodeA_ContainsRefWithBackpointer();
        }

        [DataContract(IsReference = true)]
        class RefEdibleItem
        {
        }

        [DataContract]
        class Fruit : RefEdibleItem
        {
        }

        [DataContract(IsReference = true)]
        class RefApple : Fruit
        {
        }

        [CollectionDataContract(IsReference = true)]
        public class RefGrades : List<string>
        {
        }

        [DataContract(IsReference = true)]
        class RefCustomer
        {
            [DataMember]
            string Name;
            [DataMember]
            int ZipCode;

            internal static RefCustomer CreateInstance()
            {
                RefCustomer x = new RefCustomer();
                x.Name = "Bill Gates";
                x.ZipCode = 98052;
                return x;
            }
        }


        [DataContract]
        public class CircularLinkedList_ContainsBackpointingRef
        {
            [DataMember]
            RefNode start;

            [DataMember]
            int numberOfNodes;

            public CircularLinkedList_ContainsBackpointingRef()
            {
                numberOfNodes = 4;
                RefNode currentNode = null, prevNode = null;
                start = null;
                for (int i = 0; i < numberOfNodes; i++)
                {
                    currentNode = new RefNode(i, "Hello World");
                    if (i == 0)
                        start = currentNode;
                    if (prevNode != null)
                        prevNode.Next = currentNode;
                    prevNode = currentNode;
                }
                currentNode.Next = start;
            }
        }

        [DataContract(IsReference = true)]
        public class RefNode
        {
            [DataMember]
            public RefNode Next;

            [DataMember]
            int id;

            [DataMember]
            string name;

            public RefNode(int id, string name)
            {
                this.id = id;
                this.name = name;
            }
        }


        [DataContract(IsReference = true)]
        public class RefCircularLinks_ContainsBackpointer
        {
            [DataMember]
            RefCircularLinks_ContainsBackpointer link;

            public RefCircularLinks_ContainsBackpointer()
            {
                link = this;
            }
        }


        [DataContract(IsReference = true)]
        public class RefCircularNodeA_ContainsRefWithBackpointer
        {
            [DataMember]
            RefCircularNodeB_ContainsRefWithBackpointer linkToB;

            public RefCircularNodeA_ContainsRefWithBackpointer()
            {
                linkToB = new RefCircularNodeB_ContainsRefWithBackpointer(this);
            }
        }

        [DataContract(IsReference = true)]
        public class RefCircularNodeB_ContainsRefWithBackpointer
        {
            [DataMember]
            RefCircularNodeA_ContainsRefWithBackpointer linkToA;

            public RefCircularNodeB_ContainsRefWithBackpointer(RefCircularNodeA_ContainsRefWithBackpointer nodeA)
            {
                linkToA = nodeA;
            }
        }
        #endregion
#pragma warning restore CS0169, CS0414, IDE0051, IDE1006
    }
}
