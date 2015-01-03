using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Web;

namespace WereViewApp.Modules.Cache {
    public class CacheDataInFile {
        #region Propertise
        /// <summary>
        /// "/App_Data/DataCached/"
        /// </summary>
        public string Root { get; set; }
        public string AdditionalRoot { get; set; } 
        #endregion
        
        #region Fields
        /// <summary>
        /// Doesn't contain slash.
        /// </summary>
        static string appPath = AppDomain.CurrentDomain.BaseDirectory; 
        #endregion

        #region Constructors

        /// <summary>
        /// Default root = "\App_Data\DataCached\"
        /// </summary>
        /// <param name="addtionalRoot">Should contain slash</param>
        public CacheDataInFile(string addtionalRoot) {
            AdditionalRoot = addtionalRoot;
            Root = @"App_Data\DataCached\";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="addtionalRoot">Should contain slash</param>
        /// <param name="rootName">Should contain slash</param>
        public CacheDataInFile(string addtionalRoot, string rootName) {
            AdditionalRoot = addtionalRoot;
            Root = rootName;
        } 
        #endregion

        #region File Read Write and binaries

        /// <summary>
        /// Object to binary
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public byte[] ObjectToByteArray(Object obj) {
            if (obj == null)
                return null;
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            bf.Serialize(ms, obj);
            return ms.ToArray();
        }

        /// <summary>
        /// Read Binary to Object
        /// </summary>
        /// <param name="arrBytes"></param>
        /// <returns></returns>
        public Object ReadFromBinaryObject(byte[] arrBytes) {
            if (arrBytes == null || arrBytes.Length == 0) {
                return null;
            }
            MemoryStream memStream = new MemoryStream();
            BinaryFormatter binForm = new BinaryFormatter();
            memStream.Write(arrBytes, 0, arrBytes.Length);
            memStream.Seek(0, SeekOrigin.Begin);
            Object obj = (Object)binForm.Deserialize(memStream);
            return obj;
        }

        /// <summary>
        /// Save any object into file over the previous one.
        /// </summary>
        /// <param name="fileNamelocation">Should contain extension(ex. text.txt) .Relative file location  from root + additonroot</param>
        /// <param name="SavingListOrItem">Saving item. Could be array or list or anything.</param>
        public void SaveInBinary(string fileNamelocation, object SavingListOrItem) {
            // Write data to Test.data.
            //new Thread(() => {
            fileNamelocation = appPath + Root + AdditionalRoot + fileNamelocation;

            //
            if (File.Exists(fileNamelocation)) {
                File.Delete(fileNamelocation);
            }
            // write files into binary
            try {
                FileStream fs = new FileStream(fileNamelocation, FileMode.CreateNew);
                // Create the writer for data.
                BinaryWriter w = new BinaryWriter(fs);
                byte[] BinaryObj = this.ObjectToByteArray(SavingListOrItem);
                w.Write(BinaryObj);
                w.Close();
            } catch (Exception ex) {
                DevMVCComponent.Starter.HanldeError.HandleBy(ex);
            }

            //}).Start();
        }

        /// <summary>
        /// Save any object into file over the previous one.
        /// </summary>
        /// <param name="fileNamelocation">Should contain extension(ex. text.txt) .Relative file location  from root + additonroot</param>
        /// <param name="SavingListOrItem">Saving item. Could be array or list or anything.</param>
        public void SaveInBinaryAsync(string fileNamelocation, object SavingListOrItem) {
            // Write data to Test.data.
            new Thread(() => {
                fileNamelocation = appPath + Root + AdditionalRoot + fileNamelocation;

                //
                if (File.Exists(fileNamelocation)) {
                    File.Delete(fileNamelocation);
                }
                // write files into binary
                try {
                    FileStream fs = new FileStream(fileNamelocation, FileMode.CreateNew);
                    // Create the writer for data.
                    BinaryWriter w = new BinaryWriter(fs);
                    byte[] BinaryObj = this.ObjectToByteArray(SavingListOrItem);
                    w.Write(BinaryObj);
                    w.Close();
                } catch (Exception ex) {
                    DevMVCComponent.Starter.HanldeError.HandleBy(ex);
                }

            }).Start();
        }


        public object ReadObjectFromBinaryFile(string fileNamelocation) {
            fileNamelocation = appPath + Root + AdditionalRoot + fileNamelocation;
            if (File.Exists(fileNamelocation)) {
                try {
                    byte[] fileBytes = File.ReadAllBytes(fileNamelocation);
                    return this.ReadFromBinaryObject(fileBytes);
                } catch (Exception ex) {
                    DevMVCComponent.Starter.HanldeError.HandleBy(ex);
                    return null;
                }

            } else {
                return null;
            }
        }

        public object ReadObjectFromBinaryFileAsCache(string fileNamelocation, float hoursToExpire) {
            fileNamelocation = appPath + Root + AdditionalRoot + fileNamelocation;
            if (File.Exists(fileNamelocation)) {
                try {
                    var info = new FileInfo(fileNamelocation);
                    if (info != null) {
                        var duration = DateTime.Now - info.CreationTime;
                        float hours = duration.Minutes / 60f;
                        if (hours > hoursToExpire) {
                            if (File.Exists(fileNamelocation)) {
                                File.Delete(fileNamelocation);
                            }
                            return null;
                        }
                    }

                    byte[] fileBytes = File.ReadAllBytes(fileNamelocation);
                    return this.ReadFromBinaryObject(fileBytes);
                } catch (Exception ex) {
                    DevMVCComponent.Starter.HanldeError.HandleBy(ex);
                    return null;
                }

            } else {
                return null;
            }
        }
        #endregion

        #region Save and read as normal text file.

        /// <summary>
        /// Save any object into file over the previous one.
        /// </summary>
        /// <param name="fileNamelocation">Should contain extension(ex. text.txt) .Relative file location  from root + additonroot</param>
        /// <param name="content">String to save.</param>
        public void SaveText(string fileNamelocation, string content) {
            fileNamelocation = appPath + Root + AdditionalRoot + fileNamelocation;

            try {
                if (File.Exists(fileNamelocation)) {
                    File.Delete(fileNamelocation);
                }
                File.WriteAllText(fileNamelocation, content);

            } catch (Exception ex) {
                DevMVCComponent.Starter.HanldeError.HandleBy(ex);
            }           
            
        }

        /// <summary>
        /// Save any object into file over the previous one.
        /// </summary>
        /// <param name="fileNamelocation">Should contain extension(ex. text.txt) .Relative file location  from root + additonroot</param>
        /// <param name="contents">String to save.</param>
        public void SaveText(string fileNamelocation, string [] contents) {
            fileNamelocation = appPath + Root + AdditionalRoot + fileNamelocation;

            try {
                if (File.Exists(fileNamelocation)) {
                    File.Delete(fileNamelocation);
                }
                File.WriteAllLines(fileNamelocation, contents);
            } catch (Exception ex) {
                DevMVCComponent.Starter.HanldeError.HandleBy(ex);
            }

        }

        /// <summary>
        /// Save any object into file over the previous one.
        /// </summary>
        /// <param name="fileNamelocation">Should contain extension(ex. text.txt) .Relative file location  from root + additonroot</param>
        /// <param name="content">String to save.</param>
        public void SaveTextAsync(string fileNamelocation, string content) {
            new Thread(() => {
            
            fileNamelocation = appPath + Root + AdditionalRoot + fileNamelocation;

            try {
                if (File.Exists(fileNamelocation)) {
                    File.Delete(fileNamelocation);
                }
                File.WriteAllText(fileNamelocation, content);

            } catch (Exception ex) {
                DevMVCComponent.Starter.HanldeError.HandleBy(ex);
            }
            }).Start();
        }

        /// <summary>
        /// Save any object into file over the previous one.
        /// </summary>
        /// <param name="fileNamelocation">Should contain extension(ex. text.txt) .Relative file location  from root + additonroot</param>
        /// <param name="contents">String to save.</param>
        public void SaveTextAsync(string fileNamelocation, string[] contents) {
            new Thread(() => {
                fileNamelocation = appPath + Root + AdditionalRoot + fileNamelocation;

                try {
                    if (File.Exists(fileNamelocation)) {
                        File.Delete(fileNamelocation);
                    }
                    File.WriteAllLines(fileNamelocation, contents);
                } catch (Exception ex) {
                    DevMVCComponent.Starter.HanldeError.HandleBy(ex);
                }
            }).Start();
        }

        /// <summary>
        /// Read file from relative path.
        /// </summary>
        /// <param name="fileNamelocation">Relative file path with extension.</param>
        /// <param name="contents"></param>
        /// <returns>Returns null if not found</returns>
        public string ReadFile(string fileNamelocation) {
            try {
                fileNamelocation = appPath + Root + AdditionalRoot + fileNamelocation;
                return File.ReadAllText(fileNamelocation);
            } catch (Exception ex) {
                DevMVCComponent.Starter.HanldeError.HandleBy(ex);
                return null;
            }
        }

        /// <summary>
        /// Read file from relative path.
        /// </summary>
        /// <param name="fileNamelocation">Relative file path with extension.</param>
        /// <param name="contents"></param>
        /// <returns>Returns null if not found</returns>
        public string[] ReadFileLines(string fileNamelocation) {
            try {
                fileNamelocation = appPath + Root + AdditionalRoot + fileNamelocation;
                return File.ReadAllLines(fileNamelocation);
            } catch (Exception ex) {
                DevMVCComponent.Starter.HanldeError.HandleBy(ex);
                return null;
            }
        }
        #endregion
    }
}