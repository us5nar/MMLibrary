namespace MMLibrary
{
    public delegate void ChangedGridSizeHandler(IModel sender); // deligate in the case any changes are made in the Model , which has to be transfered to GUI

    public interface IModel
    {
        int NewGridSize { set; get; } // public property for exchanging the grid size between Form1/GUI and Model class
        event ChangedGridSizeHandler GridWasChanged; // event , which is used for reporting changes in the results which has to be sent to GUI
    }
}
