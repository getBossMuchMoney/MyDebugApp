using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;


unsafe public struct VCI_BOARD_INFO//使用不安全代码
{
    public UInt16 hw_Version;
    public UInt16 fw_Version;
    public UInt16 dr_Version;
    public UInt16 in_Version;
    public UInt16 irq_Num;
    public byte can_Num;

    public fixed byte str_Serial_Num[20];
    public fixed byte str_hw_Type[40];
    public fixed byte Reserved[8];
}

/////////////////////////////////////////////////////
//2.定义CAN信息帧的数据类型。
unsafe public struct VCI_CAN_OBJ  //使用不安全代码
{
    public uint ID;
    public uint TimeStamp;        //时间标识
    public byte TimeFlag;         //是否使用时间标识
    public byte SendType;         //发送标志。保留，未用
    public byte RemoteFlag;       //是否是远程帧
    public byte ExternFlag;       //是否是扩展帧
    public byte DataLen;          //数据长度
    public fixed byte Data[8];    //数据
    public fixed byte Reserved[3];//保留位

}

//3.定义初始化CAN的数据类型
public struct VCI_INIT_CONFIG
{
    public UInt32 AccCode;
    public UInt32 AccMask;
    public UInt32 Reserved;
    public byte Filter;   //0或1接收所有帧。2标准帧滤波，3是扩展帧滤波。
    public byte Timing0;  //波特率参数，具体配置，请查看二次开发库函数说明书。
    public byte Timing1;
    public byte Mode;     //模式，0表示正常模式，1表示只听模式,2自测模式
}

/*------------其他数据结构描述---------------------------------*/
//4.USB-CAN总线适配器板卡信息的数据类型1，该类型为VCI_FindUsbDevice函数的返回参数。
public struct VCI_BOARD_INFO1
{
    public UInt16 hw_Version;
    public UInt16 fw_Version;
    public UInt16 dr_Version;
    public UInt16 in_Version;
    public UInt16 irq_Num;
    public byte can_Num;
    public byte Reserved;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)] public byte[] str_Serial_Num;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
    public byte[] str_hw_Type;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
    public byte[] str_Usb_Serial;
}

public struct MOD_STA
{
    public ushort u16_Vab;
    public ushort u16_Vbc;
    public ushort u16_Vca;
    public ushort u16_Ia;
    public ushort u16_Ib;
    public ushort u16_Ic;
    public ushort u16_Iai1;
    public ushort u16_Ibi1;
    public ushort u16_Ici1;
    public ushort u16_Iai2;
    public ushort u16_Ibi2;
    public ushort u16_Ici2;
    public ushort u16_Vbus;
    public ushort u16_Idabpri1;
    public ushort u16_Idabpri2;
    public ushort u16_Idabout1;
    public ushort u16_Idabout2;
    public ushort u16_Freq;
    public ushort u16_Idcout;
    public ushort u16_Vout;
    public ushort u16_Pin;
    public ushort u16_Pinact;
    public ushort u16_Pout;
    public short i16_Temp1;
    public short i16_Temp2;
    public short i16_Temp3;
    public short i16_Temp4;
    public ushort u16_Addr;
    public ushort u16_WorkMode;
    public ushort u16_State;

    public uint u32_ErrCode;
    public uint u32_Version;
    public uint u32_Resv;

}

[StructLayout(LayoutKind.Explicit)]
unsafe public struct U_MOD_STA
{
    [FieldOffset(0)]
    public MOD_STA REG;           // 结构体部分

    [FieldOffset(0)]
    public fixed ushort buff[36]; // 固定大小的 uint16_t 数组
}


[StructLayout(LayoutKind.Explicit)]
public struct CMD_WD
{
    [FieldOffset(0)]
    public ushort all;

    [FieldOffset(0)]
    private BitFields BIT;

    public ushort PowerOnOff
    {
        get => (byte)(all & 0x01U);
        set => all = (ushort)((all & ~0x01U) | (value & 0x01U));
    }

