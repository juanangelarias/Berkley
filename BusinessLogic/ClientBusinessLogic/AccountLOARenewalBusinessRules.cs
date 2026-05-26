using James.Shared.Constants;
using James.Shared.Model;

namespace ClientBusinessLogic;

public static class AccountLOARenewalBusinessRules
{
    private static int _minimumNarrativeLength = 75;

    public static LineOfAuthorityReason? ApplyFormBusinessRule(LineOfAuthorityReason? reason)
    {
        if (reason == null)
            return null;

        if (reason!.Type == LOARenewalType.Rapid)
        {
            reason.BusinessOverview = null;
            reason.BondRisk = null;
            reason.DebtHighlights = null;
            reason.FollowUpConditions = null;
            reason.KeyChanges = null;
            reason.Outlook = null;
        }
        else if (reason.Type == LOARenewalType.Short)
        {
            reason.KeyChanges = null;
            reason.Outlook = null;
        }

        return reason;
    }

    public static int GetMinimumNarrativeLength() => _minimumNarrativeLength;
    
    public static List<string> ValidateLineOfAuthorityReason(LineOfAuthorityReason? reason,
        int loaRecords)
    {
        if (reason == null)
            return ["No data"];

        var errors = new List<string>();
        switch (reason.Type)
        {
            case LOARenewalType.Rapid:
                if ((reason.Recommendation?.Length ?? 0) < _minimumNarrativeLength)
                    errors.Add($"Criteria must be at least {_minimumNarrativeLength} characters");

                if ((reason.FinancialAnalysis?.Length ?? 0) < _minimumNarrativeLength)
                    errors.Add($"Financial Analysis must be at least {_minimumNarrativeLength} characters");
                break;
            case LOARenewalType.Short:
                AddRequiredNarrativeErrors(errors,
                    (nameof(reason.Recommendation), "Recommendation", reason.Recommendation),
                    (nameof(reason.DebtHighlights), "Debt Highlights", reason.DebtHighlights),
                    (nameof(reason.BusinessOverview), "Business Overview", reason.BusinessOverview),
                    (nameof(reason.FollowUpConditions), "Follow Up Conditions", reason.FollowUpConditions),
                    (nameof(reason.BondRisk), "Bond Risk", reason.BondRisk),
                    (nameof(reason.FinancialAnalysis), "Financial Analysis", reason.FinancialAnalysis));
                break;
            case LOARenewalType.Standard:
                AddRequiredNarrativeErrors(errors,
                    (nameof(reason.Recommendation), "Recommendation", reason.Recommendation),
                    (nameof(reason.DebtHighlights), "Debt Highlights", reason.DebtHighlights),
                    (nameof(reason.BusinessOverview), "Business Overview", reason.BusinessOverview),
                    (nameof(reason.FollowUpConditions), "Follow Up Conditions", reason.FollowUpConditions),
                    (nameof(reason.BondRisk), "Bond Risk", reason.BondRisk),
                    (nameof(reason.FinancialAnalysis), "Financial Analysis", reason.FinancialAnalysis),
                    (nameof(reason.KeyChanges), "Key Changes", reason.KeyChanges),
                    (nameof(reason.Outlook), "Outlook", reason.Outlook));
                break;
        }

        if (loaRecords == 0)
            errors.Add("There is no Line of Authority to submit.");

        return errors;
    }

    private static void AddRequiredNarrativeErrors(List<string> errors,
        params (string Field, string Label, string? Value)[] fields)
    {
        foreach (var (_, label, value) in fields)
        {
            if ((value?.Length ?? 0) < _minimumNarrativeLength)
                errors.Add($"{label} must be at least {_minimumNarrativeLength} characters");
        }
    }
}