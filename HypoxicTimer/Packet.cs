using System;
using System.Collections.Generic;
using HidLibrary;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.IO.Ports;
using System.Drawing;
using System.Media;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Windows.Forms;

namespace HypoxicTimer
{
        [Serializable]
        public class Packet
        {
            static SerialPort aSerialPort = null;
            static HidDevice aHidDevice = null;
            static DateTime lastPingTime = DateTime.MinValue;
            enum SupportedOximter { CMS50E, CMS60C, CMS50E_HID };
            static SupportedOximter OximeterType;
            static byte[] cms60cHandshake = { 0x7D, 0x81, 0xA1 };
            static public Packet ReadPacket(string ComPort, string OximeterTypeString)
            {
                if (OximeterTypeString == "CMS 60C")
                    OximeterType = SupportedOximter.CMS60C;
                else if (OximeterTypeString == "CMS 50E (HID)")
                    OximeterType = SupportedOximter.CMS50E_HID;
                else
                    OximeterType = SupportedOximter.CMS50E;

                if (OximeterType == SupportedOximter.CMS50E_HID)
                {
                    if (aHidDevice == null)
                    {
                        InitializeCms50EHid();
                    }
                }
                else if (aSerialPort == null)
                {
                    int baudrate = OximeterType == SupportedOximter.CMS60C ? 115200 : 19200;
                    aSerialPort = new SerialPort(ComPort, baudrate);
                    aSerialPort.ReadTimeout = 100;
                    try
                    {
                        Logger.Log("Open Port for " + OximeterType, 1);
                        aSerialPort.Open();
                        System.GC.SuppressFinalize(aSerialPort.BaseStream);
                    }
                    catch (UnauthorizedAccessException e)
                    {
                        MessageBox.Show("Can't open the Oximeter - is another application using it?");
                        aSerialPort = null;
                        throw e;
                    }
                    catch (IOException e)
                    {
                        MessageBox.Show("Can't open the Oximeter - is it plugged in?\r\n" + e);
                        aSerialPort = null;
                        throw e;
                    }
                    if (OximeterType == SupportedOximter.CMS60C)
                    {
                        Write8Bytes(cms60cHandshake);
                    }
                }


                RealTimePacket aPacket = null;
                for (int i = 0; i < 10 && aPacket == null; i++)
                {
                    if (OximeterType == SupportedOximter.CMS60C)
                    {
                        aPacket = GetPacketFromCms60C();
                    }
                    else if (OximeterType == SupportedOximter.CMS50E_HID)
                    {
                        aPacket = GetPacketFromCms50EHid();
                    }
                    else
                    {
                        aPacket = GetPacketFromCms50E();
                    }
                }

                if (aPacket == null)
                {
                    Logger.Log("Failure", 2);
                }
                return aPacket;
            }

            private static RealTimePacket GetPacketFromCms50E()
            {
                int[] buffer = null;
                int? headN = ReadByte();
                if (headN == null)
                {
                    Logger.Log("Got Null", 2);
                }

                int head = (int)headN;
                if (((head & 0x80) != 0x80))
                {
                    Logger.Log("Header byte no good " + (head & 0xb0).ToString("x") + " & 0x80 " + ((head & 0xf0)).ToString("x"), 2);
                    return null;
                }
                buffer = ReadBytes(4);
                if (buffer == null)
                {
                    Logger.Log("Could not read 4 byte packet", 2);
                    //no read
                    return null;
                }
                for (int j = 0; j < 4; j++)
                {
                    if ((buffer[j] & 0x80) == 0x80)
                    {
                        return null;
                    }
                }
                RealTimePacket aPacket = new RealTimePacket();
                aPacket.TimeStamp = DateTime.Now;
                aPacket.Pulse1 = buffer[0];
                aPacket.Pulse2 = buffer[1];
                aPacket.Pulse = (aPacket.Pulse2 << 8) | aPacket.Pulse1;
                aPacket.HeartRate = buffer[2];
                aPacket.SpO2 = buffer[3];
                //aPacket.isBeat = (head & 0x40) == 0x40;
                aPacket.isValid = true; //assume for now
                return aPacket;
            }

            private static RealTimePacket GetPacketFromCms60C()
            {
                int[] buffer = ReadBytes(9);
                if (buffer == null)
                {
                    if (OximeterType == SupportedOximter.CMS60C)
                    {
                        Write8Bytes(cms60cHandshake);
                    }
                    Logger.Log("Could not read 9 byte packet", 5);
                    return null;
                }

                if(buffer[0] != 0x01)
                {
                    Logger.Log("Header byte no good " + buffer[0].ToString("x2"), 5);
                    return null;
                }

                if (buffer[1] != 0xe0)
                {
                    Logger.Log("Finger Out " + buffer[1].ToString("x2"), 5);
                    return null;
                }


                RealTimePacket aPacket = new RealTimePacket();
                aPacket.TimeStamp = DateTime.Now;
                aPacket.Pulse1 = buffer[3];
                aPacket.Pulse2 = buffer[4];
                aPacket.Pulse = ((aPacket.Pulse2 & 0x0f) << 8) + aPacket.Pulse1;
                aPacket.HeartRate = buffer[5] & 0x7f;
                aPacket.SpO2 = buffer[6] & 0x7f;
                aPacket.isBeat = (buffer[2] & 0x40) == 0x40;
                aPacket.isValid = true; //assume for now
                return aPacket;
            }

