// ---------------------------------------------
//      --- Trellog-Action by Scarementus ---
//      ---        Licence MIT       ---
// ---------------------------------------------

namespace Trellog_Action;

public class TrelloService
{
    private List<ICard> Added { get; set; } = new();
    private List<ICard> Fixed { get; set; } = new();
    private List<ICard> Changed { get; set; } = new();
    public string VersionString { get; set; }
    public Version VersionNum { get; set; }

    public IMarkdownDocument Document { get; set; }

    public void Initialise(string appKey, string userToken)
    {
        TrelloAuthorization.Default.AppKey = appKey;
        TrelloAuthorization.Default.UserToken = userToken;
    }

    public async Task ProcessRelease(ActionInputs inputs)
    {
        var factory = new TrelloFactory();
        var list = factory.List(inputs.InputList);


        await list.Refresh();
        await list.Cards.Refresh();

        if (VersionString is null)
        {
            var oList = factory.List(inputs.OutputList);
            await oList.Refresh();
            var card2 = oList.Cards.Last();
            var versionBump = card2.Name.Substring(1);
            VersionNum = Version.Parse(versionBump);
            int major = VersionNum.Major;
            int minor = VersionNum.Minor;
            int patch = VersionNum.Build;
            switch (inputs.ReleaseType)
            {
                case "Major":
                    major++;
                    minor = 0;
                    patch = 0;
                    break;
                case "Minor":
                    minor++;
                    minor = 0;
                    patch = 0;
                    break;
                case "Patch":
                    patch++;
                    break;
            }

            VersionNum = new Version(major, minor, patch);

            VersionString = $"v{VersionNum.ToString()}";
        }
        else
        {
            VersionString = inputs.Version;
            VersionNum = Version.Parse(VersionString);
        }

        foreach (var card in list.Cards)
        {
            if (card.Labels.Any(x => x.Name == "Added"))
                Added.Add(card);
            if (card.Labels.Any(x => x.Name == "Changed"))
                Changed.Add(card);
            if (card.Labels.Any(x => x.Name == "Fixed"))
                Fixed.Add(card);
        }
    }

    public IMarkdownDocument BuildMarkdown()
    {
        Document = new MarkdownDocument();

        Document.Append(new MarkdownHeader($"[{VersionNum}] - {DateTime.Now.ToString("d")}", 2));

        if (Added.Any())
        {
            Document.AppendHeader("Added", 3);
            foreach (ICard card in Added)
            {
                if (card.CheckLists.Any())
                {
                    Document.AppendHeader(card.Name, 3);
                    var listItems = new List<MarkdownTextListItem>();
                    foreach (ICheckList checkList in card.CheckLists)
                    {
                        listItems = new List<MarkdownTextListItem>();
                        listItems.AddRange(checkList.CheckItems.Where(x => x.State == CheckItemState.Complete)
                            .Select(checkItem => new MarkdownTextListItem(checkItem.Name)));
                        Document.AppendList(listItems);
                    }
                }
                else
                {
                    Document.Append(new MarkdownList(new MarkdownTextListItem(card.Name)));
                }
            }
        }

        if (Changed.Any())
        {
            Document.AppendHeader("Changed", 2);
            foreach (ICard card in Changed)
            {
                if (card.CheckLists.Any())
                {
                    Document.AppendHeader(card.Name, 3);
                    var listItems = new List<MarkdownTextListItem>();
                    foreach (ICheckList checkList in card.CheckLists)
                    {
                        listItems = new List<MarkdownTextListItem>();
                        listItems.AddRange(checkList.CheckItems.Where(x => x.State == CheckItemState.Complete)
                            .Select(checkItem => new MarkdownTextListItem(checkItem.Name)));
                        Document.AppendList(listItems);
                    }
                }
                else
                {
                    Document.Append(new MarkdownOrderedList(new MarkdownTextListItem(card.Name)));
                }
            }
        }

        if (Fixed.Any())
        {
            Document.AppendHeader("Fixed", 2);
            foreach (ICard card in Fixed)
            {
                if (card.CheckLists.Any())
                {
                    Document.AppendHeader(card.Name, 3);
                    var listItems = new List<MarkdownTextListItem>();
                    foreach (ICheckList checkList in card.CheckLists)
                    {
                        listItems = new List<MarkdownTextListItem>();
                        listItems.AddRange(checkList.CheckItems.Where(x => x.State == CheckItemState.Complete)
                            .Select(checkItem => new MarkdownTextListItem(checkItem.Name)));
                        Document.AppendList(listItems);
                    }
                }
                else
                {
                    Document.Append(new MarkdownOrderedList(new MarkdownTextListItem(card.Name)));
                }
            }
        }

        return Document;
    }

    public async Task UpdateTrello(ActionInputs inputs)
    {
        var factory = new TrelloFactory();
        var list = factory.List(inputs.OutputList);
        if (inputs.OutputList != null)
        {
            await list.Cards.Add(VersionString, description: Document.ToString(), dueDate: DateTime.Now,
                isComplete: true);
        }

        if (inputs.CleanInputList)
        {
            list = factory.List(inputs.InputList);
            foreach (ICard card in list.Cards)
            {
                await card.Delete();
            }
        }
    }
}