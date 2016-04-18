using System;
using System.IO;
using System.Web;
using DevMvcComponent;
using ImageResizer;

namespace WeReviewApp.Modules.Uploads {
    public class UploadProcessor {
        private static readonly string AppPath = AppDomain.CurrentDomain.BaseDirectory;

        /// <summary>
        /// </summary>
        /// <param name="additionalRoot">should contain slash. use : "~/Uploads/Images/" + AdditionalRoot</param>
        public UploadProcessor(string additionalRoot) {
            AdditionalRoots = additionalRoot;
            RootPath = "~/Uploads/Images/";
            FileAdditionalTemp = "_temp";
        }

        /// <summary>
        /// </summary>
        /// <param name="additionalRoot">should contain slash at the end. root + additional path</param>
        /// <param name="root">should contain slash at the end. by default "~/Uploads/Images/"</param>
        public UploadProcessor(string additionalRoot, string root) {
            AdditionalRoots = additionalRoot;
            RootPath = root;
            FileAdditionalTemp = "_temp";
        }

        /// <summary>
        ///     "~/Uploads/Images/"
        /// </summary>
        public string RootPath { get; set; }

        /// <summary>
        ///     By default empty
        /// </summary>
        public string AdditionalRoots { get; set; }

        /// <summary>
        ///     filename + "_temp"; will be added with uploaded file.
        ///     default temp value = "_temp"
        /// </summary>
        public string FileAdditionalTemp { get; set; }

        /// <summary>
        ///     /Uploads/Images/ + Additional Root
        ///     no ~ telda is included.
        /// </summary>
        /// <returns></returns>
        public string GetCombinePathWithAdditionalRoots() {
            return RootPath.Remove(0, 1) + AdditionalRoots;
        }

        /// <summary>
        /// </summary>
        /// <param name="submittedFile"></param>
        /// <returns>Returns extension without '.' and filename without extension</returns>
        public string GetFilename(HttpPostedFileBase submittedFile, ref string extension) {
            string fileName = null;

            if (submittedFile != null) {
                fileName = submittedFile.FileName;
                extension = Path.GetExtension(fileName);
                fileName = fileName.Replace(extension, "");
                extension = extension.Replace(".", "");
            }
            return fileName;
        }

        /// <summary>
        /// </summary>
        /// <param name="submittedFile"></param>
        /// <returns>filename without extension</returns>
        public static string GetFilename(HttpPostedFileBase submittedFile) {
            string fileName = null;
            var extension = "";
            if (submittedFile != null) {
                fileName = submittedFile.FileName;
                extension = Path.GetExtension(fileName);
                fileName = fileName.Replace(extension, "");
            }
            return fileName;
        }

        /// <summary>
        /// </summary>
        /// <param name="submittedFile"></param>
        /// <returns>Returns extension without '.'</returns>
        public static string GetExtension(HttpPostedFileBase submittedFile) {
            string fileName = null;
            var extension = "";
            if (submittedFile != null) {
                fileName = submittedFile.FileName;
                extension = Path.GetExtension(fileName);
                extension = extension.Replace(".", "");
            }
            return extension;
        }

        /// <summary>
        /// </summary>
        /// <param name="submittedFile">Give extension and saves the file.</param>
        /// <param name="fileName">pass as 'filename' without extension.</param>
        /// <param name="number">give number like 0-255</param>
        /// <param name="isNumbering">if numbering true then filename ='filename-number' else filename ='filename-0' </param>
        /// <param name="asTemp">if true then filename ='filename-number_temp'</param>
        /// <param name="isPrivate">root/private/additional</param>
        /// <param name="additinalPathWithRoot">use slash at the end. determinate from constructor's additional path</param>
        /// <param name="rootPath">use slash at the end. Constructors give it efficiently.</param>
        /// <returns>root/private/additionpath/filename-number_temp.jpg</returns>
        public bool UploadFile(HttpPostedFileBase submittedFile, string fileName, int number = -1,
            bool isNumbering = false, bool asTemp = false, bool isPrivate = false, string additinalPathWithRoot = null,
            string rootPath = null) {
            if (submittedFile == null) {
                return false;
            }

            if (additinalPathWithRoot == null) {
                additinalPathWithRoot = AdditionalRoots;
            }

            if (rootPath == null) {
                rootPath = RootPath;
            }

            if (isPrivate) {
                rootPath += "Private/";
            }

            rootPath += additinalPathWithRoot;
            //root/private/addition path
            if (fileName == null) {
                fileName = GetFilename(submittedFile);
            }

            var fileExtension = "";
            fileExtension = GetExtension(submittedFile);

            if (isNumbering) {
                fileName += "-" + number;
            } else {
                fileName += "-" + 0;
            }
            if (asTemp) {
                fileName += FileAdditionalTemp;
            }
            fileName += "." + fileExtension;
            ///filename-number_temp.jpg
            var virtualFileLocation = rootPath + fileName;
            var absLocation = VirtualPathtoAbsoluteServerPath(virtualFileLocation);
            try {
                submittedFile.SaveAs(absLocation);
            } catch (Exception ex) {
                Mvc.Error.HandleBy(ex);
                return false;
            }
            return true;
        }

