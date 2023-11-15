using UniRx;

namespace Codebase.InterfaceAdapters.Stick
{
    public interface IStick
    {
        public ReactiveProperty<float> StickLength { get; set; }
    }
}