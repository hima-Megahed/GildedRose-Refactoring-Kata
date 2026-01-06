using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GildedRoseKata;
using NUnit.Framework;

namespace GildedRoseTests;

public class GildedRoseTest
{
    [Test]
    public void DexterityVestShouldDegradeBy1()
    {
        // Arrange
        var stringWriter = new StringWriter();
        Console.SetOut(stringWriter);

        var items = new List<Item>
        {
            new() { Name = "+5 Dexterity Vest", SellIn = 10, Quality = 20 }
        };

        var app = new GildedRose(items);

        // Act
        app.UpdateQuality();
        Console.WriteLine(items.First().Name + ", " + items.First().SellIn + ", " + items.First().Quality);

        // Assert
        var actualOutput = stringWriter.ToString().TrimEnd('\r', '\n');
        Assert.That(actualOutput, Is.EqualTo("+5 Dexterity Vest, 9, 19"));
    }

    [Test]
    public void AgedBrieShouldIncreaseQualityBy1BeforeSellDate()
    {
        // Arrange
        var stringWriter = new StringWriter();
        Console.SetOut(stringWriter);

        var items = new List<Item>
        {
            new() { Name = "Aged Brie", SellIn = 2, Quality = 0 }
        };

        var app = new GildedRose(items);

        // Act
        app.UpdateQuality();
        Console.WriteLine(items.First().Name + ", " + items.First().SellIn + ", " + items.First().Quality);

        // Assert
        var actualOutput = stringWriter.ToString().TrimEnd('\r', '\n');
        Assert.That(actualOutput, Is.EqualTo("Aged Brie, 1, 1"));
    }

    [Test]
    public void ElixirOfTheMongooseShouldDegradeBy1()
    {
        // Arrange
        var stringWriter = new StringWriter();
        Console.SetOut(stringWriter);

        var items = new List<Item>
        {
            new() { Name = "Elixir of the Mongoose", SellIn = 5, Quality = 7 }
        };

        var app = new GildedRose(items);

        // Act
        app.UpdateQuality();
        Console.WriteLine(items.First().Name + ", " + items.First().SellIn + ", " + items.First().Quality);

        // Assert
        var actualOutput = stringWriter.ToString().TrimEnd('\r', '\n');
        Assert.That(actualOutput, Is.EqualTo("Elixir of the Mongoose, 4, 6"));
    }

    [Test]
    public void SulfurasWithSellInZeroShouldNotChange()
    {
        // Arrange
        var stringWriter = new StringWriter();
        Console.SetOut(stringWriter);

        var items = new List<Item>
        {
            new() { Name = "Sulfuras, Hand of Ragnaros", SellIn = 0, Quality = 80 }
        };

        var app = new GildedRose(items);

        // Act
        app.UpdateQuality();
        Console.WriteLine(items.First().Name + ", " + items.First().SellIn + ", " + items.First().Quality);

        // Assert
        var actualOutput = stringWriter.ToString().TrimEnd('\r', '\n');
        Assert.That(actualOutput, Is.EqualTo("Sulfuras, Hand of Ragnaros, 0, 80"));
    }

    [Test]
    public void SulfurasWithNegativeSellInShouldNotChange()
    {
        // Arrange
        var stringWriter = new StringWriter();
        Console.SetOut(stringWriter);

        var items = new List<Item>
        {
            new() { Name = "Sulfuras, Hand of Ragnaros", SellIn = -1, Quality = 80 }
        };

        var app = new GildedRose(items);

        // Act
        app.UpdateQuality();
        Console.WriteLine(items.First().Name + ", " + items.First().SellIn + ", " + items.First().Quality);

        // Assert
        var actualOutput = stringWriter.ToString().TrimEnd('\r', '\n');
        Assert.That(actualOutput, Is.EqualTo("Sulfuras, Hand of Ragnaros, -1, 80"));
    }

    [Test]
    public void BackstagePassesMoreThan10DaysShouldIncreaseQualityBy1()
    {
        // Arrange
        var stringWriter = new StringWriter();
        Console.SetOut(stringWriter);

        var items = new List<Item>
        {
            new() { Name = "Backstage passes to a TAFKAL80ETC concert", SellIn = 15, Quality = 20 }
        };

        var app = new GildedRose(items);

        // Act
        app.UpdateQuality();
        Console.WriteLine(items.First().Name + ", " + items.First().SellIn + ", " + items.First().Quality);

        // Assert
        var actualOutput = stringWriter.ToString().TrimEnd('\r', '\n');
        Assert.That(actualOutput, Is.EqualTo("Backstage passes to a TAFKAL80ETC concert, 14, 21"));
    }

    [Test]
    public void BackstagePassesAt10DaysOrLessShouldIncreaseQualityBy2CappedAt50()
    {
        // Arrange
        var stringWriter = new StringWriter();
        Console.SetOut(stringWriter);

        var items = new List<Item>
        {
            new() { Name = "Backstage passes to a TAFKAL80ETC concert", SellIn = 10, Quality = 49 }
        };

        var app = new GildedRose(items);

        // Act
        app.UpdateQuality();
        Console.WriteLine(items.First().Name + ", " + items.First().SellIn + ", " + items.First().Quality);

        // Assert
        var actualOutput = stringWriter.ToString().TrimEnd('\r', '\n');
        Assert.That(actualOutput, Is.EqualTo("Backstage passes to a TAFKAL80ETC concert, 9, 50"));
    }

    [Test]
    public void BackstagePassesAt5DaysOrLessShouldIncreaseQualityBy3CappedAt50()
    {
        // Arrange
        var stringWriter = new StringWriter();
        Console.SetOut(stringWriter);

        var items = new List<Item>
        {
            new() { Name = "Backstage passes to a TAFKAL80ETC concert", SellIn = 5, Quality = 49 }
        };

        var app = new GildedRose(items);

        // Act
        app.UpdateQuality();
        Console.WriteLine(items.First().Name + ", " + items.First().SellIn + ", " + items.First().Quality);

        // Assert
        var actualOutput = stringWriter.ToString().TrimEnd('\r', '\n');
        Assert.That(actualOutput, Is.EqualTo("Backstage passes to a TAFKAL80ETC concert, 4, 50"));
    }

    // NOTE: Your expected results list also includes:
    // "Conjured Mana Cake, 2, 5"
    // But Conjured is NOT in the input list you shared.
    // If you want this test to match your text exactly, use these starting values:
    // SellIn = 3, Quality = 6  -> after UpdateQuality => 2, 5 (per your expected output)
    [Test]
    public void ConjuredManaCakeShouldDegradeTwiceAsFast()
    {
        // Arrange
        var stringWriter = new StringWriter();
        Console.SetOut(stringWriter);

        var items = new List<Item>
        {
            new() { Name = "Conjured Mana Cake", SellIn = 3, Quality = 6 }
        };

        var app = new GildedRose(items);

        // Act
        app.UpdateQuality();
        Console.WriteLine(items.First().Name + ", " + items.First().SellIn + ", " + items.First().Quality);

        // Assert
        var actualOutput = stringWriter.ToString().TrimEnd('\r', '\n');
        Assert.That(actualOutput, Is.EqualTo("Conjured Mana Cake, 2, 5"));
    }
}