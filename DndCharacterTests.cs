using System;
using Dnd;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace OOP_Lab5.Tests;

// за AAA: Arrange, Act, Assert стат MaxLevel повертається до початкового значення після кожного тесту.
[TestClass]
[DoNotParallelize]
public class DndCharacterTests
{
    // персонаж у тестах
    private DndCharacter hero = null!;

    //перед тестом
    [TestInitialize]
    public void SetUp()
    {
        DndCharacter.MaxLevel = 20;
        hero = new DndCharacter("Arthur", CharacterClass.Warrior, 5, 150);
    }

    //після тесту
    [TestCleanup]
    public void TearDown()
    {
        DndCharacter.MaxLevel = 20;
        hero = null!;
    }

    //тест конструкторів
    [TestMethod]
    public void Constructor_Default_SetsExpectedProperties() // тестуємо конструктор без параметрів
    {
        // Arrange + Act
        var character = new DndCharacter();
        // Assert
        Assert.AreEqual("Безіменний герой", character.Name);
        Assert.AreEqual(CharacterClass.Warrior, character.Class);
        Assert.AreEqual(1, character.Level);
        Assert.AreEqual(100, character.Health);
    }

    [TestMethod]
    public void Constructor_NameOnly_SetsDefaults() // тестуємо конструктор з параметром імені
    {
        var character = new DndCharacter("Lira");
        Assert.AreEqual("Lira", character.Name);
        Assert.AreEqual(CharacterClass.Warrior, character.Class);
        Assert.AreEqual(1, character.Level);
    }

    [TestMethod]
    public void Constructor_NameAndClass_SetsDefaults() // тестуємо конструктор з параметрами імені та класу
    {
        var character = new DndCharacter("Lira", CharacterClass.Mage);
        Assert.AreEqual(CharacterClass.Mage, character.Class);
        Assert.AreEqual(100, character.Health);
    }

    [TestMethod]
    public void Constructor_Full_SetsAllProperties() // тестуємо конструктор з усіма параметрами
    {
        Assert.AreEqual("Arthur", hero.Name);
        Assert.AreEqual(CharacterClass.Warrior, hero.Class);
        Assert.AreEqual(5, hero.Level);
        Assert.AreEqual(150, hero.Health);
        Assert.AreEqual(0, hero.Experience);
        Assert.AreEqual("Guild", hero.Faction);
    }

    [TestMethod]
    public void Constructor_ValidCreation_IncrementsStaticCount() // тестуємо, що при створенні нового персонажа лічильник збільшується
    {
        int before = DndCharacter.CharacterCount;
        var other = new DndCharacter("Bob");
        Assert.AreEqual(before + 1, DndCharacter.CharacterCount);
        Assert.IsNotNull(other);
    }

    [TestMethod]
    public void Constructor_InvalidCreation_DoesNotIncrementCount() // тестуємо, що при створенні нового персонажа з некоректними параметрами лічильник не збільшується
    {
        int before = DndCharacter.CharacterCount;
        Assert.ThrowsExactly<ArgumentException>(() => new DndCharacter("", CharacterClass.Warrior, 1, 100)); 
        Assert.AreEqual(before, DndCharacter.CharacterCount);
    }

    [DataTestMethod] // тестуємо встановленні 
    [DataRow("")]
    [DataRow("   ")]
    public void Name_Empty_Throws(string name) 
    {
        Assert.ThrowsExactly<ArgumentException>(() => hero.Name = name);
    }

    [TestMethod]
    public void Name_Valid_ChangesValue()
    {
        hero.Name = "Merlin";
        Assert.AreEqual("Merlin", hero.Name);
    }

    [TestMethod]
    public void Class_InvalidEnum_Throws()
    {
        Assert.ThrowsExactly<ArgumentException>(() => hero.Class = (CharacterClass)99);
    }

    [TestMethod]
    public void Class_Valid_ChangesValue()
    {
        hero.Class = CharacterClass.Rogue;
        Assert.AreEqual(CharacterClass.Rogue, hero.Class);
    }

