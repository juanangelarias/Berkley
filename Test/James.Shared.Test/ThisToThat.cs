using System.Collections.ObjectModel;

namespace James.Shared.Test;

public class ThisToThatTests
{
    [Fact]
    public void CopySimple()
    {
        var source = new SimpleA { I = 32 };
        var copiedSource = ThisToThat.ToEntityType<SimpleB>(source);
        Assert.Equal(source.I, copiedSource.I);
        Assert.Equal(source.Cheese, copiedSource.Cheese);
    }

    [Fact]
    public void CopyEnumerationOfSimple()
    {
        var source = new[]
        {
            new SimpleA {I = 7},
            new SimpleA {I = 4},
            new SimpleA {I = 8},
            new SimpleA {I = 1},
            new SimpleA {I = 2}
        };
        var copiedSource = ThisToThat.ToEntityType<List<SimpleB>>(source);
        Assert.Equal(source.Length, copiedSource.Count);
        for (var i = 0; i < source.Length; i++)
        {
            Assert.Equal(source[i].I, copiedSource[i].I);
            Assert.Equal(source[i].Cheese, copiedSource[i].Cheese);
        }
    }

    private static Random _rnd = new();

    [Fact]
    public void CopyComplex()
    {
        var source = new ComplexA(_rnd.Next(0, 999));
        var copiedSource = ThisToThat.ToEntityType<ComplexB>(source);
        CompareComplex(source, copiedSource);
    }

    [Fact]
    public void CopyEnumerationOfComplex()
    {
        var sampleSize = _rnd.Next(5, 15);
        var source = Enumerable.Range(0, sampleSize).Select(s => new ComplexA(s)).ToList();
        var copiedSource = ThisToThat.ToEntityType<Collection<ComplexB>>(source);
        Assert.Equal(source.Count, copiedSource.Count);
        for (var i = 0; i < sampleSize; i++)
            CompareComplex(source[i], copiedSource[i]);
    }

    [Fact]
    public void CopyEnum()
    {
        foreach (EnumA enA in Enum.GetValues<EnumA>())
        {
            var origName = Enum.GetName(typeof(EnumA), enA);
            var origClass = new EnumAClass { TestVal = enA };
            var newClass = ThisToThat.ToEntityType<EnumBClass>(origClass);
            var newName = Enum.GetName(typeof(EnumB), newClass.TestVal);
            Assert.Equal(origName.ToLower(), newName.ToLower());
        }
    }

    [Fact]
    public void CopyListOfStrings()
    {
        WithListOfStringsA a = new();
        var b = ThisToThat.ToEntityType<WithListOfStringsB>(a);
        Assert.NotNull(b);
        Assert.NotNull(b.ListOfStrings);
        Assert.Equal(a.ListOfStrings.Count, b.ListOfStrings.Count);
        for( var i =0;i<a.ListOfStrings.Count;i++)
            Assert.Equal(a.ListOfStrings[i], b.ListOfStrings[i]);
    }

