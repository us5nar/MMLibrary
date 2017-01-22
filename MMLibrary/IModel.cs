namespace MMLibrary
{
    public delegate void ChangedGridSizeHandler(IModel sender);
    public interface IModel
    {
        //void Search(IViewGUI userView);
        /* bool AudioFileIsValid();
         bool TagIsValid();
         void UpdateXML();
         void ReadFromXML();
         void PlayAudio();
         void SendNewDataToController();*/
        // Check for duplicates
        //void CheckfoDuplicates();
        //event ChangedGridSizeHandler GridWasChanged;
        int NewGridSize { set; get; }
        event ChangedGridSizeHandler GridWasChanged;
    }
}
