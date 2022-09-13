using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class CSVReader
{
    // CVS파일의 줄을 나누는 정규표현식이다.
    private static string LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r";
    // CVS파일의 줄을 객채마다 나누는 정규표현식이다.
    private static string SPLIT_RE = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";
    // \"를 제거해준다.
    private static char[] TRIM_CHARS = { '\"' };

    public static List<Dictionary<string, object>> Read(string file)
    {
        // 반환할 리스트 변수를 정의한다.
        var list = new List<Dictionary<string, object>>();
        file = "CSVFolder/" + file;
        // CVS파일을 가져온다.
        TextAsset data = Resources.Load(file) as TextAsset;

        // data를 줄로 나누어 변수로 선언한다.
        var lines = Regex.Split(data.text, LINE_SPLIT_RE);

        // 만약 1줄 이하라면 그대로 반환한다.
        // 왜냐하면 첫번째줄은 헤더이기 때문에 1줄이하일 경우 데이터가 없는 것과 같다.
        if (lines.Length <= 1) return list;

        // 2줄 이상일 경우 첫번째 줄을 헤더로 나누어 지정한다.
        var header = Regex.Split(lines[0], SPLIT_RE);
        for (var i = 1; i < lines.Length; i++)
        {
            // 줄은 있으나 데이터가 없는 경우 스킵
            var values = Regex.Split(lines[i], SPLIT_RE);
            if (values.Length == 0 || values[0] == "") continue;

            var entry = new Dictionary<string, object>();
            for (var j = 0; j < header.Length && j < values.Length; j++)
            {
                string value = values[j];
                // \"를 앞 뒤로 모두 제거해주고 \를 공백으로 제거해 준다.
                value = value.TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS).Replace("\\", "");
                object finalvalue = value;

                // 만약 value가 int 또는 float으로 변환이 가능하다면 변환하여 저장한다.
                if (int.TryParse(value, out int n))
                    finalvalue = n;
                else if (float.TryParse(value, out float f))
                    finalvalue = f;

                entry[header[j]] = finalvalue;
            }
            list.Add(entry);
        }
        return list;
    }
}