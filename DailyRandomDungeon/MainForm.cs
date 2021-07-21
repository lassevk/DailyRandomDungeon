using System.Windows.Forms;

namespace DailyRandomDungeon
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            var sb = new Panel {
                Dock = DockStyle.Fill,
                AutoScroll = true
            };

            Controls.Add(sb);
            var addCharacterControl = new AddCharacterControl {
                Dock = DockStyle.Bottom
            };

            Controls.Add(addCharacterControl);

            AddCharacters(sb);
            addCharacterControl.CharacterAdded += (sender, args) =>
            {
                while (sb.Controls.Count > 0)
                    sb.Controls.RemoveAt(0);
                // sb.Controls.Clear();
                AddCharacters(sb);
            };
        }

        private static void AddCharacters(Panel scrollPanel)
        {
            int y = 0;
            var characters = DungeonTimers.GetCharacters();
            characters.Reverse();
            foreach (string characterName in characters)
            {
                var c1 = new ExistingCharacterControl(characterName);
                scrollPanel.Controls.Add(c1);
                c1.Top = y;
                c1.Dock = DockStyle.Top;

                y += c1.Height;
            }
        }
    }
}