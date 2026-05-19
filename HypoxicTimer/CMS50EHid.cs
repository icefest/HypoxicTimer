using System;
using System.Linq;
using System.Threading;
using System.Diagnostics;
using HidLibrary;

namespace HypoxicTimer
{
    public class CMS50EHid : Oximeter
    {
        private HidDevice _device;
        private Thread _readThread;
        private Thread _keepAliveThread;
        private bool _isRunning;

        // pulseoxdl standard keep-alive / init sequence for CMS50E
        private readonly byte[] _keepAliveCommand = new byte[] { 0x7D, 0x81, 0xA1, 0x80, 0x80, 0x80, 0x80, 0x80, 0x80 };

        public CMS50EHid()
        {
            // Typical VID/PID for Contec/CMS50E HID devices
            // Adjust if your specific device enumerates differently
            var devices = HidDevices.Enumerate(0x11CA, 0x0224).ToList(); 
            _device = devices.FirstOrDefault();

            if (_device == null)
            {
                Logger.Log("CMS50E HID not found on USB.");
            }
        }

        public override void Start()
        {
            if (_device == null) return;

            _device.OpenDevice();
            _isRunning = true;

            // Start Read Thread
            _readThread = new Thread(ReadLoop);
            _readThread.IsBackground = true;
            _readThread.Start();

            // Start pulseoxdl Keep-Alive Thread
            _keepAliveThread = new Thread(KeepAliveLoop);
            _keepAliveThread.IsBackground = true;
            _keepAliveThread.Start();
        }

        public override void Stop()
        {
            _isRunning = false;

            if (_device != null && _device.IsOpen)
            {
                _device.CloseDevice();
            }
        }

        private void KeepAliveLoop()
        {
            while (_isRunning)
            {
                if (_device != null && _device.IsOpen)
                {
                    // Send initialization/keep-alive packet
                    // hidlibrary requires a report ID as the first byte (usually 0)
                    byte[] reportData = new byte[_keepAliveCommand.Length + 1];
                    reportData[0] = 0x00; 
                    Buffer.BlockCopy(_keepAliveCommand, 0, reportData, 1, _keepAliveCommand.Length);
                    
                    _device.Write(reportData);
                }
                
                // pulseoxdl standard keep-alive interval
                Thread.Sleep(2000); 
            }
        }

        private void ReadLoop()
        {
            byte[] buffer = new byte[7]; // 7 byte frames for live data
            int byteIndex = 0;

            while (_isRunning)
            {
                var report = _device.ReadReport(500); // 500ms timeout
                if (report.ReadStatus == HidDeviceData.ReadStatus.Success)
                {
                    foreach (byte b in report.Data)
                    {
                        // Sync bit check (CMS50E frame sync has highest bit set: > 127)
                        if ((b & 0x80) != 0) 
                        {
                            byteIndex = 0;
                        }

                        buffer[byteIndex] = b;
                        byteIndex++;

                        if (byteIndex == buffer.Length)
                        {
                            ProcessFrame(buffer);
                            byteIndex = 0; // Reset for next frame
                        }
                    }
                }
            }
        }

        private void ProcessFrame(byte[] frame)
        {
            // Frame format based on pulseoxdl byte unpacking
            // Byte 0: Sync/Status
            // Byte 1: SpO2
            // Byte 2: Pulse Rate
            
            if (frame.Length >= 3)
            {
                int spo2 = frame[1] & 0x7F;
                int pulse = frame[2] & 0x7F;

                // Fire the base Oximeter event
                if (spo2 > 0 && spo2 <= 100 && pulse > 0)
                {
                    OnDataReceived(spo2, pulse);
                }
            }
        }
    }
}