using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;



public class MIUController : IDisposable
{
    public enum MIU_MSG
    {
        MIU_OK = 0x00000000,
        MIU_CREATE_ERR = 0x00000001,
        MIU_NODEVICE = 0x00000002,
        MIU_TIMEOUT = 0x00000003,
        MIU_REQWRITEERROR = 0x00000004,
        MIU_BOARDNUMBERDUPLICATION = 0x00000005,
        MIU_ALREADY_OPENED = 0x00000006,
        MIU_ALREADY_CLOSED = 0x00000007,
        MIU_NOT_SUPPORT_MODEl = 0x00000008,
        MIU_NOT_DIRECT_MODE = 0x00000009,
        MIU_ENDPOINTOPENFAIL = 0x00000052,
        MIU_NEW_FRAME = 0x0100,
        MIU_RECEIVING = 0x0101,
        AF_SUCCESS = 0x0300,
        AF_TRY_AGAIN = 0x0301,
        AF_LIMIT_FAIL = 0x0302,
        AF_PEAK = 0x0303,
        MIU_VOLTAGECTL_ERROR = 0x00001001,
        MIU_PINCONTROL_ERROR = 0x00001002,
        MIU_ADCSET_ERROR = 0x00001003,
        MIU_ADCREAD_ERROR = 0x00001004,
        MIU_CALIBRATIONINIT_ERROR = 0x00001005,
    }
    public readonly int MIU_EVENT_TIMEOUT = 0x0000F001;
    public readonly int MIU_EVENT_ATTACHED = 0x0000F002;
    public readonly int MIU_EVENT_DETACHED = 0x0000F003;

    public readonly int OS_TESTPIN_COUNT = 55;

    public const uint MIU_MONO = 0x01000000;
    public const uint MIU_BAYER = 0x02000000;
    public const uint MIU_YUV = 0x03000000;
    public const uint MIU_RGB = 0x04000000;
    public const uint MIU_OCCUPY8BIT = 0x00080000;
    public const uint MIU_OCCUPY10BIT = 0x000A0000;
    public const uint MIU_OCCUPY12BIT = 0x000C0000;
    public const uint MIU_OCCUPY16BIT = 0x00100000;
    public const uint MIU_OCCUPY24BIT = 0x00180000;
    public const uint MIU_OCCUPY_CHECK = (MIU_OCCUPY10BIT | MIU_OCCUPY12BIT);

    public const uint MIU_MONO8 = (MIU_MONO | MIU_OCCUPY8BIT | 0x0001);
    public const uint MIU_MONO10_PACKED = (MIU_MONO | MIU_OCCUPY10BIT | 0x0002);
    public const uint MIU_MONO12_PACKED = (MIU_MONO | MIU_OCCUPY12BIT | 0x0003);
    public const uint MIU_MONO14 = (MIU_MONO | MIU_OCCUPY16BIT | 0x0004);

