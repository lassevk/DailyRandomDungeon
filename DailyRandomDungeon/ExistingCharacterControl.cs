using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace DailyRandomDungeon
{
    public class ExistingCharacterControl : UserControl
    {
        private readonly Label lblCharacterName;
        private readonly Label lblTimeLeft;
        private readonly Button btnDailyCompleted;
        private readonly Button btnUndoDailyCompleted;
        private readonly Button btnRemoveCharacter;
        private readonly Timer tmUpdate;

        public ExistingCharacterControl(string characterName)
        {
            Width = 500;
            Height = 8 * 2 + 40;
            BorderStyle = BorderStyle.FixedSingle;

            lblCharacterName = new Label { Left = 128, Top = 12, AutoSize = true, Text = characterName };
            lblTimeLeft = new Label
            {
                Left = 8,
                Top = 12,
                AutoSize = true,
                Text = "19h 55m",
                TextAlign = ContentAlignment.MiddleCenter
            };

            btnDailyCompleted = new Button
            {
                Width = 200,
                Height = 40,
                Top = 8,
                Left = Width - 256,
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Text = "Completed"
            };

            btnDailyCompleted.Click += DailyCompleted;

            btnUndoDailyCompleted = new Button
            {
                Width = 200,
                Height = 40,
                Top = 8,
                Left = Width - 256,
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Text = "Undo",
                Visible = false
            };

            btnUndoDailyCompleted.Click += UndoDailyCompleted;

            btnRemoveCharacter = new Button {
                Top = 8,
                Width = 40,
                Height = 40,
                Text = "X",
                Left = Width - 48,
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
            };

            btnRemoveCharacter.Click += RemoveCharacter;
            
            tmUpdate = new Timer { Interval = 5000, Enabled = true };
            tmUpdate.Tick += Update;

            Controls.AddRange(new Control[] { lblCharacterName, lblTimeLeft, btnDailyCompleted, btnUndoDailyCompleted, btnRemoveCharacter });
            Update(null, null);
        }

        private void RemoveCharacter(object sender, EventArgs e)
        {
            if (MessageBox.Show(
                    this, $"Really remove character '{lblCharacterName.Text}'?\n\nWARNING! There is no undo", "Remove character", MessageBoxButtons.YesNo,
                    MessageBoxIcon.Stop, MessageBoxDefaultButton.Button2)
             != DialogResult.Yes)
                return;

            DungeonTimers.Remove(lblCharacterName.Text);
            Parent.Controls.Remove(this);
        }

        private void UndoDailyCompleted(object sender, EventArgs e)
        {
            DungeonTimers.UndoCompleted(lblCharacterName.Text);
            Update(null, null);
        }

        private void DailyCompleted(object sender, EventArgs e)
        {
            DungeonTimers.SetCompleted(lblCharacterName.Text);
            Update(null, null);
        }

        private void Update(object sender, EventArgs e)
        {
            TimeSpan left = DungeonTimers.GetTimeLeftFor(lblCharacterName.Text);
            if (left == TimeSpan.Zero)
            {
                BackColor = Color.LightGreen;
                lblTimeLeft.Text = "";

                btnDailyCompleted.Visible = true;
                btnUndoDailyCompleted.Visible = false;

                tmUpdate.Interval = 10000;
            }
            else if (left < TimeSpan.FromHours(1))
            {
                BackColor = Color.LightYellow;
                lblTimeLeft.Text = TimeLeftToString(left);

                btnDailyCompleted.Visible = true;
                btnUndoDailyCompleted.Visible = false;

                tmUpdate.Interval = 1000;
            }
            else if (left > TimeSpan.FromHours(19))
            {
                BackColor = Color.FromArgb(255, 192, 192);
                lblTimeLeft.Text = TimeLeftToString(left);

                btnDailyCompleted.Visible = false;
                btnUndoDailyCompleted.Visible = true;

                tmUpdate.Interval = 10000;
            }
            else
            {
                BackColor = Color.FromArgb(255, 192, 192);
                lblTimeLeft.Text = TimeLeftToString(left);

                btnDailyCompleted.Visible = true;
                btnUndoDailyCompleted.Visible = false;

                tmUpdate.Interval = 10000;
            }
        }

        private string TimeLeftToString(TimeSpan timeLeft)
        {
            string timeLeftString;

            if (timeLeft.Hours > 4)
            {
                if (timeLeft.Minutes > 55)
                    timeLeftString = timeLeft.Add(TimeSpan.FromHours(1)).ToString(@"h'h'", CultureInfo.InvariantCulture);
                else if (timeLeft.Minutes >= 25)
                    timeLeftString = timeLeft.ToString(@"h'h 30m'", CultureInfo.InvariantCulture);
                else
                    timeLeftString = timeLeft.ToString(@"h'h'", CultureInfo.InvariantCulture);
            }
            else if (timeLeft.Hours > 0 && timeLeft.Minutes > 0)
                timeLeftString = timeLeft.ToString(@"h'h 'm'm'", CultureInfo.InvariantCulture);
            else if (timeLeft.Hours > 0)
                timeLeftString = timeLeft.ToString(@"h'h'", CultureInfo.InvariantCulture);
            else if (timeLeft.Minutes > 0)
                timeLeftString = timeLeft.ToString(@"m'm 's's'", CultureInfo.InvariantCulture);
            else
                timeLeftString = timeLeft.ToString("s's'", CultureInfo.InvariantCulture);

            return timeLeftString;
        }
    }
}