using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Newtonsoft.Json;

namespace DailyRandomDungeon
{
    public static class DungeonTimers
    {
        public static TimeSpan GetTimeLeftFor(string characterName)
        {
            Dictionary<string, DateTime> state = LoadState();
            if (!state.TryGetValue(characterName, out DateTime whenAvailable))
                return TimeSpan.Zero;

            DateTime cutoff = DateTime.Now;
            if (whenAvailable < cutoff)
                return TimeSpan.Zero;

            return whenAvailable - cutoff;
        }

        public static void SetCompleted(string characterName)
        {
            Dictionary<string, DateTime> state = LoadState();
            if (state.ContainsKey(characterName))
                state[characterName + ".undo"] = state[characterName];

            state[characterName] = DateTime.Now.AddHours(20);
            SaveState(state);
        }

        public static void UndoCompleted(string characterName)
        {
            Dictionary<string, DateTime> state = LoadState();
            if (state.ContainsKey(characterName + ".undo"))
            {
                state[characterName] = state[characterName + ".undo"];
                state.Remove(characterName + ".undo");
            }
            else
            {
                if (!state.Remove(characterName))
                    return;
            }

            SaveState(state);
        }

        public static List<string> GetCharacters()
            => LoadState().Keys.Where(s => !s.EndsWith(".undo")).OrderBy(s => s, StringComparer.InvariantCultureIgnoreCase).ToList();

        private static string StateFilePath()
        {
            var settingsPath = Environment.GetEnvironmentVariable("DROPBOX");
            if (String.IsNullOrWhiteSpace(settingsPath))
                settingsPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            else
                settingsPath = Path.Combine(settingsPath, "UserFolder");

            return Path.Combine(settingsPath, ".eso-daily-dungeon-timers.json");
        }

        private static Dictionary<string, DateTime> LoadState()
        {
            var filePath = StateFilePath();
            if (!File.Exists(filePath))
                return new();

            return JsonConvert.DeserializeObject<Dictionary<string, DateTime>>(File.ReadAllText(filePath, Encoding.UTF8));
        }

        private static void SaveState(Dictionary<string, DateTime> state)
        {
            File.WriteAllText(StateFilePath(), JsonConvert.SerializeObject(state), Encoding.UTF8);
        }

        public static bool HasUndo(string characterName)
        {
            Dictionary<string, DateTime> state = LoadState();
            return state.ContainsKey(characterName + ".undo");
        }

        public static void Remove(string characterName)
        {
            Dictionary<string, DateTime> state = LoadState();
            if (state.Remove(characterName) | state.Remove(characterName + ".undo"))
                SaveState(state);
        }

        public static void Add(string characterName)
        {
            Dictionary<string, DateTime> state = LoadState();
            state.Add(characterName, DateTime.Now);
            SaveState(state);
        }
    }
}