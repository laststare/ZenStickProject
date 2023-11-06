namespace Codebase.InterfaceAdapters.DataSave
{
    public interface IDataSave
    {
        public void SaveBestScore(int bestScore);
        public int LoadBestScore();
    }
}