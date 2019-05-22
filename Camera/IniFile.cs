using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;


 public class IniFile
 {
    /// <summary>
    /// ini 파일명을 저장
    /// </summary>
    private string INIFileName;
    private string sectionName;
    /// <summary>
    ///     ini  파일을 지정하거나 가져올때 쓰는 속성
    /// </summary>
    public string FileName
    {
        get { return INIFileName; }
        set { INIFileName = value; }
    }
    public string SectionName
    {
        get { return sectionName; }
        set { sectionName = value; }
    }
    /// <summary>
    /// 생성자 : 사용할 ini 파일을 지정
    /// </summary>
    /// <param name="FileName">사용할 파일명</param>
    public IniFile(string FileName)
    {
        INIFileName = FileName;
    }
    /// <summary>
    /// ini 파일에서 정보를 가져오기 위한 API 기초 함수
    /// </summary>
    [DllImport("kernel32.dll")]
    private static extern int GetPrivateProfileString(
                string section,
                string key,
                string def,
                StringBuilder retVal,
                int size,
                string filePath);
    /// <summary>
    /// ini 파일에서 정보를 쓰기위한 위한 API 기초 함수
    /// </summary>
    [DllImport("kernel32.dll")]
    private static extern long WritePrivateProfileString(
                string section,
                string key,
                string val,
                string filePath);
    /// <summary>
    /// ini 파일에 정보를 기록하기 위한 함수
    /// </summary>
    /// <param name="Section">섹션명</param>
    /// <param name="Key">키 명</param>
    /// <param name="Value">기록할 값</param>
    private void _IniWriteValue(string Section, string Key, string Value)
    {
        WritePrivateProfileString(Section, Key, Value, INIFileName);
    }
    /// <summary>
    /// ini 파일에 정보를 가져오기 위한 함수
    /// </summary>
    /// <param name="Section">섹션명</param>
    /// <param name="Key">키 명</param>
    /// <returns>가져온 값</returns>
    private string _IniReadValue(string Section, string Key, string def)
    {
        StringBuilder temp = new StringBuilder(2000);
        int i = GetPrivateProfileString(Section, Key, def, temp, 2000, INIFileName);
        int pos = temp.ToString().IndexOf(";");
        if (pos > 0) temp = temp.Remove(pos, temp.Length - pos);
        return temp.ToString().Trim();
    }
    /// <summary>
    /// 문자열 타입으로 값을 기록한다
    /// </summary>
    /// <param name="Section">섹션명</param>
    /// <param name="Key">키 명</param>
    /// <param name="Value">기록 할 문자열</param>
    public void SetString(string Key, string Value)
    {
        _IniWriteValue(sectionName, Key, Value.Trim());
    }
    /// <summary>
    /// 정수 타입으로 값을 기록한다
    /// </summary>
    /// <param name="Section">섹션명 </param>
    /// <param name="Key">키 명</param>
    /// <param name="Value">기록 할  정수값</param>
    /// 
    public void SetInteger(string Key, int Value)
    {
        _IniWriteValue(sectionName, Key, Value.ToString().Trim());
    }
    /// <summary>
    /// 논리 타입으로 값을 기록 한다.
    /// </summary>
    /// <param name="Section">섹션명</param>
    /// <param name="Key">키 명</param>
    /// <param name="Value">기록 할 논리 값</param>
    public void SetBoolean(string Key, bool Value)
    {
        _IniWriteValue(sectionName, Key, Value ? "1" : "0");
    }
    /// <summary>
    /// 논리 타입으로 값을 가져온다
    /// </summary>
    /// <param name="Section">섹션명</param>
    /// <param name="Key">키 값</param>
    /// <param name="def">기본값</param>
    /// <returns>가져온 논리값</returns>
    public bool GetBoolean(string Key, bool def)
    {
        bool temp = def;
        string stTemp = _IniReadValue(sectionName, Key, "");
        if (stTemp == "") return def;
        if (stTemp.Trim() == "1") return true;
        else return false;
    }
    /// <summary>
    /// 문자열로 값을 가져 온다
    /// </summary>
    /// <param name="Section">섹션명</param>
    /// <param name="Key">키 명</param>
    /// <returns>가져온 문자열</returns>
    public string GetString(string Key, string def = "")
    {
        return _IniReadValue(sectionName, Key, def).Trim();
    }
    /// <summary>
    /// 정수 타입으로 값을 가져 온다
    /// </summary>
    /// <param name="Section">섹션명</param>
    /// <param name="Key">키 명</param>
    /// <param name="def">기본값</param>
    /// <returns>가져온 정수값</returns>
    public int GetInteger(string Key, int def = 0)
    {
        int temp = def;
        string stTemp = _IniReadValue(sectionName, Key, "");
        if (stTemp == "") return def;
        try
        {
            temp = int.Parse(stTemp.Trim());
        }
        catch (Exception)
        {
            return def;
        }
        return temp;
    }
    public byte GetByte(string Key, byte def = 0)
    {
        return (byte)GetInteger(Key, (int)def);
    }
    public ushort GetUShort(string Key, ushort def = 0)
    {
        return (ushort)GetInteger(Key, (int)def);
    }
    public float GetFloat(string Key, float def =0.0f)
    {
        float temp = def;
        string stTemp = _IniReadValue(sectionName, Key, "");
        if (stTemp == "") return def;
        float.TryParse(stTemp, out temp);
        return temp;
    }
    public byte GetHexFormat(string Key, byte def = 0)
    {
        byte temp = def;
        string stTemp = _IniReadValue(sectionName, Key, "");
        if (stTemp == "") return def;
        int pos = stTemp.IndexOf("x");
        if (pos > 0) stTemp = stTemp.Remove(0, pos + 1);
        byte.TryParse(stTemp, System.Globalization.NumberStyles.HexNumber, null, out temp);
        return temp;
    }

    public bool IsFileExist()
    {
        return (System.IO.File.Exists(INIFileName));
    }
}

