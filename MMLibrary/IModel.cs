namespace MMLibrary
{
    public delegate void ChangedGridSizeHandler(IModel sender);
    public interface IModel
    {
        int NewGridSize { set; get; }
        event ChangedGridSizeHandler GridWasChanged;
    }
}
