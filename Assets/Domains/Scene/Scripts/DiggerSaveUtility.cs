namespace Domains.Scene.Scripts
{
    public static class DiggerSaveUtility
    {
        private const string AutoSaveKey = "AutoSave";
        // private const string ForceDeleteOnStartKey = "ForceDeleteOnStart";

        public static void Reset()
        {
            var path = "GameSave.es3";

            if (ES3.FileExists(path))
                if (ES3.KeyExists(AutoSaveKey, path))
                    ES3.DeleteKey(AutoSaveKey, path);
            // if (ES3.KeyExists(ForceDeleteOnStartKey, path)) ES3.DeleteKey(ForceDeleteOnStartKey, path);
        }
    }
}