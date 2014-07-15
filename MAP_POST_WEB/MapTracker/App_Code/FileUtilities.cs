using System;
using System.IO;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Security.Cryptography;
    public class FileUtilities
    {
        public static bool IsImageFile(string fileName)
        {

            string[] xe = fileName.Split(".".ToCharArray());

            if (MyContext.FilterImageFile.IndexOf(xe[xe.Length-1].ToLower()) > -1)
                return true;
            return false;
        }
        public static Point getNewPoint(string photoPath, int thumbnailSize)
        {
            try
            {
                Bitmap photo = new Bitmap(photoPath);

                if (photo.Width < thumbnailSize)
                {
                  return  new Point(photo.Width, photo.Height);
                }

                

                int width, height;
                if (photo.Width > photo.Height)
                {
                    width = thumbnailSize;
                    height = photo.Height * thumbnailSize / photo.Width;
                }
                else
                {
                    width = photo.Width * thumbnailSize / photo.Height;
                    height = thumbnailSize;
                }
                return new Point(width, height);
            }catch
            {
                return new Point(0, 0);
            }
            return new Point(0, 0);

            
        }
        public static string GetExtFile(string fileName)
        {
            string[] xe = fileName.Split(".".ToCharArray());
            return xe[xe.Length - 1].ToLower();
        }
        public static bool IsPriceFile(string fileName)
        {

            string[] xe = fileName.Split(".".ToCharArray());
            if (MyContext.PriceFile.IndexOf(xe[1].ToLower()) > -1)
                return true;
            return false;
        }
        public static bool IsNormal(string fileName)
        {

            string[] xe = fileName.Split(".".ToCharArray());
            if (MyContext.DangerousFile.IndexOf(xe[1]) > -1)
                return false;
            return true;
        }
        public static bool IsExist(string fileName)
        {

           return   File.Exists(fileName);
        }
        public static string CreateFolderDate(string path)
        {
            string tempath = path;
            string yearFolder = DateTime.Today.Year.ToString();
            tempath = tempath + "/" + yearFolder;
            if (Directory.Exists(tempath) == false)
                Directory.CreateDirectory(tempath);

            tempath = path;
            string monthFolder = DateTime.Today.Month.ToString();
            tempath = tempath + "/" + yearFolder + "/" + monthFolder;
            if (Directory.Exists(tempath) == false)
                Directory.CreateDirectory(tempath);

            tempath = path;
            string dayFolder = DateTime.Today.Day.ToString();
            tempath = tempath + "/" + yearFolder + "/" + monthFolder + "/" + dayFolder;
            if (Directory.Exists(tempath) == false)
                Directory.CreateDirectory(tempath);
            return yearFolder + "/" + monthFolder + "/" + dayFolder;
        }
        public static string [] GetFileInFolder(string path)
        {
             return Directory.GetFiles(path);
        }
        public static int GetHeightImage(int WidthImage, int HeightImage,  int  width_height,out int withxx)
       {
           int _maxwidth_height = width_height;
           int _width = WidthImage;//394
           int _height = HeightImage;//300
           if (_width > _maxwidth_height)
           {
               _height = (_maxwidth_height * _height / _width) + 2;
               _width = _maxwidth_height;
               if (_height > width_height)
                   _height = width_height;
           }
           if (_height > _maxwidth_height)
           {
               _width = (_maxwidth_height * _width / _height) + 2;
               _height = _maxwidth_height;
               if (_width > width_height)
                   _height = width_height;
           }
           withxx = _width;
           return _height;
       }
          public static int GetHeightImage1(int WidthImage, int HeightImage,  int  width_height,out int withxx)
       {
           int _maxwidth_height = width_height;//mong muon 200
           int _width = WidthImage;//800
           int _height = HeightImage;//11000
           if (_width > _maxwidth_height)
           {
               _height = (_maxwidth_height * _height / _width) ;
               _width = _maxwidth_height;
              // if (_height > width_height)
              //     _height = width_height;
           }
          /* if (_height > _maxwidth_height)
           {
               _width = (_maxwidth_height * _width / _height) ;
               _height = _maxwidth_height;
               if (_width > width_height)
                   _height = width_height;
           }*/
           withxx = _width;
           return _height;
       }
        public static bool ResizeImageWidth_Height(string  filexxpathold, string _uploadPathI, int width_height)
        {
           
                if (true)
                {
                    string _filename = "";
                  
                    System.Drawing.Image _img = System.Drawing.Image.FromFile(filexxpathold);
                        
                    int _maxwidth_height = width_height;
                    int _width = _img.Width;//394
                    int _height = _img.Height;//300
                    if (_width > _maxwidth_height)
                    {
                        _height = (_maxwidth_height * _height / _width) + 2;
                        _width = _maxwidth_height;
                        if (_height > width_height)
                            _height = width_height;
                    }
                    if (_height > _maxwidth_height)
                    {
                        _width = (_maxwidth_height * _width / _height) + 2;
                        _height = _maxwidth_height;
                        if (_width > width_height)
                            _height = width_height;
                    }
                    System.Drawing.Image fullSizeImg = System.Drawing.Image.FromFile(filexxpathold);

                    System.Drawing.Image.GetThumbnailImageAbort dummyCallBack = new System.Drawing.Image.GetThumbnailImageAbort(ThumbnailCallback);

                    System.Drawing.Image thumbNailImg = fullSizeImg.GetThumbnailImage(_width, _height, dummyCallBack, IntPtr.Zero);

                    //We need to create a unique filename for each generated image
                    

                  //  String MyString = MyDate.ToString("ddMMyyhhmmss") + ".png";

                    //Save the thumbnail in Png format. You may change it to a diff format with the ImageFormat property
                    thumbNailImg.Save(_uploadPathI);

                    thumbNailImg.Dispose();

                    //System.Drawing.Image.GetThumbnailImageAbort dummyCallBack = new System.Drawing.Image.GetThumbnailImageAbort(ThumbnailCallback);
                    //System.Drawing.Image _newImg =
                    //    _img.GetThumbnailImage(_width, _height, dummyCallBack, IntPtr.Zero);
                    //_newImg.Save(_uploadPathI);
                    //_newImg.Dispose();
                    //_img.Dispose();
                    //filexx.Dispose();
                    return true;
                }
                else
                    return false;
            return false;
        }
        private static bool ThumbnailCallback()
        {
            return false;
        }
        
        public static string AddTextToFileName(string fileName, string suffix)
        {
            Regex re = new Regex("(.*)(\\.[^\\\\/]*\\b)");
            //return re.Replace(fileName, "$1") + suffix + re.Replace(fileName, "$2");
            string fx = re.Replace(fileName, "$1") + suffix ;
            return fx.Replace(".", "_icon.");
        }
        public static void DeleteFile(string fileName)
        {
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
              
        }
        public static string ReadFile(string phypath)
        {
            try
            {

                FileStream file = new FileStream(phypath, FileMode.OpenOrCreate, FileAccess.Read);

                StreamReader sr = new StreamReader(file);

                string body = sr.ReadToEnd();

                sr.Close();

                file.Close();
                return body;
            }
            catch
            {
                return "";
            }

        }
        public static System.Collections.ArrayList ReadFileLine(string phypath)
        {
            System.Collections.ArrayList arl =new System.Collections.ArrayList();

                using (StreamReader sr = new StreamReader(phypath)) 
                {
                    while (sr.Peek() >= 0) 
                    {
                       arl.Add( sr.ReadLine());
                    }
                }
                return arl;
            

        }
        public static void WriteFile(string FilePath, string content)
        {
            StreamWriter Tex = File.CreateText(FilePath);
            Tex.Write(content);
            Tex.Close();

        }
        public static void RenameFile(string nameold, string nameNew)
        {
            File.Move(nameold, nameNew);
        }
        public static string CreateThumbnailImage(string sourceImage, string thumbnailExtension, int maxImageSize)
        {
            int thumbnailWidth;
            int thumbnailHeight;

            // Determine thumbnail filename
            string thumbnailFilename = AddTextToFileName(sourceImage, thumbnailExtension);

            // Open original image and determine thumbnail size based on image dimensions and the max image size
            System.Drawing.Image originalImage = System.Drawing.Image.FromFile(sourceImage);
            int sourceWidth = originalImage.Width;
            int sourceHeight = originalImage.Height;
            double widthHeightRatio = (double)sourceWidth / (double)sourceHeight;

            // If width greater than height, then width should be max image size, otherwise height should be.
            // Image should keep the same proportions.
            if (widthHeightRatio > 1.0)
            {
                thumbnailWidth = maxImageSize;
                thumbnailHeight = (int)(maxImageSize / widthHeightRatio);
            }
            else
            {
                thumbnailWidth = (int)(maxImageSize * widthHeightRatio);
                thumbnailHeight = maxImageSize;
            }

            // Create bitmap and graphics objects for the new image
            System.Drawing.Bitmap thumbnailBitmap = new System.Drawing.Bitmap(thumbnailWidth, thumbnailHeight, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(thumbnailBitmap);

            // set graphics parameters to optimize thumbnail image
            g.CompositingMode = CompositingMode.SourceOver;
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;

            // Transform image to new size and save thumbnail
            g.DrawImage(originalImage, 0, 0, thumbnailWidth, thumbnailHeight);
            thumbnailBitmap.Save(thumbnailFilename, originalImage.RawFormat);

            // Return thumbnail filename
            return System.IO.Path.GetFileName(thumbnailFilename);
        }
        public static string GetRandomPass()
        {
            return RandomPassword.Generate(5);
        }
    }
public class RandomPassword
{
    // Define default min and max password lengths.
    private static int DEFAULT_MIN_PASSWORD_LENGTH = 8;
    private static int DEFAULT_MAX_PASSWORD_LENGTH = 10;

    // Define supported password characters divided into groups.
    // You can add (or remove) characters to (from) these groups.
    private static string PASSWORD_CHARS_LCASE = "abcdefgijkmnopqrstwxyz";
    private static string PASSWORD_CHARS_UCASE = "ABCDEFGHJKLMNPQRSTWXYZ";
    private static string PASSWORD_CHARS_NUMERIC = "23456789";
    private static string PASSWORD_CHARS_SPECIAL = "*$-+?_&=!%{}/";

    /// <summary>
    /// Generates a random password.
    /// </summary>
    /// <returns>
    /// Randomly generated password.
    /// </returns>
    /// <remarks>
    /// The length of the generated password will be determined at
    /// random. It will be no shorter than the minimum default and
    /// no longer than maximum default.
    /// </remarks>
    public static string Generate()
    {
        return Generate(DEFAULT_MIN_PASSWORD_LENGTH,
                        DEFAULT_MAX_PASSWORD_LENGTH);
    }

    /// <summary>
    /// Generates a random password of the exact length.
    /// </summary>
    /// <param name="length">
    /// Exact password length.
    /// </param>
    /// <returns>
    /// Randomly generated password.
    /// </returns>
    public static string Generate(int length)
    {
        return Generate(length, length);
    }

    /// <summary>
    /// Generates a random password.
    /// </summary>
    /// <param name="minLength">
    /// Minimum password length.
    /// </param>
    /// <param name="maxLength">
    /// Maximum password length.
    /// </param>
    /// <returns>
    /// Randomly generated password.
    /// </returns>
    /// <remarks>
    /// The length of the generated password will be determined at
    /// random and it will fall with the range determined by the
    /// function parameters.
    /// </remarks>
    public static string Generate(int minLength,
                                  int maxLength)
    {
        // Make sure that input parameters are valid.
        if (minLength <= 0 || maxLength <= 0 || minLength > maxLength)
            return null;

        // Create a local array containing supported password characters
        // grouped by types. You can remove character groups from this
        // array, but doing so will weaken the password strength.
        char[][] charGroups = new char[][] 
        {
            PASSWORD_CHARS_LCASE.ToCharArray(),
            PASSWORD_CHARS_UCASE.ToCharArray(),
            PASSWORD_CHARS_NUMERIC.ToCharArray(),
            PASSWORD_CHARS_SPECIAL.ToCharArray()
        };

        // Use this array to track the number of unused characters in each
        // character group.
        int[] charsLeftInGroup = new int[charGroups.Length];

        // Initially, all characters in each group are not used.
        for (int i = 0; i < charsLeftInGroup.Length; i++)
            charsLeftInGroup[i] = charGroups[i].Length;

        // Use this array to track (iterate through) unused character groups.
        int[] leftGroupsOrder = new int[charGroups.Length];

        // Initially, all character groups are not used.
        for (int i = 0; i < leftGroupsOrder.Length; i++)
            leftGroupsOrder[i] = i;

        // Because we cannot use the default randomizer, which is based on the
        // current time (it will produce the same "random" number within a
        // second), we will use a random number generator to seed the
        // randomizer.

        // Use a 4-byte array to fill it with random bytes and convert it then
        // to an integer value.
        byte[] randomBytes = new byte[4];

        // Generate 4 random bytes.
        RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
        rng.GetBytes(randomBytes);

        // Convert 4 bytes into a 32-bit integer value.
        int seed = (randomBytes[0] & 0x7f) << 24 |
                    randomBytes[1] << 16 |
                    randomBytes[2] << 8 |
                    randomBytes[3];

        // Now, this is real randomization.
        Random random = new Random(seed);

        // This array will hold password characters.
        char[] password = null;

        // Allocate appropriate memory for the password.
        if (minLength < maxLength)
            password = new char[random.Next(minLength, maxLength + 1)];
        else
            password = new char[minLength];

        // Index of the next character to be added to password.
        int nextCharIdx;

        // Index of the next character group to be processed.
        int nextGroupIdx;

        // Index which will be used to track not processed character groups.
        int nextLeftGroupsOrderIdx;

        // Index of the last non-processed character in a group.
        int lastCharIdx;

        // Index of the last non-processed group.
        int lastLeftGroupsOrderIdx = leftGroupsOrder.Length - 1;

        // Generate password characters one at a time.
        for (int i = 0; i < password.Length; i++)
        {
            // If only one character group remained unprocessed, process it;
            // otherwise, pick a random character group from the unprocessed
            // group list. To allow a special character to appear in the
            // first position, increment the second parameter of the Next
            // function call by one, i.e. lastLeftGroupsOrderIdx + 1.
            if (lastLeftGroupsOrderIdx == 0)
                nextLeftGroupsOrderIdx = 0;
            else
                nextLeftGroupsOrderIdx = random.Next(0,
                                                     lastLeftGroupsOrderIdx);

            // Get the actual index of the character group, from which we will
            // pick the next character.
            nextGroupIdx = leftGroupsOrder[nextLeftGroupsOrderIdx];

            // Get the index of the last unprocessed characters in this group.
            lastCharIdx = charsLeftInGroup[nextGroupIdx] - 1;

            // If only one unprocessed character is left, pick it; otherwise,
            // get a random character from the unused character list.
            if (lastCharIdx == 0)
                nextCharIdx = 0;
            else
                nextCharIdx = random.Next(0, lastCharIdx + 1);

            // Add this character to the password.
            password[i] = charGroups[nextGroupIdx][nextCharIdx];

            // If we processed the last character in this group, start over.
            if (lastCharIdx == 0)
                charsLeftInGroup[nextGroupIdx] =
                                          charGroups[nextGroupIdx].Length;
            // There are more unprocessed characters left.
            else
            {
                // Swap processed character with the last unprocessed character
                // so that we don't pick it until we process all characters in
                // this group.
                if (lastCharIdx != nextCharIdx)
                {
                    char temp = charGroups[nextGroupIdx][lastCharIdx];
                    charGroups[nextGroupIdx][lastCharIdx] =
                                charGroups[nextGroupIdx][nextCharIdx];
                    charGroups[nextGroupIdx][nextCharIdx] = temp;
                }
                // Decrement the number of unprocessed characters in
                // this group.
                charsLeftInGroup[nextGroupIdx]--;
            }

            // If we processed the last group, start all over.
            if (lastLeftGroupsOrderIdx == 0)
                lastLeftGroupsOrderIdx = leftGroupsOrder.Length - 1;
            // There are more unprocessed groups left.
            else
            {
                // Swap processed group with the last unprocessed group
                // so that we don't pick it until we process all groups.
                if (lastLeftGroupsOrderIdx != nextLeftGroupsOrderIdx)
                {
                    int temp = leftGroupsOrder[lastLeftGroupsOrderIdx];
                    leftGroupsOrder[lastLeftGroupsOrderIdx] =
                                leftGroupsOrder[nextLeftGroupsOrderIdx];
                    leftGroupsOrder[nextLeftGroupsOrderIdx] = temp;
                }
                // Decrement the number of unprocessed groups.
                lastLeftGroupsOrderIdx--;
            }
        }

        // Convert password characters into a string and return the result.
        return new string(password);
    }

}