            static public int? ReadByte()
            {
                try
                {
                    int result = aSerialPort.ReadByte();
                    Logger.Log("Read " + result.ToString("x2") + " " + result.ToString("d3") + " " + Convert.ToString(result, 2).PadLeft(8, '0'), 6);
                    return result;
                }
                catch (Exception e)
                {
                    Logger.Log("Exception reading " + e, 1);
                    return null;
                }
            }

            static private int[] ReadBytes(int size)
            {
                int[] result = new int[size];
                for (int i = 0; i < size; i++)
                {
                    int? abyte = ReadByte();
                    if (abyte == null)
                        return null;
                    result[i] = (int)abyte;
                }
                return result;
            }

            private static void Write8Bytes(byte[] buffer)
            {
                byte[] eightbytes = new byte[8];
                int i = 0;
                for (; i < buffer.Length; i++)
                {
                    eightbytes[i] = buffer[i];
                }
                for (; i < 8; i++)
                {
                    eightbytes[i] = 0x80;
                }
                try
                {
                    aSerialPort.Write(eightbytes, 0, eightbytes.Length);
                }
                catch (Exception e)
                {
                    Logger.Log("Exception writing " + e, 1);
                }

            }


            static public void Debug()
            {
                for (int j = 0; j < 30; j++)
                {
                    int? head = Packet.ReadByte();
                    if (head == null)
                        Logger.Log("Got [" + j.ToString("d2") + "] NULL ", 4);
                    else
                        Logger.Log("Got [" + j.ToString("d2") + "] " + Utils.ToString((int)head), 4);
                }
                Logger.Log("Done", 4);
            }

            static int lastPulse = 0;

            private static void InitializeCms50EHid()
            {
                Logger.Log("Initializing CMS50E (HID)...", 1);
                
                try
                {
                    foreach (var device in HidDevices.Enumerate(0x28E9, 0x028A))
                    {
                        aHidDevice = device;
                        break;
                    }
                    if (aHidDevice == null)
                    {
                        foreach (var device in HidDevices.Enumerate(0x11CA, 0x0224))
                        {
                            aHidDevice = device;
                            break;
                        }
                    }

                    if (aHidDevice == null)
                    {
                        MessageBox.Show("CMS50E HID device not found. Please ensure it is plugged in.");
                        throw new IOException("CMS50E HID device not found.");
                    }
                    
                    aHidDevice.OpenDevice();
                    if (!aHidDevice.IsOpen)
                    {
                        MessageBox.Show("Could not open CMS50E HID device.");
                        aHidDevice = null;
                        throw new IOException("Could not open CMS50E HID device.");
                    }

                    // Stop sending data
                    WriteHidCommand(new byte[] {
                        0x7d, 0x81, 0xa7, 0x80, 0x80, 0x80, 0x80, 0x80,
                        0x80, 0x7d, 0x81, 0xa2, 0x80, 0x80, 0x80, 0x80,
                        0x80, 0x80
                    });
                    ReadHidResponse(0xf0);

                    // Unknown 0
                    WriteHidCommand(new byte[] { 0x82, 0x02 });
                    ReadHidResponse(0xf2);

                    // Unknown 1
                    WriteHidCommand(new byte[] { 0x80, 0x00 });
                    ReadHidResponse(0xf0);

                    // Time synchronization
                    var now = DateTime.Now;
                    var year = (byte)(now.Year % 100);
                    var month = (byte)now.Month;
                    var day = (byte)now.Day;
                    var hour = (byte)now.Hour;
                    var minute = (byte)now.Minute;
                    var second = (byte)now.Second;
                    var timeSyncData = new byte[] {
                        0x83, year, month, day, hour, minute, second, 0x00, 0x00
                    };
                    WriteHidCommand(timeSyncData, true);
                    ReadHidResponse(0xf3);

                    // User name
                    WriteHidCommand(new byte[] { 0x8e, 0x03, 0x11 });
                    ReadHidResponse(0xfe);

                    // Model name
                    WriteHidCommand(new byte[] { 0x81, 0x01 });
                    ReadHidResponse(0xf1);

                    // Start live stream
                    WriteHidCommand(new byte[] { 0x9b, 0x00, 0x1b });
                    WriteHidCommand(new byte[] { 0x9b, 0x01, 0x1c });
                    
                    var report = aHidDevice.ReadReport(1000);
                    if (report.ReadStatus != HidDeviceData.ReadStatus.Success || report.Data[0] != 0xeb)
                    {
                        Logger.Log("Failed to start live data stream. Expected 0xeb header.", 1);
                    }
                    
                    lastPingTime = DateTime.Now;
                }
                catch (Exception e)
                {
                    aHidDevice = null;
                    if (!(e is IOException && e.Message.Contains("not found")))
                    {
                        MessageBox.Show("Exception initializing CMS50E HID device: " + e.Message);
                    }
                    throw;
                }
            }