    public const uint MIU_BAYERGR8 = (MIU_BAYER | MIU_OCCUPY8BIT | 0x0001);
    public const uint MIU_BAYERRG8 = (MIU_BAYER | MIU_OCCUPY8BIT | 0x0002);
    public const uint MIU_BAYERGB8 = (MIU_BAYER | MIU_OCCUPY8BIT | 0x0003);
    public const uint MIU_BAYERBG8 = (MIU_BAYER | MIU_OCCUPY8BIT | 0x0004);
    public const uint MIU_BAYERGR10_PACKED = (MIU_BAYER | MIU_OCCUPY10BIT | 0x0005);
    public const uint MIU_BAYERRG10_PACKED = (MIU_BAYER | MIU_OCCUPY10BIT | 0x0006);
    public const uint MIU_BAYERGB10_PACKED = (MIU_BAYER | MIU_OCCUPY10BIT | 0x0007);
    public const uint MIU_BAYERBG10_PACKED = (MIU_BAYER | MIU_OCCUPY10BIT | 0x0008);
    public const uint MIU_BAYERGR12_PACKED = (MIU_BAYER | MIU_OCCUPY12BIT | 0x0009);
    public const uint MIU_BAYERRG12_PACKED = (MIU_BAYER | MIU_OCCUPY12BIT | 0x000A);
    public const uint MIU_BAYERGB12_PACKED = (MIU_BAYER | MIU_OCCUPY12BIT | 0x000B);
    public const uint MIU_BAYERBG12_PACKED = (MIU_BAYER | MIU_OCCUPY12BIT | 0x000C);
    public const uint MIU_RGB565 = (MIU_RGB | MIU_OCCUPY16BIT | 0x0001);
    public const uint MIU_BGR565 = (MIU_RGB | MIU_OCCUPY16BIT | 0x0002);
    public const uint MIU_RGB8_PACKED = (MIU_RGB | MIU_OCCUPY24BIT | 0x0001);
    public const uint MIU_BGR8_PACKED = (MIU_RGB | MIU_OCCUPY24BIT | 0x0002);
    public const uint MIU_YUV422_PACKED = (MIU_YUV | MIU_OCCUPY16BIT | 0x0001);
    public const uint MIU_YUV422_YUYV_PACKED = (MIU_YUV | MIU_OCCUPY16BIT | 0x0002);

    bool disposed = false;

    public enum MIU_REG_TYPE
    {
        REG_MIU = 0,
        REG_IIC1,
        REG_LEDCONTROL,
        REG_OS,
    }
    public enum MIU_CONF
    {
        SensorMode,
        SensorWidth,
        SensorHeight,
        MAXWidth,
        MAXHeight,
        MCLKOnOff,
        MCLKSelection,
        MCLK,
        PCLKInversion,
        IICDeviceID,
        IICMode,
        IICSpeed,
        IICReadRestart,
        IICReadRestartInterval,
        IICSCKPinCheck,
        IICAddressLength,
        IICDataLength,
        MIPILaneEnable,
        MIPIDataType,
        MIPI8bitMode,
        MIUIOVoltage,
        InitialSkipCount,
        PreviewSkipCount,
        ParallelSamplingMode,
        ParallelBitsPerPixel,
        ParallelPixelComponent,
        ParallelBitShift,
    }
    //[StructLayout(LayoutKind.Sequential)]
    public struct MIU_INITIALValue
    {
        public byte SensorMode;
        public ushort nWidth;
        public ushort nHeight;
        public byte MCLKOnOff;
        public byte MCLKSelection;
        public float MCLK;
        public byte PCLKInversion;
        public byte IICDeviceID;
        public byte IICMode;
        public byte IICSpeed;
        public byte IICReadRestart;
        public byte IICReadRestartInterval;
        public byte IICSCKPinCheck;
        public byte IICAddressLength;
        public byte IICDataLength;
        public byte MIPILaneEnable;
        public byte MIPIDataType;
        public float MIUIOVoltage;
        public byte FirstPowerChannel;
        public float FirstPowerVoltage;
        public byte SecondPowerChannel;
        public float SecondPowerVoltage;
        public byte ThirdPowerChannel;
        public float ThirdPowerVoltage;
        public byte FourthPowerChannel;
        public float FourthPowerVoltage;
        public byte FifthPowerChannel;
        public float FifthPowerVoltage;
        public byte Power5VoltOnOff;
        public byte Power12VoltOnOff;
        public byte InitialSkipCount;
        public ushort PreviewSkipCount;
        public byte ParallelSamplingMode;
        public byte ParallelBitsPerPixel;
        public byte ParallelPixelComponent;
        public byte ParallelBitShift;
        public byte MIPI8bitMode;
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct MIU_DEVICE
    {
        public byte bMIUOpen;
        public MIU_INITIALValue InitialValue;
        public byte CurrentState;

    }
    



