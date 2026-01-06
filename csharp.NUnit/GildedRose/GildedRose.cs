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

    private void DecreaseNormalItemQualityForPassedSellIn(Item item)
    {
        if (item.SellIn >= 0 || item.Quality <= 0) return;
        if (item.Name == SulfurasHandOfRagnaros) return;
        IncreaseAgedBrieQuality(item);

        if (item.Name == AgedBrieItem) return;
        if (item.Name != BackstagePasses)
            item.Quality--;
        else
            item.Quality = 0;
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

        item.Quality++;
        if (item.Quality >= 50) return;

        if (item.SellIn <= 5)
            item.Quality++;

        if (item.SellIn <= 10)
            item.Quality++;
    }

    private void IncreaseAgedBrieQuality(Item item)
    {
        if (item.Name == AgedBrieItem && item.Quality < 50)
            item.Quality++;
    }

    private static void DecreaseSellIn(Item item)
    {
        if (item.Name == SulfurasHandOfRagnaros)
            return;
        item.SellIn--;
    }
}