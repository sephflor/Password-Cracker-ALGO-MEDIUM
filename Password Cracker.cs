using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using System.Text;
using System;

class Result
{

    /*
     * Complete the 'passwordCracker' function below.
     *
     * The function is expected to return a STRING.
     * The function accepts following parameters:
     *  1. STRING_ARRAY passwords
     *  2. STRING loginAttempt
     */

    public static string passwordCracker(List<string> passwords, string loginAttempt)
    {
         HashSet<string> passSet = new HashSet<string>(passwords);
        Dictionary<int, List<string>> dp = new Dictionary<int, List<string>>();
        
        List<string> result = Crack(loginAttempt, 0, passSet, dp);
        
        if (result == null) {
            return "WRONG PASSWORD";
        }
        
        return string.Join(" ", result);
    }
    
    private static List<string> Crack(string attempt, int index, HashSet<string> passSet, Dictionary<int, List<string>> dp) {
        if (index == attempt.Length) {
            return new List<string>();
        }
        
        if (dp.ContainsKey(index)) {
            return dp[index];
        }
        
        for (int i = index; i < attempt.Length; i++) {
            string current = attempt.Substring(index, i - index + 1);
            
            if (passSet.Contains(current)) {
                List<string> rest = Crack(attempt, i + 1, passSet, dp);
                
                if (rest != null) {
                    List<string> result = new List<string> { current };
                    result.AddRange(rest);
                    dp[index] = result;
                    return result;
                }
            }
        }
        
        dp[index] = null;
        return null;
    }

    // Alternative iterative DP approach
    public static string passwordCrackerDP(List<string> passwords, string loginAttempt) {
        int n = loginAttempt.Length;
        string[] dp = new string[n + 1];
        dp[0] = "";
        
        for (int i = 0; i <= n; i++) {
            if (dp[i] != null) {
                foreach (string pass in passwords) {
                    int end = i + pass.Length;
                    if (end <= n && loginAttempt.Substring(i, pass.Length) == pass) {
                        if (dp[end] == null) {
                            dp[end] = dp[i] + (i == 0 ? "" : " ") + pass;
                        }
                    }
                }
            }
        }
        
        return dp[n] ?? "WRONG PASSWORD";
    }


    }

class Solution
{
    public static void Main(string[] args)
    {
        TextWriter textWriter = new StreamWriter(@System.Environment.GetEnvironmentVariable("OUTPUT_PATH"), true);

        int t = Convert.ToInt32(Console.ReadLine().Trim());

        for (int tItr = 0; tItr < t; tItr++)
        {
            int n = Convert.ToInt32(Console.ReadLine().Trim());

            List<string> passwords = Console.ReadLine().TrimEnd().Split(' ').ToList();

            string loginAttempt = Console.ReadLine();

            string result = Result.passwordCracker(passwords, loginAttempt);

            textWriter.WriteLine(result);
        }

        textWriter.Flush();
        textWriter.Close();
    }
}
