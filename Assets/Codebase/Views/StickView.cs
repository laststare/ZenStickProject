using Codebase.InterfaceAdapters.Stick;
using Codebase.Utilities;

namespace Codebase.Views
{
    public class StickView : ViewBase
    {
        private StickViewModel _stickViewModel;

        public void Init(StickViewModel stickViewModel)
        {
            _stickViewModel = stickViewModel;
        }   
        
    }
}