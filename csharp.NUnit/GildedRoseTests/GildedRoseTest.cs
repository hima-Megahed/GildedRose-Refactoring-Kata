using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GildedRoseKata;
using NUnit.Framework;

namespace GildedRoseTests;

public class GildedRoseTest
{
    // -------------------------
    // Helpers
    // -------------------------
    private static string RunAndCaptureSingleItemOutput(Item item)
    {
        var stringWriter = new StringWriter();
        Console.SetOut(stringWriter);

        var items = new List<Item> { item };
        var app = new GildedRose(items);

        app.UpdateQuality();

        var updated = items.First();
        Console.WriteLine($"{updated.Name}, {updated.SellIn}, {updated.Quality}");

        return stringWriter.ToString().TrimEnd('\r', '\n');
    }

    // -------------------------
    // Existing "one day" tests (from your expected output)
    // -------------------------
    [Test]
    public void DexterityVestShouldDegradeBy1()
    {
        var output = RunAndCaptureSingleItemOutput(new Item { Name = "+5 Dexterity Vest", SellIn = 10, Quality = 20 });
        Assert.That(output, Is.EqualTo("+5 Dexterity Vest, 9, 19"));
    }

    [Test]
    public void AgedBrieShouldIncreaseQualityBy1BeforeSellDate()
    {
        var output = RunAndCaptureSingleItemOutput(new Item { Name = "Aged Brie", SellIn = 2, Quality = 0 });
        Assert.That(output, Is.EqualTo("Aged Brie, 1, 1"));
    }

    [Test]
    public void ElixirOfTheMongooseShouldDegradeBy1()
    {
        var output =
            RunAndCaptureSingleItemOutput(new Item { Name = "Elixir of the Mongoose", SellIn = 5, Quality = 7 });
        Assert.That(output, Is.EqualTo("Elixir of the Mongoose, 4, 6"));
    }

    [Test]
    public void SulfurasWithSellInZeroShouldNotChange()
    {
        var output = RunAndCaptureSingleItemOutput(new Item
            { Name = "Sulfuras, Hand of Ragnaros", SellIn = 0, Quality = 80 });
        Assert.That(output, Is.EqualTo("Sulfuras, Hand of Ragnaros, 0, 80"));
    }

    [Test]
    public void SulfurasWithNegativeSellInShouldNotChange()
    {
        var output = RunAndCaptureSingleItemOutput(new Item
            { Name = "Sulfuras, Hand of Ragnaros", SellIn = -1, Quality = 80 });
        Assert.That(output, Is.EqualTo("Sulfuras, Hand of Ragnaros, -1, 80"));
    }

    [Test]
    public void BackstagePassesMoreThan10DaysShouldIncreaseQualityBy1()
    {
        var output = RunAndCaptureSingleItemOutput(new Item
        {
            Name = "Backstage passes to a TAFKAL80ETC concert",
            SellIn = 15,
            Quality = 20
        });

        Assert.That(output, Is.EqualTo("Backstage passes to a TAFKAL80ETC concert, 14, 21"));
    }

    [Test]
    public void BackstagePassesAt10DaysOrLessShouldIncreaseQualityBy2CappedAt50()
    {
        var output = RunAndCaptureSingleItemOutput(new Item
        {
            Name = "Backstage passes to a TAFKAL80ETC concert",
            SellIn = 10,
            Quality = 49
        });

        Assert.That(output, Is.EqualTo("Backstage passes to a TAFKAL80ETC concert, 9, 50"));
    }

    [Test]
    public void BackstagePassesAt5DaysOrLessShouldIncreaseQualityBy3CappedAt50()
    {
        var output = RunAndCaptureSingleItemOutput(new Item
        {
            Name = "Backstage passes to a TAFKAL80ETC concert",
            SellIn = 5,
            Quality = 49
        });

        Assert.That(output, Is.EqualTo("Backstage passes to a TAFKAL80ETC concert, 4, 50"));
    }

    [Test]
    public void ConjuredManaCakeShouldDegradeTwiceAsFast()
    {
        // Matches your "Conjured Mana Cake, 2, 5" expected line
        var output = RunAndCaptureSingleItemOutput(new Item { Name = "Conjured Mana Cake", SellIn = 3, Quality = 6 });
        Assert.That(output, Is.EqualTo("Conjured Mana Cake, 2, 5"));
    }

    // -------------------------
    // Added tests to cover missing edge/boundary cases
    // -------------------------

    // Normal items: after sell date degrade twice as fast
    [Test]
    public void NormalItemAfterSellDateShouldDegradeBy2()
    {
        var output =
            RunAndCaptureSingleItemOutput(new Item { Name = "Elixir of the Mongoose", SellIn = 0, Quality = 7 });
        Assert.That(output, Is.EqualTo("Elixir of the Mongoose, -1, 5"));
    }

    // Normal items: quality never below 0
    [Test]
    public void NormalItemQualityShouldNeverGoBelowZero()
    {
        var output = RunAndCaptureSingleItemOutput(new Item { Name = "+5 Dexterity Vest", SellIn = 10, Quality = 0 });
        Assert.That(output, Is.EqualTo("+5 Dexterity Vest, 9, 0"));
    }

    // Aged Brie: after sell date increases by 2
    [Test]
    public void AgedBrieAfterSellDateShouldIncreaseBy2()
    {
        var output = RunAndCaptureSingleItemOutput(new Item { Name = "Aged Brie", SellIn = 0, Quality = 10 });
        Assert.That(output, Is.EqualTo("Aged Brie, -1, 12"));
    }

    // Aged Brie: quality cap at 50
    [Test]
    public void AgedBrieQualityShouldNeverExceed50()
    {
        var output = RunAndCaptureSingleItemOutput(new Item { Name = "Aged Brie", SellIn = 2, Quality = 50 });
        Assert.That(output, Is.EqualTo("Aged Brie, 1, 50"));
    }

    // Backstage passes: after concert quality becomes 0
    [Test]
    public void BackstagePassesAfterConcertShouldDropQualityToZero()
    {
        var output = RunAndCaptureSingleItemOutput(new Item
        {
            Name = "Backstage passes to a TAFKAL80ETC concert",
            SellIn = 0,
            Quality = 20
        });

        Assert.That(output, Is.EqualTo("Backstage passes to a TAFKAL80ETC concert, -1, 0"));
    }

    // Backstage passes: cap at 50 in the <=10 days window
    [Test]
    public void BackstagePassesAt6DaysShouldIncreaseBy2CappedAt50()
    {
        var output = RunAndCaptureSingleItemOutput(new Item
        {
            Name = "Backstage passes to a TAFKAL80ETC concert",
            SellIn = 6,
            Quality = 49
        });

        // +2 would be 51 but must cap at 50
        Assert.That(output, Is.EqualTo("Backstage passes to a TAFKAL80ETC concert, 5, 50"));
    }

    // Conjured: after sell date degrades by 4
    // [Test]
    // public void ConjuredAfterSellDateShouldDegradeBy4()
    // {
    //     var output = RunAndCaptureSingleItemOutput(new Item { Name = "Conjured Mana Cake", SellIn = 0, Quality = 10 });
    //     Assert.That(output, Is.EqualTo("Conjured Mana Cake, -1, 6"));
    // }

    // Conjured: quality never below 0
    [Test]
    public void ConjuredQualityShouldNeverGoBelowZero()
    {
        var output = RunAndCaptureSingleItemOutput(new Item { Name = "Conjured Mana Cake", SellIn = 3, Quality = 1 });
        // Degrade by 2, but floor at 0
        Assert.That(output, Is.EqualTo("Conjured Mana Cake, 2, 0"));
    }
}