namespace Luola
{
    public delegate string OnSelected();

    public class MenuItem
    {
        public MenuItem(string text)
        {
            Text = text;
            OnSelected = null;
        }

        public string Text { get; set; }
        public event OnSelected OnSelected;

        public void Select()
        {
            var text = OnSelected?.Invoke();
            if (text != null)
                Text = text;
        }
    }
}