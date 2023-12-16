namespace GenerateInsertStatements
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string[] primaryKeys = File.ReadAllLines("C:\\Development\\ACM\\GenerateInsertStatements\\PrimaryKeys.txt");
            string[] attendences = File.ReadAllLines("C:\\Development\\ACM\\GenerateInsertStatements\\Attendences.txt");
            if (primaryKeys == null || attendences == null)
                throw new Exception("null input data");
            if (primaryKeys.Length != attendences.Length)
                throw new Exception("input data are different lengths");

            const int MemberID = 37;

            List<string> outputLines = new();
            for (int i = 0; i < primaryKeys.Length; i++)
            {
                if (int.TryParse(attendences[i], out int didAttend) && didAttend != 0)
                    outputLines.Add($"INSERT INTO [dbo].[Attendee] ([DinnerID],[MemberID],[LevelID],[IsSponsor],[IsInductee]) VALUES ({primaryKeys[i]}, {MemberID}, 1, 0, 0)");
            }

            File.WriteAllLines("C:\\Development\\ACM\\GenerateInsertStatements\\InsertStatements.sql", outputLines);
        }
    }
}
