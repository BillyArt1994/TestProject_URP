using AsiActionEngine.RunTime;

namespace AsiActionEngine.Editor
{
    public partial class InspectorWindow
    {
        public void SelectProperty(IProperty _property)
        {
            mSelectProperty = _property;
            needInit = true;
            Repaint();
        }
    }
}