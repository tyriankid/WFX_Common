namespace Hidistro.UI.Common.Controls
{
    using Hidistro.Entities;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using System.Xml;

    public class RegionSelector : WebControl
    {
        private bool _CustomerCss;
        private bool _CustomerJs;
        private static string _IDPrev = "";
        private WebControl areaDiv;
        private WebControl areaSpan;
        private WebControl cityDiv;
        private int? cityId;
        private WebControl citySpan;
        private int? countyId;
        private int? currentRegionId;
        private bool dataLoaded;
        private WebControl ddlCitys;
        private WebControl ddlCountys;
        private WebControl ddlProvinces;
        private bool isShowClear = true;
        private WebControl proviceDiv;
        private WebControl proviceSpan;
        private int? provinceId;


        public RegionSelector()
        {
            this.ProvinceTitle = "请选择省";
            this.CityTitle = "市：";
            this.CountyTitle = "区/县：";
            this.NullToDisplay = "-请选择-";
            this.Separator = "，";
            this.IsShift = true;
        }

        protected override void CreateChildControls()
        {
            this.Controls.Clear();
            if (!this.dataLoaded)
            {
                if (!string.IsNullOrEmpty(this.Context.Request.Form[IDPrev + "regionSelectorValue"]))
                {
                    this.currentRegionId = new int?(int.Parse(this.Context.Request.Form[IDPrev + "regionSelectorValue"]));
                }
                this.dataLoaded = true;
            }
            if (this.currentRegionId.HasValue)
            {
                XmlNode region = RegionHelper.GetRegion(this.currentRegionId.Value);
                if (region != null)
                {
                    if (region.Name == "county")
                    {
                        this.countyId = new int?(this.currentRegionId.Value);
                        this.cityId = new int?(int.Parse(region.ParentNode.Attributes["id"].Value));
                        this.provinceId = new int?(int.Parse(region.ParentNode.ParentNode.Attributes["id"].Value));
                    }
                    else if (region.Name == "city")
                    {
                        this.cityId = new int?(this.currentRegionId.Value);
                        this.provinceId = new int?(int.Parse(region.ParentNode.Attributes["id"].Value));
                    }
                    else if (region.Name == "province")
                    {
                        this.provinceId = new int?(this.currentRegionId.Value);
                    }
                }
            }
            this.Controls.Add(CreateTag("<div class=\"address_wap\"><div class=\"dp_border\"></div><div class=\"dp_address\">"));
            this.ddlProvinces = this.CreateHypLink("province_top", "provincename", this.provinceId, this.ProvinceTitle, this.ProvinceWidth, out this.proviceSpan);
            this.FillHypLink(this.proviceSpan, "province", RegionHelper.GetAllProvinces(), this.provinceId, out this.proviceDiv);
            this.ddlCitys = this.CreateHypLink("city_top", "cityname", this.cityId, this.CityTitle, this.CityWidth, out this.citySpan);
            Dictionary<int, string> regions = new Dictionary<int, string>();
            if (this.provinceId.HasValue)
            {
                regions = RegionHelper.GetCitys(this.provinceId.Value);
            }
            this.FillHypLink(this.citySpan, "city", regions, this.cityId, out this.cityDiv);
            this.ddlCountys = this.CreateHypLink("area_top", "areaname", this.cityId, this.CountyTitle, this.CountyWidth, out this.areaSpan);
            Dictionary<int, string> countys = new Dictionary<int, string>();
            if (this.cityId.HasValue)
            {
                countys = RegionHelper.GetCountys(this.cityId.Value);
            }
            this.FillHypLink(this.areaSpan, "area", countys, this.countyId, out this.areaDiv);
            this.Controls.Add(this.ddlProvinces);
            this.Controls.Add(this.ddlCitys);
            this.Controls.Add(this.ddlCountys);
            this.Controls.Add(CreateTag("</div>"));
            this.Controls.Add(this.proviceDiv);
            this.Controls.Add(this.cityDiv);
            this.Controls.Add(this.areaDiv);
            this.Controls.Add(CreateTag("</div>"));
            if (!this.CustomerCss)
            {
                Literal child = new Literal();
                StringBuilder builder = new StringBuilder();
                builder.Append("<style type=\"text/css\">");
                builder.AppendLine(".dropdown_button {" + string.Format("background: url('{0}') no-repeat;", this.Page.ClientScript.GetWebResourceUrl(base.GetType(), "Hidistro.UI.Common.Controls.images.combo_arrow.jpg")) + "}");
                builder.AppendLine(".dp_address a:hover .dropdown_button {" + string.Format("background: url('{0}') no-repeat;", this.Page.ClientScript.GetWebResourceUrl(base.GetType(), "Hidistro.UI.Common.Controls.images.combo_arrow1.jpg")) + "}");
                builder.AppendLine("</style>");
                child.Text = builder.ToString();
                this.Controls.Add(child);
                WebControl control = new WebControl(HtmlTextWriterTag.Link);
                control.Attributes.Add("rel", "stylesheet");
                control.Attributes.Add("href", this.Page.ClientScript.GetWebResourceUrl(base.GetType(), "Hidistro.UI.Common.Controls.css.region.css"));
                control.Attributes.Add("type", "text/css");
                control.Attributes.Add("media", "screen");
                control.ID = "regionStyle";
                //this.Controls.Add(control);
            }
        }

        private WebControl CreateDropDownList(string controlId)
        {
            WebControl control = new WebControl(HtmlTextWriterTag.Select);
            control.Attributes.Add("id", IDPrev + controlId);
            control.Attributes.Add("name", controlId);
            control.Attributes.Add("selectset", "regions");
            WebControl child = new WebControl(HtmlTextWriterTag.Option);
            child.Controls.Add(new LiteralControl(this.NullToDisplay));
            child.Attributes.Add("value", "");
            control.Controls.Add(child);
            return control;
        }

        private WebControl CreateHypLink(string controlId, string spanId, int? selectedId, string showname, int width, out WebControl webSpan)
        {
            WebControl control = new WebControl(HtmlTextWriterTag.A);
            control.Attributes.Add("id", IDPrev + controlId);
            control.Attributes.Add("href", "javascript:");
            control.Attributes.Add("class", "dropdown_box");
            if (width > 0)
            {
                control.Attributes.Add("style", "width:" + width + "px");
            }
            webSpan = new WebControl(HtmlTextWriterTag.Span);
            webSpan.Attributes.Add("id", IDPrev + spanId);
            webSpan.Attributes.Add("class", "dropdown_selected");
            webSpan.Controls.Add(CreateTag(showname));
            if (selectedId.HasValue)
            {
                webSpan.Attributes.Add("value", selectedId.Value.ToString());
            }
            control.Controls.Add(webSpan);
            control.Controls.Add(CreateTag("<span class=\"dropdown_button\"></span>"));
            return control;
        }

        private static WebControl CreateOption(string val, string text)
        {
            WebControl control = new WebControl(HtmlTextWriterTag.Option);
            control.Attributes.Add("value", val);
            control.Controls.Add(new LiteralControl(text.Trim()));
            return control;
        }

        private static Literal CreateTag(string tagName)
        {
            return new Literal { Text = tagName };
        }

        private static Label CreateTitleControl(string title)
        {
            Label label = new Label
            {
                Text = title
            };
            label.Attributes.Add("style", "margin-left:5px");
            return label;
        }

        private static void FillDropDownList(WebControl ddlRegions, Dictionary<int, string> regions, int? selectedId)
        {
            foreach (int num in regions.Keys)
            {
                WebControl child = CreateOption(num.ToString(CultureInfo.InvariantCulture), regions[num]);
                if (selectedId.HasValue && (num == selectedId.Value))
                {
                    child.Attributes.Add("selected", "true");
                }
                ddlRegions.Controls.Add(child);
            }
        }

        private void FillHypLink(WebControl spanContrel, string type, Dictionary<int, string> regions, int? selectedId, out WebControl divContrel)
        {
            divContrel = new WebControl(HtmlTextWriterTag.Div);
            divContrel.Attributes.Add("class", "ap_content ap_" + type + " dp_address");
            divContrel.Attributes.Add("id", IDPrev + type + "_floor");
            divContrel.Controls.Add(CreateTag("<div class=\"dp_address_list " + type + " clearfix\" id=\"" + IDPrev + type + "_info\"> <ul id=\"" + IDPrev + type + "_select\">"));
            foreach (int num in regions.Keys)
            {
                if (selectedId.HasValue && (num == selectedId.Value))
                {
                    spanContrel.Controls.RemoveAt(0);
                    spanContrel.Controls.AddAt(0, CreateTag(regions[num]));
                }
                divContrel.Controls.Add(CreateTag(string.Concat(new object[] { "<li><a href=\"javascript:;\" id=\"select_new_", IDPrev, type, "_", num, "\">", regions[num], "</a></li>" })));
            }
            if (this.IsShowClear)
            {
                divContrel.Controls.Add(CreateTag("<li><a href=\"javascript:;\" t=\"clear\" id=\"select_new_" + type + "_clear_" + IDPrev + "\">[清空]</a></li>"));
            }
            divContrel.Controls.Add(CreateTag("</ul></div>"));
        }

        public int? GetSelectedRegionId()
        {
            if (!string.IsNullOrEmpty(this.Context.Request.Form[IDPrev + "regionSelectorValue"]))
            {
                return new int?(int.Parse(this.Context.Request.Form[IDPrev + "regionSelectorValue"]));
            }
            return null;
        }

        public string GetSelectedRegionName()
        {
            if (!string.IsNullOrEmpty(this.Context.Request.Form[IDPrev + "regionSelectorName"]))
            {
                return this.Context.Request.Form[IDPrev + "regionSelectorName"];
            }
            return string.Empty;
        }

        protected override void Render(HtmlTextWriter writer)
        {
            base.Render(writer);
            writer.AddAttribute("id", IDPrev + "regionSelectorValue");
            writer.AddAttribute("name", IDPrev + "regionSelectorValue");
            writer.AddAttribute("value", this.currentRegionId.HasValue ? this.currentRegionId.Value.ToString(CultureInfo.InvariantCulture) : "");
            writer.AddAttribute("type", "hidden");
            writer.RenderBeginTag(HtmlTextWriterTag.Input);
            writer.RenderEndTag();
            writer.AddAttribute("id", IDPrev + "regionIsShift");
            writer.AddAttribute("name", IDPrev + "regionIsShift");
            writer.AddAttribute("value", this.IsShift.ToString().ToLower());
            writer.AddAttribute("type", "hidden");
            writer.RenderBeginTag(HtmlTextWriterTag.Input);
            writer.RenderEndTag();
            writer.AddAttribute("id", IDPrev + "regionSelectorName");
            writer.AddAttribute("name", IDPrev + "regionSelectorName");
            writer.AddAttribute("value", this.SelectedRegionName);
            writer.AddAttribute("type", "hidden");
            writer.RenderBeginTag(HtmlTextWriterTag.Input);
            writer.RenderEndTag();
            writer.AddAttribute("id", IDPrev + "regionSelectorNull");
            writer.AddAttribute("name", IDPrev + "regionSelectorNull");
            writer.AddAttribute("value", this.NullToDisplay);
            writer.AddAttribute("type", "hidden");
            writer.RenderBeginTag(HtmlTextWriterTag.Input);
            writer.RenderEndTag();
            if (!this.CustomerJs && !this.Page.ClientScript.IsStartupScriptRegistered(this.Page.GetType(), "RegionSelectScript"))
            {
                string script = string.Format("<script src=\"{0}\" type=\"text/javascript\"></script>", this.Page.ClientScript.GetWebResourceUrl(base.GetType(), "Hidistro.UI.Common.Controls.region.helper.js"));
                this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "RegionSelectScript", script, false);
            }
        }

        public void SetSelectedRegionId(int? selectedRegionId)
        {
            this.currentRegionId = selectedRegionId;
            this.dataLoaded = true;
        }

        public string CityTitle { get; set; }

        public int CityWidth { get; set; }

        public override ControlCollection Controls
        {
            get
            {
                base.EnsureChildControls();
                return base.Controls;
            }
        }

        public string CountyTitle { get; set; }

        public int CountyWidth { get; set; }

        public bool CustomerCss
        {
            get
            {
                return this._CustomerCss;
            }
            set
            {
                this._CustomerCss = value;
            }
        }

        public bool CustomerJs
        {
            get
            {
                return this._CustomerJs;
            }
            set
            {
                this._CustomerJs = value;
            }
        }

        public static string IDPrev
        {
            get
            {
                return _IDPrev;
            }
            set
            {
                _IDPrev = value;
            }
        }

        public bool IsShift { get; set; }

        public bool IsShowClear
        {
            get
            {
                return this.isShowClear;
            }
            set
            {
                this.isShowClear = value;
            }
        }

        public string NullToDisplay { get; set; }

        public string ProvinceTitle { get; set; }

        public int ProvinceWidth { get; set; }

        private string SelectedRegionName
        {
            get
            {
                if (this.currentRegionId.HasValue)
                {
                    return RegionHelper.GetFullRegion(this.currentRegionId.Value, " ");
                }
                return "";
            }
        }

        public string SelectedRegions
        {
            get
            {
                int? selectedRegionId = this.GetSelectedRegionId();
                if (!selectedRegionId.HasValue)
                {
                    return "";
                }
                return RegionHelper.GetFullRegion(selectedRegionId.Value, this.Separator);
            }
            set
            {
                string[] strArray = value.Split(new char[] { ',' });
                if (strArray.Length >= 3)
                {
                    int? selectedRegionId = new int?(RegionHelper.GetRegionId(strArray[2], strArray[1], strArray[0]));
                    this.SetSelectedRegionId(selectedRegionId);
                }
            }
        }

        public string Separator { get; set; }
    }
}