            private static void SetChecksum(byte[] data, int end)
            {
                byte sum = 0;
                for (int i = 0; i < end; i++)
                {
                    sum += data[i];
                }
                data[end] = (byte)(sum & 0x7F);
            }

            private static void WriteHidCommand(byte[] cmdData, bool setChecksum = false)
            {
                byte[] reportData = new byte[65];
                reportData[0] = 0x00;
                
                for (int i = 0; i < cmdData.Length; i++)
                {
                    reportData[i + 1] = cmdData[i];
                }
                for (int i = cmdData.Length; i < 64; i++)
                {
                    reportData[i + 1] = 0x00;
                }
                
                if (setChecksum)
                {
                    SetChecksum(reportData, 10);
                }
                
                aHidDevice.Write(reportData);
            }

            private static void ReadHidResponse(byte expectedHeader)
            {
                for (int i = 0; i < 5; i++)
                {
                    var report = aHidDevice.ReadReport(1000);
                    if (report.ReadStatus == HidDeviceData.ReadStatus.Success)
                    {
                        if (report.Data[0] == expectedHeader)
                        {
                            return; // Expected response received
                        }
                        // If it's old live data (0xeb) or other artifacts, keep trying
                    }
                    else
                    {
                        throw new IOException("Failed to read response from CMS50E HID device. ReadStatus: " + report.ReadStatus);
                    }
                }
                throw new IOException(string.Format("Unexpected response header: Did not receive Expected 0x{0:X2} after 5 attempts", expectedHeader));
            }

            private static void SendHidPing()
            {
                WriteHidCommand(new byte[] { 0x9a, 0x1a });
                lastPingTime = DateTime.Now;
            }

            private static Queue<RealTimePacket> packetQueue = new Queue<RealTimePacket>();
            private static int lastSpO2 = 0;
            private static int lastHeartRate = 0;

            private static RealTimePacket GetPacketFromCms50EHid()
            {
                if ((DateTime.Now - lastPingTime).TotalSeconds >= 4.0)
                {
                    SendHidPing();
                }

                if (packetQueue.Count > 0)
                {
                    return packetQueue.Dequeue();
                }

                var report = aHidDevice.ReadReport(500);
                if (report.ReadStatus != HidDeviceData.ReadStatus.Success || report.Data == null || report.Data.Length == 0)
                {
                    if (report.ReadStatus == HidDeviceData.ReadStatus.WaitFail || report.ReadStatus == HidDeviceData.ReadStatus.NotConnected || !aHidDevice.IsOpen)
                    {
                        aHidDevice = null;
                        throw new IOException("CMS50E HID device disconnected.");
                    }
                    return null;
                }
                
                int index = 0;
                while (index < report.Data.Length && report.Data[index] == 0xeb)
                {
                    byte type = report.Data[index + 1];
                    if (type == 0x00)
                    {
                        int pulseWave = report.Data[index + 3];
                        
                        RealTimePacket aPacket = new RealTimePacket();
                        aPacket.TimeStamp = DateTime.Now;
                        aPacket.HeartRate = lastHeartRate;
                        aPacket.SpO2 = lastSpO2;
                        aPacket.Pulse1 = pulseWave;
                        aPacket.Pulse = pulseWave;
                        aPacket.isValid = (lastSpO2 > 0 && lastSpO2 <= 100);
                        
                        packetQueue.Enqueue(aPacket);
                        
                        index += 5;
                    }
                    else if (type == 0x01)
                    {
                        byte prFlag = report.Data[index + 2];
                        byte prValue = report.Data[index + 3];
                        byte spo2Value = report.Data[index + 4];
                        
                        if (spo2Value != 0x7f && prValue != 0xff)
                        {
                            lastHeartRate = (prFlag & 0x02) != 0 ? prValue + 128 : prValue;
                            lastSpO2 = spo2Value;
                        }
                        index += 7;
                    }
                    else
                    {
                        break;
                    }
                }
                
                if (packetQueue.Count > 0)
                {
                    return packetQueue.Dequeue();
                }
                
                return null;
            }
        }
        [Serializable]
        public class RealTimePacket : Packet
        {
            public DateTime TimeStamp;
            public int Pulse1;
            public int Pulse2;
            public int Pulse;
            public int HeartRate;
            public int SpO2;
            public bool isBeat;
            public bool isValid;
            public override String ToString()
            {
                return "DateTime " + TimeStamp + ", Pulse1 " + Pulse1 + ", Pulse2 " + Pulse2 + ", Pulse " + Pulse + ", HeartRate " + HeartRate + ", SpO2 " + SpO2 + ", isBeat " + isBeat + ", isValid " + isValid;
            }
        }
        [Serializable]
        public class RecordedTimePacket : Packet
        {
            public int Hours;
            public int Minutes;
            public int Seconds;
        }
        [Serializable]
        public class RecordedDataPacket : Packet
        {
            public int HeartRate;
            public int SpO2;
            public bool isValid;
            public int header;
        }
}
