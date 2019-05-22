using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static MIUController;

namespace Camera
{
    class LPMC650 : IDisposable
    {
        struct ISPParam
        {
            public string FileName;
            public byte DeviceID;
        }
        public readonly byte NO_DEVICE = 99;
        public readonly byte BUFFER_COUNT = 7;
        private byte deviceIndex;
        public uint PixelFormat { get; set; }
        public bool Connected { get; set; }
        public bool Burst { get; set; }
        private MIU_INITIALValue InitValue;
        public IniFile MIUIni;
        IplImage Image; 
        IplImage ImageItp;
        public CvSize ImageSize;
        public LPMC650()
        {
            deviceIndex = NO_DEVICE;
            Burst = false;
            PixelFormat = MIU_BAYERRG10_PACKED;
            ImageSize = new CvSize(1920, 1080);
            Image = new IplImage(ImageSize, BitDepth.U8, 1);
            ImageItp = new IplImage(ImageSize, BitDepth.U8, 3);
        }
        ~LPMC650()
        {
           MIUController.MIUCloseAll();
        }

        public void Dispose()
        {
            Cv.ReleaseImage(Image);
            Cv.ReleaseImage(ImageItp);
        }
        
        public bool Connect()
        {
            MIU_MSG Ret = MIU_MSG.MIU_OK;
            Connected = false;
            byte DeviceCount = 0;
            byte[] DeviceList = new byte[4] { 0, 0, 0, 0 };
            IntPtr DList = Marshal.AllocHGlobal(4);
            Ret = (MIU_MSG)MIUController.MIUGetDeviceList(ref DeviceCount, DList);
            Marshal.FreeHGlobal(DList);
            if (DeviceCount == 0)
                return false;
            Ret = (MIU_MSG)MIUController.MIUOpenDevice(0);
            if (Ret != MIU_MSG.MIU_OK)
                return false;
            Connected = true;
            deviceIndex = 0;
            return true;
        }
        public void Disconnect()
        {
            this.Stop();
            MIUController.MIUCloseDevice(0);
            MIUController.MIUCloseAll();
            Connected = false;
            deviceIndex = NO_DEVICE;
        }
        public bool Initialize(string IniFileName)
        {
            MIUIni = new IniFile(IniFileName);
            MIUIni.SectionName = "MIUConfiguration";
            //InitValue
            InitValue.SensorMode = MIUIni.GetByte(MIU_CONF.SensorMode.ToString());
            InitValue.nWidth = MIUIni.GetUShort(MIU_CONF.SensorWidth.ToString());
            InitValue.nHeight = MIUIni.GetUShort(MIU_CONF.SensorHeight.ToString());
            InitValue.MCLKOnOff = MIUIni.GetByte(MIU_CONF.MCLKOnOff.ToString());
            InitValue.MCLKSelection = MIUIni.GetByte(MIU_CONF.MCLKSelection.ToString());
            InitValue.MCLK = MIUIni.GetFloat(MIU_CONF.MCLK.ToString());
            InitValue.PCLKInversion = MIUIni.GetByte(MIU_CONF.PCLKInversion.ToString());
            InitValue.IICDeviceID = MIUIni.GetHexFormat(MIU_CONF.IICDeviceID.ToString());
            InitValue.IICMode = MIUIni.GetByte(MIU_CONF.IICMode.ToString());
            InitValue.IICSpeed = MIUIni.GetByte(MIU_CONF.IICSpeed.ToString());
            InitValue.IICReadRestart = MIUIni.GetByte(MIU_CONF.IICReadRestart.ToString(), 1);
            InitValue.IICReadRestartInterval = MIUIni.GetByte(MIU_CONF.IICReadRestartInterval.ToString());
            InitValue.IICSCKPinCheck = MIUIni.GetByte(MIU_CONF.IICSCKPinCheck.ToString());
            InitValue.IICAddressLength = MIUIni.GetByte(MIU_CONF.IICAddressLength.ToString());
            InitValue.IICDataLength = MIUIni.GetByte(MIU_CONF.IICDataLength.ToString());
            InitValue.MIPILaneEnable = MIUIni.GetByte(MIU_CONF.MIPILaneEnable.ToString(), 1);
            InitValue.MIPIDataType = MIUIni.GetHexFormat(MIU_CONF.MIPIDataType.ToString());
            InitValue.MIUIOVoltage = MIUIni.GetFloat(MIU_CONF.MIUIOVoltage.ToString());
            InitValue.FirstPowerVoltage = MIUIni.GetFloat("1stPowerVoltage");
            InitValue.FirstPowerChannel = MIUIni.GetByte("1stPowerChannel");
            InitValue.SecondPowerVoltage = MIUIni.GetFloat("2ndPowerVoltage");
            InitValue.SecondPowerChannel = MIUIni.GetByte("2ndPowerChannel");
            InitValue.ThirdPowerVoltage = MIUIni.GetFloat("3rdPowerVoltage");
            InitValue.ThirdPowerChannel = MIUIni.GetByte("3rdPowerChannel");
            InitValue.FourthPowerVoltage = MIUIni.GetFloat("4thPowerVoltage");
            InitValue.FourthPowerChannel = MIUIni.GetByte("4thPowerChannel");
            InitValue.FifthPowerVoltage = MIUIni.GetFloat("5thPowerVoltage");
            InitValue.FifthPowerChannel = MIUIni.GetByte("5thPowerChannel");
            InitValue.Power5VoltOnOff = MIUIni.GetByte("5VoltPowerOnOff");
            InitValue.Power12VoltOnOff = MIUIni.GetByte("12VoltPowerOnOff");
            InitValue.InitialSkipCount = MIUIni.GetByte(MIU_CONF.InitialSkipCount.ToString());
            InitValue.PreviewSkipCount = MIUIni.GetUShort(MIU_CONF.PreviewSkipCount.ToString());
            InitValue.ParallelSamplingMode = MIUIni.GetByte(MIU_CONF.ParallelSamplingMode.ToString());
            InitValue.ParallelBitsPerPixel = MIUIni.GetByte(MIU_CONF.ParallelBitsPerPixel.ToString());
            InitValue.ParallelPixelComponent = MIUIni.GetByte(MIU_CONF.ParallelPixelComponent.ToString());
            InitValue.ParallelBitShift = MIUIni.GetByte(MIU_CONF.ParallelBitShift.ToString());
            MIU_MSG ret = (MIU_MSG)MIUController.MIUInitialize(deviceIndex, InitValue);
            if (ret != MIU_MSG.MIU_OK)
                return false;

            Thread ispThread = new Thread(new ParameterizedThreadStart(ISPDownload));
            ISPParam ispparam = new ISPParam();
            ispparam.FileName = IniFileName;
            ispparam.DeviceID = InitValue.IICDeviceID;
            ispThread.Start(ispparam);
            ispThread.Join();
            return true;
        }

