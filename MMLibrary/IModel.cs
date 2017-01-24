namespace MMLibrary
{
    public delegate void ChangedGridSizeHandler(IModel sender);
    public interface IModel
    {
         //void UpdateXML();
         //void ReadFromXML();

        int NewGridSize { set; get; }
        event ChangedGridSizeHandler GridWasChanged;
    }
}
