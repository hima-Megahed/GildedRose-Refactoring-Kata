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
            if (item.Name == SulfurasHandOfRagnaros) continue;

            UpdateQualityFor(item);

            DecreaseSellIn(item);
            DecreaseItemQualityForPassedSellIn(item);

            ClipQuality(item);
        }
    }

    private void ClipQuality(Item item) =>
        item.Quality = item.Quality < 0 ? 0 : item.Quality > 50 ? 50 : item.Quality;

    private void UpdateQualityFor(Item item)
    {
        switch (item.Name)
        {
            case AgedBrieItem:
                IncreaseAgedBrieQuality(item);
                return;
            case BackstagePasses:
                IncreaseBackStagePassesQuality(item);
                return;
            default:
                item.Quality--; // Decrease normal item quality
                return;
        }
    }

    private void DecreaseItemQualityForPassedSellIn(Item item)
    {
        if (item.SellIn >= 0 || item.Quality <= 0 || item.Name == SulfurasHandOfRagnaros) return;

        if (item.Name == AgedBrieItem) IncreaseAgedBrieQuality(item);
        else if (item.Name == BackstagePasses) item.Quality = 0;
        else item.Quality--;
    }

    private void IncreaseBackStagePassesQuality(Item item)
    {
        if (item.Quality >= 50) return;

        item.Quality++;
        if (item.Quality >= 50) return;

        if (item.SellIn <= 5)
            item.Quality++;

        if (item.SellIn <= 10)
            item.Quality++;
    }

    private void IncreaseAgedBrieQuality(Item item)
    {
        if (item.Quality < 50)
            item.Quality++;
    }

    private void DecreaseSellIn(Item item) => item.SellIn--;
}