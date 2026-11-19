namespace ScreenManager 
{
    public interface IScreen
    {
        void Activate();
        void Deactivate();
        void Release();
    }
}