    [Fact]
    public void CopyString()
    {
        List<string> a = ["TestString!!", "TestString2!?!"];
        var b = ThisToThat.ToEntityType<List<string>>(a);

        Assert.NotNull(b);
        Assert.Equal(a.Count, b.Count);
        for (var i = 0; i < a.Count; i++)
            Assert.Equal(a[i], b[i]);
    }
    private static void CompareComplex(ComplexA source, ComplexB copiedSource)
    {
        Assert.Equal(source.Id, copiedSource.Id);
        Assert.Equal(source.Name, copiedSource.Name);
        Assert.Equal(source.SimpleList1.Count, copiedSource.SimpleList1.Count);
        for (var i = 0; i < source.SimpleList1.Count; i++)
        {
            Assert.Equal(source.SimpleList1[i].I, copiedSource.SimpleList1[i].I);
        }
        Assert.Equal(source.SimpleList2.Count, copiedSource.SimpleList2.Count);
        for (var i = 0; i < source.SimpleList2.Count; i++)
        {
            Assert.Equal(source.SimpleList2[i].I, copiedSource.SimpleList2[i].I);
        }
        Assert.Equal(source.SubClass1.Id, copiedSource.SubClass1.Id);
        Assert.Equal(source.SubClass1.Name, copiedSource.SubClass1.Name);
        Assert.Equal(source.SubClass1.Simple1, copiedSource.SubClass1.Simple1);
        Assert.Equal(source.SubClass1.Simple2, copiedSource.SubClass1.Simple2);
        Assert.Equal(source.SubClass1.ThisIsAFlag, copiedSource.SubClass1.ThisIsAFlag);
        Assert.Equal(source.SubClass1.Value1, copiedSource.SubClass1.Value1);
        Assert.Equal(source.SubClass1.Value2, copiedSource.SubClass1.Value2);
        Assert.Equal(source.SubClass1.Value3, copiedSource.SubClass1.Value3);
        Assert.Equal(source.SubClass1.Date1, copiedSource.SubClass1.Date1);
        Assert.Equal(source.SubClass1.DateTime1, copiedSource.SubClass1.DateTime1);
        Assert.Equal(source.SubClass1.DateTime2, copiedSource.SubClass1.DateTime2);
        Assert.Equal(source.SubClass2.Id, copiedSource.SubClass2.Id);
        Assert.Equal(source.SubClass2.Name, copiedSource.SubClass2.Name);
        Assert.Equal(source.SubClass2.Simple1, copiedSource.SubClass2.Simple1);
        Assert.Equal(source.SubClass2.Simple2, copiedSource.SubClass2.Simple2);
        Assert.Equal(source.SubClass2.ThisIsAFlag, copiedSource.SubClass2.ThisIsAFlag);
        Assert.Equal(source.SubClass2.Value1, copiedSource.SubClass2.Value1);
        Assert.Equal(source.SubClass2.Value2, copiedSource.SubClass2.Value2);
        Assert.Equal(source.SubClass2.Value3, copiedSource.SubClass2.Value3);
        Assert.Equal(source.SubClass2.Date1, copiedSource.SubClass2.Date1);
        Assert.Equal(source.SubClass2.DateTime1, copiedSource.SubClass2.DateTime1);
        Assert.Equal(source.SubClass2.DateTime2, copiedSource.SubClass2.DateTime2);
    }
    private class SimpleA
    {
        public SimpleA()
        {
        }

        public SimpleA(int seed)
        {
            I = seed;
            Cheese = (seed % 10) switch
            {
                0 => "Mozzarella",
                1 => "Pepper Jack",
                2 => "Colby",
                3 => "Cheddar",
                4 => "Parmesan",
                5 => "Sharp Cheddar",
                6 => "Mild Cheddar",
                7 => "CheeseWiz",
                8 => "Swiss",
                _ => "American"
            };
        }

        public int I { get; set; }
        public string Cheese { get; set; } = "Provolone";
        public string NotInDest { get; set; } = "Shouldn't cause a problem";
    }
    private class SimpleB
    {
        public SimpleB()
        {
        }

        public SimpleB(int seed)
        {
            I = seed;
            Cheese = ((3 + seed) % 10) switch
            {
                0 => "Mozzarella",
                1 => "Pepper Jack",
                2 => "Colby",
                3 => "Feta",
                4 => "Parmesan",
                5 => "Gouda",
                6 => "Brie",
                7 => "CheeseWiz",
                8 => "Swiss",
                _ => "Blue"
            };
        }
        public int I { get; set; }
        public string Cheese { get; set; } = "NotProvolone";
        public string NotFromSource { get; set; } = "Shouldn't cause a problem";
    }

    private class SubClassA
    {
        public SubClassA()
        {
            Id = Guid.NewGuid();
        }

        public SubClassA(int seed) : this()
        {
            Name = "Sample" + seed;
            Simple1 = new SimpleA
            {
                I = seed,
                Cheese = (seed % 5) switch
                {
                    0 => "Mozzarella",
                    1 => "Pepper Jack",
                    2 => "Colby",
                    3 => "Cheddar",
                    _ => "American"
                }
            };
            Simple2 = new SimpleB
            {
                I = seed,
                Cheese = ((seed + 1) % 6) switch
                {
                    0 => "Mozzarella",
                    1 => "Pepper Jack",
                    2 => "Colby",
                    3 => "Cheddar",
                    4 => "Parmesan",
                    _ => "American"
                }
            };
            ThisIsAFlag = seed % 2 == 0;
            Value1 = 1.2345f * seed;
            Value2 = 6.789m * seed;
            Value3 = 0.1234d * seed;
            DateTime1 = DateTime.Now.AddHours(seed);
            DateTime2 = DateTime.Now.AddDays(seed);
            Date1 = DateOnly.FromDateTime(DateTime.Now.AddMonths(seed));
            IncompatibleId = 1L * seed * int.MaxValue;
            NotInB = "Seed:" + seed;
            IncompatibleField = new Guid();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public SimpleA Simple1 { get; set; }
        public SimpleB Simple2 { get; set; }
        public bool ThisIsAFlag { get; set; }
        public float Value1 { get; set; }
        public decimal Value2 { get; set; }
        public double Value3 { get; set; }
        public DateTime DateTime1 { get; set; }
        public DateOnly Date1 { get; set; }
        public DateTimeOffset DateTime2 { get; set; }
        public string NotInB { get; set; }
        public long IncompatibleId { get; set; }
        public Guid IncompatibleField;
    }
    private class SubClassB
    {
        public SubClassB()
        {
            Id = Guid.NewGuid();
        }

