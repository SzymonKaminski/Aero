# Aero
## Packet and file serialization code gen
For the server we are hoping to have a code gen to create the packet reader and writers for the messages and maybe to also be used on the file formats.
The idea is you define the class and annotate the fields and then have the code gen generate the read and write functions, the benfit of this is avoid having to write uguly error prone code over and over again and allows us to make sweeping changes to the readers and writers with out having to re write all those functions.
Planning to use .Net 5 Source Generators. <https://devblogs.microsoft.com/dotnet/introducing-c-source-generators/>

An other plus from this should be better packet peep inspection, the code gen can also create imgui views for each class with better views on the data and better tracking of reads.

Why Aero, well she did talk alot so....

# How to install

TODO

# Attributes
## ``[Aero]``
Marks a class as one that should have readers, writers and such generated for it.

## ``[AeroBlock]``
Marks a struct as one that can be serialised and included in an Aero class.

If a struct isn't marked with this it won't be serialised.

## ``[AeroArray]``
Marks a field as an array, there are a few variants of this.
* ``AeroArray(uint length)`` : eg ``[AeroArray(2)]`` Will read 2 values of the type of the array this is attached to
* ``AeroArray(string nameOfFeild)``: like the normal use only will take the length from the named field.
    * The named field must be a number type, eg, byte, short, int
    * Should use ``nameof`` eg ``[AeroArray(nameof(ArrayLen))]``
* ``AeroArray(Type lengthType)`` read a number type of that type and use that for the length of the array
    * eg. ``AeroArray(typeof(uint))`` Will read a uint and then read that value number of elements.

## ``[AeroIf]``
A field with this will be conditionally serialised if the logic passes.
* ``[AeroIf(nameof(TestValue), 1)]``: Equivalent to ``if (TestValue == 1)`` around the read
* ``[AeroIf(nameof(TestValue), 1, 2)]``: Equivalent to ``if (TestValue == 1 || TestValue == 2)`` around the read
* ``[AeroIf(nameof(TestValue), Op.NotEqual, 1)]``: Equivalent to ``if (TestValue != 1)`` around the read
* ``[AeroIf(nameof(TestValue), Op.HasFlag, Flags.Flag1)]``: Equivalent to ``if (TestValue & Flags.Flag1)`` around the read

Multiple values in the one attribute will be ored as you can see above, to get and logic do like below:
```csharp
[AeroIf(nameof(TestValue), 1)]
[AeroIf(nameof(TestValue), 2)]
public int TestInt;
```

and creates code equivalent to:
```csharp
    if ((Byte == 0) && (Byte == 1))
    {
    
    }
```