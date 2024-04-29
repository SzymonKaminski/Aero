using System;
using System.Text;
using Aero.Gen.Attributes;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Aero.Gen
{
    public partial class Genv2
    {
        private void GenerateEncounterClassMembers(ClassDeclarationSyntax cd, SemanticModel sm)
        {
            var encounterAttr = AgUtils.NodeWithName<AttributeSyntax>(cd, AeroEncounterAttribute.Name);

            var encounterType = encounterAttr.ArgumentList.Arguments[0].Expression.ToString().Trim('"');

            string AddFieldName(string fieldName)
            {
                StringBuilder byteString = new StringBuilder();

                foreach (byte b in fieldName)
                {
                    byteString.AppendFormat("0x{0:X2}, ", b);
                }

                byteString.Append("0x00,");

                return byteString.ToString();
            }

            using (Function("public static byte[] GetHeader()"))
            {
                var rootNode = AeroSourceGraphGen.BuildTree(SyntaxReceiver, cd);
                var fieldIdx = 0;

                AddLine("var data = new byte[]");
                AddLine("{");
                Indent();

                AddLine(AddFieldName(encounterType) + $" // {encounterType}");

                AeroSourceGraphGen.WalkTree(rootNode, node =>
                {
                    if (node.Depth != 0)
                    {
                        return;
                    }

                    var name = node.Name;
                    var count = 1;


                    if (node is AeroArrayNode arr)
                    {
                        name = arr.Nodes[0].Name;
                        count = arr.Length;
                    }

                    var byteIdx = BitConverter.ToString(new[]{(byte)fieldIdx});
                    var byteCount = BitConverter.ToString(new[]{(byte)count});

                    byte byteType = node.TypeStr switch
                    {
                        "uint"                               => 0,
                        "float"                              => 1,
                        string t when t.EndsWith("EntityId") => 2,
                        "ulong"                              => 3,
                        "byte"                               => 4,
                        // there's no type 5
                        "ushort"                             => 6,
                        string t when t.EndsWith("Timer")    => 7,
                        "bool"                               => 8,

                        "uint[]"                               => 128,
                        "float[]"                              => 129,
                        string t when t.EndsWith("EntityId[]") => 130,
                        "ulong[]"                              => 131,
                        "byte[]"                               => 132,
                        // there's no type 5 + 128 = 133 either
                        "ushort[]"                             => 134,
                        string t when t.EndsWith("Timer[]")    => 135,
                        "bool[]"                               => 136,
                        _ => 9
                    };

                    var typeStr = BitConverter.ToString(new[] { byteType });


                    AddLine($"0x{byteIdx}, 0x{typeStr}, 0x{byteCount}, // idx: {fieldIdx}, type: {node.TypeStr}, count: {count}, name: {name}");
                    AddLine(AddFieldName(name));

                    fieldIdx++;
                });

                UnIndent();
                AddLine("};");

                AddLine("return data;");
            }

            AddLine();
        }
    }
}