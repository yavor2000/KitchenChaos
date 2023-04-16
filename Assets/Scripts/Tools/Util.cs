using System;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;

namespace Tools
{
public class Util
{
    public static string GetStackTrace(int startIdx = 2, int endIdx = -1)
    {
        string trace = new System.Diagnostics.StackTrace().ToString();
        if (startIdx == 0 && endIdx == -1)
        {
            return trace;
        }
        
        string[] traceLines = trace.Split(new []{ Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
        int lineCnt = traceLines.Length;
        StringBuilder sb = new StringBuilder();
        int startIdxCorrected = startIdx < 0 ? 0 : startIdx;
        startIdxCorrected = startIdxCorrected > lineCnt - 1 ? lineCnt - 1 : startIdxCorrected;
        
        int endIdxCorrected = (endIdx == -1 || endIdx > lineCnt) ? lineCnt - 1 : endIdx;
        for (int i = startIdxCorrected; i <= endIdxCorrected; i++)
        {
            sb.AppendLine(traceLines[i]);
        }

        return sb.ToString();
    }
}
}