        public void Play()
        {
            if (!Connected) return;
            MIUController.MIUInitializeImageValue(deviceIndex, PixelFormat, InitValue.nWidth, InitValue.nHeight,
                InitValue.nWidth, InitValue.nHeight, BUFFER_COUNT);

            MIUController.MIUStart(deviceIndex);

        }
        public void Stop()
        {
            int errCode = 0;
            for (int i = 0; i < 100; i++)
            {
                errCode = MIUController.MIUStop(0);
                if (errCode != 0)
                {
                    MIUController.MIUStop(0);
                    Thread.Sleep(1);
                }
                else
                    break;
            }
        }
        public IplImage GetImage()
        {
            Int64 timestamp = 0;
            int FrameSize = Image.ImageSize * 3;
            byte[] byteBuf = new byte[FrameSize];
            IntPtr Address = Marshal.AllocHGlobal(4);
            try
            {
                MIU_MSG errCode = (MIU_MSG)MIUController.MIUGetImageData(0, Address, ref timestamp);
                if (errCode == MIU_MSG.MIU_NEW_FRAME)
                {
                    unsafe
                    {
                        int* byteAddr = (int*)Marshal.ReadInt32(Address);
                        Marshal.Copy((IntPtr)byteAddr, byteBuf, 0, FrameSize);
                        if(MIU_1012bitPacked(PixelFormat))
                        {
                            ConvertColor(PixelFormat, ref byteBuf, ref ImageItp);
                        }
                        else
                        {
                            Image = IplImage.FromPixelData(ImageSize, 1, byteBuf);
                            ConvertColor(PixelFormat, ref Image, ref ImageItp);
                        }
                    }
                }
                Marshal.FreeHGlobal(Address);
            }
            catch(Exception ex)
            {
                Stop();
                Marshal.FreeHGlobal(Address);
                Debug.WriteLine(ex.ToString());
            }
            return ImageItp;
        }
        public void ChangeMono()
        {
            Thread thread = new Thread(new ThreadStart(delegate ()
            {
                MIUController.MIUPause(deviceIndex);
                Thread.Sleep(100);
                MIUController.MIUWriteRegister(deviceIndex, MIU_REG_TYPE.REG_MIU, 0x36, 2);
                PixelFormat = MIU_MONO8;
                MIUController.MIUResumeChageFormat(deviceIndex, PixelFormat);
                Thread.Sleep(300);
            }));
            thread.Start();
            thread.Join();
        }
        private enum I_MODE
        {
            NO_MODE,
            MIU_MODE,
            SENSOR_MODE,
        }
        private void ISPDownload(object param)
        {
            ISPParam ispparam = (ISPParam)param;
            I_MODE InitMode = I_MODE.NO_MODE;
            int BurstWriteLength = 0;
            byte[] BurstWriteData = new byte[2048];
            int bustSleep = 0;
            try
            {
                foreach(var l in File.ReadAllLines(ispparam.FileName))
                {
                    int data = 0;
                    int address = 0;
                    string line = l.ToUpper();
                    int pos = line.IndexOf("//");
                    if (pos >= 0) line = line.Remove(pos);
                    if (line.Length < 1) continue;
                    if (line.IndexOf("[MIU]") >= 0)
                    {
                        InitMode = I_MODE.MIU_MODE;
                        continue;
                    }
                    else if (line.IndexOf("[SENSOR]") >= 0)
                    {
                        InitMode = I_MODE.SENSOR_MODE;
                        continue;
                    }
                    if (InitMode == I_MODE.NO_MODE) continue;
                    if (line.IndexOf("SLEEP") >= 0)
                    {
                        int.TryParse(line.Substring(5), out data);
                        if (data == 0) continue;
                        if (BurstWriteLength == 0)
                        {
                            Thread.Sleep(data);
                            bustSleep = 0;
                        }
                        else bustSleep = data;
                        Debug.WriteLine("Sleep:{0}msec", data);
                    }
                    else if (line.IndexOf("RESET") >= 0)
                    {
                        int.TryParse(line.Substring(5), out data);
                        MIUController.MIUWriteRegister(deviceIndex, MIU_REG_TYPE.REG_MIU, 0x04, (ushort)data);
                    }
                    else if (line.IndexOf("ENABLE") >= 0)
                    {
                        int.TryParse(line.Substring(6), out data);
                        MIUController.MIUWriteRegister(deviceIndex, MIU_REG_TYPE.REG_MIU, 0x05, (ushort)data);
                    }
                    else if (line.IndexOf("GPIO0") >= 0)
                    {
                        int.TryParse(line.Substring(5), out data);
                        MIUSetGPIOStatus(deviceIndex, 0, (byte)data);
                    }
                    else if (line.IndexOf("GPIO1") >= 0)
                    {
                        int.TryParse(line.Substring(5), out data);
                        MIUSetGPIOStatus(deviceIndex, 1, (byte)data);
                    }
                    else if (line.IndexOf("GPIO2") >= 0)
                    {
                        int.TryParse(line.Substring(5), out data);
                        MIUSetGPIOStatus(deviceIndex, 2, (byte)data);
                    }
                    else if (line.IndexOf("GPIO3") >= 0)
                    {
                        int.TryParse(line.Substring(5), out data);
                        MIUSetGPIOStatus(deviceIndex, 3, (byte)data);
                    }
                    else if(line.IndexOf("GPO0") >= 0)
                    {
                        int.TryParse(line.Substring(4), out data);
                        MIUSetGPOStatus(deviceIndex, 0, (byte)data);
                    }
                    else if (line.IndexOf("GPO1") >= 0)
                    {
                        int.TryParse(line.Substring(4), out data);
                        MIUSetGPOStatus(deviceIndex, 1, (byte)data);
                    }
                    else if(line.IndexOf("GPIOMODE0") >= 0)
                    {
                        int.TryParse(line.Substring(9), out data);
                        MIUSetGPIOMode(deviceIndex, 0, (byte)data);
                    }
                    else if (line.IndexOf("GPIOMODE1") >= 0)
                    {
                        int.TryParse(line.Substring(9), out data);
                        MIUSetGPIOMode(deviceIndex, 1, (byte)data);
                    }
                    else if (line.IndexOf("GPIOMODE2") >= 0)
                    {
                        int.TryParse(line.Substring(9), out data);
                        MIUSetGPIOMode(deviceIndex, 2, (byte)data);
                    }
                    else if (line.IndexOf("GPIOMODE3") >= 0)
                    {
                        int.TryParse(line.Substring(9), out data);
                        MIUSetGPIOMode(deviceIndex, 3, (byte)data);
                    }
                    else if(line.IndexOf("0X") >= 0)
                    {
                        int errorcode = 0;
                        pos = line.IndexOf("0X");
                        int nextpos = line.LastIndexOf("0X");
                        if (pos == nextpos) continue;
                        int.TryParse(line.Substring(pos + 2, nextpos - 2), System.Globalization.NumberStyles.HexNumber, null,  out address);
                        int.TryParse(line.Substring(nextpos + 2), System.Globalization.NumberStyles.HexNumber, null, out data);
                        if(InitMode == I_MODE.MIU_MODE)
                        {
                            MIUWriteRegister(deviceIndex, MIU_REG_TYPE.REG_MIU, (ushort)address, (ushort)data);
                        }
                        else if(InitMode == I_MODE.SENSOR_MODE)
                        {
                            if(Burst == true && (address == 0x0f12 || address == 0x0f14))
                            {
                                if(BurstWriteLength == 0)
                                {
                                    BurstWriteData[0] = (byte)(address >> 8);
                                    BurstWriteData[1] = (byte)(address & 0xff);
                                    BurstWriteData[2] = (byte)(data >> 8);
                                    BurstWriteData[3] = (byte)(data & 0xff);
                                    BurstWriteLength = 4;
                                }
                                else
                                {
                                    BurstWriteData[BurstWriteLength] = (byte)(data >> 8);
                                    BurstWriteData[BurstWriteLength + 1] = (byte)(data & 0xff);
                                    BurstWriteLength += 2;
                                }
                                if(BurstWriteLength == 2048)
                                {
                                    errorcode = MIUController.IICWrite(deviceIndex, MIU_REG_TYPE.REG_IIC1,
                                        ispparam.DeviceID, 0x00010000, BurstWriteLength, ref BurstWriteData, false);

                                    if (errorcode != 0)
                                        Debug.WriteLine($"1ErrorCode : 0x{errorcode:X}");
                                    else
                                        Debug.WriteLine("Write Ok: {0}", BurstWriteLength);
                                    BurstWriteLength = 0;
                                }
                            }
                            else
                            {
                                if(BurstWriteLength > 0)
                                {
                                    if(BurstWriteLength == 4)
                                    {
                                        address = (BurstWriteData[0] << 8) + BurstWriteData[1];
                                        data = (BurstWriteData[2] << 8) + BurstWriteData[3];
                                        Debug.WriteLine($"Addr:0x{address:X}, Data:0x{data:X}");
                                        errorcode = MIUController.MIUWriteRegister(deviceIndex, MIU_REG_TYPE.REG_IIC1,
                                            (ushort)address, (ushort)data);
                                        if (errorcode != 0)
                                            Debug.WriteLine($"2ErrorCode : 0x{errorcode:X}");
                                    }
                                    else
                                    {
                                        errorcode = MIUController.IICWrite(deviceIndex, MIU_REG_TYPE.REG_IIC1,
                                            ispparam.DeviceID, 0x00010000, BurstWriteLength, ref BurstWriteData, false);
                                        if (errorcode != 0)
                                            Debug.WriteLine($"3ErrorCode : 0x{errorcode:X}");
                                        else
                                            Debug.WriteLine("Write Ok: {0}", BurstWriteLength);
                                    }
                                    if(bustSleep > 0)
                                    {
                                        Debug.WriteLine("bustSleep: {0}", bustSleep);
                                        Thread.Sleep(bustSleep);
                                        bustSleep = 0;
                                    }
                                    BurstWriteLength = 0;
                                }
                                Debug.WriteLine($"Addr:0x{address:X}, Data:0x{data:X}");
                                errorcode = MIUController.MIUWriteRegister(deviceIndex, MIU_REG_TYPE.REG_IIC1,
                                    (ushort)address, (ushort)data);
                                if (errorcode != 0)
                                    Debug.WriteLine($"4ErrorCode : 0x{errorcode:X}");
                            }
                        }
                    }
                    else if(line.IndexOf("I2C_MODE") >= 0)
                    {
                        int.TryParse(line.Substring(8), out data);
                        MIUController.MIUSetI2CMode(deviceIndex, (ushort)data);
                    }
                    else if(line.IndexOf("I2CID") >= 0)
                    {
                        int.TryParse(line.Substring(5), out data);
                        MIUController.MIUSetI2CID(deviceIndex, (byte)data);
                    }
                }
            }
            catch
            {
                
            }
        }
        
