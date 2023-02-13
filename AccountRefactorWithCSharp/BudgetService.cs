namespace AccountRefactorWithCSharp;

public class BudgetService
{
    private readonly IBudgetRepo _budgetRepo;

    public BudgetService(IBudgetRepo budgetRepo)
    {
        _budgetRepo = budgetRepo;
    }

    public double Query(DateTime start, DateTime end)
    {
        var budgets = _budgetRepo.GetAll();

        var startMonthDays = DateTime.DaysInMonth(start.Year, start.Month);
        var endMonthDays = DateTime.DaysInMonth(end.Year, end.Month);

        if (start.ToString("yyyyMM") != end.ToString("yyyyMM"))
        {
            var currentMonth = new DateTime(start.Year, start.Month, 1).AddMonths(1);
            var sum = 0;
            while (currentMonth < new DateTime(end.Year, end.Month, 1))
            {
                var budget = GetBudget(budgets, $"{currentMonth:yyyyMM}");
                if (budget != null)
                {
                    sum += budget.Amount;
                }

                currentMonth = currentMonth.AddMonths(1);
            }

            // 相同的變數，應該放在一起，才能夠透過IDE來做重構動作
            // 如果在StartBudget中穿插了EndBudget，有可能會導致無法重構，
            // 同時也會凸顯不了重複的壞味道。
            //var startBudget = GetBudget(budgets, start.ToString("yyyyMM"));
            var endBudget = GetBudget(budgets, end.ToString("yyyyMM"));

            var startBudgetPerDay = startBudget?.Amount / startMonthDays ?? 0;
            var endBudgetPerDay = endBudget?.Amount / endMonthDays ?? 0;

            sum += startBudgetPerDay * (startMonthDays - start.Day + 1) + endBudgetPerDay * (end.Day);
            return sum;
        }

        var oneMonthBudget = GetBudget(budgets, start.ToString("yyyyMM"));
        if (oneMonthBudget == null) return 0;

        var amount = oneMonthBudget.Amount;
        var amountPerDay = amount / startMonthDays;
        return amountPerDay * ((end - start).Days + 1);
    }

    private static Budget? GetBudget(List<Budget> budgets, string yearMonth)
    {
        return budgets.FirstOrDefault(b => b.YearMonth == yearMonth);
    }
}