    public ushort Pfc_ClearErr
    {
        get => (byte)((all >> 1) & 0x01U);
        set => all = (ushort)((all & ~(0x01U << 1)) | ((value & 0x01U) << 1));
    }

    public ushort ClearErr
    {
        get => (byte)((all >> 2) & 0x01U);
        set => all = (ushort)((all & ~(0x01U << 2)) | ((value & 0x01U) << 2));
    }

    public ushort Boot
    {
        get => (byte)((all >> 3) & 0x01U);
        set => all = (ushort)((all & ~(0x01U << 3)) | ((value & 0x01U) << 3));
    }

    public ushort PfcOL
    {
        get => (byte)((all >> 4) & 0x01U);
        set => all = (ushort)((all & ~(0x01U << 4)) | ((value & 0x01U) << 4));
    }

    public ushort DcOL
    {
        get => (byte)((all >> 5) & 0x01U);
        set => all = (ushort)((all & ~(0x01U << 5)) | ((value & 0x01U) << 5));
    }

    public ushort DataSave
    {
        get => (byte)((all >> 6) & 0x01U);
        set => all = (ushort)((all & ~(0x01U << 6)) | ((value & 0x01U) << 6));
    }

    // 辅助结构体（不实际使用，仅用于占位）
    [StructLayout(LayoutKind.Sequential)]
    private struct BitFields { }
}

public struct MOD_SET
{
    public Int32 i32_MaxIout;
    public short i16_MaxVout;
    public short i16_MaxPout;
    public ushort u16_OutputMode;
    public ushort u16_SlopeIcc;
    public ushort u16_SlopeVcv;
    public ushort u16_SlopePcp;
    public CMD_WD u16_CmdWd;
    public ushort u16_ModuleEn;
    public ushort u16_EnCalType;
    public ushort u16_RunCal;
    public ushort u16_RefVbus;
    public ushort u16_OverloadCoeffi;
    public ushort u16_OverloadTime;
    public ushort i16_DabShPha;
    public ushort i16_DabPriInShPha;
    public ushort i16_DabSecInShPha;
    public int i32_RefValCCal1;
    public int i32_ActValCCal1;
    public int i32_RefValCCal2;
    public int i32_ActValCCal2;
    public int i32_RefValCCal3;
    public int i32_ActValCCal3;
    public int i32_RefValCCal4;
    public int i32_ActValCCal4;
    public int i32_RefValCCal5;
    public int i32_ActValCCal5;
    public int i32_RefValCCal6;
    public int i32_ActValCCal6;
    public int i32_RefValCCal7;
    public int i32_ActValCCal7;
    public int i32_RefValCCal8;
    public int i32_ActValCCal8;

    public int i32_RefValUCal1;
    public int i32_ActValUCal1;
    public int i32_RefValUCal2;
    public int i32_ActValUCal2;
    public int i32_RefValUCal3;
    public int i32_ActValUCal3;
    public int i32_RefValUCal4;
    public int i32_ActValUCal4;
    public int i32_RefValUCal5;
    public int i32_ActValUCal5;
    public int i32_RefValUCal6;
    public int i32_ActValUCal6;
    public int i32_RefValUCal7;
    public int i32_ActValUCal7;
    public int i32_RefValUCal8;
    public int i32_ActValUCal8;


    public int i32_CtrlK1;
    public int i32_CtrlK2;
    public int i32_CtrlK3;
    public int i32_CtrlK4;
    public int i32_CtrlK5;
    public int i32_CtrlK6;
    public int i32_CtrlK7;
    public int i32_CtrlK8;
    public int i32_CtrlK9;
    public int i32_CtrlK10;
    public int i32_CtrlK11;
    public int i32_CtrlK12;
    public int i32_CtrlK13;
    public int i32_CtrlK14;
    public int i32_CtrlK15;
    public int i32_CtrlK16;
    public int i32_CtrlK17;
    public int i32_CtrlK18;
    public int i32_CtrlK19;
    public int i32_CtrlK20;

}

