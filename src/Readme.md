# DavDay 2019 - Goodbye JS, Hello .Net !


## Ajax
### Inject
services.AddSingleton<IBoardItemService, BoardItemService>();
@inject IBoardItemService boardService

### Init
List<BoardItem> Tasks = new List<BoardItem>();

### Query
protected override async Task OnInitializedAsync()
{
    Tasks = (await boardService.GetBoardItems()).ToList();
}

## DragAndDrop
<ItemsContainer Tasks="Tasks" OnStatusUpdated="HandleStatusUpdated">
    <ItemList ListStatus="Status.TODO" AllowedStatuses="@(new Status[] { Status.INPROGRESS })" />
    <ItemList ListStatus="Status.INPROGRESS" AllowedStatuses="@(new Status[] { Status.TODO })" />
    <ItemList ListStatus="Status.DONE" AllowedStatuses="@(new Status[] { Status.INPROGRESS })" />
</ItemsContainer>

### Drop event
protected override void OnParametersSet()
    {
        Items.Clear();
        Items.AddRange(Container.Tasks.Where(x => x.Status == ListStatus));
    }

    private void HandleDragEnter()
    {
        if (ListStatus == Container.Payload.Status) return;

        if (AllowedStatuses != null && !AllowedStatuses.Contains(Container.Payload.Status))
        {
            dropClass = "no-drop";
        }
        else
        {
            dropClass = "can-drop";
        }
    }

    private void HandleDragLeave()
    {
        dropClass = "";
    }

    private async Task HandleDrop()
    {
        dropClass = "";

        if (AllowedStatuses != null && !AllowedStatuses.Contains(Container.Payload.Status)) return;

        await Container.UpdateJobAsync(ListStatus);
    }


### Update Event
@code {
    async Task HandleStatusUpdated(BoardItem updatedTask)
    {
        await boardService.UpdateBoardItem(updatedTask);
    }
}

### Step next
<ItemList ListStatus="Status.TODO" AllowedStatuses="@(new Status[] { Status.INPROGRESS })" />


## Create new item
@page "/create"
@inject IBoardItemService boardService
@inject NavigationManager navigationManager
@inject Blazored.LocalStorage.ILocalStorageService localStorage

<a href="/">Back to board</a>

<EditForm Model="Item" OnValidSubmit="@CreateItemAsync">

    <DataAnnotationsValidator />
    <ValidationSummary />

    <div class="form-group">
        <label>Summary</label>
        <input type="text" class="form-control" placeholder="Summary" @bind="Item.Summary" />
    </div>

    <div class="form-group">
        <label>Summary</label>
        <input type="text" class="form-control" placeholder="Description" @bind="Item.Description" />
    </div>
    <div class="form-group">
        <label>Summary</label>
        <input type="text" class="form-control" placeholder="Author" @bind="Item.Author" />
    </div>
    <input type="submit" />
</EditForm>

@code {
    public BoardItem Item { get; set; } = new BoardItem();

    public bool ShowError { get; set; }

    protected override async Task OnInitializedAsync()
    {
        navigationManager.LocationChanged += StoreToLocalStorage;

        if (await localStorage.ContainKeyAsync("draft"))
            Item = await localStorage.GetItemAsync<BoardItem>("draft");
    }

    private async void StoreToLocalStorage(object sender, LocationChangedEventArgs e)
    {
        await localStorage.SetItemAsync("draft", Item);
    }

    public async Task CreateItemAsync()
    {
        if (await boardService.CreateBoardItem(Item))
        {
            navigationManager.LocationChanged -= StoreToLocalStorage;
            navigationManager.NavigateTo("/");
        }
        else
        {
            ShowError = true;
        }
    }
}

## Localstorage
services.AddBlazoredLocalStorage();
protected override async Task OnInitializedAsync()
    {
        navigationManager.LocationChanged += StoreToLocalStorage;

        if (await localStorage.ContainKeyAsync("draft"))
            Item = await localStorage.GetItemAsync<BoardItem>("draft");
    }

    private async void StoreToLocalStorage(object sender, LocationChangedEventArgs e)
    {
        await localStorage.SetItemAsync("draft", Item);
    }
	
	public async Task CreateItemAsync()
    {
        if (await boardService.CreateBoardItem(Item))
        {
            navigationManager.LocationChanged -= StoreToLocalStorage;
            navigationManager.NavigateTo("/");
        }
        else
        {
            ShowError = true;
        }
    }


## File download
func.js

function saveAsFile(filename, bytesBase64) {
    var link = document.createElement('a');
    link.download = filename;
    link.href = "data:application/octet-stream;base64," + bytesBase64;
    document.body.appendChild(link); // Needed for Firefox
    link.click();
    document.body.removeChild(link);
}

window['saveAsFile'] = saveAsFile;

.cs
@inject IJSRuntime js
public async Task SaveAs(string filename, byte[] data)
{
    await js.InvokeAsync<object>(
        "saveAsFile",
        filename,
        Convert.ToBase64String(data));
}  