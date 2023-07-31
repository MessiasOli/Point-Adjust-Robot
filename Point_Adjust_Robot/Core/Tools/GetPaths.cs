namespace Point_Adjust_Robot.Core.Tools
{
    public static class GetPaths
    {
        public static string Logs()
        {
            string path = Directory.GetCurrentDirectory();
            path =  path.Contains("bin") ? PathVS(path) : PathIIS(path);

            path += path.Contains("/") ? "/Log" : "\\Log";

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            Console.WriteLine(path);
            return path;
        }

        private static string PathIIS(string path) => Directory.GetParent(path).ToString();
        private static string PathVS(string path) => Directory.GetParent(Directory.GetParent(Directory.GetParent(Directory.GetParent(path).ToString()).ToString()).ToString()).ToString();
    }
}
