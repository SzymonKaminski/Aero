﻿using System;
using System.Reflection;
using Aero.Gen;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Aero.TestCases
{
    public partial class TestCase1Main
    {
        public void AeroTest2() {}
    }
    
    class Program
    {
        private static Compilation InputCompilation = CreateCompilation(@"
using System.Net.Security;
using Aero.Gen.Attributes;

namespace Aero.TestCases
{

    [Flags]
    public enum TestFlags : byte
    {
        Flag1,
        Flag2,
        Flag3,
        Flag4
    }

    [AeroBlock]
    public struct TestSubDataOne
    {
        public byte   Byte;
        public char   Char;
        public int    IntTest;
        public uint   UintTest;

        [AeroArray(5)]
        public int[] ArrayTest;
    }

    [AeroBlock]
    public struct TestSubDataTwo
    {
        public byte   Byte;
        public char   Char;
        public int    IntTest;
        public uint   UintTest;
        public TestSubDataOne SubDataTwo;
    }

public class Test2
{
    [Aero]
    public partial class TestCase1
    {
        
        public TestSubDataTwo IntArray4;

        [AeroArray(typeof(int))]
        public TestSubDataTwo[] IntArray4;

        public TestFlags Flags;

        public byte   Byte;
        public char   Char;
        public int    IntTest;
        public uint   UintTest;
        public short  ShortTest;
        public ushort UshortTest;
        public long   Long;
        public ulong  ULong;
        public float  Float;
        
        [AeroIf(""IntTest"", -100)]
        public double Double;
        
        [AeroIf(nameof(IntTest), 100)]
        [AeroIf(nameof(IntTest), 200)]
        [AeroArray(nameof(Byte))]
        public int[] IntArray;

        [AeroIf(nameof(Byte), AeroIfAttribute.Ops.NotEqual, 0.5f, 1.0f)]
        [AeroArray(2)]
        public int[] IntArray2;

        //[AeroArray(typeof(int))]
        //public int[] IntArray3;

        public TestSubDataOne SubDataOne;
        public TestSubDataTwo SubData2;

        public TestCase1()
        {
            
        }
    }
}
}");
        
        static void Main(string[] args)
        {
            AeroGenerator   generator = new AeroGenerator();
            GeneratorDriver driver    = CSharpGeneratorDriver.Create(generator);
            driver = driver.RunGeneratorsAndUpdateCompilation(InputCompilation, out var outputCompilation, out var diagnostics);

            foreach (var diag in diagnostics) {
                Console.WriteLine(diag);
            }

            var data = new byte[] { 0x02, 0x01, 0x41, 0x9C, 0xFF, 0xFF, 0xFF, 0x64, 0x00, 0x00, 0x00, 0xCE, 0xFF, 0x32, 0x00, 0xC0, 0xBD,
                0xF0, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0x40, 0x42, 0x0F, 0x00, 0x00, 0x00, 0x00, 0x00, 0x9A, 0x99,
                0x99, 0x3F, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x04, 0x40 }.AsSpan();
            var test = new TestCase1Main();
            test.Unpack(data);

            /*foreach (var log in test.DiagLogs) {
                Console.WriteLine(log);
            }*/

        }
        
        private static Compilation CreateCompilation(string source)
            => CSharpCompilation.Create("compilation",
                new[] { CSharpSyntaxTree.ParseText(source) },
                new[] { MetadataReference.CreateFromFile(typeof(Binder).GetTypeInfo().Assembly.Location) },
                new CSharpCompilationOptions(OutputKind.ConsoleApplication));
    }
}