        public SubClassB(int seed) : this()
        {
            Name = "Sample" + seed;
            Simple1 = new SimpleA
            {
                I = seed,
                Cheese = (seed % 5) switch
                {
                    0 => "Mozzarella",
                    1 => "Pepper Jack",
                    2 => "Colby",
                    3 => "Cheddar",
                    _ => "American"
                }
            };
            Simple2 = new SimpleB
            {
                I = seed,
                Cheese = ((seed + 1) % 6) switch
                {
                    0 => "Mozzarella",
                    1 => "Pepper Jack",
                    2 => "Colby",
                    3 => "Cheddar",
                    4 => "Parmesan",
                    _ => "American"
                }
            };
            ThisIsAFlag = seed % 2 == 0;
            Value1 = 1.2345f * seed;
            Value2 = 6.789m * seed;
            Value3 = 0.1234d * seed;
            DateTime1 = DateTime.Now.AddHours(seed);
            DateTime2 = DateTime.Now.AddDays(seed);
            Date1 = DateOnly.FromDateTime(DateTime.Now.AddMonths(seed));
            IncompatibleId = new Guid();
            NotInA = 2.345f * seed;
            IncompatibleField = DateTime.Now.AddMinutes(seed * 123);
        }

        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public SimpleA Simple1 { get; set; }
        public SimpleB Simple2 { get; set; }
        public bool ThisIsAFlag { get; set; }
        public float Value1 { get; set; }
        public decimal Value2 { get; set; }
        public double Value3 { get; set; }
        public DateTime DateTime1 { get; set; }
        public DateOnly Date1 { get; set; }
        public DateTimeOffset DateTime2 { get; set; }
        public float NotInA { get; set; }
        public Guid IncompatibleId { get; set; } = Guid.NewGuid();
        public DateTime IncompatibleField;
    }

    private class ComplexA
    {
        public ComplexA() { }

        public ComplexA(int seed)
        {
            var listLength = 4 + seed % 6;
            SimpleList1 = Enumerable.Range(0, listLength)
                .Select(s => new SimpleA(seed)).ToList();
            SimpleList2 = Enumerable.Range(0, listLength)
                .Select(s => new SimpleB(seed)).ToList();
            SubClass1 = new SubClassA(seed);
            SubClass2 = new SubClassB(seed);
            Name = "Seed is " + seed;
            NotInB = 3.4567f * seed;
            PropNotInB = 8.90123f * seed;
        }

        public List<SimpleA> SimpleList1 { get; set; }
        public List<SimpleB> SimpleList2 { get; set; }
        public SubClassA SubClass1 { get; set; }
        public SubClassB SubClass2 { get; set; }
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public float NotInB;
        public float PropNotInB { get; set; }
    }
    private class ComplexB
    {
        public ComplexB() { }
        public ComplexB(int seed)
        {
            var listLength = 4 + seed % 6;
            SimpleList1 = Enumerable.Range(0, listLength)
                .Select(s => new SimpleB(seed)).ToList();
            SimpleList2 = Enumerable.Range(0, listLength)
                .Select(s => new SimpleA(seed)).ToList();
            SubClass1 = new SubClassB(seed);
            SubClass2 = new SubClassA(seed);
            Name = "Seed is " + seed;
            NotInA = 3.4567f * seed;
            PropNotInA = 8.90123f * seed;
        }
        public List<SimpleB> SimpleList1 { get; set; }
        public List<SimpleA> SimpleList2 { get; set; }
        public SubClassB SubClass1 { get; set; }
        public SubClassA SubClass2 { get; set; }
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public float NotInA;
        public float PropNotInA { get; set; }
    }

    private class EnumAClass
    {
        public EnumA TestVal { get; set; }
    }
    private enum EnumA
    {
        a, b, c, d
    }
    private class EnumBClass
    {
        public EnumB TestVal { get; set; }
    }
    private enum EnumB
    {
        A, B, C, D
    }

    private class WithListOfStringsA
    {
        public List<string> ListOfStrings { get; set; } = [.. "It's the end of the world and I feel fine.".Split(' ')];
    }
    private class WithListOfStringsB
    {
        public List<string> ListOfStrings { get; set; } = [];
    }
}