        /// <summary>
        ///     Process to resize.
        /// </summary>
        /// <param name="sourceLocation">"file.jpg" Write as root ~/Uploads/</param>
        /// <param name="processedLocation"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="ext"></param>
        public void ResizeImageAndProcessImage(string sourceLocation, string processedLocation, double width,
            double height, string ext) {
            if (sourceLocation != null && processedLocation != null) {
                var source = sourceLocation.Replace("~", AppPath).Replace('/', '\\');
                var target = processedLocation.Replace("~", AppPath).Replace('/', '\\');
                var setting = "height=" + height + "&width=" + width + "&mode=stretch&format=" + ext;
                ImageBuilder.Current.Build(source, target, new ResizeSettings(setting));
            }
        }

        /// <summary>
        ///     Resize the image
        /// </summary>
        /// <param name="category"></param>
        /// <param name="sourceFileName">like "Justuploaded"</param>
        /// <param name="processedFileName"></param>
        /// <param name="isSourceAddTemp">if true then sourceFileName(removing ext) += FileAdditionalTemp</param>
        /// <param name="ext"></param>
        /// <param name="additionalRootPath">
        ///     Advertise/ should contain slash. But it should be define at the class creation time.
        ///     Best way to set it in class constructor.
        /// </param>
        /// <param name="rootPath"></param>
        public void ResizeImageAndProcessImage(IUploadableFile file, IImageCategory category,
            string sourceFileName = null, string processedFileName = null, bool isSourceAddTemp = true,
            bool isPrivate = false, string additionalRootPath = null, string rootPath = null) {
            if (file != null && category != null) {
                if (rootPath == null) {
                    rootPath = RootPath;
                }

                if (isPrivate) {
                    rootPath += "Private/";
                }

                if (additionalRootPath == null) {
                    additionalRootPath = AdditionalRoots;
                }

                rootPath += additionalRootPath;
                sourceFileName = GetOrganizeName(file, true, isSourceAddTemp); //soruce as temp
                processedFileName = GetOrganizeName(file, true, false); // target
                var path1 = rootPath + sourceFileName;
                var path2 = rootPath + processedFileName;
                var source = VirtualPathtoAbsoluteServerPath(path1);
                var target = VirtualPathtoAbsoluteServerPath(path2);
                var setting = "height=" + category.Height + "&width=" + category.Width + "&mode=stretch&format=" +
                              file.Extension;
                if (File.Exists(target)) {
                    File.Delete(target);
                }
                try {
                    ImageBuilder.Current.Build(source, target, new ResizeSettings(setting));
                } catch (Exception ex) {
                    Mvc.Error.HandleBy(ex);
                }
            } else {
                throw new Exception("Data missing while upload.");
            }
        }

        public string VirtualPathtoAbsoluteServerPath(string virtualPath) {
            if (virtualPath != null && virtualPath.StartsWith(@"~/")) {
                var abs = AppPath + virtualPath.Remove(0, 2);
                return abs.Replace("/", "\\");
            }
            return virtualPath;
        }

        /// <summary>
        ///     Path will have a slash at the end.
        /// </summary>
        /// <returns></returns>
        public string GetAbsolutePath() {
            var absolutePath = VirtualPathtoAbsoluteServerPath(GetCombinePathWithAdditionalRoots());
            return absolutePath;
        }

        /// <summary>
        ///     Remove temp associated with this file.
        /// </summary>
        /// <param name="file"></param>
        /// <param name="isAddTemp"></param>
        /// <param name="isPrivate"></param>
        /// <param name="additionalRootPath"></param>
        /// <param name="rootPath"></param>
        public void RemoveTempImage(IUploadableFile file, bool isAddTemp = true, bool isPrivate = false,
            string additionalRootPath = null, string rootPath = null) {
            var fileName = GetOrganizeName(file, true, isAddTemp);
            if (rootPath == null) {
                rootPath = RootPath;
            }
            if (additionalRootPath == null) {
                additionalRootPath = AdditionalRoots;
            }

            rootPath += additionalRootPath;

            var path1 = rootPath + fileName;
            var source = path1.Replace("~", AppPath).Replace('/', '\\');
            try {
                File.Delete(source);
            } catch (Exception ex) {
                Mvc.Error.HandleBy(ex);
            }
        }

        /// <summary>
        ///     Returns "Guid-Sequence_temp.ext"
        /// </summary>
        /// <param name="file"></param>
        /// <param name="asTemp">if true then add temp text</param>
        /// <param name="tempString"></param>
        /// <returns>Returns "Guid-Sequence_temp.ext" only returns the filename</returns>
        public static string GetOrganizeNameStatic(IUploadableFile file, bool includeExtention = false,
            bool asTemp = false, string tempString = "_temp") {
            var ext = "";
            if (!asTemp) {
                tempString = "";
            }
            if (includeExtention) {
                ext = "." + file.Extension;
            }
            return file.UploadGuid + "-" + file.Sequence + tempString + ext;
        }

        /// <summary>
        ///     Returns "Guid-Sequence_temp.ext"
        /// </summary>
        /// <param name="file"></param>
        /// <param name="asTemp">if true then add temp text</param>
        /// <param name="tempString"></param>
        /// <returns>Returns "Guid-Sequence_temp.ext" only returns the filename</returns>
        public string GetOrganizeName(IUploadableFile file, bool includeExtention = false, bool asTemp = false,
            string tempString = "_temp") {
            var ext = "";
            if (!asTemp) {
                tempString = "";
            }
            if (includeExtention) {
                ext = "." + file.Extension;
            }
            return file.UploadGuid + "-" + file.Sequence + tempString + ext;
        }
    }
}