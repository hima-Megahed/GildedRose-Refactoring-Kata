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

    private void UpdateQualityFor(Item item)
    {
        switch (item.Name)
        {
            case AgedBrieItem:
                item.Quality++; //Increase Aged Brie
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
        if (item.SellIn >= 0) return;

        if (item.Name == AgedBrieItem) item.Quality++;
        else if (item.Name == BackstagePasses) item.Quality = 0;
        else item.Quality--;
    }

    private void IncreaseBackStagePassesQuality(Item item)
    {
        switch (item.SellIn)
        {
            case <= 5:
                item.Quality += 3;
                break;
            case <= 10:
                item.Quality += 2;
                break;
            default:
                item.Quality++;
                break;
        }
    }

    private void DecreaseSellIn(Item item) => item.SellIn--;

    private void ClipQuality(Item item) =>
        item.Quality = item.Quality < 0 ? 0 : item.Quality > 50 ? 50 : item.Quality;
}