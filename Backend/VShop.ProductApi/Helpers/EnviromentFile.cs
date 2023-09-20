using System;
using System.IO;

namespace VShop.ProductApi;

public static class EnviromentFile
{
    public static bool Load(string enviromentFilePath)
    {
        if (!File.Exists(path: enviromentFilePath))
            return false;

        const int VARIABLE_NAME = 0, VARIABLE_VALUE = 1;
        foreach (string currentLine in File.ReadAllLines(path: enviromentFilePath))
        {
            string[] variable = currentLine.Split('=', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

            if (variable.Length != 2)
                continue;

            Environment.SetEnvironmentVariable(variable[VARIABLE_NAME], variable[VARIABLE_VALUE]);
        }

        return true;
    }

}
