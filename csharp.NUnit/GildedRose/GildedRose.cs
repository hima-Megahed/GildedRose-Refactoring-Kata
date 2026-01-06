using System.Collections.Generic;

namespace GildedRoseKata;

public class GildedRose(IList<Item> items)
{
    private const string SulfurasHandOfRagnaros = "Sulfuras, Hand of Ragnaros";
    private const string AgedBrieItem = "Aged Brie";
    private const string BackstagePasses = "Backstage passes to a TAFKAL80ETC concert";

    public void UpdateQuality()
    {
        foreach (var item in items)
        {
            DecreaseNormalItemQuality(item);

            IncreaseAgedBrieQuality(item);
            IncreaseBackStagePassesQuality(item);

            DecreaseSellIn(item);

            DecreaseNormalItemQualityForPassedSellIn(item);
        }
    }

    private static void DecreaseNormalItemQualityForPassedSellIn(Item item)
    {
        if (item.SellIn >= 0) return;
        if (item.Name != AgedBrieItem)
        {
            if (item.Name != BackstagePasses)
            {
                if (item.Quality <= 0) return;
                if (item.Name != SulfurasHandOfRagnaros)
                {
                    item.Quality--;
                }
            }
            else
            {
                item.Quality = 0;
            }
        }
        else
        {
            if (item.Quality < 50)
            {
                item.Quality++;
            }
        }
    }

    private static void DecreaseNormalItemQuality(Item item)
    {
        if (item.Quality <= 0 ||
            item.Name is AgedBrieItem or BackstagePasses or SulfurasHandOfRagnaros) return;

        item.Quality--;
    }

    private void IncreaseBackStagePassesQuality(Item item)
    {
        if (item.Quality >= 50) return;
        if (item.Name != BackstagePasses) return;

        item.Quality += 1;
        if (item.Quality >= 50) return;

        if (item.SellIn <= 5)
            item.Quality += 1;

        if (item.SellIn <= 10)
            item.Quality += 1;
    }

    private void IncreaseAgedBrieQuality(Item item)
    {
        if (item.Name == AgedBrieItem && item.Quality < 50)
            item.Quality += 1;
    }

    private static void DecreaseSellIn(Item item)
    {
        if (item.Name == SulfurasHandOfRagnaros)
            return;
        item.SellIn -= 1;
    }
}