        public static void ConvertColor(uint PixelFormat, ref IplImage pSrc, ref IplImage pDest)
        {
            switch (PixelFormat)
            {
                case MIU_MONO8:
                    Cv.CvtColor(pSrc, pDest, ColorConversion.GrayToRgb);
                    break;
                case MIU_MONO10_PACKED:

                    break;
                case MIU_BAYERGR8:
                    Cv.CvtColor(pSrc, pDest, ColorConversion.BayerGrToRgb);
                    break;
                case MIU_BAYERGB8:
                    Cv.CvtColor(pSrc, pDest, ColorConversion.BayerGrToRgb);
                    break;
                case MIU_BAYERRG8:
                    Cv.CvtColor(pSrc, pDest, ColorConversion.BayerRgToRgb);
                    break;
                case MIU_BAYERBG8:
                    Cv.CvtColor(pSrc, pDest, ColorConversion.BayerBgToRgb);
                    break;
            }
        }
        public static void ConvertColor(uint PixelFormat, ref byte[] pixelData, ref IplImage pDest)
        {
            IplImage tempImg = new IplImage(pDest.Size, BitDepth.U8, 1);
            switch (PixelFormat)
            {
                case MIU_BAYERRG10_PACKED:
                    Shift10BitMode(ref pixelData, ref tempImg);
                    Cv.CvtColor(tempImg, pDest, ColorConversion.BayerRgToRgb);
                    break;
            }
            Cv.ReleaseImage(tempImg);
        }
        public static void Shift10BitMode(ref byte[] pixelData, ref IplImage pDest)
        {
            int cnt = 0;
            int extSize = pDest.ImageSize * 5 / 4;
            int[] intImg = new int[pDest.ImageSize / 4];
            int nByteWidth = pDest.Width * 5 / 4;
            int nWidth = pDest.Width >> 2;
            for (int i = 0; i < pDest.Height; i++)
            {
                int k = 0;
                for (int j = 0; j < nWidth; j++)
                {
                    int step = nByteWidth * i + k;
                    int Value = (pixelData[step + 3] << 24);
                    Value += (pixelData[step + 2] << 16);
                    Value += (pixelData[step + 1] << 8);
                    Value += (pixelData[step]);
                    intImg[j + nWidth * i] = Value;
                    cnt++;
                    k += 5;
                }
            }
            Marshal.Copy(intImg, 0, pDest.ImageData, pDest.ImageSize / 4);
        }
    }
}
