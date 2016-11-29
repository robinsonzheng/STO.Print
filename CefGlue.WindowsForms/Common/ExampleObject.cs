﻿using System;
using System.Windows.Forms;

namespace Xilium.CefGlue.Client
{
    public class ExampleObject
    {
        #region 属性

        public string RepeatTow
        {
            get
            {
                return Repeat("hi ", 2);
            }
        }


        #endregion 属性

        #region 方法
        /// <summary>
        /// 重复叠加字符串
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="n">次数</param>
        /// <returns></returns>
        public string Repeat(string str, int n)
        {
            string result = String.Empty;
            for (int i = 0; i < n; i++)
            {
                result += str;
            }
            return result;
        }
        /// <summary>
        /// 无返回值
        /// </summary>
        public void EchoVoid()
        {
            MessageBox.Show("BindingTestAv8Handler : EchoVoid()");
        }


        /// <summary>
        /// 返回逻辑型
        /// </summary>
        /// <param name="arg0">参数</param>
        /// <returns></returns>
        public Boolean EchoBoolean(Boolean arg0)
        {
            return arg0;
        }
        /// <summary>
        /// 返回可空逻辑型
        /// </summary>
        /// <param name="arg0">参数</param>
        /// <returns></returns>
        public Boolean? EchoNullableBoolean(Boolean? arg0)
        {
            return arg0;
        }
        /// <summary>
        /// 返回 8 位有符号整数
        /// </summary>
        /// <param name="arg0">参数</param>
        /// <returns></returns>
        public SByte EchoSByte(SByte arg0)
        {
            return arg0;
        }
        /// <summary>
        /// 返回可空 8 位有符号整数
        /// </summary>
        /// <param name="arg0">参数</param>
        /// <returns></returns>
        public SByte? EchoNullableSByte(SByte? arg0)
        {
            return arg0;
        }
        /// <summary>
        /// 返回 16 位有符号整数
        /// </summary>
        /// <param name="arg0">参数</param>
        /// <returns></returns>
        public Int16 EchoInt16(Int16 arg0)
        {
            return arg0;
        }
        /// <summary>
        /// 返回可空 16 位有符号整数
        /// </summary>
        /// <param name="arg0">参数</param>
        /// <returns></returns>
        public Int16? EchoNullableInt16(Int16? arg0)
        {
            return arg0;
        }
        /// <summary>
        /// 返回 32 位有符号整数
        /// </summary>
        /// <param name="arg0">参数</param>
        /// <returns></returns>
        public Int32 EchoInt32(Int32 arg0)
        {
            return arg0;
        }
        /// <summary>
        /// 返回可空 32 位有符号整数
        /// </summary>
        /// <param name="arg0">参数</param>
        /// <returns></returns>
        public Int32? EchoNullableInt32(Int32? arg0)
        {
            return arg0;
        }
        /// <summary>
        /// 返回 64 位有符号整数
        /// </summary>
        /// <param name="arg0">参数</param>
        /// <returns></returns>
        public Int64 EchoInt64(Int64 arg0)
        {
            return arg0;
        }
        /// <summary>
        /// 返回可空 64 位有符号整数
        /// </summary>
        /// <param name="arg0">参数</param>
        /// <returns></returns>
        public Int64? EchoNullableInt64(Int64? arg0)
        {
            return arg0;
        }
        /// <summary>
        /// 返回 8 位无符号整数
        /// </summary>
        /// <param name="arg0">参数</param>
        /// <returns></returns>
        public Byte EchoByte(Byte arg0)
        {
            return arg0;
        }
        /// <summary>
        /// 返回可空 8 位无符号整数
        /// </summary>
        /// <param name="arg0">参数</param>
        /// <returns></returns>
        public Byte? EchoNullableByte(Byte? arg0)
        {
            return arg0;
        }
        /// <summary>
        /// 返回 16 位无符号整数
        /// </summary>
        /// <param name="arg0">参数</param>
        /// <returns></returns>
        public UInt16 EchoUInt16(UInt16 arg0)
        {
            return arg0;
        }
        /// <summary>
        /// 返回可空 16 位无符号整数
        /// </summary>
        /// <param name="arg0">参数</param>
        /// <returns></returns>
        public UInt16? EchoNullableUInt16(UInt16? arg0)
        {
            return arg0;
        }
        /// <summary>
        /// 返回 32 位无符号整数
        /// </summary>
        /// <param name="arg0">参数</param>
        /// <returns></returns>
        public UInt32 EchoUInt32(UInt32 arg0)
        {
            return arg0;
        }
        /// <summary>
        /// 返回可空 32 位无符号整数
        /// </summary>
        /// <param name="arg0">参数</param>
        /// <returns></returns>
        public UInt32? EchoNullableUInt32(UInt32? arg0)
        {
            return arg0;
        }
        /// <summary>
        /// 返回 64 位无符号整数
        /// </summary>
        /// <param name="arg0">参数</param>
        /// <returns></returns>
        public UInt64 EchoUInt64(UInt64 arg0)
        {
            return arg0;
        }
        /// <summary>
        /// 返回可空 64 位无符号整数
        /// </summary>
        /// <param name="arg0">参数</param>
        /// <returns></returns>
        public UInt64? EchoNullableUInt64(UInt64? arg0)
        {
            return arg0;
        }
        /// <summary>
        /// 返回单精度浮点数
        /// </summary>
        /// <param name="arg0">参数</param>
        /// <returns></returns>
        public Single EchoSingle(Single arg0)
        {
            return arg0;
        }
        /// <summary>
        /// 返回可空单精度浮点数
        /// </summary>
        /// <param name="arg0">参数</param>
        /// <returns></returns>
        public Single? EchoNullableSingle(Single? arg0)
        {
            return arg0;
        }
        /// <summary>
        /// 返回双精度浮点数
        /// </summary>
        /// <param name="arg0">参数</param>
        /// <returns></returns>
        public Double EchoDouble(Double arg0)
        {
            return arg0;
        }
        /// <summary>
        /// 返回可空双精度浮点数
        /// </summary>
        /// <param name="arg0">参数</param>
        /// <returns></returns>
        public Double? EchoNullableDouble(Double? arg0)
        {
            return arg0;
        }
        /// <summary>
        /// 返回Unicode字符
        /// </summary>
        /// <param name="arg0">参数</param>
        /// <returns></returns>
        public Char EchoChar(Char arg0)
        {
            return arg0;
        }
        /// <summary>
        /// 返回可空Unicode字符
        /// </summary>
        /// <param name="arg0">参数</param>
        /// <returns></returns>
        public Char? EchoNullableChar(Char? arg0)
        {
            return arg0;
        }
        /// <summary>
        /// 返回时间类型
        /// </summary>
        /// <param name="arg0">参数</param>
        /// <returns></returns>
        public DateTime EchoDateTime(DateTime arg0)
        {
            return arg0;
        }
        /// <summary>
        /// 返回可空时间类型
        /// </summary>
        /// <param name="arg0">参数</param>
        /// <returns></returns>
        public DateTime? EchoNullableDateTime(DateTime? arg0)
        {
            return arg0;
        }
        /// <summary>
        /// 返回十进制数
        /// </summary>
        /// <param name="arg0">参数</param>
        /// <returns></returns>
        public Decimal EchoDecimal(Decimal arg0)
        {
            return arg0;
        }
        /// <summary>
        /// 返回可空十进制数
        /// </summary>
        /// <param name="arg0">参数</param>
        /// <returns></returns>
        public Decimal? EchoNullableDecimal(Decimal? arg0)
        {
            return arg0;
        }
        /// <summary>
        /// 返回字符串
        /// </summary>
        /// <param name="arg0">参数</param>
        /// <returns></returns>
        public String EchoString(String arg0)
        {
            return arg0;
        }
        /// <summary>
        /// 转为小写
        /// </summary>
        /// <param name="arg0">参数</param>
        /// <returns></returns>
        public String LowercaseMethod(String arg0)
        {
            String result = String.Empty;
            if (arg0 != null)
            {
                result = arg0.ToLower();
            }
            MessageBox.Show("BindingTestAv8Handler : " + result);
            return result;
        }
        #endregion 方法
    }
}
