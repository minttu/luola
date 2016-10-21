#region Copyright & License Information

// Copyright 2016 Minttu Imberg
// This file is part of luola, which is free software. It is made
// available to you under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of
// the License, or (at your option) any later version. For more
// information, see LICENSE-CODE.

#endregion

namespace luola.Scenes
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