    [DataTestMethod] // тестуємо встановлення рівня персонажа
    [DataRow(0)]
    [DataRow(21)]
    [DataRow(-5)]
    public void Level_OutsideRange_Throws(int level)
    {
        Assert.ThrowsExactly<ArgumentException>(() => hero.Level = level);
    }

    [DataTestMethod] 
    [DataRow(1)]
    [DataRow(20)]
    public void Level_Boundaries_Accepted(int level)
    {
        hero.Level = level;
        Assert.AreEqual(level, hero.Level);
    }

    [TestMethod]
    public void Health_Negative_Throws()
    {
        Assert.ThrowsExactly<ArgumentException>(() => hero.Health = -1);
    }

    [TestMethod]
    public void Health_Zero_IsAllowed()
    {
        hero.Health = 0;
        Assert.AreEqual(0, hero.Health);
    }

    [TestMethod]
    public void Faction_Set_ChangesValue()
    {
        hero.Faction = "Alliance";
        Assert.AreEqual("Alliance", hero.Faction);
    }

    [TestMethod]
    public void FullTitle_ReturnsCalculatedText()
    {
        Assert.AreEqual("Arthur (Warrior) - рівень 5", hero.FullTitle);
    }

    [DataTestMethod]
    [DataRow(1, 100)]
    [DataRow(5, 140)]
    [DataRow(20, 290)]
    public void CalculateRecommendedHealth_ValidLevel_ReturnsExpected(int level, int expected)
    {
        int actual = DndCharacter.CalculateRecommendedHealth(level);
        Assert.AreEqual(expected, actual);
    }

    [DataTestMethod]
    [DataRow(0)]
    [DataRow(21)]
    public void CalculateRecommendedHealth_InvalidLevel_Throws(int level)
    {
        Assert.ThrowsExactly<ArgumentException>(() => DndCharacter.CalculateRecommendedHealth(level));
    }

    [TestMethod]
    public void MaxLevel_ValidUpdate_AffectsValidation()
    {
        DndCharacter.MaxLevel = 30;
        hero.Level = 25;
        Assert.AreEqual(30, DndCharacter.MaxLevel);
        Assert.AreEqual(340, DndCharacter.CalculateRecommendedHealth(25));
    }

    [TestMethod]
    public void MaxLevel_Zero_Throws()
    {
        Assert.ThrowsExactly<ArgumentException>(() => DndCharacter.MaxLevel = 0);
    }

    [DataTestMethod]
    [DataRow(100, 100)]
    [DataRow(0, 0)]
    [DataRow(-10, 0)]
    public void GainExperience_Amount_ChangesExperienceCorrectly(int amount, int expected)
    {
        hero.GainExperience(amount);
        Assert.AreEqual(expected, hero.Experience);
    }

    [TestMethod]
    public void GainExperience_WithReason_AddsExperience()
    {
        hero.GainExperience(40, "квест");
        Assert.AreEqual(40, hero.Experience);
    }

    [TestMethod]
    public void GainExperience_TwoOverloads_Accumulate()
    {
        hero.GainExperience(30);
        hero.GainExperience(20, "бій");
        Assert.AreEqual(50, hero.Experience);
    }

    [DataTestMethod]
    [DataRow(20, 130)]
    [DataRow(150, 0)]
    [DataRow(200, 0)]
    [DataRow(-5, 150)]
    public void TakeDamage_Amount_UpdatesHealthWithoutNegativeValues(int damage, int expected)
    {
        hero.TakeDamage(damage);
        Assert.AreEqual(expected, hero.Health);
    }

    [TestMethod]
    public void TakeDamage_WithType_UpdatesHealth()
    {
        hero.TakeDamage(30, "вогонь");
        Assert.AreEqual(120, hero.Health);
    }

    [TestMethod]
    public void TakeDamage_TwoOverloads_AccumulateDamage()
    {
        hero.TakeDamage(20);
        hero.TakeDamage(30, "магія");
        Assert.AreEqual(100, hero.Health);
    }

