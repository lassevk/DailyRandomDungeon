using System;
using System.Windows.Forms;

namespace DailyRandomDungeon
{
    public class AddCharacterControl : UserControl
    {
        private const string _HintText = "Enter a character name and hit enter to add";
        
        private readonly TextBox eCharacterName;
        
        public AddCharacterControl()
        {
            Width = 500;
            Height = 8 * 2 + 40;
            BorderStyle = BorderStyle.FixedSingle;

            eCharacterName = new TextBox {
                Left = 8,
                Top = 8,
                Height = 40,
                Width = Width - 16,
                Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right,
                Text = _HintText
            };

            eCharacterName.SelectAll();

            eCharacterName.KeyDown += CharacterNameKeyDown;
            eCharacterName.GotFocus += CharacterNameGotFocus;
            eCharacterName.LostFocus += CharacterNameLostFocus;
            
            Controls.Add(eCharacterName);
        }

        private void CharacterNameLostFocus(object sender, EventArgs e)
        {
            if (eCharacterName.Text == "")
            {
                eCharacterName.Text = _HintText;
                eCharacterName.SelectAll();
            }
        }

        private void CharacterNameGotFocus(object sender, EventArgs e)
        {
            if (eCharacterName.Text == _HintText)
                eCharacterName.Text = "";
        }

        private void CharacterNameKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Return)
                return;

            var characterName = eCharacterName.Text.Trim();
            if (characterName == _HintText)
                return;
            
            eCharacterName.SelectAll();

            if (DungeonTimers.GetCharacters().Contains(characterName))
            {
                MessageBox.Show(
                    this, $"Character '{characterName}' already exists", "Character exists", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }

            DungeonTimers.Add(characterName);
            CharacterAdded?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler CharacterAdded;
    }
}