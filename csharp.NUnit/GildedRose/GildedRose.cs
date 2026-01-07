using System.Collections.Generic;

namespace GildedRoseKata;

public class GildedRose(IList<Item> items)
{
    private const string SulfurasHandOfRagnaros = "Sulfuras, Hand of Ragnaros";
    private const string AgedBrieItem = "Aged Brie";
    private const string BackstagePasses = "Backstage passes to a TAFKAL80ETC concert";

    public void UpdateQuality()
    {
        DecreaseNormalItemQuality();

        IncreaseAgedBrieQuality();
        IncreaseBackStagePassesQuality();

        DecreaseSellIn();
        DecreaseItemQualityForPassedSellIn();
    }

    private void DecreaseItemQualityForPassedSellIn()
    {
        foreach (var item in items)
        {
            if (item.SellIn >= 0 || item.Quality <= 0 || item.Name == SulfurasHandOfRagnaros) continue;

            if (item.Name == AgedBrieItem) IncreaseAgedBrieQuality();
            else if (item.Name == BackstagePasses) item.Quality = 0;
            else item.Quality--;
        }
    }

    private void DecreaseNormalItemQuality()
    {
        foreach (var item in items)
        {
            if (item.Quality <= 0 ||
                item.Name is AgedBrieItem or BackstagePasses or SulfurasHandOfRagnaros) continue;

            item.Quality--;
        }
    }

    private void IncreaseBackStagePassesQuality()
    {
        foreach (var item in items)
        {
            if (item.Quality >= 50) continue;
            if (item.Name != BackstagePasses) continue;

            item.Quality++;
            if (item.Quality >= 50) continue;

            if (item.SellIn <= 5)
                item.Quality++;

            if (item.SellIn <= 10)
                item.Quality++;
        }
    }

    private void IncreaseAgedBrieQuality()
    {
        foreach (var item in items)
        {
            if (item.Name == AgedBrieItem && item.Quality < 50)
                item.Quality++;
        }
    }

    private void DecreaseSellIn()
    {
        foreach (var item in items)
        {
            if (item.Name == SulfurasHandOfRagnaros)
                continue;
            item.SellIn--;
        }
    }
}