    [TestMethod]
    public void ToString_UsesSupportedFormat()
    {
        Assert.AreEqual("Arthur;Warrior;5;150", hero.ToString());
    }

    [TestMethod]
    public void Parse_ValidText_ReturnsCharacter()
    {
        DndCharacter actual = DndCharacter.Parse("Merlin;Mage;8;170");
        Assert.AreEqual("Merlin", actual.Name);
        Assert.AreEqual(CharacterClass.Mage, actual.Class);
        Assert.AreEqual(8, actual.Level);
        Assert.AreEqual(170, actual.Health);
    }

    [TestMethod] 
    public void Parse_ClassIsCaseInsensitive() 
    {
        var actual = DndCharacter.Parse("Lira;mage;4;90");
        Assert.AreEqual(CharacterClass.Mage, actual.Class);
    }

    [TestMethod]
    public void Parse_NameWithSpaces_IsSupported()
    {
        var actual = DndCharacter.Parse("Sir Arthur;Warrior;5;150");
        Assert.AreEqual("Sir Arthur", actual.Name);
    }

    [TestMethod]
    public void Parse_ToString_RoundtripPreservesFourFields()
    {
        var actual = DndCharacter.Parse(hero.ToString());
        Assert.AreEqual(hero.Name, actual.Name);
        Assert.AreEqual(hero.Class, actual.Class);
        Assert.AreEqual(hero.Level, actual.Level);
        Assert.AreEqual(hero.Health, actual.Health);
    }

    [DataTestMethod]
    [DataRow("")]
    [DataRow(" ")]
    public void Parse_EmptyText_ThrowsArgumentException(string text)
    {
        Assert.ThrowsExactly<ArgumentException>(() => DndCharacter.Parse(text));
    }

    [DataTestMethod]
    [DataRow("Arthur;Warrior;5")]
    [DataRow("Arthur;Warrior;5;150;extra")]
    [DataRow(";Warrior;5;150")]
    [DataRow("Arthur;Paladin;5;150")]
    [DataRow("Arthur;99;5;150")]
    [DataRow("Arthur;Warrior;abc;150")]
    [DataRow("Arthur;Warrior;5;abc")]
    public void Parse_BadFormat_ThrowsFormatException(string text)
    {
        Assert.ThrowsExactly<FormatException>(() => DndCharacter.Parse(text));
    }

    [DataTestMethod]
    [DataRow("Arthur;Warrior;0;150")]
    [DataRow("Arthur;Warrior;21;150")]
    [DataRow("Arthur;Warrior;5;-1")]
    public void Parse_InvalidValue_ThrowsArgumentException(string text)
    {
        Assert.ThrowsExactly<ArgumentException>(() => DndCharacter.Parse(text));
    }

    [TestMethod]
    public void TryParse_ValidText_ReturnsTrueAndObject()
    {
        bool result = DndCharacter.TryParse("Lira;Rogue;3;120", out var actual);
        Assert.IsTrue(result);
        Assert.IsNotNull(actual);
        Assert.AreEqual(CharacterClass.Rogue, actual.Class);
    }

    [DataTestMethod]
    [DataRow("")]
    [DataRow("invalid")]
    [DataRow("Arthur;Mage;0;100")]
    [DataRow("Arthur;Other;3;100")]
    public void TryParse_InvalidText_ReturnsFalseAndNull(string text)
    {
        bool result = DndCharacter.TryParse(text, out var actual);
        Assert.IsFalse(result);
        Assert.IsNull(actual);
    }

    [TestMethod]
    public void TryParse_InvalidText_DoesNotIncrementCount()
    {
        int before = DndCharacter.CharacterCount;
        bool result = DndCharacter.TryParse("bad", out var actual);
        Assert.IsFalse(result);
        Assert.IsNull(actual);
        Assert.AreEqual(before, DndCharacter.CharacterCount);
    }
}