    [StructLayout(LayoutKind.Sequential)]
    public struct AF_ROI
    {
        public int nXStart;
        public int nYStart;
    }
    public struct AFCONTROL
    {
        public Byte nActuatorI2CID;
        public Byte nActuatorI2CMode;// (0 : 8bit address, 8bit data, 1 : 8bit address, 16bit data, 2 : 16bit address, 8bit data, 3 : 16bit address, 16bit data)
        public Byte nSensorI2CID;
        public Byte nSensorI2CMode;// (0 : 8bit address, 8bit data, 1 : 8bit address, 16bit data, 2 : 16bit address, 8bit data, 3 : 16bit address, 16bit data)
        public ushort nMaxValue;
        public ushort nMinValue;
        public ushort nStepValue;
        public ushort nActuatorType;//Default : 0
        public int nRoiWidth;
        public int nRoiHeight;
        public bool bRoiOn;
        public AF_ROI AFRoi;
    }
#if x86
    private const string DllFile = "LPMC500DLL.dll";
#else
    private const string DllFile = "LPMC500DLLx64.dll";
#endif
    public static bool IsExistDllFile()
    {
        return File.Exists(DllFile);
    }
    public static bool MIU_1012bitPacked(uint format)
    {
        if ((format & MIU_OCCUPY10BIT) == MIU_OCCUPY10BIT)
            return true;
        if ((format & MIU_OCCUPY12BIT) == MIU_OCCUPY12BIT)
            return true;
        return false;
    }
    [DllImport(DllFile, CallingConvention = CallingConvention.Cdecl)]
    public static extern int MIUGetDeviceList(ref byte pDeviceCount, IntPtr pDeviceList);
    [DllImport(DllFile, CallingConvention = CallingConvention.Cdecl)]
    public static extern int MIUOpenDevice(byte iDeviceIndex);
    [DllImport(DllFile, CallingConvention = CallingConvention.Cdecl)]
    public static extern int MIUCloseDevice(byte iDeviceIndex);
    [DllImport(DllFile, CallingConvention = CallingConvention.Cdecl)]
    public static extern int MIUCloseAll();
    [DllImport(DllFile, CallingConvention = CallingConvention.Cdecl)]
    public static extern int MIUInitialize(byte iDeviceIndex, MIU_INITIALValue InitalValue);
    [DllImport(DllFile, CallingConvention = CallingConvention.Cdecl)]
    public static extern int MIUWriteRegister(byte iDeviceIndex, MIU_REG_TYPE iRegisterType, ushort nAddress, ushort nData);
    [DllImport(DllFile, CallingConvention = CallingConvention.Cdecl)]
    public static extern int MIUReadRegister(byte iDeviceIndex, MIU_REG_TYPE iRegisterType, ushort nAddress, ref ushort pData);
    [DllImport(DllFile, CallingConvention = CallingConvention.Cdecl)]
    public static extern int MIUWriteDirectI2C(byte iDeviceIndex, ushort nSlaveAddress, ushort nAddress, byte nAddressByteCount, ref byte pData, ushort WriteByteCount);
    [DllImport(DllFile, CallingConvention = CallingConvention.Cdecl)]
    public static extern int MIUReadDirectI2C(byte iDeviceIndex, ushort nSlaveAddress, ushort nAddress, byte nAddressByteCount, ref byte pData, ushort ReadByteCount);
    [DllImport(DllFile, CallingConvention = CallingConvention.Cdecl)]
    public static extern int MIUInitializeImageValue(byte iDeviceIndex, uint PixelFormat, uint nWidth, uint nHeight, uint nMaxWidth, uint nMaxHeight, byte nBufferCount);
    [DllImport(DllFile, CallingConvention = CallingConvention.Cdecl)]
    public static extern int MIUStart(byte iDeviceIndex);
    [DllImport(DllFile, CallingConvention = CallingConvention.Cdecl)]
    public static extern int MIUStop(byte iDeviceIndex);
    [DllImport(DllFile, CallingConvention = CallingConvention.Cdecl)]
    public static extern int MIUPause(byte iDeviceIndex);
    [DllImport(DllFile, CallingConvention = CallingConvention.Cdecl)]
    public static extern int MIUResume(byte iDeviceIndex, uint nWidth, uint nHeight);
    [DllImport(DllFile, CallingConvention = CallingConvention.Cdecl)]
    public static extern int MIUResumeChageFormat(byte iDeviceIndex, uint PixelFormat);
    [DllImport(DllFile, CallingConvention = CallingConvention.Cdecl)]
    public static extern int MIUPreviewPause(byte iDeviceIndex);
    [DllImport(DllFile, CallingConvention = CallingConvention.Cdecl)]
    public static extern int MIUPreviewResume(byte iDeviceIndex);
    [DllImport(DllFile, CallingConvention = CallingConvention.Cdecl)]
    public static extern int MIUGetImageData(byte iDeviceIndex, IntPtr ppImagePoint, ref Int64 pTimeStamp);
    [DllImport(DllFile, CallingConvention = CallingConvention.Cdecl)]
    public static extern int MIUGetDiagnostics(byte iDeviceIndex, ref uint pImageCount, ref float pFramerate, ref float pBitrate);
    [DllImport(DllFile, CallingConvention = CallingConvention.Cdecl)]
    public static extern int MIUGetSensorFrameRate(byte iDeviceInde, ref float pSensorFramerate);
    [DllImport(DllFile, CallingConvention = CallingConvention.Cdecl)]
    public static extern int IICWrite(byte iDeviceIndex, MIU_REG_TYPE iRegisterType, byte SlaveID, int ProtocolType, int DataSize, ref byte[] pdata, bool bWriteMode);
    [DllImport(DllFile, CallingConvention = CallingConvention.Cdecl)]
    public static extern int IICWriteCX(byte iDeviceIndex, MIU_REG_TYPE iRegisterType, byte SlaveID, int ProtocolType, int DataSize, ref byte pdata, bool bWriteMode);
    [DllImport(DllFile, CallingConvention = CallingConvention.Cdecl)]
    public static extern int IICWriteN(byte iDeviceIndex, MIU_REG_TYPE iRegisterType, byte SlaveID, int DataSize, ref byte pdata);
    [DllImport(DllFile, CallingConvention = CallingConvention.Cdecl)]
    public static extern int IICRead(byte iDeviceIndex, MIU_REG_TYPE iRegisterType, byte SlaveID, int ProtocolType, int DataSize, ref byte pdata, int ReadSize, ref byte pReadData);
    [DllImport(DllFile, CallingConvention = CallingConvention.Cdecl)]
    public static extern int IICReadCX(byte iDeviceIndex, MIU_REG_TYPE iRegisterType, byte SlaveID, int ProtocolType, int DataSize, ref byte pdata, int ReadSize, ref byte pReadData);
    [DllImport(DllFile, CallingConvention = CallingConvention.Cdecl)]
    public static extern uint CurrentMeasureAllPowerOn(byte iDeviceIndex, byte n1stPowerPin, float n1stVoltage,
        byte n2ndPowerPin, float n2ndVoltage,
        byte n3rdPowerPin, float n3rdVoltage,
        byte n4thPowerPin, float n4thVoltage,
        byte n5thPowerPin, float n5thVoltage);
    [DllImport(DllFile, CallingConvention = CallingConvention.Cdecl)]
    public static extern uint CurrentMeasureMode(byte iDeviceIndex, byte Mode);
    [DllImport(DllFile, CallingConvention = CallingConvention.Cdecl)]
    public static extern uint CurrentMeasureResult(byte iDeviceIndex, byte nChannelNumber, ref double CurrentMeasureResult, byte Mode);
    [DllImport(DllFile, CallingConvention = CallingConvention.Cdecl)]
    public static extern uint CurrentMeasurePowerOff(byte iDeviceIndex);
    [DllImport(DllFile, CallingConvention = CallingConvention.Cdecl)]
    public static extern void AutoCalibration(byte iDeviceIndex);
    [DllImport(DllFile, CallingConvention = CallingConvention.Cdecl)]
    public static extern void InitializeCalibration(byte iDeviceIndex);
    [DllImport(DllFile, CallingConvention = CallingConvention.Cdecl)]
    public static extern int FirmwareDownload(byte iDeviceIndex, uint nSize, ref byte pData);
    [DllImport(DllFile, CallingConvention = CallingConvention.Cdecl)]
    public static extern int MIUMainClockControl(byte iDeviceIndex, byte bOnOff, byte ClockSelection);
    [DllImport(DllFile, CallingConvention = CallingConvention.Cdecl)]
    public static extern int MIUSetMCLK(byte iDeviceIndex, float fMCLK);
    [DllImport(DllFile, CallingConvention = CallingConvention.Cdecl)]
    public static extern int MIUSetPowerChannel0(byte iDeviceIndex, float fVoltage, byte OnOff);
    [DllImport(DllFile, CallingConvention = CallingConvention.Cdecl)]
    public static extern int MIUSetPowerChannel1(byte iDeviceIndex, float fVoltage, byte OnOff);
    [DllImport(DllFile, CallingConvention = CallingConvention.Cdecl)]
    public static extern int MIUSetPowerChannel2(byte iDeviceIndex, float fVoltage, byte OnOff);
    [DllImport(DllFile, CallingConvention = CallingConvention.Cdecl)]
    public static extern int MIUSetPowerChannel3(byte iDeviceIndex, float fVoltage, byte OnOff);
    [DllImport(DllFile, CallingConvention = CallingConvention.Cdecl)]
    public static extern int MIUSetPowerChannel4(byte iDeviceIndex, float fVoltage, byte OnOff);
    [DllImport(DllFile, CallingConvention = CallingConvention.Cdecl)]
    public static extern int MIUSetPowerChannel5V(byte iDeviceIndex, byte OnOff);
    [DllImport(DllFile, CallingConvention = CallingConvention.Cdecl)]
    public static extern int MIUSetPowerChannel12V(byte iDeviceIndex, byte OnOff);
    [DllImport(DllFile, CallingConvention = CallingConvention.Cdecl)]
    public static extern void MIUGetFirmwareInfomation(byte iDeviceIndex, ref byte pInformation);
    [DllImport(DllFile, CallingConvention = CallingConvention.Cdecl)]
    public static extern int MIUWriteSPISensor(byte iDeviceIndex, ushort nData);
    [DllImport(DllFile, CallingConvention = CallingConvention.Cdecl)]
    public static extern int MIUReadSPISensor(byte iDeviceIndex, ref ushort pData);
    [DllImport(DllFile, CallingConvention = CallingConvention.Cdecl)]
    public static extern int MIUGetGPIStatus(byte iDeviceIndex, byte nGPINumber, ref byte pStatus);
    [DllImport(DllFile, CallingConvention = CallingConvention.Cdecl)]
    public static extern int MIUSetGPOStatus(byte iDeviceIndex, byte nGPONumber, byte nStatus);
    [DllImport(DllFile, CallingConvention = CallingConvention.Cdecl)]
    public static extern int MIUGetGPIOMode(byte iDeviceIndex, byte nGPIONumber, ref byte pMode);
    [DllImport(DllFile, CallingConvention = CallingConvention.Cdecl)]
    public static extern int MIUSetGPIOMode(byte iDeviceIndex, byte nGPIONumber, byte nMode);
    [DllImport(DllFile, CallingConvention = CallingConvention.Cdecl)]
    public static extern int MIUGetGPIOStatus(byte iDeviceIndex, byte nGPIONumber, ref byte pStatus);
    [DllImport(DllFile, CallingConvention = CallingConvention.Cdecl)]
    public static extern int MIUSetGPIOStatus(byte iDeviceIndex, byte nGPIONumber, byte nStatus);
    [DllImport(DllFile, CallingConvention = CallingConvention.Cdecl)]
    public static extern uint OpenShortTest(byte iDeviceIndex, ref float UpperTestResult, ref float LowerTestResult,
        byte nInputCurrent, byte nSensorMode, ref int UpperEn, ref int LowerEn, int UpperSleep, int LowerSleep);
    [DllImport(DllFile, CallingConvention = CallingConvention.Cdecl)]
    public static extern uint LeakageTest(byte iDeviceIndex, ref float LeakageTestResult, float IOVoltage, byte nSensorMode,
        ref int LeakageEn, int LeakageSleep);
    [DllImport(DllFile, CallingConvention = CallingConvention.Cdecl)]
    public static extern int MIUSetI2CMode(byte iDeviceIndex, ushort nMode);
    [DllImport(DllFile, CallingConvention = CallingConvention.Cdecl)]
    public static extern int MIUSetI2CID(byte iDeviceIndex, byte nI2CID);
    [DllImport(DllFile, CallingConvention = CallingConvention.Cdecl)]
    public static extern int MIUSet5V12VPowSequence(byte iDeviceIndex, int nSequence);
    [DllImport(DllFile, CallingConvention = CallingConvention.Cdecl)]
    public static extern int MIUSetPowerChSleep(byte iDeviceIndex, ref int PowerChSleep);
    [DllImport(DllFile, CallingConvention = CallingConvention.Cdecl)]
    public static extern int SetAFSettingValue(uint nAFConfigValue, uint nOffsetValue);
    [DllImport(DllFile, CallingConvention = CallingConvention.Cdecl)]
    public static extern int ProgressAF(byte iDeviceIndex, ref byte pImage, uint nWidth, uint nHeight, ref int pActTarget,
        ref AFCONTROL afControlValue, bool bAutoFocusAction = true);
    [DllImport(DllFile, CallingConvention = CallingConvention.Cdecl)]
    public static extern int MIUMoveVCM(byte iDeviceIndex, int code, AFCONTROL afControlValue);
    [DllImport(DllFile, CallingConvention = CallingConvention.Cdecl)]
    public static extern int MIULSTtoFPGA(byte iDeviceIndex, byte iLSCRomIndex);
    [DllImport(DllFile, CallingConvention = CallingConvention.Cdecl)]
    public static extern int MIUReadLsVersion(byte iDeviceIndex, uint nSize, ref byte pData, byte nMemoryIndex);
    [DllImport(DllFile, CallingConvention = CallingConvention.Cdecl)]
    public static extern int MIURomDataDownload(byte iDeviceIndex, uint nSize, ref byte pData, byte nMemoryIndex);
    [DllImport(DllFile, CallingConvention = CallingConvention.Cdecl)]
    public static extern int LSCTableMake(ref double pAverageImage, uint nWidth, uint nHeight, ref ushort pEvenTable, ref ushort pOddTable,
        ref int pEvenSize, ref int pOddSize);
    [DllImport(DllFile, CallingConvention = CallingConvention.Cdecl)]
    public static extern int MIUGetDeviceCount();
    [DllImport(DllFile, CallingConvention = CallingConvention.Cdecl)]
    public static extern int MIUCheckLicense(ref byte inputStr);
    [DllImport(DllFile, CallingConvention = CallingConvention.Cdecl)]
    public static extern int MIUGetDllVersion(ref byte nVersion);

    public delegate void MIUCallback(byte iDeviceIndex, int Event);
    [DllImport(DllFile, CallingConvention = CallingConvention.StdCall)]
    public static extern void USBRestartCallback(MIUCallback pCallback, uint WaitTime);
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    protected virtual void Dispose(bool disposing)
    {
        if (disposed)
            return;
        if (disposing)
        {

        }
        disposed = true;
    }
    ~MIUController()
    {
        Dispose(false);
    }
}

