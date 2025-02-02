﻿using System.Reflection;

namespace Helper
{
    public class OptionIniParser
    {
        public static string ReadKey(string keyName, string assemblyName)
        {
            StreamReader _streamReader = new(Path.Combine(RegistryService.GameAppdataFolderPath(assemblyName), ConstStrings.C_OPTIONSINI_FILENAME));
            {
                string keyValue = ConstStrings.C_REGISTRY_SERVICE_WRONG_PARAMETER;
                using (_streamReader)
                {
                    string importedOptionsFileText;
                    while ((importedOptionsFileText = _streamReader.ReadLine()!) != null)
                    {
                        string[] parts = importedOptionsFileText.Split(" = ");
                        if ((parts.Length == 2) && (parts[0] == keyName))
                        {
                            keyValue = parts[1];
                        }
                    }
                }
                return keyValue;
            }
        }

        public static void WriteKey(string keyName, string keyValue, string assemblyName)
        {
            StreamReader _streamReader = new(Path.Combine(RegistryService.GameAppdataFolderPath(assemblyName), ConstStrings.C_OPTIONSINI_FILENAME));
            string importedOptionsFileText;
            string changedOptionsFileText;
            importedOptionsFileText = _streamReader.ReadToEnd();
            _streamReader.Close();
            _streamReader.Dispose();

            if (!importedOptionsFileText.Contains(keyName))
            {
                changedOptionsFileText = importedOptionsFileText += (keyName + " = " + keyValue + Environment.NewLine);
            }
            else
            {
                changedOptionsFileText = importedOptionsFileText.Replace(keyName + " = " + ReadKey(keyName, assemblyName), keyName + " = " + keyValue + Environment.NewLine);
            }

            File.WriteAllText(Path.Combine(RegistryService.GameAppdataFolderPath(assemblyName), ConstStrings.C_OPTIONSINI_FILENAME), changedOptionsFileText);
        }

        public static void DeleteKey(string keyName, string assemblyName)
        {
            StreamReader _streamReader = new(Path.Combine(RegistryService.GameAppdataFolderPath(assemblyName), ConstStrings.C_OPTIONSINI_FILENAME));
            string importedOptionsFileText;
            string changedOptionsFileText;
            importedOptionsFileText = _streamReader.ReadToEnd();
            _streamReader.Close();
            _streamReader.Dispose();

            if (importedOptionsFileText.Contains(keyName))
            {
                changedOptionsFileText = importedOptionsFileText.Replace(keyName + " = " + ReadKey(keyName, assemblyName), "");
            }
            else
            {
                return;
            }

            File.WriteAllText(Path.Combine(RegistryService.GameAppdataFolderPath(assemblyName), ConstStrings.C_OPTIONSINI_FILENAME), changedOptionsFileText);
        }

        public static void ClearOptionsFile(string assemblyName)
        {
            StreamReader _streamReader = new(Path.Combine(RegistryService.GameAppdataFolderPath(assemblyName), ConstStrings.C_OPTIONSINI_FILENAME));
            string importedOptionsFileText = _streamReader.ReadToEnd();
            _streamReader.Close();
            _streamReader.Dispose();

            char[] lineFeedChars = { '\n', '\r' };
            string[] cleanStringArray = importedOptionsFileText.Split(lineFeedChars, StringSplitOptions.RemoveEmptyEntries);
            string cleanString = String.Join(Environment.NewLine, cleanStringArray);

            string changedOptionsFileText = cleanString + Environment.NewLine;

            File.WriteAllText(Path.Combine(RegistryService.GameAppdataFolderPath(assemblyName), ConstStrings.C_OPTIONSINI_FILENAME), changedOptionsFileText);
        }
    }
}
