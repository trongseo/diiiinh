////////////////////////////////////////////////////////////////////////////////////////////
//             				 Pager Control For ASP.NET 2.0			                      //
//								Created By Bidel.Akbari									  //
//									November 2006									  	  //
//								bidel.akbari@gmail.com							          //
////////////////////////////////////////////////////////////////////////////////////////////

#region // using Directives
using System;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Collections.Generic;
using System.Collections;
#endregion


    [ToolboxData("<{0}:Pager runat=\"server\"></{0}:Pager>")]
    public class Pager : WebControl, IPostBackEventHandler, INamingContainer
    {
        #region // PostBack Stuff
        private static readonly object EventCommand = new object();

        public event CommandEventHandler Command
        {
            add { Events.AddHandler(EventCommand, value); }
            remove { Events.RemoveHandler(EventCommand, value); }
        }

        protected virtual void OnCommand(CommandEventArgs e)
        {
            CommandEventHandler clickHandler = (CommandEventHandler)Events[EventCommand];
            if (clickHandler != null) clickHandler(this, e);
        }

        void IPostBackEventHandler.RaisePostBackEvent(string eventArgument)
        {
            OnCommand(new CommandEventArgs(this.UniqueID, Convert.ToInt32(eventArgument)));
        }
        #endregion

        #region // Accessors

        private double _itemCount; // total count of rows        
        private int _pageCount; // Total No of Pages       

        [Browsable(false)]
        public double ItemCount
        {
            get { return _itemCount; }
            set
            {
                _itemCount = value;
                double divide = ItemCount / PageSize;
                double ceiled = System.Math.Ceiling(divide);
                PageCount = Convert.ToInt32(ceiled);
            }
        }

        [Browsable(false)]
        public int CurrentIndex
        {
            get
            {
                if (ViewState["aspnetPagerCurrentIndex"] == null)
                {
                    ViewState["aspnetPagerCurrentIndex"] = 1;
                    return 1;
                }
                else
                {
                    return Convert.ToInt32(ViewState["aspnetPagerCurrentIndex"]);
                }
            }
            set { ViewState["aspnetPagerCurrentIndex"] = value; }
        }

        [Browsable(false)]
        public int PageCount
        {
            get { return _pageCount; }
            set { _pageCount = value; }
        }

        #endregion

        #region // General property

        private int _pageSize = 5;          // the Size of items that can be displayed on page        
        private string _cssLayout = "";    // generate alt title for page indeces

        [Category("GeneralSettings")]
        public int PageSize
        {
            get { return _pageSize; }
            set { _pageSize = value; }
        }

        [Category("GeneralSettings")]
        public string CSSLayout
        {
            get { return _cssLayout; }
            set { _cssLayout = value; }
        }

        #endregion

        #region // Summary property

        // show summary
        private string _summary = "";
        private string _LAST = "&gt;&gt;";
        private string _FIRST = "&lt;&lt;";
        private string _previous = "&lt;";
        private string _next = "&gt;";
        private string _imgLAST = "";
        private string _imgFIRST = "";
        private string _imgprevious = "";
        private string _imgnext = "";
        private string _tipLAST = "";
        private string _tipFIRST = "";
        private string _tipprevious = "";
        private string _tipnext = "";
        private string _tippage = "";
        private string _icurr = "$curr";
        private string _itotal = "$total";
        private string _isize = "$size";
        private string _ifrom = "$recfrom";
        private string _ito = "$recto";
        private bool _rightToLeft = false;

        [Category("GlobalizaionSettings")]
        public string showSummary
        {
            get { return _summary; }
            set { _summary = value; }
        }

        [Category("GlobalizaionSettings")]
        public string LastClause
        {
            get { return _LAST; }
            set { _LAST = value; }
        }

        [Category("GlobalizaionSettings")]
        public string FirstClause
        {
            get { return _FIRST; }
            set { _FIRST = value; }
        }

        [Category("GlobalizaionSettings")]
        public string PreviousClause
        {
            get { return _previous; }
            set { _previous = value; }
        }

        [Category("GlobalizaionSettings")]
        public string NextClause
        {
            get { return _next; }
            set { _next = value; }
        }

        [Category("GlobalizaionSettings")]
        public string ImageLastClause
        {
            get { return _imgLAST; }
            set { _imgLAST = value; }
        }

        [Category("GlobalizaionSettings")]
        public string ImageFirstClause
        {
            get { return _imgFIRST; }
            set { _imgFIRST = value; }
        }

        [Category("GlobalizaionSettings")]
        public string ImagePreviousClause
        {
            get { return _imgprevious; }
            set { _imgprevious = value; }
        }

        [Category("GlobalizaionSettings")]
        public string ImageNextClause
        {
            get { return _imgnext; }
            set { _imgnext = value; }
        }

        [Category("GlobalizaionSettings")]
        public string TipLastClause
        {
            get { return _tipLAST; }
            set { _tipLAST = value; }
        }

        [Category("GlobalizaionSettings")]
        public string TipFirstClause
        {
            get { return _tipFIRST; }
            set { _tipFIRST = value; }
        }

        [Category("GlobalizaionSettings")]
        public string TipPreviousClause
        {
            get { return _tipprevious; }
            set { _tipprevious = value; }
        }

        [Category("GlobalizaionSettings")]
        public string TipNextClause
        {
            get { return _tipnext; }
            set { _tipnext = value; }
        }

        [Category("GlobalizaionSettings")]
        public string TipPageClause
        {
            get { return _tippage; }
            set { _tippage = value; }
        }


        [Category("GlobalizaionSettings")]
        public bool RTL
        {
            get { return _rightToLeft; }
            set { _rightToLeft = value; }
        }

        #endregion

        #region // Render Utilities

        private string GetFirstTip()
        { 
            string str = "";
            int from = 0, to = 0;
            if (ItemCount > 0)
            {
                from = 1;
                if (ItemCount > PageSize) to = PageSize;
                else to = (int)ItemCount;
            }
            str = TipFirstClause
                .Replace(_icurr, "1")
                .Replace(_itotal, PageCount.ToString())
                .Replace(_isize, PageSize.ToString())
                .Replace(_ifrom, from.ToString())
                .Replace(_ito, to.ToString());
            return str;
        }

        private string GetLastTip()
        {
            string str = "";
            int from = 0, to = 0;
            if (ItemCount > 0)
            {
                to = (int)ItemCount;
                if (ItemCount % PageSize == 0)
                    from = (PageCount - 1) * PageSize + 1;
                else
                    from = (PageCount - 1) * PageSize + 1;
            }
            str = TipLastClause
                .Replace(_icurr, PageCount.ToString())
                .Replace(_itotal, PageCount.ToString())
                .Replace(_isize, PageSize.ToString())
                .Replace(_ifrom, from.ToString())
                .Replace(_ito, to.ToString());
            return str;
        }

        private string GetNextTip()
        {
            string str = "";
            int current = CurrentIndex == PageCount ? CurrentIndex : CurrentIndex + 1;
            int from = 0, to = 0;
            if (ItemCount > 0)
            {
                from = (current - 1) * PageSize + 1;
                to = (int)((current * PageSize) > ItemCount ? ItemCount : current * PageSize);
            }
            str = TipNextClause
                .Replace(_icurr, Convert.ToString(current))
                .Replace(_itotal, PageCount.ToString())
                .Replace(_isize, PageSize.ToString())
                .Replace(_ifrom, from.ToString())
                .Replace(_ito, to.ToString());
            return str;
        }

        private string GetBackTip()
        {
            string str = "";
            int current = CurrentIndex == 1 ? 1 : CurrentIndex - 1;
            int from = 0, to = 0;
            if (ItemCount > 0)
            {
                from = (current - 1) * PageSize + 1;
                to = (int)((current * PageSize) > ItemCount ? ItemCount : current * PageSize);
            }
            str = TipPreviousClause
                .Replace(_icurr, Convert.ToString(current))
                .Replace(_itotal, PageCount.ToString())
                .Replace(_isize, PageSize.ToString())
                .Replace(_ifrom, from.ToString())
                .Replace(_ito, to.ToString());
            return str;
        }

        private string GetPageTip(int index)
        {
            string str = "";
            int from = 0, to = 0;
            if (ItemCount > 0)
            {
                from = (index - 1) * PageSize + 1;
                to = (int)((index * PageSize) > ItemCount ? ItemCount : index * PageSize);
            }
            str = TipPageClause
                .Replace(_icurr, Convert.ToString(index))
                .Replace(_itotal, PageCount.ToString())
                .Replace(_isize, PageSize.ToString())
                .Replace(_ifrom, from.ToString())
                .Replace(_ito, to.ToString());
            return str;
        }

        private string RenderSummary()
        {
            string str = "";
            str = showSummary
                .Replace(_icurr, CurrentIndex.ToString())
                .Replace(_itotal, PageCount.ToString()
                .Replace(_isize, PageSize.ToString())
                .Replace(_ifrom, Convert.ToString((CurrentIndex - 1) * PageCount + 1))
                .Replace(_ito, Convert.ToString(CurrentIndex * PageCount)));
            return str;
        }

        private string RenderFirst()
        {
            string templateCell = "";

            if (ImageFirstClause != "")
                templateCell += "<td class=\"PagerImagePageCells\">";
            else
                templateCell += "<td class=\"PagerOtherPageCells\">";
            if (GetFirstTip() != "")
                templateCell += "<a class=\"PagerHyperlinkStyle\" href=\"{0}\" title=\"" + GetFirstTip() + " " + "\">";
            else
                templateCell += "<a class=\"PagerHyperlinkStyle\" href=\"{0}\">";
            if (ImageFirstClause != "")
                templateCell += "<img src=\"" + ImageFirstClause + "\" border=\"0\" />";
            else
                templateCell += FirstClause;
            templateCell += "</a></td>";
            return String.Format(templateCell, Page.ClientScript.GetPostBackClientHyperlink(this, "1"));
        }

        private string RenderLast()
        {
            string templateCell = "";
            if (ImageLastClause != "")
                templateCell += "<td class=\"PagerImagePageCells\">";
            else
                templateCell += "<td class=\"PagerOtherPageCells\">";
            if (GetLastTip() != "")
                templateCell += "<a class=\"PagerHyperlinkStyle\" href=\"{0}\" title=\"" + GetLastTip() + " " + "\">";
            else
                templateCell += "<a class=\"PagerHyperlinkStyle\" href=\"{0}\">";
            if (ImageLastClause != "")
                templateCell += "<img src=\"" + ImageLastClause + "\" border=\"0\" />";
            else
                templateCell += LastClause;
            templateCell += "</a></td>";
            return String.Format(templateCell, Page.ClientScript.GetPostBackClientHyperlink(this, PageCount.ToString()));
        }

        private string RenderBack()
        {
            string back = "";
            if (CurrentIndex == 1) back = CurrentIndex.ToString();
            else back = (CurrentIndex - 1).ToString();
            string templateCell = "";
            if (ImagePreviousClause != "")
                templateCell += "<td class=\"PagerImagePageCells\">";
            else
                templateCell += "<td class=\"PagerOtherPageCells\">";
            if (GetBackTip() != "")
                templateCell += "<a class=\"PagerHyperlinkStyle\" href=\"{0}\" title=\"" + GetBackTip() + " " + "\">";
            else
                templateCell += "<a class=\"PagerHyperlinkStyle\" href=\"{0}\">";
            if (ImagePreviousClause != "")
                templateCell += "<img src=\"" + ImagePreviousClause + "\" border=\"0\" />";
            else
                templateCell += PreviousClause;
            templateCell += "</a></td>";
            return String.Format(templateCell, Page.ClientScript.GetPostBackClientHyperlink(this, back));
        }

        private string RenderNext()
        {
            string next = "";
            if (CurrentIndex == PageCount) next = CurrentIndex.ToString();
            else next = (CurrentIndex + 1).ToString();
            string templateCell = "";
            if (ImageNextClause != "")
                templateCell += "<td class=\"PagerImagePageCells\">";
            else
                templateCell += "<td class=\"PagerOtherPageCells\">";
            if (GetNextTip() != "")
                templateCell += "<a class=\"PagerHyperlinkStyle\" href=\"{0}\" title=\"" + GetNextTip() + " " + "\">";
            else
                templateCell += "<a class=\"PagerHyperlinkStyle\" href=\"{0}\">";
            if (ImageNextClause != "")
                templateCell += "<img src=\"" + ImageNextClause + "\" border=\"0\" />";
            else
                templateCell += NextClause;
            templateCell += "</a></td>";
            return String.Format(templateCell, Page.ClientScript.GetPostBackClientHyperlink(this, next));
        }

        private string RenderCurrent()
        {
            return "<td class=\"PagerCurrentPageCell\"><span class=\"PagerHyperlinkStyle\" " + CurrentIndex + " ><strong> " + CurrentIndex.ToString() + " </strong></span></td>";
        }

        private string RenderOther(int index)
        {
            string templateCell = "";

            if (GetPageTip(index) != "")
                templateCell += "<td class=\"PagerOtherPageCells\"><a class=\"PagerHyperlinkStyle\" href=\"{0}\" title=\"" + GetPageTip(index) + "\"> " + index.ToString() + " </a></td>";
            else
                templateCell += "<td class=\"PagerOtherPageCells\"><a class=\"PagerHyperlinkStyle\" href=\"{0}\"> " + index.ToString() + " </a></td>";
            return String.Format(templateCell, Page.ClientScript.GetPostBackClientHyperlink(this, index.ToString()));
        }

        private string RenderDot()
        {
            string templateCell = "<td class=\"PagerDotPageCells\">...</td>";
            return templateCell;
        }
        #endregion

        #region // Smart ShortCut Stuff

        #endregion

        #region // Override Control's Render operation
        protected override void Render(HtmlTextWriter writer)
        {
            if (PageCount == 0) return;
            if (Page != null) Page.VerifyRenderingInServerForm(this);
            writer.AddAttribute(HtmlTextWriterAttribute.Cellpadding, "3");
            writer.AddAttribute(HtmlTextWriterAttribute.Cellspacing, "1");
            writer.AddAttribute(HtmlTextWriterAttribute.Border, "0");
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "PagerContainerTable");
            if (RTL) writer.AddAttribute(HtmlTextWriterAttribute.Dir, "rtl");
            //add reference css layout for control
            if (CSSLayout != "") writer.Write("<link href='" + CSSLayout + "' type='text/css' rel='stylesheet' />");
            writer.RenderBeginTag(HtmlTextWriterTag.Table);
            writer.RenderBeginTag(HtmlTextWriterTag.Tr);
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "PagerInfoCell");
            if (showSummary != "")
            {
                writer.RenderBeginTag(HtmlTextWriterTag.Td);
                writer.Write(RenderSummary());
                writer.RenderEndTag();
            }
            //add the first link
            writer.Write(RenderFirst());
            //add the back link
            writer.Write(RenderBack());



            //////////////////////////////////////
            //add the first number (1) when current far
            //if (CurrentIndex - 2 > 1)
            //    writer.Write(RenderOther(1));
            ////add the second number (2) when current far
            //if (CurrentIndex - 2 > 2)
            //    writer.Write(RenderOther(2));

            //////add three dot (...) when current far
            ////if (CurrentIndex - 2 > 3)
            ////    writer.Write(RenderDot());

            ////add current - 2 page
            //if (CurrentIndex - 2 > 0)
            //    writer.Write(RenderOther(CurrentIndex - 2));
            ////add current - 1 page
            //if (CurrentIndex - 1 > 0)
            //    writer.Write(RenderOther(CurrentIndex - 1));
            int numberlink = 10;
            if  ( numberlink >= PageCount) // truong hop numberlink dau tien
            {

                for (int i = 1; i <= PageCount; i++)
                {
                    if (i  == CurrentIndex)
                    {
                        writer.Write(RenderCurrent());
                    }
                    else
                    {
                        writer.Write(RenderOther(i ));
                    }

                }
                
            }else
                if  (CurrentIndex <= numberlink / 2 )// trang hien tai trong khu vuc < hon numberlink/2
                {

                    for (int i = 1; i <= numberlink; i++)
                    {
                        if (i  == CurrentIndex)
                        {
                            writer.Write(RenderCurrent());
                        }
                        else
                        {
                            writer.Write(RenderOther(i));
                        }

                    }

                }
            else
                if (PageCount - CurrentIndex < numberlink / 2 ) //5  thang cuoi cung
                {
                    for (int i = PageCount - numberlink+1; i <= PageCount; i++)
                    {
                        if (i  == CurrentIndex)
                        {
                            writer.Write(RenderCurrent());
                        }
                        else
                        {
                            writer.Write(RenderOther(i ));
                        }

                    }

                }
                else // XU LY KHUC GIUA
                {
                    for (int i = CurrentIndex - (numberlink / 2)+1; i < CurrentIndex; i++)
                    {

                        writer.Write(RenderOther(i ));
                    }
                    writer.Write(RenderCurrent());
                    for (int i = CurrentIndex +1; i < CurrentIndex + (numberlink / 2)+1; i++)
                    {

                        writer.Write(RenderOther(i ));
                    }
                }
            

            //4 trang
            // 1 2 3 4 5 6 7 8   9 10   11
            ////add current page
            //writer.Write(RenderCurrent());

            ////add current page + 1
            //if (CurrentIndex + 1 <= PageCount)
            //    writer.Write(RenderOther(CurrentIndex + 1));
            ////add current page + 2 
            //if (CurrentIndex + 2 <= PageCount)
            //    writer.Write(RenderOther(CurrentIndex + 2));

            //////add three dot (...) when current far
            ////if (CurrentIndex + 3 < PageCount - 1)
            ////    writer.Write(RenderDot());

            //if (PageCount - 1 > CurrentIndex + 2)
            //    writer.Write(RenderOther(PageCount - 1));

            //if (PageCount > CurrentIndex + 2)
            //    writer.Write(RenderOther(PageCount));


            ///////////////////////////////////////////////////
            //add the next link
            writer.Write(RenderNext());
            //add the last link
            writer.Write(RenderLast());

            writer.RenderEndTag();

            writer.RenderEndTag();

            base.Render(writer);
        }
        
        #endregion
    }
