using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Hidistro.Entities
{
    /// <summary>
    /// 对象序列化类【通用处理类】
    /// 最后更新 JHB: ON 2012-12-26
    /// </summary>
    public class ObjectSerializerHelper
    {
        #region 数据集(DataSet)序列化的方法

        /// <summary>
        /// 序列化DataSet对象并压缩
        /// </summary>
        /// <param name="ds">待序列化的数据集</param>
        /// <param name="filePath">序列化后的文件地址</param>
        public static bool DataSetSerializer(DataSet ds, string filePath)
        {
            bool _state = true;
            try
            {
                IFormatter formatter = new BinaryFormatter();//定义BinaryFormatter以序列化DataSet对象
                MemoryStream ms = new MemoryStream();//创建内存流对象
                formatter.Serialize(ms, ds);//把DataSet对象序列化到内存流
                byte[] buffer = ms.ToArray();//把内存流对象写入字节数组
                ms.Close();//关闭内存流对象
                ms.Dispose();//释放资源
                Stream _Stream = File.Open(filePath, FileMode.Create);//创建文件
                GZipStream gzipStream = new GZipStream(_Stream, CompressionMode.Compress, true);//创建压缩对象
                gzipStream.Write(buffer, 0, buffer.Length);//把压缩后的数据写入文件
                gzipStream.Close();//关闭压缩流,这里要注意：一定要关闭，要不然解压缩的时候会出现小于4K的文件读取不到数据，大于4K的文件读取不完整
                gzipStream.Dispose();//释放对象
                _Stream.Flush();//释放内存
                _Stream.Close();//关闭流
                _Stream.Dispose();//释放对象
            }
            catch (Exception ex)
            {
                ex.ToString();
                _state = false;
            }
            return _state;
        }

       /// <summary>
        /// 不压缩直接序列化DataSet
       /// </summary>
        /// <param name="ds">待序列化的数据集</param>
        /// <param name="filePath">序列化后的文件地址</param>
        public static bool DataSetSerializerNoCompress(DataSet ds, string filePath)
        {
            bool _state = true;
            try
            {
                IFormatter formatter = new BinaryFormatter();//定义BinaryFormatter以序列化DataSet对象
                Stream _Stream = File.Open(filePath, FileMode.Create);//创建文件
                formatter.Serialize(_Stream, ds);//把DataSet对象序列化到文件
                _Stream.Flush();//释放内存
                _Stream.Close();//关闭流
                _Stream.Dispose();//释放对象
            }
            catch
            {
                _state = false;
            }
            return _state;
        }

        /// <summary>
        /// 反序列化压缩的DataSet 
        /// </summary>
        /// <param name="FilePath">待反序列化的文件地址</param>
        public static DataSet DataSetDeserialize(string FilePath)
        {
            try
            {
                Stream _Stream = File.Open(FilePath, FileMode.Open);//打开文件
                _Stream.Position = 0;//设置文件流的位置 
                GZipStream gzipStream = new GZipStream(_Stream, CompressionMode.Decompress);//创建解压对象
                byte[] buffer = new byte[4096];//定义数据缓冲
                int offset = 0;//定义读取位置 
                MemoryStream ms = new MemoryStream();//定义内存流 
                while ((offset = gzipStream.Read(buffer, 0, buffer.Length)) != 0)
                {
                    ms.Write(buffer, 0, offset);//解压后的数据写入内存流   
                }
                IFormatter formatter = new BinaryFormatter();//定义BinaryFormatter以反序列化DataSet对象
                ms.Position = 0;//设置内存流的位置
                DataSet ds;
                try
                {
                    ds = (DataSet)formatter.Deserialize(ms);//反序列化   
                }
                catch
                {
                    ds = null;
                }
                ms.Close();//关闭内存流   
                ms.Dispose();//释放资源 
                _Stream.Flush();//释放内存
                _Stream.Close();//关闭文件流   
                _Stream.Dispose();//释放资源   
                gzipStream.Close();//关闭解压缩流   
                gzipStream.Dispose();//释放资源   
                return ds;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 反序列化未压缩的DataSet   
        /// </summary>
        /// <param name="FilePath">待反序列化的文件地址</param>
        public static DataSet DataSetDeserializeNoCompress(string FilePath)
        {
            Stream _Stream = File.Open(FilePath, FileMode.Open);//打开文件 
            _Stream.Position = 0;//设置文件流的位置 
            IFormatter formatter = new BinaryFormatter();//定义BinaryFormatter以反序列化DataSet对象 
            DataSet ds;
            try
            {
                ds = (DataSet)formatter.Deserialize(_Stream);//反序列化   
            }
            catch
            {
                ds = null;
            }
            _Stream.Flush();//释放内存
            _Stream.Close();//关闭文件流   
            _Stream.Dispose();//释放资源   
            return ds;
        }

        #endregion 数据集序列化的方法
    }
}
