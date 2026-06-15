using Entity = EF_Client_App_Entity;
using DAL = EF_Client_App_DAL;
using EF_Client_App_Entity;
namespace EF_Client_App_BL;

public class Matching
{
   private List<Entity.Array> _arrays;
   public List<Entity.Description> Descriptions;
   public List<Entity.Serie> Series;
   
   public Matching(List<Entity.Array> p_arrays, List<Entity.Description> p_descriptions, List<Entity.Serie> p_eries)
    {
        _arrays = p_arrays;
        Descriptions = p_descriptions;
        Series = p_eries;
    }

    public void PopulateDescriptionsSeries()
    {
        foreach (var description in Descriptions)
        {
            PopulateSerieList(description.FirstLineArray);
            PopulateSerieList(description.Line3LevelSpec);
            PopulateSerieList(description.Line4PchCont);

        }
    }

    private void PopulateSerieList(List<Serie>? seriesList)
    {
        if (seriesList == null)
            return;

        var expandedList = new List<Serie>();

        foreach (var placeholder in seriesList)
        {
            if (placeholder == null)
                continue;

            // récupère TOUTES les séries avec le même ID
            var matchingSeries = Series
                .Where(s => s.ID == placeholder.ID)
                .ToList();

            if (matchingSeries.Any())
            {
                expandedList.AddRange(matchingSeries);
            }
        }

        // remplace la liste originale
        seriesList.Clear();
        seriesList.AddRange(expandedList);
    }

    public void RenderSeriesValuesByYear(int idArray, char frequency, DateOnly date)
    {
        Console.WriteLine("Rendering Series Values by Year");

        var array = _arrays.Find(a => a.ArrayId == idArray);
        var descriptions = Descriptions.Where(d => d.ArrayId == idArray);

        if (array != null)
        {
            Console.WriteLine($"{array.Title}");
            Console.WriteLine("=======================\n");
        }

        string targetDate = date.ToString("yyyy-MM-dd");

        foreach (var d in descriptions)
        {
            Console.WriteLine($"# {d.TextDescription}");

            // FIRST LINE ARRAY
            if (d.FirstLineArray != null)
            {
                foreach (var serie in d.FirstLineArray)
                {
                    if (serie?.Observations != null && serie.Frequency == frequency)
                    {
                        foreach (KeyValuePair<string, decimal> kvp in serie.Observations)
                        {
                            if (kvp.Key == targetDate)
                            {
                                Console.WriteLine(
                                    $"  │ (FirstLineArray) {serie.ID} => {kvp.Key} : {kvp.Value} frequency : {serie.Frequency}"
                                );
                            }
                        }
                    }
                }
            }

            // LINE 3 LEVEL SPEC
            if (d.Line3LevelSpec != null)
            {
                foreach (var serie in d.Line3LevelSpec)
                {
                    if (serie?.Observations != null && serie.Frequency == frequency)
                    {
                        foreach (KeyValuePair<string, decimal> kvp in serie.Observations)
                        {
                            if (kvp.Key == targetDate)
                            {
                                Console.WriteLine(
                                    $"  │ (Line3LevelSpec) {serie.ID} => {kvp.Key} : {kvp.Value} frequency : {serie.Frequency}"
                                );
                            }
                        }
                    }
                }
            }

            // LINE 4 PCH CONT
            if (d.Line4PchCont != null)
            {
                foreach (var serie in d.Line4PchCont)
                {
                    if (serie?.Observations != null && serie.Frequency == frequency)
                    {
                        foreach (KeyValuePair<string, decimal> kvp in serie.Observations)
                        {
                            if (kvp.Key == targetDate)
                            {
                                Console.WriteLine(
                                    $"  │ (Line4PchCont) {serie.ID} => {kvp.Key} : {kvp.Value} frequency : {serie.Frequency}"
                                );
                            }
                        }
                    }
                }
            }
        }
    }

    public void TestRender(int idArray)
    {
        Console.WriteLine("Rendering Series Values by Year");

        var array = _arrays.Find(a => a.ArrayId == idArray);
        var descriptions = Descriptions.Where(d => d.ArrayId == idArray);

        if (array != null)
        {
            Console.WriteLine($"{array.Title}");
            Console.WriteLine("=======================\n");
        }

        foreach (var d in descriptions)
        {
            Console.WriteLine($"# {d.TextDescription}");

            // FIRST LINE ARRAY
            if (d.FirstLineArray != null)
            {
                foreach (var serie in d.FirstLineArray)
                {
                    if (serie?.Observations != null)
                    {
                        foreach (KeyValuePair<string, decimal> kvp in serie.Observations)
                        {
                            Console.WriteLine(
                                $"  │ (FirstLineArray) {serie.ID} => {kvp.Key} : {kvp.Value} frequency : {serie.Frequency}"
                            );
                        }
                    }
                }
            }

            // LINE 3 LEVEL SPEC
            if (d.Line3LevelSpec != null)
            {
                foreach (var serie in d.Line3LevelSpec)
                {
                    if (serie?.Observations != null)
                    {
                        foreach (KeyValuePair<string, decimal> kvp in serie.Observations)
                        {
                            Console.WriteLine(
                                $"  │ (Line3LevelSpec) {serie.ID} => {kvp.Key} : {kvp.Value} frequency : {serie.Frequency}"
                            );
                        }
                    }
                }
            }

            // LINE 4 PCH CONT
            if (d.Line4PchCont != null)
            {
                foreach (var serie in d.Line4PchCont)
                {
                    if (serie?.Observations != null)
                    {
                        foreach (KeyValuePair<string, decimal> kvp in serie.Observations)
                        {
                            Console.WriteLine(
                                $"  │ (Line4PchCont) {serie.ID} => {kvp.Key} : {kvp.Value} frequency : {serie.Frequency}"
                            );
                        }
                    }
                }
            }
        }
    }

} 