[StructLayout(LayoutKind.Explicit)]
unsafe public struct U_MOD_SET
{
    [FieldOffset(0)]
    public MOD_SET REG;           // 结构体部分

    [FieldOffset(0)]
    public fixed ushort buff[122]; // 固定大小的 uint16_t 数组
}



[StructLayout(LayoutKind.Explicit)]
public struct CANID_UNION
{
    [FieldOffset(0)]
    public uint IdFrame;

    [FieldOffset(0)]
    private BitFields BIT;

    public byte AckSts
    {
        get => (byte)(IdFrame & 0x03U);
        set => IdFrame = (IdFrame & ~0x03U) | ((uint)value & 0x03U);
    }

    public byte FuncCode
    {
        get => (byte)((IdFrame >> 2) & 0x7FU);
        set => IdFrame = (IdFrame & ~(0x7FU << 2)) | (((uint)value & 0x7FU) << 2);
    }

    public byte DesId
    {
        get => (byte)((IdFrame >> 9) & 0x3FU);
        set => IdFrame = (IdFrame & ~(0x3FU << 9)) | (((uint)value & 0x3FU) << 9);
    }

    public byte SrcId
    {
        get => (byte)((IdFrame >> 15) & 0x3FU);
        set => IdFrame = (IdFrame & ~(0x3FU << 15)) | (((uint)value & 0x3FU) << 15);
    }

    public byte Index
    {
        get => (byte)((IdFrame >> 21) & 0xFFU);
        set => IdFrame = (IdFrame & ~(0xFFU << 21)) | (((uint)value & 0xFFU) << 21);
    }

    //public byte Resv
    //{
    //    get => (byte)((IdFrame >> 31) & 0x07U);
    //    set => IdFrame = (IdFrame & ~(0x07U << 31)) | (((uint)value & 0x07U) << 31);
    //}

    // 辅助结构体（不实际使用，仅用于占位）
    [StructLayout(LayoutKind.Sequential)]
    private struct BitFields { }
}



[StructLayout(LayoutKind.Explicit)]
public struct CanAppId
{
    [FieldOffset(0)]
    public uint IdFrame;

    [FieldOffset(0)]
    private BitFields BIT;

    public byte AckSts
    {
        get => (byte)((IdFrame >> 4) & 0x07U);
        set => IdFrame = (IdFrame & ~(0x07U << 4)) | (((uint)value & 0x07U) << 4);
    }

    public byte ActFlag
    {
        get => (byte)((IdFrame >> 7) & 0x01U);
        set => IdFrame = (IdFrame & ~(0x01U << 7)) | (((uint)value & 0x01U) << 7);
    }

    public byte FuncCode
    {
        get => (byte)((IdFrame >> 8) & 0xFFU);
        set => IdFrame = (IdFrame & ~(0xFFU << 8)) | (((uint)value & 0xFFU) << 8);
    }

    public byte DesId
    {
        get => (byte)((IdFrame >> 16) & 0x3FU);
        set => IdFrame = (IdFrame & ~(0x3FU << 16)) | (((uint)value & 0x3FU) << 16);
    }

    public byte SlaverFlag
    {
        get => (byte)((IdFrame >> 22) & 0x01U);
        set => IdFrame = (IdFrame & ~(0x01U << 22)) | (((uint)value & 0x01U) << 22);
    }

    public byte SrcId
    {
        get => (byte)((IdFrame >> 23) & 0x3FU);
        set => IdFrame = (IdFrame & ~(0x3FU << 23)) | (((uint)value & 0x3FU) << 23);
    }


    //public byte Resv
    //{
    //    get => (byte)((IdFrame >> 31) & 0x07U);
    //    set => IdFrame = (IdFrame & ~(0x07U << 31)) | (((uint)value & 0x07U) << 31);
    //}

    // 辅助结构体（不实际使用，仅用于占位）
    [StructLayout(LayoutKind.Sequential)]
    private struct BitFields { }
}


namespace MyDebugApp
{
    internal class GloabalCfg
    {
        
    }
}
